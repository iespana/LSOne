
/*

	Incident No.	: 23904
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 3.6.2013

	Description		: Rename field in HospitalityTypeLog table
	
						
*/

USE LSPOSNET_Audit

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'HOSPITALITYTYPELog'  AND COLUMN_NAME = 'DINEINTABLEREQUIRED')
BEGIN
	EXECUTE sp_rename N'dbo.HOSPITALITYTYPELog.DINEINTABLEREQUIRED', N'UPDATETABLEFROMPOS', 'COLUMN' 
END
GO


