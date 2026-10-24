using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.Excluir;

public sealed record ExcluirSessaoPromptRequest(string Nome) : IRequest<ResultadoOperacao>;

