using System.Diagnostics;

namespace AiLlama32.Api.Configuracao
{
    public class RegistroRequisicaoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RegistroRequisicaoMiddleware> _logger;


        public RegistroRequisicaoMiddleware(RequestDelegate next, ILogger<RegistroRequisicaoMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cronometro = Stopwatch.StartNew();
            await _next(context);
            cronometro.Stop();

            _logger.LogInformation(
                "{Method} {Path}{QueryString} respondeu {StatusCode} em {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Response.StatusCode,
                cronometro.ElapsedMilliseconds);


        }
    }
}