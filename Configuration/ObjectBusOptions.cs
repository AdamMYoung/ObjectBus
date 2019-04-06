namespace ObjectBus.Options
{
    public class ObjectBusOptions
    {
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
        /// <returns></returns>
        public ObjectBusOptions Configure(string connectionString, string queueName)
        {
            ConnectionString = connectionString;
            QueueName = queueName;
            return this;
        }
    }
}
