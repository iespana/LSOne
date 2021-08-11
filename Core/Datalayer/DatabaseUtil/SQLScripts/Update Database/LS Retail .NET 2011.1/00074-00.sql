
/*

	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: N/A
	Date created	: 13.04.2011

	Description		: Add description column to STATIONSELECTION

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: STATIONSELECTION - added field DESCRIPTION
						
*/

USE LSPOSNET
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'STATIONSELECTION' AND COLUMN_NAME = 'DESCRIPTION')
BEGIN
	ALTER TABLE STATIONSELECTION ADD DESCRIPTION [nvarchar](60) NULL
END

GO



