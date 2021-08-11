﻿
/*

	Incident No.	: 24286
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 30.6.2013

	Description		: Adds a new field to RestaurantStation table
	
*/


USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RESTAURANTSTATION' AND COLUMN_NAME = 'OPOSMAXCHARS')
BEGIN
	ALTER TABLE RESTAURANTSTATION ADD OPOSMAXCHARS INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RESTAURANTSTATION' AND COLUMN_NAME = 'OPOSFONTSIZEV')
BEGIN
	ALTER TABLE RESTAURANTSTATION ADD OPOSFONTSIZEV INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RESTAURANTSTATION' AND COLUMN_NAME = 'OPOSFONTSIZEH')
BEGIN
	ALTER TABLE RESTAURANTSTATION ADD OPOSFONTSIZEH INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'ALLOWCHANGESAFTERSPLITBILL')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD ALLOWCHANGESAFTERSPLITBILL tinyint NULL
END
GO








