using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventBus
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, Assembly assemblies)
        {
            services.AddScoped<IEventService, EventService>();
            AddEventtBusClasses(services, assemblies);

            return services;
        }
        public static void AddEventtBusClasses(IServiceCollection services, Assembly assemblies)
        {
            ImplementTypes(services, typeof(IEventHandler<,>), assemblies);
        }
        private static void ImplementTypes(IServiceCollection services, Type type, Assembly assemblies)
        {
            var @interfaces = assemblies.GetTypes().SelectMany(t => t.GetInterfaces()).Where(p => p.IsGenericType && (p.GetGenericTypeDefinition() == type)).ToList();
            foreach (var @interface in @interfaces)
            {
                var @typeImpls = assemblies.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface).Where(n => @interface.IsAssignableFrom(n)).ToList();
                foreach (var @typeImpl in @typeImpls)
                {
                    services.AddTransient(@interface, @typeImpl);
                }
            }
        }
    }
}
