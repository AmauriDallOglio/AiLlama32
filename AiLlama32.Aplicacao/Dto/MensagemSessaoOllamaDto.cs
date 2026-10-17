namespace AiLlama32.Aplicacao.Dto
{
    public class MensagemSessaoOllama
    {
        public string Papel { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;
    }
}
