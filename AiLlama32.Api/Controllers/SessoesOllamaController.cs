using AiLlama32.Aplicacao.Rotas.Alterar;
using AiLlama32.Aplicacao.Rotas.AssuntoGeral;
using AiLlama32.Aplicacao.Rotas.DeletarHistorico;
using AiLlama32.Aplicacao.Rotas.Excluir;
using AiLlama32.Aplicacao.Rotas.Inserir;
using AiLlama32.Aplicacao.Rotas.LimparHistorico;
using AiLlama32.Aplicacao.Rotas.Listar;
using AiLlama32.Aplicacao.Rotas.Obter;
using AiLlama32.Aplicacao.Rotas.ObterConfiguracaoOllama;
using AiLlama32.Aplicacao.Rotas.ObterHistorico;
using AiLlama32.Aplicacao.Rotas.ObterStatusIntegracao;
using AiLlama32.Aplicacao.Rotas.PerguntarSessao;
using AiLlama32.Aplicacao.Rotas.Pesquisar;
using Microsoft.AspNetCore.Mvc;

namespace AiLlama32.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessoesOllamaController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Listar([FromServices] ListarHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(new ListarRequest(), cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Obter(ObterRequest request, [FromServices] ObterHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpPost("inserir")]
        public async Task<IActionResult> Inserir([FromBody] InserirRequest request, [FromServices] InserirHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpPut("alterar/{id:guid}")]
        public async Task<IActionResult> Alterar(AlterarRequest request, [FromServices] AlterarHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpDelete("excluir/{id:guid}")]
        public async Task<IActionResult> Excluir(ExcluirRequest request, [FromServices] ExcluirHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpGet("pesquisar")]
        public async Task<IActionResult> Pesquisar([FromQuery] PesquisarRequest request, [FromServices] PesquisarHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpPost("{id:guid}/perguntar")]
        public async Task<IActionResult> Perguntar(PerguntarSessaoRequest request, [FromServices] PerguntarSessaoHandler perguntarSessaoHandler, CancellationToken cancelamento)
        {
            var resultado = await perguntarSessaoHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpGet("{id:guid}/historico")]
        public async Task<IActionResult> ObterHistorico(ObterHistoricoRequest request, [FromServices] ObterHistoricoHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpDelete("{id:guid}/historico")]
        public async Task<IActionResult> LimparHistorico(LimparHistoricoRequest request, [FromServices] LimparHistoricoHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpDelete("{id:guid}/deletar-historico")]
        public async Task<IActionResult> DeletarHistorico(DeletarHistoricoRequest request, [FromServices] DeletarHistoricoHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpPost("AssuntoGeral")]
        public async Task<IActionResult> AssuntoGeral([FromQuery] AssuntoGeralRequest request, [FromServices] AssuntoGeralHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(request, cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpGet("status-integracao")]
        public async Task<IActionResult> ObterStatusIntegracao([FromServices] ObterStatusIntegracaoHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(new ObterStatusIntegracaoRequest(), cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }

        [HttpGet("configuracao-ollama")]
        public async Task<IActionResult> ObterConfiguracaoOllama([FromServices] ObterConfiguracaoOllamaHandler rotaHandler, CancellationToken cancelamento)
        {
            var resultado = await rotaHandler.Executar(new ObterConfiguracaoOllamaRequest(), cancelamento);
            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status200OK, resultado);
        }
    }
}