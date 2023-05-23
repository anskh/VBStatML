Imports System.Windows
Imports Microsoft.Extensions.DependencyInjection
Imports VBMvvm.Abstractions
Imports VBMvvm.Helpers

Public Class WindowManager
    Implements IWindowManager

    Private ReadOnly _ServiceProvider As IServiceProvider
    Private ReadOnly _ViewLocator As ViewLocator

    Public Sub New(serviceProvider As IServiceProvider, viewLocator As ViewLocator)
        _ServiceProvider = serviceProvider
        _ViewLocator = viewLocator
    End Sub
    Public Sub ShutdownApplication(Optional exitCode As Integer = 0) Implements IWindowManager.ShutdownApplication
        If Application.Current Is Nothing Then Throw New InvalidOperationException("There's no application to shut down.")
        Application.Current.Shutdown(exitCode)
    End Sub

    Public Function ShowWindow(Of TViewModel)(Optional owningWindow As Window = Nothing, Optional scope As IServiceScope = Nothing) As Window Implements IWindowManager.ShowWindow
        Dim serviceProvider = GetServiceProvider(owningWindow, scope)
        Dim window = CType(_ViewLocator.GetViewForViewModel(Of TViewModel)(serviceProvider), Window)
        window.Owner = owningWindow
        window.Show()
        Return window
    End Function

    Public Function ShowWindow(viewModel As Object, Optional owningWindow As Window = Nothing, Optional scope As IServiceScope = Nothing) As Window Implements IWindowManager.ShowWindow
        Dim serviceProvider = GetServiceProvider(owningWindow, scope)
        Dim window = CType(_ViewLocator.GetViewForViewModel(viewModel)(serviceProvider), Window)
        window.Owner = owningWindow
        window.Show()
        Return window
    End Function

    Public Function ShowDialog(Of TViewModel)(Optional owningWindow As Window = Nothing, Optional scope As IServiceScope = Nothing) As (Boolean?, TViewModel) Implements IWindowManager.ShowDialog
        Dim serviceProvider = GetServiceProvider(owningWindow, scope)
        Dim viewModel = serviceProvider.GetRequiredService(Of TViewModel)
        Dim window = CType(_ViewLocator.GetViewForViewModel(viewModel, serviceProvider), Window)
        window.Owner = owningWindow
        Dim result = window.ShowDialog()
        Return (result, viewModel)
    End Function

    Private Function GetServiceProvider(Optional owningWindow As Window = Nothing, Optional scope As IServiceScope = Nothing) As IServiceProvider
        Dim serviceProvider As IServiceProvider = Nothing
        If scope IsNot Nothing Then
            serviceProvider = scope.ServiceProvider
        Else
            If owningWindow IsNot Nothing Then
                Dim service = ServiceProviderPropertyExtension.GetServiceProvider(owningWindow)
                If service IsNot Nothing Then
                    serviceProvider = service
                Else
                    serviceProvider = _ServiceProvider
                End If
            End If
        End If
        Return serviceProvider
    End Function

    Public Function ShowMessageBox(message As String, Optional caption As String = "", Optional button As MessageBoxButton = MessageBoxButton.OK, Optional icon As MessageBoxImage = MessageBoxImage.None) As MessageBoxResult Implements IWindowManager.ShowMessageBox
        Dim title As String = Application.Current.MainWindow.Title
        If Not String.IsNullOrEmpty(caption) Then
            title = caption
        End If
        Return MessageBox.Show(message, title, button, icon)
    End Function
End Class
