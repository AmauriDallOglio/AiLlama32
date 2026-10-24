using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.ObterPorNome;

public sealed class ObterPorNomeSessaoDocumentoHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<ObterPorNomeSessaoDocumentoRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(ObterPorNomeSessaoDocumentoRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        var resultado = servico.ObterDocumento(request.Nome);
        return Task.FromResult<ResultadoOperacao>(new ObterPorNomeSessaoDocumentoResponse(resultado));
    }
}

