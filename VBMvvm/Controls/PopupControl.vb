Imports System.Windows
Imports System.Windows.Controls

Namespace Controls
    Public Class PopupControl
        Inherits ContentControl
        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(PopupControl), New FrameworkPropertyMetadata(GetType(PopupControl)))
        End Sub
    End Class
End Namespace