/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 2011 - Sprint 5
	Date created	: 21.01.2011

	Description		: Adds field to the table POSMMLINEGROUPS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMMLINEGROUPS
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMMLINEGROUPS') AND NAME='DESCRIPTION')
BEGIN
	ALTER TABLE [dbo].[POSMMLINEGROUPS] ADD DESCRIPTION  NVARCHAR(30) NULL;
END
GO