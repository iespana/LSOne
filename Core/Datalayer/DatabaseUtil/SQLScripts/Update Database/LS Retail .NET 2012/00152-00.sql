/*

	Incident No.	: 13238
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012/Mímir
	Date created	: 28.11.2011

	Description		: Add a new field to table SalesParameters

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: SalesParameters - new fields added
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SALESPARAMETERS' AND COLUMN_NAME='CALCCUSTOMERDISCS')
Begin
	Alter table SALESPARAMETERS 
	Add CALCCUSTOMERDISCS int NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SALESPARAMETERS' AND COLUMN_NAME='CALCPERIODICDISCS')
Begin
	Alter table SALESPARAMETERS 
	Add CALCPERIODICDISCS int NULL
END
GO

-- To Document table description
exec spDB_SetTableDescription_1_0 'SALESPARAMETERS','Table that contains configurations about discount calculations'

-- To Document field description
exec spDB_SetFieldDescription_1_0 'SALESPARAMETERS','DISC','A configuration that controls how customer line discounts and multiline discounts interact with each other'
exec spDB_SetFieldDescription_1_0 'SALESPARAMETERS','KEY_','Unique value for the primary key - only one row can be in this table'
exec spDB_SetFieldDescription_1_0 'SALESPARAMETERS','DATAAREAID','DATAAREAID'
exec spDB_SetFieldDescription_1_0 'SALESPARAMETERS','CALCCUSTOMERDISCS','If set to true the customer line discount, multiline and total discounts are calculated automatically when an item is added to the transaction. When empty or null default value is true'
exec spDB_SetFieldDescription_1_0 'SALESPARAMETERS','CALCPERIODICDISCS','If set to true the periodic discounts are calculated automatically when an item is added to the transaction. When empty or null default value is true'

