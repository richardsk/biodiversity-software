IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SelectProviderMetadata')
	BEGIN
		DROP  Procedure  SelectProviderMetadata
	END

GO

CREATE Procedure SelectProviderMetadata
	@providerRegistryId int
AS

	select * from ProviderRegistry
	where providerregistryid = @providerRegistryId
	
	select * from vwAccRules
	where providerregistryid = @providerRegistryId
		
	select e.* from vwProviderEntities e
	inner join providerregistry_entity pre on pre.entityid = e.entityid
	where e.providerregistryid = @providerRegistryId

	select c.* from vwContacts c
	inner join EntityContact ec on ec.contactid = c.contactid
	inner join entity e on e.entityid = ec.entityid
	inner join providerregistry_entity pre on pre.entityid = e.entityid
	where providerregistryid = @providerRegistryId
GO


GRANT EXEC ON SelectProviderMetadata TO PUBLIC

GO


