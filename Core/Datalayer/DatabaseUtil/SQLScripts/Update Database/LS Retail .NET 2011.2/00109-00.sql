﻿
/*

	Incident No.	: 11589
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2012/Tyr
	Date created	: 09.09.2011

	Description		: Changes TAXDATA table Primary Key to a new Guid key

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: TAXDATA - PK changed
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'TAXDATA' AND CONSTRAINT_NAME = 'PK_TAXDATA')
Begin
	Alter Table TAXDATA
	DROP CONSTRAINT PK_TAXDATA
End

IF NOT EXISTS (SELECT 'X' FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXDATA') AND NAME='ID')
Begin
	ALTER TABLE TAXDATA ADD ID UniqueIdentifier Default NEWID() NOT NULL 
	ALTER TABLE TAXDATA ADD Primary Key (ID, DATAAREAID)
End

IF NOT EXISTS (SELECT 'X' FROM sys.indexes WHERE name = N'IX_TAXDATA_Old_Primary_Key')
Begin
	CREATE NONCLUSTERED INDEX IX_TAXDATA_Old_Primary_Key
	ON TAXDATA (TAXCODE, TAXLIMITMIN, TAXFROMDATE, TAXTODATE, DATAAREAID);
End


GO