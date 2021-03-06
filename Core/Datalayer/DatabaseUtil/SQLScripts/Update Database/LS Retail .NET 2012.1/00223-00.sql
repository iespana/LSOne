/*

	Incident No.	: 18003
	Responsible		: Sigfús Jóhannesson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 02.08.2012

	Description		: Add DataAreaID to POSISRFIDTABLE
	
	
	Tables affected	: POSISRFIDTABLE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISRFIDTABLE') AND NAME='DATAAREAID')
BEGIN
	ALTER TABLE dbo.POSISRFIDTABLE ADD DATAAREAID NVARCHAR(4) NOT NULL DEFAULT ''
	ALTER TABLE dbo.POSISRFIDTABLE DROP CONSTRAINT PK_POSISRFIDTABLE
	ALTER TABLE dbo.POSISRFIDTABLE ADD CONSTRAINT PK_POSISRFIDTABLE Primary key (RFID, DATAAREAID)
END
GO
