using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Excluir;

public sealed class ExcluirSessaoDocumentoResponse : ResultadoOperacao
{
    public ExcluirSessaoDocumentoResponse(ResultadoOperacao resultado)
    {
        Sucesso = resultado.Sucesso;
        Mensagem = resultado.Mensagem;
        Resultado = resultado.Resultado;
        StatusCodigo = resultado.StatusCodigo;
        Erros = resultado.Erros;
    }
}

