using Microsoft.Extensions.DependencyInjection;
using ObjectBus.Options;
using System;

namespace ObjectBus.Extensions
{
    public static class ServiceBusExtensions
    {
        /// <summary>
        /// Creates an Azure Service Bus using the object provided as the transport mechanism. 
        /// The service bus is injected via the IServiceBus<T>.
        /// </summary>
        /// <typeparam name="T">Object to send/recieve.</typeparam>
        /// <param name="serviceCollection">Collection of services to inject into.</param>
        /// <param name="optionsAction">Configuration options of the application.</param>
        /// <returns></returns>
        public static IServiceCollection CreateObjectBus<TObjectType>(this IServiceCollection serviceCollection, Action<ObjectBusOptions> optionsAction)
        {
            return CreateObjectBus<TObjectType, ObjectBus<TObjectType>>(serviceCollection, optionsAction);
        }

        /// <summary>
        /// Creates an Azure Service Bus using the object provided as the transport mechanism. 
        /// The service bus is injected via the IServiceBus<T>.
        /// </summary>
        /// <typeparam name="T">Object to send/recieve.</typeparam>
        /// <param name="serviceCollection">Collection of services to inject into.</param>
        /// <param name="optionsAction">Configuration options of the application.</param>
        /// <returns></returns>
        public static IServiceCollection CreateObjectBus<TObjectType, TObjectBus>(this IServiceCollection serviceCollection, Action<ObjectBusOptions> optionsAction)
            where TObjectBus : ObjectBus<TObjectType>
        {
            serviceCollection.Configure(optionsAction);
            serviceCollection.AddSingleton<IObjectBus<TObjectType>, TObjectBus>();

            return serviceCollection;
        }
    }
}
