using AiLlama32.Aplicacao.Servico;
using System.Text.Json;

namespace AiLlama32.Api.Configuracao;

public sealed class InicializadorCacheServico(SessoesConhecimentoServico servico, ILogger<InicializadorCacheServico> logger)
{
    public bool Inicializar()
    {
        try
        {
            var totais = servico.InicializarCache();
            logger.LogInformation("Cache inicializado: {Prompts} prompts e {Documentos} documentos carregados.", totais.Prompts, totais.Documentos);
            return true;
        }
        catch (Exception excecao) when (excecao is IOException or UnauthorizedAccessException or JsonException)
        {
            const string mensagem = "Nao foi possivel inicializar o Cache de prompts e documentos. Verifique o arquivo ConfiguracaoOllama:CaminhoArquivoConhecimento, o JSON e as permissoes. A API nao sera iniciada.";
            logger.LogCritical(excecao, mensagem);
            return false;
        }
    }
}

public static class InicializacaoCacheConfiguracao
{
    public static bool CarregarCacheNaInicializacao(this WebApplication app)
    {
        using var escopo = app.Services.CreateScope();
        return escopo.ServiceProvider.GetRequiredService<InicializadorCacheServico>().Inicializar();
    }
}
