/*

	Incident No.	: ONE-3299
	Responsible		: Anca Roibu 
	Sprint			: Stunsig (21.6-5.7)
	Date created	: 27.06.2017

	Description		: Mark FORCESTARTOFDAYONLOGON column from store table as obsolete
	
	Tables affected	: RBOSTORETABLE
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'FORCESTARTOFDAYONLOGON')
	EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'FORCESTARTOFDAYONLOGON', '[Obsolete, ONE-3299]';
GO
