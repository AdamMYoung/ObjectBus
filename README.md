# ObjectBus

ObjectBus is a library to make it quick and simple to serialize/serialize objects across applications via an Azure Service Bus.

[NuGet package](https://www.nuget.org/packages/ObjectBus)

## Usage

First create your ObjectBus in the ConfigureServices() method, by specifying the connection information and bus type.

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

To receive objects, subscribe to the `MessageRecieved` event of the ObjectBus. 

```csharp
public MyClass(IObjectBus<YourMessageObject> objectBus)
{
	NewObjectBus = objectBus;
	NewObjectBus.MessageRecieved += onMessageRecieved;
}
```

The received object is within the `MessageEventArgs` argument.

```csharp
private void onMessageRecieved(object sender, MessageEventArgs<RecordingChunk> e)
{
	var myObject = e.Object;
	//Do something with object.
}
```

## Overrides

If the current implementation doesn't fit your use case, you can create a subclass of ObjectBus, and override the `SendAsync()` or `HandleMessageAsync()` methods like so:

```csharp
public class SubclassObjectBus : ObjectBus<YourMessageObject> 
{
	public SubclassObjectBus(IOptions<YourMessageObject> options) : base(options) {}
	
	//Handle incoming messages.
	override Task HandleMessageAsync(YourMessageObject message) {}
	
	//Handle outgoing messages.
	override Task SendAsync(YourMessageObject message){}
}
```

This can then be injected in the startup method similar to before, with the subclass specified.
```csharp
services.CreateObjectBus<YourMessageObject, SubclassObjectBus>(p =>
	p.Configure("ConnectionString", "QueueName", BusTypes.Sender));	
```
