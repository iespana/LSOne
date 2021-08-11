/*

	Incident No.	: ONE-5146
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Fræbblarninr
	Date created	: 30.9.2016

	Description		: Add new column for ledger compatablity 
	
	
	Tables affected	: INVENTTRANS
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME LIKE 'INVENTTRANS' AND COLUMN_NAME = 'COMPATIBILITY'  )

BEGIN
 ALTER TABLE  INVENTTRANS  ADD COMPATIBILITY VARCHAR(60) NULL
END	

 
GO