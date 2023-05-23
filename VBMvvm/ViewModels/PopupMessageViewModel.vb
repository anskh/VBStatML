Imports System.Windows

Namespace ViewModels
    Public Class PopupMessageViewModel
        Inherits PopupViewModel

        Public Property Message As String
            Get
                Return GetProperty(Of String)(NameOf(Message))
            End Get
            Set(value As String)
                SetProperty(value, NameOf(Message))
            End Set
        End Property
        Public Property Image As MessageBoxImage
            Get
                Return GetProperty(Of MessageBoxImage)(NameOf(Image))
            End Get
            Set(value As MessageBoxImage)
                SetProperty(value, NameOf(Image))
            End Set
        End Property
        Public Property Button As MessageBoxButton
            Get
                Return GetProperty(Of MessageBoxButton)(NameOf(Button))
            End Get
            Set(value As MessageBoxButton)
                SetProperty(value, NameOf(Button))
            End Set
        End Property
    End Class
End Namespace