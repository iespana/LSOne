/*

	Incident No.	: N/A
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2013.1\July, August, September
	Date created	: 18.10.2013

	Description		: Increase the capacity of DESCRIPTION column in POSMENUHEADERLog
*/

USE LSPOSNET_Audit

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSMENUHEADERLog' AND COLUMN_NAME = 'DESCRIPTION')
Begin
  Alter Table POSMENUHEADERLog alter column DESCRIPTION nvarchar(100)
End
GO

