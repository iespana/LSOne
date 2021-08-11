/*
	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 21.01.2014

	Description		: Added calendar day when business day was set. Default vaule will be set as the same date as the business day so that it makes sense for old systems
                      where this functionality was not previously in place.
	
						
*/
USE LSPOSNET
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONTABLE' AND COLUMN_NAME = 'BUSINESSSYSTEMDAY')
BEGIN
	ALTER TABLE RBOTRANSACTIONTABLE ADD BUSINESSSYSTEMDAY DATETIME null
END

GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONTABLE' AND COLUMN_NAME = 'BUSINESSSYSTEMDAY')
BEGIN
	Update RBOTRANSACTIONTABLE
	SET BUSINESSSYSTEMDAY = BUSINESSDAY
	WHERE BUSINESSSYSTEMDAY IS NULL
END
GO

