﻿/*
	Incident No.	: ONE-9285
	Responsible		: Adrian Chiorean
	Sprint			: Ferengi
	Date created	: 01.11.2018

	Description		: Add PROCESSINGSTATUS column to table INVENTJOURNALTABLE
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTJOURNALTABLE' AND COLUMN_NAME = 'PROCESSINGSTATUS')
BEGIN
	ALTER TABLE INVENTJOURNALTABLE ADD PROCESSINGSTATUS INT NOT NULL DEFAULT 0
END
GO