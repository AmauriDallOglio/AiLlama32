using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Rotas.AssuntoGeral
{
    public class AssuntoGeralHandler(SessaoOllamaServico servico) : IContratoBaseHandler<AssuntoGeralRequest, ResultadoOperacao>
    {
        public async Task<ResultadoOperacao> Executar(AssuntoGeralRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return (Converter(await servico.AssuntoGeral(request.Dados)));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<object> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new AssuntoGeralResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 400);
        }
    }

}
