/*

	Incident No.	: ONE-5785
	Responsible		: Hörður Kristjánsson
	Sprint			: Koriander
	Date created	: 16.01.2017

	Description		: Adding omni suspension type to functionality profile
	
	
	Tables affected	: POSFUNCTIONALITYPROFILE
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'OMNISUSPENSIONTYPE')
BEGIN
	ALTER TABLE dbo.POSFUNCTIONALITYPROFILE ADD OMNISUSPENSIONTYPE nvarchar(40) NULL
END
GO
