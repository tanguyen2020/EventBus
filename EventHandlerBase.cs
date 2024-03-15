using Microsoft.Extensions.DependencyInjection;

namespace EventBus.EventHandler
{
    public abstract class EventHandlerBase
    {
    }

    public abstract class EventHandlerWrapper<TResponse> : EventHandlerBase
    {
        public abstract Task<TResponse> Handle(IEvent<TResponse> request, IServiceProvider serviceProvider);
    }

    public class EventHandlerWrapperImpl<TRequest, TResponse> : EventHandlerWrapper<TResponse>
        where TRequest : IEvent<TResponse>
    {
        public override Task<TResponse> Handle(IEvent<TResponse> @request, IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetRequiredService<IEventHandler<TRequest, TResponse>>();
            return service.HandleAsync((TRequest)@request);
        }
    }
}
