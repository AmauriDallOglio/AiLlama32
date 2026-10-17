namespace AiLlama32.Aplicacao.Dto
{
    public class AppSettingsDto
    {
        public LoggingDto LoggingDto { get; set; } = new LoggingDto();
        public ConfiguracaoOllamaDto ConfiguracaoOllamaDto { get; set; } = new ConfiguracaoOllamaDto();
        public RateLimitDto RateLimitDto { get; set; } = new RateLimitDto();
        public SegurancaDto SegurancaDto { get; set; } = new SegurancaDto();
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
        public int RequisicoesPermitidas { get; set; } = 100;
        public int JanelaEmSegundos { get; set; } = 60;
        public int TamanhoFila { get; set; }
    }

    public class ConfiguracaoOllamaDto
    {
        public string UrlServidor { get; set; } = "http://localhost:11434";
        public string ModeloPadrao { get; set; } = "llama3.2:latest";
        public bool UsarOllamaReal { get; set; } = false;
        public int QuantidadeMaximaTokens { get; set; } = 1024;
        public decimal TemperaturaPadrao { get; set; } = 0.7m;
        public string InstrucaoSistemaPadrao { get; set; } = "Responda em portugues, com objetividade, usando o contexto da sessao.";
        public string InstrucaoAssuntoGeral { get; set; } = "Responda em portugues de forma clara, objetiva e util.";
        public string MensagemSessaoInicial { get; set; } = "Sessao inicial preparada para documentacao.";
        public string TipoConsultaAssuntoGeral { get; set; } = "Assunto geral direto no llama3.2:latest";
        public string PacoteIntegracao { get; set; } = "OllamaSharp";
        public string CaminhoArquivoSessoes { get; set; } = "Dados/sessoes-ollama.json";
        public string Observacao { get; set; } = "Configure o endpoint do Ollama e o modelo no Azure App Settings quando usar integracao real.";
    }

    public class SegurancaDto
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
