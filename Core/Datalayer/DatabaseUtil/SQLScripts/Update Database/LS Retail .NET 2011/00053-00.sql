/*

	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: 2011 - Sprint 5
	Date created	: 25.01.2011

	Description		: Add field to RESTAURANTSTATION

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RESTAURANTSTATION	-	field added
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RESTAURANTSTATION') AND NAME='REMOTEHOSTID')
BEGIN
	ALTER TABLE [dbo].[RESTAURANTSTATION] ADD REMOTEHOSTID  NVARCHAR(20) NULL;
END
GO