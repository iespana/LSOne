﻿/*

	Incident No.	: 19420
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS Retail .NET 2013\Earth
	Date created	: 25.10.2012

	Description		: Added keyboard code and layout name columns for POS keyboard to POS user
	
	
	Tables affected	: RBOSTAFFTABLE
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTAFFTABLE' AND COLUMN_NAME = 'KEYBOARDCODE')
BEGIN
	ALTER TABLE dbo.RBOSTAFFTABLE ADD KEYBOARDCODE nvarchar(20)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTAFFTABLE' AND COLUMN_NAME = 'LAYOUTNAME')
BEGIN
	ALTER TABLE dbo.RBOSTAFFTABLE ADD LAYOUTNAME nvarchar(50)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'KEYBOARDCODE')
BEGIN
	ALTER TABLE dbo.RBOSTORETABLE ADD KEYBOARDCODE nvarchar(20)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'LAYOUTNAME')
BEGIN
	ALTER TABLE dbo.RBOSTORETABLE ADD LAYOUTNAME nvarchar(50)
END
GO