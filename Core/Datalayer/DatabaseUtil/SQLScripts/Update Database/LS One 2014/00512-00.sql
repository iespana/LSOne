
/*
	Incident No.	: N/A
	Responsible		: Gudbjorn
	Sprint			: LS One 2014 - Nimbo
	Date created	: 10.02.2014

	Description		: Populating empty business date columns. 
					  Done so that functionality that depends on business date is still sane on old data created before business date column was created
						
*/
USE LSPOSNET
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONTABLE' AND COLUMN_NAME = 'BUSINESSDAY')
BEGIN
	Update RBOTRANSACTIONTABLE
	SET BUSINESSDAY = TRANSDATE
	WHERE BUSINESSDAY = '1900-01-01' OR BUSINESSDAY IS NULL
END
GO



