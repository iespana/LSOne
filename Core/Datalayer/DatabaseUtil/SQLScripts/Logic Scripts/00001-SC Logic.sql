
/*

	Incident No.	: 5703
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 01\Dot Net Team
	Date created	: 06.10.2010

	Description		: This is the "Logic Script.sql" file from the Store Controller

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: n/a
						
*/

Use LSPOSNET 

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spEXECsp_RECOMPILE]'))
begin
	exec ('create procedure dbo.spEXECsp_RECOMPILE as select 1 a')
end 

go

alter PROCEDURE dbo.spEXECsp_RECOMPILE AS 
/*
----------------------------------------------------------------------------
-- Object Name: dbo.spEXECsp_RECOMPILE 
-- Project: SQL Server Database Maintenance
-- Business Process: SQL Server Database Maintenance
-- Purpose: Execute sp_recompile for all tables in a database
-- Detailed Description: Execute sp_recompile for all tables in a database
-- Database: Admin
-- Dependent Objects: None
-- Called By: TBD
-- Upstream Systems: None
-- Downstream Systems: None
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified  | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 06.07.2007 | JKadlec | Original code
-- 002 | N\A | 05.07.2012 | JKadlec | Updated code for SQL Server 2008 R2
*/

SET NOCOUNT ON 

-- 1 - Declaration statements for all variables
DECLARE @TableName varchar(128)
DECLARE @OwnerName varchar(128)
DECLARE @CMD1 varchar(8000)
DECLARE @TableListLoop int
DECLARE @TableListTable table
(UIDTableList int IDENTITY (1,1),
OwnerName varchar(128),
TableName varchar(128))

-- 2 - Outer loop for populating the database names
INSERT INTO @TableListTable(OwnerName, TableName)
SELECT u.[Name], o.[Name]
FROM sys.objects o
INNER JOIN sys.schemas u
 ON o.schema_id  = u.schema_id
WHERE o.Type = 'U'
ORDER BY o.[Name]

-- 3 - Determine the highest UIDDatabaseList to loop through the records
SELECT @TableListLoop = MAX(UIDTableList) FROM @TableListTable

-- 4 - While condition for looping through the database records
WHILE @TableListLoop > 0
 BEGIN

 -- 5 - Set the @DatabaseName parameter
 SELECT @TableName = TableName,
 @OwnerName = OwnerName
 FROM @TableListTable
 WHERE UIDTableList = @TableListLoop

 -- 6 - String together the final backup command
 SELECT @CMD1 = 'EXEC sp_recompile ' + '[' + @OwnerName + '.' + @TableName + ']' + char(13)

 -- 7 - Execute the final string to complete the backups
 -- SELECT @CMD1
 EXEC (@CMD1)

 -- 8 - Descend through the database list
 SELECT @TableListLoop = @TableListLoop - 1
END

SET NOCOUNT OFF

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vSECURITY_AllUsers_1_0]'))
begin
   drop view dbo.vSECURITY_AllUsers_1_0
end

GO 

create view dbo.vSECURITY_AllUsers_1_0

as

select u.*,(case when (u.LockOutCounter >= settings.[Value]) then 1 else 0 end) as Disabled 
from USERS u 
join SYSTEMSETTINGS settings on settings.GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}'
where Deleted = 0 

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vSECURITY_UsersInGroup_1_0]'))
begin
   drop view dbo.vSECURITY_UsersInGroup_1_0
end

GO

create view dbo.vSECURITY_UsersInGroup_1_0

as

select u.*, ug.Name AS UserGroupName, ug.GUID AS UserGroupGUID,ug.IsAdminGroup,
	   (case when (u.LockOutCounter >= settings.[Value]) then 1 else 0 end) as Disabled 
from   USERS u inner join
	   USERSINGROUP on u.GUID = USERSINGROUP.UserGUID and u.DATAAREAID = USERSINGROUP.DATAAREAID inner join
	   USERGROUPS ug on USERSINGROUP.UserGroupGUID = ug.GUID and USERSINGROUP.DATAAREAID = ug.DATAAREAID
	   join SYSTEMSETTINGS settings on settings.GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}' and settings.DATAAREAID = u.DATAAREAID
where     (u.Deleted = 0)

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_GetProfile_1_0]'))
begin
   drop procedure dbo.spSECURITY_GetProfile_1_0
end

GO

create procedure dbo.spSECURITY_GetProfile_1_0
(@Login nvarchar(250),@PasswordHash nvarchar(250),@dataareaID nvarchar(10))
as

set nocount on

declare @userGUID uniqueidentifier

if Exists(Select 'x' from vSECURITY_AllUsers_1_0 where Login = @Login and PasswordHash = @PasswordHash and DATAAREAID = @dataareaID)
  begin
	Select @userGUID = GUID from vSECURITY_AllUsers_1_0 where Login = @Login and PasswordHash = @PasswordHash and DATAAREAID = @dataareaID

	-- Return the settings values for the user
	select s.GUID, COALESCE(u.Value,s.Value) as Value   
	from USERSETTINGS u right outer join SYSTEMSETTINGS s on u.SettingsGUID = s.GUID and u.DATAAREAID = s.DATAAREAID
	and s.UserCanOverride = 1 and u.UserGUID = @userGUID and u.DATAAREAID = @dataareaID and s.Internal = 0
	and s.Type = 'C79AE480-7EE1-11DB-9FE1-0800200C9A66' and u.Type = 'C79AE480-7EE1-11DB-9FE1-0800200C9A66'
	
	-- Codes that we are granted directly on the user
	select p.PermissionCode, p.CodeIsEncrypted, p.GUID
	from USERPERMISSION up inner join
	PERMISSIONS p on up.PermissionGUID = p.GUID
	where up.[Grant] = 1 and up.UserGUID = @userGUID and up.DATAAREAID = @dataareaID

	union

	-- Codes that we are granted from a Groups
	select Distinct p.PermissionCode, p.CodeIsEncrypted, p.GUID
	from   vSECURITY_UsersInGroup_1_0 u inner join
	USERGROUPPERMISSIONS on u.UserGroupGUID = USERGROUPPERMISSIONS.UserGroupGUID and u.DATAAREAID = USERGROUPPERMISSIONS.DATAAREAID inner join
	dbo.PERMISSIONS p on dbo.USERGROUPPERMISSIONS.PermissionGUID = p.GUID and u.GUID = @userGUID and u.DATAAREAID = @dataareaID

	-- We need to strip out codes where we had exclusive deny on the user
	where u.DATAAREAID = @dataareaID and not Exists(
		select pp.PermissionCode, pp.CodeIsEncrypted, pp.GUID
		from USERPERMISSION up inner join
		PERMISSIONS pp on up.PermissionGUID = pp.GUID and pp.DATAAREAID = pp.DATAAREAID
		where up.[Grant] = 0 and up.UserGUID = @userGUID and up.DATAAREAID = @dataareaID
		and pp.GUID = p.GUID
	)

  end
else
  begin
	select s.GUID, s.Value    
	from SYSTEMSETTINGS s
	where s.GUID = NULL
  
	-- If we did not have a valid login then we return empty recordsets
	select p.PermissionCode, p.CodeIsEncrypted, p.GUID 
	from PERMISSIONS p where p.GUID = null

  end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_GetUserPermissions_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_GetUserPermissions_1_0]

GO

create procedure dbo.spSECURITY_GetUserPermissions_1_0
(@userGuid uniqueidentifier,@baseLanguage nvarchar(10),@language nvarchar(10),@dataareaID nvarchar(10)) 
as

set nocount on

-- HasPermission Values:
-- 0 : Exclusive Deny
-- 1 : Exclusive Grant
-- 2 : Inherit from Group - Grant
-- 3 : Inherit from Group - No permission

Select ps.GUID,Coalesce(pl2.Description,pl1.Description,ps.Description) as Description,ps.HasPermission,Coalesce(pg2.Name,pg1.Name,pg.Name) as PermissionGroupName,ps.PermissionCode,ps.CodeIsEncrypted, ps.DATAAREAID from
(
	select p.GUID,p.Description,p.SortCode, p.PermissionGroupGUID,p.PermissionCode,p.CodeIsEncrypted,up.[Grant] as HasPermission,p.DATAAREAID
	from   USERPERMISSION up INNER JOIN
		   PERMISSIONS p ON up.PermissionGUID = p.GUID and up.DATAAREAID = p.DATAAREAID
	where  up.UserGUID = @userGuid and up.DATAAREAID = @dataareaID

union 
	select p.GUID, p.Description,p.SortCode,p.PermissionGroupGUID,p.PermissionCode,p.CodeIsEncrypted,COALESCE(pug.HasPermission,3),p.DATAAREAID 
	from   PERMISSIONS p Left Outer join
		(select Distinct p.GUID, HasPermission = 2
		from   vSECURITY_UsersInGroup_1_0 u inner join
		USERGROUPPERMISSIONS on u.UserGroupGUID = USERGROUPPERMISSIONS.UserGroupGUID and u.DATAAREAID = USERGROUPPERMISSIONS.DATAAREAID inner join
		dbo.PERMISSIONS p on dbo.USERGROUPPERMISSIONS.PermissionGUID = p.GUID and u.GUID = @userGuid and u.DATAAREAID = @dataareaID) pug 
	on p.GUID = pug.GUID
	where not Exists(
		Select 'x' from USERPERMISSION up
		where up.UserGUID = @userGuid and up.DATAAREAID = @dataareaID and up.PermissionGUID = p.GUID)
) ps
Inner Join PERMISSIONGROUP pg on ps.PermissionGroupGUID = pg.GUID and ps.DATAAREAID = pg.DATAAREAID
left outer join PERMISSIONSLOCALIZATION pl1 on pl1.PermissionGUID = ps.GUID and ps.DATAAREAID = pl1.DATAAREAID and pl1.Locale = @baseLanguage
left outer join PERMISSIONSLOCALIZATION pl2 on pl2.PermissionGUID = ps.GUID and ps.DATAAREAID = pl2.DATAAREAID and pl2.Locale = @language
left outer join PERMISSIONGROUPLOCALIZATION pg1 on pg1.GUID = pg.GUID and pg.DATAAREAID = pg1.DATAAREAID and pg1.Locale = @baseLanguage
left outer join PERMISSIONGROUPLOCALIZATION pg2 on pg2.GUID = pg.GUID and pg.DATAAREAID = pg2.DATAAREAID and pg2.Locale = @language
Order By PermissionGroupName,SortCode

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_GrantGroupPermission_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_GrantGroupPermission_1_0]

GO

create procedure dbo.spSECURITY_GrantGroupPermission_1_0
(@GroupGUID uniqueidentifier,@dataareaID nvarchar(10),@PermissionGUID uniqueidentifier)
as

set nocount on

declare @userGUID uniqueidentifier

insert into USERGROUPPERMISSIONS (GUID,UserGroupGUID,PermissionGUID,DATAAREAID)
values (NEWID(),@GroupGUID,@PermissionGUID,@dataareaID)

-- We need to invalidate all user profiles for users that are in this group
-- so that the users will get a new permission vector.

Declare usersInGroup Cursor For
select GUID from vSECURITY_UsersInGroup_1_0 where UserGroupGUID = @GroupGUID and DATAAREAID = @dataareaID

Open usersInGroup
Fetch usersInGroup Into @userGUID

WHILE @@FETCH_STATUS = 0
  Begin
	  update USERS Set LocalProfileHash = ''
	  where GUID = @userGUID and DATAAREAID = @dataareaID

	  Fetch usersInGroup Into @userGUID
  End
		
Close usersInGroup
Deallocate usersInGroup

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_AddPermission_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_AddPermission_1_0]

GO

create procedure dbo.spSECURITY_AddPermission_1_0
(@dataAreaID nvarchar(10),
 @GUID uniqueidentifier,
 @Description nvarchar(100),
 @PermissionGroupGUID uniqueidentifier,
 @SortCode nvarchar(8),
 @PermissionCode varchar(80),
 @CodeIsEncrypted bit)
as

set nocount on

declare @userGroupGUID uniqueidentifier

if not exists(select 'x' from PERMISSIONS where GUID = @GUID and DATAAREAID = @dataAreaID)
  begin

	Insert into PERMISSIONS (DATAAREAID,GUID,Description,PermissionGroupGUID,SortCode,PermissionCode,CodeIsEncrypted)
	values (@dataAreaID,@GUID,@Description,@PermissionGroupGUID,@SortCode,@PermissionCode,@CodeIsEncrypted)

	-- When inserting a new permission to the system then all groups marked as admin groups
	-- shall by default get the new permission
	declare adminGroups Cursor For
	select GUID from USERGROUPS where IsAdminGroup = 1 and DATAAREAID = @dataAreaID

	open adminGroups
	fetch adminGroups Into @userGroupGUID

	while @@FETCH_STATUS = 0
	  begin
		  exec spSECURITY_GrantGroupPermission_1_0 @userGroupGUID,@dataAreaID,@GUID

		  fetch adminGroups Into @userGroupGUID
	  end
		
	close adminGroups
	deallocate adminGroups
  end
else
  begin
	update PERMISSIONS set SortCode = @SortCode, Description = @Description,PermissionGroupGUID = @PermissionGroupGUID
	where GUID = @GUID and DATAAREAID = @dataAreaID
  end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_AddPermissionLocalization_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_AddPermissionLocalization_1_0]

GO

create procedure dbo.spSECURITY_AddPermissionLocalization_1_0
(@locale nvarchar(10),
 @GUID uniqueidentifier,
 @Description nvarchar(100),
 @dataAreaID nvarchar(10))
as

set nocount on

if not exists(select 'x' from PERMISSIONSLOCALIZATION where PermissionGUID = @GUID and DATAAREAID = @dataAreaID and Locale = @locale)
  begin

	Insert into PERMISSIONSLOCALIZATION (Locale,PermissionGUID,Description,DATAAREAID)
	values (@locale,@GUID,@Description,@dataAreaID)
	
  end
else
  begin
	update PERMISSIONSLOCALIZATION set Description = @Description
	where PermissionGUID = @GUID and DATAAREAID = @dataAreaID and Locale = @locale
  end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_AddPermissionGroupLocalization_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_AddPermissionGroupLocalization_1_0]

GO

create procedure dbo.spSECURITY_AddPermissionGroupLocalization_1_0
(@locale nvarchar(10),
 @GUID uniqueidentifier,
 @Name nvarchar(50),
 @dataAreaID nvarchar(10))
as

set nocount on

if not exists(select 'x' from PERMISSIONGROUPLOCALIZATION where GUID = @GUID and DATAAREAID = @dataAreaID and Locale = @locale)
  begin

	Insert into PERMISSIONGROUPLOCALIZATION (Locale,GUID,Name,DATAAREAID)
	values (@locale,@GUID,@Name,@dataAreaID)
	
  end
