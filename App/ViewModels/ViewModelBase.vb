Imports Microsoft.Extensions.DependencyInjection
Imports VBMvvm
Imports VBMvvm.Abstractions
Imports VBMvvm.ViewModels

Namespace ViewModels
    Public MustInherit Class ViewModelBase
        Inherits PropertyChangedBase
        Protected ReadOnly _ServiceProvider As IServiceProvider = Application.ServiceProvider
        Protected ReadOnly _WindowManager As IWindowManager = Application.ServiceProvider.GetRequiredService(Of IWindowManager)
        Protected ReadOnly _PopupManager As IPopupManager = Application.ServiceProvider.GetRequiredService(Of IPopupManager)
        Protected ReadOnly _ViewLocator As ViewLocator = Application.ServiceProvider.GetRequiredService(Of ViewLocator)
    End Class
End Namespace