using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Alterar
{
    public class AlterarHandler(SessaoOllamaServico servico) : IContratoBaseHandler<AlterarRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(AlterarRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.Alterar(request.Id, null))));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<SessaoOllamaDto> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new AlterarResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 400);
        }
    }

}
