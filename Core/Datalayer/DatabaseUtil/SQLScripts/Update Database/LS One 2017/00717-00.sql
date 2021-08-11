/*

	Incident No.	: ONE-5578
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Turmeric 1.12 - 15.12
	Date created	: 22.11.2016

	Description		: Add barcode functionality to manually trigger gift cards
	
	
	Tables affected	: RBOGIFTCARDTABLE
						
*/

USE LSPOSNET
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOGIFTCARDTABLE' AND COLUMN_NAME = 'BARCODE')
BEGIN
ALTER TABLE dbo.RBOGIFTCARDTABLE ADD
	BARCODE NVARCHAR(80) NULL
END

GO