using AiLlama32.Aplicacao.Dto;
using Microsoft.Extensions.Options;

namespace AiLlama32.Api.Configuracao
{
    public static class AppSettingsConfiguracao
    {
        public static void Carregar(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<AppSettingsDto>(configuration);

            CarregaBancoDeDados(services);
            services.RegistrarRateLimit(configuration.Get<AppSettingsDto>() ?? new AppSettingsDto());


        }

        private static AppSettingsDto CarregaBancoDeDados(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var monitor = provider.GetRequiredService<IOptionsMonitor<AppSettingsDto>>();
            AppSettingsDto appSettingsDto = monitor.CurrentValue;





            return appSettingsDto;
        }

        public static void AtivarAppSettinngsConfiguracao(this WebApplication app)
        {
            // Resolve o monitor para ativar o OnChange
            var monitor = app.Services.GetRequiredService<IOptionsMonitor<AppSettingsDto>>();

            monitor.OnChange(settings =>
            {
                Console.WriteLine($"[CONFIG] AppSettingsDto alterado em {DateTime.Now}");
                Console.WriteLine($"Nova API Key: {settings.Seguranca.ApiKey}");
                Console.WriteLine($"RateLimit habilitado: {settings.RateLimit.Habilitado}");
            });

            // Configuração dinâmica do RateLimiter
            if (monitor.CurrentValue.RateLimit.Habilitado)
                app.UseRateLimiter();
        }




    }
}
