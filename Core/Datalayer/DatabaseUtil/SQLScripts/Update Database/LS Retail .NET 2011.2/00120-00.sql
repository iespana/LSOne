
/*

	Incident No.	: 12175	
	Responsible		: Guðbjörn Einarsson	
	Sprint			: LS Retail .NET 2012/Freyr
	Date created	: 06.10.2011

	Description		: Add SuspendAllowEOD column to tables RBOSTORETABLE and RBOTERMINALTABLE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOSTORETABLE - Added SuspendAllowEOD	
					  RBOTERMINALTABLE - Added SuspendAllowEOD	
					  
					  	
*/								

USE LSPOSNET

GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOSTORETABLE' AND COLUMN_NAME='SUSPENDALLOWEOD')
Begin
	Alter Table RBOSTORETABLE
	ADD SUSPENDALLOWEOD int NOT NULL default 3
End
GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOTERMINALTABLE' AND COLUMN_NAME='SUSPENDALLOWEOD')
Begin
	Alter Table RBOTERMINALTABLE
	ADD SUSPENDALLOWEOD int NOT NULL default 3
End
GO