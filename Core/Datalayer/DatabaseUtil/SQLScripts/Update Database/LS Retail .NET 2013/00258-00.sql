/*

	Incident No.	: XXX
	Responsible		: Sigfus Johannesson
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 19.11.2012

	Description		: Added settings to hardware profile

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSHARDWAREPROFILE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'POSHARDWAREPROFILE' AND COLUMN_NAME = N'DUALDISPLAYSCREENNUMBER')
BEGIN
ALTER TABLE POSHARDWAREPROFILE ADD DUALDISPLAYSCREENNUMBER INT NULL
END

GO

