﻿create procedure [dbo].[GetConfigsByServiceAddress]
@Address nvarchar(max)
as
select C.Id, C.[Key], C.[Value], C.[Description] from [dbo].[Configs] as C
inner join [dbo].[Microservices] as M
on M.Id = C.ServiceId
where M.[Address] = @Address