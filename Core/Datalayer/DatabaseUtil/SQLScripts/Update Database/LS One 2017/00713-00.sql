/*

	Incident No.	: ONE-4639
	Responsible		: Indri�i
	Sprint			: Garlic 2.12
	Date created	: 2.12.2016

	Description		: Alter columns for EFTINFO
	
	
	Tables affected	: New table added RBOTRANSACTIONEFTINFOTRANS
						
*/

USE LSPOSNET
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' AND COLUMN_NAME = 'STORE' AND CHARACTER_MAXIMUM_LENGTH <20)
	ALTER TABLE RBOTRANSACTIONEFTINFOTRANS
	ALTER COLUMN STORE nvarchar(20) NOT NULL
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' AND COLUMN_NAME = 'TERMINAL' AND CHARACTER_MAXIMUM_LENGTH <20)
	ALTER TABLE RBOTRANSACTIONEFTINFOTRANS
	ALTER COLUMN TERMINAL NVARCHAR(20) NOT NULL
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' AND  COLUMN_NAME = 'AMOUNTINCENTS' AND DATA_TYPE = 'INT')
	ALTER TABLE RBOTRANSACTIONEFTINFOTRANS
	ALTER COLUMN AMOUNTINCENTS NUMERIC(28, 12) NULL
GO