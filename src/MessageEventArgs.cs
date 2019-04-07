using System;

namespace ObjectBus
{
    public class MessageEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Object recieved from the service bus.
        /// </summary>
        public T Object { get; internal set; }
    }
}
