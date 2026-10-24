using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.ListarTodos;

public sealed class ListarTodosSessaoPromptResponse : ResultadoOperacao
{
    public ListarTodosSessaoPromptResponse(ResultadoOperacao resultado)
    {
        Sucesso = resultado.Sucesso;
        Mensagem = resultado.Mensagem;
        Resultado = resultado.Resultado;
        StatusCodigo = resultado.StatusCodigo;
        Erros = resultado.Erros;
    }
}

