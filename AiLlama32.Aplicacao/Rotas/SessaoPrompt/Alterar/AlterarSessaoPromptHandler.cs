using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Alterar;

public sealed class AlterarSessaoPromptHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<AlterarSessaoPromptRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(AlterarSessaoPromptRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        if (request.Dados is null)
            return Task.FromResult<ResultadoOperacao>(ResultadoOperacao.GerarErro("Informe os dados da requisicao.", 400));
        var resultado = servico.GravarPrompt(request.Dados, request.Nome);
        return Task.FromResult<ResultadoOperacao>(new AlterarSessaoPromptResponse(resultado));
    }
}

