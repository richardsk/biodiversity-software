IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertUpdateEntity')
	BEGIN
		DROP  Procedure  InsertUpdateEntity
	END

GO

CREATE Procedure InsertUpdateEntity
	@providerRegistryId int,
	@roleId int,
	@EntityID [int],
	@Name [varchar](255),
	@Identifier [varchar](255) = NULL,
	@Acronym [varchar](255) = NULL,
	@LogoURL [varchar](255) = NULL,
	@Description [text] = NULL,
	@Address [varchar](255) = NULL,
	@DecimalLatitude [float] = NULL,
	@DecimalLongitude [float] = null
AS

	if (@entityid is null or @entityid = 0)
	begin
		insert entity(name, identifier, acronym) --todo more
		select @name, @identifier, @acronym
		
		select @entityId = @@identity
		
		insert providerregistry_entity
		select @providerregistryId,
			@entityId,
			@roleId
			
	end
	else
	begin 
		update entity
		set name = @name,
			IDENTIFIER = @IDENTIFIER,
			acronym = @acronym
		where entityid = @entityid
		
		update providerregistry_entity
		set entityroleid = @roleid
		where providerregistryid = @providerregistryid and
			entityid = @entityid
			
	end

GO


GRANT EXEC ON InsertUpdateEntity TO PUBLIC

GO


