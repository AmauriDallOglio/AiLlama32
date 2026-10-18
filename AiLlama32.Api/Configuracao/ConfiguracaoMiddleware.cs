namespace AiLlama32.Api.Configuracao
{
    public static class ConfiguracaoMiddleware
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            //Faz a validação da chave de API via X-Api-Key no header
            //Retorna 401 Unauthorized com mensagem em JSON se a chave estiver ausente ou inválida.
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }

        public static IApplicationBuilder UseRegistroRequisicao(this IApplicationBuilder builder)
        {
            //Mede o tempo de execução da requisição com Stopwatch.
            //Loga método, path, querystring, status e tempo.
            return builder.UseMiddleware<RegistroRequisicaoMiddleware>();
        }

        public static IApplicationBuilder ConfigurarMiddlewaresApi(this IApplicationBuilder app)
        {
            app.UseRegistroRequisicao();
            app.UseApiKeyMiddleware();
            return app;
        }
    }
}
