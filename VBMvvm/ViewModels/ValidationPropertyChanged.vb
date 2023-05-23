Imports System.ComponentModel
Imports System.Linq.Expressions
Imports System.Windows.Controls
Imports VBMvvm.Controls
Imports VBMvvm.Helpers

Namespace ViewModels
    Public MustInherit Class ValidationPropertyChanged
        Inherits PropertyChangedBase
        Implements INotifyDataErrorInfo

        Private ReadOnly _Errors As Dictionary(Of String, List(Of ErrorResult)) = New Dictionary(Of String, List(Of ErrorResult))
        Private ReadOnly _Rules As Dictionary(Of String, List(Of ValidationRule)) = New Dictionary(Of String, List(Of ValidationRule))

        Private Class ValidationRule
            Private _ValidationError As ErrorResult
            Private _IsValid As Func(Of Boolean)

            Public Property ValidationError As ErrorResult
                Get
                    Return _ValidationError
                End Get
                Set(value As ErrorResult)
                    _ValidationError = value
                End Set
            End Property
            Public Property IsValid As Func(Of Boolean)
                Get
                    Return _IsValid
                End Get
                Set(value As Func(Of Boolean))
                    _IsValid = value
                End Set
            End Property
        End Class

        Protected Sub AddValidationRule(Of TProperty)(ByVal [property] As Expression(Of Func(Of TProperty)),
                                                      ByVal validate As Predicate(Of TProperty),
                                                      ByVal errorMessage As String,
                                                      Optional ByVal errorLevel As ErrorLevels = ErrorLevels.Error)
            Dim propertyName As String = ExpressionHelper.GetMemberName([property])
            Dim rule As ValidationRule = New ValidationRule With {
                .ValidationError = New ErrorResult(errorMessage, errorLevel),
                .IsValid = Function() As Boolean
                               Dim val As TProperty = [property].Compile()()
                               Return validate(val)
                           End Function
            }
            Dim ruleSet As List(Of ValidationRule) = Nothing
            If Not _Rules.TryGetValue(propertyName, ruleSet) Then
                ruleSet = New List(Of ValidationRule)
                _Rules.Add(propertyName, ruleSet)
            End If
            ruleSet.Add(rule)
        End Sub
        Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get
                Return _Errors.Any()
            End Get
        End Property

        Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged
        Protected Overridable Sub OnErrorsChanged(propertyName As String)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        End Sub

        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
            Dim errorSet As List(Of ErrorResult) = Nothing
            If _Errors.TryGetValue(propertyName, errorSet) Then
                Return errorSet
            Else
                Return Enumerable.Empty(Of ErrorResult)
            End If
        End Function
        Protected Overrides Sub OnPropertyChanged(propertyName As String)
            MyBase.OnPropertyChanged(propertyName)
            If (String.IsNullOrEmpty(propertyName)) Then
                ValidateAllRules()
            Else
                Dim ruleSet As List(Of ValidationRule) = Nothing
                If _Rules.TryGetValue(propertyName, ruleSet) Then
                    For Each rule As ValidationRule In ruleSet
                        If rule.IsValid.Invoke() Then
                            AddError(propertyName, rule.ValidationError)
                        Else
                            RemoveError(propertyName, rule.ValidationError)
                        End If
                    Next
                End If
            End If
        End Sub
        Protected Overridable Function ValidateAllRules() As Boolean
            For Each ruleSet As KeyValuePair(Of String, List(Of ValidationRule)) In _Rules
                For Each rule As ValidationRule In ruleSet.Value
                    If rule.IsValid.Invoke() Then
                        AddError(ruleSet.Key, rule.ValidationError)
                    Else
                        RemoveError(ruleSet.Key, rule.ValidationError)
                    End If
                Next
            Next
            Return Not HasErrors
        End Function
        Protected Sub Validate(Of TProperty)(isValid As Boolean, [property] As Expression(Of Func(Of TProperty)), validationError As ErrorResult)
            Validate(isValid, ExpressionHelper.GetMemberName([property]), validationError)
        End Sub
        Protected Sub Validate(isValid As Boolean, propertyName As String, validationError As ErrorResult)
            If isValid Then
                RemoveError(propertyName, validationError)
            Else
                AddError(propertyName, validationError)
            End If
        End Sub
        Protected Sub AddError(Of TProperty)([property] As Expression(Of Func(Of TProperty)), validationError As ErrorResult)
            AddError(ExpressionHelper.GetMemberName([property]), validationError)
        End Sub
        Protected Sub AddError(propertyName As String, validationError As ErrorResult)
            Dim errorSet As List(Of ErrorResult) = Nothing
            If Not _Errors.TryGetValue(propertyName, errorSet) Then
                errorSet = New List(Of ErrorResult)
                _Errors.Add(propertyName, errorSet)
            End If
            If Not errorSet.Contains(validationError) Then
                errorSet.Add(validationError)
                OnErrorsChanged(propertyName)
            End If
        End Sub
        Protected Sub RemoveError(Of TProperty)([property] As Expression(Of Func(Of TProperty)), validationError As ErrorResult)
            RemoveError(ExpressionHelper.GetMemberName([property]), validationError)
        End Sub
        Protected Sub RemoveError(propertyName As String, validationError As ErrorResult)
            Dim errorSet As List(Of ErrorResult) = Nothing
            If _Errors.TryGetValue(propertyName, errorSet) Then
                If errorSet.Contains(validationError) Then
                    errorSet.Remove(validationError)
                    If Not errorSet.Any Then
                        _Errors.Remove(propertyName)
                    End If
                    OnErrorsChanged(propertyName)
                End If
            End If
        End Sub
    End Class
End Namespace