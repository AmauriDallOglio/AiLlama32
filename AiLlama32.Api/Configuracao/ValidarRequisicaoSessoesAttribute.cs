using AiLlama32.Aplicacao.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AiLlama32.Api.Configuracao;

public sealed class ValidarRequisicaoSessoesAttribute(string mensagem) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        context.Result = new BadRequestObjectResult(ResultadoOperacao.GerarErro(mensagem, 400,
            new { Codigo = "REQUISICAO_INVALIDA", Resposta = mensagem, EncontrouNoContexto = false, Fontes = Array.Empty<object>() }));
    }
}

