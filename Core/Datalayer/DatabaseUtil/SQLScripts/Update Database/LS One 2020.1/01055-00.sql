﻿/*
	Incident No.	: ONE-12534
	Responsible		: Helgi Runar Gunnarsson
	Sprint			: Pizza Hut
	Date created	: 21.08.2020

	Description		: Returning transaction that includes assembly item is not working properly
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ISASSEMBLY' AND TABLE_NAME = 'RBOTRANSACTIONSALESTRANS')
    BEGIN
        ALTER TABLE RBOTRANSACTIONSALESTRANS ADD ISASSEMBLY BIT NULL
        EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'ISASSEMBLY', 'Determines if the item is an assembly'
    END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ISASSEMBLYCOMPONENT' AND TABLE_NAME = 'RBOTRANSACTIONSALESTRANS')
    BEGIN
        ALTER TABLE RBOTRANSACTIONSALESTRANS ADD ISASSEMBLYCOMPONENT BIT NULL
        EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'ISASSEMBLYCOMPONENT', 'Determines if the item is an assembly component'
    END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ASSEMBLYID' AND TABLE_NAME = 'RBOTRANSACTIONSALESTRANS')
    BEGIN
        ALTER TABLE RBOTRANSACTIONSALESTRANS ADD ASSEMBLYID UNIQUEIDENTIFIER NULL
        EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'ASSEMBLYID', 'ID of the assembly'
    END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ASSEMBLYCOMPONENTID' AND TABLE_NAME = 'RBOTRANSACTIONSALESTRANS')
    BEGIN
        ALTER TABLE RBOTRANSACTIONSALESTRANS ADD ASSEMBLYCOMPONENTID UNIQUEIDENTIFIER NULL
        EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'ASSEMBLYCOMPONENTID', 'ID of the assembly component'
    END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ASSEMBLYPARENTLINEID' AND TABLE_NAME = 'RBOTRANSACTIONSALESTRANS')
    BEGIN
        ALTER TABLE RBOTRANSACTIONSALESTRANS ADD ASSEMBLYPARENTLINEID INT NULL
        EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'ASSEMBLYPARENTLINEID', 'Line ID of the parent item'
    END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'HASPRICE' AND TABLE_NAME = 'RBOTRANSACTIONSALESTRANS')
    BEGIN
        ALTER TABLE RBOTRANSACTIONSALESTRANS ADD HASPRICE BIT NOT NULL DEFAULT 1
        EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONSALESTRANS', 'HASPRICE', '1 if a net amount was calculated for this line'
    END
GO