Imports App.ViewModels
Imports App.Views
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports VBMvvm
Imports VBMvvm.Abstractions
Imports VBMvvm.Helpers

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Private ReadOnly _Host As IHost
    Private Shared _ServiceProvider As IServiceProvider
    Public Sub New()
        Dim hostBuilder = Host.CreateDefaultBuilder() _
            .ConfigureAppConfiguration(Sub(context As HostBuilderContext, builder As IConfigurationBuilder)
                                           builder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                                           builder.AddJsonFile("AppSettings.json", False, True)
                                           builder.AddEnvironmentVariables()
                                       End Sub) _
             .ConfigureServices(AddressOf ConfigurationServices) _
             .ConfigureLogging(Sub(context As HostBuilderContext, builder As ILoggingBuilder)
                                   builder.AddConfiguration(context.Configuration.GetSection("Logging"))
                                   builder.AddDebug
                               End Sub)
        _Host = hostBuilder.Build()
        _ServiceProvider = _Host.Services
    End Sub

    Private Sub ConfigurationServices(context As HostBuilderContext, services As IServiceCollection)
        services.AddMvvm()

        services.AddSingleton(context.Configuration)
        services.AddSingleton(Of IHasPopupContent, MainWindowViewModel)
        services.AddMvvmSingleton(Of MainWindowViewModel, MainWindow)

    End Sub

    Protected Overrides Async Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)

        Await _Host.StartAsync
        Dim _ViewLocator = CType(_ServiceProvider.GetRequiredService(GetType(ViewLocator)), ViewLocator)
        MainWindow = _ViewLocator.GetViewForViewModel(Of MainWindowViewModel)(_ServiceProvider)
        MainWindow.Show()
    End Sub
    Protected Overrides Async Sub OnExit(e As ExitEventArgs)
        MyBase.OnExit(e)
        Await _Host.StopAsync(TimeSpan.FromSeconds(1))
        _Host.Dispose()
    End Sub

    Public Shared ReadOnly Property ServiceProvider As IServiceProvider
        Get
            Return _ServiceProvider
        End Get
    End Property

End Class
