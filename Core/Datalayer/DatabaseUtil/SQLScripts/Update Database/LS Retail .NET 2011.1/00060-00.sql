/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 2011 - Sprint 5
	Date created	: 23.02.2011

	Description		: Add enum fields to be able to resolve the address format

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: CONTACTTABLE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('CONTACTTABLE') AND NAME='ADDRESSFORMAT')
BEGIN
	ALTER TABLE [dbo].[CONTACTTABLE] ADD ADDRESSFORMAT int NULL;
END
GO