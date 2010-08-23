Public Class TDWGRDFException
    Inherits Exception

    Public Shared Sub LogError(ByVal ex As Exception)
        Try
            EventLog.WriteEntry("Application", ex.Message + " : " + ex.StackTrace)
        Catch e As Exception
        End Try
    End Sub
End Class

