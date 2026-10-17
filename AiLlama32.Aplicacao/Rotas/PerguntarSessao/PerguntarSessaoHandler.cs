using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace AiLlama32.Aplicacao.Rotas.PerguntarSessao
{
    public class PerguntarSessaoHandler(SessaoOllamaServico servico) : IContratoBaseHandler<PerguntarSessaoRequest, ResultadoOperacao>
    {
        public async Task<ResultadoOperacao> Executar(PerguntarSessaoRequest requisicao, CancellationToken cancelamento = default)
        {
            var resultado = await servico.Perguntar(requisicao.SessaoId, requisicao);
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new PerguntarSessaoResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(
                    string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem,
                    StatusCodes.Status400BadRequest);
        }
    }

}
