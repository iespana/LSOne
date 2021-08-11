/*
	Incident No.	: ONE-13547
	Responsible		: Sigurður Bjarni Sigurðsson
	Sprint			: Kīlauea
	Date created	: 07.06.2021

	Description		: Add "Show individual deposits" configuration to functionality profile
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'ZRPTSHOWINDIVIDUALDEPOSITS')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD ZRPTSHOWINDIVIDUALDEPOSITS BIT NULL DEFAULT(1) WITH VALUES;

	EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'ZRPTSHOWINDIVIDUALDEPOSITS', 'Configuration to control whether individual customer deposits are shown on X/Z reports';
END


GO