/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2012 - Sprint Baldur
	Date created	: 28.07.2011

	Description		: New columns to save Store Server host and port for Store Controller

	Logic scripts   : N.A.
	
	Table affected	: RBOPARAMETERS
						
						
*/


USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOPARAMETERS') AND NAME='SCSTORESERVERHOST')
BEGIN
	ALTER TABLE dbo.RBOPARAMETERS ADD SCSTORESERVERHOST nvarchar(100) NULL
END

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOPARAMETERS') AND NAME='SCSTORESERVERPORT')
BEGIN
	ALTER TABLE dbo.RBOPARAMETERS ADD SCSTORESERVERPORT int NULL
END

exec spDB_SetFieldDescription_1_0 'RBOPARAMETERS','SCSTORESERVERHOST','Host name of a Store Server for a Store Controller to connect to'
exec spDB_SetFieldDescription_1_0 'RBOPARAMETERS','SCSTORESERVERPORT','Port number of for a Store Server host'

GO


