using AiLlama32.Aplicacao.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Rotas.LimparHistorico
{
    public record LimparHistoricoRequest(Guid Id) : IRequest<ResultadoOperacao>;
}
