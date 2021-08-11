
/*

	Incident No.	: N/A
	Responsible		: Hrólfur Gestsson 
	Description		: Add a field to force the selection of UOM when an item is sold	
							
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOINVENTTABLE' AND COLUMN_NAME = 'MUSTSELECTUOM')
BEGIN
	ALTER TABLE RBOINVENTTABLE ADD MUSTSELECTUOM TINYINT
END
GO
