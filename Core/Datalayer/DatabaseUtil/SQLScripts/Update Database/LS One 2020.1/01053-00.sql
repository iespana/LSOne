/*
	Incident No.	: ONE-12385
	Responsible		: Helgi Runar Gunnarsson
	Sprint			: Kol 
	Date created	: 10.08.2020

	Description		: Disallow direct sale of an item on the POS
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CANBESOLD' AND TABLE_NAME = 'RETAILITEM')
    BEGIN
        ALTER TABLE RETAILITEM ADD CANBESOLD BIT NOT NULL DEFAULT 1
        EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEM', 'CANBESOLD', 'Determines if the item can be sold on the POS.'
    END
GO