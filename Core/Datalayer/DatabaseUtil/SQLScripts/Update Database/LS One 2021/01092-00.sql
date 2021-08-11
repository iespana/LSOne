/*
	Incident No.	: ONE-8911
	Responsible		: Helgi Runar Gunnarsson
	Sprint			: Króna 
	Date created	: 09.12.2020

	Description		: Show item id in return popup dialog
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'DISPLAYITEMIDINRETURNDIALOG' AND TABLE_NAME = 'POSFUNCTIONALITYPROFILE')
    BEGIN
        ALTER TABLE POSFUNCTIONALITYPROFILE ADD DISPLAYITEMIDINRETURNDIALOG BIT NOT NULL DEFAULT 0
        EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'DISPLAYITEMIDINRETURNDIALOG', 'Determines if the item ID is shown in the retun popup dialog. This setting is set to true if the item ID is shown in the POS receipt control'
    END
GO