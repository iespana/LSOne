﻿/*

	Incident No.	: 18492
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 10.09.2012

	Description		: Add column TOPLEFTCUSTOMSTRING, TOPCENTERCUSTOMSTRING, TOPRIGHTCUSTOMSTRING, BOTTOMLEFTCUSTOMSTRING, BOTTOMCENTERCUSTOMSTRING and BOTTOMRIGHTCUSTOMSTRING to KMINTERFACEPROFILE
	
	
	Tables affected	: KMINTERFACEPROFILE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='TOPLEFTCUSTOMSTRING')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILE ADD TOPLEFTCUSTOMSTRING NVARCHAR(50) NULL

END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='TOPCENTERCUSTOMSTRING')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILE ADD TOPCENTERCUSTOMSTRING NVARCHAR(50) NULL

END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='TOPRIGHTCUSTOMSTRING')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILE ADD TOPRIGHTCUSTOMSTRING NVARCHAR(50) NULL

END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='BOTTOMLEFTCUSTOMSTRING')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILE ADD BOTTOMLEFTCUSTOMSTRING NVARCHAR(50) NULL

END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='BOTTOMCENTERCUSTOMSTRING')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILE ADD BOTTOMCENTERCUSTOMSTRING NVARCHAR(50) NULL

END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='BOTTOMRIGHTCUSTOMSTRING')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILE ADD BOTTOMRIGHTCUSTOMSTRING NVARCHAR(50) NULL

END
GO