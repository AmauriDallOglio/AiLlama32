using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Executor.Pesquisar;

public sealed class PesquisarExecutorResponse : ResultadoOperacao
{
    public PesquisarExecutorResponse(ResultadoOperacao resultado)
    {
        Sucesso = resultado.Sucesso;
        Mensagem = resultado.Mensagem;
        Resultado = resultado.Resultado;
        StatusCodigo = resultado.StatusCodigo;
        Erros = resultado.Erros;
    }
}

