using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.ObterHistorico
{
    public record ObterHistoricoRequest(Guid Id) : IRequest<ResultadoOperacao>;
}