else
  begin
	update PERMISSIONGROUPLOCALIZATION set Name = @Name
	where GUID = @GUID and DATAAREAID = @dataAreaID and Locale = @locale
  end

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spMAINT_AddSystemSetting_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spMAINT_AddSystemSetting_1_0]

GO

create procedure dbo.spMAINT_AddSystemSetting_1_0
(@dataAreaID nvarchar(10),
 @GUID uniqueidentifier,
 @Name nvarchar(30),
 @Value nvarchar(30),
 @Description nvarchar(80),
 @UserCanOverride bit,
 @Internal bit,
 @Type uniqueidentifier)
as

set nocount on

declare @userGroupGUID uniqueidentifier

if not exists(select 'x' from SYSTEMSETTINGS where GUID = @GUID)
  begin

	Insert into SYSTEMSETTINGS (DATAAREAID,GUID,[Name],[Value],[Description],UserCanOverride,Internal,Type)
	values (@dataAreaID,@GUID,@Name,@Value,@Description,@UserCanOverride,@Internal,@Type)

  end
else
  begin
	update SYSTEMSETTINGS set 
	[Name] = @Name, 
	[Description] = @Description,
	UserCanOverride = @UserCanOverride,
	Internal = @Internal,
	Type = @Type
	where GUID = @GUID and DATAAREAID = @dataAreaID
  end

 -- When inserting a new setting to the system then all users in the systems will have
 -- to have their profile invalidated.
 Update USERS Set LocalProfileHash = '',LastChangeTime = GETDATE() where DATAAREAID = @dataAreaID 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_SetContext_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_SetContext_1_0]

GO

create procedure dbo.spSECURITY_SetContext_1_0
(@UserGUID uniqueidentifier)
as

set nocount on

declare @bin varbinary(128)

set @bin = CAST(@UserGUID as varbinary(128))
Set CONTEXT_INFO @bin

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_Login_1_1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_Login_1_1]

GO

create procedure [dbo].[spSECURITY_Login_1_1]
(@DATAAREAID nvarchar(10),@Login nvarchar(250),@PasswordHash nvarchar(250),@LocalProfileHash nvarchar(250),@Success bit out,@IsActiveDirectoryUser bit out,@UserGUID uniqueidentifier out, @NeedPasswordChange bit out, @PasswordExpiresDays int out, @IsLockedOut bit out, @ReturnedPermissions bit out,@Domain nvarchar(50) out,@adOffLinePassed bit out,@isServerUser bit out)
as

set nocount on

declare @serverProfileHash nvarchar(250)
declare @serverPasswordHash nvarchar(250)
declare @serverLogin nvarchar(250)
declare @isDomainUser bit
declare @loginSuccess bit
declare @expires Datetime
declare @passwordGracePeriod int
declare @passwordExpiresDelta int
declare @LockOutCounter int
declare @LockOutThreshold int
declare @bin varbinary(128)
declare @activeDirectoryEnabled int
declare @passwordNeverExpires bit

Set @serverPasswordHash = NULL
Set @serverLogin = NULL
Set @UserGUID = NULL
Set @PasswordExpiresDays = 0
Set @IsLockedOut = 0
set @ReturnedPermissions = 0

Select @LockOutThreshold = [Value] from 
SYSTEMSETTINGS where GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}' and DATAAREAID = @DATAAREAID

Select @activeDirectoryEnabled = [Value] from 
SYSTEMSETTINGS where GUID = '{be5cd3f0-69c5-11db-bd13-0800200c9a66}' and DATAAREAID = @DATAAREAID



select top 1 @serverProfileHash = LocalProfileHash,
	@serverLogin = Login,
	@serverPasswordHash = PasswordHash, 
	@isDomainUser = IsDomainUser,
	@UserGUID = GUID,
	@NeedPasswordChange = NeedPasswordChange,
	@expires = ExpiresDate,
	@LockOutCounter = LockOutCounter,
	@isServerUser = IsServerUser
from vSECURITY_AllUsers_1_0 where DATAAREAID = @DATAAREAID and Login = @Login

if @serverLogin = @Login and @isDomainUser = 1 and (@LockOutCounter < @LockOutThreshold) and @activeDirectoryEnabled = 1
  begin
		-- Support for Active directory login
		Select @Domain = Value from SYSTEMSETTINGS
		where GUID = '{e489e620-a91a-11de-8a39-0800200c9a66}' and DATAAREAID = @DATAAREAID
		
		Set @IsActiveDirectoryUser = 1
		Set @loginSuccess = 1
		
		-- Support for Off line active directory login.
		if @serverPasswordHash = @PasswordHash
			Set @adOffLinePassed = 1
		else
			Set @adOffLinePassed = 0
  end
else
  begin
	Set @IsActiveDirectoryUser = 0

	if @serverPasswordHash = @PasswordHash and @serverLogin = @Login
	  begin
	if(@LockOutCounter < @LockOutThreshold)
	  begin

			Set @loginSuccess = 1
	 
		if(@LockOutCounter > 0)
		begin
		  update USERS Set LockOutCounter = 0,LastChangeTime = GETDATE()
		  where GUID = @UserGUID and DATAAREAID = @DATAAREAID
		end
	  end
		else
	  begin
		-- Login was not successful because the user has been locked out
		Set @loginSuccess = 0
		Set @IsLockedOut  = 1
	  end
	  end
	else
	  begin
		Set @loginSuccess = 0

		-- We need to count the failed login attempts unless if it is the admin then we do not count
		if @Login <> 'admin' and @isServerUser = 0
		begin
			update USERS Set LockOutCounter = (@LockOutCounter+1),LastChangeTime = GETDATE()
			where GUID = @UserGUID and DATAAREAID = @DATAAREAID
		end
	  end
	end

-- Set the context info of the connection to the user UUID for auditing
-- purposes.
set @bin = CAST(@UserGUID as varbinary(128))
Set CONTEXT_INFO @bin
  
Set @passwordExpiresDelta = DATEDIFF ( day , @expires , GetDate() )
	
select @passwordNeverExpires = (case when (Value = '0') then 1 else 0 end) from SYSTEMSETTINGS 
where GUID = '7CB84D26-B28B-4086-8DCF-646F68CEF956' and DATAAREAID = @DATAAREAID

if @passwordExpiresDelta >= 0 and @isServerUser = 0 and @passwordNeverExpires = 0
	begin
		Set @NeedPasswordChange = 1
	end
else
	begin
	Select @passwordGracePeriod = [Value] from 
	SYSTEMSETTINGS where GUID = '{CF221FB0-C4B6-4574-9739-EF227D0B06E8}' and DATAAREAID = @DATAAREAID

	if (-@passwordExpiresDelta) <= @passwordGracePeriod
	begin
		Set @PasswordExpiresDays = -@passwordExpiresDelta
	end
		end

select @serverProfileHash = LocalProfileHash from vSECURITY_AllUsers_1_0 where Login = @Login and DATAAREAID = @DATAAREAID
		
	if @serverProfileHash <> @LocalProfileHash or @serverProfileHash = ''
	begin
	-- We need to send a new local profile

	set @ReturnedPermissions = 1

	exec spSECURITY_GetProfile_1_0 @Login,@PasswordHash,@DATAAREAID
	end

Set @Success = @loginSuccess

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_Login_1_2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_Login_1_2]

GO

CREATE PROCEDURE [dbo].[spSECURITY_Login_1_2]
(
	@DATAAREAID NVARCHAR(10),
	@Login NVARCHAR(250),
	@PasswordHash NVARCHAR(250),
	@LocalProfileHash NVARCHAR(250),
	@Success BIT OUT,
	@IsActiveDirectoryUser BIT OUT,
	@UserGUID UNIQUEIDENTIFIER OUT, 
	@NeedPasswordChange BIT OUT, 
	@PasswordExpiresDays INT OUT, 
	@IsLockedOut BIT OUT, 
	@LoginDisabled BIT OUT, 
	@ReturnedPermissions BIT OUT,
	@Domain NVARCHAR(50) OUT,
	@adOffLinePassed BIT OUT,
	@isServerUser BIT OUT
)
AS

SET NOCOUNT ON

DECLARE @serverProfileHash NVARCHAR(250)
DECLARE @serverPasswordHash NVARCHAR(250)
DECLARE @serverLogin NVARCHAR(250)
DECLARE @isDomainUser BIT
DECLARE @loginSuccess BIT
DECLARE @expires DATETIME
DECLARE @passwordGracePeriod INT
DECLARE @passwordExpiresDelta INT
DECLARE @LockOutCounter INT
DECLARE @LockOutThreshold INT
DECLARE @bin VARBINARY(128)
DECLARE @activeDirectoryEnabled INT
DECLARE @passwordNeverExpires BIT

SET @serverPasswordHash = NULL
SET @serverLogin = NULL
SET @UserGUID = NULL
SET @PasswordExpiresDays = 0
SET @IsLockedOut = 0
SET @LoginDisabled = 0
SET @ReturnedPermissions = 0

--Quick exit if no username was provided
IF @Login IS NULL OR @Login = ''
BEGIN
	SET @Success = 0
	SET @IsActiveDirectoryUser = 0
	SET @UserGUID = (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) -- 00000000-0000-0000-0000-000000000000
	SET @NeedPasswordChange = 0
	SET @isServerUser = 0

	RETURN
END

SELECT @LockOutThreshold = [Value] 
FROM SYSTEMSETTINGS 
WHERE [GUID] = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}' AND DATAAREAID = @DATAAREAID

SELECT @activeDirectoryEnabled = [Value] 
FROM SYSTEMSETTINGS 
WHERE [GUID] = '{be5cd3f0-69c5-11db-bd13-0800200c9a66}' AND DATAAREAID = @DATAAREAID


SELECT TOP 1 @serverProfileHash = LocalProfileHash,
			@serverLogin = [Login],
			@serverPasswordHash = PasswordHash, 
			@isDomainUser = IsDomainUser,
			@UserGUID = [GUID],
			@NeedPasswordChange = NeedPasswordChange,
			@expires = ExpiresDate,
			@LockOutCounter = LockOutCounter,
			@isServerUser = IsServerUser
FROM vSECURITY_AllUsers_1_0 
WHERE DATAAREAID = @DATAAREAID AND [Login] = @Login

--Quick exit if the user does not exist in the database or it will fail at SET CONTEXT_INFO
IF @serverLogin IS NULL OR @serverLogin = ''
BEGIN
	SET @Success = 0
	SET @IsActiveDirectoryUser = 0
	SET @UserGUID = (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) -- 00000000-0000-0000-0000-000000000000
	SET @NeedPasswordChange = 0
	SET @isServerUser = 0

	RETURN
END

IF @serverLogin = @Login AND @isDomainUser = 1 AND (@LockOutCounter < @LockOutThreshold) AND @activeDirectoryEnabled = 1
BEGIN
	-- Support for Active directory login
	SELECT @Domain = [Value] 
	FROM SYSTEMSETTINGS
	WHERE [GUID] = '{e489e620-a91a-11de-8a39-0800200c9a66}' AND DATAAREAID = @DATAAREAID
		
	SET @IsActiveDirectoryUser = 1
	SET @loginSuccess = 1
		
	-- Support for Off line active directory login.
	IF @serverPasswordHash = @PasswordHash
		SET @adOffLinePassed = 1
	ELSE
		SET @adOffLinePassed = 0
END
ELSE
BEGIN
	SET @IsActiveDirectoryUser = 0

	-- Login was not successful because the user has been locked out
	IF(@LockOutCounter >= @LockOutThreshold)
	BEGIN
		SET @loginSuccess = 0
		SET @IsLockedOut  = 1
		IF(@LockOutCounter = 1000)
		BEGIN
			SET @LoginDisabled = 1
		END
	END

	IF @serverPasswordHash = @PasswordHash AND @serverLogin = @Login
	BEGIN
		IF(@LockOutCounter < @LockOutThreshold)
		BEGIN
			SET @loginSuccess = 1
	 
			IF(@LockOutCounter > 0)
			BEGIN
			  UPDATE USERS SET LockOutCounter = 0,
								LastChangeTime = GETDATE()
			  WHERE [GUID] = @UserGUID and DATAAREAID = @DATAAREAID
			END
		END
	END
	ELSE
	BEGIN
		SET @loginSuccess = 0

		-- We need to count the failed login attempts unless if it is the admin then we do not count
		IF @Login <> 'admin' AND @isServerUser = 0 AND @LockOutCounter <> 1000
		BEGIN
			UPDATE USERS SET LockOutCounter = (@LockOutCounter+1),
							 LastChangeTime = GETDATE()
			WHERE [GUID] = @UserGUID AND DATAAREAID = @DATAAREAID

			IF @LockOutCounter+1 >= @LockOutThreshold
			BEGIN
				SET @IsLockedOut  = 1
			END
		END
	END
END

-- Set the context info of the connection to the user UUID for auditing
-- purposes.
SET @bin = CAST(@UserGUID AS VARBINARY(128))
SET CONTEXT_INFO @bin
  
SET @passwordExpiresDelta = DATEDIFF(day, @expires, GETDATE())
	
SELECT @passwordNeverExpires = (CASE WHEN ([Value] = '0') THEN 1 ELSE 0 END) 
FROM SYSTEMSETTINGS 
WHERE [GUID] = '7CB84D26-B28B-4086-8DCF-646F68CEF956' AND DATAAREAID = @DATAAREAID

IF @passwordExpiresDelta >= 0 AND @isServerUser = 0 AND @passwordNeverExpires = 0
BEGIN
	SET @NeedPasswordChange = 1
END
ELSE
BEGIN
	SELECT @passwordGracePeriod = [Value] 
	FROM SYSTEMSETTINGS 
	WHERE [GUID] = '{CF221FB0-C4B6-4574-9739-EF227D0B06E8}' AND DATAAREAID = @DATAAREAID

	IF (-@passwordExpiresDelta) <= @passwordGracePeriod
	BEGIN
		SET @PasswordExpiresDays = -@passwordExpiresDelta
	END
END
		
IF @serverProfileHash <> @LocalProfileHash OR @serverProfileHash = ''
BEGIN
	-- We need to send a new local profile
	SET @ReturnedPermissions = 1

	EXEC spSECURITY_GetProfile_1_0 @Login, @serverPasswordHash, @DATAAREAID
END

SET @Success = @loginSuccess
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_Login_1_3]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_Login_1_3]

GO

CREATE PROCEDURE [dbo].[spSECURITY_Login_1_3]
(
	@DATAAREAID NVARCHAR(10),
	@Login NVARCHAR(250),
	@PasswordHash NVARCHAR(250),
	@LocalProfileHash NVARCHAR(250),
	@Success BIT OUT,
	@IsActiveDirectoryUser BIT OUT,
	@UserGUID UNIQUEIDENTIFIER OUT, 
	@NeedPasswordChange BIT OUT, 
	@PasswordExpiresDays INT OUT, 
	@IsLockedOut BIT OUT, 
	@LoginDisabled BIT OUT, 
	@ReturnedPermissions BIT OUT,
	@Domain NVARCHAR(50) OUT,
	@adOffLinePassed BIT OUT,
	@isServerUser BIT OUT,
	@staffID NVARCHAR(20) OUT
)
AS

