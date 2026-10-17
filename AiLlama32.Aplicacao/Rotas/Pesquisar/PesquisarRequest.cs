using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Rotas.Pesquisar
{
    public record PesquisarRequest(PesquisaSessaoOllamaDto Dados) : IRequest<ResultadoOperacao>;
}
