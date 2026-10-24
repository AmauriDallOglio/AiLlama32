using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Rotas.Executor.Pesquisar;
using AiLlama32.Api.Configuracao;
using Microsoft.AspNetCore.Mvc;

namespace AiLlama32.Api.Controllers;

[ApiController]
[TratarFalhasSessoes]
[ValidarRequisicaoSessoes("Informe um JSON valido com nomePrompt, nomeDocumento e pergunta.")]
[Route("api/Executor")]
public sealed class ExecutorController : ControllerBase
{
    [HttpPost("Pesquisar")]
    public async Task<IActionResult> Pesquisar([FromBody] ExecutarSessaoDto dados,
        [FromServices] PesquisarExecutorHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new PesquisarExecutorRequest(dados), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }
}
