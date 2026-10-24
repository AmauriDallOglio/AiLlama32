using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.ListarTodos;

public sealed class ListarTodosSessaoDocumentoHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<ListarTodosSessaoDocumentoRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(ListarTodosSessaoDocumentoRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        var resultado = servico.ListarDocumentos();
        return Task.FromResult<ResultadoOperacao>(new ListarTodosSessaoDocumentoResponse(resultado));
    }
}

