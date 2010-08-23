Public Class TDWGRDFException
    Inherits Exception

    Public Shared Sub LogError(ByVal ex As Exception)
        Try
            EventLog.WriteEntry("Application", "RDFBrowser error : " + ex.Message + " : " + ex.StackTrace, EventLogEntryType.Error)
        Catch e As Exception
            MsgBox("Failed to write to error log : " + ex.Message)
        End Try
    End Sub
End Class

