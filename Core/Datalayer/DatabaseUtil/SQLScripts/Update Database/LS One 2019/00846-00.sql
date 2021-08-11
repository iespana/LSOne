/*
	Incident No.	: ONE-8469
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Borg
	Date created	: 13.09.2018

	Description		: Column in table POSTRANSACTIONSERVICEPROFILE marked as obsolete
*/

USE LSPOSNET

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE' AND COLUMN_NAME = 'IMMEDIATELYUPDATEINVENTORY')
BEGIN	
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'IMMEDIATELYUPDATEINVENTORY', 'OBSOLETE: This property is obsolete as the inventory is now all centralized by default';
END
GO