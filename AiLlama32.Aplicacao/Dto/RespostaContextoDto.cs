namespace AiLlama32.Aplicacao.Dto;

public sealed record RespostaContextoDto(string Resposta, bool EncontrouNoContexto, IReadOnlyList<string> Fontes);
