/*

	Incident No.	: ONE-6907
	Responsible		: Hörður Kristjánsson
	Sprint			: Pax
	Date created	: 01.06.2017

	Description		: Adding omni item image lookup group to functionality profile
	
	
	Tables affected	: POSFUNCTIONALITYPROFILE
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'OMNIITEMIMAGELOOKUPGROUP')
BEGIN
	ALTER TABLE dbo.POSFUNCTIONALITYPROFILE ADD OMNIITEMIMAGELOOKUPGROUP uniqueidentifier NULL
END
GO
