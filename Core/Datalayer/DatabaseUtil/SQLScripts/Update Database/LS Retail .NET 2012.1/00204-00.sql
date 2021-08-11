/*

	Incident No.	: 17539
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 27.06.2012

	Description		: Add ManuallyEnterVendorId to RBOParameters
	
	
	Tables affected	: RBOParameters
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOPARAMETERS') AND NAME='MANUALLYENTERVENDORID')
BEGIN
	ALTER TABLE dbo.RBOPARAMETERS ADD MANUALLYENTERVENDORID tinyint NULL
END
GO
