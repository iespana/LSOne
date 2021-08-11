/*

	Incident No.	: ONE-8573
	Responsible		: Ovidiu Caba
	Sprint			: Maltz
	Date			: 04.05.2018

	Description		: Fix bug: display the correct list of users on login, depending on the store that it is attached to their profile
*/

USE LSPOSNET
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spPOSGetLoginType_1_1_Unsecure]'))
begin
   drop procedure dbo.spPOSGetLoginType_1_1_Unsecure
end

GO

create procedure dbo.spPOSGetLoginType_1_1_Unsecure
(@storeID nvarchar(20),@terminalID nvarchar(20),@dataareaID nvarchar(10),@licenseCode nvarchar(30), @version nvarchar(10),@cultureName nvarchar(20) out,@keyboardCode nvarchar(20) out,@layoutName nvarchar(50) out,@allowTrainingMode bit out,
 @licensePassword nvarchar(50) out, @licenseExpireDate datetime out, @storeExists bit out, @terminalExists bit out)
as

set nocount on

declare @showStaffListAtLogon bit
declare @limitStaffListToStore bit
declare @nameConvention nvarchar(10)

set @showStaffListAtLogon = 0
set @limitStaffListToStore = 0
set @storeExists = 0
set @terminalExists = 0
set @licensePassword = ''
set @licenseExpireDate = CURRENT_TIMESTAMP
set @allowTrainingMode = 0

if exists(select 'x' from RBOSTORETABLE where  STOREID = @storeID)
begin
	set @storeExists = 1
end

if exists(select 'x' from RBOTERMINALTABLE where TERMINALID = @terminalID and STOREID = @storeID)
begin
	set @terminalExists = 1
end


select @showStaffListAtLogon = ISNULL(f.SHOWSTAFFLISTATLOGON,0), @limitStaffListToStore = ISNULL(LIMITSTAFFLISTTOSTORE,0),@cultureName = s.CULTURENAME,
@keyboardCode = ISNULL(s.KEYBOARDCODE,''), @layoutName = ISNULL(s.LAYOUTNAME,''), @allowTrainingMode = 0
from RBOTERMINALTABLE t
join RBOSTORETABLE s on t.STOREID = s.STOREID and s.DATAAREAID = t.DATAAREAID and s.STOREID = @storeID
join POSFUNCTIONALITYPROFILE f on 
	case 
		when (t.FUNCTIONALITYPROFILE is NOT NULL and t.FUNCTIONALITYPROFILE <> '') then t.FUNCTIONALITYPROFILE
		when (s.FUNCTIONALITYPROFILE is NOT NULL and s.FUNCTIONALITYPROFILE <> '') then s.FUNCTIONALITYPROFILE
		else ''
	end  = f.PROFILEID  and f.DATAAREAID = s.DATAAREAID
where t.TERMINALID = @terminalID and t.DATAAREAID = @dataareaID

--IF NOT EXISTS(SELECT VOLUMENO FROM POSISLICENSE WHERE DATAAREAID = @dataareaID AND VOLUMENO = @licenseCode AND TERMINALID = @terminalID AND STOREID = @storeID)
--BEGIN
--	INSERT INTO POSISLICENSE(ID, STOREID, TERMINALID, VOLUMENO, PASSWORD, EXPIREDATE, REPLICATIONCOUNTER, [VERSION]) 
--	values(NEWID(),@storeID, @terminalID, @licenseCode, '', CURRENT_TIMESTAMP, 0, @version)
--	set @licensePassword = ''
--	set @licenseExpireDate = CURRENT_TIMESTAMP
--END
--ELSE
--BEGIN
--	select @licensePassword = ISNULL(l.PASSWORD, ''), @licenseExpireDate = ISNULL(l.EXPIREDATE, '1900-01-01 00:00:00.000') 
--	FROM POSISLICENSE l 
--	WHERE DATAAREAID = @dataareaID AND STOREID = @storeID AND TERMINALID = @terminalID AND VOLUMENO = @licenseCode

--	UPDATE POSISLICENSE SET [VERSION] = @version where DATAAREAID = @dataareaID AND STOREID = @storeID AND TERMINALID = @terminalID AND VOLUMENO = @licenseCode
--END


