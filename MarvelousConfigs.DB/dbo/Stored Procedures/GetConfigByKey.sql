CREATE PROCEDURE [dbo].[GetConfigByKey]
@Key nvarchar(max)
as
select C.Id, C.[Key], C.[Value] from dbo.[Configs] as C
where C.[Key] = @Key
