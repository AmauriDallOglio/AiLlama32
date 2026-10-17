using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Dto
{
    public class PesquisaSessaoOllamaDto
    {
        public string? Termo { get; set; }
        public string? Modelo { get; set; }
        public string? Tag { get; set; }
        public bool? Ativo { get; set; }
    }
}