SET NOCOUNT ON

DECLARE @serverProfileHash NVARCHAR(250)
DECLARE @serverPasswordHash NVARCHAR(250)
DECLARE @serverLogin NVARCHAR(250)
DECLARE @isDomainUser BIT
DECLARE @loginSuccess BIT
DECLARE @expires DATETIME
DECLARE @passwordGracePeriod INT
DECLARE @passwordExpiresDelta INT
DECLARE @LockOutCounter INT
DECLARE @LockOutThreshold INT
DECLARE @bin VARBINARY(128)
DECLARE @activeDirectoryEnabled INT
DECLARE @passwordNeverExpires BIT

SET @serverPasswordHash = NULL
SET @serverLogin = NULL
SET @UserGUID = NULL
SET @PasswordExpiresDays = 0
SET @IsLockedOut = 0
SET @LoginDisabled = 0
SET @ReturnedPermissions = 0

--Quick exit if no username was provided
IF @Login IS NULL OR @Login = ''
BEGIN
	SET @Success = 0
	SET @IsActiveDirectoryUser = 0
	SET @UserGUID = (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) -- 00000000-0000-0000-0000-000000000000
	SET @NeedPasswordChange = 0
	SET @isServerUser = 0

	RETURN
END

SELECT @LockOutThreshold = [Value] 
FROM SYSTEMSETTINGS 
WHERE [GUID] = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}' AND DATAAREAID = @DATAAREAID

SELECT @activeDirectoryEnabled = [Value] 
FROM SYSTEMSETTINGS 
WHERE [GUID] = '{be5cd3f0-69c5-11db-bd13-0800200c9a66}' AND DATAAREAID = @DATAAREAID


SELECT TOP 1 @serverProfileHash = LocalProfileHash,
			@serverLogin = [Login],
			@serverPasswordHash = PasswordHash, 
			@isDomainUser = IsDomainUser,
			@UserGUID = [GUID],
			@NeedPasswordChange = NeedPasswordChange,
			@expires = ExpiresDate,
			@LockOutCounter = LockOutCounter,
			@isServerUser = IsServerUser,
			@staffID = STAFFID
FROM vSECURITY_AllUsers_1_0 
WHERE DATAAREAID = @DATAAREAID AND [Login] = @Login

--Quick exit if the user does not exist in the database or it will fail at SET CONTEXT_INFO
IF @serverLogin IS NULL OR @serverLogin = ''
BEGIN
	SET @Success = 0
	SET @IsActiveDirectoryUser = 0
	SET @UserGUID = (SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)) -- 00000000-0000-0000-0000-000000000000
	SET @NeedPasswordChange = 0
	SET @isServerUser = 0

	RETURN
END

IF @serverLogin = @Login AND @isDomainUser = 1 AND (@LockOutCounter < @LockOutThreshold) AND @activeDirectoryEnabled = 1
BEGIN
	-- Support for Active directory login
	SELECT @Domain = [Value] 
	FROM SYSTEMSETTINGS
	WHERE [GUID] = '{e489e620-a91a-11de-8a39-0800200c9a66}' AND DATAAREAID = @DATAAREAID
		
	SET @IsActiveDirectoryUser = 1
	SET @loginSuccess = 1
		
	-- Support for Off line active directory login.
	IF @serverPasswordHash = @PasswordHash
		SET @adOffLinePassed = 1
	ELSE
		SET @adOffLinePassed = 0
END
ELSE
BEGIN
	SET @IsActiveDirectoryUser = 0

	-- Login was not successful because the user has been locked out
	IF(@LockOutCounter >= @LockOutThreshold)
	BEGIN
		SET @loginSuccess = 0
		SET @IsLockedOut  = 1
		IF(@LockOutCounter = 1000)
		BEGIN
			SET @LoginDisabled = 1
		END
	END

	IF @serverPasswordHash = @PasswordHash AND @serverLogin = @Login
	BEGIN
		IF(@LockOutCounter < @LockOutThreshold)
		BEGIN
			SET @loginSuccess = 1
	 
			IF(@LockOutCounter > 0)
			BEGIN
			  UPDATE USERS SET LockOutCounter = 0,
								LastChangeTime = GETDATE()
			  WHERE [GUID] = @UserGUID and DATAAREAID = @DATAAREAID
			END
		END
	END
	ELSE
	BEGIN
		SET @loginSuccess = 0

		-- We need to count the failed login attempts unless if it is the admin then we do not count
		IF @Login <> 'admin' AND @isServerUser = 0 AND @LockOutCounter <> 1000
		BEGIN
			UPDATE USERS SET LockOutCounter = (@LockOutCounter+1),
							 LastChangeTime = GETDATE()
			WHERE [GUID] = @UserGUID AND DATAAREAID = @DATAAREAID

			IF @LockOutCounter+1 >= @LockOutThreshold
			BEGIN
				SET @IsLockedOut  = 1
			END
		END
	END
END

-- Set the context info of the connection to the user UUID for auditing
-- purposes.
SET @bin = CAST(@UserGUID AS VARBINARY(128))
SET CONTEXT_INFO @bin
  
SET @passwordExpiresDelta = DATEDIFF(day, @expires, GETDATE())
	
SELECT @passwordNeverExpires = (CASE WHEN ([Value] = '0') THEN 1 ELSE 0 END) 
FROM SYSTEMSETTINGS 
WHERE [GUID] = '7CB84D26-B28B-4086-8DCF-646F68CEF956' AND DATAAREAID = @DATAAREAID

IF @passwordExpiresDelta >= 0 AND @isServerUser = 0 AND @passwordNeverExpires = 0
BEGIN
	SET @NeedPasswordChange = 1
END
ELSE
BEGIN
	SELECT @passwordGracePeriod = [Value] 
	FROM SYSTEMSETTINGS 
	WHERE [GUID] = '{CF221FB0-C4B6-4574-9739-EF227D0B06E8}' AND DATAAREAID = @DATAAREAID

	IF (-@passwordExpiresDelta) <= @passwordGracePeriod
	BEGIN
		SET @PasswordExpiresDays = -@passwordExpiresDelta
	END
END
		
IF @serverProfileHash <> @LocalProfileHash OR @serverProfileHash = ''
BEGIN
	-- We need to send a new local profile
	SET @ReturnedPermissions = 1

	EXEC spSECURITY_GetProfile_1_0 @Login, @serverPasswordHash, @DATAAREAID
END

SET @Success = @loginSuccess
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_LoginFromTokenLogin_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_LoginFromTokenLogin_1_0]

GO

create procedure dbo.spSECURITY_LoginFromTokenLogin_1_0
(@DATAAREAID nvarchar(10),@TokenLoginHash nvarchar(250), @tokenIsUser bit out,@validToken bit out, @login nvarchar(250) out, @passwordHash nvarchar(250) out)
as

set nocount on

if Exists(select 'x' from USERS where Login = @TokenLoginHash)
begin
	set @tokenIsUser = 1
	set @validToken = 0
end
else
begin
	set @tokenIsUser = 0

	select @login = u.Login, @passwordHash = u.PasswordHash from USERLOGINTOKENS ut
	left outer join USERS u on u.GUID = ut.USERGUID and u.DATAAREAID = ut.DATAAREAID
	where ut.[HASH] = @TokenLoginHash and u.DATAAREAID = @DATAAREAID
	
	if @login is NULL
	begin
		set @validToken = 0
	end
	else
	begin
		set @validToken = 1
	end
end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_Login_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_Login_1_0]

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vSECURITY_AllUserGroups_1_0]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vSECURITY_AllUserGroups_1_0]

GO

CREATE view dbo.vSECURITY_AllUserGroups_1_0

as

select GUID,DATAAREAID,Name,IsAdminGroup,Locked
from USERGROUPS
where Deleted = 0

GO
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[GETCONVERSIONS]') )
DROP FUNCTION [DBO].[GETCONVERSIONS]
GO

CREATE FUNCTION [DBO].[GETCONVERSIONS] (@ITEMID VARCHAR( 8000 ))
RETURNS TABLE 
AS
RETURN

SELECT  [FROMUNIT]
	  ,[TOUNIT]
	  ,[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]
  WHERE ITEMID = @ITEMID
  UNION ALL 
  SELECT [TOUNIT]
	  ,[FROMUNIT]
	  ,1/[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]
   WHERE ITEMID = @ITEMID

GO
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[GETSEQUENCEASINDEX]') )
DROP FUNCTION [DBO].[GETSEQUENCEASINDEX]
GO

CREATE FUNCTION [dbo].[GETSEQUENCEASINDEX] (@MASTERID UNIQUEIDENTIFIER)
RETURNS TABLE 
AS
RETURN

SELECT ROW_NUMBER() OVER (ORDER BY RETAILITEMID, SEQUENCE) AS DIMENSIONSEQUENCE, 
ID DIMENSIONID,
RETAILITEMID,
SEQUENCE
from RETAILITEMDIMENSION t where t.RETAILITEMID = @MASTERID 

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[DBO].[VINVENTSUM]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [DBO].[VINVENTSUM]

GO
CREATE VIEW [dbo].[VINVENTSUM]

AS 
 
SELECT
	   ITEMID,
	   STOREID, 
	   SUM(QUANTITY) AS QUANTITY
	   FROM 
	   (
SELECT 
			 ITEMID,
			 STOREID,
			 QUANTITY  
			 FROM 
					INVENTSUM
	   UNION ALL
	   SELECT 
			 ITEMID,
			 STOREID, 
			 SUM(QUANTITY) AS QUANTITY
			 FROM 
			 (
			 SELECT 
					TS.ITEMID,
					TS.STORE STOREID, 
					TS.UNITQTY                 
					* 
					CASE 
						   WHEN I.SALESUNITID = I.INVENTORYUNITID THEN 1
						   WHEN UCTOINVENTORY.FACTOR  IS NOT NULL THEN 1/UCTOINVENTORY.FACTOR
						   WHEN UCTOINVENTORYGEN.FACTOR  IS NOT NULL THEN 1/UCTOINVENTORYGEN.FACTOR
						   ELSE 1
					END
					AS QUANTITY
					FROM 
						   RBOTRANSACTIONSALESTRANS TS
						   JOIN RBOTRANSACTIONTABLE T ON 
								  TS.TRANSACTIONID = T.TRANSACTIONID 
								  AND T.STORE = TS.STORE 
								  AND T.TERMINAL = TS.TERMINALID 
						   JOIN RETAILITEM I ON TS.ITEMID = I.ITEMID      
						   LEFT JOIN  INVENTTRANSREASON ITR ON ITR.REASONID = TS.REASONCODEID
						   LEFT JOIN RBOSTATEMENTTABLE ST ON ST.STATEMENTID = TS.STATEMENTID
						   LEFT JOIN 
						   (
								  SELECT  
										FROMUNIT,
										TOUNIT,
										FACTOR,
										ROUNDOFF,
										ITEMID,
										DATAAREAID,
										MARKUP
										FROM DBO.UNITCONVERT
								  UNION ALL 
								  SELECT 
										TOUNIT,
										FROMUNIT,
										1/FACTOR,
										ROUNDOFF,
										ITEMID,
										DATAAREAID,
										MARKUP
										FROM DBO.UNITCONVERT
								  ) UCTOINVENTORY ON 
										(
											   I.SALESUNITID = UCTOINVENTORY.FROMUNIT 
											   AND UCTOINVENTORY.TOUNIT = I.INVENTORYUNITID
										) 
										AND TS.ITEMID = UCTOINVENTORY.ITEMID                                
						   LEFT JOIN GETCONVERSIONS('') UCTOINVENTORYGEN ON 
								  (
										I.SALESUNITID = UCTOINVENTORYGEN.FROMUNIT 
										AND UCTOINVENTORYGEN.TOUNIT = I.INVENTORYUNITID
								  ) 
					WHERE 
					(TS.STATEMENTID =  '0' OR (ST.STATEMENTID IS NOT NULL AND ST.POSTED = 0))
					AND T.ENTRYSTATUS = 0 
					AND TS.TRANSACTIONSTATUS = 0                   
					AND (ISNULL(ITR.ACTION, '0') = '0')
					AND I.ITEMTYPE <> 2
			 ) INTERNALSOURCE
	   GROUP BY  
	   ITEMID,
	   STOREID
) SRC
GROUP BY  
ITEMID,
STOREID      

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_GetGroupPermissions_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_GetGroupPermissions_1_0]

GO

create procedure dbo.spSECURITY_GetGroupPermissions_1_0
(@groupGuid uniqueidentifier,@baseLanguage nvarchar(10),@language nvarchar(10),@dataareaID nvarchar(10)) 
as

set nocount on

Select ps.GUID,Coalesce(pl2.Description,pl1.Description,ps.Description) as Description,ps.HasPermission,Coalesce(pg2.Name,pg1.Name,pg.Name) as PermissionGroupName,ps.PermissionCode,ps.CodeIsEncrypted,ps.DATAAREAID from
(
	select p.GUID,p.Description,p.SortCode, p.PermissionGroupGUID,p.PermissionCode,p.CodeIsEncrypted,HasPermission = 1, p.DATAAREAID
	from   USERGROUPPERMISSIONS ugp INNER JOIN
		   PERMISSIONS p ON ugp.PermissionGUID = p.GUID and ugp.DATAAREAID = p.DATAAREAID
	where  ugp.UserGroupGUID = @groupGuid and ugp.DATAAREAID = @dataareaID
	
	union 
	
	select p.GUID, p.Description,p.SortCode,p.PermissionGroupGUID,p.PermissionCode,p.CodeIsEncrypted,HasPermission = 0,p.DATAAREAID
	from   PERMISSIONS p 
	where not Exists(
		Select 'x' from USERGROUPPERMISSIONS ugp 
		where ugp.UserGroupGUID = @groupGuid and ugp.DATAAREAID = @dataareaID and ugp.PermissionGUID = p.GUID) and p.DATAAREAID = @dataareaID
) ps
Inner Join PERMISSIONGROUP pg on ps.PermissionGroupGUID = pg.GUID and ps.DATAAREAID = pg.DATAAREAID
left outer join PERMISSIONSLOCALIZATION pl1 on pl1.PermissionGUID = ps.GUID and ps.DATAAREAID = pl1.DATAAREAID and pl1.Locale = @baseLanguage
left outer join PERMISSIONSLOCALIZATION pl2 on pl2.PermissionGUID = ps.GUID and ps.DATAAREAID = pl2.DATAAREAID and pl2.Locale = @language
left outer join PERMISSIONGROUPLOCALIZATION pg1 on pg1.GUID = pg.GUID and pg.DATAAREAID = pg1.DATAAREAID and pg1.Locale = @baseLanguage
left outer join PERMISSIONGROUPLOCALIZATION pg2 on pg2.GUID = pg.GUID and pg.DATAAREAID = pg2.DATAAREAID and pg2.Locale = @language
Order By PermissionGroupName,SortCode

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vLookup_NamePrefixes_1_0]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vLookup_NamePrefixes_1_0]

