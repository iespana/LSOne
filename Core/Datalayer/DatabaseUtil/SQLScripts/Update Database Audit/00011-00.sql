
/*

	Incident No.	: 
	Responsible		: GuÃ°bjÃ¶rn Einarsson
	Sprint			: LS Retail .NET v 2010 - Sprint 4
	Date created	: 28.12.2010
	
	Description		: Change RBOSTATEMENTLINELog and RBOSTATEMENTTABLELog so that their normal primary keys are changed from int to nvarchar(20)

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- RBOSTATEMENTLINELog - columns changed
						- RBOSTATEMENTTABLELog - column changed
						
*/


USE LSPOSNET_Audit

GO

IF 'int' IN (SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'RBOSTATEMENTLINELog' AND COLUMN_NAME = 'STATEMENTID')
BEGIN
	alter table RBOSTATEMENTLINELog
	drop column STATEMENTID
	
	alter table RBOSTATEMENTLINELog
	add STATEMENTID nvarchar(20) 
END
GO

IF 'int' IN (SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'RBOSTATEMENTLINELog' AND COLUMN_NAME = 'LINENUMBER')
BEGIN
	alter table RBOSTATEMENTLINELog
	drop column LINENUMBER
	
	alter table RBOSTATEMENTLINELog
	add LINENUMBER nvarchar(20) 
END
GO

IF 'int' IN (SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'RBOSTATEMENTTABLELog' AND COLUMN_NAME = 'STATEMENTID')
BEGIN
	alter table RBOSTATEMENTTABLELog
	drop column STATEMENTID
	
	alter table RBOSTATEMENTTABLELog
	add STATEMENTID nvarchar(20) 
END
GO
