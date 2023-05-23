Imports System.Windows
Imports System.Windows.Input
Imports VBMvvm.Commands

Namespace ViewModels
    Public Class PopupViewModel
        Inherits ValidationPropertyChanged
        Public Event CloseHandler As EventHandler
        Public Sub New()
            CloseCommand = New Command(AddressOf OnClose)
            YesCommand = New Command(AddressOf OnYes, AddressOf CanYes)
            NoCommand = New Command(AddressOf OnNo)
            OKCommand = New Command(AddressOf OnOK, AddressOf CanOK)
            CancelCommand = New Command(AddressOf OnCancel)
            PopupResult = MessageBoxResult.None
        End Sub

        Protected Overridable Function CanYes(obj As Object) As Boolean
            Return True
        End Function
        Protected Overridable Function CanOK(obj As Object) As Boolean
            Return True
        End Function

        Protected Overridable Sub OnYes(obj As Object)
            PopupResult = MessageBoxResult.Yes
            OnClose(obj)
        End Sub
        Protected Overridable Sub OnOK(obj As Object)
            PopupResult = MessageBoxResult.OK
            OnClose(obj)
        End Sub
        Protected Overridable Sub OnNo(obj As Object)
            PopupResult = MessageBoxResult.No
            OnClose(obj)
        End Sub
        Protected Overridable Sub OnCancel(obj As Object)
            PopupResult = MessageBoxResult.Cancel
            OnClose(obj)
        End Sub

        Protected Sub OnClose(obj As Object)
            RaiseEvent CloseHandler(Me, EventArgs.Empty)
        End Sub
        Public Property PopupResult As MessageBoxResult
            Get
                Return GetProperty(Of MessageBoxResult)(NameOf(PopupResult))
            End Get
            Protected Set(value As MessageBoxResult)
                SetProperty(value, NameOf(PopupResult))
            End Set
        End Property
        Public Property Caption As String
            Get
                Return GetProperty(Of String)(NameOf(Caption))
            End Get
            Set(value As String)
                SetProperty(value, NameOf(Caption))
            End Set
        End Property

        Public ReadOnly Property CloseCommand As ICommand
        Public ReadOnly Property YesCommand As ICommand
        Public ReadOnly Property OKCommand As ICommand
        Public ReadOnly Property NoCommand As ICommand
        Public ReadOnly Property CancelCommand As ICommand
    End Class
End Namespace