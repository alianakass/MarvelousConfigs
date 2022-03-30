CREATE procedure [dbo].[UpdateMicroserviceById]
	@Id integer,
	@ServiceName nvarchar(50), 
	@URL nvarchar(max)
	as
	update dbo.[Microservices]
	set
	ServiceName = @ServiceName, [URL] = @URL
	where Id = @Id