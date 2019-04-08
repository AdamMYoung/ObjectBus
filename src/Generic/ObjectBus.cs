﻿using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ObjectBus.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectBus
{
    public class ObjectBus<T> : IObjectBus<T>
    {
        /// <summary>
        /// All currently instantantiated clients.
        /// </summary>
        private static IDictionary<string, IQueueClient> Clients { get; set; } = new Dictionary<string, IQueueClient>();

        /// <summary>
        /// Client to handle message queue calls.
        /// </summary>
        private IQueueClient Client { get; }

        /// <summary>
        /// Configuration options for the message bus.
        /// </summary>
        private ObjectBusOptions Options { get; }

        /// <summary>
        /// Event called when a message has been recieved.
        /// </summary>
        public event EventHandler<MessageEventArgs<T>> MessageRecieved;

        /// <summary>
        /// Instantaites a new MessageBus from the provided options.
        /// </summary>
        /// <param name="options">Options for connection configuration.</param>
        public ObjectBus(IOptions<ObjectBusOptions> options)
        {
            Options = options.Value;

            if (Options.ConnectionString == null || Options.QueueName == null)
                throw new NullReferenceException("ConnectionString and QueueName must be provided.");

            var key = Options.ConnectionString + Options.QueueName;
            if (!Clients.TryGetValue(key, out IQueueClient value))
            {
                value = new QueueClient(options.Value.ConnectionString, options.Value.QueueName);
                Clients.Add(key, value);
            }

            Client = value;

            if (Options.ClientType != BusType.Sender)
            {
                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                Client.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            }
        }

        /// <summary>
        /// Sends the provided object to registered recipients.
        /// </summary>
        /// <param name="message">Object to send.</param>
        public async Task SendAsync(T message)
        {
            if (Options.ClientType == BusType.Reciever)
                throw new InvalidOperationException("ObjectBus is set to an invalid BusType");

            var messageObj = JsonConvert.SerializeObject(message);
            var messageData = new Message(Encoding.UTF8.GetBytes(messageObj));

            await Client.SendAsync(messageData);
        }

        /// <summary>
        /// Processes the incoming message into the generic type provided.
        /// </summary>
        /// <param name="message">Incoming message.</param>
        /// <param name="token">Async cancellation token.</param>
        /// <returns></returns>
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var result = Encoding.UTF8.GetString(message.Body);
            try
            {
                var resultObj = JsonConvert.DeserializeObject<T>(result);

                if (token != null && resultObj != null)
                {
                    if (MessageRecieved.GetInvocationList().Length > 0)
                    {
                        MessageRecieved(this, new MessageEventArgs<T> { Object = resultObj });
                        await Client.CompleteAsync(message.SystemProperties.LockToken);
                    }
                }
            }
            catch (JsonSerializationException)
            {
                //Do nothing, as other azure busses may handle it.
            }

        }

        /// <summary>
        /// Handles the exceptions generated by the message bus.
        /// </summary>
        /// <param name="exceptionReceivedEventArgs">Exception being raised.</param>
        /// <returns></returns>
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}