using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.PerguntarAmauri;

public sealed class PerguntarAmauriHandler(SessaoOllamaServico servico)
    : IContratoBaseHandler<PerguntarAmauriRequest, ResultadoOperacao>
{
    public async Task<ResultadoOperacao> Executar(PerguntarAmauriRequest request, CancellationToken cancelamento = default)
    {
        if (string.IsNullOrWhiteSpace(request.Pergunta))
            return ResultadoOperacao.GerarErro("A pergunta e obrigatoria.", 400);

        var resultado = await servico.PerguntarAmauri(request.Pergunta, cancelamento);
        return resultado.Sucesso
            ? ResultadoOperacao.GerarSucesso(resultado.Dados, resultado.Mensagem)
            : ResultadoOperacao.GerarErro(resultado.Mensagem, 502);
    }
}
