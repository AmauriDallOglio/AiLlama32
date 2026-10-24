using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.AlterarAssuntoAmauri;

public sealed class AlterarAssuntoAmauriHandler(SessaoOllamaServico servico)
    : IContratoBaseHandler<AlterarAssuntoAmauriRequest, ResultadoOperacao>
{
    public async Task<ResultadoOperacao> Executar(AlterarAssuntoAmauriRequest request, CancellationToken cancelamento = default)
    {
        if (string.IsNullOrWhiteSpace(request.Assunto))
            return ResultadoOperacao.GerarErro("O novo assunto e obrigatorio.", 400);

        var resultado = await servico.AlterarAssuntoAmauri(request.Assunto.Trim(), cancelamento);
        return resultado.Sucesso
            ? ResultadoOperacao.GerarSucesso(resultado.Dados, resultado.Mensagem)
            : ResultadoOperacao.GerarErro(resultado.Mensagem, 502, resultado.Dados);
    }
}
