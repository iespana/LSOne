/*

	Incident No.	: 14106	
	Responsible		: Hrólfur	
	Sprint			: LS Retail .NET 2012/Thor
	Date created	: 29.12.2011

	Description		: Adding fuel settings 

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboTransactionFuelTrans - Added OriginatesFromForeCourt
					  PosHardwareProfile	-	Added ForeCourtSuspendAllowed
					  
					  
					  
					  	
*/								

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOTRANSACTIONFUELTRANS') AND NAME='ORIGINATESFROMFORECOURT')
ALTER TABLE RBOTRANSACTIONFUELTRANS ADD [ORIGINATESFROMFORECOURT] [TINYINT] NULL
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSHARDWAREPROFILE') AND NAME='FORECOURTSUSPENDALLOWED')
ALTER TABLE POSHARDWAREPROFILE ADD [FORECOURTSUSPENDALLOWED] [TINYINT] NULL
GO
