
/*

	Incident No.	: 12175
	Responsible		: Guðbjörn Einarsson
	Sprint			: Freyr
	Date created	: 06.10.2011
	
	Description		: Add column SUSPENDALLOWEOD to table RBOSTORETABLE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : RBOSTORETABLE - SUSPENDALLOWEOD added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOSTORETABLELog' AND COLUMN_NAME='SUSPENDALLOWEOD')
Begin
	Alter Table RBOSTORETABLELog
	ADD SUSPENDALLOWEOD int NOT NULL default 3
End
GO