
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

declare @DATAAREAID nvarchar(10)

set @DATAAREAID = 'LSR'

--- ADD ADMIN USER
if not Exists (Select 'x' from USERS where DATAAREAID = @DATAAREAID and Login = 'admin')
begin

declare @adminID uniqueidentifier

set @adminID = 'B328ABD9-C2F6-44BE-9CCF-74ADEC8D5BFB'

declare @expiresTime int
declare @setting nvarchar(50)

Select @setting = [Value] from SYSTEMSETTINGS where GUID = '{7CB84D26-B28B-4086-8DCF-646F68CEF956}' and DATAAREAID = @DATAAREAID

Set @expiresTime = CONVERT(int,@setting)

if not Exists(Select 'x' from USERS where GUID = @adminID)
begin
	insert into USERS (
		DATAAREAID, GUID, Login, PasswordHash, NeedPasswordChange, ExpiresDate, IsDomainUser, IsServerUser,
		LastChangeTime, LocalProfileHash, FirstName, MiddleName, LastName, NamePrefix,
		NameSuffix, STAFFID, EMAIL)
	values (
		@DATAAREAID, @adminID, 'admin', 'CB053B8C1AE47D44154A52D64DB8BE050F6EC120', 1, DateAdd(Day,@expiresTime,GetDate()), 0, 0,
		GetDate(), '', 'System', '', 'Administrator', '',
		'', 'admin', '')
end

--- ADD USER PERMISSIONS
insert into USERSINGROUP (GUID, UserGUID, UserGroupGUID, DATAAREAID)
values ('{fbad78af-c864-4aa2-b617-5c4b9fb66a1f}', @adminID, '{ff21a0e8-40e0-4bf6-8670-eb159de2b48c}', @DATAAREAID)

--- ADD STAFF
insert into RBOSTAFFTABLE (STAFFID, NAME, DATAAREAID,MAXDISCOUNTPCT,MAXTOTALDISCOUNTPCT,MAXLINEDISCOUNTAMOUNT,MAXTOTALDISCOUNTAMOUNT,USERPROFILE ) values ('admin', 'System Administrator', @DATAAREAID,99,99,99999,99999, 'admin')

--- ADD USER PROFILE

insert into POSUSERPROFILE (PROFILEID, DATAAREAID, [DESCRIPTION], MAXLINEDISCOUNTAMOUNT, MAXDISCOUNTPCT, MAXTOTALDISCOUNTAMOUNT, MAXTOTALDISCOUNTPCT, MAXLINERETURNAMOUNT, MAXTOTALRETURNAMOUNT, STOREID, VISUALPROFILE, LAYOUTID, KEYBOARDCODE, LAYOUTNAME, OPERATORCULTURE)
values ('admin', @DATAAREAID, 'admin', 9999, 100, 9999, 100, 9999, 9999, '', '', '', '', '', '')

end