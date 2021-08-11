/*

	Incident No.	: 19561
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2013\Earth
	Date created	: 31.10.2012

	Description		: Adds a new field to POSMENULINE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMENULINE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMENULINE') AND NAME='STYLEID')
BEGIN
	ALTER TABLE dbo.POSMENULINE ADD STYLEID NVARCHAR(20)
END
GO

-- Update the Customer Add operation to be a User Operation
UPDATE POSISOPERATIONS
SET USEROPERATION = 1
WHERE OPERATIONID = 612
GO

