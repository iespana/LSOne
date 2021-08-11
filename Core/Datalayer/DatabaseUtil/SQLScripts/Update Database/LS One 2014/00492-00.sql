﻿/*
	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 19.01.2014

	Description		: Fixed names
	
						
*/
USE LSPOSNET
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TAXRETURNRANGE')
BEGIN
	exec sp_rename 'dbo.TAXRETURNRANGE', 'TAXREFUNDRANGE'
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TAXREFUNDRANGE' AND COLUMN_NAME = 'TAXRETURN') 
BEGIN
	EXEC sp_rename 'dbo.TAXREFUNDRANGE.TAXRETURN', 'TAXREFUND', 'COLUMN';
	EXEC sp_rename 'dbo.TAXREFUNDRANGE.TAXRETURNPERCENTAGE', 'TAXREFUNDPERCENTAGE', 'COLUMN';
END
GO