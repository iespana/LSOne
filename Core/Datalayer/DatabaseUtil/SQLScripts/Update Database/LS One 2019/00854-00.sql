/*
	Incident No.	: ONE-8902
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Edosian
	Date created	: 2.11.2018

	Description		: Change the name of operation Clear discounts and Clear periodic discount. Add new Clear all discounts operation
*/

USE LSPOSNET

IF EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = 307)
BEGIN
	UPDATE OPERATIONS SET [DESCRIPTION] = 'Clear line discount' WHERE ID = 307	
END
GO

IF EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = 305)
BEGIN
	UPDATE OPERATIONS SET [DESCRIPTION] = 'Clear manually triggered periodic discounts' WHERE ID = 305
END
GO

IF NOT EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = 308)
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	VALUES (NEWID(), 308, 'Clear all discounts', 2, 0, 1)
END

IF NOT EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = 309)
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	VALUES (NEWID(), 309, 'Clear total discount', 2, 0, 1)
END

GO
