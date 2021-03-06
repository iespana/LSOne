/*
	Incident No.	: ONE-13590
	Responsible		: Sigurður Bjarni Sigurðsson
	Sprint			: Etna
	Date created	: 27.04.2021

	Description		: Add Table override operation to Hopitality
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM [POSISHOSPITALITYOPERATIONS] WHERE [OPERATIONID] = 808 and [DATAAREAID] = 'LSR')
	BEGIN
		INSERT POSISHOSPITALITYOPERATIONS(OPERATIONID,OPERATIONNAME,PERMISSIONID,CHECKUSERACCESS,ALLOWPARAMETER,DATAAREAID) VALUES('808','Unlock table','0','0','0','LSR')
	END
GO