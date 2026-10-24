using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Excluir;

public sealed class ExcluirSessaoPromptHandler(SessoesConhecimentoServico servico)
    : IContratoBaseHandler<ExcluirSessaoPromptRequest, ResultadoOperacao>
{
    public Task<ResultadoOperacao> Executar(ExcluirSessaoPromptRequest request, CancellationToken cancelamento = default)
    {
        cancelamento.ThrowIfCancellationRequested();
        var resultado = servico.Excluir(request.Nome, prompt: true);
        return Task.FromResult<ResultadoOperacao>(new ExcluirSessaoPromptResponse(resultado));
    }
}

