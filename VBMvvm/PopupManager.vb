Imports VBMvvm.Abstractions
Imports VBMvvm.ViewModels
Imports Microsoft.Extensions.DependencyInjection
Imports System.Windows

Public Class PopupManager
    Implements IPopupManager

    Private ReadOnly _ServiceProvider As IServiceProvider
    Private ReadOnly _PopupMessageViewModel As PopupMessageViewModel

    Public Sub New(serviceProvider As IServiceProvider, popupMessageViewModel As PopupMessageViewModel)
        _ServiceProvider = serviceProvider
        _PopupMessageViewModel = popupMessageViewModel
    End Sub


    Public Function ShowPopupMessage(message As String,
                                     Optional caption As String = "",
                                     Optional owner As IHasPopupContent = Nothing,
                                     Optional button As MessageBoxButton = MessageBoxButton.OK,
                                     Optional icon As MessageBoxImage = MessageBoxImage.None) As MessageBoxResult _
                                     Implements IPopupManager.ShowPopupMessage
        If owner Is Nothing Then
            owner = _ServiceProvider.GetServices(Of IHasPopupContent)().FirstOrDefault()
        End If
        If owner IsNot Nothing Then
            _PopupMessageViewModel.Message = message
            _PopupMessageViewModel.Caption = caption
            _PopupMessageViewModel.Button = button
            _PopupMessageViewModel.Image = icon
            Dim popup = owner.ShowPopup(_PopupMessageViewModel)
            If popup IsNot Nothing Then
                Return popup.PopupResult
            End If
        End If
        Return MessageBoxResult.None
    End Function

    Public Function ShowPopup(Of TPopupViewModel As PopupViewModel)(popupViewModel As TPopupViewModel,
                                                                    Optional owner As IHasPopupContent = Nothing) As TPopupViewModel _
                                                                    Implements IPopupManager.ShowPopup
        If owner Is Nothing Then
            owner = _ServiceProvider.GetServices(GetType(IHasPopupContent)).FirstOrDefault()
        End If
        If owner IsNot Nothing Then
            Return owner.ShowPopup(popupViewModel)
        End If

        Return Nothing
    End Function
End Class
