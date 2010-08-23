Imports System.Data.OleDb

Public Class RDFModel

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

    Public Shared Function GetModel(ByVal modelId As Integer) As DataSet
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("select * from RDFModel where ModelId = " + modelId.ToString)
                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)
            End Using

            cnn.Close()
        End Using

        Return ds
    End Function

    Public Shared Sub DeleteModel(ByVal id As Integer)

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("delete * from RDFModel where ModelId = " + id.ToString)
                cmd.Connection = cnn
                cmd.ExecuteNonQuery()
            End Using

            cnn.Close()
        End Using
    End Sub

    Public Shared Sub UpdateModel(ByVal row As DataRow)

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Dim cmdStr As String = ""

            If row("ModelId").ToString = "" Then
                cmdStr = "insert into RDFModel(Name, Url, Description, Type, QueryTemplateUrl, GUIDElement, HarvestTemplateUrl) values ('"
                cmdStr += row("Name").ToString + "', '" + row("Url").ToString + "', '"
                cmdStr += row("Description").ToString + "', '" + row("Type").ToString
                cmdStr += "', '" + row("QueryTemplateUrl").ToString + "', '" + row("GUIDElement").ToString
                cmdStr += "', '" + row("HarvestTemplateUrl").ToString + "')"
            Else
                cmdStr = "update RDFModel set Name='" + row("Name").ToString
                cmdStr += "', Url='" + row("Url").ToString + "', Description='"
                cmdStr += row("Description").ToString + "', Type='"
                cmdStr += row("Type").ToString + "', QueryTemplateUrl='"
                cmdStr += row("QueryTemplateUrl").ToString + "', GUIDElement = '"
                cmdStr += row("GUIDElement").ToString + "', HarvestTemplateUrl = '"
                cmdStr += row("HarvestTemplateUrl").ToString + "' "
                cmdStr += "where ModelId = " + row("ModelId").ToString
            End If

            Using cmd As New OleDbCommand(cmdStr)
                cmd.Connection = cnn
                cmd.ExecuteNonQuery()
            End Using

            cnn.Close()
        End Using
    End Sub

End Class
