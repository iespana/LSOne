
/*

	Incident No.	: 11748
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012/Tyr
	Date created	: 13.09.2001

	Description		: Update fields to the GiftCardTable

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboGiftCardTable			- fields added, default values added to fields
						
*/

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='RBOGIFTCARDTABLE' AND COLUMN_NAME='REFILLABLE' AND IS_NULLABLE ='YES')
begin
UPDATE RBOGIFTCARDTABLE 
set REFILLABLE = 0 where REFILLABLE is NULL
ALTER TABLE dbo.RBOGIFTCARDTABLE ALTER COLUMN REFILLABLE TINYINT NOT NULL
END

GO



