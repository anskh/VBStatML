Imports System.ComponentModel
Imports System.Reflection
Imports System.Windows
Imports System.Windows.Controls
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports VBMvvm.Abstractions
Imports VBMvvm.Controls
Imports VBMvvm.Helpers

Public Class ViewLocator
    Private ReadOnly _ServiceProvider As IServiceProvider
    Private ReadOnly _Logger As ILogger(Of ViewLocator)
    Private ReadOnly _ViewModelRegistry As ViewModelRegistry
    Public Sub New(serviceProvider As IServiceProvider, logger As ILogger(Of ViewLocator))
        _ServiceProvider = serviceProvider
        _Logger = logger
        _ViewModelRegistry = serviceProvider.GetRequiredService(Of ViewModelRegistry)
    End Sub
    Public Function GetViewForViewModel(Of TViewModel)(Optional serviceProvider As IServiceProvider = Nothing) As Object
        Dim service As IServiceProvider = _ServiceProvider
        If serviceProvider IsNot Nothing Then
            service = serviceProvider
        End If
        Dim viewModel As TViewModel = service.GetRequiredService(Of TViewModel)
        Return GetViewForViewModel(viewModel, service)
    End Function
    Public Function GetViewForViewModel(viewModel As Object, Optional serviceProvider As IServiceProvider = Nothing) As Object
        _Logger.LogDebug("View for view model " + viewModel.GetType().ToString() + " requested")
        Dim viewType As Type = Nothing
        If Not _ViewModelRegistry.ViewModelTypeToViewType.TryGetValue(viewModel.GetType(), viewType) Then
            _Logger.LogError("Could not find view for view model type " + viewModel.GetType().ToString())
            Throw New InvalidOperationException("No View found for ViewModel of type " + viewModel.GetType().ToString)
        End If
        Dim view As Object = _ServiceProvider.GetRequiredService(viewType)
        _Logger.LogDebug("Resolved to instance of " + view.GetType().ToString)
        If serviceProvider IsNot Nothing And TypeOf view Is DependencyObject Then
            Dim viewObject As DependencyObject = CType(view, DependencyObject)
            SetServiceProvider(viewObject, serviceProvider)
        End If
        If TypeOf view Is FrameworkElement Then
            Dim element As FrameworkElement = CType(view, FrameworkElement)
            AttachHandler(element, viewModel)
            element.DataContext = viewModel
        End If
        InitializeComponent(view)
        Return view
    End Function

    Private Sub AttachHandler(element As FrameworkElement, viewModel As Object)
        If TypeOf viewModel Is IOnLoadedHandler Then
            Dim onLoadedHandler = CType(viewModel, IOnLoadedHandler)
            Dim OnLoadEvent As RoutedEventHandler = Async Sub(sender As Object, args As RoutedEventArgs)
                                                        RemoveHandler element.Loaded, OnLoadEvent
                                                        Await onLoadedHandler.OnLoadedAsync
                                                    End Sub
            AddHandler element.Loaded, OnLoadEvent
        End If
        If TypeOf viewModel Is IOnClosingHandler Then
            Dim onClosingHandler = CType(viewModel, IOnClosingHandler)
            If TypeOf element Is Window Then
                Dim window = CType(element, Window)

                Dim OnClosingEvent As CancelEventHandler = Sub(sender As Object, args As CancelEventArgs)
                                                               RemoveHandler window.Closing, OnClosingEvent
                                                               onClosingHandler.OnClosing()
                                                           End Sub
                AddHandler window.Closing, OnClosingEvent
            Else
                Dim OnUnloadEvent As RoutedEventHandler = Sub(sender As Object, args As RoutedEventArgs)
                                                              RemoveHandler element.Unloaded, OnUnloadEvent
                                                              onClosingHandler.OnClosing()
                                                          End Sub
                AddHandler element.Unloaded, OnUnloadEvent
            End If
        End If
        If TypeOf viewModel Is ICancelableOnClosingHandler Then
            Dim cancelableOnClosingHandler = CType(viewModel, ICancelableOnClosingHandler)
            If TypeOf element IsNot Window Then
                Throw New ArgumentException("If a view model implements ICancelableOnClosingHandler, the corresponding view must be a window.")
            End If
            Dim window = CType(element, Window)
            Dim OnClosingEvent As CancelEventHandler = Sub(sender As Object, args As CancelEventArgs) args.Cancel = cancelableOnClosingHandler.OnCLosing
            AddHandler window.Closing, OnClosingEvent
            Dim OnClosedEvent As EventHandler = Sub(sender As Object, args As EventArgs)
                                                    RemoveHandler window.Closing, OnClosingEvent
                                                    RemoveHandler window.Closed, OnClosedEvent
                                                End Sub
            AddHandler window.Closed, OnClosedEvent
        End If
        If TypeOf viewModel Is IHasPopupContent Then
            If TypeOf element Is ContentControl Then
                Dim viewContent = CType(element, ContentControl)
                Dim control = New PopupControl With {
                    .Content = viewContent.Content
                }
                viewContent.Content = control
            End If
        End If
    End Sub

    Private Sub InitializeComponent(element As Object)
        Dim method = element.GetType().GetMethod("InitializeComponent", BindingFlags.Instance Or BindingFlags.Public)
        If method IsNot Nothing Then
            method.Invoke(element, Nothing)
        End If
    End Sub
End Class
