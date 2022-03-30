create procedure DeleteOrRestoreConfigById
	@Id integer,
	@IsDeleted bit,
	@Updated DateTime
	as
	update dbo.[Configs]
	set IsDeleted = @IsDeleted, Updated = @Updated
	where Id = @Id