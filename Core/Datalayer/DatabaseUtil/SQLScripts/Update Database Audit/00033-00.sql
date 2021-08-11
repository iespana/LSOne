
/*

	Incident No.	: 12177
	Responsible		: Guðbjörn Einarsson
	Sprint			: Freyr
	Date created	: 07.10.2011
	
	Description		: Add column SUSPENDALLOWEOD to table RBOTERMINALTABLE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : RBOTERMINALTABLE - SUSPENDALLOWEOD added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOTERMINALTABLELog' AND COLUMN_NAME='SUSPENDALLOWEOD')
Begin
	Alter Table RBOTERMINALTABLELog
	ADD SUSPENDALLOWEOD int NOT NULL default 3
End
GO