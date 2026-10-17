using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Inserir
{
    public class InserirHandler(SessaoOllamaServico servico) : IContratoBaseHandler<InserirRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(InserirRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.Inserir(request.Dados))));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<object> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new InserirResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 400);
        }
    }
}
