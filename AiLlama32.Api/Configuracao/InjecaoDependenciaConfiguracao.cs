using AiLlama32.Aplicacao.Dto;
using AiLlama32.Aplicacao.Rotas.Alterar;
using AiLlama32.Aplicacao.Rotas.AlterarAssuntoAmauri;
using AiLlama32.Aplicacao.Rotas.AssuntoGeral;
using AiLlama32.Aplicacao.Rotas.DeletarHistorico;
using AiLlama32.Aplicacao.Rotas.Excluir;
using AiLlama32.Aplicacao.Rotas.Executor.Pesquisar;
using AiLlama32.Aplicacao.Rotas.Inserir;
using AiLlama32.Aplicacao.Rotas.LimparHistorico;
using AiLlama32.Aplicacao.Rotas.Listar;
using AiLlama32.Aplicacao.Rotas.Obter;
using AiLlama32.Aplicacao.Rotas.ObterConfiguracaoOllama;
using AiLlama32.Aplicacao.Rotas.ObterHistorico;
using AiLlama32.Aplicacao.Rotas.ObterStatusIntegracao;
using AiLlama32.Aplicacao.Rotas.PerguntarAmauri;
using AiLlama32.Aplicacao.Rotas.PerguntarSessao;
using AiLlama32.Aplicacao.Rotas.Pesquisar;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.Alterar;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.Excluir;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.Incluir;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.ListarTodos;
using AiLlama32.Aplicacao.Rotas.SessaoDocumento.ObterPorNome;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.Alterar;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.Excluir;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.Incluir;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.ListarTodos;
using AiLlama32.Aplicacao.Rotas.SessaoPrompt.ObterPorNome;
using AiLlama32.Aplicacao.Servico;
using Microsoft.AspNetCore.Mvc;

namespace AiLlama32.Api.Configuracao
{
    public class InjecaoDependenciaConfiguracao
    {
        public static void RegistrarServicos(WebApplicationBuilder builder)
        {


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddScoped<ListarHandler>();
            builder.Services.AddScoped<ObterHandler>();
            builder.Services.AddScoped<InserirHandler>();
            builder.Services.AddScoped<AlterarHandler>();
            builder.Services.AddScoped<ExcluirHandler>();
            builder.Services.AddScoped<PesquisarHandler>();
            builder.Services.AddScoped<ObterHistoricoHandler>();
            builder.Services.AddScoped<LimparHistoricoHandler>();
            builder.Services.AddScoped<DeletarHistoricoHandler>();
            builder.Services.AddScoped<AssuntoGeralHandler>();
            builder.Services.AddScoped<ObterStatusIntegracaoHandler>();
            builder.Services.AddScoped<ObterConfiguracaoOllamaHandler>();
            builder.Services.AddScoped<PerguntarSessaoHandler>();
            builder.Services.AddScoped<PerguntarAmauriHandler>();
            builder.Services.AddScoped<AlterarAssuntoAmauriHandler>();

            builder.Services.AddScoped<SessaoOllamaServico>();
            builder.Services.AddSingleton<Cache>();
            builder.Services.AddScoped<SessoesConhecimentoServico>();
            builder.Services.AddScoped<InicializadorCacheServico>();
            builder.Services.AddScoped<ListarTodosSessaoPromptHandler>();
            builder.Services.AddScoped<ObterPorNomeSessaoPromptHandler>();
            builder.Services.AddScoped<IncluirSessaoPromptHandler>();
            builder.Services.AddScoped<AlterarSessaoPromptHandler>();
            builder.Services.AddScoped<ExcluirSessaoPromptHandler>();
            builder.Services.AddScoped<ListarTodosSessaoDocumentoHandler>();
            builder.Services.AddScoped<ObterPorNomeSessaoDocumentoHandler>();
            builder.Services.AddScoped<IncluirSessaoDocumentoHandler>();
            builder.Services.AddScoped<AlterarSessaoDocumentoHandler>();
            builder.Services.AddScoped<ExcluirSessaoDocumentoHandler>();
            builder.Services.AddScoped<PesquisarExecutorHandler>();
            builder.Services.AddHttpClient<ExecutorServico>(cliente => cliente.Timeout = TimeSpan.FromMinutes(5));
            builder.Services.AddHttpClient<ModelosSessaoOllamaServico>(cliente =>
                cliente.Timeout = TimeSpan.FromMinutes(5));

        }
    }
}