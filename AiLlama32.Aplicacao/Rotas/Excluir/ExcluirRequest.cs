using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Excluir
{
    public record ExcluirRequest(Guid Id) : IRequest<ResultadoOperacao>;
}
