using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Servico;
using AiLlama32.Aplicacao.Rotas.Executor.Pesquisar;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

var arquivo = Path.Combine(Path.GetTempPath(), $"AiLlama32-tratamentos-{Guid.NewGuid():N}.json");
var opcoes = Options.Create(new AppSettingsDto { ConfiguracaoOllama = new()
{
    CaminhoArquivoConhecimento = arquivo, UrlServidor = "http://ollama.test:11434"
} });
var cadastros = new SessoesConhecimentoServico(new Ambiente(), opcoes, new Cache());
var fake = new OllamaSimulado();
using var http = new HttpClient(fake);
var executor = new ExecutorServico(http, cadastros, opcoes);
var pesquisar = new PesquisarExecutorHandler(executor);
var pergunta = new ExecutarSessaoDto { NomePrompt = "prompt", NomeDocumento = "documento", Pergunta = "O que significa DDD?" };
var prompt = new SessaoPromptDto("prompt", "Responda usando o documento.");
var documento = new SessaoDocumentoDto("documento", "DDD significa Domain-Driven Design. DDD organiza o modelo em torno das regras do negocio.");
var total = 0;

void Gravar(SessaoPromptDto? p, SessaoDocumentoDto? d)
{
    File.WriteAllText(arquivo, JsonSerializer.Serialize(new
    {
        prompts = p is null ? Array.Empty<SessaoPromptDto>() : new[] { p },
        documentos = d is null ? Array.Empty<SessaoDocumentoDto>() : new[] { d }
    }));
    // Simula nova inicializacao: edicoes externas nao recarregam um cache existente.
    cadastros = new SessoesConhecimentoServico(new Ambiente(), opcoes, new Cache());
    executor = new ExecutorServico(http, cadastros, opcoes);
    pesquisar = new PesquisarExecutorHandler(executor);
}

async Task Verificar(int status, string? codigo = null, bool encontrou = false)
{
    var resultado = await pesquisar.Executar(new PesquisarExecutorRequest(pergunta), default);
    if ((resultado.StatusCodigo ?? 200) != status) throw new Exception($"Esperado HTTP {status}: {resultado.Mensagem}");
    var dados = JsonSerializer.SerializeToElement(resultado.Resultado);
    if (codigo is not null && dados.GetProperty("Codigo").GetString() != codigo) throw new Exception($"Esperado codigo {codigo}");
    if (dados.GetProperty("EncontrouNoContexto").GetBoolean() != encontrou) throw new Exception("Estado de contexto incorreto.");
    if (string.IsNullOrWhiteSpace(dados.GetProperty("Resposta").GetString())) throw new Exception("Resposta/mensagem vazia.");
    if (!encontrou && dados.GetProperty("Fontes").GetArrayLength() != 0) throw new Exception("Erro nao pode retornar fontes.");
    Console.WriteLine($"PASS {++total}: HTTP {status}, {codigo ?? resultado.Mensagem}");
}

try
{
    pergunta.NomePrompt = "";
    await Verificar(400, "NOME_PROMPT_INVALIDO");
    pergunta.NomePrompt = "prompt";
    pergunta.NomeDocumento = "";
    await Verificar(400, "NOME_DOCUMENTO_INVALIDO");
    pergunta.NomeDocumento = "documento";
    pergunta.Pergunta = " ";
    await Verificar(400, "PERGUNTA_VAZIA");
    pergunta.Pergunta = "O que significa DDD?";
    Gravar(null, null);
    await Verificar(404, "PROMPT_NAO_ENCONTRADO");
    Gravar(prompt, null);
    await Verificar(404, "DOCUMENTO_NAO_ENCONTRADO");
    Gravar(prompt with { InstrucaoSistema = " " }, documento);
    await Verificar(422, "PROMPT_SEM_DADOS");
    Gravar(prompt with { Modelo = "" }, documento);
    await Verificar(422, "PROMPT_SEM_MODELO");
    Gravar(prompt, documento with { Conteudo = " " });
    await Verificar(422, "DOCUMENTO_SEM_DADOS");
    Gravar(prompt, documento);
    foreach (var (modo, status, codigo) in new[]
    {
        ("off", 503, "OLLAMA_INDISPONIVEL"),
        ("timeout", 504, "OLLAMA_TEMPO_ESGOTADO"),
        ("modelo", 422, "MODELO_NAO_ENCONTRADO"),
        ("erro", 502, "OLLAMA_ERRO"),
        ("vazio", 502, "OLLAMA_RESPOSTA_INVALIDA"),
        ("json", 502, "OLLAMA_RESPOSTA_INVALIDA"),
        ("fonte", 502, "OLLAMA_RESPOSTA_INVALIDA")
    })
    {
        fake.Modo = modo;
        await Verificar(status, codigo);
    }
    fake.Modo = "sucesso";
    await Verificar(200, encontrou: true);
    if (!fake.UltimoTrecho.Contains("regras do negocio")) throw new Exception("Paragrafo foi fragmentado em frases isoladas.");
    var chamadas = fake.Chamadas;
    pergunta.Pergunta = "Quais planetas existem no sistema solar?";
    await Verificar(200);
    if (fake.Chamadas != chamadas) throw new Exception("Busca sem fonte nao deve chamar Ollama.");
    pergunta.Pergunta = "DDD?";
    fake.Modo = "recusa";
    await Verificar(200);
    opcoes.Value.ConfiguracaoOllama.UrlServidor = "";
    await Verificar(503, "OLLAMA_CONFIGURACAO_INVALIDA");
    Console.WriteLine($"{total} verificacoes aprovadas.");
    CacheVerificacoes.Executar();
}
finally { File.Delete(arquivo); }

sealed class OllamaSimulado : HttpMessageHandler
{
    public string Modo = "sucesso";
    public int Chamadas;
    public string UltimoTrecho = "";
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Chamadas++;
        if (Modo == "off") throw new HttpRequestException("Conexao recusada.");
        if (Modo == "timeout") throw new TaskCanceledException("Tempo esgotado.");
        if (Modo == "modelo") return new(HttpStatusCode.NotFound);
        if (Modo == "erro") return new(HttpStatusCode.InternalServerError);
        using var corpo = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(cancellationToken));
        var mensagens = corpo.RootElement.GetProperty("messages");
        using var entrada = JsonDocument.Parse(mensagens[1].GetProperty("content").GetString()!);
        var trecho = entrada.RootElement.GetProperty("trechos")[0];
        UltimoTrecho = trecho.GetProperty("texto").GetString()!;
        var id = trecho.GetProperty("id").GetInt32();
        var conteudo = Modo switch
        {
            "vazio" => " ", "json" => "invalid", "fonte" => "{\"indices\":[999999]}",
            "recusa" => "{\"indices\":[]}", _ => JsonSerializer.Serialize(new { indices = new[] { id } })
        };
        return new(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(new { message = new { content = conteudo } }), Encoding.UTF8, "application/json") };
    }
}

sealed class Ambiente : IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = "Testing";
    public string ApplicationName { get; set; } = "ExecutorTratamentos";
    public string WebRootPath { get; set; } = Path.GetTempPath();
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    public string ContentRootPath { get; set; } = Path.GetTempPath();
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
}
