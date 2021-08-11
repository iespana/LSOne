/*
	Incident No.	: ONE-7977
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Eket
	Date created	: 17.11.2017

	Description		: Mark standalone field and PosisCustTransaction table as obsolete
*/

USE LSPOSNET

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTERMINALTABLE' AND COLUMN_NAME = 'STANDALONE')
BEGIN
	EXECUTE spDB_SetFieldDescription_1_0 'RBOTERMINALTABLE', 'STANDALONE', '[Obsolete, ONE-7977] Standalone property removed from Terminal.';
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISCUSTTRANSACTIONS')
BEGIN	
	EXECUTE spDB_SetTableDescription_1_0 'POSISCUSTTRANSACTIONS', '[Obsolete, ONE-7977] This table was only used in standalone functionality';
END
GO
