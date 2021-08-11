/*

	Incident No.	: ONE-2119
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Klósigar
	Date created	: 11.5.2015

	Description		: Create setting in Site Manager
	
	
	Tables affected	: CUSTTABLE
						
*/
USE LSPOSNET
GO
  
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME LIKE 'CUSTTABLE' AND COLUMN_NAME = 'ORGID' AND CHARACTER_MAXIMUM_LENGTH < 20  )
BEGIN
	ALTER TABLE CUSTTABLE  
	ALTER COLUMN ORGID NVARCHAR(20)
END	
GO