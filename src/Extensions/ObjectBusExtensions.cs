using Microsoft.Extensions.DependencyInjection;
using ObjectBus.Options;
using System;

namespace ObjectBus.Extensions
{
    public static class ServiceBusExtensions
    {
        /// <summary>
        /// Creates an Azure Service Bus using the object provided as the transport mechanism. 
        /// The ObjectBus is injected via the IObjectBus<TObjectType>.
        /// </summary>
        /// <typeparam name="TObjectType">Object to send/recieve.</typeparam>
        /// <param name="serviceCollection">Collection of services to inject into.</param>
        /// <param name="optionsAction">Configuration options of the application.</param>
        /// <returns></returns>
        public static IServiceCollection CreateObjectBus<TObjectType>(this IServiceCollection serviceCollection, Action<ObjectBusOptions<TObjectType>> optionsAction)
        {
            return CreateObjectBus<TObjectType, ObjectBus<TObjectType>>(serviceCollection, optionsAction);
        }

        /// <summary>
        /// Creates an Azure Service Bus using the object provided as the transport mechanism. 
        /// The service bus is injected via the IObjectBus<T>, using the provided ObjectBus implementation
        /// defined as TObjectBus.
        /// </summary>
        /// <typeparam name="TObjectType">Object to send/recieve.</typeparam>
        /// <typeparam name="TObjectBus">ObjectBus implementation to inject.</typeparam>
        /// <param name="serviceCollection">Collection of services to inject into.</param>
        /// <param name="optionsAction">Configuration options of the application.</param>
        /// <returns></returns>
        public static IServiceCollection CreateObjectBus<TObjectType, TObjectBus>(this IServiceCollection serviceCollection, Action<ObjectBusOptions<TObjectType>> optionsAction)
            where TObjectBus : ObjectBus<TObjectType>
        {
            serviceCollection.Configure(optionsAction);
            serviceCollection.AddScoped<IObjectBus<TObjectType>, TObjectBus>();

            return serviceCollection;
        }
    }
}
