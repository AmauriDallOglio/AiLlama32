using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Excluir;

public sealed class ExcluirSessaoDocumentoHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<ExcluirSessaoDocumentoRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(ExcluirSessaoDocumentoRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        var resultado = servico.Excluir(request.Nome, prompt: false);
        return Task.FromResult<ResultadoOperacao>(new ExcluirSessaoDocumentoResponse(resultado));
    }
}

