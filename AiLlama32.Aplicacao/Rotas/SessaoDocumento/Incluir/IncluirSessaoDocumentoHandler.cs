using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Incluir;

public sealed class IncluirSessaoDocumentoHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<IncluirSessaoDocumentoRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(IncluirSessaoDocumentoRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        if (request.Dados is null)
            return Task.FromResult<ResultadoOperacao>(ResultadoOperacao.GerarErro("Informe os dados da requisicao.", 400));
        var resultado = servico.GravarDocumento(request.Dados);
        return Task.FromResult<ResultadoOperacao>(new IncluirSessaoDocumentoResponse(resultado));
    }
}

