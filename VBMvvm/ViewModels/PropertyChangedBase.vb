Imports System.ComponentModel
Imports System.Linq.Expressions
Imports System.Runtime.CompilerServices
Imports VBMvvm.Helpers

Namespace ViewModels
    Public MustInherit Class PropertyChangedBase
        Implements INotifyPropertyChanged

        Private ReadOnly _Properties As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub Refresh()
            OnPropertyChanged(String.Empty)
        End Sub
        Protected Overridable Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
        Protected Sub OnPropertyChanged(Of TProperty)([property] As Expression(Of Func(Of TProperty)))
            OnPropertyChanged(GetMemberName([property]))
        End Sub
        Protected Overridable Function SetProperty(Of TProperty)(value As TProperty, <CallerMemberName> propertyName As String) As Boolean
            If EqualityComparer(Of TProperty).Default.Equals(GetProperty(Of TProperty)(propertyName), value) Then
                Return False
            End If
            _Properties(propertyName) = value
            OnPropertyChanged(propertyName)
            Return True
        End Function
        Protected Overridable Function SetProperty(Of TProperty)(value As TProperty, [property] As Expression(Of Func(Of TProperty))) As Boolean
            Return SetProperty(value, GetMemberName([property]))
        End Function
        Protected Overridable Function GetProperty(Of TProperty)(<CallerMemberName> propertyName As String) As TProperty
            Dim _Value As Object = Nothing
            If _Properties.TryGetValue(propertyName, _Value) Then
                If _Value IsNot Nothing Then
                    Return CType(_Value, TProperty)
                End If
            End If
            Return Nothing
        End Function
        Protected Overridable Function GetProperty(Of TProperty)([property] As Expression(Of Func(Of TProperty))) As TProperty
            Return GetProperty(Of TProperty)(GetMemberName([property]))
        End Function
    End Class
End Namespace