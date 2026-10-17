using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;
using Microsoft.Extensions.Options;

namespace AiLlama32.Aplicacao.Rotas.ObterConfiguracaoOllama
{
    public class ObterConfiguracaoOllamaHandler(IOptions<ConfiguracaoOllamaDto> opcoes) : IContratoBaseHandler<ObterConfiguracaoOllamaRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(ObterConfiguracaoOllamaRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((ResultadoOperacao.GerarSucesso(new ObterConfiguracaoOllamaResponse { Dados = opcoes.Value })));
        }
    }
}
