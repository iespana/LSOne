﻿
/*

	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 10.06.2013

	Description		: New column to CUSTOMERADDRESS
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTOMERADDRESS' AND COLUMN_NAME = 'CONTACTNAME')
BEGIN
	ALTER TABLE [dbo].[CUSTOMERADDRESS] ADD CONTACTNAME NVARCHAR(50); 
END
GO