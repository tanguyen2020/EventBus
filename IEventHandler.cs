namespace EventBus
{
    public interface IEventHandler<in TRequest, TResponse>
        where TRequest : IEvent<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest @response);
    }
}
