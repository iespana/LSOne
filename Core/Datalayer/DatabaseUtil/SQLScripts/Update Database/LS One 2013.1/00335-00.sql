﻿USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KITCHENDISPLAYLOG') AND NAME='ITEMID')
BEGIN
	 ALTER TABLE KITCHENDISPLAYLOG Add ITEMID nvarchar(20) NOT NULL default '' 
END	
GO