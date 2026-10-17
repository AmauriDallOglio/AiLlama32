namespace AiLlama32.Aplicacao.Dto
{
    public class AppSettingsDto
    {
        public LoggingDto Logging { get; set; } = new LoggingDto();
        public ConfiguracaoOllamaDto ConfiguracaoOllama { get; set; } = new ConfiguracaoOllamaDto();
        public RateLimitDto RateLimit { get; set; } = new RateLimitDto();
        public SegurancaDto Seguranca { get; set; } = new SegurancaDto();
    }

    public class LoggingDto
    {
        public LogLevelDto LogLevel { get; set; } = new LogLevelDto();
    }

    public class LogLevelDto
    {
        public string Default { get; set; } = string.Empty;
        [System.Text.Json.Serialization.JsonPropertyName("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; } = string.Empty;
    }

    public class RateLimitDto
    {
        public bool Habilitado { get; set; } = true;
        public int RequisicoesPermitidas { get; set; } = 0;
        public int JanelaEmSegundos { get; set; } = 0;
        public int TamanhoFila { get; set; }
    }

    public class ConfiguracaoOllamaDto
    {
        public string UrlServidor { get; set; } = string.Empty;
        public string ModeloPadrao { get; set; } = string.Empty;
        public bool UsarOllamaReal { get; set; } = false;
        public int QuantidadeMaximaTokens { get; set; } = 0;
        public decimal TemperaturaPadrao { get; set; } = 0;
        public string InstrucaoSistemaPadrao { get; set; } = string.Empty;
        public string InstrucaoAssuntoGeral { get; set; } = string.Empty;
        public string MensagemSessaoInicial { get; set; } = string.Empty;
        public string TipoConsultaAssuntoGeral { get; set; } = string.Empty;
        public string PacoteIntegracao { get; set; } = string.Empty;
        public string CaminhoArquivoSessoes { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
    }

    public class SegurancaDto
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
