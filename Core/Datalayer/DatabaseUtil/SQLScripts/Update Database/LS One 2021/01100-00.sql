/*
	Incident No.	: ONE-13167
	Responsible		: Jonas Haraldsson
	Sprint			: Bahraini dinar
	Date created	: 09.02.2021

	Description		: Add KDSCOMPONENTSASITEMS column to RETAILITEMASSEMBLY
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'SENDCOMPONENTSTOKDS' AND TABLE_NAME = 'RETAILITEMASSEMBLY')
BEGIN
    ALTER TABLE RETAILITEMASSEMBLY ADD SENDCOMPONENTSTOKDS INT NOT NULL DEFAULT 0
END

GO

UPDATE 
	RETAILITEMASSEMBLY
SET 
	EXPANDASSEMBLY = EXPANDASSEMBLY & ~4, 
	SENDCOMPONENTSTOKDS = 1
WHERE 
	EXPANDASSEMBLY & 4 = 4

GO
