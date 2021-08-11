
/*

	Incident No.	: 11747
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012/Tyr
	Date created	: 13.09.2011
	
	Description		: Add fields to RBOGIFTCARDTABLELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- RBOGIFTCARDTABLELog - fields added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOGIFTCARDTABLELog') AND NAME='REFILLABLE')
ALTER TABLE dbo.RBOGIFTCARDTABLELog ADD REFILLABLE TINYINT NULL



GO