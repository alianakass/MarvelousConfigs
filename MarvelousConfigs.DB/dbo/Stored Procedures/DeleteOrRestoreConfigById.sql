create procedure DeleteOrRestoreConfigById
	@Id integer,
	@IsDeleted bit
	as
	update dbo.[Configs]
	set IsDeleted = @IsDeleted, Updated = SYSDATETIME()
	where Id = @Id