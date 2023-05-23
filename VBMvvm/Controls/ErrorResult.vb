Namespace Controls
    Public Class ErrorResult
        Private _ErrorMessage As String
        Private _ErrorLevel As ErrorLevels
        Public Sub New(ByVal message As String, Optional ByVal errorLevel As ErrorLevels = ErrorLevels.Error)
            _ErrorMessage = message
            _ErrorLevel = errorLevel
        End Sub
        Public ReadOnly Property ErrorMessage As String
            Get
                Return _ErrorMessage
            End Get
        End Property
        Public ReadOnly Property ErrorLevel As ErrorLevels
            Get
                Return _ErrorLevel
            End Get
        End Property

    End Class
End Namespace