GO

create view vLookup_NamePrefixes_1_0

as 

Select GUID,Prefix,OriginalValue,Locked from NAMEPREFIXES
where Deleted = 0

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vLookup_Language_1_0]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vLookup_Language_1_0]

GO

create view vLookup_Language_1_0

as 

Select * from LANGUAGES

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vLookup_Countries_1_0]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vLookup_Countries_1_0]

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vLookup_Religion_1_0]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vLookup_Religion_1_0]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_SetDomainUserPasswordHash_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_SetDomainUserPasswordHash_1_0]

GO

create procedure dbo.spSECURITY_SetDomainUserPasswordHash_1_0
(@UserGUID uniqueidentifier,@dataareaID nvarchar(10),@newPasswordHash nvarchar(250))
as

set nocount on

update USERS Set PasswordHash = @newPasswordHash,LastChangeTime = GETDATE()
where GUID = @UserGUID and IsDomainUser = 1 and DATAAREAID = @dataareaID

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_SetLocalProfileHash_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_SetLocalProfileHash_1_0]

GO

create procedure dbo.spSECURITY_SetLocalProfileHash_1_0
(@Login nvarchar(250),@dataareaID nvarchar(10),@LocalProfileHash nvarchar(250))
as

set nocount on

  -- The server should call this procedure after Server and Client get
  -- a new local profile. The server may not expose this procedure to the client.

update USERS Set LocalProfileHash = @LocalProfileHash,LastChangeTime = GETDATE() 
where Login = @Login and Deleted = 0 and DATAAREAID = @dataareaID

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_DenyGroupPermission_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_DenyGroupPermission_1_0]

GO

create procedure dbo.spSECURITY_DenyGroupPermission_1_0
(@GroupGUID uniqueidentifier,@dataareaID nvarchar(10),@PermissionGUID uniqueidentifier)
as

set nocount on

declare @userGUID uniqueidentifier

-- Check if the Group is a Admin Group since it is not possible to remove permissions
-- from administrator groups.
if Exists(Select 'x' from USERGROUPS where GUID = @GroupGUID and DATAAREAID = @dataareaID and IsAdminGroup = 0)
begin

  delete from USERGROUPPERMISSIONS
  where UserGroupGUID = @GroupGUID and DATAAREAID = @dataareaID and PermissionGUID = @PermissionGUID


  -- We need to invalidate all user profiles for users that are in this group
  -- so that the users will get a new permission vector.

  declare usersInGroup Cursor For
  select GUID from vSECURITY_UsersInGroup_1_0 where UserGroupGUID = @GroupGUID and DATAAREAID = @dataareaID

  open usersInGroup
  fetch usersInGroup Into @userGUID

  while @@FETCH_STATUS = 0
	begin
		update USERS Set LocalProfileHash = '',LastChangeTime = GETDATE()
		where GUID = @userGUID and DATAAREAID = @dataareaID

		fetch usersInGroup Into @userGUID
	end
		
  close usersInGroup
  deallocate usersInGroup
end

GO

if exists (select * from dbo.sysobjects where id = OBJECT_ID(N'[dbo].[spSECURITY_DoesUserHavePermission_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_DoesUserHavePermission_1_0]

GO

create procedure dbo.spSECURITY_DoesUserHavePermission_1_0
(@Login nvarchar(250), @PasswordHash nvarchar(250), @dataareaID nvarchar(10), @permissionGUID varchar(60))
as

set nocount on

declare @userGUID uniqueidentifier

Select @userGUID = GUID from vSECURITY_AllUsers_1_0 where Login = @Login and PasswordHash = @PasswordHash and DATAAREAID = @dataareaID

	-- Codes that we are granted directly on the user
	select p.PermissionCode, p.CodeIsEncrypted, p.GUID
	from USERPERMISSION up inner join
	PERMISSIONS p on up.PermissionGUID = p.GUID
	where up.[Grant] = 1 and up.UserGUID = @userGUID and up.DATAAREAID = @dataareaID and p.PermissionCode = @permissionGUID

	union

	-- Codes that we are granted from a Groups
	select Distinct p.PermissionCode, p.CodeIsEncrypted, p.GUID
	from   vSECURITY_UsersInGroup_1_0 u inner join
	USERGROUPPERMISSIONS on u.UserGroupGUID = USERGROUPPERMISSIONS.UserGroupGUID and u.DATAAREAID = USERGROUPPERMISSIONS.DATAAREAID inner join
	dbo.PERMISSIONS p on dbo.USERGROUPPERMISSIONS.PermissionGUID = p.GUID and u.GUID = @userGUID and u.DATAAREAID = @dataareaID

	-- We need to strip out codes where we had exclusive deny on the user
	where u.DATAAREAID = @dataareaID and p.PermissionCode = @permissionGUID
	and not Exists(
		select pp.PermissionCode, pp.CodeIsEncrypted, pp.GUID
		from USERPERMISSION up inner join
		PERMISSIONS pp on up.PermissionGUID = pp.GUID and pp.DATAAREAID = pp.DATAAREAID
		where up.[Grant] = 0 and up.UserGUID = @userGUID and up.DATAAREAID = @dataareaID
		and pp.GUID = p.GUID)
		
GO

GO

if exists (select * from dbo.sysobjects where id = OBJECT_ID(N'[dbo].[spSECURITY_VerifyCredentials_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_VerifyCredentials_1_0]

GO

create procedure dbo.spSECURITY_VerifyCredentials_1_0
(@Login nvarchar(250), @PasswordHash nvarchar(250), @DataAreaID nvarchar(10), @IsValid bit out,@IsLockedOut bit out)
as

set nocount on

declare @LockOutThreshold int
declare @LockOutCounter int
declare @LoadedPasswordHash nvarchar(250)

select top 1 @LockOutCounter = LockOutCounter, @LoadedPasswordHash = PasswordHash
from vSECURITY_AllUsers_1_0 where DATAAREAID = @DataAreaID and Login = @Login

Select @LockOutThreshold = [Value] from 
SYSTEMSETTINGS where GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}' and DATAAREAID = @DataAreaID

if(@LockOutCounter is NULL)
begin
	set @IsValid = 0
	set @IsLockedOut = 0
end
else 
begin
	if @LoadedPasswordHash = @PasswordHash and @LockOutCounter < @LockOutThreshold
	begin
		set @IsValid = 1
		
		if(@LockOutCounter > 0)
		begin
		  update USERS Set LockOutCounter = 0,LastChangeTime = GETDATE()
		  where Login = @Login and DATAAREAID = @DataAreaID
		end
		
		set @IsLockedOut = 0
	end
	else if @LoadedPasswordHash <> @PasswordHash
	begin
		set @IsValid = 0
	
		if @Login <> 'admin'
		begin
			update USERS Set LockOutCounter = (@LockOutCounter+1),LastChangeTime = GETDATE()
			where Login = @Login and DATAAREAID = @DataAreaID
		end
		
		set @IsLockedOut = 0
	end
	else
	begin
		set @IsValid = 0
		
		if @Login <> 'admin'
		begin
			update USERS Set LockOutCounter = (@LockOutCounter+1),LastChangeTime = GETDATE()
			where Login = @Login and DATAAREAID = @DataAreaID
		end
		
		set @IsLockedOut = 1
	end
end

GO

if exists (select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'spSECURITY_UserNeedsPasswordChange_1_0')
drop procedure [dbo].[spSECURITY_UserNeedsPasswordChange_1_0]

GO

CREATE PROCEDURE dbo.spSECURITY_UserNeedsPasswordChange_1_0
	(@userID uniqueidentifier,
	 @NeedPasswordChange bit out)
AS
BEGIN

	SET NOCOUNT ON;

	declare @isDomainUser bit
	declare @expires Datetime
	declare @passwordGracePeriod int
	declare @passwordExpiresDelta int
	declare @passwordNeverExpires bit

	select top 1 
		@isDomainUser = IsDomainUser,
		@expires = ExpiresDate,
		@NeedPasswordChange = NeedPasswordChange
	from USERS
	where GUID = @userID

	Set @passwordExpiresDelta = DATEDIFF ( day , @expires , GetDate() )

	select @passwordNeverExpires = (case when (Value = '0') then 1 else 0 end) from SYSTEMSETTINGS 
	where GUID = '7CB84D26-B28B-4086-8DCF-646F68CEF956' 

	if @passwordExpiresDelta >= 0 and @passwordNeverExpires = 0
	  begin
		  Set @NeedPasswordChange = 1
	  end

END
GO

if exists (select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'spSECURITY_LockUser_1_0')
drop procedure [dbo].[spSECURITY_LockUser_1_0]

GO

CREATE PROCEDURE dbo.spSECURITY_LockUser_1_0
	(@userID uniqueidentifier)
AS
BEGIN

	SET NOCOUNT ON;

	declare @lockoutThreshold int

	select
		@lockoutThreshold = CAST(Value as int)
	from SYSTEMSETTINGS
	where GUID = '6278EA02-CC60-4AD2-BEA6-88CD0A8312AB'

	update USERS
	set LockOutCounter = @lockoutThreshold
	where GUID = @userID
END
GO

-- ***************************************************************************************************

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spMAINT_GetSettingPair_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spMAINT_GetSettingPair_1_0]

GO

create procedure dbo.spMAINT_GetSettingPair_1_0
(@userGuid uniqueidentifier,@dataareaID nvarchar(10), @settingGuid uniqueidentifier, @typeGuid uniqueidentifier out, @userSettingExists bit out,@userValue nvarchar(50) out, @userValueLong nvarchar(MAX) out, @systemValue nvarchar(50) out)
as

set nocount on

if Exists(Select 'x' from USERSETTINGS where UserGUID = @userGuid and DATAAREAID = @dataareaID and SettingsGUID = @settingGuid)
  begin
	 Select @userValue = [Value], @typeGuid = [Type], @userValueLong = [LONGTEXT] from USERSETTINGS 
	 where UserGUID = @userGuid and DATAAREAID = @dataareaID and SettingsGUID = @settingGuid

	 set @userSettingExists = 1
  end 
else
  begin
	 set @userValue = ''
	 set @userSettingExists = 0
  end
  
 IF EXISTS(SELECT 'x' FROM SYSTEMSETTINGS WHERE GUID = @settingGuid and DATAAREAID = @dataareaID)
 BEGIN
	 Select @systemValue = [Value], @typeGuid = [Type] from SYSTEMSETTINGS where GUID = @settingGuid and DATAAREAID = @dataareaID
 END
 ELSE
 BEGIN
	SET @systemValue = ''
 END

GO

-- ***************************************************************************************************

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spMAINT_GetSystemSetting_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spMAINT_GetSystemSetting_1_0]

GO

create procedure dbo.spMAINT_GetSystemSetting_1_0
(@SettingGuid uniqueidentifier,@dataareaID nvarchar(10), @Type uniqueidentifier out, @Value nvarchar(50) out)
as

set nocount on

Select @Value = [Value], @Type = [Type] from SYSTEMSETTINGS where GUID = @SettingGuid and DATAAREAID = @dataareaID

GO

-- ***************************************************************************************************

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spMAINT_SetSystemSetting_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spMAINT_SetSystemSetting_1_0]

GO

create procedure dbo.spMAINT_SetSystemSetting_1_0
(@SettingGuid uniqueidentifier,@dataareaID nvarchar(10), @Type uniqueidentifier, @Value nvarchar(50))
as

set nocount on

update SYSTEMSETTINGS Set
	[Value] = @Value
where GUID = @SettingGuid and [Type] = @Type and DATAAREAID = @dataareaID

if(@Type = 'C79AE480-7EE1-11DB-9FE1-0800200C9A66')
begin
-- We need to invalidate all user profiles
update USERS Set LocalProfileHash = '',LastChangeTime = GETDATE() where DATAAREAID = @dataareaID
end

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[INSERT_INVENTTRANS]'))
begin
   drop trigger dbo.INSERT_INVENTTRANS
end

GO

create trigger INSERT_INVENTTRANS
on INVENTTRANS after insert as

;WITH cte AS
(
	SELECT inserted.ITEMID, inserted.STOREID, SUM(inserted.ADJUSTMENTININVENTORYUNIT) AS ADJUSTMENTININVENTORYUNIT FROM inserted GROUP BY inserted.ITEMID, inserted.STOREID
)
MERGE INVENTSUM AS INV
USING cte AS INS
ON INV.ITEMID = INS.ITEMID AND INV.STOREID = INS.STOREID
WHEN MATCHED THEN
	UPDATE SET QUANTITY = QUANTITY + INS.ADJUSTMENTININVENTORYUNIT
WHEN NOT MATCHED THEN
	INSERT ([ITEMID],[STOREID],[QUANTITY]) VALUES (INS.ITEMID, INS.STOREID, INS.ADJUSTMENTININVENTORYUNIT);
	
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spStatement_MarkTransactions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spStatement_MarkTransactions]

GO

create procedure dbo.spStatement_MarkTransactions
(@DATAAREAID nvarchar(20),
 @StatementID nvarchar(20),
 @StoreID nvarchar(20),
 @StartingDate DateTime,
 @EndDate DateTime)
as

set nocount on

update transTable 
Set transTable.STATEMENTID = @StatementID
from RBOTRANSACTIONTABLE transTable
Join RBOTERMINALTABLE terminalTable on 
	terminalTable.TERMINALID = transTable.TERMINAL and terminalTable.DATAAREAID = transTable.DATAAREAID
where 
transTable.DATAAREAID = @DATAAREAID and
transTable.STORE = @StoreID and 
transTable.TRANSDATE > @StartingDate and 
transTable.TRANSDATE < @EndDate and 
transTable.STATEMENTID = '' and
terminalTable.INCLUDEINSTATEMENT = 1

update paymentTable 
Set paymentTable.STATEMENTID = @StatementID
from RBOTRANSACTIONPAYMENTTRANS paymentTable
Join RBOTERMINALTABLE terminalTable on 
	terminalTable.TERMINALID = paymentTable.TERMINAL and terminalTable.DATAAREAID = paymentTable.DATAAREAID
where 
paymentTable.DATAAREAID = @DATAAREAID and
paymentTable.STORE = @StoreID and 
paymentTable.TRANSDATE > @StartingDate and 
paymentTable.TRANSDATE < @EndDate and 
paymentTable.STATEMENTID = '' and
terminalTable.INCLUDEINSTATEMENT = 1

update safeTenderTable 
Set safeTenderTable.STATEMENTID = @StatementID
from RBOTRANSACTIONSAFETENDERTRANS safeTenderTable
Join RBOTERMINALTABLE terminalTable on 
	terminalTable.TERMINALID = safeTenderTable.TERMINAL and terminalTable.DATAAREAID = safeTenderTable.DATAAREAID
