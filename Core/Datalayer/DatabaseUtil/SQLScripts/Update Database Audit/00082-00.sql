/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 1.10.2013

	Description		: Added a new column for posislicense
*/

USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISLICENSELog' AND COLUMN_NAME = 'ID')
Begin
  Alter Table POSISLICENSELog add ID UniqueIdentifier
End
GO

