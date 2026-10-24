using AiLlama32.Aplicacao.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace AiLlama32.Api.Configuracao;

public sealed class TratarFalhasSessoesAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not (IOException or UnauthorizedAccessException or JsonException)) return;
        var mensagem = context.Exception is JsonException
            ? "O arquivo de sessoes esta invalido. Corrija o cadastro antes de tentar novamente."
            : "O armazenamento de sessoes esta indisponivel. Verifique o arquivo configurado e as permissoes de acesso.";
        context.Result = new ObjectResult(ResultadoOperacao.GerarErro(mensagem, 503,
            new { Codigo = "ARMAZENAMENTO_INDISPONIVEL", Resposta = mensagem, EncontrouNoContexto = false, Fontes = Array.Empty<object>() }))
            { StatusCode = 503 };
        context.ExceptionHandled = true;
    }
}
