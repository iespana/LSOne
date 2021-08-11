
/*

	Incident No.	: 9732
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS Retail.NET 2011.1\Sprint 2
	Date created	: 19.04.2011

	Description		: N/A

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboTerminalTable - new field added
						
*/


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOTERMINALTABLE') AND NAME='AUTOLOCKTERMINALTIMEOUT')
ALTER TABLE dbo.RBOTERMINALTABLE ADD AUTOLOCKTERMINALTIMEOUT INT NULL
GO

