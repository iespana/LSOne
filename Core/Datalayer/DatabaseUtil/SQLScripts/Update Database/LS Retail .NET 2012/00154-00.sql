/*

	Incident No.	: 12062	
	Responsible		: Óskar Bjarnason	
	Sprint			: LS Retail .NET 2012/Thor
	Date created	: 19.12.2011

	Description		: Add UseCentralsuspension column to tables POSTRANSACTIONSERVICEPROFILE 

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSTRANSACTIONSERVICEPROFILE - Added UserConfirmation	
					  
					  
					  
					  	
*/								

USE LSPOSNET

GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='POSTRANSACTIONSERVICEPROFILE' AND COLUMN_NAME='USERCONFIRMATION')
Begin
	Alter Table POSTRANSACTIONSERVICEPROFILE
	ADD USERCONFIRMATION tinyint NULL 
End
GO