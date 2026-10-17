using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.DeletarHistorico
{
    public class DeletarHistoricoHandler(SessaoOllamaServico servico) : IContratoBaseHandler<DeletarHistoricoRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(DeletarHistoricoRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.LimparHistorico(request.Id))));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<object> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new DeletarHistoricoResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 404);
        }
    }

}
