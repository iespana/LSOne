
/*

	Incident No.	: 5703
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 01\Dot Net Team
	Date created	: 06.10.2010

	Description		: This is the "Create Administrator Script.sql" file from the Store Controller

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: n/a
						
*/

use LSPOSNET

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_SetLoginDBGuid_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_SetLoginDBGuid_1_0]

GO


CREATE procedure [dbo].[spSECURITY_SetLoginDBGuid_1_0]
(
 @Login nvarchar(32), 
 @GUID uniqueidentifier
 )
as

-- The user is created so that it will have to change password on next logon
-- and password also expires in x amount of days as is defined in the settings.

set nocount on




	
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE  name  = 'LoginDB'))
begin
	update 
		LoginDB.dbo.Credentials  set UserGuid = @GUID 
		where  
		UserName = @Login
end
