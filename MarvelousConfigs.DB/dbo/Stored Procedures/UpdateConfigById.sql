	create procedure [dbo].[UpdateConfigById]
	@Id integer,
	@Key nvarchar(max), 
	@Value nvarchar(50),
	@ServiceId integer,
	@Updated DateTime
	as
	update dbo.[Configs]
	set
	[Key] = @Key, [Value] = @Value, ServiceId = @ServiceId, Updated = @Updated
	where Id = @Id