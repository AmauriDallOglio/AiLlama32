using AiLlama32.Aplicacao.Dto;

namespace AiLlama32.Aplicacao.Rotas.Listar
{
    public sealed class ListarResponse
    {
        public IReadOnlyCollection<SessaoOllamaDto>? Dados { get; init; }
    }
}
