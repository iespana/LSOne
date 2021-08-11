/*
	Incident No.	: ONE-8929
	Responsible		: Adrian Chiorean
	Sprint			: ghlQ
	Date created	: 14.08.2018

	Description		: Add ISFOREU column to TAXGROUPHEADING
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TAXGROUPHEADING' AND COLUMN_NAME = 'ISFOREU')
BEGIN
	ALTER TABLE TAXGROUPHEADING ADD ISFOREU BIT NOT NULL DEFAULT 0
	EXECUTE spDB_SetFieldDescription_1_0 'TAXGROUPHEADING', 'ISFOREU', 'Specifies if this tax group is used for transactions between countries in the European Union. Used by SAP Business One integration.';
END
GO