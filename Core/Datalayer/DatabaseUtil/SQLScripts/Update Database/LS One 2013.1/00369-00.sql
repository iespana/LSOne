
/*

	Incident No.	: 24118
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 18.6.2013

	Description		: Adds a new field to HospitalityType table
	
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HOSPITALITYTYPE' AND COLUMN_NAME = 'MAXNOOFSPLITS')
BEGIN
	ALTER TABLE HOSPITALITYTYPE ADD MAXNOOFSPLITS INT
END
GO




