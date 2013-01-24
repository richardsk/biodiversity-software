IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'ListProviders')
	BEGIN
		DROP  Procedure  ListProviders
	END

GO

CREATE Procedure ListProviders
	
AS

	select *
	from ProviderRegistry

GO


GRANT EXEC ON ListProviders TO PUBLIC

GO


