using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Incluir;

public sealed record IncluirSessaoPromptRequest(SessaoPromptDto? Dados) : IRequest<ResultadoOperacao>;

