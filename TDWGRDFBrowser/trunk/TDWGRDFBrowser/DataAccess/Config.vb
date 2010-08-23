Imports System.Data.OleDb

Public Class Config

    Public Shared ConncetionString As String = ""

    Public Shared Function ListRDFModels() As DataSet
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("select * from RDFModel")
                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)
            End Using

            cnn.Close()
        End Using

        Return ds
    End Function

End Class
