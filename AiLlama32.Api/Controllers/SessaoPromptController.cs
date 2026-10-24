using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.ListarTodos;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.ObterPorNome;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.Incluir;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.Alterar;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.Excluir;
using AiLlama32.Api.Configuracao;
using Microsoft.AspNetCore.Mvc;

namespace AiLlama32.Api.Controllers;

[ApiController]
[TratarFalhasSessoes]
[ValidarRequisicaoSessoes("Informe um JSON valido com nome, instrucaoSistema e modelo.")]
[Route("api/sessao_prompt")]
public sealed class SessaoPromptController : ControllerBase
{
    [HttpGet("ListarTodos")]
    public async Task<IActionResult> ListarTodos([FromServices] ListarTodosSessaoPromptHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new ListarTodosSessaoPromptRequest(), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }

    [HttpGet("ObterPorNome/{nome}")]
    public async Task<IActionResult> ObterPorNome([FromRoute] string nome, [FromServices] ObterPorNomeSessaoPromptHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new ObterPorNomeSessaoPromptRequest(nome), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }

    [HttpPost("Incluir")]
    public async Task<IActionResult> Incluir([FromBody] SessaoPromptDto dados, [FromServices] IncluirSessaoPromptHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new IncluirSessaoPromptRequest(dados), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status201Created, resultado);
    }

    [HttpPut("Alterar/{nome}")]
    public async Task<IActionResult> Alterar([FromRoute] string nome, [FromBody] SessaoPromptDto dados, [FromServices] AlterarSessaoPromptHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new AlterarSessaoPromptRequest(nome, dados), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }

    [HttpDelete("Excluir/{nome}")]
    public async Task<IActionResult> Excluir([FromRoute] string nome, [FromServices] ExcluirSessaoPromptHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new ExcluirSessaoPromptRequest(nome), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }
}
