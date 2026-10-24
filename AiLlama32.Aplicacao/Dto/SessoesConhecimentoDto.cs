namespace AiLlama32.Aplicacao.Dto;

public sealed record SessaoPromptDto(string Nome, string InstrucaoSistema, string Modelo = "llama3.2:latest");
public sealed record SessaoDocumentoDto(string Nome, string Conteudo);

public sealed class ExecutarSessaoDto
{
    public string NomePrompt { get; set; } = string.Empty;
    public string NomeDocumento { get; set; } = string.Empty;
    public string Pergunta { get; set; } = string.Empty;
}
