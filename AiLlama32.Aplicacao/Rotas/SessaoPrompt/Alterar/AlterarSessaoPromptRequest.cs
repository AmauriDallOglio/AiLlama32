using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Alterar;

public sealed record AlterarSessaoPromptRequest(string Nome, SessaoPromptDto? Dados) : IRequest<ResultadoOperacao>;

