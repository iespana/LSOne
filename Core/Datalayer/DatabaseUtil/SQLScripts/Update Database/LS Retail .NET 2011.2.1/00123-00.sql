
/*

	Incident No.	: 12827
	Responsible		: Guðbjörn Einarsson	
	Sprint			: LS Retail .NET 2012/Thor
	Date created	: 21.10.2011

	Description		: Add PARTNERINFO column to table RBOTRANSACTIONDISCOUNTTRANS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOTRANSACTIONDISCOUNTTRANS - Added PARTNERINFO
					  
					  
					  	
*/								

USE LSPOSNET

GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOTRANSACTIONDISCOUNTTRANS' AND COLUMN_NAME='PARTNERINFO')
Begin
	Alter Table RBOTRANSACTIONDISCOUNTTRANS
	ADD PARTNERINFO nvarchar(250) NULL 
End
GO