where 
safeTenderTable.DATAAREAID = @DATAAREAID and
safeTenderTable.STORE = @StoreID and 
safeTenderTable.TRANSDATE > @StartingDate and 
safeTenderTable.TRANSDATE < @EndDate and 
safeTenderTable.STATEMENTID = '' and
terminalTable.INCLUDEINSTATEMENT = 1

update bankedTenderTable 
Set bankedTenderTable.STATEMENTID = @StatementID
from RBOTRANSACTIONBANKEDTENDE20338 bankedTenderTable
Join RBOTERMINALTABLE terminalTable on 
	terminalTable.TERMINALID = bankedTenderTable.TERMINAL and terminalTable.DATAAREAID = bankedTenderTable.DATAAREAID
where 
bankedTenderTable.DATAAREAID = @DATAAREAID and
bankedTenderTable.STORE = @StoreID and 
bankedTenderTable.TRANSDATE > @StartingDate and 
bankedTenderTable.TRANSDATE < @EndDate and 
bankedTenderTable.STATEMENTID = '' and
terminalTable.INCLUDEINSTATEMENT = 1

update tenderDeclarationTable 
Set tenderDeclarationTable.STATEMENTID = @StatementID
from RBOTRANSACTIONTENDERDECLA20165 tenderDeclarationTable
Join RBOTERMINALTABLE terminalTable on 
	terminalTable.TERMINALID = tenderDeclarationTable.TERMINAL and terminalTable.DATAAREAID = tenderDeclarationTable.DATAAREAID
where 
tenderDeclarationTable.DATAAREAID = @DATAAREAID and
tenderDeclarationTable.STORE = @StoreID and 
tenderDeclarationTable.TRANSDATE > @StartingDate and 
tenderDeclarationTable.TRANSDATE < @EndDate and 
tenderDeclarationTable.STATEMENTID = '' and
terminalTable.INCLUDEINSTATEMENT = 1

update salesTable 
Set salesTable.STATEMENTID = @StatementID
from RBOTRANSACTIONSALESTRANS salesTable
Join RBOTERMINALTABLE terminalTable on 
	terminalTable.TERMINALID = salesTable.TERMINALID and terminalTable.DATAAREAID = salesTable.DATAAREAID
where 
salesTable.DATAAREAID = @DATAAREAID and
salesTable.STORE = @StoreID and 
salesTable.TRANSDATE > @StartingDate and 
salesTable.TRANSDATE < @EndDate and 
salesTable.STATEMENTID = '0' and
terminalTable.INCLUDEINSTATEMENT = 1

GO


IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spLOYALTY_ExpirePoints]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spLOYALTY_ExpirePoints]
GO
CREATE PROCEDURE [dbo].[spLOYALTY_ExpirePoints]
@LoyaltyCustID as varchar(20), @CardNumber as varchar(20), @ToDate as dateTime, @DataAreaID as varchar(4), @UserID as uniqueidentifier

AS
BEGIN

DECLARE @v_CardNumber as varchar (20), @v_LineNum as int, @v_TransactionID as varchar(20), @v_ReceiptID as varchar(20), @v_RemainingPoints as numeric(28,12), @v_StoreID as varchar(20), @v_TerminalID as varchar(20), @v_LoyaltyCustID as varchar(20), @v_LoyaltySchemeID as varchar(20), @v_LoyaltyPointTransLineNum as numeric(28,12);
DECLARE @err as int, @v_cardType as int;

IF (@ToDate IS NULL) OR (@ToDate='1900-01-01 00:00:000')
	SET @ToDate = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()));		--set today if not defined

SET @v_cardType = (SELECT LOYALTYTENDER FROM RBOLOYALTYMSRCARDTABLE WHERE CARDNUMBER=@CardNumber AND DATAAREAID=@DataAreaID);

--TEST SET @v_cardType = 1;

IF @v_cardType IS NULL
	RETURN 1					--Loyalty card could not be found

--IF @v_cardType = 2
--	RETURN 3					--Loyalty card is no tender card - points could not be used

IF @v_cardType = 3
	RETURN 2					--Loyalty card is blocked

IF (@v_cardType IN (0, 2))	--Loyalty card type is card tender
BEGIN
	DECLARE Expired_cursor CURSOR FOR 
	SELECT CARDNUMBER, LINENUM, TRANSACTIONID, RECEIPTID, REMAININGPOINTS, STOREID, TERMINALID, LOYALTYCUSTID, LOYALTYSCHEMEID, LOYALTYPOINTTRANSLINENUM
	FROM RBOLOYALTYTRANS
	WHERE
	CARDNUMBER = @CardNumber
	and [TYPE] = 0					--points issuing entry type
	AND [STATUS] = 1					--open entry
	AND EXPIRATIONDATE < @ToDate	--expired
	AND ENTRYTYPE = 0;				--not voided
END

IF @v_cardType = 1					--Loyalty card type is contact tender
BEGIN
	DECLARE Expired_cursor CURSOR FOR 
	SELECT CARDNUMBER, LINENUM, TRANSACTIONID, RECEIPTID, REMAININGPOINTS, STOREID, TERMINALID, LOYALTYCUSTID, LOYALTYSCHEMEID, LOYALTYPOINTTRANSLINENUM
	FROM RBOLOYALTYTRANS
	WHERE
	LOYALTYCUSTID = @LoyaltyCustID
	and [TYPE] = 0					--points issuing entry type
	AND [STATUS] = 1					--open entry
	AND EXPIRATIONDATE < @ToDate	--expired
	AND ENTRYTYPE = 0;				--not voided
END

OPEN Expired_cursor;

FETCH NEXT FROM Expired_cursor 
INTO @v_CardNumber, @v_LineNum, @v_TransactionID, @v_ReceiptID, @v_RemainingPoints, @v_StoreID, @v_TerminalID, @v_LoyaltyCustID, @v_LoyaltySchemeID, @v_LoyaltyPointTransLineNum;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		--print 'CardNumber: ' + @v_CardNumber + ',linenum: ' + CONVERT(varchar,@v_LineNum);
		
		BEGIN TRAN
		
		INSERT INTO RBOLOYALTYTRANS(CARDNUMBER,LINENUM,TRANSACTIONID,RECEIPTID,POINTS,DATEOFISSUE,STOREID,TERMINALID,SEQUENCENUMBER,LOYALTYCUSTID,ENTRYTYPE,EXPIRATIONDATE,LOYALTYSCHEMEID,STATEMENTID,STATEMENTCODE,STAFFID,LOYALTYPOINTTRANSLINENUM,MODIFIEDDATE,MODIFIEDTIME,CREATEDDATE,CREATEDTIME,DATAAREAID,[TYPE],[STATUS],REMAININGPOINTS,MODIFIEDBY,CREATEDBY)
		VALUES(@v_CardNumber,(SELECT MAX(ISNULL(LINENUM,0))+1 FROM RBOLOYALTYTRANS WHERE CARDNUMBER=@v_CardNumber AND DATAAREAID=@DataAreaID),@v_TransactionID,@v_ReceiptID,-1*@v_RemainingPoints,GETDATE(),@v_StoreID,@v_TerminalID,(SELECT MAX(ISNULL(SEQUENCENUMBER,0))+1 FROM RBOLOYALTYTRANS WHERE DATAAREAID=@DataAreaID),@v_LoyaltyCustID,0,'1900-01-01 00:00:000',@v_LoyaltySchemeID,'','','',@v_LoyaltyPointTransLineNum,'1900-01-01 00:00:000',0,GETDATE(),CONVERT(int,current_timestamp),@DataAreaID,2,0,0,@UserID,@UserID);
		SET @err = @@ERROR
		IF @err != 0 GOTO HANDLE_ERROR
		
		UPDATE RBOLOYALTYTRANS
		SET [STATUS]=0, REMAININGPOINTS=0
		WHERE CARDNUMBER=@v_CardNumber AND LINENUM=@v_LineNum AND DATAAREAID=@DataAreaID;
		SET @err = @@ERROR
		IF @err != 0 GOTO HANDLE_ERROR
		
		COMMIT TRAN
		
		FETCH NEXT FROM Expired_cursor 
		INTO @v_CardNumber, @v_LineNum, @v_TransactionID, @v_ReceiptID, @v_RemainingPoints, @v_StoreID, @v_TerminalID, @v_LoyaltyCustID, @v_LoyaltySchemeID, @v_LoyaltyPointTransLineNum;
	END


CLOSE Expired_cursor;
DEALLOCATE Expired_cursor;

RETURN 0;

HANDLE_ERROR:
	BEGIN
		ROLLBACK TRAN;
		CLOSE Expired_cursor;
		DEALLOCATE Expired_cursor;
		RETURN 3;				--Insert/Update error
	END
END
GO

/*  Stored Procedure spLOYALTY_UpdateRemainingPoints  */

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spLOYALTY_UpdateRemainingPoints]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spLOYALTY_UpdateRemainingPoints]
GO
CREATE PROCEDURE [dbo].[spLOYALTY_UpdateRemainingPoints]
@LoyaltyCustID as varchar(20), @CardNumber as varchar(20), @UsedPointsAmount as numeric(28,12), @PointsUseDate as datetime, @DataAreaID as varchar(4)
AS
BEGIN

DECLARE @v_CardNumber as varchar (20), @v_LineNum as int, @v_RemainingPoints as numeric(28,12), @v_LoyaltyCustID as varchar(20);
DECLARE @err as int, @v_cardType as int;

IF (@PointsUseDate IS NULL) OR (@PointsUseDate='1900-01-01 00:00:000')
	SET @PointsUseDate = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()));		--set today if not defined

SET @v_cardType = (SELECT LOYALTYTENDER FROM RBOLOYALTYMSRCARDTABLE WHERE CARDNUMBER=@CardNumber AND DATAAREAID=@DataAreaID);

--print 'v_cardType' + CONVERT(varchar, @v_cardType)

IF @v_cardType IS NULL
	RETURN 1								--Loyalty card could not be found

IF @v_cardType = 3
	RETURN 2								--Loyalty card is blocked

--IF @v_cardType = 2
--	RETURN 3								--Loyalty card is no tender card - points could not be used

IF (@v_cardType IN (0, 2))							--Loyalty card type is card tender
BEGIN
	DECLARE Points_cursor CURSOR FOR 
	SELECT CARDNUMBER, LINENUM, REMAININGPOINTS
	FROM RBOLOYALTYTRANS
	WHERE
	CARDNUMBER = @CardNumber
	and [TYPE] = 0							--points issuing entry type
	AND [STATUS] = 1							--open entry
	AND EXPIRATIONDATE >= @PointsUseDate	--expired
	AND ENTRYTYPE = 0						--not voided
	ORDER BY EXPIRATIONDATE,DATEOFISSUE ASC;
END

IF @v_cardType = 1							--Loyalty card type is contact tender
BEGIN
	DECLARE Points_cursor CURSOR FOR 
	SELECT CARDNUMBER, LINENUM, REMAININGPOINTS
	FROM RBOLOYALTYTRANS
	WHERE
	LOYALTYCUSTID = @LoyaltyCustID
	and [TYPE] = 0							--points issuing entry type
	AND [STATUS] = 1							--open entry
	AND EXPIRATIONDATE >= @PointsUseDate	--expired
	AND ENTRYTYPE = 0						--not voided
	ORDER BY EXPIRATIONDATE,DATEOFISSUE ASC;
END

OPEN Points_cursor;

FETCH NEXT FROM Points_cursor 
INTO @v_CardNumber, @v_LineNum, @v_RemainingPoints;

BEGIN TRAN
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--print 'CardNumber: ' + @v_CardNumber + ',linenum: ' + CONVERT(varchar,@v_LineNum);
		--print 'Used: ' + convert(varchar, @UsedPointsAmount);
		--print 'Remaining: ' + convert(varchar, @v_RemainingPoints);
		IF ABS(@UsedPointsAmount) < @v_RemainingPoints
		BEGIN
			--print 'Used < remaining'
			UPDATE RBOLOYALTYTRANS
			SET REMAININGPOINTS = REMAININGPOINTS + @UsedPointsAmount
			WHERE CARDNUMBER=@v_CardNumber AND LINENUM=@v_LineNum AND DATAAREAID=@DataAreaID;
		
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR
			
			COMMIT TRAN
			CLOSE Points_cursor;
			DEALLOCATE Points_cursor;
			RETURN 0;
		END
		
		IF ABS(@UsedPointsAmount) = @v_RemainingPoints
		BEGIN
			--print 'used = remaining'
			UPDATE RBOLOYALTYTRANS
			SET REMAININGPOINTS = 0, [STATUS] = 0
			WHERE CARDNUMBER=@v_CardNumber AND LINENUM=@v_LineNum AND DATAAREAID=@DataAreaID;

			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR
			
			COMMIT TRAN
			CLOSE Points_cursor;
			DEALLOCATE Points_cursor;
			RETURN 0;
		END
		
		IF ABS(@UsedPointsAmount) > @v_RemainingPoints
		BEGIN
			--print 'used > remaining'
			UPDATE RBOLOYALTYTRANS
			SET REMAININGPOINTS = 0, [STATUS] = 0
			WHERE CARDNUMBER=@v_CardNumber AND LINENUM=@v_LineNum AND DATAAREAID=@DataAreaID;
			
			--print 'Used: ' + convert(varchar, @UsedPointsAmount);		
			
			SET @UsedPointsAmount+=@v_RemainingPoints;
			
			--print 'Used2: ' + convert(varchar, @UsedPointsAmount);	
			
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR
		END
		
		FETCH NEXT FROM Points_cursor 
		INTO @v_CardNumber, @v_LineNum, @v_RemainingPoints;
	END
COMMIT TRAN

CLOSE Points_cursor;
DEALLOCATE Points_cursor;

RETURN 0;

HANDLE_ERROR:
	BEGIN
		ROLLBACK TRAN;
		CLOSE Points_cursor;
		DEALLOCATE Points_cursor;
		RETURN 4;							--Insert/Update error
	END
END
GO
/* Stored Procedure  spCUSTOMER_RecreateCustomerLedger */


IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCUSTOMER_RecreateCustomerLedger]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spCUSTOMER_RecreateCustomerLedger]
GO
CREATE PROCEDURE [dbo].[spCUSTOMER_RecreateCustomerLedger]
@customerID as varchar(20), @statementID as varchar(20), @DataAreaID as varchar(4), @UserID as uniqueidentifier

