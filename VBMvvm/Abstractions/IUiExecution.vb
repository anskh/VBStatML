Imports System.Windows.Threading

Namespace Abstractions
    Public Interface IUiExecution
        Sub Execute(action As Action, Optional priority As DispatcherPriority = DispatcherPriority.DataBind)
        Function ExecuteAsync(action As Action, Optional priority As DispatcherPriority = DispatcherPriority.DataBind) As Task
    End Interface
End Namespace