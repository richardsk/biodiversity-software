Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml
Imports System.IO


<System.Web.Services.WebService(Namespace:="http://nomina.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ProviderRegistry
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function ListProviders() As Integer()
        Dim ids As New List(Of Integer)

        NominaDataAccess.RegistryData.ConnectionString = ConfigurationManager.ConnectionStrings("Nomina").ConnectionString
        Dim ds As DataSet = NominaDataAccess.RegistryData.ListProviders()
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            For Each row As DataRow In ds.Tables(0).Rows
                ids.Add(row("ProviderRegistryId"))
            Next
        End If

        Return ids.ToArray
    End Function

    <WebMethod()> _
    Public Function GetProvider(ByVal providerId As Integer) As XmlDocument
        NominaDataAccess.RegistryData.ConnectionString = ConfigurationManager.ConnectionStrings("Nomina").ConnectionString
        Dim res As String = NominaDataAccess.RegistryData.GetProviderMetadata(providerId)
        Dim doc As New XmlDocument
        doc.LoadXml(res)
        Return doc
    End Function

    <WebMethod()> _
    Public Sub AddProvider(ByVal securityToken As String, ByVal providerXml As XmlDocument)
        'todo sec token not implmeneted

        Dim newProv As New System.Data.DataSet
        Dim sr As New StringReader(providerXml.OuterXml)
        newProv.ReadXml(sr)

        NominaDataAccess.RegistryData.ConnectionString = ConfigurationManager.ConnectionStrings("Nomina").ConnectionString
        Dim exprov As DataSet = NominaDataAccess.RegistryData.GetProviderByEndpoint(newProv.Tables("metadata").Rows(0)("endpointUrl").ToString)

        If exprov Is Nothing OrElse exprov.Tables.Count = 0 OrElse exprov.Tables(0).Rows.Count = 0 Then
            Dim newId As Integer = NominaDataAccess.RegistryData.InsertUpdateProvider(newProv)

            'acc rules
            If newProv.Tables.Contains("accessionRule") Then
                For Each row As DataRow In newProv.Tables("accessionRule").Rows
                    NominaDataAccess.RegistryData.InsertUpdateAccessionRule(newId, row)
                Next
            End If

            'todo insert entities, etc

        Else
            Throw New Exception("Provider already exixts with this endpoint")
        End If

    End Sub

    <WebMethod()> _
    Public Sub UploadMetadata(ByVal securityToken As String, ByVal metadataUrl As String)
        'todo sec token to impl
        Try
            Dim doc As New XmlDocument
            doc.Load(metadataUrl)
            AddProvider(securityToken, doc)

            HttpContext.Current.Response.Write("Success")
        Catch ex As Exception
            HttpContext.Current.Response.Write("Error : " + ex.Message)
        End Try

    End Sub

End Class