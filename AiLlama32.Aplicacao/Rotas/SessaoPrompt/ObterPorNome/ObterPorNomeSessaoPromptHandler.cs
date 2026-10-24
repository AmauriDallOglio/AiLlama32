using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.ObterPorNome;

public sealed class ObterPorNomeSessaoPromptHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<ObterPorNomeSessaoPromptRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(ObterPorNomeSessaoPromptRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        var resultado = servico.ObterPrompt(request.Nome);
        return Task.FromResult<ResultadoOperacao>(new ObterPorNomeSessaoPromptResponse(resultado));
    }
}

