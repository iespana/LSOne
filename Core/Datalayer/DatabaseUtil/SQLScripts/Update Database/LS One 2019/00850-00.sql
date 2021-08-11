/*
	Incident No.	: ONE-9187
	Responsible		: Ovidiu Caba
	Sprint			: Edosian
	Date created	: 18.10.2018

	Description		: Add Timeout and MaxMessageSize to site service profile
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'TIMEOUT' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD 
		[TIMEOUT] INT NOT NULL DEFAULT(60),
		[MAXMESSAGESIZE] INT NOT NULL DEFAULT(55000000)

	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'TIMEOUT', 'Site Service timeout in seconds. The default value is 60.';
	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'MAXMESSAGESIZE', 'Site Service maximum message size. The default value is 55000000.';
END

GO