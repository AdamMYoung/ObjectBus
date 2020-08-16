using ObjectBus;

namespace ObjectBus.Options
{
    public class ObjectBusOptions<T>
    {
        /// <summary>
        /// Type of client the ObjectBus represents.
        /// </summary>
        internal BusType ClientType { get; private set; }

        /// <summary>
        /// Connection string to the service bus.
        /// </summary>
        internal string ConnectionString { get; private set; }

        /// <summary>
        /// Name of the queue to connect to.
        /// </summary>
        internal string QueueName { get; private set; }

        /// <summary>
        /// Configures the service bus using the provided connection string and queue name parameters.
        /// </summary>
        /// <param name="connectionString">Connection string of the Azure service bus.</param>
        /// <param name="queueName">Name of the queue to connect to.</param>
        /// <param name="clientType">Type of client the bus represents.</param>
        /// <returns></returns>
        public ObjectBusOptions<T> Configure(string connectionString, string queueName, BusType clientType)
        {
            ConnectionString = connectionString;
            QueueName = queueName;
            ClientType = clientType;
            return this;
        }
    }
}
