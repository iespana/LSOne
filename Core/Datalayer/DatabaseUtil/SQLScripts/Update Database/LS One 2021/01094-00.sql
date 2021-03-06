/*
	Incident No.	: ONE-9073
	Responsible		: Celia Blacu
	Sprint			: Renminbi
	Date created	: 07.12.2020

	Description		: Enabling Embed Store/Terminal ID for Z RPT. Increasing field size for ZREPORTID in tables POSZREPORT and RBOTRANSACTIONTABLE
*/

USE LSPOSNET
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('POSZREPORT'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	DECLARE @CONSTRAINTNAME NVARCHAR(128)
	SELECT 
		@CONSTRAINTNAME = K.CONSTRAINT_NAME
	FROM 
		INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
		JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K
	ON C.TABLE_NAME = K.TABLE_NAME
		AND C.CONSTRAINT_CATALOG = K.CONSTRAINT_CATALOG
		AND C.CONSTRAINT_SCHEMA = K.CONSTRAINT_SCHEMA
		AND C.CONSTRAINT_NAME = K.CONSTRAINT_NAME
	WHERE 
		C.CONSTRAINT_TYPE = 'PRIMARY KEY' AND K.TABLE_NAME = 'POSZREPORT' AND K.COLUMN_NAME = 'ZREPORTID'

	IF @CONSTRAINTNAME IS NOT NULL
		EXEC('ALTER TABLE [dbo].[POSZREPORT] DROP CONSTRAINT [' + @CONSTRAINTNAME + '] WITH ( ONLINE = OFF )')
	
	ALTER TABLE [dbo].[POSZREPORT]
		ALTER COLUMN ZREPORTID NVARCHAR(61) NOT NULL

	EXEC (
		'ALTER TABLE [dbo].[POSZREPORT] ADD  CONSTRAINT ['+ @CONSTRAINTNAME + '] PRIMARY KEY CLUSTERED 
		(
			[ZREPORTID] ASC,
			[STOREID] ASC,
			[TERMINALID] ASC,
			[DATAAREAID] ASC
		)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]'
	)
	
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTRANSACTIONTABLE'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONTABLE]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTRANSACTIONPAYMENTTRANS'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONPAYMENTTRANS]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOSTAFFTABLE'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOSTAFFTABLE]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTERMINALTABLE'), 'LASTZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTERMINALTABLE]
		ALTER COLUMN LASTZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTRANSACTIONBANKEDTENDE20338'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONBANKEDTENDE20338]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTRANSACTIONINCOMEEXPEN20158'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONINCOMEEXPEN20158]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTRANSACTIONSAFETENDERTRANS'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONSAFETENDERTRANS]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO

IF 	
	(SELECT COLUMNPROPERTY(OBJECT_ID('RBOTRANSACTIONTENDERDECLA20165'), 'ZREPORTID', 'PRECISION')) < 61
BEGIN
	ALTER TABLE [dbo].[RBOTRANSACTIONTENDERDECLA20165]
		ALTER COLUMN ZREPORTID nvarchar(61)
END
GO