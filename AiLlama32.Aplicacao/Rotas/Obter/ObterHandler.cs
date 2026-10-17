using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Obter
{
    public class ObterHandler(SessaoOllamaServico servico) : IContratoBaseHandler<ObterRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(ObterRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.Obter(request.Id))));
        }

        private static ResultadoOperacao Converter(SessaoOllamaDto? dado)
        {
            return dado is null
                ? ResultadoOperacao.GerarErro("Sessão não encontrada.", 404)
                : ResultadoOperacao.GerarSucesso(new ObterResponse { Dados = dado }, "Sessão obtida com sucesso.");
        }
    }
}
