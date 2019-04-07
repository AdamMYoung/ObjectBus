# ObjectBus

ObjectBus is a library to make it quick and simple to serialize/deserialize objects to transmit via an Azure Serial Bus.

[NuGet package](https://www.nuget.org/packages/ObjectBus/1.0.0)

## Usage

First create your ObjectBus in the ConfigureServices() method, like so:

```csharp
services.CreateObjectBus<YourMessageObject>(p =>
	p.Configure("ConnectionString", "QueueName"));
```
Then, access the ObjectBus via dependency injection in the constructor:

```csharp
private IObjectBus<YourMessageObject> NewObjectBus { get; }

public MyClass(IObjectBus<YourMessageObject> objectBus)
{
	NewObjectBus = objectBus;
}
```

## Send

To send your objects, simply call the `SendAsync(T object)` method with the object you wish to send.

## Recieve

To recieve objects, subscribe to the `MessageRecieved` event of the ObjectBus. The recieved object is within the `MessageEventArgs` argument.
