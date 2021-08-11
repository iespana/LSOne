﻿/*
	Incident No.	: ONE-12677
	Responsible		: Jonas Haraldsson
	Sprint			: Hlölli
	Date created	: 25.09.2020

	Description		: Adding missing indexes for discount calculation performance on the POS
*/

USE LSPOSNET

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IDX_PERIODICDISCOUNTLINE_OFFERID')
BEGIN
    CREATE NONCLUSTERED INDEX
        IDX_PERIODICDISCOUNTLINE_OFFERID
    ON
        PERIODICDISCOUNTLINE (OFFERID);
END
GO


IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IDX_SPECIALGROUPITEMS_ITEMID')
BEGIN
    CREATE NONCLUSTERED INDEX
        IDX_SPECIALGROUPITEMS_ITEMID
    ON
        SPECIALGROUPITEMS(ITEMID)
    INCLUDE
        (GROUPID, GROUPMASTERID);
END
GO