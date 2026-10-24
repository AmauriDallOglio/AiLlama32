using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.AlterarAssuntoAmauri;

public sealed class AlterarAssuntoAmauriRequest : IRequest<ResultadoOperacao>
{
    public string Assunto { get; set; } = string.Empty;
}
