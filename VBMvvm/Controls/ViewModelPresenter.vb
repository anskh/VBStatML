Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports Microsoft.Extensions.DependencyInjection
Imports VBMvvm.Helpers

Namespace Controls
    Public Class ViewModelPresenter
        Inherits ContentControl

        Public Shared ReadOnly ViewModelProperty As DependencyProperty = DependencyProperty.Register("ViewModel", GetType(Object), GetType(ViewModelPresenter), New PropertyMetadata(Nothing, AddressOf OnViewModelChanged))
        Public Property ViewModel As Object
            Get
                Return GetValue(ViewModelProperty)
            End Get
            Set(value As Object)
                SetValue(ViewModelProperty, value)
            End Set
        End Property
        Private Shared Sub OnViewModelChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            If DesignerProperties.GetIsInDesignMode(d) Then
                Return
            End If
            Dim self = CType(d, ViewModelPresenter)
            self.Content = Nothing
            If e.NewValue IsNot Nothing Then
                Dim serviceProvider = GetServiceProvider(d)
                Dim view = serviceProvider.GetRequiredService(Of ViewLocator)().GetViewForViewModel(e.NewValue)
                self.Content = view
            End If
        End Sub
        Private Shared Function GetServiceProvider(d As DependencyObject) As IServiceProvider
            Dim runner = d
            Do
                Dim serviceProvider = ServiceProviderPropertyExtension.GetServiceProvider(runner)
                If serviceProvider IsNot Nothing Then
                    Return serviceProvider
                End If
                runner = runner.GetParentObject()
            Loop While runner IsNot Nothing
            Throw New Exception("Could not locate IServiceProvider in visual tree.")
        End Function
    End Class
End Namespace