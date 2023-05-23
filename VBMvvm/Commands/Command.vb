Imports System.Windows.Input

Namespace Commands
    Public Class Command
        Implements ICommand

        Private _Execute As Action(Of Object)
        Private _CanExecute As Predicate(Of Object)
        Private Event _CanExecuteChangedInternal As EventHandler

        Public Sub New(ByVal execute As Action(Of Object))
            Me.New(execute, AddressOf _DefaultCanExecute)
        End Sub

        Public Sub New(ByVal execute As Action(Of Object), ByVal canExecute As Predicate(Of Object))
            If execute Is Nothing Then
                Throw New ArgumentNullException(NameOf(execute))
            End If
            If canExecute Is Nothing Then
                Throw New ArgumentNullException(NameOf(canExecute))
            End If
            _Execute = execute
            _CanExecute = canExecute
        End Sub

        Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
            AddHandler(ByVal value As EventHandler)
                AddHandler _CanExecuteChangedInternal, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler _CanExecuteChangedInternal, value
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                RaiseEvent _CanExecuteChangedInternal(sender, e)
            End RaiseEvent
        End Event

        Public Function CanExecute(ByVal param As Object) As Boolean Implements ICommand.CanExecute
            Return _CanExecute IsNot Nothing AndAlso _CanExecute.Invoke(param)
        End Function

        Public Sub Execute(ByVal param As Object) Implements ICommand.Execute
            If CanExecute(param) Then
                _Execute(param)
            End If
        End Sub

        Public Sub OnCanExecuteChanged()
            'referencing the hidden "Event" field of the CanExecuteChangedInternal event:
            Dim handler As EventHandler = _CanExecuteChangedInternalEvent
            If handler IsNot Nothing Then
                handler.Invoke(Me, EventArgs.Empty)
            End If
        End Sub

        Private Shared Function _DefaultCanExecute(ByVal param As Object) As Boolean
            Return True
        End Function
    End Class

    Public Class Command(Of T)
        Implements ICommand

        Private _Execute As Action(Of T)
        Private _CanExecute As Predicate(Of T)
        Private Event _CanExecuteChangedInternal As EventHandler

        Public Sub New(ByVal execute As Action(Of T))
            Me.New(execute, AddressOf _DefaultCanExecute)
        End Sub

        Public Sub New(ByVal execute As Action(Of T), ByVal canExecute As Predicate(Of T))
            If execute Is Nothing Then
                Throw New ArgumentNullException(NameOf(execute))
            End If
            If canExecute Is Nothing Then
                Throw New ArgumentNullException(NameOf(canExecute))
            End If
            _Execute = execute
            _CanExecute = canExecute
        End Sub

        Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
            AddHandler(ByVal value As EventHandler)
                AddHandler _CanExecuteChangedInternal, value
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler _CanExecuteChangedInternal, value
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                RaiseEvent _CanExecuteChangedInternal(sender, e)
            End RaiseEvent
        End Event

        Public Function CanExecute(ByVal param As Object) As Boolean Implements ICommand.CanExecute
            Return _CanExecute IsNot Nothing AndAlso _CanExecute.Invoke(CType(param, T))
        End Function

        Public Sub Execute(ByVal param As Object) Implements ICommand.Execute
            If CanExecute(param) Then
                _Execute(CType(param, T))
            End If
        End Sub

        Public Sub OnCanExecuteChanged()
            'referencing the hidden "Event" field of the CanExecuteChangedInternal event:
            Dim handler As EventHandler = _CanExecuteChangedInternalEvent
            If handler IsNot Nothing Then
                handler.Invoke(Me, EventArgs.Empty)
            End If
        End Sub

        Public Sub Destroy()
            _CanExecute = Function(underscore) False
            Me._Execute = Sub(underscore)
                              Return
                          End Sub
        End Sub

        Private Shared Function _DefaultCanExecute(ByVal param As Object) As Boolean
            Return True
        End Function
    End Class
End Namespace