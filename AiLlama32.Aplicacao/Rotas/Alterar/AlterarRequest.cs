using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Alterar
{
    public record AlterarRequest(Guid Id) : IRequest<ResultadoOperacao>;

}
