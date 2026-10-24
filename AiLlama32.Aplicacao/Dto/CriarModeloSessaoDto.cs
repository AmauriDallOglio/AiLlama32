namespace AiLlama32.Aplicacao.Dto;

public sealed class CriarModeloSessaoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public string ModeloBase { get; set; } = string.Empty;
}
