using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.ListarTodos;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.ObterPorNome;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.Incluir;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.Alterar;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.Excluir;
using AiLlama32.Api.Configuracao;
using Microsoft.AspNetCore.Mvc;

namespace AiLlama32.Api.Controllers;

[ApiController]
[TratarFalhasSessoes]
[ValidarRequisicaoSessoes("Informe um JSON valido com nome e conteudo.")]
[Route("api/sessao_documento")]
public sealed class SessaoDocumentoController : ControllerBase
{
    [HttpGet("ListarTodos")]
    public async Task<IActionResult> ListarTodos([FromServices] ListarTodosSessaoDocumentoHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new ListarTodosSessaoDocumentoRequest(), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }

    [HttpGet("ObterPorNome/{nome}")]
    public async Task<IActionResult> ObterPorNome([FromRoute] string nome, [FromServices] ObterPorNomeSessaoDocumentoHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new ObterPorNomeSessaoDocumentoRequest(nome), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }

    [HttpPost("Incluir")]
    public async Task<IActionResult> Incluir([FromBody] SessaoDocumentoDto dados, [FromServices] IncluirSessaoDocumentoHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new IncluirSessaoDocumentoRequest(dados), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status201Created, resultado);
    }

    [HttpPut("Alterar/{nome}")]
    public async Task<IActionResult> Alterar([FromRoute] string nome, [FromBody] SessaoDocumentoDto dados, [FromServices] AlterarSessaoDocumentoHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new AlterarSessaoDocumentoRequest(nome, dados), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }

    [HttpDelete("Excluir/{nome}")]
    public async Task<IActionResult> Excluir([FromRoute] string nome, [FromServices] ExcluirSessaoDocumentoHandler handler, CancellationToken cancelamento)
    {
        var resultado = await handler.Executar(new ExcluirSessaoDocumentoRequest(nome), cancelamento);
        return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
    }
}
