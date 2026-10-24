using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.Alterar;

public sealed record AlterarSessaoDocumentoRequest(string Nome, SessaoDocumentoDto? Dados) : IRequest<ResultadoOperacao>;

