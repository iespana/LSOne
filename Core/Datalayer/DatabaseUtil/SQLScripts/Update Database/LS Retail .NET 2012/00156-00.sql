/*

	Incident No.	: 13742
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012/Skadi
	Date created	: 04.12.2012

	Description		: Added column CanBeDeleted to NUMBERSEQUENCETABLE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: NUMBERSEQUENCETABLE - Added CanBeDeleted	
					  
					  
					  
					  	
*/								

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='NUMBERSEQUENCETABLE' AND COLUMN_NAME='CANBEDELETED')
Begin
	Alter Table NUMBERSEQUENCETABLE ADD CANBEDELETED tinyint NULL default 0	
End
GO