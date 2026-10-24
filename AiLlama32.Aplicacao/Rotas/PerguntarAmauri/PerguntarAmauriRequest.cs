using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.PerguntarAmauri;

public sealed class PerguntarAmauriRequest : IRequest<ResultadoOperacao>
{
    public string Pergunta { get; set; } = string.Empty;
}
