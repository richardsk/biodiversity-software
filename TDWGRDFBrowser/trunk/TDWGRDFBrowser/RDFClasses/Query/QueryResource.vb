Namespace Query

    Public Enum ResourceProtocol
        URL
        TAPIR
        LSID
        OAI_PMH
        CACHE
    End Enum

    Public Class QueryResource
        Public ResProtocol As ResourceProtocol
        Public ResourceName As String
        Public ResourceUrl As String
        Public ClassName As String

        Public Model As RDFModel

        Public Overrides Function ToString() As String
            Return ResourceName
        End Function
    End Class

End Namespace
