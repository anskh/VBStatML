Imports Microsoft.Extensions.DependencyInjection
Imports VBMvvm
Imports VBMvvm.Abstractions
Imports VBMvvm.Commands
Imports VBMvvm.ViewModels

Namespace ViewModels
    Public Class MainWindowViewModel
        Inherits HasPopupViewModel
        Implements IOnLoadedHandler, ICancelableOnClosingHandler
        Protected ReadOnly _ServiceProvider As IServiceProvider
        Protected ReadOnly _WindowManager As IWindowManager
        Protected ReadOnly _PopupManager As IPopupManager
        Protected ReadOnly _ViewLocator As ViewLocator
        Public Sub New(serviceProvider As IServiceProvider, windowManager As IWindowManager, popupManager As IPopupManager, viewLocator As ViewLocator)
            _ServiceProvider = serviceProvider
            _WindowManager = windowManager
            _PopupManager = popupManager
            _ViewLocator = viewLocator
        End Sub
        Public Function OnCLosing() As Boolean Implements ICancelableOnClosingHandler.OnCLosing

            Return False
        End Function

        Public Function OnLoadedAsync() As Task Implements IOnLoadedHandler.OnLoadedAsync

            Return Task.CompletedTask
        End Function
        Public ReadOnly Property TesPopupCommand As ICommand = New Command(AddressOf OnTesPopup)

        Private Sub OnTesPopup(obj As Object)
            _PopupManager.ShowPopupMessage("coba", "tes", Me)
        End Sub
    End Class
End Namespace