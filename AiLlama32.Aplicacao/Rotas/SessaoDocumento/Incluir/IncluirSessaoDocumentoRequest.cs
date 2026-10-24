using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Incluir;

public sealed record IncluirSessaoDocumentoRequest(SessaoDocumentoDto? Dados) : IRequest<ResultadoOperacao>;

