using System;
using System.Threading.Tasks;

namespace ObjectBus
{
    public interface IObjectBus<TObjectType>
    {
        /// <summary>
        /// Event called when a new message has been recieved.
        /// </summary>
        event EventHandler<MessageEventArgs<TObjectType>> MessageRecieved;

        /// <summary>
        /// Sends the provided object to registered recipients.
        /// </summary>
        /// <param name="message">Object to send.</param>
        Task SendAsync(TObjectType message);
    }
}
