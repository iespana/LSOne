/*
	Incident No.	: ONE-11663
	Responsible		: Adrian Chiorean
	Sprint			: Chihuahua 
	Date created	: 28.05.2020

	Description		: Hide PayCard operation from UI
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = 201 AND TYPE = 2)
BEGIN
	UPDATE OPERATIONS SET TYPE = 1 WHERE ID = 201 -- Hide pay card operation
	UPDATE RBOSTORETENDERTYPETABLE SET POSOPERATION = 215 WHERE POSOPERATION = 201 -- Change pay card to authorize card
	UPDATE POSMENULINE SET OPERATION = 0, PARAMETER = '' WHERE OPERATION = 201 -- Change pay card buttons to no operation
END
GO