create procedure DeleteOrRestoreConfigById
	@Id integer,
	@IsDeleted bit,
	@Updated DateTime = GETDATE
	as
	update dbo.[Configs]
	set IsDeleted = @IsDeleted, Updated = @Updated
	where Id = @Id