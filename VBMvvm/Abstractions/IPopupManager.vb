Imports System.Windows
Imports VBMvvm.ViewModels

Namespace Abstractions
    Public Interface IPopupManager
        Function ShowPopup(Of TPopupViewModel As PopupViewModel)(popupViewModel As TPopupViewModel,
                                                                 Optional owner As IHasPopupContent = Nothing) As TPopupViewModel
        Function ShowPopupMessage(message As String,
                                  Optional caption As String = "",
                                  Optional owner As IHasPopupContent = Nothing,
                                  Optional button As MessageBoxButton = MessageBoxButton.OK,
                                  Optional icon As MessageBoxImage = MessageBoxImage.None) As MessageBoxResult
    End Interface
End Namespace