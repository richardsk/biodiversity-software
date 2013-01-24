IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertUpdateProvider')
	BEGIN
		DROP  Procedure  InsertUpdateProvider
	END

GO

CREATE Procedure InsertUpdateProvider
	@providerRegistryId int,
	@title varchar(255),
	@description varchar(255),
	@metadataUrl varchar(255),
	@endpointUrl varchar(255),
	@LogoUrl varchar(255) = NULL,
	@Rights varchar(255) = NULL,
	@Citation varchar(255) = NULL,
	@DataURI varchar(255) = NULL,
	@DataURIType varchar(255) = NULL,
	@ResponseFormat char(10) = NULL,
	@RefreshPeriodHours int = NULL,
	@TaxonomicScope varchar(255) = NULL,
	@GeospatialScopeWKT text = NULL,
	@CreateTimestamp datetime = NULL,
	@ModifiedTimestamp  datetime = NULL
AS

	if (@createTimestamp is null) set @createTimestamp = getdate()
	if (@modifiedTimestamp is null) set @modifiedTimestamp = getdate()

	if (@providerRegistryId is null or @providerRegistryId = 0)
	begin
		insert providerregistry
		select @title, 
			@description, 
			@logoUrl,
			@rights,
			@citation,
			@metadataUrl, 
			@endpointUrl,
			@dataUri,
			@dataUriType,
			@responseFormat,
			@refreshPeriodHours,
			@taxonomicScope,
			@geospatialScopeWKT,
			@createTimestamp,
			@modifiedTimestamp
			
			select @@identity
	end
	else
	begin 
		update providerregistry
		set title = @title,
			description = @description,
			metadataurl = @metadataurl,
			endpointurl = @endpointurl,
			logourl = @logoUrl,
			rights = @rights,
			citation = @citation,
			datauri = @dataUri,
			datauritype = @dataUriType,
			responseformat = @responseFormat,
			refreshperiodhours = @refreshPeriodHours,
			taxonomicscope = @taxonomicScope,
			geospatialscopewkt = @geospatialScopeWKT,
			createtimestamp = @createTimestamp,
			modifiedtimestamp = @modifiedTimestamp
		where providerregistryid = @providerRegistryId
	end

GO


GRANT EXEC ON InsertUpdateProvider TO PUBLIC

GO