AS
BEGIN

	--print 'Begin Customer Ledger Entry creation'

	-- PARAMETERS testing values
	--SET @DATAAREAID = 'LSR';
	--SET @customerID = '';
	--SET @statementID = '';

	--Stored proc variables
	DECLARE @transType int
	DECLARE @transactionID nvarchar(20)
	DECLARE @receiptID nvarchar(20)
	DECLARE @storeID nvarchar(20)
	DECLARE @terminalID nvarchar(20)
	DECLARE @amountToAccount numeric(28,12)
	DECLARE @customerAccount nvarchar(30)
	DECLARE @saleIsReturn tinyint
	DECLARE @TransDate datetime

	DECLARE @AMOUNTTENDERED numeric(28,12)
	DECLARE @TENDERCOUNT int
	DECLARE @WholeDiscountAmount numeric(28,12)
	DECLARE @err as int
	DECLARE @LocalCurrency as nvarchar(3)

	SET @LocalCurrency = (SELECT CURRENCYCODE FROM COMPANYINFO WHERE KEY_=0 AND DATAAREAID = @DataAreaID)

	BEGIN TRAN
	
	IF @statementID = ''
	BEGIN
		IF @customerID = ''
		BEGIN
			-- Recreate whole CusotmerLedgerEntries table
			-- TODO check
			DELETE FROM CUSTOMERLEDGERENTRIES
			WHERE TERMINALID <> ''
			
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT <> ''
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
		END
		ELSE
		BEGIN
			-- Recreate one customer's customer ledger entries
			-- TODO check
			DELETE FROM CUSTOMERLEDGERENTRIES 
			WHERE CUSTOMER = @customerID
			AND TERMINALID <> ''
			
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT = @customerID
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
		END
	END
	ELSE
	BEGIN
		IF @customerID = ''
		BEGIN
			-- Create customer ledger entries by statement ID
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT <> ''
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
			AND STATEMENTID = @statementID
			AND [TYPE] <> 3
		END
		ELSE
		BEGIN
			-- Create customer ledger entries by statement ID for one customer only
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT = @customerID
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
			AND STATEMENTID = @statementID
			AND [TYPE] <> 3
		END
	END

	OPEN TRANSACTIONLIST
	FETCH FROM TRANSACTIONLIST INTO @transType, @transactionID, @receiptID, @storeID, @terminalID, @amountToAccount, @customerAccount, @saleIsReturn, @transDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @transType = 2 
		BEGIN
			-- Get the number of valid payments on the transactions
			SELECT @TENDERCOUNT = COALESCE(COUNT(tt.TRANSACTIONID), 0)
			FROM RBOTRANSACTIONPAYMENTTRANS tt
			LEFT JOIN RBOTENDERTYPETABLE te ON tt.TENDERTYPE = te.TENDERTYPEID
			WHERE tt.TRANSACTIONSTATUS = 0 -- we don't want voided payments
			AND tt.TRANSACTIONID = @transactionID
			AND tt.STORE = @storeID
			AND tt.TERMINAL = @terminalID	
			AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')

			-- Check if there are any customer account payments
			SELECT @AMOUNTTENDERED = COALESCE(SUM(AMOUNTMST), 0)
			FROM RBOTRANSACTIONPAYMENTTRANS P	
			JOIN RBOSTORETENDERTYPETABLE T ON 
				P.TENDERTYPE = T.TENDERTYPEID 
				AND T.POSOPERATION = 202 -- is customer account tender
				AND T.STOREID = P.STORE 
				AND T.DATAAREAID = P.DATAAREAID
			LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
			WHERE P.DATAAREAID =  @DataAreaID
			AND P.TRANSACTIONSTATUS = 0 -- we don't want voided payments
			AND P.TRANSACTIONID = @transactionID
			AND P.STORE = @storeID
			AND P.TERMINAL = @terminalID
			AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')			
			
			IF @AMOUNTTENDERED <> 0
			BEGIN
				SET @TENDERCOUNT = @TENDERCOUNT - 1 
				IF @AMOUNTTENDERED < 0 SET @saleIsReturn = 1
				IF @saleIsReturn = 1
				BEGIN
					PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = CreditMemo, Status = Open, Amount = '+ CAST(-@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
					
					INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
					 VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,2						--Credit Memo
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,1						--Open
						   ,@UserID)
					
					SET @err = @@ERROR
					IF @err != 0 GOTO HANDLE_ERROR
				END
				ELSE
				BEGIN
					PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Invoice, Status = Open, Amount = '+ CAST(-@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
					INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
					 VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,1						--Invoice
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,1						--Open
						   ,@UserID)
						   
					SET @err = @@ERROR
					IF @err != 0 GOTO HANDLE_ERROR
				END
			END
			
			IF @TENDERCOUNT > 0
			BEGIN
				-- Get the sum of non-customer account payments to save to ledger		
				SELECT @AMOUNTTENDERED = COALESCE(SUM(AMOUNTMST), 0)
				FROM RBOTRANSACTIONPAYMENTTRANS P	
				JOIN RBOSTORETENDERTYPETABLE T ON 
					P.TENDERTYPE = T.TENDERTYPEID 
					AND T.POSOPERATION <> 202 -- is NOT customer account tender
					AND T.STOREID = P.STORE 
					AND T.DATAAREAID = P.DATAAREAID
				LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
				WHERE P.DATAAREAID =  @DataAreaID
				AND P.TRANSACTIONSTATUS = 0 -- we don't want voided payments
				AND P.TRANSACTIONID = @transactionID
				AND P.STORE = @storeID
				AND P.TERMINAL = @terminalID	
				AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')
				
				-- There are either other payments on the transaction or it wasn't paid with a customer account
				IF @AMOUNTTENDERED <> 0
				BEGIN
					IF @AMOUNTTENDERED < 0 SET @saleIsReturn = 1
					PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Sale, Status = Closed, Amount = '+ CAST(-@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
					INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
					 VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,3						--Sale
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,0
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,0						--Closed
						   ,@UserID)
						   
					SET @err = @@ERROR
					IF @err != 0 GOTO HANDLE_ERROR
				END
			END
			
			--IF @saleIsReturn = 0
			BEGIN
				SET @WholeDiscountAmount = (SELECT SUM(ISNULL(WHOLEDISCAMOUNTWITHTAX,0)) 
											FROM RBOTRANSACTIONSALESTRANS 
											WHERE DATAAREAID = @DataAreaID 
											AND TRANSACTIONSTATUS = 0 -- we don't want voided payments
											AND TRANSACTIONID = @transactionID
											AND STORE = @storeID
											AND TERMINALID = @terminalID)
				IF @WholeDiscountAmount <> 0 
				BEGIN
					IF @saleIsReturn = 0
					BEGIN
						PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Discount, Status = Closed, Amount = '+ CAST(ABS(@WholeDiscountAmount) as varchar)+' is return: '+cast(@saleIsReturn as varchar)
						INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
						VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,4						--Discount
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,@WholeDiscountAmount
						   ,@WholeDiscountAmount
						   ,0
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,0						--Closed
						   ,@UserID)
						SET @err = @@ERROR
						IF @err != 0 GOTO HANDLE_ERROR
					END
					ELSE
					BEGIN
						PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Discount, Status = Closed, Amount = '+ CAST(-ABS(@WholeDiscountAmount) as varchar)+' is return: '+cast(@saleIsReturn as varchar)
						INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
						VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,4						--Discount
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,@WholeDiscountAmount
						   ,@WholeDiscountAmount
						   ,0
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,0						--Closed
						   ,@UserID)
						SET @err = @@ERROR
						IF @err != 0 GOTO HANDLE_ERROR
					END						
					
				END
			END
		END
		
		ELSE
		IF @transType = 3
		BEGIN 
			-- Get the sum of non-customer account payments to save to ledger		
			SELECT @AMOUNTTENDERED = COALESCE(SUM(AMOUNTMST), 0)
			FROM RBOTRANSACTIONPAYMENTTRANS P	
			JOIN RBOSTORETENDERTYPETABLE T ON 
				P.TENDERTYPE = T.TENDERTYPEID 
				AND T.POSOPERATION <> 202 -- is NOT customer account tender
				AND T.STOREID = P.STORE 
				AND T.DATAAREAID = P.DATAAREAID
			LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
			WHERE P.DATAAREAID =  @DataAreaID
			AND P.TRANSACTIONSTATUS = 0 -- we don't want voided payments
			AND P.TRANSACTIONID = @transactionID
			AND P.STORE = @storeID
			AND P.TERMINAL = @terminalID
			AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')			
			
			-- There are either other payments on the transaction or it wasn't paid with a customer account
			IF @AMOUNTTENDERED <> 0
			BEGIN
				PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Payment, Status = Open, Amount = '+ CAST(@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
				INSERT INTO [CUSTOMERLEDGERENTRIES]
				   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
				VALUES
				   (@DataAreaID
				   ,@TransDate
				   ,@customerAccount
				   ,0						--Payment
				   ,@receiptID
				   ,''
				   ,''
				   ,@LocalCurrency
				   ,@AMOUNTTENDERED
				   ,@AMOUNTTENDERED
				   ,@AMOUNTTENDERED
				   ,@storeID
				   ,@terminalID
				   ,@transactionID
				   ,@receiptID
				   ,1						--Open
				   ,@UserID)
				SET @err = @@ERROR
				IF @err != 0 GOTO HANDLE_ERROR
			END
		END 
		
		
		FETCH NEXT FROM TRANSACTIONLIST INTO @transType, @transactionID, @receiptID, @storeID, @terminalID, @amountToAccount, @customerAccount, @saleIsReturn, @TransDate

	END

	COMMIT TRAN
	--print '------------------------ DONE';

	close TRANSACTIONLIST
	deallocate TRANSACTIONLIST
	RETURN 0;

	HANDLE_ERROR:
		BEGIN
			ROLLBACK TRAN;
			CLOSE Expired_cursor;
			DEALLOCATE Expired_cursor;
			RETURN 3;				--Insert error
		END
END
GO
/*  Stored Procedure spCUSTOMER_UpdateRemainingAmount  */

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCUSTOMER_UpdateRemainingAmount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spCUSTOMER_UpdateRemainingAmount]
GO
CREATE PROCEDURE [dbo].[spCUSTOMER_UpdateRemainingAmount]
@CustID as varchar(20), @DataAreaID as varchar(4)

AS
BEGIN

DECLARE @v_EntryNo_Positive as int, @v_EntryNo_Negative  as int, @v_RemAm_Positive as numeric(28,12), @v_RemAm_Negative as numeric(28,12), @err as int

BEGIN
	DECLARE Positive_cursor CURSOR FOR 
	
	SELECT ENTRYNO, REMAININGAMOUNT
	FROM CUSTOMERLEDGERENTRIES
	WHERE
		CUSTOMER = @CustID
		AND [STATUS] = 1 	--open entry
		AND ([TYPE] = 0	OR [TYPE] = 2) --invoice  payment /credit memo	
		AND DATAAREAID = @DataAreaID
END

BEGIN
	DECLARE Negative_cursor CURSOR FOR 
	SELECT ENTRYNO, REMAININGAMOUNT
	FROM CUSTOMERLEDGERENTRIES
	WHERE
		CUSTOMER = @CustID 
		AND [STATUS] = 1 	--open entry
		AND [TYPE] = 1	--invoice 
		AND DATAAREAID = @DataAreaID
	
END

OPEN Negative_cursor;
OPEN Positive_cursor;

FETCH NEXT FROM Negative_cursor 
INTO @v_EntryNo_Negative, @v_RemAm_Negative;

IF @@FETCH_STATUS <> 0
BEGIN

CLOSE Negative_cursor;
DEALLOCATE Negative_cursor;

CLOSE Positive_cursor;
DEALLOCATE Positive_cursor;

return 0;
END


FETCH NEXT FROM Positive_cursor
INTO @v_EntryNo_Positive, @v_RemAm_Positive;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		--print 'CardNumber: ' + @v_CardNumber + ',linenum: ' + CONVERT(varchar,@v_LineNum);	
	
		/*1*/
		IF ABS(@v_RemAm_Negative) > @v_RemAm_Positive	
		BEGIN
			BEGIN TRAN
										
			UPDATE CUSTOMERLEDGERENTRIES
			SET [REMAININGAMOUNT]= (@v_RemAm_Negative + @v_RemAm_Positive)
			WHERE ENTRYNO=@v_EntryNo_Negative AND DATAAREAID=@DataAreaID;					
			
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR
						
			UPDATE CUSTOMERLEDGERENTRIES
			SET [STATUS] = 0,
			[REMAININGAMOUNT] = 0
			WHERE ENTRYNO=@v_EntryNo_Positive   AND DATAAREAID=@DataAreaID;					
			
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR						

			set @v_RemAm_Negative = (@v_RemAm_Negative + @v_RemAm_Positive);
			set @v_EntryNo_Positive = 0;						
			COMMIT TRAN
		END	
		ELSE
		/*2*/	
		IF ABS(@v_RemAm_Negative) < @v_RemAm_Positive	
		BEGIN
			BEGIN TRAN
									
						
			UPDATE CUSTOMERLEDGERENTRIES
			SET [REMAININGAMOUNT]= (@v_RemAm_Negative + @v_RemAm_Positive)
			WHERE ENTRYNO=@v_EntryNo_Positive   AND DATAAREAID=@DataAreaID;					
			
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR						
			
			UPDATE CUSTOMERLEDGERENTRIES
			SET [STATUS]=0,
			[REMAININGAMOUNT]= 0
			WHERE ENTRYNO=@v_EntryNo_Negative AND DATAAREAID=@DataAreaID;					
			
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR			
							
			set @v_RemAm_Positive = (@v_RemAm_Negative + @v_RemAm_Positive);
			set @v_RemAm_Negative = 0; 		
									
			COMMIT TRAN
		END		
		ELSE
		/*3*/
		IF ABS(@v_RemAm_Negative) = @v_RemAm_Positive	
		BEGIN
			BEGIN TRAN
										
			UPDATE CUSTOMERLEDGERENTRIES
			SET [STATUS]=0,
			[REMAININGAMOUNT] = 0
			WHERE (ENTRYNO=@v_EntryNo_Positive  OR ENTRYNO=@v_EntryNo_Negative) AND DATAAREAID=@DataAreaID;					
			
			SET @err = @@ERROR
			IF @err != 0 GOTO HANDLE_ERROR
			
			set @v_RemAm_Negative = 0;	
			set @v_RemAm_Positive = 0;			
						
			COMMIT TRAN
		END	
			
			IF @v_RemAm_Negative >=0
			BEGIN								
				FETCH NEXT FROM Negative_cursor 
				INTO @v_EntryNo_Negative, @v_RemAm_Negative;
			END;

		IF @v_EntryNo_Positive <= 0
		BEGIN
			FETCH NEXT FROM Positive_cursor
			INTO @v_EntryNo_Positive, @v_RemAm_Positive;
		END	

	END


CLOSE Negative_cursor;
DEALLOCATE Negative_cursor;

CLOSE Positive_cursor;
DEALLOCATE Positive_cursor;

RETURN 0;

HANDLE_ERROR:
	BEGIN

		ROLLBACK TRAN;
		CLOSE Positive_cursor;
		DEALLOCATE Positive_cursor;		

		ROLLBACK TRAN;
		CLOSE Negative_cursor;
		DEALLOCATE Negative_cursor;
		RETURN 1;				--Update error
	END
END

go


