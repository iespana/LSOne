/*

	Incident No.	: ONE-3587
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Santiago

	Description		: Making variantid columns nullable
	
						
*/


USE LSPOSNET_Audit
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VENDORITEMSLog' and COLUMN_NAME = 'VARIANTID' and IS_NULLABLE = 'NO')
BEGIN
	
	ALTER TABLE [dbo].VENDORITEMSLog
	alter column VARIANTID nvarchar(20) NULL
END
GO


