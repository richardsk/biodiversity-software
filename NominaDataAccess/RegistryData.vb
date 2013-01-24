Imports System.Xml
Imports System.Data.SqlClient

Public Class RegistryData

    Public Shared ConnectionString As String = ""

    Public Shared Function GetProvider(ByVal providerRegistryId As Integer) As DataSet
        Dim ds As New DataSet
        Dim cnn As New SqlConnection(ConnectionString)
        cnn.Open()

        Dim cmd As New SqlCommand("select * from ProviderRegistry where ProviderRegistryId = " + providerRegistryId.ToString)
        cmd.Connection = cnn

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(ds)

        cnn.Close()


        Return ds
    End Function

    Public Shared Function GetProviderByEndpoint(ByVal endpoint As String) As DataSet
        Dim ds As New DataSet
        Dim cnn As New SqlConnection(ConnectionString)
        cnn.Open()

        Dim cmd As New SqlCommand("select * from ProviderRegistry where EndpointUrl = '" + endpoint + "'")
        cmd.Connection = cnn

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(ds)

        cnn.Close()

        Return ds
    End Function

    Private Shared Function GetRowVal(ByVal row As DataRow, ByVal field As String) As Object
        Dim val As Object = DBNull.Value

        If row.Table.Columns.Contains(field) Then
            val = row(field)
        End If
        Return val
    End Function

    Private Shared Function GetMetadataDesc(ByVal ds As DataSet) As String
        Dim desc As String = ""
        For Each t As DataTable In ds.Tables
            If t.TableName = "description" AndAlso t.Columns.Contains("metadata_id") Then
                desc = GetRowVal(t.Rows(0), "description_text")
                Exit For
            End If
        Next
        Return desc
    End Function

    Private Shared Function GetEntityDesc(ByVal ds As DataSet) As String
        Dim desc As String = ""
        For Each t As DataTable In ds.Tables
            If t.TableName = "description" AndAlso t.Columns.Contains("entity_id") Then
                desc = GetRowVal(t.Rows(0), "description_text")
                Exit For
            End If
        Next
        Return desc
    End Function

    Public Shared Function InsertUpdateProvider(ByVal provDs As DataSet) As Integer

        Dim cnn As New SqlConnection(ConnectionString)
        cnn.Open()

        Dim cmd As New SqlCommand("insertupdateprovider")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = cnn

        Dim provRow As DataRow = provDs.Tables("metadata").Rows(0)

        cmd.Parameters.Add("@providerregistryid", SqlDbType.Int).Value = GetRowVal(provRow, "ProviderRegistryId")
        cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = GetRowVal(provDs.Tables("title").Rows(0), "title_text")
        cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = GetMetadataDesc(provDs)
        cmd.Parameters.Add("@logoUrl", SqlDbType.VarChar).Value = GetRowVal(provRow, "logoUrl")
        cmd.Parameters.Add("@rights", SqlDbType.VarChar).Value = GetRowVal(provDs.Tables("rights").Rows(0), "rights_text")
        cmd.Parameters.Add("@citation", SqlDbType.VarChar).Value = GetRowVal(provDs.Tables("citation").Rows(0), "citation_text")
        cmd.Parameters.Add("@metadataUrl", SqlDbType.VarChar).Value = GetRowVal(provRow, "metadataurl")
        cmd.Parameters.Add("@endpointUrl", SqlDbType.VarChar).Value = GetRowVal(provRow, "endpointurl")
        cmd.Parameters.Add("@dataUri", SqlDbType.VarChar).Value = GetRowVal(provRow, "dataUri")
        cmd.Parameters.Add("@dataUriType", SqlDbType.Char).Value = GetRowVal(provRow, "dataUriType")
        cmd.Parameters.Add("@responseFormat", SqlDbType.VarChar).Value = GetRowVal(provRow, "responseFormat")
        cmd.Parameters.Add("@refreshPeriodHours", SqlDbType.Int).Value = GetRowVal(provRow, "refreshPeriodHours")
        cmd.Parameters.Add("@taxonomicScope", SqlDbType.VarChar).Value = GetRowVal(provRow, "taxonomicScope")
        cmd.Parameters.Add("@geospatialScopeWKT", SqlDbType.Text).Value = GetRowVal(provRow, "geospatialScopeSKT")
        cmd.Parameters.Add("@createTimestamp", SqlDbType.DateTime).Value = GetRowVal(provRow, "createdTimestamp")
        cmd.Parameters.Add("@modifiedTimestamp", SqlDbType.DateTime).Value = GetRowVal(provRow, "modifiedTimestamp")

        Dim id As Integer = CInt(cmd.ExecuteScalar())

        cnn.Close()

        Return id
    End Function


    Public Shared Function InsertUpdateAccessionRule(ByVal providerRegId As Integer, ByVal rule As DataRow) As Integer

        Dim cnn As New SqlConnection(ConnectionString)
        cnn.Open()

        Dim cmd As New SqlCommand("insertupdateaccessionrule")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = cnn

        cmd.Parameters.Add("@providerregistryid", SqlDbType.Int).Value = providerRegId
        cmd.Parameters.Add("@accessionType", SqlDbType.VarChar).Value = rule("name")
        cmd.Parameters.Add("@isAllowed", SqlDbType.Bit).Value = (rule("accessionRule_Text").ToString.ToLower = "true")

        Dim id As Integer = CInt(cmd.ExecuteScalar())

        cnn.Close()

        Return id
    End Function

    Public Shared Function GetProviderMetadata(ByVal providerId As Integer) As String
        Dim ds As New DataSet
        Dim cnn As New SqlConnection(ConnectionString)
        cnn.Open()

        Dim cmd As New SqlCommand("selectprovidermetadata")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("@providerRegistryId", SqlDbType.Int).Value = providerId
        cmd.Connection = cnn

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(ds)

        Dim doc As String = IO.File.ReadAllText(Configuration.ConfigurationManager.AppSettings("metadataXmlDoc"))
        Dim entDoc As String = IO.File.ReadAllText(Configuration.ConfigurationManager.AppSettings("entityXmlDoc"))
        Dim contDoc As String = IO.File.ReadAllText(Configuration.ConfigurationManager.AppSettings("contactXmlDoc"))


        'fill sections of metadata doc
        For Each dt As DataTable In ds.Tables
            If dt.Columns.Contains("endpointurl") Then
                'metadata 
                doc = doc.Replace("[title]", dt.Rows(0)("Title").ToString)
                doc = doc.Replace("[description]", dt.Rows(0)("Description").ToString)
                doc = doc.Replace("[logo_url]", dt.Rows(0)("LogoUrl").ToString)
                doc = doc.Replace("[rights]", dt.Rows(0)("Rights").ToString)
                doc = doc.Replace("[citation]", dt.Rows(0)("Citation").ToString)
                doc = doc.Replace("[metadata_url]", dt.Rows(0)("MetadataUrl").ToString)
                doc = doc.Replace("[endpoint_url]", dt.Rows(0)("EndpointUrl").ToString)
                doc = doc.Replace("[data_uri]", dt.Rows(0)("DataURI").ToString)
                doc = doc.Replace("[data_uri_type]", dt.Rows(0)("DataURIType").ToString)
                doc = doc.Replace("[response_format]", dt.Rows(0)("ResponseFormat").ToString)
                doc = doc.Replace("[refresh_period]", dt.Rows(0)("RefreshPeriodHours").ToString)
                doc = doc.Replace("[taxonomic_scope]", dt.Rows(0)("TaxonomicScope").ToString)
                doc = doc.Replace("[geospatial_scope]", dt.Rows(0)("GeospatialScopeWKT").ToString)
                doc = doc.Replace("[modified_timestamp]", dt.Rows(0)("ModifiedTimestamp").ToString)

            ElseIf dt.Columns.Contains("isallowed") Then
                'accessionRule
                Dim allRules As String = ""
                For Each row As DataRow In dt.Rows
                    Dim thisRule As String = "<accessionRule name=""" + row("Type").ToString + """>" + row("IsAllowed").ToString.ToLower + "</accessionRule>"
                    allRules += thisRule + Environment.NewLine
                Next
                doc = doc.Replace("[accession_rules]", allRules)
            Else
                'todo others

            End If
        Next

        'tidy up
        doc = doc.Replace("[accession_rules]", "")
        doc = doc.Replace("[related_entities]", "")
        'todo others

        cnn.Close()

        Return doc
    End Function

    Public Shared Function ListProviders() As DataSet
        Dim ds As New DataSet
        Dim cnn As New SqlConnection(ConnectionString)
        cnn.Open()

        Dim cmd As New SqlCommand("listproviders")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = cnn

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(ds)

        cnn.Close()

        Return ds
    End Function

End Class
