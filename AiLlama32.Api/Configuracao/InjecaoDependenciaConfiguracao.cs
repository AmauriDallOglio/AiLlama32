using AiLlama32.Aplicacao.Rotas.Alterar;
using AiLlama32.Aplicacao.Rotas.AssuntoGeral;
using AiLlama32.Aplicacao.Rotas.DeletarHistorico;
using AiLlama32.Aplicacao.Rotas.Excluir;
using AiLlama32.Aplicacao.Rotas.Inserir;
using AiLlama32.Aplicacao.Rotas.LimparHistorico;
using AiLlama32.Aplicacao.Rotas.Listar;
using AiLlama32.Aplicacao.Rotas.Obter;
using AiLlama32.Aplicacao.Rotas.ObterConfiguracaoOllama;
using AiLlama32.Aplicacao.Rotas.ObterHistorico;
using AiLlama32.Aplicacao.Rotas.ObterStatusIntegracao;
using AiLlama32.Aplicacao.Rotas.PerguntarSessao;
using AiLlama32.Aplicacao.Rotas.Pesquisar;
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

            builder.Services.AddScoped<SessaoOllamaServico>();

        }
    }
}