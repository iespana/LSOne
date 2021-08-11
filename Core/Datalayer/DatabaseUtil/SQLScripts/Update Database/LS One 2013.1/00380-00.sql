﻿
/*

	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 2.7.2013

	Description		: Adds a new field to Functionality profile table
	
*/


USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'DEFAULTITEMIMAGE')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD DEFAULTITEMIMAGE VARBINARY(MAX) NULL
END
GO




