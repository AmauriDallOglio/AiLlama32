using AiLlama32.Aplicacao.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using OllamaSharp;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace AiLlama32.Aplicacao.Servico
{


    public class SessaoOllamaServico
    {
        private readonly IOptions<AppSettingsDto> opcoesOllama;
        private readonly List<SessaoOllamaDto> sessoes;
        private readonly string caminhoArquivoSessoes;
        private readonly ModelosSessaoOllamaServico modelosServico;
        private readonly JsonSerializerOptions opcoesJson = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };
        ConfiguracaoOllamaDto configuracaoOllamaDto = new ConfiguracaoOllamaDto();

        public SessaoOllamaServico(IOptions<AppSettingsDto> opcoesOllama, IWebHostEnvironment ambiente, ModelosSessaoOllamaServico modelosServico)
        {
            this.modelosServico = modelosServico;
            this.opcoesOllama = opcoesOllama;
            caminhoArquivoSessoes = ResolverCaminhoArquivoSessoes(ambiente.ContentRootPath);
            sessoes = CarregarSessoes();
            configuracaoOllamaDto = opcoesOllama.Value.ConfiguracaoOllama;
        }


        private List<SessaoOllamaDto> CriarSessoesIniciais()
        {

            var modeloPadrao = ObterModeloPadrao();

            return
            [
                new SessaoOllamaDto
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Nome = "Assistente de documentacao",
                Descricao = "Sessao para recuperar informacoes de documentacao interna.",
                Modelo = modeloPadrao,
                InstrucaoSistema = configuracaoOllamaDto.InstrucaoSistemaPadrao,
                Temperatura = configuracaoOllamaDto.TemperaturaPadrao,
                Tags = ["documentacao", "suporte"],
                Historico =
                [
                    new MensagemSessaoOllama { Papel = "sistema", Conteudo = configuracaoOllamaDto.MensagemSessaoInicial }
                ],
                Ativo = true
            },
            new SessaoOllamaDto
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Nome = "Assistente tecnico",
                Descricao = "Sessao para perguntas tecnicas sobre APIs.",
                Modelo = modeloPadrao,
                InstrucaoSistema = configuracaoOllamaDto.InstrucaoSistemaPadrao,
                Temperatura = configuracaoOllamaDto.TemperaturaPadrao,
                Tags = ["api", "tecnico"],
                Ativo = true
            }
            ];
        }

        public IReadOnlyCollection<SessaoOllamaDto> Listar()
        {
            return sessoes.Select(Clonar).ToList();
        }

        public SessaoOllamaDto? Obter(Guid id)
        {
            var sessao = sessoes.FirstOrDefault(item => item.Id == id);
            return sessao is null ? null : Clonar(sessao);
        }

        public ResultadoOllamaDto<object> Inserir(SessaoOllamaCriarDto requisicao)
        {
            var validacao = ValidarSessao(requisicao.Nome, requisicao.Temperatura);

            if (!string.IsNullOrWhiteSpace(validacao))
            {
                return CriarFalha<object>(validacao);
            }

            var configuracao = opcoesOllama.Value;
            var sessao = new SessaoOllamaDto
            {
                Id = Guid.NewGuid(),
                Nome = requisicao.Nome,
                Descricao = requisicao.Descricao,
                Modelo = ObterModeloPadrao(),
                InstrucaoSistema = requisicao.InstrucaoSistema,
                Temperatura = requisicao.Temperatura,
                Tags = requisicao.Tags,
                Historico =
                [
                    new MensagemSessaoOllama
                {
                    Papel = "sistema",
                    Conteudo = string.IsNullOrWhiteSpace(requisicao.InstrucaoSistema)
                        ? "Sessao criada sem instrucao personalizada."
                        : requisicao.InstrucaoSistema
                }
                ],
                Ativo = true,
                CriadoEm = DateTimeOffset.Now,
                AtualizadoEm = DateTimeOffset.Now
            };

            sessoes.Add(sessao);
            SalvarSessoes();
            return CriarSucesso<object>(new
            {
                Sessao = Clonar(sessao),
                Gravado = true,
                CaminhoArquivo = caminhoArquivoSessoes
            }, "Sessao personalizada criada e gravada para uso nas rotas do Ollama.");
        }

        public ResultadoOllamaDto<SessaoOllamaDto> Alterar(Guid id, SessaoOllamaAlterarDto requisicao)
        {
            var sessao = sessoes.FirstOrDefault(item => item.Id == id);

            if (sessao is null)
            {
                return CriarFalha<SessaoOllamaDto>("Sessao nao encontrada para alteracao.");
            }

            var validacao = ValidarSessao(requisicao.Nome, requisicao.Temperatura);

            if (!string.IsNullOrWhiteSpace(validacao))
            {
                return CriarFalha<SessaoOllamaDto>(validacao);
            }

            sessao.Nome = requisicao.Nome;
            sessao.Descricao = requisicao.Descricao;
            sessao.Modelo = ObterModeloPadrao();
            sessao.InstrucaoSistema = requisicao.InstrucaoSistema;
            sessao.Temperatura = requisicao.Temperatura;
            sessao.Tags = requisicao.Tags;
            sessao.Ativo = requisicao.Ativo;
            sessao.AtualizadoEm = DateTimeOffset.Now;

            SalvarSessoes();
            return CriarSucesso(Clonar(sessao), "Sessao personalizada alterada.");
        }

        public ResultadoOllamaDto<bool> Excluir(Guid id)
        {
            var sessao = sessoes.FirstOrDefault(item => item.Id == id);

            if (sessao is null)
            {
                return CriarFalha<bool>("Sessao nao encontrada para exclusao.");
            }

            sessoes.Remove(sessao);
            SalvarSessoes();
            return CriarSucesso(true, "Sessao personalizada excluida.");
        }

        public IReadOnlyCollection<SessaoOllamaDto> Pesquisar(PesquisaSessaoOllamaDto pesquisa)
        {
            var consulta = sessoes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(pesquisa.Termo))
            {
                consulta = consulta.Where(sessao =>
                    sessao.Nome.Contains(pesquisa.Termo, StringComparison.OrdinalIgnoreCase) ||
                    sessao.Descricao.Contains(pesquisa.Termo, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(pesquisa.Modelo))
            {
                consulta = consulta.Where(sessao => sessao.Modelo.Contains(pesquisa.Modelo, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(pesquisa.Tag))
            {
                consulta = consulta.Where(sessao => sessao.Tags.Any(tag => tag.Contains(pesquisa.Tag, StringComparison.OrdinalIgnoreCase)));
            }

            if (pesquisa.Ativo.HasValue)
            {
                consulta = consulta.Where(sessao => sessao.Ativo == pesquisa.Ativo.Value);
            }

            return consulta.Select(Clonar).ToList();
        }

        public async Task<ResultadoOllamaDto<object>> Perguntar(Guid id, PerguntarSessaoDto requisicao)
        {
            var sessao = sessoes.FirstOrDefault(item => item.Id == id);

            if (sessao is null)
            {
                return CriarFalha<object>("Sessao nao encontrada para pergunta.");
            }

            if (!sessao.Ativo)
            {
                return CriarFalha<object>("Sessao inativa nao pode ser usada para perguntas.");
            }

            if (string.IsNullOrWhiteSpace(requisicao.Pergunta))
            {
                return CriarFalha<object>("A pergunta e obrigatoria.");
            }

            if (requisicao.IncluirHistorico)
            {
                sessao.Historico.Add(new MensagemSessaoOllama
                {
                    Papel = "usuario",
                    Conteudo = requisicao.Pergunta
                });
            }

            string resposta;

            try
            {
                resposta = await PerguntarOllamaReal(sessao, requisicao.Pergunta);
            }
            catch (Exception excecao)
            {
                return CriarFalha<object>(
                    $"Nao foi possivel consultar o llama3.2:latest no Ollama para a sessao '{sessao.Nome}'. Verifique se o servidor '{configuracaoOllamaDto.UrlServidor}' esta ativo e se o modelo '{ObterModeloPadrao()}' foi baixado. Detalhe: {excecao.Message}",
                    usouOllamaReal: true);
            }

            if (requisicao.IncluirHistorico)
            {
                sessao.Historico.Add(new MensagemSessaoOllama
                {
                    Papel = "assistente",
                    Conteudo = resposta
                });
            }

            sessao.AtualizadoEm = DateTimeOffset.Now;
            SalvarSessoes();

            return CriarSucesso<object>(new
            {
                SessaoId = sessao.Id,
                sessao.Nome,
                sessao.Modelo,
                Pergunta = requisicao.Pergunta,
                Resposta = resposta,
                requisicao.IncluirHistorico,
                TotalMensagensHistorico = sessao.Historico.Count
            }, "Pergunta respondida usando a sessao personalizada.", usouOllamaReal: true);
        }

        public ResultadoOllamaDto<object> ObterHistorico(Guid id)
        {
            var sessao = sessoes.FirstOrDefault(item => item.Id == id);

            if (sessao is null)
            {
                return CriarFalha<object>("Sessao nao encontrada para consultar historico.");
            }

            return CriarSucesso<object>(new
            {
                SessaoId = sessao.Id,
                sessao.Nome,
                sessao.Descricao,
                sessao.Modelo,
                TotalMensagensHistorico = sessao.Historico.Count,
                Historico = sessao.Historico
                    .OrderBy(mensagem => mensagem.CriadoEm)
                    .Select(mensagem => new
                    {
                        mensagem.Papel,
                        mensagem.Conteudo,
                        mensagem.CriadoEm
                    })
                    .ToList()
            }, "Historico da sessao obtido.");
        }

        public ResultadoOllamaDto<object> LimparHistorico(Guid id)
        {
            var sessao = sessoes.FirstOrDefault(item => item.Id == id);

            if (sessao is null)
            {
                return CriarFalha<object>("Sessao nao encontrada para limpar historico.");
            }

            sessao.Historico.Clear();
            sessao.AtualizadoEm = DateTimeOffset.Now;
            SalvarSessoes();

            return CriarSucesso<object>(new
            {
                SessaoId = sessao.Id,
                sessao.Nome,
                TotalMensagensHistorico = sessao.Historico.Count
            }, "Historico da sessao limpo.");
        }

        public async Task<ResultadoOllamaDto<object>> AssuntoGeral(PerguntarSessaoDto requisicao)
        {
            if (string.IsNullOrWhiteSpace(requisicao.Pergunta))
            {
                return CriarFalha<object>("A pergunta e obrigatoria para o assunto geral.");
            }

            var configuracao = opcoesOllama.Value.ConfiguracaoOllama;
            var modeloDireto = ObterModeloPadrao();

            try
            {
                var resposta = await PerguntarLlama32Real(modeloDireto, requisicao.Pergunta);

                return CriarSucesso<object>(new
                {
                    Modelo = modeloDireto,
                    Pergunta = requisicao.Pergunta,
                    Resposta = resposta,
                    UsouSessaoPersonalizada = false,
                    TipoConsulta = configuracao.TipoConsultaAssuntoGeral
                }, "Pergunta respondida diretamente no modelo llama3.2:latest.", usouOllamaReal: true);
            }
            catch (Exception excecao)
            {
                return CriarFalha<object>(
                    $"Nao foi possivel consultar o llama3.2:latest no Ollama. Verifique se o servidor '{configuracao.UrlServidor}' esta ativo e se o modelo '{modeloDireto}' foi baixado. Detalhe: {excecao.Message}",
                    usouOllamaReal: true);
            }
        }

        public ResultadoOllamaDto<object> ObterStatusIntegracao()
        {
            var configuracao = configuracaoOllamaDto;

            return CriarSucesso<object>(new
            {
                configuracao.UrlServidor,
                configuracao.ModeloPadrao,
                configuracao.UsarOllamaReal,
                configuracao.QuantidadeMaximaTokens,
                configuracao.TemperaturaPadrao,
                TotalSessoes = sessoes.Count,
                PacotePreparado = configuracao.PacoteIntegracao
            }, "Status da integracao Ollama obtido.");
        }

        public async Task<ResultadoOllamaDto<object>> AlterarAssuntoAmauri(string assunto, CancellationToken cancelamento = default)
        {
            if (string.IsNullOrWhiteSpace(assunto))
                return CriarFalha<object>("O novo assunto e obrigatorio.");

            const string modelo = "sessao_amauri";
            var instrucaoSistema = "Voce e Arq. Atlas. Responda em portugues do Brasil, de forma clara e objetiva. "
                + "Use o seguinte assunto e contexto nas proximas respostas:\n" + assunto.Trim();

            try
            {
                using var cliente = new HttpClient
                {
                    BaseAddress = new Uri(configuracaoOllamaDto.UrlServidor.TrimEnd('/') + "/"),
                    Timeout = TimeSpan.FromMinutes(5)
                };
                // Reutiliza os pesos do modelo existente e substitui seu contexto persistido.
                // Substitui exemplos antigos pelo novo contexto (lista vazia herda os antigos no Ollama).
                using var resposta = await cliente.PostAsJsonAsync("api/create", new
                {
                    model = modelo,
                    from = modelo,
                    system = instrucaoSistema,
                    messages = new[] { new { role = "user", content = assunto.Trim() } },
                    stream = false
                }, cancelamento);

                if (!resposta.IsSuccessStatusCode)
                    return CriarFalha<object>(
                        $"Nao foi possivel alterar sessao_amauri no Ollama (HTTP {(int)resposta.StatusCode}). Verifique se o modelo existe no servidor configurado.",
                        usouOllamaReal: true);

                using var conteudo = await JsonDocument.ParseAsync(await resposta.Content.ReadAsStreamAsync(cancelamento), cancellationToken: cancelamento);
                if (conteudo.RootElement.ValueKind != JsonValueKind.Object
                    || !conteudo.RootElement.TryGetProperty("status", out var status)
                    || status.ValueKind != JsonValueKind.String || status.GetString() != "success")
                    return CriarFalha<object>("O Ollama nao confirmou a alteracao de sessao_amauri.", usouOllamaReal: true);

                string texto;
                try
                {
                    texto = await modelosServico.GerarResposta(modelo, assunto.Trim(), cancelamento);
                }
                catch (Exception excecao) when ((excecao is HttpRequestException or OperationCanceledException or JsonException) && !cancelamento.IsCancellationRequested)
                {
                    var falha = CriarFalha<object>("Assunto atualizado no Ollama, mas nao foi possivel gerar a resposta. Consulte a sessao ou use a rota de perguntas.", usouOllamaReal: true);
                    falha.Dados = new { Modelo = modelo, Assunto = assunto.Trim(), AssuntoAtualizado = true, Resposta = (string?)null };
                    return falha;
                }

                return CriarSucesso<object>(new
                {
                    Modelo = modelo,
                    Assunto = assunto.Trim(),
                    InstrucaoSistema = instrucaoSistema,
                    AssuntoAtualizado = true,
                    Resposta = texto
                }, "Assunto de sessao_amauri atualizado e respondido pelo Ollama.", usouOllamaReal: true);
            }
            catch (OperationCanceledException) when (cancelamento.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception excecao) when (excecao is HttpRequestException or OperationCanceledException or JsonException)
            {
                return CriarFalha<object>("Nao foi possivel concluir a alteracao de sessao_amauri. Verifique a conexao com o Ollama e consulte o modelo antes de tentar novamente.",
                    usouOllamaReal: true);
            }
        }

        public async Task<ResultadoOllamaDto<object>> PerguntarAmauri(string pergunta, CancellationToken cancelamento = default)
        {
            const string modelo = "sessao_amauri";
            try
            {
                var resposta = await modelosServico.ResponderComContexto(modelo, pergunta, cancelamento);

                return CriarSucesso<object>(new
                {
                    Modelo = modelo,
                    Pergunta = pergunta,
                    resposta.Resposta,
                    resposta.EncontrouNoContexto,
                    resposta.Fontes
                }, resposta.EncontrouNoContexto ? "Resposta extraida do contexto de sessao_amauri." : "Informacao ausente no contexto de sessao_amauri.", usouOllamaReal: true);
            }
            catch (OperationCanceledException) when (cancelamento.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception)
            {
                return CriarFalha<object>(
                    "Nao foi possivel consultar sessao_amauri. Verifique se o Ollama esta ativo em UrlServidor e se o modelo sessao_amauri esta instalado nesse servidor.",
                    usouOllamaReal: true);
            }
        }

        private async Task<string> PerguntarOllamaReal(SessaoOllamaDto sessao, string pergunta)
        {
            var configuracao = configuracaoOllamaDto;
            var modelo = ObterModeloPadrao();
            var cliente = new OllamaApiClient(new Uri(configuracao.UrlServidor), modelo);
            var chat = new Chat(cliente, sessao.InstrucaoSistema);
            var resposta = new StringBuilder();

            await foreach (var trecho in chat.SendAsync(pergunta))
            {
                resposta.Append(trecho);
            }

            return resposta.ToString();
        }

        private async Task<string> PerguntarLlama32Real(string modelo, string pergunta)
        {
            var configuracao = configuracaoOllamaDto;
            var cliente = new OllamaApiClient(new Uri(configuracao.UrlServidor), modelo);
            var chat = new Chat(cliente, configuracao.InstrucaoAssuntoGeral);
            var resposta = new StringBuilder();

            await foreach (var trecho in chat.SendAsync(pergunta))
            {
                resposta.Append(trecho);
            }

            return resposta.ToString();
        }

        private string ObterModeloPadrao()
        {
            var modeloConfigurado = configuracaoOllamaDto.ModeloPadrao;
            return string.IsNullOrWhiteSpace(modeloConfigurado) ? "llama3.2:latest" : modeloConfigurado;
        }

        private List<SessaoOllamaDto> CarregarSessoes()
        {
            if (!File.Exists(caminhoArquivoSessoes))
            {
                var sessoesIniciais = CriarSessoesIniciais();
                SalvarSessoes(sessoesIniciais);
                return sessoesIniciais;
            }

            var conteudo = File.ReadAllText(caminhoArquivoSessoes);

            if (string.IsNullOrWhiteSpace(conteudo))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<SessaoOllamaDto>>(conteudo, opcoesJson) ?? [];
        }

        private void SalvarSessoes()
        {
            SalvarSessoes(sessoes);
        }

        private void SalvarSessoes(List<SessaoOllamaDto> sessoesParaSalvar)
        {
            var diretorio = Path.GetDirectoryName(caminhoArquivoSessoes);

            if (!string.IsNullOrWhiteSpace(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            var conteudo = JsonSerializer.Serialize(sessoesParaSalvar, opcoesJson);
            File.WriteAllText(caminhoArquivoSessoes, conteudo);
        }

        private string ResolverCaminhoArquivoSessoes(string caminhoBase)
        {
            var caminhoConfigurado = configuracaoOllamaDto.CaminhoArquivoSessoes;
            var caminho = string.IsNullOrWhiteSpace(caminhoConfigurado)
                ? "Dados/sessoes-ollama.json"
                : caminhoConfigurado;

            return Path.IsPathRooted(caminho)
                ? caminho
                : Path.Combine(caminhoBase, caminho);
        }

        private static string? ValidarSessao(string nome, decimal temperatura)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return "O nome da sessao e obrigatorio.";
            }

            if (temperatura is < 0 or > 2)
            {
                return "A temperatura deve estar entre 0 e 2.";
            }

            return null;
        }

        private ResultadoOllamaDto<T> CriarSucesso<T>(T dados, string mensagem, bool? usouOllamaReal = null)
        {
            return new ResultadoOllamaDto<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                UsouOllamaReal = usouOllamaReal ?? configuracaoOllamaDto.UsarOllamaReal,
                Dados = dados
            };
        }

        private ResultadoOllamaDto<T> CriarFalha<T>(string mensagem, bool? usouOllamaReal = null)
        {
            return new ResultadoOllamaDto<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                UsouOllamaReal = usouOllamaReal ?? configuracaoOllamaDto.UsarOllamaReal
            };
        }

        private static SessaoOllamaDto Clonar(SessaoOllamaDto sessao)
        {
            return new SessaoOllamaDto
            {
                Id = sessao.Id,
                Nome = sessao.Nome,
                Descricao = sessao.Descricao,
                Modelo = sessao.Modelo,
                InstrucaoSistema = sessao.InstrucaoSistema,
                Temperatura = sessao.Temperatura,
                Tags = sessao.Tags.ToList(),
                Historico = sessao.Historico.Select(mensagem => new MensagemSessaoOllama
                {
                    Papel = mensagem.Papel,
                    Conteudo = mensagem.Conteudo,
                    CriadoEm = mensagem.CriadoEm
                }).ToList(),
                Ativo = sessao.Ativo,
                CriadoEm = sessao.CriadoEm,
                AtualizadoEm = sessao.AtualizadoEm
            };
        }
    }
}