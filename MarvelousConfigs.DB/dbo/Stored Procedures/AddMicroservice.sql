CREATE procedure [dbo].[AddMicroservice] 
	@ServiceName nvarchar(50), 
	@URL nvarchar(max)
	as
	insert into dbo.[Microservices]
	values
	(@ServiceName, @URL, 0)
	select @@IDENTITY