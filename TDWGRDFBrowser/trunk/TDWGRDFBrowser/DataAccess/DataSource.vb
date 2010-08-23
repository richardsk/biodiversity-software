Imports System.Data.OleDb

Public Class DataSource

    Public Shared ConncetionString As String = ""

    Public Shared Function ListDataSources() As DataSet
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("select * from DataSource")
                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)
            End Using

            cnn.Close()
        End Using

        Return ds
    End Function

    Public Shared Function GetDataSource(ByVal dataSourceId As Integer) As DataSet
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("select * from DataSource where DataSourceId = " + dataSourceId.ToString)

                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)
            End Using

            cnn.Close()
        End Using

        Return ds
    End Function

    Public Shared Function ListDataSourceModels(ByVal dsId As Integer) As DataSet
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Dim cmdStr As String = "select * from DataSourceModel dsm inner join RDFModel m on m.ModelId = dsm.RDFModelId where DataSourceId = " + dsId.ToString
            Using cmd As New OleDbCommand(cmdStr)
                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)
            End Using

            cnn.Close()
        End Using

        Return ds
    End Function

    Public Shared Function ListModelDataSources(ByVal modelId As String) As DataSet
        Dim ds As New DataSet

        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Using cmd As New OleDbCommand("select * from DataSource ds inner join DataSourceModel dsm " + _
                "on dsm.DataSourceId = ds.DataSourceId where RDFModelId = " + modelId)

                cmd.Connection = cnn
                Dim da As New OleDbDataAdapter(cmd)

                da.Fill(ds)
            End Using

            cnn.Close()
        End Using

        Return ds
    End Function

    Public Shared Sub UpdateDataSource(ByVal row As DataRow)
        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Dim cmdStr As String = ""

            If row("DataSourceId").ToString = "" Then
                'insert
                cmdStr = "insert into DataSource(Name,Url, QuerierClass, Include) "
                cmdStr += "values('" + row("Name").ToString + "','" + row("Url").ToString
                Dim inc As String = "1"
                If Not row("Include") Then inc = "0"
                cmdStr += "', '" + row("QuerierClass").ToString + "', " + inc + ")"
            Else
                'update
                cmdStr = "update DataSource set Name='" + row("Name").ToString + "', Url='"
                cmdStr += row("Url").ToString + "', QuerierClass='" + row("QuerierClass").ToString
                Dim inc As String = "1"
                If Not row("Include") Then inc = "0"
                cmdStr += "', Include=" + inc + " where DataSourceId = " + row("DataSourceId").ToString
            End If

            Using cmd As New OleDbCommand(cmdStr)
                cmd.Connection = cnn
                cmd.ExecuteNonQuery()
            End Using

            cnn.Close()
        End Using
    End Sub

    Public Shared Sub DeleteDataSource(ByVal id As Integer)
        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Dim cmdStr As String = "delete * from DataSource where DataSourceId = " + id.ToString
            Using cmd As New OleDbCommand(cmdStr)
                cmd.Connection = cnn
                cmd.ExecuteNonQuery()
            End Using

            cnn.Close()
        End Using
    End Sub

    Public Shared Sub UpdateDataSourceModels(ByVal dsId As Integer, ByVal ds As DataSet)
        Using cnn As New OleDbConnection(ConncetionString)
            cnn.Open()

            Dim cmdStr As String = "delete * from DataSourceModel where DataSourceId = " + dsId.ToString
            Using cmd As New OleDbCommand(cmdStr)
                cmd.Connection = cnn
                cmd.ExecuteNonQuery()
            End Using

            For Each row As DataRow In ds.Tables(0).Rows
                cmdStr = "insert into DataSourceModel(DataSourceId, RDFModelId) "
                cmdStr += "values(" + dsId.ToString + ", " + row("ModelId").ToString + ")"

                Using cmd As New OleDbCommand(cmdStr)
                    cmd.Connection = cnn
                    cmd.ExecuteNonQuery()
                End Using
            Next

            cnn.Close()
        End Using
    End Sub

End Class
