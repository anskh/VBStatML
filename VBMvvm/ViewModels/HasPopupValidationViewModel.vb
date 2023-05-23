Imports System.Threading
Imports System.Windows.Threading
Imports VBMvvm.Abstractions

Namespace ViewModels
    Public Class HasPopupValidationViewModel
        Inherits ValidationPropertyChanged
        Implements IHasPopupContent

        Public Property CurrentPopup As PopupViewModel Implements IHasPopupContent.CurrentPopup
            Get
                Return GetProperty(Of PopupViewModel)(NameOf(CurrentPopup))
            End Get
            Set(value As PopupViewModel)
                SetProperty(value, NameOf(CurrentPopup))
            End Set
        End Property

        Public Property IsPopupVisible As Boolean Implements IHasPopupContent.IsPopupVisible
            Get
                Return GetProperty(Of Boolean)(NameOf(IsPopupVisible))
            End Get
            Set(value As Boolean)
                SetProperty(value, NameOf(IsPopupVisible))
            End Set
        End Property

        Public Sub HidePopup(sender As Object, e As EventArgs) Implements IHasPopupContent.HidePopup
            IsPopupVisible = False
            If CurrentPopup IsNot Nothing Then
                RemoveHandler CurrentPopup.CloseHandler, AddressOf HidePopup
                CurrentPopup = Nothing
            End If
        End Sub

        Public Function ShowPopup(popupViewModel As PopupViewModel) As PopupViewModel Implements IHasPopupContent.ShowPopup
            AddHandler popupViewModel.CloseHandler, AddressOf HidePopup
            CurrentPopup = popupViewModel
            IsPopupVisible = True
            While IsPopupVisible
                With Dispatcher.CurrentDispatcher
                    If .HasShutdownFinished Or .HasShutdownStarted Then
                        Exit While
                    End If
                    .Invoke(DispatcherPriority.Background, New ThreadStart(Sub() Thread.Sleep(20)))
                End With
            End While
            Return popupViewModel
        End Function
    End Class
End Namespace