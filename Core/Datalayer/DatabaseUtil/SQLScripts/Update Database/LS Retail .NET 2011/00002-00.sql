
/*

	Incident No.	: 5703
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 01\Dot Net Team
	Date created	: 06.10.2010
	
	Description		: This is the "1.sql" script from the Store Controller

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:  
						
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[LANGUAGES]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.LANGUAGES
		(
		GUID uniqueidentifier NOT NULL,
		[Language] nvarchar(60) NOT NULL,
		ISOLanguageCode nvarchar(2) NOT NULL,
		OriginalValue nvarchar(60) NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL
		)  ON [PRIMARY]

	ALTER TABLE [dbo].[LANGUAGES] ADD  CONSTRAINT [DF__LANGUAGES__DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]


	ALTER TABLE dbo.LANGUAGES ADD CONSTRAINT PK_tblLanguage 
	PRIMARY KEY CLUSTERED (GUID,DATAAREAID) ON [PRIMARY]

END
GO
-- Name Prefix -----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[NAMEPREFIXES]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.NAMEPREFIXES
		(
		GUID uniqueidentifier NOT NULL,
		Prefix nvarchar(5) NOT NULL,
		OriginalValue nvarchar(5) NOT NULL,
		Locked bit NOT NULL,
		Deleted bit NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL
		)  ON [PRIMARY]

	ALTER TABLE dbo.NAMEPREFIXES ADD CONSTRAINT DF_NAMEPREFIXES_Locked DEFAULT 0 FOR Locked


	ALTER TABLE dbo.NAMEPREFIXES ADD CONSTRAINT DF_NAMEPREFIXES_Deleted DEFAULT 0 FOR Deleted


	ALTER TABLE dbo.NAMEPREFIXES ADD CONSTRAINT DF_NAMEPREFIXES_DATAAREAID DEFAULT 'LSR' FOR DATAAREAID


	ALTER TABLE dbo.NAMEPREFIXES ADD CONSTRAINT PK_NAMEPREFIXES
	PRIMARY KEY CLUSTERED (GUID,DATAAREAID) ON [PRIMARY]
END
GO

-- User -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.USERS
		(
		GUID uniqueidentifier NOT NULL,
		Login nvarchar(32) NOT NULL,
		PasswordHash nvarchar(40) NOT NULL,
		NeedPasswordChange bit NOT NULL,
		ExpiresDate datetime NOT NULL,
		LockOutCounter int NOT NULL,
		IsDomainUser bit NOT NULL,
		LastChangeTime datetime NOT NULL,
		LocalProfileHash nvarchar(40) NOT NULL,
		FirstName nvarchar(31) NOT NULL,
		MiddleName nvarchar(15) NOT NULL,
		LastName nvarchar(20) NOT NULL,
		NamePrefix nvarchar(8) NOT NULL,
		NameSuffix nvarchar(8) NOT NULL,
		STAFFID nvarchar(10) null,
		DATAAREAID nvarchar(4) not null,
		Deleted bit NOT NULL
		)  ON [PRIMARY]


	alter table dbo.USERS add constraint
	PK_USERS primary key clustered (GUID,DATAAREAID) 


	ALTER TABLE dbo.USERS ADD CONSTRAINT DF_USERS_Deleted DEFAULT 0 FOR Deleted


	ALTER TABLE dbo.USERS ADD CONSTRAINT DF_USERS_LockOutCounter DEFAULT 0 FOR LockOutCounter

	DECLARE @v sql_variant 
	SET @v = N'Will only show on old data if deleted.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'user', N'dbo', N'table', N'USERS', N'column', N'Deleted'

END
GO

-- UserGroup -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERGROUPS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.USERGROUPS
		(
		GUID uniqueidentifier NOT NULL,
		Name nvarchar(32) NOT NULL,
		IsAdminGroup bit NOT NULL,
		Locked bit NOT NULL,
		DATAAREAID nvarchar(4) not null,
		Deleted bit NOT NULL
		)  ON [PRIMARY]

	ALTER TABLE dbo.USERGROUPS ADD CONSTRAINT DF_USERGROUP_Locked DEFAULT 0 FOR Locked


	ALTER TABLE dbo.USERGROUPS ADD CONSTRAINT DF_USERGROUP_Deleted DEFAULT 0 FOR Deleted



	ALTER TABLE dbo.USERGROUPS ADD CONSTRAINT DF_USERGROUP_IsAdminGroup DEFAULT 0 FOR IsAdminGroup


	alter table dbo.USERGROUPS add constraint
	PK_USERGROUPS primary key clustered (GUID,DATAAREAID)

END
GO

-- UsersInGroup -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERSINGROUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.USERSINGROUP
		(
		GUID uniqueidentifier NOT NULL,
		UserGUID uniqueidentifier NOT NULL,
		UserGroupGUID uniqueidentifier NOT NULL,
		DATAAREAID nvarchar(4) not null
		)  ON [PRIMARY]

	alter table dbo.USERSINGROUP add constraint
	PK_USERSINGROUP primary key clustered (GUID,DATAAREAID) 

END
GO

-- Permission -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PERMISSIONS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.PERMISSIONS
		(
		GUID uniqueidentifier NOT NULL,
		Description nvarchar(100) NOT NULL,
		PermissionGroupGUID uniqueidentifier NOT NULL,
		SortCode nvarchar(8) NOT NULL,
		PermissionCode varchar(80) NOT NULL,
		CodeIsEncrypted bit NOT NULL,
		DATAAREAID nvarchar(4) not null
		)  ON [PRIMARY]


	alter table dbo.PERMISSIONS add constraint
	PK_tblPermission primary key clustered (GUID,DATAAREAID) 


	CREATE TABLE dbo.PERMISSIONSLOCALIZATION
		(
		Locale nvarchar(10) NOT NULL,
		PermissionGUID uniqueidentifier NOT NULL,
		Description nvarchar(100) NOT NULL,
		DATAAREAID nvarchar(4) not null
		)  ON [PRIMARY]


	alter table dbo.PERMISSIONSLOCALIZATION add constraint
	PK_PERMISSIONSLOCALIZATION primary key clustered (Locale,PermissionGUID,DATAAREAID) 

END
GO

-- UserGroupPermissions -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERGROUPPERMISSIONS]') AND TYPE IN ('U'))
	BEGIN
	CREATE TABLE dbo.USERGROUPPERMISSIONS
		(
		GUID uniqueidentifier NOT NULL,
		UserGroupGUID uniqueidentifier NOT NULL,
		PermissionGUID uniqueidentifier NOT NULL,
		DATAAREAID nvarchar(4) not null
		)  ON [PRIMARY]

	alter table dbo.USERGROUPPERMISSIONS add constraint
	PK_USERGROUPPERMISSIONS primary key clustered (GUID,DATAAREAID) 

END
GO


-- UserPermission -----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERPERMISSION]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.USERPERMISSION
		(
		GUID uniqueidentifier NOT NULL,
		UserGUID uniqueidentifier NOT NULL,
		PermissionGUID uniqueidentifier NOT NULL,
		[Grant] bit NOT NULL,
		DATAAREAID nvarchar(4) not null
		)  ON [PRIMARY]

	alter table dbo.USERPERMISSION add constraint
	PK_USERPERMISSION primary key clustered (GUID,DATAAREAID) 

END
GO


-- Settings --------------------------------------------------------
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERSETTINGS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.USERSETTINGS
		(
		GUID uniqueidentifier NOT NULL,
		UserGUID uniqueidentifier NOT NULL,
		SettingsGUID uniqueidentifier NOT NULL,
		[Value] nvarchar(50) NOT NULL,
		DATAAREAID nvarchar(4) not null,
		)  ON [PRIMARY]


	alter table dbo.USERSETTINGS add constraint
	PK_USERSETTINGS primary key clustered (GUID,DATAAREAID)

END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[SYSTEMSETTINGSTYPE]') AND TYPE IN ('U'))
BEGIN
	create table dbo.SYSTEMSETTINGSTYPE
	(
		GUID uniqueidentifier NOT NULL,
		[Description] nvarchar(30) NOT NULL
	) on [PRIMARY]


	ALTER TABLE dbo.SYSTEMSETTINGSTYPE ADD CONSTRAINT
	PK_SYSTEMSETTINGSTYPEE PRIMARY KEY CLUSTERED (GUID) ON [PRIMARY]

END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[SYSTEMSETTINGS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.SYSTEMSETTINGS
		(
		GUID uniqueidentifier NOT NULL,
		[Name] nvarchar(30) NOT NULL,
		[Value] nvarchar(50) NOT NULL,
		Description nvarchar(80) NOT NULL,
		UserCanOverride bit,
		Type uniqueidentifier NOT NULL,
		DATAAREAID nvarchar(4) not null
		)  ON [PRIMARY]


	alter table dbo.SYSTEMSETTINGS add constraint
	PK_SYSTEMSETTINGS primary key clustered (GUID,DATAAREAID) 


	alter table dbo.SYSTEMSETTINGS 
	add Internal bit NOT NULL constraint DF_tblSystemSettings_Internal default 0

END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PERMISSIONGROUP]') AND TYPE IN ('U'))
BEGIN
	create table dbo.PERMISSIONGROUP
		(
		GUID uniqueidentifier NOT NULL,
		Name nvarchar(50) NOT NULL,
		DATAAREAID nvarchar(4) not null,
		Deleted bit NOT NULL
		)  on [PRIMARY]

	ALTER TABLE dbo.PERMISSIONGROUP ADD CONSTRAINT DF_tblPermissionGroup_Deleted DEFAULT 0 FOR Deleted

	alter table dbo.PERMISSIONGROUP add constraint
	PK_PERMISSIONGROUP primary key clustered (GUID,DATAAREAID) 

END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PERMISSIONGROUPLOCALIZATION]') AND TYPE IN ('U'))
BEGIN
	create table dbo.PERMISSIONGROUPLOCALIZATION
		(
		Locale nvarchar(10) NOT NULL,
		GUID uniqueidentifier NOT NULL,
		Name nvarchar(50) NOT NULL,
		DATAAREAID nvarchar(4) not null,
		)  on [PRIMARY]

	alter table dbo.PERMISSIONGROUPLOCALIZATION add constraint
	PK_PERMISSIONGROUPLOCALIZATION primary key clustered (Locale,GUID,DATAAREAID) 

END
GO

--ALTER TABLE dbo.PERMISSIONS ADD CONSTRAINT FK_tblPermission_tblPermissionGroup 
--FOREIGN KEY (PermissionGroupGUID) REFERENCES dbo.PERMISSIONGROUP (GUID)

GO

IF NOT EXISTS(SELECT * FROM [SYSTEMSETTINGSTYPE] WHERE [GUID] = 'c79ae480-7ee1-11db-9fe1-0800200c9a66' AND [DESCRIPTION] = 'Generic')
	insert into SYSTEMSETTINGSTYPE (GUID, Description)
	values ('c79ae480-7ee1-11db-9fe1-0800200c9a66','Generic')
GO

IF NOT EXISTS(SELECT * FROM [SYSTEMSETTINGSTYPE] WHERE [GUID] = 'daee3ff0-7ee1-11db-9fe1-0800200c9a66' AND [DESCRIPTION] = 'UI Field Visibility')
	insert into SYSTEMSETTINGSTYPE (GUID, Description)
	values ('daee3ff0-7ee1-11db-9fe1-0800200c9a66','UI Field Visibility')
GO

IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{592c4140-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{592c4140-d50b-11da-a94d-0800200c9a66}','Abkhazian','ab','Abkhazian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{592c4141-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{592c4141-d50b-11da-a94d-0800200c9a66}','Afar','aa','Afar');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{592c4142-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{592c4142-d50b-11da-a94d-0800200c9a66}','Afrikaans','af','Afrikaans');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{592c4143-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{592c4143-d50b-11da-a94d-0800200c9a66}','Albanian','sq','Albanian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{592c4144-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{592c4144-d50b-11da-a94d-0800200c9a66}','Amharic','am','Amharic');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{71663090-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{71663090-d50b-11da-a94d-0800200c9a66}','Arabic','ar','Arabic');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{71663091-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{71663091-d50b-11da-a94d-0800200c9a66}','Armenian','hy','Armenian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{71663092-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{71663092-d50b-11da-a94d-0800200c9a66}','Assamese','as','Assamese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{71663093-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{71663093-d50b-11da-a94d-0800200c9a66}','Aymara','ay','Aymara');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{71663094-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{71663094-d50b-11da-a94d-0800200c9a66}','Azerbaijani','az','Azerbaijani');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{847752e0-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{847752e0-d50b-11da-a94d-0800200c9a66}','Bashkir','ba','Bashkir');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{847752e1-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{847752e1-d50b-11da-a94d-0800200c9a66}','Basque','eu','Basque');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{847752e2-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{847752e2-d50b-11da-a94d-0800200c9a66}','Bengali','bn','Bengali');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{847752e3-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{847752e3-d50b-11da-a94d-0800200c9a66}','Bihari','bh','Bihari');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{847752e4-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{847752e4-d50b-11da-a94d-0800200c9a66}','Bislama','bi','Bislama');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9a873640-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9a873640-d50b-11da-a94d-0800200c9a66}','Breton','be','Breton');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9a873641-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9a873641-d50b-11da-a94d-0800200c9a66}','Bulgarian','bg','Bulgarian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9a875d50-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9a875d50-d50b-11da-a94d-0800200c9a66}','Burmese','my','Burmese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9a875d51-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9a875d51-d50b-11da-a94d-0800200c9a66}','Byelorussian','be','Byelorussian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9a875d52-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9a875d52-d50b-11da-a94d-0800200c9a66}','Catalan','ca','Catalan');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ab0f15a0-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ab0f15a0-d50b-11da-a94d-0800200c9a66}','Chinese','zh','Chinese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ab0f15a1-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ab0f15a1-d50b-11da-a94d-0800200c9a66}','Corsican','co','Corsican');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ab0f15a2-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ab0f15a2-d50b-11da-a94d-0800200c9a66}','Croatian','hr','Croatian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ab0f15a3-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ab0f15a3-d50b-11da-a94d-0800200c9a66}','Czech','cs','Czech');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ab0f15a4-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ab0f15a4-d50b-11da-a94d-0800200c9a66}','Danish','da','Danish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bfe6a010-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bfe6a010-d50b-11da-a94d-0800200c9a66}','Dutch','nl','Dutch');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bfe6a011-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bfe6a011-d50b-11da-a94d-0800200c9a66}','Dzongkha','dz','Dzongkha');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bfe6a012-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bfe6a012-d50b-11da-a94d-0800200c9a66}','English','en','English');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bfe6a013-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bfe6a013-d50b-11da-a94d-0800200c9a66}','Esperanto','eo','Esperanto');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bfe6a014-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bfe6a014-d50b-11da-a94d-0800200c9a66}','Estonian','et','Estonian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{cecf7570-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{cecf7570-d50b-11da-a94d-0800200c9a66}','Faroese','fo','Faroese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{cecf7571-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{cecf7571-d50b-11da-a94d-0800200c9a66}','Fijian','fj','Fijian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{cecf7572-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{cecf7572-d50b-11da-a94d-0800200c9a66}','Finnish','fi','Finnish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{cecf7573-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{cecf7573-d50b-11da-a94d-0800200c9a66}','French','fr','French');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{cecf7574-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{cecf7574-d50b-11da-a94d-0800200c9a66}','Frisian','fy','Frisian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ebc936c0-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ebc936c0-d50b-11da-a94d-0800200c9a66}','Gallegan','gl','Gallegan');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ebc936c1-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ebc936c1-d50b-11da-a94d-0800200c9a66}','Georgian','ka','Georgian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ebc936c2-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ebc936c2-d50b-11da-a94d-0800200c9a66}','German','de','German');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ebc936c3-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ebc936c3-d50b-11da-a94d-0800200c9a66}','Greek, Modern (1453-)','el','Greek, Modern (1453-)');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{ebc936c4-d50b-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{ebc936c4-d50b-11da-a94d-0800200c9a66}','Greenlandic','kl','Greenlandic');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{4d504140-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{4d504140-d50c-11da-a94d-0800200c9a66}','Guarani','gn','Guarani');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{4d504141-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{4d504141-d50c-11da-a94d-0800200c9a66}','Gujarati','gu','Gujarati');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{4d504142-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{4d504142-d50c-11da-a94d-0800200c9a66}','Hausa','ha','Hausa');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{4d504143-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{4d504143-d50c-11da-a94d-0800200c9a66}','Hebrew','he','Hebrew');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{4d504144-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{4d504144-d50c-11da-a94d-0800200c9a66}','Hindi','hi','Hindi');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{95641920-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{95641920-d50c-11da-a94d-0800200c9a66}','Hungarian','hu','Hungarian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{95641921-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{95641921-d50c-11da-a94d-0800200c9a66}','Icelandic','is','Icelandic');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{95644030-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{95644030-d50c-11da-a94d-0800200c9a66}','Indonesian','id','Indonesian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{95644031-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{95644031-d50c-11da-a94d-0800200c9a66}','Interlingua (International Auxiliary language Association)','ia','Interlingua (International Auxiliary language Association)');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{95644032-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{95644032-d50c-11da-a94d-0800200c9a66}','Inuktitut','iu','Inuktitut');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{aab2cf60-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{aab2cf60-d50c-11da-a94d-0800200c9a66}','Inupiak','ik','Inupiak');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{aab2cf61-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{aab2cf61-d50c-11da-a94d-0800200c9a66}','Irish','ga','Irish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{aab2cf62-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{aab2cf62-d50c-11da-a94d-0800200c9a66}','Italian','it','Italian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{aab2cf63-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{aab2cf63-d50c-11da-a94d-0800200c9a66}','Japanese','ja','Japanese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{aab2cf64-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{aab2cf64-d50c-11da-a94d-0800200c9a66}','Javanese','jv','Javanese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bd7c6160-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bd7c6160-d50c-11da-a94d-0800200c9a66}','Kannada','kn','Kannada');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bd7c6161-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bd7c6161-d50c-11da-a94d-0800200c9a66}','Kashmiri','ks','Kashmiri');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bd7c8870-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bd7c8870-d50c-11da-a94d-0800200c9a66}','Kazakh','kk','Kazakh');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bd7c8871-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bd7c8871-d50c-11da-a94d-0800200c9a66}','Khmer','km','Khmer');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{bd7c8872-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{bd7c8872-d50c-11da-a94d-0800200c9a66}','Kinyarwanda','rw','Kinyarwanda');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d2166da0-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d2166da0-d50c-11da-a94d-0800200c9a66}','Kirghiz','ky','Kirghiz');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d2166da1-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d2166da1-d50c-11da-a94d-0800200c9a66}','Korean','ko','Korean');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d2166da2-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d2166da2-d50c-11da-a94d-0800200c9a66}','Kurdish','ku','Kurdish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d2166da3-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d2166da3-d50c-11da-a94d-0800200c9a66}','Langue d''Oc (post 1500)','oc','Langue d''Oc (post 1500)');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d2166da4-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d2166da4-d50c-11da-a94d-0800200c9a66}','Lao','lo','Lao');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e4521780-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e4521780-d50c-11da-a94d-0800200c9a66}','Latin','la','Latin');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e4521781-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e4521781-d50c-11da-a94d-0800200c9a66}','Latvian','lv','Latvian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e4521782-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e4521782-d50c-11da-a94d-0800200c9a66}','Lingala','ln','Lingala');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e4521783-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e4521783-d50c-11da-a94d-0800200c9a66}','Lithuanian','lt','Lithuanian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e4521784-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e4521784-d50c-11da-a94d-0800200c9a66}','Macedonian','mk','Macedonian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{fac354c0-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{fac354c0-d50c-11da-a94d-0800200c9a66}','Malagasy','mg','Malagasy');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{fac354c1-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{fac354c1-d50c-11da-a94d-0800200c9a66}','Malay','ms','Malay');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{fac354c2-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{fac354c2-d50c-11da-a94d-0800200c9a66}','Maltese','ml','Maltese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{fac354c3-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{fac354c3-d50c-11da-a94d-0800200c9a66}','Maori','mi','Maori');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{fac354c4-d50c-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{fac354c4-d50c-11da-a94d-0800200c9a66}','Marathi','mr','Marathi');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{af089f80-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{af089f80-d50d-11da-a94d-0800200c9a66}','Moldavian','mo','Moldavian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{af08c690-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{af08c690-d50d-11da-a94d-0800200c9a66}','Mongolian','mn','Mongolian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{af08c691-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{af08c691-d50d-11da-a94d-0800200c9a66}','Nauru','na','Nauru');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{af08c692-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{af08c692-d50d-11da-a94d-0800200c9a66}','Nepali','ne','Nepali');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{af08c693-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{af08c693-d50d-11da-a94d-0800200c9a66}','Norwegian','no','Norwegian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{c3f4c360-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{c3f4c360-d50d-11da-a94d-0800200c9a66}','Oriya','or','Oriya');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{c3f4c361-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{c3f4c361-d50d-11da-a94d-0800200c9a66}','Oromo','om','Oromo');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{c3f4c362-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{c3f4c362-d50d-11da-a94d-0800200c9a66}','Panjabi','pa','Panjabi');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{c3f4c363-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{c3f4c363-d50d-11da-a94d-0800200c9a66}','Persian','fa','Persian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{c3f4ea70-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{c3f4ea70-d50d-11da-a94d-0800200c9a66}','Polish','pl','Polish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d736e0c0-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d736e0c0-d50d-11da-a94d-0800200c9a66}','Portuguese','pt','Portuguese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d736e0c1-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d736e0c1-d50d-11da-a94d-0800200c9a66}','Pushto','ps','Pushto');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d736e0c2-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d736e0c2-d50d-11da-a94d-0800200c9a66}','Quechua','qu','Quechua');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d736e0c3-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d736e0c3-d50d-11da-a94d-0800200c9a66}','Rhaeto-Romance','rm','Rhaeto-Romance');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{d736e0c4-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{d736e0c4-d50d-11da-a94d-0800200c9a66}','Romanian','ro','Romanian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e77b9ca0-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e77b9ca0-d50d-11da-a94d-0800200c9a66}','Rundi','rn','Rundi');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e77b9ca1-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e77b9ca1-d50d-11da-a94d-0800200c9a66}','Russian','ru','Russian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e77b9ca2-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e77b9ca2-d50d-11da-a94d-0800200c9a66}','Samoan','sm','Samoan');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e77b9ca3-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e77b9ca3-d50d-11da-a94d-0800200c9a66}','Sango','sg','Sango');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{e77b9ca4-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{e77b9ca4-d50d-11da-a94d-0800200c9a66}','Sanskrit','sa','Sanskrit');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{b0baedf0-d50e-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{b0baedf0-d50e-11da-a94d-0800200c9a66}','Serbian','sr','Serbian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{b0baedf1-d50e-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{b0baedf1-d50e-11da-a94d-0800200c9a66}','Serbo-Croatian','sh','Serbo-Croatian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{b0baedf2-d50e-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{b0baedf2-d50e-11da-a94d-0800200c9a66}','Shona','sn','Shona');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{b0baedf3-d50e-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{b0baedf3-d50e-11da-a94d-0800200c9a66}','Sindhi','sd','Sindhi');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{b0baedf4-d50e-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{b0baedf4-d50e-11da-a94d-0800200c9a66}','Singhalese','si','Singhalese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{c57b9500-d50e-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{c57b9500-d50e-11da-a94d-0800200c9a66}','Siswant','ss','Siswant');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9aacd4c4-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9aacd4c4-d50d-11da-a94d-0800200c9a66}','Slovak','sk','Slovak');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9aacd4c3-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9aacd4c3-d50d-11da-a94d-0800200c9a66}','Slovenian','sl','Slovenian');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9aacd4c2-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9aacd4c2-d50d-11da-a94d-0800200c9a66}','Somali','so','Somali');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9aacd4c1-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9aacd4c1-d50d-11da-a94d-0800200c9a66}','Sotho, Southern','st','Sotho, Southern');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{9aacd4c0-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{9aacd4c0-d50d-11da-a94d-0800200c9a66}','Spanish','es','Spanish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{7bdad104-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{7bdad104-d50d-11da-a94d-0800200c9a66}','Sudanese','su','Sudanese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{7bdad103-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{7bdad103-d50d-11da-a94d-0800200c9a66}','Swahili','sw','Swahili');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{7bdad102-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{7bdad102-d50d-11da-a94d-0800200c9a66}','Swedish','sv','Swedish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{7bdad101-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{7bdad101-d50d-11da-a94d-0800200c9a66}','Tagalog','tl','Tagalog');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{7bdad100-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{7bdad100-d50d-11da-a94d-0800200c9a66}','Tajik','tg','Tajik');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{671a7813-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{671a7813-d50d-11da-a94d-0800200c9a66}','Tamil','ta','Tamil');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{671a7812-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{671a7812-d50d-11da-a94d-0800200c9a66}','Tatar','tt','Tatar');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{671a7811-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{671a7811-d50d-11da-a94d-0800200c9a66}','Telugu','te','Telugu');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{671a7810-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{671a7810-d50d-11da-a94d-0800200c9a66}','Thai','th','Thai');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{671a5100-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{671a5100-d50d-11da-a94d-0800200c9a66}','Tibetan','bo','Tibetan');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{540e37c4-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{540e37c4-d50d-11da-a94d-0800200c9a66}','Tigrinya','ti','Tigrinya');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{540e37c3-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{540e37c3-d50d-11da-a94d-0800200c9a66}','Tonga (Nyasa)','to','Tonga (Nyasa)');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{540e37c2-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{540e37c2-d50d-11da-a94d-0800200c9a66}','Tsonga','ts','Tsonga');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{540e37c1-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{540e37c1-d50d-11da-a94d-0800200c9a66}','Tswana','tn','Tswana');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{540e37c0-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{540e37c0-d50d-11da-a94d-0800200c9a66}','Turkish','tr','Turkish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{3ba26302-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{3ba26302-d50d-11da-a94d-0800200c9a66}','Turkmen','tk','Turkmen');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{3ba26301-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{3ba26301-d50d-11da-a94d-0800200c9a66}','Twi','tw','Twi')
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{3ba26300-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{3ba26300-d50d-11da-a94d-0800200c9a66}','Uighur','ug','Uighur')
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{3ba23bf1-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{3ba23bf1-d50d-11da-a94d-0800200c9a66}','Ukrainian','uk','Ukrainian')
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{3ba23bf0-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{3ba23bf0-d50d-11da-a94d-0800200c9a66}','Urdu','ur','Urdu');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{2608c114-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{2608c114-d50d-11da-a94d-0800200c9a66}','Uzbek','uz','Uzbek');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{2608c113-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{2608c113-d50d-11da-a94d-0800200c9a66}','Vietnamese','vi','Vietnamese');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{2608c112-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{2608c112-d50d-11da-a94d-0800200c9a66}','Volapnk','vo','Volapnk');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{2608c111-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{2608c111-d50d-11da-a94d-0800200c9a66}','Welsh','cy','Welsh');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{2608c110-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{2608c110-d50d-11da-a94d-0800200c9a66}','Wolof','wo','Wolof');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{139f9e94-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{139f9e94-d50d-11da-a94d-0800200c9a66}','Xhosa','xh','Xhosa');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{139f9e93-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{139f9e93-d50d-11da-a94d-0800200c9a66}','Yiddish','yi','Yiddish');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{139f9e92-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{139f9e92-d50d-11da-a94d-0800200c9a66}','Yoruba','yo','Yoruba');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{139f9e91-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{139f9e91-d50d-11da-a94d-0800200c9a66}','Zhuang','za','Zhuang');
IF NOT EXISTS(SELECT * FROM LANGUAGES WHERE [GUID] = '{139f9e90-d50d-11da-a94d-0800200c9a66}')
	Insert into LANGUAGES (GUID,[Language],ISOLanguageCode,OriginalValue) values('{139f9e90-d50d-11da-a94d-0800200c9a66}','Zulu','zu','Zulu');
GO

IF NOT EXISTS(SELECT * FROM NAMEPREFIXES WHERE [GUID] = '{807f2ff0-d506-11da-a94d-0800200c9a66}')
	Insert into NAMEPREFIXES (GUID,Prefix,OriginalValue) values('{807f2ff0-d506-11da-a94d-0800200c9a66}','Dr','Dr');
IF NOT EXISTS(SELECT * FROM NAMEPREFIXES WHERE [GUID] = '{b1629940-d506-11da-a94d-0800200c9a66}')
	Insert into NAMEPREFIXES (GUID,Prefix,OriginalValue) values('{b1629940-d506-11da-a94d-0800200c9a66}','Mr','Mr');
IF NOT EXISTS(SELECT * FROM NAMEPREFIXES WHERE [GUID] = '{b6b24f30-d506-11da-a94d-0800200c9a66}')
	Insert into NAMEPREFIXES (GUID,Prefix,OriginalValue) values('{b6b24f30-d506-11da-a94d-0800200c9a66}','Mrs','Mrs');
IF NOT EXISTS(SELECT * FROM NAMEPREFIXES WHERE [GUID] = '{bbebbe00-d506-11da-a94d-0800200c9a66}')
	Insert into NAMEPREFIXES (GUID,Prefix,OriginalValue) values('{bbebbe00-d506-11da-a94d-0800200c9a66}','Miss','Miss');
IF NOT EXISTS(SELECT * FROM NAMEPREFIXES WHERE [GUID] = '{c204b760-d506-11da-a94d-0800200c9a66}')
	Insert into NAMEPREFIXES (GUID,Prefix,OriginalValue) values('{c204b760-d506-11da-a94d-0800200c9a66}','Ms','Ms');
IF NOT EXISTS(SELECT * FROM NAMEPREFIXES WHERE [GUID] = '{c71679f0-d506-11da-a94d-0800200c9a66}')
	Insert into NAMEPREFIXES (GUID,Prefix,OriginalValue) values('{c71679f0-d506-11da-a94d-0800200c9a66}','Rev','Rev');

GO

IF NOT EXISTS(SELECT * FROM USERGROUPS WHERE [GUID] = '{ff21a0e8-40e0-4bf6-8670-eb159de2b48c}')
Insert into USERGROUPS (GUID,Name,IsAdminGroup,Locked,DATAAREAID) values('{ff21a0e8-40e0-4bf6-8670-eb159de2b48c}','Administrators',1,1,'LSR');

GO

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{0d44bba0-3041-11df-9aae-0800200c9a66}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{0d44bba0-3041-11df-9aae-0800200c9a66}', 'SerialNumber', '','Customer serial number', 0,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{6ea88f92-10bc-4ad3-89b4-97666db45ece}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{6ea88f92-10bc-4ad3-89b4-97666db45ece}', 'SQLScriptVersion', '2','Last SQL Script that was installed', 0,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{7cb84d26-b28b-4086-8dcf-646f68cef956}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{7cb84d26-b28b-4086-8dcf-646f68cef956}', 'PasswordExpires', '90','User password expiration in days', 0,1,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{cf221fb0-c4b6-4574-9739-ef227d0b06e8}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{cf221fb0-c4b6-4574-9739-ef227d0b06e8}', 'PasswordGracePeriod', '5','Grace period before password expiration (in days)', 0,1,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{6278ea02-cc60-4ad2-bea6-88cd0a8312ab}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{6278ea02-cc60-4ad2-bea6-88cd0a8312ab}', 'LockoutThreshold', '3','Password lockout threshold', 0,1,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{17e851c0-3037-11df-9aae-0800200c9a66}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{17e851c0-3037-11df-9aae-0800200c9a66}', 'WriteAuditing', '1','One if write auditing is turned on else zero', 0,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{7d27f7ba-a9d7-4bfa-9fdd-e06b6704a3bb}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{7d27f7ba-a9d7-4bfa-9fdd-e06b6704a3bb}', 'IdentificationMask', '###-##-####','Identification Mask', 0,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{af8575a4-30a0-4f9a-bb22-bae56025be86}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{af8575a4-30a0-4f9a-bb22-bae56025be86}', 'WeightUnit', '1','Units used for weight', 1,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{6a12f603-e31a-4f0f-9932-c3980bcc9615}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{6a12f603-e31a-4f0f-9932-c3980bcc9615}', 'HeightUnit', '1','Units used for height', 1,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{da67b1ec-ee05-495c-b706-14d99f348c57}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{da67b1ec-ee05-495c-b706-14d99f348c57}', 'LengthUnit', '1','Units used for length', 1,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{2cf043aa-b7c2-4158-90ee-69ac5b7aec32}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{2cf043aa-b7c2-4158-90ee-69ac5b7aec32}', 'NameConvention', '1','Controls how names are displayed', 1,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

IF NOT EXISTS(SELECT * FROM SYSTEMSETTINGS WHERE [GUID] = '{f67862df-49a7-4da6-bf52-14632064a428}')
	Insert into SYSTEMSETTINGS (GUID,Name,[Value],Description,UserCanOverride,Internal,Type,DATAAREAID)
	values ('{f67862df-49a7-4da6-bf52-14632064a428}', 'AddressConvention', '1','Controls how addresses are displayed', 1,0,'C79AE480-7EE1-11DB-9FE1-0800200C9A66','LSR');

GO

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = '{0ca8e620-e997-11da-8ad9-0800200c9a66}')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('{0ca8e620-e997-11da-8ad9-0800200c9a66}','User and security management','LSR');

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = '808ed9f0-e997-11da-8ad9-0800200c9a66')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('808ed9f0-e997-11da-8ad9-0800200c9a66','System Administration','LSR');

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = 'df4afbd0-b35c-11de-8a39-0800200c9a66')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('df4afbd0-b35c-11de-8a39-0800200c9a66','Profiles','LSR');

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = 'ceac1ad0-e997-11da-8ad9-0800200c9a66')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('ceac1ad0-e997-11da-8ad9-0800200c9a66','Reports','LSR');


update SYSTEMSETTINGS Set Internal = 1 
	where GUID = '{7cb84d26-b28b-4086-8dcf-646f68cef956}' 
	or GUID = '{cf221fb0-c4b6-4574-9739-ef227d0b06e8}'
	or GUID = '{6278ea02-cc60-4ad2-bea6-88cd0a8312ab}'

GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[SIGNEDACTIONS]') AND TYPE IN ('U'))
BEGIN
	create table dbo.SIGNEDACTIONS
		(
			GUID uniqueidentifier NOT NULL,
			ContextGuid uniqueidentifier NULL,
			ActionGuid uniqueidentifier NOT NULL,
			Reason ntext NOT NULL,
			UserGuid uniqueidentifier NOT NULL,
			CreatedOn datetime NOT NULL,
			DATAAREAID nvarchar(10) not null
		) on [PRIMARY]

	alter table dbo.SIGNEDACTIONS add constraint
	PK_SIGNEDACTIONS primary key clustered (GUID,DATAAREAID) 

END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[COUNTRY]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[COUNTRY](
		[COUNTRYID] [nvarchar](10) NOT NULL,
		[TYPE] [int] NULL,
		[ISOCODE] [nvarchar](3) NULL,
		[ADDRFORMAT] [nvarchar](10) NULL,
		[NAME] [nvarchar](50) NULL,
		[GIROACCOUNTVALIDATIONMETHOD] [int] NULL,
		[BANKACCOUNTNUMVALIDATION] [int] NULL,
		[EUROZONECOUNTRY] [int] NULL,
		[CURRENCYCODE] [nvarchar](3) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_045COUNTRYIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[COUNTRYID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__COUNTRY__764C846B]  DEFAULT ('') FOR [COUNTRYID]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__TYPE__7740A8A4]  DEFAULT ((0)) FOR [TYPE]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__ISOCODE__7834CCDD]  DEFAULT ('') FOR [ISOCODE]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__ADDRFOR__7928F116]  DEFAULT ('') FOR [ADDRFORMAT]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__NAME__7A1D154F]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__GIROACC__7B113988]  DEFAULT ((0)) FOR [GIROACCOUNTVALIDATIONMETHOD]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__BANKACC__7C055DC1]  DEFAULT ((0)) FOR [BANKACCOUNTNUMVALIDATION]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__EUROZON__7CF981FA]  DEFAULT ((0)) FOR [EUROZONECOUNTRY]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__CURRENC__7DEDA633]  DEFAULT ('') FOR [CURRENCYCODE]

	ALTER TABLE [dbo].[COUNTRY] ADD  CONSTRAINT [DF__COUNTRY__DATAARE__7EE1CA6C]  DEFAULT ('LSR') FOR [DATAAREAID]

	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AF','AFGHANISTAN','AF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AX','ÅLAND ISLANDS','AX','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AL','ALBANIA','AL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('DZ','ALGERIA','DZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AS','AMERICAN SAMOA','AS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AD','ANDORRA','AD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AO','ANGOLA','AO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AI','ANGUILLA','AI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AQ','ANTARCTICA','AQ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AG','ANTIGUA AND BARBUDA','AG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AR','ARGENTINA','AR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AM','ARMENIA','AM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AW','ARUBA','AW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AU','AUSTRALIA','AU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AT','AUSTRIA','AT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AZ','AZERBAIJAN','AZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BS','BAHAMAS','BS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BH','BAHRAIN','BH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BD','BANGLADESH','BD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BB','BARBADOS','BB','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BY','BELARUS','BY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BE','BELGIUM','BE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BZ','BELIZE','BZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BJ','BENIN','BJ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BM','BERMUDA','BM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BT','BHUTAN','BT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BO','BOLIVIA, PLURINATIONAL STATE OF','BO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BA','BOSNIA AND HERZEGOVINA','BA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BW','BOTSWANA','BW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BV','BOUVET ISLAND','BV','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BR','BRAZIL','BR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IO','BRITISH INDIAN OCEAN TERRITORY','IO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BN','BRUNEI DARUSSALAM','BN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BG','BULGARIA','BG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BF','BURKINA FASO','BF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BI','BURUNDI','BI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KH','CAMBODIA','KH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CM','CAMEROON','CM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CA','CANADA','CA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CV','CAPE VERDE','CV','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KY','CAYMAN ISLANDS','KY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CF','CENTRAL AFRICAN REPUBLIC','CF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TD','CHAD','TD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CL','CHILE','CL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CN','CHINA','CN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CX','CHRISTMAS ISLAND','CX','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CC','COCOS (KEELING) ISLANDS','CC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CO','COLOMBIA','CO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KM','COMOROS','KM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CG','CONGO','CG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CD','CONGO, THE DEMOCRATIC REPUBLIC OF THE','CD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CK','COOK ISLANDS','CK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CR','COSTA RICA','CR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CI','CÔTE D''IVOIRE','CI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('HR','CROATIA','HR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CU','CUBA','CU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CY','CYPRUS','CY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CZ','CZECH REPUBLIC','CZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('DK','DENMARK','DK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('DJ','DJIBOUTI','DJ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('DM','DOMINICA','DM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('DO','DOMINICAN REPUBLIC','DO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('EC','ECUADOR','EC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('EG','EGYPT','EG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SV','EL SALVADOR','SV','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GQ','EQUATORIAL GUINEA','GQ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ER','ERITREA','ER','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('EE','ESTONIA','EE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ET','ETHIOPIA','ET','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('FK','FALKLAND ISLANDS (MALVINAS)','FK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('FO','FAROE ISLANDS','FO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('FJ','FIJI','FJ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('FI','FINLAND','FI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('FR','FRANCE','FR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GF','FRENCH GUIANA','GF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PF','FRENCH POLYNESIA','PF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TF','FRENCH SOUTHERN TERRITORIES','TF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GA','GABON','GA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GM','GAMBIA','GM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GE','GEORGIA','GE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('DE','GERMANY','DE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GH','GHANA','GH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GI','GIBRALTAR','GI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GR','GREECE','GR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GL','GREENLAND','GL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GD','GRENADA','GD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GP','GUADELOUPE','GP','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GU','GUAM','GU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GT','GUATEMALA','GT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GG','GUERNSEY','GG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GN','GUINEA','GN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GW','GUINEA-BISSAU','GW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GY','GUYANA','GY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('HT','HAITI','HT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('HM','HEARD ISLAND AND MCDONALD ISLANDS','HM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VA','HOLY SEE (VATICAN CITY STATE)','VA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('HN','HONDURAS','HN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('HK','HONG KONG','HK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('HU','HUNGARY','HU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IS','ICELAND','IS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IN','INDIA','IN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ID','INDONESIA','ID','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IR','IRAN, ISLAMIC REPUBLIC OF','IR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IQ','IRAQ','IQ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IE','IRELAND','IE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IM','ISLE OF MAN','IM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IL','ISRAEL','IL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('IT','ITALY','IT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('JM','JAMAICA','JM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('JP','JAPAN','JP','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('JE','JERSEY','JE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('JO','JORDAN','JO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KZ','KAZAKHSTAN','KZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KE','KENYA','KE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KI','KIRIBATI','KI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KP','KOREA, DEMOCRATIC PEOPLE''S REPUBLIC OF','KP','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KR','KOREA, REPUBLIC OF','KR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KW','KUWAIT','KW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KG','KYRGYZSTAN','KG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LA','LAO PEOPLE''S DEMOCRATIC REPUBLIC','LA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LV','LATVIA','LV','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LB','LEBANON','LB','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LS','LESOTHO','LS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LR','LIBERIA','LR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LY','LIBYAN ARAB JAMAHIRIYA','LY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LI','LIECHTENSTEIN','LI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LT','LITHUANIA','LT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LU','LUXEMBOURG','LU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MO','MACAO','MO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MK','MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF','MK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MG','MADAGASCAR','MG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MW','MALAWI','MW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MY','MALAYSIA','MY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MV','MALDIVES','MV','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ML','MALI','ML','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MT','MALTA','MT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MH','MARSHALL ISLANDS','MH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MQ','MARTINIQUE','MQ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MR','MAURITANIA','MR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MU','MAURITIUS','MU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('YT','MAYOTTE','YT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MX','MEXICO','MX','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('FM','MICRONESIA, FEDERATED STATES OF','FM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MD','MOLDOVA, REPUBLIC OF','MD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MC','MONACO','MC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MN','MONGOLIA','MN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ME','MONTENEGRO','ME','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MS','MONTSERRAT','MS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MA','MOROCCO','MA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MZ','MOZAMBIQUE','MZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MM','MYANMAR','MM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NA','NAMIBIA','NA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NR','NAURU','NR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NP','NEPAL','NP','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NL','NETHERLANDS','NL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AN','NETHERLANDS ANTILLES','AN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NC','NEW CALEDONIA','NC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NZ','NEW ZEALAND','NZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NI','NICARAGUA','NI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NE','NIGER','NE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NG','NIGERIA','NG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NU','NIUE','NU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NF','NORFOLK ISLAND','NF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MP','NORTHERN MARIANA ISLANDS','MP','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('NO','NORWAY','NO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('OM','OMAN','OM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PK','PAKISTAN','PK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PW','PALAU','PW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PS','PALESTINIAN TERRITORY, OCCUPIED','PS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PA','PANAMA','PA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PG','PAPUA NEW GUINEA','PG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PY','PARAGUAY','PY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PE','PERU','PE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PH','PHILIPPINES','PH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PN','PITCAIRN','PN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PL','POLAND','PL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PT','PORTUGAL','PT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PR','PUERTO RICO','PR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('QA','QATAR','QA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('RE','RÉUNION','RE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('RO','ROMANIA','RO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('RU','RUSSIAN FEDERATION','RU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('RW','RWANDA','RW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('BL','SAINT BARTHÉLEMY','BL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SH','SAINT HELENA','SH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('KN','SAINT KITTS AND NEVIS','KN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LC','SAINT LUCIA','LC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('MF','SAINT MARTIN','MF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('PM','SAINT PIERRE AND MIQUELON','PM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VC','SAINT VINCENT AND THE GRENADINES','VC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('WS','SAMOA','WS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SM','SAN MARINO','SM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ST','SAO TOME AND PRINCIPE','ST','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SA','SAUDI ARABIA','SA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SN','SENEGAL','SN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('RS','SERBIA','RS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SC','SEYCHELLES','SC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SL','SIERRA LEONE','SL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SG','SINGAPORE','SG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SK','SLOVAKIA','SK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SI','SLOVENIA','SI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SB','SOLOMON ISLANDS','SB','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SO','SOMALIA','SO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ZA','SOUTH AFRICA','ZA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GS','SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS','GS','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ES','SPAIN','ES','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('LK','SRI LANKA','LK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SD','SUDAN','SD','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SR','SURINAME','SR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SJ','SVALBARD AND JAN MAYEN','SJ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SZ','SWAZILAND','SZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SE','SWEDEN','SE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('CH','SWITZERLAND','CH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('SY','SYRIAN ARAB REPUBLIC','SY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TW','TAIWAN, PROVINCE OF CHINA','TW','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TJ','TAJIKISTAN','TJ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TZ','TANZANIA, UNITED REPUBLIC OF','TZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TH','THAILAND','TH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TL','TIMOR-LESTE','TL','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TG','TOGO','TG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TK','TOKELAU','TK','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TO','TONGA','TO','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TT','TRINIDAD AND TOBAGO','TT','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TN','TUNISIA','TN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TR','TURKEY','TR','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TM','TURKMENISTAN','TM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TC','TURKS AND CAICOS ISLANDS','TC','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('TV','TUVALU','TV','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('UG','UGANDA','UG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('UA','UKRAINE','UA','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('AE','UNITED ARAB EMIRATES','AE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('GB','UNITED KINGDOM','GB','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('US','UNITED STATES','US','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('UM','UNITED STATES MINOR OUTLYING ISLANDS','UM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('UY','URUGUAY','UY','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('UZ','UZBEKISTAN','UZ','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VU','VANUATU','VU','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VE','VENEZUELA, BOLIVARIAN REPUBLIC OF','VE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VN','VIET NAM','VN','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VG','VIRGIN ISLANDS, BRITISH','VG','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('VI','VIRGIN ISLANDS, U.S.','VI','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('WF','WALLIS AND FUTUNA','WF','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('EH','WESTERN SAHARA','EH','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('YE','YEMEN','YE','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ZM','ZAMBIA','ZM','LSR')
	Insert into COUNTRY (COUNTRYID, NAME, ISOCODE, DATAAREAID) Values('ZW','ZIMBABWE','ZW','LSR')
END
GO


IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = '1ddeef60-d9ae-11de-8a39-0800200c9a66' and DATAAREAID = 'LSR')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('1ddeef60-d9ae-11de-8a39-0800200c9a66','General','LSR');
GO

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = '78535d50-04e7-11df-8a39-0800200c9a66' and DATAAREAID = 'LSR')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('78535d50-04e7-11df-8a39-0800200c9a66','Item master','LSR');
GO

IF NOT EXISTS(SELECT * FROM PERMISSIONGROUP WHERE [GUID] = 'a54c7dc0-a922-11df-94e2-0800200c9a66' and DATAAREAID = 'LSR')
	Insert into PERMISSIONGROUP (GUID,Name,DATAAREAID) 
	values ('a54c7dc0-a922-11df-94e2-0800200c9a66','Hospitality','LSR')
GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOCOLORGROUPTRANS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOCOLORGROUPTRANS](
		[COLORGROUP] [nvarchar](10) NOT NULL,
		[COLOR] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](10) NOT NULL,
		[WEIGHT] [int] NOT NULL,
		[NOINBARCODE] [nvarchar](10) NOT NULL,
		[DESCRIPTION] [ntext] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_RBOCOLORGROUPTRANS_1] PRIMARY KEY CLUSTERED 
	(
		[COLORGROUP] ASC,
		[COLOR] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	SET ANSI_PADDING OFF

	ALTER TABLE [dbo].[RBOCOLORGROUPTRANS] ADD  CONSTRAINT [DF__RBOCOLORGROUPTRANS__NAME__434FFC05x]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[RBOCOLORGROUPTRANS] ADD  CONSTRAINT [DF__RBOCOLORGROUPTRANS__WEIGH__4444203Ex]  DEFAULT ((0)) FOR [WEIGHT]

	ALTER TABLE [dbo].[RBOCOLORGROUPTRANS] ADD  CONSTRAINT [DF__RBOCOLORGROUPTRANS__NOINB__45384477x]  DEFAULT ('') FOR [NOINBARCODE]

	ALTER TABLE [dbo].[RBOCOLORGROUPTRANS] ADD  CONSTRAINT [DF__RBOCOLORGROUPTRANS__DATAA__47208CE9x]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSTYLEGROUPTRANS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOSTYLEGROUPTRANS](
		[STYLEGROUP] [nvarchar](10) NOT NULL,
		[STYLE] [nvarchar](4) NOT NULL,
		[NAME] [nvarchar](10) NULL,
		[WEIGHT] [int] NULL,
		[NOINBARCODE] [nvarchar](10) NULL,
		[DESCRIPTION] [ntext] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_RBOSTYLEGROUPTRANS] PRIMARY KEY CLUSTERED 
	(
		[STYLEGROUP] ASC,
		[STYLE] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSTYLEGROUPTRANS__NAME]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSTYLEGROUPTRANS__WEIGHT]  DEFAULT ((0)) FOR [WEIGHT]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSTYLEGROUPTRANS__NOINBARCODE]  DEFAULT ('') FOR [NOINBARCODE]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSTYLEGROUPTRANS__DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]

END
GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSIZEGROUPTRANS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOSIZEGROUPTRANS](
		[SIZEGROUP] [nvarchar](10) NOT NULL,
		[SIZE_] [nvarchar](4) NOT NULL,
		[NAME] [nvarchar](10) NULL,
		[WEIGHT] [int] NULL,
		[NOINBARCODE] [nvarchar](10) NULL,
		[DESCRIPTION] [ntext] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_RBOSIZEGROUPTRANS] PRIMARY KEY CLUSTERED 
	(
		[SIZEGROUP] ASC,
		[SIZE_] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[RBOSIZEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSIZEGROUPTRANS__NAME]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[RBOSIZEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSIZEGROUPTRANS__WEIGHT]  DEFAULT ((0)) FOR [WEIGHT]

	ALTER TABLE [dbo].[RBOSIZEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSIZEGROUPTRANS__NOINBARCODE]  DEFAULT ('') FOR [NOINBARCODE]

	ALTER TABLE [dbo].[RBOSIZEGROUPTRANS] ADD  CONSTRAINT [DF__RBOSIZEGROUPTRANS__DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]
END

GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOTENDERTYPETABLE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOTENDERTYPETABLE](
		[TENDERTYPEID] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](30) NULL,
		[DEFAULTFUNCTION] [int] NULL,
		[MODIFIEDDATE] [datetime] NULL,
		[MODIFIEDTIME] [int] NULL,
		[MODIFIEDBY] [nvarchar](5) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_20155TENDERTYPEIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[TENDERTYPEID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDER__TENDE__14D5043E]  DEFAULT ('') FOR [TENDERTYPEID]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDERT__NAME__15C92877]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDER__DEFAU__16BD4CB0]  DEFAULT ((0)) FOR [DEFAULTFUNCTION]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDER__MODIF__17B170E9]  DEFAULT ('1900-01-01 00:00:00.000') FOR [MODIFIEDDATE]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDER__MODIF__18A59522]  DEFAULT ((0)) FOR [MODIFIEDTIME]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDER__MODIF__1999B95B]  DEFAULT ('?') FOR [MODIFIEDBY]

	ALTER TABLE [dbo].[RBOTENDERTYPETABLE] ADD  CONSTRAINT [DF__RBOTENDER__DATAA__1A8DDD94]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISUPDATESMASTER]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSISUPDATESMASTER](
		[UPDATEID] [int] NOT NULL,
		[STOREID] [nvarchar](10) NOT NULL,
		[POSID] [nvarchar](10) NOT NULL,
		[FILEID] [int] NOT NULL,
		[FILECREATEDATE] [datetime] NULL,
		[STATUS] [int] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		[TEXT] [nvarchar](250) NULL,
		[HIGHPRIORITY] [int] NULL,
		[FOLDERPATH] [nvarchar](20) NULL,
		[POSAPPLICATIONPATH] [nvarchar](100) NULL,
		[USERNAME] [nvarchar](50) NULL,
		[NAME] [nvarchar](50) NULL,
		[FILEVERSION] [nvarchar](30) NULL,
		[FILEMODIFIEDDATE] [datetime] NULL,
		[COMPANY] [nvarchar](40) NULL,
		[DESCRIPTION] [nvarchar](255) NULL,
		[SCHEDULED] [datetime] NULL,
	 CONSTRAINT [PK_POSISUPDATESMASTER] PRIMARY KEY CLUSTERED 
	(
		[UPDATEID] ASC,
		[STOREID] ASC,
		[POSID] ASC,
		[FILEID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOCOLORGROUPTABLE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOCOLORGROUPTABLE](
		[COLORGROUP] [nvarchar](10) NOT NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_20092GROUPIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[COLORGROUP] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RBOCOLORGROUPTABLE] ADD  CONSTRAINT [DF__RBOCOLORG__COLOR__3BAEDA3D]  DEFAULT ('') FOR [COLORGROUP]

	ALTER TABLE [dbo].[RBOCOLORGROUPTABLE] ADD  CONSTRAINT [DF__RBOCOLORG__DESCR__3CA2FE76]  DEFAULT ('') FOR [DESCRIPTION]

	ALTER TABLE [dbo].[RBOCOLORGROUPTABLE] ADD  CONSTRAINT [DF__RBOCOLORG__DATAA__3D9722AF]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSIZEGROUPTABLE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOSIZEGROUPTABLE](
		[SIZEGROUP] [nvarchar](10) NOT NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_20133GROUPIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[SIZEGROUP] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RBOSIZEGROUPTABLE] ADD  CONSTRAINT [DF__RBOSIZEGR__SIZEG__77E3BFD5]  DEFAULT ('') FOR [SIZEGROUP]

	ALTER TABLE [dbo].[RBOSIZEGROUPTABLE] ADD  CONSTRAINT [DF__RBOSIZEGR__DESCR__78D7E40E]  DEFAULT ('') FOR [DESCRIPTION]

	ALTER TABLE [dbo].[RBOSIZEGROUPTABLE] ADD  CONSTRAINT [DF__RBOSIZEGR__DATAA__79CC0847]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSTYLEGROUPTABLE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOSTYLEGROUPTABLE](
		[STYLEGROUP] [nvarchar](10) NOT NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_20152GROUPIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[STYLEGROUP] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTABLE] ADD  CONSTRAINT [DF__RBOSTYLEG__STYLE__7EE5C31F]  DEFAULT ('') FOR [STYLEGROUP]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTABLE] ADD  CONSTRAINT [DF__RBOSTYLEG__DESCR__7FD9E758]  DEFAULT ('') FOR [DESCRIPTION]

	ALTER TABLE [dbo].[RBOSTYLEGROUPTABLE] ADD  CONSTRAINT [DF__RBOSTYLEG__DATAA__00CE0B91]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[BARCODESETUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[BARCODESETUP](
		[BARCODESETUPID] [nvarchar](10) NOT NULL,
		[BARCODETYPE] [int] NULL,
		[FONTNAME] [nvarchar](32) NULL,
		[FONTSIZE] [int] NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[MINIMUMLENGTH] [int] NULL,
		[MAXIMUMLENGTH] [int] NULL,
		[RBOBARCODEMASK] [nvarchar](22) NULL,
		[MODIFIEDDATE] [datetime] NULL,
		[MODIFIEDTIME] [int] NULL,
		[MODIFIEDBY] [nvarchar](5) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_1214BARCODESETUPIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[BARCODESETUPID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__BARCO__7579F271]  DEFAULT ('') FOR [BARCODESETUPID]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__BARCO__766E16AA]  DEFAULT ((0)) FOR [BARCODETYPE]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__FONTN__77623AE3]  DEFAULT ('') FOR [FONTNAME]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__FONTS__78565F1C]  DEFAULT ((0)) FOR [FONTSIZE]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__DESCR__794A8355]  DEFAULT ('') FOR [DESCRIPTION]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__MINIM__7A3EA78E]  DEFAULT ((0)) FOR [MINIMUMLENGTH]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__MAXIM__7B32CBC7]  DEFAULT ((0)) FOR [MAXIMUMLENGTH]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__RBOBA__7C26F000]  DEFAULT ('') FOR [RBOBARCODEMASK]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__MODIF__7D1B1439]  DEFAULT ('1900-01-01 00:00:00.000') FOR [MODIFIEDDATE]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__MODIF__7E0F3872]  DEFAULT ((0)) FOR [MODIFIEDTIME]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__MODIF__7F035CAB]  DEFAULT ('?') FOR [MODIFIEDBY]

	ALTER TABLE [dbo].[BARCODESETUP] ADD  CONSTRAINT [DF__BARCODESE__DATAA__7FF780E4]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTDIMGROUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[INVENTDIMGROUP](
		[DIMGROUPID] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](30) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_INVENTDIMGROUP] PRIMARY KEY CLUSTERED 
	(
		[DIMGROUPID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[INVENTDIMGROUP] ADD  CONSTRAINT [DF__INVENTDIM__DIMGR__40712D55]  DEFAULT ('') FOR [DIMGROUPID]

	ALTER TABLE [dbo].[INVENTDIMGROUP] ADD  CONSTRAINT [DF__INVENTDIMG__NAME__4165518E]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[INVENTDIMGROUP] ADD  CONSTRAINT [DF__INVENTDIM__DATAA__425975C7]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

-- 15.6.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[TAXITEMGROUPHEADING]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[TAXITEMGROUPHEADING](
		[TAXITEMGROUP] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_TAXITEMGROUPHEADING] PRIMARY KEY CLUSTERED 
	(
		[TAXITEMGROUP] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[TAXITEMGROUPHEADING] ADD  CONSTRAINT [DF_TAXITEMGROUPHEADING_NAME]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[TAXITEMGROUPHEADING] ADD  CONSTRAINT [DF_TAXITEMGROUPHEADING_DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

-- 1.7.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[DECIMALSETTINGS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[DECIMALSETTINGS](
		[ID] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[MINDECIMALS] [int] NULL,
		[MAXDECIMALS] [int] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_DECIMALSETTINGS] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[DECIMALSETTINGS] ADD  CONSTRAINT [DF_DECIMALSETTING_ID]  DEFAULT ('') FOR [ID]

	ALTER TABLE [dbo].[DECIMALSETTINGS] ADD  CONSTRAINT [DF_DECIMALSETTING_NAME]  DEFAULT ('') FOR [NAME]

	ALTER TABLE [dbo].[DECIMALSETTINGS] ADD  CONSTRAINT [DF_DECIMALSETTING_DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

-- 7.7.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISPARAMETERS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSISPARAMETERS](
		[WEBSERVICEURL] [nvarchar](255) NOT NULL,
		[WEBSERVICEUSERNAME] [nvarchar](50) NOT NULL,
		[WEBSERVICEPASSWORD] [nvarchar](50) NOT NULL,
		[KEY_] [int] NOT NULL,
		[MODIFIEDDATETIME] [datetime] NOT NULL,
		[DEL_MODIFIED] [int] NOT NULL,
		[MODIFIEDBY] [nvarchar](5) NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_PSISPARAMETERS] PRIMARY KEY CLUSTERED 
	(
		[KEY_] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[POSISPARAMETERS] ADD  CONSTRAINT [DF_POSISPARAMETERS_WEBSERVICEURL]  DEFAULT ('Not set') FOR [WEBSERVICEURL]

	ALTER TABLE [dbo].[POSISPARAMETERS] ADD  CONSTRAINT [DF_POSISPARAMETERS_MODIFIEDDATETIME]  DEFAULT ('1900-01-01 00:00:00.000') FOR [MODIFIEDDATETIME]

	ALTER TABLE [dbo].[POSISPARAMETERS] ADD  CONSTRAINT [DF_POSISPARAMETERS_DEL_MODIFIED]  DEFAULT ((0)) FOR [DEL_MODIFIED]
END
GO

-- 13.7 2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[REPLICATIONACTIONS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[REPLICATIONACTIONS](
		[ActionID] [int] IDENTITY(1,1) NOT NULL,
		[Action] [int] NOT NULL,
		[ObjectName] [nvarchar](50) NOT NULL,
		[AuditContext] [uniqueidentifier] NOT NULL,
		[Parameters] [ntext] NOT NULL,
		[DateCreated] [datetime] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_REPLICATIONACTIONS] PRIMARY KEY CLUSTERED 
	(
		[ActionID] ASC,
		[DATAAREAID]
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

-- 16.07.2010
IF NOT EXISTS(SELECT * FROM sys.indexes where object_id = OBJECT_ID('CUSTTABLE') AND NAME = 'Index_Name')
BEGIN
	CREATE NONCLUSTERED INDEX [Index_Name] ON [dbo].[CUSTTABLE] 
	(
		[NAME] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
GO

-- 27.07.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTITEMDEPARTMENT]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOINVENTITEMDEPARTMENT](
		[DEPARTMENTID] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[DEFAULTPROFIT] [numeric](28, 12) NULL,
		[DISPENSEPRINTERGROUPID] [nvarchar](10) NULL,
		[DISPENSEPRINTERSEQNUM] [int] NULL,
		[NAMEALIAS] [nvarchar](20) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [I_20111DEPARTMENTIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[DEPARTMENTID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENT__DEPAR__3B44C5E9]  DEFAULT ('') FOR [DEPARTMENTID]
	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENTI__NAME__3C38EA22]  DEFAULT ('') FOR [NAME]
	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENT__DEFAU__3D2D0E5B]  DEFAULT ((0)) FOR [DEFAULTPROFIT]
	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENT__DISPE__3E213294]  DEFAULT ('') FOR [DISPENSEPRINTERGROUPID]
	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENT__DISPE__3F1556CD]  DEFAULT ((0)) FOR [DISPENSEPRINTERSEQNUM]
	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENT__NAMEA__40097B06]  DEFAULT ('') FOR [NAMEALIAS]
	ALTER TABLE [dbo].[RBOINVENTITEMDEPARTMENT] ADD  CONSTRAINT [DF__RBOINVENT__DATAA__43DA0BEA]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

-- 28.07.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTITEMRETAILGROUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOINVENTITEMRETAILGROUP](
		[GROUPID] [nvarchar](10) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[NAMEALIAS] [nvarchar](20) NULL,
		[DEPARTMENTID] [nvarchar](10) NULL,
		[SIZEGROUPID] [nvarchar](10) NULL,
		[COLORGROUPID] [nvarchar](10) NULL,
		[ITEMREPORTNAME] [nvarchar](40) NULL,
		[SHELFREPORTNAME] [nvarchar](40) NULL,
		[DISPENSEPRINTERGROUPID] [nvarchar](10) NULL,
		[BARCODESETUPID] [nvarchar](10) NULL,
		[DISPENSEPRINTINGDISABLED] [int] NULL,
		[USEEANSTANDARDBARCODE] [int] NULL,
		[POSINVENTORYLOOKUP] [int] NULL,
		[STYLEGROUPID] [nvarchar](10) NULL,
		[FSHREPLENISHMENTRULEID] [nvarchar](10) NULL,
		[ITEMGROUPID] [nvarchar](10) NULL,
		[INVENTLOCATIONIDFORPURCHA16016] [nvarchar](10) NULL,
		[INVENTLOCATIONIDFORINVENTORY] [nvarchar](10) NULL,
		[INVENTLOCATIONIDFORSALESORDER] [nvarchar](10) NULL,
		[INVENTMODELGROUPID] [nvarchar](10) NULL,
		[INVENTDIMGROUPID] [nvarchar](10) NULL,
		[SALESTAXITEMGROUP] [nvarchar](10) NULL,
		[PURCHASETAXITEMGROUP] [nvarchar](10) NULL,
		[BASECOMPARISONUNITCODE] [nvarchar](10) NULL,
		[KEYINGINPRICE] [int] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		[LSRCOMMISSIONGROUPID] [nvarchar](10) NULL,
	 CONSTRAINT [I_17289GROUPIDX] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[GROUPID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[RBOINVENTITEMRETAILGROUP] ADD  CONSTRAINT [DF_RBOINVENTITEMRETAILGROUP_DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[HOSPITALITYSETUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[HOSPITALITYSETUP](
		[SETUP] [nvarchar](10) NOT NULL,
		[DELIVERYSALESTYPE] [nvarchar](20) NULL,
		[DINEINSALESTYPE] [nvarchar](20) NULL,
		[ORDERPROCESSTIMEMIN] [int] NULL,
		[TABLEFREECOLORB] [int] NULL,
		[TABLENOTAVAILCOLORB] [int] NULL,
		[TABLELOCKEDCOLORB] [int] NULL,
		[ORDERNOTPRINTEDCOLORB] [int] NULL,
		[ORDERPRINTEDCOLORB] [int] NULL,
		[ORDERSTARTEDCOLORB] [int] NULL,
		[ORDERFINISHEDCOLORB] [int] NULL,
		[ORDERCONFIRMEDCOLORB] [int] NULL,
		[TABLEFREECOLORF] [nvarchar](10) NULL,
		[TABLENOTAVAILCOLORF] [nvarchar](10) NULL,
		[TABLELOCKEDCOLORF] [nvarchar](10) NULL,
		[ORDERNOTPRINTEDCOLORF] [nvarchar](10) NULL,
		[ORDERPRINTEDCOLORF] [nvarchar](10) NULL,
		[ORDERSTARTEDCOLORF] [nvarchar](10) NULL,
		[ORDERFINISHEDCOLORF] [nvarchar](10) NULL,
		[ORDERCONFIRMEDCOLORF] [nvarchar](10) NULL,
		[CONFIRMSTATIONPRINTING] [tinyint] NULL,
		[REQUESTNOOFGUESTS] [tinyint] NULL,
		[NOOFDINEINTABLESCOL] [int] NULL,
		[NOOFDINEINTABLESROWS] [int] NULL,
		[STATIONPRINTING] [int] NULL,
		[DINEINTABLELOCKING] [int] NULL,
		[DINEINTABLESELECTION] [int] NULL,
		[PERIOD1TIMEFROM] [time](7) NULL,
		[PERIOD1TIMETO] [time](7) NULL,
		[PERIOD2TIMEFROM] [time](7) NULL,
		[PERIOD2TIMETO] [time](7) NULL,
		[PERIOD3TIMEFROM] [time](7) NULL,
		[PERIOD3TIMETO] [time](7) NULL,
		[PERIOD4TIMEFROM] [time](7) NULL,
		[PERIOD4TIMETO] [time](7) NULL,
		[AUTOLOGOFFATPOSEXIT] [tinyint] NULL,
		[TAKEOUTSALESTYPE] [nvarchar](20) NULL,
		[PREORDERSALESTYPE] [nvarchar](20) NULL,
		[LOGSTATIONPRINTING] [tinyint] NULL,
		[POPULATEDELIVERYINFOCODES] [tinyint] NULL,
		[ALLOWPREORDERS] [tinyint] NULL,
		[TAKEOUTNONAMENO] [nvarchar](20) NULL,
		[ADVPREORDPRINTMIN] [int] NULL,
		[CLOSETRIPONDEPART] [tinyint] NULL,
		[DELPROGRESSSTATUSINUSE] [tinyint] NULL,
		[DAYSBOMPRINTEXIST] [int] NULL,
		[DAYSBOMMONITOREXIST] [int] NULL,
		[DAYSDRIVERTRIPSEXIST] [int] NULL,
		[POSTERMINALPRINTPREORDERS] [nvarchar](10) NULL,
		[DISPLAYTIMEATORDERTAKING] [tinyint] NULL,
		[NORMALPOSSALESTYPE] [nvarchar](20) NULL,
		[ORDLISTSCROLLPAGESIZE] [int] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
	) ON [PRIMARY]

	ALTER TABLE dbo.[HOSPITALITYSETUP] ADD CONSTRAINT PK_HOSPITALITYSETUP
	PRIMARY KEY CLUSTERED (SETUP,DATAAREAID)
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[SALESTYPE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.SALESTYPE
		(
		CODE nvarchar (20) NOT NULL,
		DESCRIPTION nvarchar (30) NULL,
		REQUESTSALESPERSON tinyint NULL,
		REQUESTDEPOSITPERC int NULL,
		REQUESTCHARGEACCOUNT tinyint NULL,
		PURCHASINGCODE nvarchar (10) NULL,
		DEFAULTORDERLIMIT numeric(28,12) NULL,
		LIMITSETTING int NULL,
		REQUESTCONFIRMATION tinyint NULL,
		REQUESTDESCRIPTION tinyint NULL,
		NEWGLOBALDIMENSION2 nvarchar(20) NULL,
		SUSPENDPRINTING int NULL,
		SUSPENDTYPE int NULL,
		PREPAYMENTACCOUNTNO nvarchar(20) NULL,
		MINIMUMDEPOSIT numeric(28,12) NULL,
		PRINTITEMLINESONPOSSLIP tinyint NULL,
		VOIDEDPREPAYMENTACCOUNTNO nvarchar(20) NULL,
		DAYSOPENTRANSEXIST int NULL,
		TAXGROUPID nvarchar(10) NULL,
		PRICEGROUP nvarchar(10) NULL,
		TRANSDELETEREMINDER int NULL,
		LOCATIONCODE nvarchar(10) NULL,
		PAYMENTISPREPAYMENT tinyint NULL,
		CALCPRICEFROMVATPRICE tinyint NULL,
		DATAAREAID nvarchar(4) NOT NULL
		) ON [PRIMARY]

	ALTER TABLE dbo.SALESTYPE ADD CONSTRAINT PK_SALESTYPE
	PRIMARY KEY CLUSTERED (CODE,DATAAREAID)
END
GO

-- 29.07.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSCOLOR]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.POSCOLOR
		(
		COLORCODE nvarchar (10) NOT NULL,
		DESCRIPTION nvarchar (30) NULL,
		COLOR int NULL,
		BOLD tinyint NULL,
		DATAAREAID nvarchar (4) NOT NULL
		) ON [PRIMARY]

	ALTER TABLE dbo.POSCOLOR ADD CONSTRAINT PK_POSCOLOR
	PRIMARY KEY CLUSTERED (COLORCODE,DATAAREAID)
END
GO

-- 9.8.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSPECIALGROUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.RBOSPECIALGROUP
		(
		GROUPID nvarchar (10) NOT NULL,
		NAME nvarchar (60) NULL,
		DATAAREAID nvarchar (4) NOT NULL
		) ON [PRIMARY]

	ALTER TABLE dbo.RBOSPECIALGROUP ADD CONSTRAINT PK_RBOSPECIALGROUP
	PRIMARY KEY CLUSTERED (GROUPID,DATAAREAID)
END
GO

-- 23.8.2010
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMMLINEGROUPS') AND NAME='COLOR')
BEGIN
	alter table POSMMLINEGROUPS
	add COLOR [int] NULL
END
GO

-- 03.09.2010
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISLICENSE') AND NAME='REPLICATED')
BEGIN
	alter table POSISLICENSE
	add REPLICATED [TINYINT] NULL
END
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISLICENSE') AND NAME='ERRORTXT')
BEGIN
	alter table POSISLICENSE
	add ERRORTXT [nvarchar](80) NULL
END
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISLICENSE') AND NAME='ERROROCCURED')
BEGIN
	alter table POSISLICENSE
	add ERROROCCURED [TINYINT] NULL
END
GO


-- 15.09.2010
IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PRICEGROUP]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.PRICEGROUP
		(
		GROUPID nvarchar (10) NOT NULL,
		NAME nvarchar (60) NULL,
		DATAAREAID nvarchar (4) NOT NULL
		) ON [PRIMARY]

	ALTER TABLE dbo.PRICEGROUP ADD CONSTRAINT PK_PRICEGROUP
	PRIMARY KEY CLUSTERED (GROUPID,DATAAREAID)
END
GO

-- 21.09.2010
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSHARDWAREPROFILE') AND NAME='FISCALPRINTER')
BEGIN
	alter table POSHARDWAREPROFILE
	add FISCALPRINTER int NULL
END
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSHARDWAREPROFILE') AND NAME='FISCALPRINTERCONNECTION')
BEGIN
	alter table POSHARDWAREPROFILE
    add FISCALPRINTERCONNECTION nvarchar(30) NULL
END
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSHARDWAREPROFILE') AND NAME='FISCALPRINTERDESCRIPTION')
BEGIN
	alter table POSHARDWAREPROFILE
	add FISCALPRINTERDESCRIPTION nvarchar(30) NULL
END
GO

-- 06.10.2010
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('INVENTTABLEMODULE') AND NAME='PRICEINCLVAT')
BEGIN
	alter table INVENTTABLEMODULE
	add PRICEINCLVAT numeric(28,12) NULL
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTTRANS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.INVENTTRANS
	(
		[DATE] datetime NOT NULL,
		[ITEMID] nvarchar(20) NOT NULL,
		[VARIANTID] nvarchar(10) NOT NULL,
		[STOREID] nvarchar(10) NOT NULL,
		[QUANTITY][numeric](28, 12) NOT NULL,
		[TYPE] int NOT NULL,
		[OFFERID] nvarchar(10) NULL,
		[COSTPRICE][numeric](28, 12) NULL,
		[SALESPRICE][numeric](28, 12) NULL,
		[SALESWITHTAX][numeric](28, 12) NULL,
		[REASONCODE] nvarchar(10) NULL,
		DATAAREAID nvarchar(4) NOT NULL
	)  ON [PRIMARY]

	ALTER TABLE dbo.INVENTTRANS ADD CONSTRAINT PK_INVENTTRANS
	PRIMARY KEY CLUSTERED ([DATE],[ITEMID],[VARIANTID],[STOREID],DATAAREAID)
	
	CREATE NONCLUSTERED INDEX IX_INVENTTRANS_TYPE ON dbo.INVENTTRANS (TYPE) 
	WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX IX_INVENTTRANS_OFFERID ON dbo.INVENTTRANS (OFFERID) 
	WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTSUM]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.INVENTSUM
	(
		[ITEMID] nvarchar(20) NOT NULL,
		[VARIANTID] nvarchar(10) NOT NULL,
		[STOREID] nvarchar(10) NOT NULL,
		[QUANTITY][numeric](28, 12) NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL
	)  ON [PRIMARY]

	ALTER TABLE dbo.INVENTSUM ADD CONSTRAINT PK_INVENTSUM
	PRIMARY KEY CLUSTERED ([ITEMID],[VARIANTID],[STOREID],DATAAREAID)
END

GO