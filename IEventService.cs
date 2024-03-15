namespace EventBus
{
    public interface IEventService
    {
        Task<TResponse> SendAsync<TResponse>(IEvent<TResponse> @event);
    }
}
