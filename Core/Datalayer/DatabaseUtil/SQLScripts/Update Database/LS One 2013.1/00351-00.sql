USE LSPOSNET
GO
  
 IF NOT EXISTS(SELECT 'x' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VENDTABLE'  AND COLUMN_NAME = 'DEFAULTDELIVERYTIME')
 BEGIN
	ALTER TABLE VENDTABLE ADD DEFAULTDELIVERYTIME int NOT NULL DEFAULT 0
 END

GO