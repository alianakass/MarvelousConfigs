create procedure GetAllConfigs
	as
	select C.[Id], C.[Key], C.[Value], C.[ServiceId], C.[Created], C.[Updated]
	from [dbo].[Configs] as C
	where C.IsDeleted = 0