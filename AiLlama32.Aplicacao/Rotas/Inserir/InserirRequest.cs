using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Inserir
{
    public record InserirRequest(SessaoOllamaCriarDto Dados) : IRequest<ResultadoOperacao>;

}
