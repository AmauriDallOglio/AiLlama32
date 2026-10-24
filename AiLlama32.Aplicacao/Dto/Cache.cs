using AiLlama32.Aplicacao.Util;
using System.Text.RegularExpressions;

namespace AiLlama32.Aplicacao.Dto;

public sealed class DadosCache
{
    public List<SessaoPromptDto> Prompts { get; set; } = [];
    public List<SessaoDocumentoDto> Documentos { get; set; } = [];

    public DadosCache Copiar() => new() { Prompts = [.. Prompts], Documentos = [.. Documentos] };
}

// Singleton registrado na injecao. O estado e o bloqueio pertencem ao cache.
public sealed class Cache
{
    private readonly object bloqueio = new();
    private DadosCache? dados;
    private DadosCache Estado => dados ?? throw new InvalidOperationException("Cache nao inicializado.");

    public (int Prompts, int Documentos) Inicializar(Func<DadosCache> carregar)
    {
        lock (bloqueio)
        {
            dados ??= carregar();
            return (Estado.Prompts.Count, Estado.Documentos.Count);
        }
    }

    public T Consultar<T>(Func<DadosCache> carregar, Func<Cache, T> consulta)
    {
        lock (bloqueio)
        {
            dados ??= carregar();
            return consulta(this);
        }
    }

    public ResultadoOperacao Atualizar(Func<DadosCache> carregar, Func<Cache, ResultadoOperacao> operacao, Action<DadosCache> persistir)
    {
        lock (bloqueio)
        {
            dados ??= carregar();
            var copia = new Cache { dados = Estado.Copiar() };
            var resultado = operacao(copia);
            if (!resultado.Sucesso) return resultado;
            persistir(copia.Estado.Copiar());
            dados = copia.Estado;
            return resultado;
        }
    }

    public static bool NomeValido(string? nome) => nome is not null && nome.Length <= 128
        && Regex.IsMatch(nome, @"\A[a-zA-Z0-9][a-zA-Z0-9._-]*\z");
    private static bool MesmoNome(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    // Operacoes internas executadas pelas transacoes acima; nunca expoem as colecoes.
    internal IReadOnlyList<SessaoPromptDto> ObterTodosPrompts() => Estado.Prompts.OrderBy(x => x.Nome).ToArray();
    internal IReadOnlyList<SessaoDocumentoDto> ObterTodosDocumentos() => Estado.Documentos.OrderBy(x => x.Nome).ToArray();
    internal SessaoPromptDto? ObterPromptPorNome(string nome) => Estado.Prompts.FirstOrDefault(x => MesmoNome(x.Nome, nome));
    internal SessaoDocumentoDto? ObterDocumentoPorNome(string nome) => Estado.Documentos.FirstOrDefault(x => MesmoNome(x.Nome, nome));

    internal ResultadoOperacao InserirPrompt(SessaoPromptDto item) => GravarPrompt(item, null);
    internal ResultadoOperacao AlterarPrompt(string nome, SessaoPromptDto item) => GravarPrompt(item, nome);
    internal ResultadoOperacao InserirDocumento(SessaoDocumentoDto item) => GravarDocumento(item, null);
    internal ResultadoOperacao AlterarDocumento(string nome, SessaoDocumentoDto item) => GravarDocumento(item, nome);

    private ResultadoOperacao GravarPrompt(SessaoPromptDto item, string? nomeAtual)
    {
        if (!NomeValido(item.Nome) || string.IsNullOrWhiteSpace(item.InstrucaoSistema) || string.IsNullOrWhiteSpace(item.Modelo))
            return ResultadoOperacao.GerarErro("Informe nome valido, instrucaoSistema e modelo. Nomes aceitam letras, numeros, ponto, hifen e sublinhado.", 400);
        var indice = nomeAtual is null ? -1 : Estado.Prompts.FindIndex(x => MesmoNome(x.Nome, nomeAtual));
        if (nomeAtual is not null && indice < 0) return ResultadoOperacao.GerarErro("Prompt nao encontrado.", 404);
        if (Estado.Prompts.Where((_, i) => i != indice).Any(x => MesmoNome(x.Nome, item.Nome)))
            return ResultadoOperacao.GerarErro("Ja existe um prompt com esse nome.", 409);
        item = item with { InstrucaoSistema = item.InstrucaoSistema.Trim(), Modelo = item.Modelo.Trim() };
        if (indice < 0) Estado.Prompts.Add(item); else Estado.Prompts[indice] = item;
        return ResultadoOperacao.GerarSucesso(item, "Prompt salvo.");
    }

    private ResultadoOperacao GravarDocumento(SessaoDocumentoDto item, string? nomeAtual)
    {
        if (!NomeValido(item.Nome) || string.IsNullOrWhiteSpace(item.Conteudo))
            return ResultadoOperacao.GerarErro("Informe nome valido e conteudo do documento.", 400);
        var indice = nomeAtual is null ? -1 : Estado.Documentos.FindIndex(x => MesmoNome(x.Nome, nomeAtual));
        if (nomeAtual is not null && indice < 0) return ResultadoOperacao.GerarErro("Documento nao encontrado.", 404);
        if (Estado.Documentos.Where((_, i) => i != indice).Any(x => MesmoNome(x.Nome, item.Nome)))
            return ResultadoOperacao.GerarErro("Ja existe um documento com esse nome.", 409);
        item = item with { Conteudo = item.Conteudo.Trim() };
        if (indice < 0) Estado.Documentos.Add(item); else Estado.Documentos[indice] = item;
        return ResultadoOperacao.GerarSucesso(item, "Documento salvo.");
    }

    internal ResultadoOperacao ExcluirPrompt(string nome) => Excluir(nome, true);
    internal ResultadoOperacao ExcluirDocumento(string nome) => Excluir(nome, false);
    private ResultadoOperacao Excluir(string nome, bool prompt)
    {
        var removidos = prompt ? Estado.Prompts.RemoveAll(x => MesmoNome(x.Nome, nome)) : Estado.Documentos.RemoveAll(x => MesmoNome(x.Nome, nome));
        return removidos == 0 ? ResultadoOperacao.GerarErro("Sessao nao encontrada.", 404)
            : ResultadoOperacao.GerarSucesso(new { Nome = nome }, "Sessao excluida.");
    }
}
