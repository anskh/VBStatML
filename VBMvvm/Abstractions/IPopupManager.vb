Imports System.Windows
Imports VBMvvm.ViewModels

Namespace Abstractions
    Public Interface IPopupManager
        Function ShowPopup(Of TPopupViewModel As PopupViewModel)(popupViewModel As TPopupViewModel,
                                                                 owner As IHasPopupContent) As TPopupViewModel
        Function ShowPopupMessage(message As String,
                                  caption As String,
                                  owner As IHasPopupContent,
                                  Optional button As MessageBoxButton = MessageBoxButton.OK,
                                  Optional icon As MessageBoxImage = MessageBoxImage.None) As MessageBoxResult
    End Interface
End Namespace