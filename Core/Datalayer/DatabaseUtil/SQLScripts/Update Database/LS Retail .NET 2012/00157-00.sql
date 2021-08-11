/*

	Incident No.	: 10668
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2012/Skadi
	Date created	: 09.12.2012

	Description		: Added column USEINVENTORYLOOKUP to POSTRANSACTIONSERVICEPROFILE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSTRANSACTIONSERVICEPROFILE - Added USEINVENTORYLOOKUP	
					  
					  
	ATN: This field was not needed so this script is outcommented.					  
					  	
							

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='POSTRANSACTIONSERVICEPROFILE' AND COLUMN_NAME='USEINVENTORYLOOKUP')
Begin
	Alter Table POSTRANSACTIONSERVICEPROFILE ADD USEINVENTORYLOOKUP tinyint NULL
End

*/	