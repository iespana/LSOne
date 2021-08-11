/*

	Incident No.	: 13238
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012/Mímir
	Date created	: 28.11.2011

	Description		: Add a new field to table SalesParameters

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: SalesParameters - new fields added
						
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SALESPARAMETERSLog' AND COLUMN_NAME='CALCCUSTOMERDISCS')
Begin
	Alter table SALESPARAMETERSLog
	Add CALCCUSTOMERDISCS int NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SALESPARAMETERSLog' AND COLUMN_NAME='CALCPERIODICDISCS')
Begin
	Alter table SALESPARAMETERSLog
	Add CALCPERIODICDISCS int NULL
END
GO


