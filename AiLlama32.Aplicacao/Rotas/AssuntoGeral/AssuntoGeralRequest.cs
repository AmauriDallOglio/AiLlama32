using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.AssuntoGeral
{
    public record AssuntoGeralRequest(PerguntarSessaoDto Dados) : IRequest<ResultadoOperacao>;
 
}
