namespace AiLlama32.Aplicacao.Dto
{
    public class SessaoOllamaDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string InstrucaoSistema { get; set; } = string.Empty;
        public decimal Temperatura { get; set; }
        public List<string> Tags { get; set; } = [];
        public List<MensagemSessaoOllama> Historico { get; set; } = [];
        public bool Ativo { get; set; } = true;
        public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset AtualizadoEm { get; set; } = DateTimeOffset.UtcNow;
    }


}
