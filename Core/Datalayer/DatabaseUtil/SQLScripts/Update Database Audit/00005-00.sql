
/*

	Incident No.	: 6602
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET v 2010 - Sprint 3
	Date created	: 15.11.2010
	
	Description		: Add audit tables for Statements

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	RBOSTATEMENTTABLELog
						RBOSTATEMENTLINELog 	
						
*/


USE LSPOSNET_Audit

GO

IF EXISTS(SELECT 'x' FROM information_schema.columns WHERE TABLE_NAME = 'NUMBERSEQUENCETABLELog' AND COLUMN_NAME = 'NUMBERSEQUENCE' AND CHARACTER_MAXIMUM_LENGTH = 10)
Begin
	ALTER TABLE NUMBERSEQUENCETABLELog ALTER COLUMN NUMBERSEQUENCE nvarchar(20) NOT NULL
END