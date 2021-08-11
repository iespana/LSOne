
/*

	Incident No.	: 9741
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012/Thor
	Date created	: 23.10.2011

	Description		: Default value set on one column

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisSuspendedTransaction - default value added to field NetAmount
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (select * from sys.objects where name = 'DF_POSISSUSPENDEDTRANSACTIONS_NETAMOUNT')
BEGIN
	ALTER TABLE dbo.POSISSUSPENDEDTRANSACTIONS ADD CONSTRAINT
		DF_POSISSUSPENDEDTRANSACTIONS_NETAMOUNT DEFAULT 0 FOR NETAMOUNT
END
GO
