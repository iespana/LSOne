﻿USE LSPOSNET_Audit
GO


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KITCHENDISPLAYFUNCTIONALPROFILELog') AND NAME='BUMPPOSSIBLE')
BEGIN
	 ALTER TABLE KITCHENDISPLAYFUNCTIONALPROFILELog Add BUMPPOSSIBLE int NOT NULL default 0 
END	
GO
