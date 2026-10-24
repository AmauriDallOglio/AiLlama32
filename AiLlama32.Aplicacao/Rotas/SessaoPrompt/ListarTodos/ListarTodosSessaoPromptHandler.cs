using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.ListarTodos;

public sealed class ListarTodosSessaoPromptHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<ListarTodosSessaoPromptRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(ListarTodosSessaoPromptRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        var resultado = servico.ListarPrompts();
        return Task.FromResult<ResultadoOperacao>(new ListarTodosSessaoPromptResponse(resultado));
    }
}

