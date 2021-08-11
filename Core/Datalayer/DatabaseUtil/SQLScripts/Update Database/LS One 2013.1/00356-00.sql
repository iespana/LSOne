/*

	Incident No.	: 23904
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 3.6.2013

	Description		: Rename field in HospitalityType table
	
						
*/

USE LSPOSNET

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'HOSPITALITYTYPE'  AND COLUMN_NAME = 'DINEINTABLEREQUIRED')
BEGIN
	EXECUTE sp_rename N'dbo.HOSPITALITYTYPE.DINEINTABLEREQUIRED', N'UPDATETABLEFROMPOS', 'COLUMN' 
END
GO