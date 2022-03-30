CREATE procedure [dbo].[GetAllMicroservices]
	as
	select S.[Id], S.[ServiceName], S.[URL] from dbo.[Microservices] as S
	where IsDeleted = 0