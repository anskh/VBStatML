Imports System.Linq.Expressions
Imports System.Reflection

Namespace Helpers
    Public Module ExpressionHelper
        Public Function GetMemberName(Of TProperty)(ByVal [property] As Expression(Of Func(Of TProperty))) As String
            Return GetMemberInfo([property]).Name
        End Function
        Public Function GetMemberName(Of TEntity, TProperty)(ByVal [property] As Expression(Of Func(Of TEntity, TProperty))) As String
            Return GetMemberInfo([property]).Name
        End Function
        Private Function GetMemberInfo(ByVal expression As Expression) As MemberInfo
            Dim _LambdaExpression As LambdaExpression = CType(expression, LambdaExpression)
            If TypeOf _LambdaExpression.Body Is UnaryExpression Then
                Dim _UnaryExpression As UnaryExpression = CType(_LambdaExpression.Body, UnaryExpression)
                Return CType(_UnaryExpression.Operand, MemberExpression).Member
            Else
                Return CType(_LambdaExpression.Body, MemberExpression).Member
            End If
        End Function
    End Module
End Namespace