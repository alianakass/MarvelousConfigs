﻿create procedure [GetConfigsByServiceId]
	@ServiceId integer
	as
	select C.[Id], C.[Key], C.[Value], C.[ServiceId], C.[Created], C.[Updated]
	from [dbo].[Configs] as C
	where C.ServiceId = @ServiceId and C.IsDeleted = 0