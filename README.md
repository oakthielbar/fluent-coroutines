# Fluent Coroutines
Unity3D Coroutine scheduler with a fluent interface.

## Installation
Add the contents of the Asset folder to your Unity3D project's Asset folder.

## Usage
Call `this.FluentCoroutine()` inside of any class that inherits from `MonoBehaviour` and chain a few calls to the available methods; once you've defined what you want the `FluentCoroutine` to do, call `Finalize()` to get the completed `FluentCoroutine`. When you want to run it, just call `Execute()` and it will run as a Unity3D Coroutine.

```csharp
public class ExampleFluentCoroutine : MonoBehaviour
{
	void Start()
    {
    	this.FluentCoroutine()
            .WaitForSeconds(1f)
            .Do(PrintMessage)
            .Finalize()
            .Execute();
    }
    
    void PrintMessage()
    {
    	Debug.Log("Ahoy hoy, world!");
    }
}
```