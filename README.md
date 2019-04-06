ObjectBus

ObjectBus is a library to make it quick and simple to serialize/deserialize objects to transmit via an Azure Serial Bus.

# Usage

First add your serial bus in the ConfigureServices() method, like so:

services.CreateObjectBus<YourMessageObject>(p =>
p.Configure("ConnectionString", "QueueName"));

Then, access the object bus via dependency injection in the constructor:

private IObjectBus<YourMessageObject> NewObjectBus { get; }

public MyClass(IObjectBus<YourMessageObject> objectBus)
{
NewObjectBus = objectBus;
}
