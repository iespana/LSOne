﻿/*
	Incident No.	: ONE-13434
	Responsible		: Hörður Kristjánsson
	Sprint			: Dogecoin
	Date created	: 05.03.2021

	Description		: Increasing length of NAMEONRECEIPT for RBOSTAFFTABLE to match STAFFID
*/

USE LSPOSNET_Audit

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTAFFTABLELog' AND COLUMN_NAME = 'NAMEONRECEIPT' AND CHARACTER_MAXIMUM_LENGTH < 20)
    BEGIN
        ALTER TABLE RBOSTAFFTABLELog ALTER COLUMN NAMEONRECEIPT NVARCHAR(20)
    END
GO