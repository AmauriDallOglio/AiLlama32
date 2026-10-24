using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Excluir;

public sealed record ExcluirSessaoDocumentoRequest(string Nome) : IRequest<ResultadoOperacao>;

