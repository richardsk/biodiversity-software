IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteProvider')
	BEGIN
		DROP  Procedure  DeleteProvider
	END

GO

CREATE Procedure DeleteProvider
	@providerRegistryId int
AS

	delete AccessionRule
	where providerregistryid = @providerregistryid

	delete ProviderRegistry
	where providerregistryid = @providerregistryid

GO


GRANT EXEC ON DeleteProvider TO PUBLIC

GO


