using System.Collections.Concurrent;
using EventBus.EventHandler;

namespace EventBus
{
    public class EventService : IEventService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, EventHandlerBase> _requestHandlers = new();
        public EventService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> SendAsync<TResponse>(IEvent<TResponse> @event)
        {
            var handler = (EventHandlerWrapper<TResponse>)_requestHandlers.GetOrAdd(@event.GetType(), static eventType =>
            {
                var wrapperType = typeof(EventHandlerWrapperImpl<,>).MakeGenericType(eventType, typeof(TResponse));
                var wrapper = Activator.CreateInstance(wrapperType);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
                return (EventHandlerBase)wrapper;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            return await handler.Handle(@event, _serviceProvider);
        }
    }
}