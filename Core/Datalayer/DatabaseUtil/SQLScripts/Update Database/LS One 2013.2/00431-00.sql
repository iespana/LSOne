﻿/*

	Incident No.	: 26189
	Responsible		: Sigfus Johannesson
	Sprint			: LS One 2013.1\July, August, September
	Date created	: 16.10.2013

	Description		: Added column to RBOTERMINALTABLE
*/

USE LSPOSNET

GO

 IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTERMINALTABLE' AND COLUMN_NAME = 'SWITCHUSERWHENENTERINGPOS')
 BEGIN
	ALTER TABLE RBOTERMINALTABLE ADD SWITCHUSERWHENENTERINGPOS TINYINT
 END

 GO