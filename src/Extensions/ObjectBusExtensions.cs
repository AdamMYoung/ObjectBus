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
        public static IServiceCollection CreateObjectBus<T>(this IServiceCollection serviceCollection, Action<ObjectBusOptions> optionsAction)
        {
            if (optionsAction == null) throw new NotImplementedException("ConnectionString and QueueName must be provided");

            serviceCollection.Configure(optionsAction);
            serviceCollection.AddSingleton<IObjectBus<T>, ObjectBus<T>>();

            return serviceCollection;
        }
    }
}
