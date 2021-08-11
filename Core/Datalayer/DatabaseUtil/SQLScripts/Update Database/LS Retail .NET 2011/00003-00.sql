
/*

	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: N/A
	Date created	: 22.10.2010
	
	Description		: The field FORECOURTSUSPENDALLOWED was missing, this was discovered before a beta releas for the store controller

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:  POSHARDWAREPROFILE
						
*/

Use LSPOSNET

GO


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSHARDWAREPROFILE') AND NAME='FORECOURTSUSPENDALLOWED')
	ALTER TABLE POSHARDWAREPROFILE ADD [FORECOURTSUSPENDALLOWED] [TINYINT] NULL
GO