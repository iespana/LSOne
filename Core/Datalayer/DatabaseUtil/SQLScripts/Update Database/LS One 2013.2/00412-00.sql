﻿/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 1.9.2013

	Description		: Add a new signature field to RBOTRANSACTIONEFTINFOTRANS	
*/

USE LSPOSNET

GO

IF NOT EXISTS(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' AND COLUMN_NAME = 'SIGNATURE')
BEGIN
ALTER TABLE RBOTRANSACTIONEFTINFOTRANS ADD SIGNATURE varbinary(max) NULL
END

IF NOT EXISTS(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' AND COLUMN_NAME = 'RECEIPTLINES')
BEGIN
ALTER TABLE RBOTRANSACTIONEFTINFOTRANS ADD RECEIPTLINES nvarchar(max) NULL
END