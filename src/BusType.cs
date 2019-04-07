using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectBus
{
    public enum BusType
    {
        /// <summary>
        /// ObjectBus will be a sender of data.
        /// </summary>
        Sender,

        /// <summary>
        /// ObjectBus will be a reciever of data.
        /// </summary>
        Reciever,

        /// <summary>
        /// ObjectBus both recieves and sends data.
        /// </summary>
        SendRecieve
    }
}
