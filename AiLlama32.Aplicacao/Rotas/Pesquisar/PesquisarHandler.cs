using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Pesquisar
{
    public class PesquisarHandler(SessaoOllamaServico servico) : IContratoBaseHandler<PesquisarRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(PesquisarRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((ResultadoOperacao.GerarSucesso(new PesquisarResponse { Dados = servico.Pesquisar(request.Dados) })));
        }
    }
}
