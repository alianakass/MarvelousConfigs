﻿CREATE procedure [dbo].[GetAllMicroservicesWithConfigs]
	as
	select S.Id, S.ServiceName, S.[URL], C.Id, C.[Key], C.[Value], C.ServiceId, C.Created, C.Updated from dbo.[Microservices] as S
	inner join dbo.[Configs] as C
	on C.ServiceId = S.Id
	where C.IsDeleted = 0 and S.IsDeleted = 0