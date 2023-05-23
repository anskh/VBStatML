Imports System.Windows.Input

Namespace Commands
    Public Class AsyncCommand
        Implements ICommand

        Private _Execute As Func(Of Object, Task)
        Private _CanExecute As Predicate(Of Object)
        Private Event _CanExecuteChangedInternal As EventHandler
        Private _Task As Task

        Public Sub New(ByVal execute As Func(Of Object, Task))
            Me.New(execute, AddressOf _DefaultCanExecute)
        End Sub

        Public Sub New(ByVal execute As Func(Of Object, Task), ByVal canExecute As Predicate(Of Object))
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
                If _CanExecute IsNot Nothing Then
                    AddHandler _CanExecuteChangedInternal, value
                End If
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                If _CanExecute IsNot Nothing Then
                    RemoveHandler _CanExecuteChangedInternal, value
                End If
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
                RaiseEvent _CanExecuteChangedInternal(sender, e)
            End RaiseEvent
        End Event

        Public Function CanExecute(ByVal param As Object) As Boolean Implements ICommand.CanExecute
            Return (_CanExecute IsNot Nothing AndAlso _CanExecute(param)) And (_Task Is Nothing Or _Task.IsCompleted)
        End Function

        Public Async Sub Execute(ByVal param As Object) Implements ICommand.Execute
            If CanExecute(param) Then
                _Task = _Execute(param)
                OnCanExecuteChanged()
                Await _Task
                OnCanExecuteChanged()
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
            Me._Execute = Function(underscore) As Task
                              Return _Task
                          End Function
        End Sub

        Private Shared Function _DefaultCanExecute(ByVal param As Object) As Boolean
            Return True
        End Function
    End Class

    Public Class AsyncCommand(Of T)
        Implements ICommand

        Private _Execute As Func(Of T, Task)
        Private _CanExecute As Predicate(Of T)
        Private Event _CanExecuteChangedInternal As EventHandler
        Private _Task As Task

        Public Sub New(ByVal execute As Func(Of T, Task))
            Me.New(execute, AddressOf _DefaultCanExecute)
        End Sub

        Public Sub New(ByVal execute As Func(Of T, Task), ByVal canExecute As Predicate(Of T))
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
            Return (_CanExecute IsNot Nothing AndAlso _CanExecute(CType(param, T))) And (_Task Is Nothing Or _Task.IsCompleted)
        End Function

        Public Async Sub Execute(ByVal param As Object) Implements ICommand.Execute
            If CanExecute(param) Then
                _Task = _Execute(CType(param, T))
                OnCanExecuteChanged()
                Await _Task
                OnCanExecuteChanged()
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
            Me._Execute = Function(underscore) As Task
                              Return _Task
                          End Function
        End Sub

        Private Shared Function _DefaultCanExecute(ByVal param As Object) As Boolean
            Return True
        End Function
    End Class
End Namespace