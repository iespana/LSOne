﻿
/*

	Incident No.	: ONE-3037
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Pepper

	Description		: Changes how and when customers can be deleted. CUSTTABEL and RBOCUSTTABLE combined into one and all fields not in the data providers removed
	
						
*/

USE LSPOSNET
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTTABLE' AND COLUMN_NAME = 'DELETED')
BEGIN
	ALTER TABLE dbo.CUSTTABLE ADD DELETED BIT NULL	
	ALTER TABLE dbo.CUSTTABLE ADD CONSTRAINT DF_CUSTTABLE_DELETED DEFAULT 0 FOR DELETED
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTTABLE' AND COLUMN_NAME = 'RECEIPTOPTION')
BEGIN
	ALTER TABLE [dbo].[CUSTTABLE] ADD RECEIPTOPTION INT NULL
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTTABLE' AND COLUMN_NAME = 'RECEIPTEMAIL')
BEGIN
	ALTER TABLE [dbo].[CUSTTABLE] ADD RECEIPTEMAIL NVARCHAR(80) NULL
END
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTTABLE' AND COLUMN_NAME = 'RECEIPTOPTION')
BEGIN
	UPDATE [dbo].[CUSTTABLE] 
	SET CUSTTABLE.RECEIPTOPTION = RBOCUSTTABLE.RECEIPTOPTION
	FROM CUSTTABLE, RBOCUSTTABLE
	WHERE CUSTTABLE.RECEIPTOPTION IS NULL AND CUSTTABLE.ACCOUNTNUM = RBOCUSTTABLE.ACCOUNTNUM
END	
GO
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTTABLE' AND COLUMN_NAME = 'RECEIPTEMAIL')
BEGIN
	UPDATE [dbo].[CUSTTABLE] 
	SET CUSTTABLE.RECEIPTEMAIL = RBOCUSTTABLE.RECEIPTEMAIL
	FROM CUSTTABLE, RBOCUSTTABLE
	 WHERE CUSTTABLE.RECEIPTEMAIL IS NULL AND CUSTTABLE.ACCOUNTNUM = RBOCUSTTABLE.ACCOUNTNUM
END	
GO
	UPDATE CUSTTABLE
	SET DELETED = 0
	WHERE DELETED IS NULL

	UPDATE [dbo].[CUSTTABLE]
	SET RECEIPTOPTION = 0
	WHERE RECEIPTOPTION IS NULL
GO

