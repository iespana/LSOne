/*
	Incident No.	: ONE-6736
	Responsible		: Adrian Chiorean
	Sprint			: Färgrik
	Date created	: 09.05.2017

	Description		: Update Infocode on request operation
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM POSISOPERATIONS WHERE OPERATIONID = 129 AND LOOKUPTYPE = 0) -- Infocode on request
BEGIN
	UPDATE POSISOPERATIONS SET LOOKUPTYPE = 23 WHERE OPERATIONID = 129 -- 23 = LookupTypeEnum.InfocodeOnRequest
END

GO