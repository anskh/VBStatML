Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Media

Namespace Helpers
    Public Module DependencyObjectExtension
        <Extension()> Public Function GetParentObject(child As DependencyObject) As DependencyObject
            If child IsNot Nothing Then
                If TypeOf child Is ContentElement Then
                    Dim contentElement = CType(child, ContentElement)
                    Dim parent = ContentOperations.GetParent(contentElement)
                    If parent IsNot Nothing Then
                        Return parent
                    Else
                        If TypeOf contentElement Is FrameworkContentElement Then
                            Dim fce = CType(contentElement, FrameworkContentElement)
                            Return fce.Parent
                        End If
                    End If
                End If
                Dim childParent = VisualTreeHelper.GetParent(child)
                If childParent IsNot Nothing Then
                    Return childParent
                End If
                If TypeOf child Is FrameworkElement Then
                    Dim element = CType(child, FrameworkElement)
                    If element.Parent IsNot Nothing Then
                        Return element.Parent
                    End If
                End If
            End If
            Return Nothing
        End Function
    End Module
End Namespace
