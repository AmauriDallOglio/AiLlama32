using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Excluir;

public sealed class ExcluirSessaoPromptResponse : ResultadoOperacao
{
    public ExcluirSessaoPromptResponse(ResultadoOperacao resultado)
    {
        Sucesso = resultado.Sucesso;
        Mensagem = resultado.Mensagem;
        Resultado = resultado.Resultado;
        StatusCodigo = resultado.StatusCodigo;
        Erros = resultado.Erros;
    }
}

