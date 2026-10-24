using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AiLlama32.Aplicacao.Servico;

// Servico scoped: utiliza o Cache singleton e cuida somente da persistencia e dos retornos.
public sealed class SessoesConhecimentoServico
{
    private readonly Cache cache;
    private readonly string caminho;
    private readonly JsonSerializerOptions json = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public SessoesConhecimentoServico(IWebHostEnvironment ambiente, IOptions<AppSettingsDto> opcoes, Cache cache)
    {
        this.cache = cache;
        var arquivo = opcoes.Value.ConfiguracaoOllama.CaminhoArquivoConhecimento;
        caminho = Path.GetFullPath(Path.IsPathRooted(arquivo) ? arquivo : Path.Combine(ambiente.ContentRootPath, arquivo));
    }

    public (int Prompts, int Documentos) InicializarCache() => cache.Inicializar(Carregar);

    private DadosCache Carregar()
    {
        string conteudo;
        try { conteudo = File.ReadAllText(caminho); }
        catch (FileNotFoundException) { return new DadosCache(); }
        catch (DirectoryNotFoundException) { return new DadosCache(); }
        var dados = JsonSerializer.Deserialize<DadosCache>(conteudo, json);
        if (dados is null || dados.Prompts is null || dados.Documentos is null
            || dados.Prompts.Any(x => x is null) || dados.Documentos.Any(x => x is null))
            throw new JsonException("Cadastro de sessoes invalido.");
        return dados;
    }

    private void Salvar(DadosCache dados)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(caminho)!);
        var temporario = caminho + ".tmp";
        try
        {
            File.WriteAllText(temporario, JsonSerializer.Serialize(dados, json));
            File.Move(temporario, caminho, overwrite: true);
        }
        finally { if (File.Exists(temporario)) File.Delete(temporario); }
    }

    public ResultadoOperacao ListarPrompts() => cache.Consultar(Carregar, c =>
    {
        var itens = c.ObterTodosPrompts();
        return ResultadoOperacao.GerarSucesso(new { Sessoes = itens, Total = itens.Count }, itens.Count == 0 ? "Nenhuma sessao de prompt cadastrada." : "Sessoes de prompt listadas.");
    });

    public ResultadoOperacao ListarDocumentos() => cache.Consultar(Carregar, c =>
    {
        var itens = c.ObterTodosDocumentos();
        return ResultadoOperacao.GerarSucesso(new { Sessoes = itens, Total = itens.Count }, itens.Count == 0 ? "Nenhuma sessao de documento cadastrada." : "Sessoes de documento listadas.");
    });

    public (SessaoPromptDto? Prompt, SessaoDocumentoDto? Documento) Selecionar(string prompt, string documento)
        => cache.Consultar(Carregar, c => (c.ObterPromptPorNome(prompt), c.ObterDocumentoPorNome(documento)));

    public ResultadoOperacao ObterPrompt(string nome) => cache.Consultar(Carregar, c =>
    {
        var item = c.ObterPromptPorNome(nome);
        return item is null ? ResultadoOperacao.GerarErro("Prompt nao encontrado.", 404) : ResultadoOperacao.GerarSucesso(item);
    });

    public ResultadoOperacao ObterDocumento(string nome) => cache.Consultar(Carregar, c =>
    {
        var item = c.ObterDocumentoPorNome(nome);
        return item is null ? ResultadoOperacao.GerarErro("Documento nao encontrado.", 404) : ResultadoOperacao.GerarSucesso(item);
    });

    public ResultadoOperacao GravarPrompt(SessaoPromptDto item, string? nomeAtual = null)
        => cache.Atualizar(Carregar, c => nomeAtual is null ? c.InserirPrompt(item) : c.AlterarPrompt(nomeAtual, item), Salvar);

    public ResultadoOperacao GravarDocumento(SessaoDocumentoDto item, string? nomeAtual = null)
        => cache.Atualizar(Carregar, c => nomeAtual is null ? c.InserirDocumento(item) : c.AlterarDocumento(nomeAtual, item), Salvar);

    public ResultadoOperacao Excluir(string nome, bool prompt)
        => cache.Atualizar(Carregar, c => prompt ? c.ExcluirPrompt(nome) : c.ExcluirDocumento(nome), Salvar);
}
