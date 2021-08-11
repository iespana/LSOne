/*
	Incident No.	: N/A
	Responsible		: Sigfús Jóhannesson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 21.01.2014

	Description		: Added business day to transactions
	
						
*/
USE LSPOSNET
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONTABLE' AND COLUMN_NAME = 'BUSINESSDAY')
BEGIN
	ALTER TABLE RBOTRANSACTIONTABLE ADD BUSINESSDAY DATETIME
END

GO