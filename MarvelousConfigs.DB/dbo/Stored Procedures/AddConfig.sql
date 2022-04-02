CREATE procedure [dbo].[AddConfig]
	@Key nvarchar(max), 
	@Value nvarchar(50),  
	@ServiceId integer,
	@Created DateTime = GETDATE
	as
	insert into dbo.[Configs]
	values
	(@Key, @Value, @ServiceId, @Created, null, 0)
	select SCOPE_IDENTITY()