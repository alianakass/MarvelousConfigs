	create procedure [dbo].[UpdateConfigById]
	@Id integer,
	@Key nvarchar(max), 
	@Value nvarchar(50),
	@ServiceId integer
	as
	update dbo.[Configs]
	set
	[Key] = @Key, [Value] = @Value, ServiceId = @ServiceId, Updated = SYSDATETIME()
	where Id = @Id