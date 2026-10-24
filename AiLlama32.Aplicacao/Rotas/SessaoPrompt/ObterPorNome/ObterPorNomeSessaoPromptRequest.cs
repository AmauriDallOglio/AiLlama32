using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.SessaoPrompt.ObterPorNome;

public sealed record ObterPorNomeSessaoPromptRequest(string Nome) : IRequest<ResultadoOperacao>;

