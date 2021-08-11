/*

	Incident No.	: ONE-5146
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Fræbblarninr
	Date created	: 30.9.2016

	Description		: Add new column
	
	
	Tables affected	: Reports
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME LIKE 'REPORTS' AND COLUMN_NAME = 'ISSITESERVICEREPORT'  )

BEGIN
  ALTER TABLE REPORTS 
  ADD ISSITESERVICEREPORT BIT NOT NULL DEFAULT ((0))
END	
 
GO