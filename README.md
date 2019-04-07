# ObjectBus

ObjectBus is a library to make it quick and simple to serialize/deserialize objects to transmit via an Azure Serial Bus.

[NuGet package](https://www.nuget.org/packages/ObjectBus/1.0.0)

## Usage

First create your ObjectBus in the ConfigureServices() method, like so:

```csharp
services.CreateObjectBus<YourMessageObject>(p =>
	p.Configure("ConnectionString", "QueueName"));
```
By default, ObjectBus instances are configured to be both senders and recievers. To adjust this, pass a `BusTypes` enum into the configure method.

```csharp
services.CreateObjectBus<YourMessageObject>(p =>
	p.Configure("ConnectionString", "QueueName", BusTypes.Sender));
	
//OR

services.CreateObjectBus<YourMessageObject>(p =>
	p.Configure("ConnectionString", "QueueName", BusTypes.Reciever));
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

To send your objects, simply call the `SendAsync()` method with the object you wish to send, like so:

```csharp
public void SendObject() 
{
	var newObject = new YourMessageObject();
	NewObjectBus.SendAsync(newObject);
}
```

## Recieve

To recieve objects, subscribe to the `MessageRecieved` event of the ObjectBus. 

```csharp
public MyClass(IObjectBus<YourMessageObject> objectBus)
{
	NewObjectBus = objectBus;
	NewObjectBus.MessageRecieved += onMessageRecieved;
}
```

The recieved object is within the `MessageEventArgs` argument.

```csharp
private void onMessageRecieved(object sender, MessageEventArgs<RecordingChunk> e)
{
	var myObject = e.Object;
	//Do something with object.
}
```
