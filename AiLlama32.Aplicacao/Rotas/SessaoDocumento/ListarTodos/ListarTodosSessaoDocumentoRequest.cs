using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoDocumento.ListarTodos;

public sealed record ListarTodosSessaoDocumentoRequest() : IRequest<ResultadoOperacao>;

