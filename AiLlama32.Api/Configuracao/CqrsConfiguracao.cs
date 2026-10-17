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
using AiLlama32.Aplicacao.Util;

namespace AiLlama32.Api.Configuracao
{
    public static class CqrsConfiguracao
    {
        public static IServiceCollection RegistrarCqrs(this IServiceCollection services)
        {
            //// Sessões Ollama
            //services.AddScoped<ListarHandler>();
            //services.AddScoped<ObterHandler>();
            //services.AddScoped<InserirHandler>();
            //services.AddScoped<AlterarHandler>();
            //services.AddScoped<ExcluirHandler>();

            //// Pesquisa e histórico
            //services.AddScoped<PesquisarHandler>();
            //services.AddScoped<ObterHistoricoHandler>();
            //services.AddScoped<LimparHistoricoHandler>();
            //services.AddScoped<DeletarHistoricoHandler>();

            //// Integrações e configurações
            //services.AddScoped<AssuntoGeralHandler>();
            //services.AddScoped<ObterStatusIntegracaoHandler>();
            //services.AddScoped<ObterConfiguracaoOllamaHandler>();

            //// Perguntas e contexto da sessão
            //services.AddScoped<PerguntarSessaoHandler>();



            return services;
        }

        private static IServiceCollection RegistrarHandler<TRequest, THandler>(this IServiceCollection services)
            where TRequest : IRequest<ResultadoOperacao>
            where THandler : class, IContratoBaseHandler<TRequest, ResultadoOperacao>
        {
            services.AddScoped<THandler>();
            services.AddScoped<IContratoBaseHandler<TRequest, ResultadoOperacao>, THandler>();

            return services;
        }
    }
}
