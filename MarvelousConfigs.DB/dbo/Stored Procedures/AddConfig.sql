CREATE procedure [dbo].[AddConfig]
	@Key nvarchar(max), 
	@Value nvarchar(50),  
	@ServiceId integer
	as
	insert into dbo.[Configs]
	values
	(@Key, @Value, @ServiceId, SYSDATETIME(), null, 0)
	select SCOPE_IDENTITY()