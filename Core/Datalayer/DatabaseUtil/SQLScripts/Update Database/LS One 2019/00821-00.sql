﻿/*
	Incident No.	: ONE-7812
	Responsible		: Adrian Chiorean
	Sprint			: Nymö
	Date created	: 23.10.2017

	Description		: Add bool parameter to Price check and Inventory lookup operation
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = 101 AND LOOKUPTYPE = 0) -- Price check
BEGIN
	UPDATE OPERATIONS SET LOOKUPTYPE = 19 WHERE ID = 101 AND LOOKUPTYPE = 0 -- SET LOOKUPTYPE
	UPDATE POSMENULINE SET PARAMETER = 'N' WHERE OPERATION = 101 AND PARAMETER = '' -- SET DEFAULT PARAMETER FOR LOOKUPTYPE
END
GO

IF EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = 801 AND LOOKUPTYPE = 0) -- Inventory lookup
BEGIN
	UPDATE OPERATIONS SET LOOKUPTYPE = 19 WHERE ID = 801 AND LOOKUPTYPE = 0 -- SET LOOKUPTYPE
	UPDATE POSMENULINE SET PARAMETER = 'N' WHERE OPERATION = 801 AND PARAMETER = '' -- SET DEFAULT PARAMETER FOR LOOKUPTYPE
END
GO