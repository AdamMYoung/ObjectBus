using System;
using System.Threading.Tasks;

namespace ObjectBus
{
    public interface IObjectBus<T>
    {
        /// <summary>
        /// Event called when a new message has been recieved.
        /// </summary>
        event EventHandler<MessageEventArgs<T>> MessageRecieved;

        /// <summary>
        /// Sends the provided object to registered recipients.
        /// </summary>
        /// <param name="message">Object to send.</param>
        Task SendAsync(T message);


    }
}
