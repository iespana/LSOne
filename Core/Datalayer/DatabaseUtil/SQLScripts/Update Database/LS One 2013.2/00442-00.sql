﻿
/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.2\July, August, September
	Date created	: 31.10.2013

	Description		: New column to RBOEFTMAPPING

						
*/

USE LSPOSNET

GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOEFTMAPPING' AND COLUMN_NAME = 'CARDTYPEID')
BEGIN
	ALTER TABLE RBOEFTMAPPING ADD CARDTYPEID NVARCHAR(20)
END


