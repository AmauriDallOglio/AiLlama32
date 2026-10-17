using AiLlama32.Aplicacao.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLlama32.Aplicacao.Rotas.DeletarHistorico
{
    public record DeletarHistoricoRequest(Guid Id) : IRequest<ResultadoOperacao>;
}
