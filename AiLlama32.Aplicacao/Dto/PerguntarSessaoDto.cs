namespace AiLlama32.Aplicacao.Dto
{
    public class PerguntarSessaoDto
    {
        public string Pergunta { get; set; } = string.Empty;
        public bool IncluirHistorico { get; set; } = true;
    }
}
