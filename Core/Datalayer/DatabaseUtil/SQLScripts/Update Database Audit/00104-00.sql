/*

	Incident No.	: ONE-1303
	Responsible		: Hörður Kristjánsson
	Sprint			: Rome

	Description		: Adding "Returnable" flag to retail items
	
						
*/


USE LSPOSNET_Audit
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILITEMLog' and COLUMN_NAME = 'RETURNABLE')
BEGIN	
	ALTER TABLE [dbo].RETAILITEMLog add RETURNABLE bit NULL
END
GO