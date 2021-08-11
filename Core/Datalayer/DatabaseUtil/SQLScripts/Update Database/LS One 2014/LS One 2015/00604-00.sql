/*

	Incident No.	: ONE-1246
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Strato
	Date created	: 26.11.2014

	Description		: Create setting in Site Manager
	
	
	Tables affected	: INVENTDIMGROUP
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'INVENTDIMGROUP' and COLUMN_NAME = 'POSDISPLAYSETTING' )
BEGIN
	ALTER TABLE INVENTDIMGROUP ADD POSDISPLAYSETTING int NULL default 0
END	
GO