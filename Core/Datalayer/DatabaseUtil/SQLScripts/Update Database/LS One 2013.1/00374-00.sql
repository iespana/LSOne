
/*

	Incident No.	: 24314/24281
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 19.6.2013

	Description		: Adds a new field to PosFunctionalityProfile and HospitalityType table
	
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'DISPLAYVOIDEDITEMS')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD DISPLAYVOIDEDITEMS TINYINT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HOSPITALITYTYPE' AND COLUMN_NAME = 'SENDVOIDEDTOSTATION')
BEGIN
	ALTER TABLE HOSPITALITYTYPE ADD SENDVOIDEDTOSTATION TINYINT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HOSPITALITYTYPE' AND COLUMN_NAME = 'SENDTRANSFERSTOSTATION')
BEGIN
	ALTER TABLE HOSPITALITYTYPE ADD SENDTRANSFERSTOSTATION TINYINT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISHOSPITALITYTRANSTABLE' AND COLUMN_NAME = 'SPLITID')
BEGIN
	ALTER TABLE POSISHOSPITALITYTRANSTABLE ADD SPLITID uniqueidentifier NULL
END
GO
