# WpfAsyncPack
An async implementation of delegate command and other base common classes for WPF.

[![NuGet version](https://badge.fury.io/nu/WpfAsyncPack.svg)](https://badge.fury.io/nu/WpfAsyncPack)
[![Build status](https://ci.appveyor.com/api/projects/status/03gk0y53ccsa4aqq?svg=true)](https://ci.appveyor.com/project/kirmir/wpfasyncpack)

## Base view model

Class ```BaseViewModel``` contains the features for implementing view models with the asynchronous handling of ```INotifyPropertyChanged``` event. Every time you set property using the ```BaseViewModel.SetProperty``` method, the code of subscribed handlers will be executed asynchronously in the UI thread through the current application's ```Dispatcher```.

### An example of implementing view model

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

## Async delegate command

...
