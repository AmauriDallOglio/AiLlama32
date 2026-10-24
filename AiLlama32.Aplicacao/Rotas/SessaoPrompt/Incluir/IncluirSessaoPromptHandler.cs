using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Incluir;

public sealed class IncluirSessaoPromptHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<IncluirSessaoPromptRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(IncluirSessaoPromptRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        if (request.Dados is null)
            return Task.FromResult<ResultadoOperacao>(ResultadoOperacao.GerarErro("Informe os dados da requisicao.", 400));
        var resultado = servico.GravarPrompt(request.Dados);
        return Task.FromResult<ResultadoOperacao>(new IncluirSessaoPromptResponse(resultado));
    }
}

