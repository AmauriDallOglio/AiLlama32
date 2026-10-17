using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Rotas.LimparHistorico
{
    public class LimparHistoricoHandler(SessaoOllamaServico servico) : IContratoBaseHandler<LimparHistoricoRequest, ResultadoOperacao>
    {
        public Task<ResultadoOperacao> Executar(LimparHistoricoRequest request, CancellationToken cancelamento = default)
        {
            cancelamento.ThrowIfCancellationRequested();
            return Task.FromResult<ResultadoOperacao>((Converter(servico.LimparHistorico(request.Id))));
        }

        private static ResultadoOperacao Converter(ResultadoOllamaDto<object> resultado)
        {
            return resultado.Sucesso
                ? ResultadoOperacao.GerarSucesso(new LimparHistoricoResponse { Dados = resultado.Dados }, resultado.Mensagem)
                : ResultadoOperacao.GerarErro(string.IsNullOrWhiteSpace(resultado.Mensagem) ? string.Join("; ", resultado.Erros) : resultado.Mensagem, 404);
        }
    }
}
