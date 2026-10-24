using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

static class CacheVerificacoes
{
    public static void Executar()
    {
        var arquivo = Path.Combine(Path.GetTempPath(), $"AiLlama32-cache-{Guid.NewGuid():N}.json");
        var opcoes = Options.Create(new AppSettingsDto { ConfiguracaoOllama = new() { CaminhoArquivoConhecimento = arquivo } });
        SessoesConhecimentoServico NovoServico(Cache? cache = null) => new(new Ambiente(), opcoes, cache ?? new Cache());
        void Conferir(bool condicao, string mensagem)
        {
            if (!condicao) throw new Exception(mensagem);
            Console.WriteLine("PASS Cache: " + mensagem);
        }
        try
        {
            var compartilhado = new Cache();
            var servico = NovoServico(compartilhado);
            var outroServico = NovoServico(compartilhado);
            var vazio = servico.InicializarCache();
            Conferir(vazio.Prompts == 0 && vazio.Documentos == 0, "Inicializacao sem arquivo carrega um cache vazio.");
            servico.GravarPrompt(new("prompt", "Use o documento."));
            servico.GravarDocumento(new("documento", "Conteudo original."));
            var inicializado = NovoServico();
            var totais = inicializado.InicializarCache();
            Conferir(totais.Prompts == 1 && totais.Documentos == 1, "Inicializacao carrega prompts e documentos do JSON antes das consultas.");
            File.WriteAllText(arquivo, "invalid");
            Conferir(inicializado.InicializarCache() == totais && inicializado.ObterDocumento("documento").Sucesso, "Inicializacao repetida nao recarrega o arquivo nem perde o cache.");
            Conferir(((SessaoDocumentoDto)servico.ObterDocumento("documento").Resultado!).Conteudo == "Conteudo original.", "Consulta usa a memoria sem reler o arquivo.");
            Conferir(((SessaoDocumentoDto)outroServico.ObterDocumento("documento").Resultado!).Conteudo == "Conteudo original.", "Servicos diferentes consultam o mesmo Cache singleton.");

            servico.GravarDocumento(new("documento", "Conteudo atualizado."), "documento");
            Conferir(((SessaoDocumentoDto)NovoServico().ObterDocumento("documento").Resultado!).Conteudo == "Conteudo atualizado.", "Alteracao persiste e pode ser carregada por nova instancia.");
            var persistido = File.ReadAllText(arquivo);
            using (var bloqueio = new FileStream(arquivo + ".tmp", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                void DeveFalhar(Action acao)
                {
                    try { acao(); throw new Exception("Gravacao deveria falhar."); }
                    catch (IOException) { }
                }
                DeveFalhar(() => servico.GravarPrompt(new("novo", "Instrucao nova.")));
                DeveFalhar(() => servico.GravarDocumento(new("documento", "Nao deve persistir."), "documento"));
                DeveFalhar(() => servico.Excluir("documento", prompt: false));
                Conferir(servico.ObterPrompt("novo").StatusCodigo == 404, "Falha ao incluir preserva o cache anterior.");
                Conferir(((SessaoDocumentoDto)servico.ObterDocumento("documento").Resultado!).Conteudo == "Conteudo atualizado.", "Falhas ao alterar ou excluir preservam o cache anterior.");
                Conferir(File.ReadAllText(arquivo) == persistido, "Falhas de escrita preservam o arquivo anterior.");
            }

            var lista = JsonSerializer.SerializeToElement(servico.ListarDocumentos().Resultado);
            Conferir(lista.GetProperty("Total").GetInt32() == 1, "Listagem consulta o estado atual do cache.");
            Parallel.For(0, 8, i => NovoServico(compartilhado).GravarDocumento(new("documento_" + i, "Conteudo " + i)));
            lista = JsonSerializer.SerializeToElement(servico.ListarDocumentos().Resultado);
            Conferir(lista.GetProperty("Total").GetInt32() == 9, "Gravacoes concorrentes nao perdem cadastros.");
            servico.Excluir("documento", prompt: false);
            Conferir(servico.ObterDocumento("documento").StatusCodigo == 404 && NovoServico().ObterDocumento("documento").StatusCodigo == 404, "Exclusao atualiza memoria e persistencia.");
            Conferir(outroServico.GravarPrompt(new("prompt", "Duplicado.")).StatusCodigo == 409, "Duplicidade e validada no Cache entre servicos diferentes.");
            outroServico.GravarPrompt(new("renomeado", "Nova instrucao."), "prompt");
            Conferir(servico.ObterPrompt("prompt").StatusCodigo == 404 && servico.ObterPrompt("renomeado").Sucesso, "Alteracao e renomeacao no Cache ficam visiveis a todos os servicos.");

            using var provider = new ServiceCollection()
                .AddSingleton<Cache>()
                .AddSingleton<IWebHostEnvironment>(new Ambiente())
                .AddSingleton<IOptions<AppSettingsDto>>(opcoes)
                .AddScoped<SessoesConhecimentoServico>()
                .BuildServiceProvider(validateScopes: true);
            using var escopo1 = provider.CreateScope();
            using var escopo2 = provider.CreateScope();
            Conferir(ReferenceEquals(escopo1.ServiceProvider.GetRequiredService<Cache>(), escopo2.ServiceProvider.GetRequiredService<Cache>())
                && !ReferenceEquals(escopo1.ServiceProvider.GetRequiredService<SessoesConhecimentoServico>(), escopo2.ServiceProvider.GetRequiredService<SessoesConhecimentoServico>()),
                "Injecao mantem Cache singleton e servico scoped.");
        }
        finally
        {
            File.Delete(arquivo);
            File.Delete(arquivo + ".tmp");
        }
    }
}
