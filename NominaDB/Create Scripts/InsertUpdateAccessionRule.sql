IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertUpdateAccessionRule')
	BEGIN
		DROP  Procedure  InsertUpdateAccessionRule
	END

GO

CREATE Procedure InsertUpdateAccessionRule
	@providerRegistryId int,
	@accessionType varchar(255),
	@isAllowed bit
AS

	declare @accessionTypeId int
	select @accessionTypeId = accessiontypeid from accessiontype where type = @accessiontype
	
	
	delete accessionrule where providerRegistryId = @providerRegistryId 
		and accessionTypeId = @accessionTypeId
	
		
	insert accessionrule
	select @providerRegistryId, @accessionTypeId, @isAllowed
	

GO


GRANT EXEC ON InsertUpdateAccessionRule TO PUBLIC

GO


