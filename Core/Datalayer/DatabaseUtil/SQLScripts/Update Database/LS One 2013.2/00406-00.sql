﻿
/*

	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: 
	Date created	: 28.08.13

	Description		: Added new retail division table
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RBOINVENTITEMRETAILDIVISION')
BEGIN
	CREATE TABLE [dbo].[RBOINVENTITEMRETAILDIVISION](
		[DIVISIONID] [nvarchar](20) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [DIVISION_PK] PRIMARY KEY CLUSTERED 
	(
		[DATAAREAID] ASC,
		[DIVISIONID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOINVENTITEMDEPARTMENT' AND COLUMN_NAME = 'DIVISIONID')
BEGIN
	ALTER TABLE RBOINVENTITEMDEPARTMENT ADD DIVISIONID NVARCHAR(20)
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOINVENTTABLE' AND COLUMN_NAME = 'DIVISIONGROUP')
BEGIN
	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOINVENTTABLE' AND COLUMN_NAME = 'DIVISIONID')
	BEGIN
		EXEC sp_rename @objname = 'RBOINVENTTABLE.DIVISIONGROUP', @newname = 'DIVISIONID', @objtype = 'COLUMN'
	END
END
GO
