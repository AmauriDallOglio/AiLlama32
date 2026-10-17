using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.ObterHistorico
{
    public class ObterHistoricoHandler(SessaoOllamaServico servico) : IContratoBaseHandler<ObterHistoricoRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(ObterHistoricoRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.ObterHistorico(request.Id))));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<object> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new ObterHistoricoResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 404);
        }
    }
}
