Public Class ViewModelRegistry
    Private ReadOnly _ViewModelTypeToViewType As Dictionary(Of Type, Type) = New Dictionary(Of Type, Type)
    Public ReadOnly Property ViewModelTypeToViewType As Dictionary(Of Type, Type)
        Get
            Return _ViewModelTypeToViewType
        End Get
    End Property

End Class
