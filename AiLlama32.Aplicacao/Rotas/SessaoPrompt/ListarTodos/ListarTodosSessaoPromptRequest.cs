using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.ListarTodos;

public sealed record ListarTodosSessaoPromptRequest() : IRequest<ResultadoOperacao>;

