/*
	Incident No.	: ONE-8574
	Responsible		: Adrian Chiorean
	Sprint			: Huraga
	Date created	: 23.03.2018

	Description		: Add IF authentication token to site service profile
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'IFAUTHTOKEN' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD 
		IFAUTHTOKEN NVARCHAR(256) NULL,
		IFTCPPORT NVARCHAR(10) NULL,
		IFHTTPPORT NVARCHAR(10) NULL,
		IFPROTOCOLS NVARCHAR(20) NULL,
		IFSSLTHUMBNAIL NVARCHAR(512) NULL

	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'IFAUTHTOKEN', 'Integration Framework authorization token (JSON web token)';
	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'IFTCPPORT', 'Net/TCP port for Integration Framework connection';
	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'IFHTTPPORT', 'HTTP port for Integration Framework connection';
	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'IFPROTOCOLS', 'Enabled protocols for Integration Framework';
	EXEC spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'IFSSLTHUMBNAIL', 'Integration Framework SSL certificate thumbail for HTTPS connections';
END
GO