using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Dto
{
    public class SessaoOllamaCriarDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string InstrucaoSistema { get; set; } = string.Empty;
        public decimal Temperatura { get; set; } = 0.7m;
        public List<string> Tags { get; set; } = [];
    }
}
