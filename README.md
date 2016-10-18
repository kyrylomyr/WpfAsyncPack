# WPF Async Pack
An async implementation of delegate command and base view model for WPF.

[![NuGet version](https://badge.fury.io/nu/WpfAsyncPack.svg)](https://badge.fury.io/nu/WpfAsyncPack)
[![Build status](https://ci.appveyor.com/api/projects/status/03gk0y53ccsa4aqq?svg=true)](https://ci.appveyor.com/project/kirmir/wpfasyncpack)

The idea of ```AsyncCommand``` implementation is based on the [articles](https://msdn.microsoft.com/en-us/magazine/dn630647.aspx) of Stephen Cleary's [blog](http://blog.stephencleary.com/) about async MVVM patterns.

# Installation

Th package is available through the NuGet by running the command

```
PM> Install-Package WpfAsyncPack
```

# Usage

## Base view model

Class ```BaseViewModel``` contains the features for implementing view models with the asynchronous handling of ```INotifyPropertyChanged``` event. Every time you set property using the ```BaseViewModel.SetProperty``` method, the code of subscribed handlers will be executed asynchronously in the UI thread through the current application's ```Dispatcher```.

An example of the view model implementation:

```csharp
internal class MainViewModel : BaseViewModel
{
    private string _message;

    public string Message
    {
        get { return _message; }
        set { SetProperty(ref _message, value); }
    }
}
```

The method ```SetProperty()``` automatically determines during the compilation time the name of property to notify as changed, but you can pass the name explicitly as a third parameter:

```csharp
SetProperty(ref _message, value, nameof(AnotherProperty));
```

It is possible also to react on the value change. If the new ```value``` passed to the property setter is different from the current one, the ```SetProperty()``` method returns ```True```, otherwise - ```False```:

```csharp
public string Message
{
    get { return _message; }
    set
    {
        if (SetProperty(ref _message, value))
        {
            // The new value was set for the property.
        }
    }
}
```

The method ```RaisePropertyChangedAsync()``` can be used to notify explicitly about the changes in the certain property:

```csharp
RaisePropertyChangedAsync(nameof(ChangedProperty));
RaisePropertyChangedAsync(); // Automatically takes the name of method or property that calls it.
```

It can be overridden and extended if needed.

If view model contains the logic of updating UI that is called outside (e.g. method passed to the service as action), the code should be wrapped with the call of method ```InvokeInUiThreadAsync()``` to make sure it runs in the UI thread:

```csharp
await InvokeInUiThreadAsync(() => { /* The code that updates UI */ });
```

## Async delegate command

...
