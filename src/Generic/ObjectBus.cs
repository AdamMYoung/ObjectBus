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
    public class ObjectBus<TObjectType> : IObjectBus<TObjectType>
    {

        /// <summary>
        /// Client to handle message queue calls.
        /// </summary>
        private IQueueClient Client { get; }

        /// <summary>
        /// Configuration options for the message bus.
        /// </summary>
        private ObjectBusOptions<TObjectType> Options { get; }

        /// <summary>
        /// Event called when a message has been received.
        /// </summary>
        public event EventHandler<MessageEventArgs<TObjectType>> MessageRecieved;

        /// <summary>
        /// Instantiates a new MessageBus from the provided options.
        /// </summary>
        /// <param name="options">Options for connection configuration.</param>
        public ObjectBus(IOptions<ObjectBusOptions<TObjectType>> options)
        {
            Options = options.Value;

            if (Options.ConnectionString == null || Options.QueueName == null)
                throw new NullReferenceException("ConnectionString and QueueName must be provided.");

            Client = new QueueClient(Options.ConnectionString, Options.QueueName);
            if (Options.ClientType == BusType.Reciever)
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
        public virtual async Task SendAsync(TObjectType message)
        {
            if (Options.ClientType == BusType.Reciever)
                throw new InvalidOperationException("ObjectBus is set to an invalid BusType");

            var messageObj = JsonConvert.SerializeObject(message);
            var messageData = new Message(Encoding.UTF8.GetBytes(messageObj));

            await Client.SendAsync(messageData);
        }

        /// <summary>
        /// Handles the returned object.
        /// </summary>
        /// <param name="message">Object returned.</param>
        public async virtual Task HandleMessageAsync(TObjectType message)
        {
            if (MessageRecieved.GetInvocationList().Length > 0)
                await Task.Run(() => MessageRecieved(this, new MessageEventArgs<TObjectType> { Object = message }));
        }

        /// <summary>
        /// Processes the incoming message into the generic type provided.
        /// </summary>
        /// <param name="message">Incoming message.</param>
        /// <param name="token">Async cancellation token.</param>
        /// <returns></returns>
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var result = Encoding.UTF8.GetString(message.Body);
            var resultObj = JsonConvert.DeserializeObject<TObjectType>(result, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
            });

            await HandleMessageAsync(resultObj);
            await Client.CompleteAsync(message.SystemProperties.LockToken);
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
