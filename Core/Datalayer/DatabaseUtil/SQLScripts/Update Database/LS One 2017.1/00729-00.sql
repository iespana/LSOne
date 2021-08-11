/*

	Incident No.	: ONE-6250
	Responsible		: Adrian
	Sprint			: Bestå
	Date created	: 01.03.2017

	Description		: Mark HideTrainingMode column from store table as obsolete
	
	Tables affected	: RBOSTORETABLE
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'HIDETRAININGMODE')
	EXECUTE spDB_SetFieldDescription_1_0 'RBOSTORETABLE', 'HIDETRAININGMODE', '[Obsolete, ONE-6250]';
GO