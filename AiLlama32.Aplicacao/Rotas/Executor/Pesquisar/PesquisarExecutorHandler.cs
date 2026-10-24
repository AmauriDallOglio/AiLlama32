using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Executor.Pesquisar;

public sealed class PesquisarExecutorHandler(ExecutorServico servico)
    : IContratoBaseHandler<PesquisarExecutorRequest, ResultadoOperacao>
{
    public async Task<ResultadoOperacao> Executar(PesquisarExecutorRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        if (request.Dados is null)
            return ResultadoOperacao.GerarErro("Informe os dados da requisicao.", 400);
        var resultado = await servico.Executar(request.Dados, cancelamento);
        return new PesquisarExecutorResponse(resultado);
    }
}

