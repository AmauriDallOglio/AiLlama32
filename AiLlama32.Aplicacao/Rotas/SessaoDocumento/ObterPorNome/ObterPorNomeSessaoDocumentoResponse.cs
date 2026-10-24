using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.ObterPorNome;

public sealed class ObterPorNomeSessaoDocumentoResponse : ResultadoOperacao
{
    public ObterPorNomeSessaoDocumentoResponse(ResultadoOperacao resultado)
    {
        Sucesso = resultado.Sucesso;
        Mensagem = resultado.Mensagem;
        Resultado = resultado.Resultado;
        StatusCodigo = resultado.StatusCodigo;
        Erros = resultado.Erros;
    }
}

