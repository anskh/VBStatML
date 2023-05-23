Imports System.Windows

Namespace Helpers
    Public Module ServiceProviderPropertyExtension

        Public ReadOnly ServiceProviderProperty As DependencyProperty = DependencyProperty.RegisterAttached("ServiceProvider", GetType(IServiceProvider), GetType(ServiceProviderPropertyExtension), New PropertyMetadata(Nothing))
        Public Function GetServiceProvider(obj As DependencyObject) As IServiceProvider
            Return CType(obj.GetValue(ServiceProviderProperty), IServiceProvider)
        End Function
        Public Sub SetServiceProvider(obj As DependencyObject, value As IServiceProvider)
            obj.SetValue(ServiceProviderProperty, value)
        End Sub
    End Module
End Namespace