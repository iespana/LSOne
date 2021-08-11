﻿/*
	Incident No.	: 
	Responsible		: Simona Avornicesei
	Sprint			: Vindum
	Date created	: 01.09.2017

	Description		: Add OMNI operations for transfer orders (stock transfer)
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2115')
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, [DESCRIPTION], [TYPE], [LOOKUPTYPE], [AUDIT])
	VALUES (NEWID(), '2115', 'Receiving', 3, 0, NULL)
END
GO

IF NOT EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2116')
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, [DESCRIPTION], [TYPE], [LOOKUPTYPE], [AUDIT])
	VALUES (NEWID(), '2116', 'Sending', 3, 0, NULL)
END
GO