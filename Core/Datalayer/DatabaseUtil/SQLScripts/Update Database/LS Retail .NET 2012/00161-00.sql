
/*

	Incident No.	: 13783
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012\Askur
	Date created	: 17.02.2012

	Description		: Adding fields to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboTransactiontable - new field UnconcludedTrans added
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOTRANSACTIONTABLE') AND NAME='UNCONCLUDEDTRANS')
ALTER TABLE dbo.RBOTRANSACTIONTABLE ADD UNCONCLUDEDTRANS tinyint NULL
GO






