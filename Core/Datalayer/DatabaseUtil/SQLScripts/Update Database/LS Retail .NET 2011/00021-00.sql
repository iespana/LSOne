﻿
/*

	Incident No.	: n/a
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 03\SC Team
	Date created	: 02.12.2010

	Description		: Modifying columns for HOSPITALITYTYPE table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: 	HospitalityType	 - column datatypes/lengths modified						
						
*/

use LSPOSNET

go


IF 'nchar' IN (SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'HOSPITALITYTYPE' AND COLUMN_NAME = 'RESTAURANTID')
BEGIN
	alter table HOSPITALITYTYPE
	drop constraint PK_HOSPITALITYTYPE
	
	alter table HOSPITALITYTYPE
	alter column RESTAURANTID nvarchar(20) not null
	
	alter table HOSPITALITYTYPE add constraint PK_HOSPITALITYTYPE primary key clustered
	(
		[RESTAURANTID] ASC,
		[SEQUENCE] ASC,
		[SALESTYPE] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)		
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'ACCESSTOOTHERRESTAURANT')
BEGIN
	alter table HOSPITALITYTYPE
	alter column ACCESSTOOTHERRESTAURANT nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'POSLOGONMENUID')
BEGIN
	alter table HOSPITALITYTYPE
	alter column POSLOGONMENUID nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'TABLEBUTTONPOSMENUID')
BEGIN
	alter table HOSPITALITYTYPE
	alter column TABLEBUTTONPOSMENUID nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'SPLITBILLLOOKUPID')
BEGIN
	alter table HOSPITALITYTYPE
	alter column SPLITBILLLOOKUPID nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'TRANSFERLINESLOOKUPID')
BEGIN
	alter table HOSPITALITYTYPE
	alter column TRANSFERLINESLOOKUPID nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'LAYOUTID')
BEGIN
	alter table HOSPITALITYTYPE
	alter column LAYOUTID nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'TOPPOSMENUID')
BEGIN
	alter table HOSPITALITYTYPE
	alter column TOPPOSMENUID nvarchar(20) null
END
GO

if 10 in (SELECT CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'HOSPITALITYTYPE' and COLUMN_NAME = 'SETTINGSFROMRESTAURANT')
BEGIN
	alter table HOSPITALITYTYPE
	alter column SETTINGSFROMRESTAURANT nvarchar(20) null
END
GO