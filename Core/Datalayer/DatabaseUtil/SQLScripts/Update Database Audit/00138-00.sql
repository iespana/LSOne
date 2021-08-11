﻿/*
Incident No.	: ONE-8363 Form layout editor - Windows printer selection
	Responsible		: Hörður Kristjánsson
	Sprint			: Vega
	Date created	: 18.06.2019

	Description		: Added windows printer configuration column WINDOWSPRINTERCONFIGURATIONID for form layouts
*/

USE LSPOSNET_Audit

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISFORMLAYOUTLog' AND COLUMN_NAME = 'WINDOWSPRINTERCONFIGURATIONID')
BEGIN
	ALTER TABLE POSISFORMLAYOUTLog ADD WINDOWSPRINTERCONFIGURATIONID NVARCHAR(40) NULL
END
GO