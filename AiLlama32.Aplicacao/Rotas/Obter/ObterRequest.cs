using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Obter
{
    public record ObterRequest(Guid Id) : IRequest<ResultadoOperacao>;
}
