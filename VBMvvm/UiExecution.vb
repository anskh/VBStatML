Imports System.Windows
Imports System.Windows.Threading
Imports VBMvvm.Abstractions

Public Class UiExecution
    Implements IUiExecution

    Public Sub Execute(action As Action, Optional priority As DispatcherPriority = DispatcherPriority.DataBind) Implements IUiExecution.Execute
        Dispatcher.Invoke(action, priority)
    End Sub

    Public Function ExecuteAsync(action As Action, Optional priority As DispatcherPriority = DispatcherPriority.DataBind) As Task Implements IUiExecution.ExecuteAsync
        Return Dispatcher.InvokeAsync(action, priority).Task
    End Function
    Private ReadOnly Property Dispatcher As Dispatcher
        Get
            Dim currentDispatcher = Application.Current.Dispatcher
            If currentDispatcher Is Nothing Then
                currentDispatcher = Dispatcher.CurrentDispatcher
            End If
            Return currentDispatcher
        End Get
    End Property
End Class
