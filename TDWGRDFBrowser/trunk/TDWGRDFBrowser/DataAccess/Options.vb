Imports System.Data.OleDb

Public Class Options
    Public CacheQueryResults As Boolean = False
    Public AutoResolveToLevel As Integer = 1
End Class

Public Class OptionData

    Public Shared ConncetionString As String = ""

    Public Shared Function GetOptions() As Options
        Dim opt As New Options
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("select * from Options")
                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)

                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    opt.CacheQueryResults = ds.Tables(0).Rows(0)("CacheQueryResults")
                    opt.AutoResolveToLevel = ds.Tables(0).Rows(0)("AutoResolveLevel")
                End If
            End Using

            cnn.Close()
        End Using

        Return opt
    End Function

    Public Shared Sub SaveOptions(ByVal opt As Options)
        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Dim cmdStr As String = "update Options set CacheQueryResults = "
            If opt.CacheQueryResults Then
                cmdStr += "1, "
            Else
                cmdStr += "0, "
            End If
            cmdStr += "AutoResolveLevel = " + opt.AutoResolveToLevel.ToString

            Using cmd As New OleDbCommand(cmdStr)
                cmd.Connection = cnn
                cmd.ExecuteNonQuery()
            End Using

            cnn.Close()
        End Using
    End Sub

End Class
