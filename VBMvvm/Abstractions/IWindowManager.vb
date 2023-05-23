Imports System.Windows
Imports Microsoft.Extensions.DependencyInjection

Namespace Abstractions
    Public Interface IWindowManager
        Function ShowWindow(Of TViewModel)(Optional owningWindow As Window = Nothing,
                                           Optional scope As IServiceScope = Nothing) As Window
        Function ShowWindow(viewModel As Object, Optional owningWindow As Window = Nothing,
                            Optional scope As IServiceScope = Nothing) As Window
        Function ShowDialog(Of TViewModel)(Optional owningWindow As Window = Nothing,
                                           Optional scope As IServiceScope = Nothing) As (Boolean?, TViewModel)
        Function ShowMessageBox(message As String, Optional caption As String = "", Optional button As MessageBoxButton = MessageBoxButton.OK, Optional icon As MessageBoxImage = MessageBoxImage.None) As MessageBoxResult
        Sub ShutdownApplication(Optional exitCode As Integer = 0)
    End Interface
End Namespace