/****** Object:  StoredProcedure [dbo].[SplitList]    Script Date: 04/10/2018 15:21:00 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SplitList]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[SplitList]
GO

/****** Object:  StoredProcedure [dbo].[SplitList]    Script Date: 04/10/2018 15:21:00 ******/
CREATE FUNCTION [dbo].[SplitList] (@list VARCHAR(MAX), @separator VARCHAR(MAX) = ';')
RETURNS @table TABLE (Value VARCHAR(MAX))
AS BEGIN
  DECLARE @position INT, @previous INT
  SET @list = @list + @separator
  SET @previous = 1
  SET @position = CHARINDEX(@separator, @list)
  WHILE @position > 0 
  BEGIN
	IF @position - @previous > 0
	  INSERT INTO @table VALUES (SUBSTRING(@list, @previous, @position - @previous))
	IF @position >= LEN(@list) BREAK
	  SET @previous = @position + 1
	  SET @position = CHARINDEX(@separator, @list, @previous)
  END
  RETURN
END
GO

/****** Object:  Table-Valued Parameter [dbo].[UNITCONVERSIONSTABLETYPE]    Script Date: 11/10/2018 14:40:00 ******/
IF TYPE_ID('[dbo].[UNITCONVERSIONSTABLETYPE]') IS NOT NULL
BEGIN
	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CONVERTQTYBETWEENUNITS]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
	BEGIN
		DROP FUNCTION [dbo].[CONVERTQTYBETWEENUNITS]	
	END
	
	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GETCONVERSIONFACTOR]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
	BEGIN
		DROP FUNCTION [dbo].[GETCONVERSIONFACTOR]	
	END

	DROP TYPE [dbo].[UNITCONVERSIONSTABLETYPE]
END
GO

/****** Object:  Table-Valued Parameter [dbo].[UNITCONVERSIONSTABLETYPE]    Script Date: 11/10/2018 14:40:00 ******/
CREATE TYPE [dbo].[UNITCONVERSIONSTABLETYPE] AS TABLE
(
	[FROMUNIT] [NVARCHAR](20) NOT NULL,
	[TOUNIT] [NVARCHAR](20) NOT NULL,
	[FACTOR] [numeric](28, 12) NOT NULL,
	[ITEMID] [NVARCHAR](30) NOT NULL
)
GO

/****** Object:  StoredProcedure [dbo].[CONVERTQTYBETWEENUNITS]    Script Date: 11/10/2018 10:54:00 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CONVERTQTYBETWEENUNITS]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[CONVERTQTYBETWEENUNITS]
END
GO

/****** Object:  StoredProcedure [dbo].[CONVERTQTYBETWEENUNITS]    Script Date: 11/10/2018 10:54:00 ******/
CREATE FUNCTION [dbo].[CONVERTQTYBETWEENUNITS] (@ITEMID NVARCHAR(100), @COUNTED NUMERIC(28,12), @COUNTEDUNITID NVARCHAR(100), @INVENTORYUNITID NVARCHAR(100), @UNITCONVERSIONSTABLE UNITCONVERSIONSTABLETYPE READONLY)
RETURNS NUMERIC(28,12)
WITH EXECUTE AS CALLER
AS

BEGIN	
	DECLARE @FACTOR NUMERIC(28,12)
	SET @FACTOR = 1
		
	-- If the counted and inventory unit are the same then there is no need to do any processing
	IF (UPPER(@COUNTEDUNITID) <> UPPER(@INVENTORYUNITID))
	BEGIN
		
		SELECT @FACTOR = FACTOR FROM @UNITCONVERSIONSTABLE
		WHERE FROMUNIT = @COUNTEDUNITID AND TOUNIT = @INVENTORYUNITID AND ITEMID = @ITEMID			

		IF (@@ROWCOUNT = 0)
		BEGIN
			SELECT @FACTOR = 1/FACTOR FROM @UNITCONVERSIONSTABLE
			WHERE FROMUNIT = @INVENTORYUNITID AND TOUNIT = @COUNTEDUNITID AND ITEMID = @ITEMID	

			IF (@@ROWCOUNT = 0)
			BEGIN
				SELECT @FACTOR = FACTOR FROM @UNITCONVERSIONSTABLE
				WHERE FROMUNIT = @COUNTEDUNITID AND TOUNIT = @INVENTORYUNITID AND ITEMID = ''		 	
					
				IF (@@ROWCOUNT = 0)
				BEGIN
					SELECT @FACTOR = 1/FACTOR FROM @UNITCONVERSIONSTABLE
					WHERE FROMUNIT = @INVENTORYUNITID AND TOUNIT = @COUNTEDUNITID AND ITEMID = ''
					
					IF (@@ROWCOUNT = 0)
					BEGIN
						SET @FACTOR = 1
					END
				END					
			END
		END
	END

	RETURN @COUNTED / @FACTOR
END
GO

/****** Object:  StoredProcedure [dbo].[GETCONVERSIONFACTOR]    Script Date: 11/01/2021 10:54:00 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GETCONVERSIONFACTOR]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GETCONVERSIONFACTOR]
END
GO

/****** Object:  StoredProcedure [dbo].[GETCONVERSIONFACTOR]    Script Date: 11/01/2021 10:54:00 ******/
CREATE FUNCTION [dbo].[GETCONVERSIONFACTOR] (@ITEMID NVARCHAR(100), @FROMUNITID NVARCHAR(100), @TOUNITID NVARCHAR(100), @UNITCONVERSIONSTABLE UNITCONVERSIONSTABLETYPE READONLY)
RETURNS NUMERIC(28,12)
WITH EXECUTE AS CALLER
AS

BEGIN	
	DECLARE @FACTOR NUMERIC(28,12)
	SET @FACTOR = 1
		
	-- If the from unit and inventory unit are the same then there is no need to do any processing
	IF (UPPER(@FROMUNITID) <> UPPER(@TOUNITID))
	BEGIN
		
		SELECT @FACTOR = FACTOR FROM @UNITCONVERSIONSTABLE
		WHERE FROMUNIT = @FROMUNITID AND TOUNIT = @TOUNITID AND ITEMID = @ITEMID			

		IF (@@ROWCOUNT = 0)
		BEGIN
			SELECT @FACTOR = 1/FACTOR FROM @UNITCONVERSIONSTABLE
			WHERE FROMUNIT = @TOUNITID AND TOUNIT = @FROMUNITID AND ITEMID = @ITEMID	

			IF (@@ROWCOUNT = 0)
			BEGIN
				SELECT @FACTOR = FACTOR FROM @UNITCONVERSIONSTABLE
				WHERE FROMUNIT = @FROMUNITID AND TOUNIT = @TOUNITID AND ITEMID = ''		 	
					
				IF (@@ROWCOUNT = 0)
				BEGIN
					SELECT @FACTOR = 1/FACTOR FROM @UNITCONVERSIONSTABLE
					WHERE FROMUNIT = @TOUNITID AND TOUNIT = @FROMUNITID AND ITEMID = ''
					
					IF (@@ROWCOUNT = 0)
					BEGIN
						SET @FACTOR = 1
					END
				END					
			END
		END
	END

	RETURN @FACTOR
END
GO

/****** Object:  StoredProcedure [dbo].[CALCULATESUGGESTEDQUANTITY]    Script Date: 17/10/2019 14:33:00 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CALCULATESUGGESTEDQUANTITY]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[CALCULATESUGGESTEDQUANTITY]
END
GO

/****** Object:  StoredProcedure [dbo].[CALCULATESUGGESTEDQUANTITY]    Script Date: 17/10/2019 14:33:00 ******/
CREATE FUNCTION [dbo].[CALCULATESUGGESTEDQUANTITY] (@CALCULATE BIT, @INVENTONHAND NUMERIC(28,12), @ROUNDINGMETHOD INT, @MULTIPLE INT, @MAXINVENTORY NUMERIC(28, 12))
RETURNS NUMERIC(28,12)
WITH EXECUTE AS CALLER
AS

BEGIN	
	/**** CALCULATE SUGGESTED QUANTITY FOR REPLENISHMENT BASED ON CURRENT INVENTORY ON HAND AND REPLENISHMENT SETTINGS ****/

	IF(@CALCULATE = 0)
	RETURN 0

	IF(@MULTIPLE < 2)
	RETURN IIF(@MAXINVENTORY - @INVENTONHAND > 0, @MAXINVENTORY - @INVENTONHAND, 0)

	DECLARE @UNROUNDEDSUGESSTEDQTY NUMERIC(28,12) = @MAXINVENTORY - @INVENTONHAND
	DECLARE @QTYROUNDEDDOWN INT = CAST(@UNROUNDEDSUGESSTEDQTY / @MULTIPLE AS INT) * @MULTIPLE
	DECLARE @QTYROUNDEDUP INT = (CAST(@UNROUNDEDSUGESSTEDQTY / @MULTIPLE AS INT) + 1) * @MULTIPLE

	RETURN CASE
			WHEN @ROUNDINGMETHOD = 0 THEN IIF((@UNROUNDEDSUGESSTEDQTY - @QTYROUNDEDDOWN) >= (@QTYROUNDEDUP - @UNROUNDEDSUGESSTEDQTY), @QTYROUNDEDUP, @QTYROUNDEDDOWN)
			WHEN @ROUNDINGMETHOD = 1 THEN @QTYROUNDEDDOWN
			WHEN @ROUNDINGMETHOD = 2 THEN IIF(@UNROUNDEDSUGESSTEDQTY % @MULTIPLE != 0, @QTYROUNDEDUP, @QTYROUNDEDDOWN)
			ELSE 0
			END
END
GO

EXECUTE spDB_SetRoutineDescription_1_0 'CALCULATESUGGESTEDQUANTITY', 'Calculates suggested quantity based on inventory on hand and replenishment settings. Used to calculate suggested quantity in purchase worksheets.';
GO

/****** Object:  StoredProcedure [dbo].[GETEFFECTIVEINVENTORY]    Script Date: 29/10/2019 13:357:00 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GETEFFECTIVEINVENTORY]') AND type in (N'FN', N'IF',N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[GETEFFECTIVEINVENTORY]
END
GO

/****** Object:  StoredProcedure [dbo].[GETEFFECTIVEINVENTORY]    Script Date: 29/10/2019 13:57:00 ******/
CREATE FUNCTION [dbo].[GETEFFECTIVEINVENTORY] (@ITEMID NVARCHAR(20), @STOREID NVARCHAR(20))
RETURNS NUMERIC(28,12)
WITH EXECUTE AS CALLER
AS

BEGIN	

	/**** GET THE INVENTORY ON HAND FOR AN ITEM INCLUDING UNPOSTED ITEMS FROM PURCHASE ORDERS AND TRANSFER ORDERS ****/
	DECLARE @VALUE NUMERIC(28, 12);

	SELECT @VALUE = SUM(QUANTITY)
		FROM (
		--Inventory
		SELECT INS.QUANTITY AS QUANTITY 
		FROM VINVENTSUM INS
		WHERE INS.ITEMID = @ITEMID AND INS.STOREID = @STOREID
                                    
		UNION ALL
                                    
		--Quantity on purchase orders
		SELECT (CASE WHEN POL.UNITID <> R.INVENTORYUNITID 
		THEN COALESCE(GRL.RECEIVEDQUANTITY, POL.QUANTITY) * ISNULL(UNIT1.FACTOR, UNIT2.FACTOR)
		ELSE COALESCE(GRL.RECEIVEDQUANTITY, POL.QUANTITY) END) AS QUANTITY  
		FROM PURCHASEORDERLINE POL
		JOIN PURCHASEORDERS PO ON PO.PURCHASEORDERID = POL.PURCHASEORDERID  AND PO.STOREID = @STOREID
		JOIN RETAILITEM R ON R.ITEMID = POL.RETAILITEMID
		LEFT OUTER JOIN GOODSRECEIVINGLINE GRL ON GRL.PURCHASEORDERLINENUMBER = POL.LINENUMBER AND GRL.GOODSRECEIVINGID = POL.PURCHASEORDERID  --Get only un-posted lines
		LEFT OUTER JOIN UNITCONVERT AS UNIT1 ON UNIT1.FROMUNIT = POL.UNITID AND UNIT1.TOUNIT = R.INVENTORYUNITID AND UNIT1.ITEMID = R.ITEMID  --conversion rule per item
		LEFT OUTER JOIN UNITCONVERT AS UNIT2 ON UNIT2.FROMUNIT = POL.UNITID AND UNIT2.TOUNIT = R.INVENTORYUNITID AND UNIT2.ITEMID = '' --global conversion rule
		WHERE POL.RETAILITEMID = @ITEMID AND (GRL.PURCHASEORDERLINENUMBER IS NULL OR GRL.POSTED = 0) 
                                    
		UNION ALL
                                    
		--Quantity in transfer in
		SELECT (CASE WHEN ITOL.UNITID <> R.INVENTORYUNITID 
		THEN  ITOL.QUANTITYRECEIVED * ISNULL(UNIT1.FACTOR, UNIT2.FACTOR) 
		ELSE ITOL.QUANTITYRECEIVED END) AS QUANTITY --add pending receipt
		FROM INVENTORYTRANSFERORDERLINE AS ITOL
		INNER JOIN INVENTORYTRANSFERORDER IT ON IT.ID = ITOL.INVENTORYTRANSFERORDERID
		INNER JOIN RETAILITEM R ON R.ITEMID = ITOL.ITEMID
		LEFT OUTER JOIN UNITCONVERT AS UNIT1 ON UNIT1.FROMUNIT = ITOL.UNITID AND UNIT1.TOUNIT = R.INVENTORYUNITID AND UNIT1.ITEMID = R.ITEMID 
		LEFT OUTER JOIN UNITCONVERT AS UNIT2 ON UNIT2.FROMUNIT = ITOL.UNITID AND UNIT2.TOUNIT = R.INVENTORYUNITID AND UNIT2.ITEMID = '' 
		WHERE ITOL.ITEMID = @ITEMID AND IT.SENT = 1 AND IT.RECEIVED = 0 AND IT.REJECTED = 0 AND IT.RECEIVINGSTOREID = @STOREID
                                    
		UNION ALL
                                    
		--Quantity in transfer out
		SELECT (CASE WHEN ITOL.UNITID <> R.INVENTORYUNITID 
		THEN  -ITOL.QUANTITYSENT * ISNULL(UNIT1.FACTOR, UNIT2.FACTOR)
		ELSE -ITOL.QUANTITYSENT END) AS QUANTITY --subtract pending send
		FROM INVENTORYTRANSFERORDERLINE AS ITOL
		INNER JOIN INVENTORYTRANSFERORDER IT ON IT.ID = ITOL.INVENTORYTRANSFERORDERID
		INNER JOIN RETAILITEM R ON R.ITEMID = ITOL.ITEMID
		LEFT OUTER JOIN UNITCONVERT AS UNIT1 ON UNIT1.FROMUNIT = ITOL.UNITID AND UNIT1.TOUNIT = R.INVENTORYUNITID AND UNIT1.ITEMID = R.ITEMID 
		LEFT OUTER JOIN UNITCONVERT AS UNIT2 ON UNIT2.FROMUNIT = ITOL.UNITID AND UNIT2.TOUNIT = R.INVENTORYUNITID AND UNIT2.ITEMID = '' 
		WHERE ITOL.ITEMID = @ITEMID AND IT.SENT = 0 AND IT.SENDINGSTOREID = @STOREID
                                    
		) AS EFFECTIVEINVENTORY

		RETURN ISNULL(@VALUE, 0)
