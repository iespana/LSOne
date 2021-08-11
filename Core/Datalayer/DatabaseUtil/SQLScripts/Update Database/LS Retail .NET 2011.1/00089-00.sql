
/*

	Incident No.	: XXX
	Responsible		: Guðbjörn Einarsson
	Sprint			: N/A
	Date created	: 25.05.2011

	Description		: Add a column to TAXITEMGROUPHEADING table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: TAXITEMGROUPHEADING - column RECEIPTDISPLAY added
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXITEMGROUPHEADING') AND NAME='RECEIPTDISPLAY')
ALTER TABLE dbo.TAXITEMGROUPHEADING ADD RECEIPTDISPLAY NVARCHAR(30) NULL

GO