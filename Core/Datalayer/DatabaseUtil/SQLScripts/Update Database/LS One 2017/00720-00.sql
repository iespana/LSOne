/*

	Incident No.	: ONE-4639
	Responsible		: Mar� Bj�rk Steingr�msd�ttir
	Sprint			: Turmeric 1.12 - 15.12
	Date created	: 14.12.2016

	Description		: New fields added to functionality profile
	
	
	Tables affected	: POSFUNCTIONALITYPROFILE
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'ZRPTREPORTWIDTH')
BEGIN
	ALTER TABLE dbo.POSFUNCTIONALITYPROFILE ADD ZRPTREPORTWIDTH INT NULL
END
GO