END
GO

EXECUTE spDB_SetRoutineDescription_1_0 'GETEFFECTIVEINVENTORY', 'Get inventory on hand for an item including unposted quantities from purchase orders and transfer orders.';
GO

-- The stored procedure sp_Dimdate is used by the Power BI templates and should not be removed
IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[dbo].[sp_Dimdate]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].sp_Dimdate
GO

CREATE PROCEDURE [dbo].sp_Dimdate
AS
BEGIN

	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE  id = Object_id(N'tempdb..#Dimdate')) 
	BEGIN 
		DROP TABLE #Dimdate 
	END

	/**********************************************************************************/

	CREATE TABLE [dbo].[#Dimdate]
	(	[DateKey] INT primary key, 
		[Date] DATETIME,
		[FullDateUK] CHAR(10), -- Date in dd-MM-yyyy format
		[FullDateUSA] CHAR(10),-- Date in MM-dd-yyyy format
		[DayOfMonth] VARCHAR(2), -- Field will hold day number of Month
		[DaySuffix] VARCHAR(4), -- Apply suffix as 1st, 2nd ,3rd etc
		[DayName] VARCHAR(9), -- Contains name of the day, Sunday, Monday 
		[DayOfWeekUSA] CHAR(1),-- First Day Sunday=1 and Saturday=7
		[DayOfWeekUK] CHAR(1),-- First Day Monday=1 and Sunday=7
		[DayOfWeekInMonth] VARCHAR(2), --1st Monday or 2nd Monday in Month
		[DayOfWeekInYear] VARCHAR(2),
		[DayOfQuarter] VARCHAR(3),
		[DayOfYear] VARCHAR(3),
		[WeekOfMonth] VARCHAR(1),-- Week Number of Month 
		[WeekOfQuarter] VARCHAR(2), --Week Number of the Quarter
		[WeekOfYear] VARCHAR(2),--Week Number of the Year
		[Month] VARCHAR(2), --Number of the Month 1 to 12
		[MonthName] VARCHAR(9),--January, February etc
		[MonthOfQuarter] VARCHAR(2),-- Month Number belongs to Quarter
		[Quarter] CHAR(1),
		[QuarterName] VARCHAR(9),--First,Second..
		[Year] CHAR(4),-- Year value of Date stored in Row
		[YearName] CHAR(7), --CY 2012,CY 2013
		[MonthYear] CHAR(10), --Jan-2013,Feb-2013
		[MMYYYY] CHAR(6),
		[FirstDayOfMonth] DATE,
		[LastDayOfMonth] DATE,
		[FirstDayOfQuarter] DATE,
		[LastDayOfQuarter] DATE,
		[FirstDayOfYear] DATE,
		[LastDayOfYear] DATE,
		[IsHolidayUSA] BIT,-- Flag 1=National Holiday, 0-No National Holiday
		[IsWeekday] BIT,-- 0=Week End ,1=Week Day
		[HolidayUSA] VARCHAR(50),--Name of Holiday in US
		[IsHolidayUK] BIT Null,-- Flag 1=National Holiday, 0-No National Holiday
		[HolidayUK] VARCHAR(50) Null --Name of Holiday in UK
	)

	/********************************************************************************************/

	DECLARE @StartDate DATETIME = (select case when min(transdate) is null then '2017-01-01' 
								   else convert(nvarchar,datepart(yy,min(transdate)))+'-01-01' 
								   end
								   from rbotransactiontable) --Starting value of Date Range
	DECLARE @EndDate DATETIME = (select case when max(transdate) is null then '2017-01-01' 
								 else convert(nvarchar,datepart(yy,max(transdate)))+'-12-31' 
								 end
								 from rbotransactiontable) --End Value of Date Range
	--Temporary Variables To Hold the Values During Processing of Each Date of Year
	DECLARE
		@DayOfWeekInMonth INT,
		@DayOfWeekInYear INT,
		@DayOfQuarter INT,
		@WeekOfMonth INT,
		@CurrentYear INT,
		@CurrentMonth INT,
		@CurrentQuarter INT

	/*Table Data type to store the day of week count for the month and year*/
	DECLARE @DayOfWeek TABLE (DOW INT, MonthCount INT, QuarterCount INT, YearCount INT)

	INSERT INTO @DayOfWeek VALUES (1, 0, 0, 0)
	INSERT INTO @DayOfWeek VALUES (2, 0, 0, 0)
	INSERT INTO @DayOfWeek VALUES (3, 0, 0, 0)
	INSERT INTO @DayOfWeek VALUES (4, 0, 0, 0)
	INSERT INTO @DayOfWeek VALUES (5, 0, 0, 0)
	INSERT INTO @DayOfWeek VALUES (6, 0, 0, 0)
	INSERT INTO @DayOfWeek VALUES (7, 0, 0, 0)

	--Extract and assign various parts of Values from Current Date to Variable

	DECLARE @CurrentDate AS DATETIME = @StartDate
	SET @CurrentMonth = DATEPART(MM, @CurrentDate)
	SET @CurrentYear = DATEPART(YY, @CurrentDate)
	SET @CurrentQuarter = DATEPART(QQ, @CurrentDate)

	/********************************************************************************************/
	--Proceed only if Start Date(Current date ) is less than End date you specified above

	WHILE @CurrentDate <= @EndDate
	BEGIN
 
	/*Begin day of week logic*/

			 /*Check for Change in Month of the Current date if Month changed then 
			  Change variable value*/

		IF @CurrentMonth != DATEPART(MM, @CurrentDate) 
		BEGIN
			UPDATE @DayOfWeek
			SET MonthCount = 0
			SET @CurrentMonth = DATEPART(MM, @CurrentDate)
		END

			/* Check for Change in Quarter of the Current date if Quarter changed then change 
			 Variable value*/

		IF @CurrentQuarter != DATEPART(QQ, @CurrentDate)
		BEGIN
			UPDATE @DayOfWeek
			SET QuarterCount = 0
			SET @CurrentQuarter = DATEPART(QQ, @CurrentDate)
		END
       
			/* Check for Change in Year of the Current date if Year changed then change 
			 Variable value*/

		IF @CurrentYear != DATEPART(YY, @CurrentDate)
		BEGIN
			UPDATE @DayOfWeek
			SET YearCount = 0
			SET @CurrentYear = DATEPART(YY, @CurrentDate)
		END
	
			-- Set values in table data type created above from variables 

		UPDATE @DayOfWeek
		SET 
			MonthCount = MonthCount + 1,
			QuarterCount = QuarterCount + 1,
			YearCount = YearCount + 1
		WHERE DOW = DATEPART(DW, @CurrentDate)

		SELECT
			@DayOfWeekInMonth = MonthCount,
			@DayOfQuarter = QuarterCount,
			@DayOfWeekInYear = YearCount
		FROM @DayOfWeek
		WHERE DOW = DATEPART(DW, @CurrentDate)
	
	/*End day of week logic*/

	/* Populate Your Dimension Table with values*/
	
		INSERT INTO [dbo].[#Dimdate]
		SELECT
		
			CONVERT (char(8),@CurrentDate,112) as DateKey,
			@CurrentDate AS Date,
			CONVERT (char(10),@CurrentDate,103) as FullDateUK,
			CONVERT (char(10),@CurrentDate,101) as FullDateUSA,
			DATEPART(DD, @CurrentDate) AS DayOfMonth,
			--Apply Suffix values like 1st, 2nd 3rd etc..
			CASE 
				WHEN DATEPART(DD,@CurrentDate) IN (11,12,13) 
				THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'th'
				WHEN RIGHT(DATEPART(DD,@CurrentDate),1) = 1 
				THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'st'
				WHEN RIGHT(DATEPART(DD,@CurrentDate),1) = 2 
				THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'nd'
				WHEN RIGHT(DATEPART(DD,@CurrentDate),1) = 3 
				THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'rd'
				ELSE CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'th' 
				END AS DaySuffix,
		
			DATENAME(DW, @CurrentDate) AS DayName,
			DATEPART(DW, @CurrentDate) AS DayOfWeekUSA,

			-- check for day of week as Per US and change it as per UK format 
			CASE DATEPART(DW, @CurrentDate)
				WHEN 1 THEN 7
				WHEN 2 THEN 1
				WHEN 3 THEN 2
				WHEN 4 THEN 3
				WHEN 5 THEN 4
				WHEN 6 THEN 5
				WHEN 7 THEN 6
				END 
				AS DayOfWeekUK,
		
			@DayOfWeekInMonth AS DayOfWeekInMonth,
			@DayOfWeekInYear AS DayOfWeekInYear,
			@DayOfQuarter AS DayOfQuarter,
			DATEPART(DY, @CurrentDate) AS DayOfYear,
			DATEPART(WW, @CurrentDate) + 1 - DATEPART(WW, CONVERT(VARCHAR, 
			DATEPART(MM, @CurrentDate)) + '/1/' + CONVERT(VARCHAR, 
			DATEPART(YY, @CurrentDate))) AS WeekOfMonth,
			(DATEDIFF(DD, DATEADD(QQ, DATEDIFF(QQ, 0, @CurrentDate), 0), 
			@CurrentDate) / 7) + 1 AS WeekOfQuarter,
			DATEPART(WW, @CurrentDate) AS WeekOfYear,
			DATEPART(MM, @CurrentDate) AS Month,
			DATENAME(MM, @CurrentDate) AS MonthName,
			CASE
				WHEN DATEPART(MM, @CurrentDate) IN (1, 4, 7, 10) THEN 1
				WHEN DATEPART(MM, @CurrentDate) IN (2, 5, 8, 11) THEN 2
				WHEN DATEPART(MM, @CurrentDate) IN (3, 6, 9, 12) THEN 3
				END AS MonthOfQuarter,
			DATEPART(QQ, @CurrentDate) AS Quarter,
			CASE DATEPART(QQ, @CurrentDate)
				WHEN 1 THEN 'First'
				WHEN 2 THEN 'Second'
				WHEN 3 THEN 'Third'
				WHEN 4 THEN 'Fourth'
				END AS QuarterName,
			DATEPART(YEAR, @CurrentDate) AS Year,
			'CY ' + CONVERT(VARCHAR, DATEPART(YEAR, @CurrentDate)) AS YearName,
			LEFT(DATENAME(MM, @CurrentDate), 3) + '-' + CONVERT(VARCHAR, 
			DATEPART(YY, @CurrentDate)) AS MonthYear,
			RIGHT('0' + CONVERT(VARCHAR, DATEPART(MM, @CurrentDate)),2) + 
			CONVERT(VARCHAR, DATEPART(YY, @CurrentDate)) AS MMYYYY,
			CONVERT(DATETIME, CONVERT(DATE, DATEADD(DD, - (DATEPART(DD, 
			@CurrentDate) - 1), @CurrentDate))) AS FirstDayOfMonth,
			CONVERT(DATETIME, CONVERT(DATE, DATEADD(DD, - (DATEPART(DD, 
			(DATEADD(MM, 1, @CurrentDate)))), DATEADD(MM, 1, 
			@CurrentDate)))) AS LastDayOfMonth,
			DATEADD(QQ, DATEDIFF(QQ, 0, @CurrentDate), 0) AS FirstDayOfQuarter,
			DATEADD(QQ, DATEDIFF(QQ, -1, @CurrentDate), -1) AS LastDayOfQuarter,
			CONVERT(DATETIME, '01/01/' + CONVERT(VARCHAR, DATEPART(YY, 
			@CurrentDate))) AS FirstDayOfYear,
			CONVERT(DATETIME, '12/31/' + CONVERT(VARCHAR, DATEPART(YY, 
			@CurrentDate))) AS LastDayOfYear,
			NULL AS IsHolidayUSA,
			CASE DATEPART(DW, @CurrentDate)
				WHEN 1 THEN 0
				WHEN 2 THEN 1
				WHEN 3 THEN 1
				WHEN 4 THEN 1
				WHEN 5 THEN 1
				WHEN 6 THEN 1
				WHEN 7 THEN 0
				END AS IsWeekday,
			NULL AS HolidayUSA, Null, Null

		SET @CurrentDate = DATEADD(DD, 1, @CurrentDate)
	END

	/********************************************************************************************/
 
	SELECT * FROM [dbo].[#Dimdate]

END
GO


/****** Object:  View [dbo].[VTRANSACTIONSALESTRANSHASPRICE]    Script Date: 27/10/2020 16:37:00 ******/

/* This view is used by the system reports to exclude all sales lines for assembly items and assembly components
   that do not have price calculated (exclude assembly items that were sold with calculate price from components = true,
   and assembly components that were sold with calculate price from components = false) */

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VTRANSACTIONSALESTRANSHASPRICE]'))
begin
   drop view [dbo].[VTRANSACTIONSALESTRANSHASPRICE]
end

go

create view [dbo].[VTRANSACTIONSALESTRANSHASPRICE]
as
select * from dbo.RBOTRANSACTIONSALESTRANS where HASPRICE = 1

go

/****** Object:  View [dbo].[GETRETAILITEMCOST]    Script Date: 6/1/2021 12:00:00 ******/

/* This function returns the newest record before the time specified by parameter @EVALTIME, 
   for each combination of store and item from RETAILITEMCOSTS.

   If the @EVALTIME is null or empty, the newest record for each item & store pair is returned.

   Note that if system parameter CostCalculation is set to manual (2) the cost is stored in the RETAILITEM.
   This view only returns records from RETAILITEMCOSTS (used for last purchase price and weighted average calcualted cost)
*/

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[GETRETAILITEMCOST]') )
DROP FUNCTION [DBO].[GETRETAILITEMCOST]
GO

CREATE FUNCTION [DBO].[GETRETAILITEMCOST](@EVALTIME DATETIME)
RETURNS TABLE
AS
RETURN
WITH NEWEST_COST AS (
	SELECT
		*,
		ROW_NUMBER() OVER (
			PARTITION BY STOREID, ITEMID ORDER BY ENTRYDATE DESC
		) AS ROWNUM
	FROM (
		SELECT * FROM RETAILITEMCOST
		UNION ALL 
		SELECT * FROM RETAILITEMCOSTHISTORY
	) COSTTABLES
	WHERE (ENTRYDATE < @EVALTIME) OR (COALESCE(@EVALTIME, '') = '')
)
SELECT
	ID,
	STOREID,
	ITEMID,
	COST,
	ENTRYDATE,
	REASON,
	USERID
FROM
	NEWEST_COST
WHERE
	ROWNUM = 1

GO
