namespace AiLlama32.Aplicacao.Util
{
    // ============================================================
    //  CONTRATO BASE  (substitui o MediatR sem instalar nada)
    // ============================================================

    public interface IRequest<TResponse> { }

    public interface IContratoBaseHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Executar(TRequest request, CancellationToken cancellationToken = default);
    }
}
