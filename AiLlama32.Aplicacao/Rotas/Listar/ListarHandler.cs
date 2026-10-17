using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Listar
{
    public class ListarHandler(SessaoOllamaServico servico) : IContratoBaseHandler<ListarRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(ListarRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((ResultadoOperacao.GerarSucesso(new ListarResponse { Dados = servico.Listar() })));
        }
    }

}
