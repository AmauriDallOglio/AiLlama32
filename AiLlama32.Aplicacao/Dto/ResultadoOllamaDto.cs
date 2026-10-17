using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Dto
{
    public class ResultadoOllamaDto<T> : ResultadoOperacao
    {
        public bool UsouOllamaReal { get; set; }
        public T? Dados { get; set; }
    }
}
