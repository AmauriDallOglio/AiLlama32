namespace AiLlama32.Aplicacao.Util
{
    public class ResultadoOperacao
    {
        public bool Sucesso { get; init; }
        public string Mensagem { get; init; }
        public object? Resultado { get; init; }
        public int? StatusCodigo { get; init; }
        public IEnumerable<string> Erros { get; set; }

        protected ResultadoOperacao()
        {
            Sucesso = false;
            Mensagem = string.Empty;
            Erros = Array.Empty<string>();
        }

        protected ResultadoOperacao(bool sucesso, IEnumerable<string> erros, string mensagemSucesso)
        {
            Sucesso = sucesso;
            Erros = erros;
            Mensagem = mensagemSucesso;
        }

        public static ResultadoOperacao GerarSucesso(object? dados = null, string? mensagem = null)
        {
            return new ResultadoOperacao
            {
                Sucesso = true,
                Mensagem = mensagem ?? "Operação realizada com sucesso.",
                Resultado = dados
            };
        }

        public static ResultadoOperacao GerarErro(string mensagem, int? codigo = null, object? dados = null)
        {
            return new ResultadoOperacao
            {
                Sucesso = false,
                Mensagem = mensagem,
                StatusCodigo = codigo,
                Resultado = dados
            };
        }
    }
}
