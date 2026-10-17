using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Excluir
{
    public class ExcluirHandler(SessaoOllamaServico servico) : IContratoBaseHandler<ExcluirRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(ExcluirRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.Excluir(request.Id))));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<bool> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new ExcluirResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 400);
        }
    }

}
