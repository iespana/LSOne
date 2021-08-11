
/*

	Incident No.	: 11748
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012/Tyr
	Date created	: 12.09.2001

	Description		: Add fields to the GiftCardTable

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboGiftCardTable			- fields added, default values added to fields
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOGIFTCARDTABLE') AND NAME='REFILLABLE')
ALTER TABLE dbo.RBOGIFTCARDTABLE ADD REFILLABLE TINYINT NULL

GO



