Imports System.Runtime.CompilerServices
Imports System.Windows
Imports Microsoft.Extensions.DependencyInjection
Imports VBMvvm.Abstractions
Imports VBMvvm.Controls
Imports VBMvvm.ViewModels

Namespace Helpers
    Public Module ServiceCollectionExtension
        <Extension()> Public Function AddMvvm(services As IServiceCollection) As IServiceCollection
            services.AddSingleton(Of ViewLocator)
            services.AddSingleton(Of IWindowManager, WindowManager)
            services.AddSingleton(Of IPopupManager, PopupManager)
            services.AddSingleton(Of IUiExecution, UiExecution)
            services.AddMvvmSingleton(Of PopupMessageViewModel, PopupMessageView)
            Return services
        End Function
        <Extension()> Public Function AddMvvmTransient(Of TViewModel As Class, TView As FrameworkElement)(services As IServiceCollection) As IServiceCollection

            services.AddTransient(Of TViewModel)
            services.AddTransient(Of TView)

            services.GetViewModelRegistry().ViewModelTypeToViewType.Add(GetType(TViewModel), GetType(TView))

            Return services
        End Function
        <Extension()> Public Function AddMvvmSingleton(Of TViewModel As Class, TView As FrameworkElement)(services As IServiceCollection) As IServiceCollection

            services.AddSingleton(Of TViewModel)
            services.AddTransient(Of TView)

            services.GetViewModelRegistry().ViewModelTypeToViewType.Add(GetType(TViewModel), GetType(TView))

            Return services
        End Function
        <Extension()> Private Function GetViewModelRegistry(services As IServiceCollection) As ViewModelRegistry
            Dim registry As ViewModelRegistry
            Dim descriptor = services.FirstOrDefault(Function(x) x.ServiceType = GetType(ViewModelRegistry))
            If descriptor IsNot Nothing Then
                registry = CType(descriptor.ImplementationInstance, ViewModelRegistry)
            Else
                registry = New ViewModelRegistry
                services.AddSingleton(registry)
            End If
            Return registry
        End Function
    End Module
End Namespace