if(@showStaffListAtLogon = 1)
	begin
		select @nameConvention=Value from SystemSettings where GUID='{2CF043AA-B7C2-4158-90EE-69AC5B7AEC32}'
		if(@limitStaffListToStore = 1)
			-- We want the staff list but only limited to the store
			begin
				if(@nameConvention='1')
					begin
						select u.[GUID],[Login], u.FirstName,u.MiddleName, u.LastName, u.NamePrefix, u.NameSuffix, ISNULL(u.STAFFID,'') as STAFFID,settings2.[Value] as NameConvention
						from USERS u
						join RBOSTAFFTABLE s on u.STAFFID = s.STAFFID and u.DATAAREAID = s.DATAAREAID
						join POSUSERPROFILE up on s.USERPROFILE = up.PROFILEID and up.STOREID = @storeID
						inner join dbo.SYSTEMSETTINGS as settings on settings.GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}'
						inner join dbo.SYSTEMSETTINGS as settings2 on settings2.GUID ='{2CF043AA-B7C2-4158-90EE-69AC5B7AEC32}'
						where ((CASE WHEN (LockOutCounter >= settings.[Value]) 
							   THEN 1 ELSE 0 END)) = 0 and IsServerUser = 0 and Deleted=0 and u.DATAAREAID = @dataareaID
						order by u.FirstName, u.MiddleName, u.LastName
					end
				else
					begin
						select u.[GUID],[Login], u.FirstName,u.MiddleName, u.LastName, u.NamePrefix, u.NameSuffix, ISNULL(u.STAFFID,'') as STAFFID,settings2.[Value] as NameConvention
						from USERS u
						join RBOSTAFFTABLE s on u.STAFFID = s.STAFFID and u.DATAAREAID = s.DATAAREAID
						join POSUSERPROFILE up on s.USERPROFILE = up.PROFILEID and up.STOREID = @storeID
						inner join dbo.SYSTEMSETTINGS as settings on settings.GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}'
						inner join dbo.SYSTEMSETTINGS as settings2 on settings2.GUID ='{2CF043AA-B7C2-4158-90EE-69AC5B7AEC32}'
						where ((CASE WHEN (LockOutCounter >= settings.[Value]) 
							   THEN 1 ELSE 0 END)) = 0 and IsServerUser = 0 and Deleted=0 and u.DATAAREAID = @dataareaID
						order by u.LastName, u.FirstName, u.MiddleName
					end
			end
		else
			begin
				if(@nameConvention='1')
					begin
						-- We want the staff list
						select u.[GUID],[Login], FirstName,MiddleName, LastName, NamePrefix, NameSuffix, ISNULL(STAFFID,'') as STAFFID,settings2.[Value] as NameConvention
						from USERS u
						inner join dbo.SYSTEMSETTINGS as settings on settings.GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}'
						inner join dbo.SYSTEMSETTINGS as settings2 on settings2.GUID ='{2CF043AA-B7C2-4158-90EE-69AC5B7AEC32}'
						where ((CASE WHEN (LockOutCounter >= settings.[Value]) 
							   THEN 1 ELSE 0 END)) = 0 and IsServerUser = 0 and Deleted=0 and u.DATAAREAID = @dataareaID
						order by u.FirstName, u.MiddleName, u.LastName
					end
				else
					begin
						-- We want the staff list
						select u.[GUID],[Login], FirstName,MiddleName, LastName, NamePrefix, NameSuffix, ISNULL(STAFFID,'') as STAFFID,settings2.[Value] as NameConvention
						from USERS u
						inner join dbo.SYSTEMSETTINGS as settings on settings.GUID = '{6278EA02-CC60-4AD2-BEA6-88CD0A8312AB}'
						inner join dbo.SYSTEMSETTINGS as settings2 on settings2.GUID ='{2CF043AA-B7C2-4158-90EE-69AC5B7AEC32}'
						where ((CASE WHEN (LockOutCounter >= settings.[Value]) 
							   THEN 1 ELSE 0 END)) = 0 and IsServerUser = 0 and Deleted=0 and u.DATAAREAID = @dataareaID
						order by u.LastName, u.FirstName, u.MiddleName
					end
			end
	end
else
	begin
		-- We do not want the stafflist
		-- We on purpose do a select statement that will return no result
		select 1 where 0 = 1
	end

/****** END ******/

GO