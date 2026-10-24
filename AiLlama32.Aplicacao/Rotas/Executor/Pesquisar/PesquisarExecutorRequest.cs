using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Aplicacao.Rotas.Executor.Pesquisar;

public sealed record PesquisarExecutorRequest(ExecutarSessaoDto? Dados) : IRequest<ResultadoOperacao>;

