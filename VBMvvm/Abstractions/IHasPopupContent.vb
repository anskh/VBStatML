Imports VBMvvm.ViewModels

Namespace Abstractions
    Public Interface IHasPopupContent
        Function ShowPopup(popupViewModel As PopupViewModel) As PopupViewModel
        Sub HidePopup(sender As Object, e As EventArgs)
        Property CurrentPopup As PopupViewModel
        Property IsPopupVisible As Boolean
    End Interface
End Namespace
