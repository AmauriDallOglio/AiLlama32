using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.PerguntarSessao
{
    public class PerguntarSessaoRequest : PerguntarSessaoDto, IRequest<ResultadoOperacao>
    {
        public Guid SessaoId { get; set; }
    }
}
