/*

	Incident No.	: ONE-5578
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Oregano 17.11 - 1.12
	Date created	: 22.11.2016

	Description		: Add barcode functionality to manually trigger periodic discounts 
	
	
	Tables affected	: PERIODICDISCOUNT
						
*/

USE LSPOSNET
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PERIODICDISCOUNT' AND COLUMN_NAME = 'BARCODE')
BEGIN
ALTER TABLE dbo.PERIODICDISCOUNT ADD
	BARCODE NVARCHAR(80) NULL
END

GO