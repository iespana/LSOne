/*
	Incident No.	: ONE-12581
	Responsible		: Jonas Haraldsson
	Sprint			: Hlölli
	Date created	: 16.09.2020

	Description		: Adding menu type lookup to operations Set hospitality menu type and Change hospitality menu type
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = 1303 AND LOOKUPTYPE = 0) -- Price check
BEGIN
	UPDATE OPERATIONS SET LOOKUPTYPE = 27 WHERE ID = 1303 AND LOOKUPTYPE = 0 -- SET LOOKUPTYPE
END
GO

IF EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = 1304 AND LOOKUPTYPE = 0) -- Price check
BEGIN
	UPDATE OPERATIONS SET LOOKUPTYPE = 27 WHERE ID = 1304 AND LOOKUPTYPE = 0 -- SET LOOKUPTYPE
END
GO