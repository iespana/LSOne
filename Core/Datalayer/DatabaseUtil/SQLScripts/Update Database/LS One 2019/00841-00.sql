/*
	Incident No.	: ONE-8717
	Responsible		: Simona Avornicesei
	Sprint			: Kerla
	Date created	: 31.05.2018

	Description		: Replicate the SAP free text and pictures for items to LS One
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILITEMIMAGE' AND COLUMN_NAME = 'IMAGENAME')
BEGIN
	ALTER TABLE RETAILITEMIMAGE ADD IMAGENAME VARCHAR(256) NULL;

	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMIMAGE', 'IMAGENAME', 'File name of the stored image.';
END
GO