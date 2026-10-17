using AiLlama32.Aplicacao.Dto;

namespace AiLlama32.Aplicacao.Rotas.Pesquisar
{
    public sealed class PesquisarResponse
    {
        public IReadOnlyCollection<SessaoOllamaDto>? Dados { get; init; }
    }
}
