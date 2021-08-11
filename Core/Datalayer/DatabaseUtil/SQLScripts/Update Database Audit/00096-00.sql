﻿USE LSPOSNET_Audit
GO
  
IF NOT EXISTS (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'INVENTDIMGROUPLog' and COLUMN_NAME = 'POSDISPLAYSETTING' )
BEGIN
	ALTER TABLE INVENTDIMGROUPLog ADD POSDISPLAYSETTING int NULL default 0
END	
GO