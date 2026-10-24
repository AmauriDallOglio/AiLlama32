using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Alterar;

public sealed class AlterarSessaoDocumentoHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<AlterarSessaoDocumentoRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(AlterarSessaoDocumentoRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        if (request.Dados is null)
            return Task.FromResult<ResultadoOperacao>(ResultadoOperacao.GerarErro("Informe os dados da requisicao.", 400));
        var resultado = servico.GravarDocumento(request.Dados, request.Nome);
        return Task.FromResult<ResultadoOperacao>(new AlterarSessaoDocumentoResponse(resultado));
    }
}

