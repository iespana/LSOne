
/*

	Incident No.	: 12033	
	Responsible		: Guðbjörn Einarsson	
	Sprint			: LS Retail .NET 2012/Thor
	Date created	: 20.10.2011

	Description		: Add font columns to table RESTAURANTSTATION

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RESTAURANTSTATION - Added FONTNAME
									    - Added FONTSIZE
					  
					  
					  	
*/								

USE LSPOSNET

GO

IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RESTAURANTSTATION' AND COLUMN_NAME='FONTNAME')
Begin
	Alter Table RESTAURANTSTATION
	ADD FONTNAME nvarchar(60) NULL 
End
GO


IF NOT EXISTS (SELECT 'X' FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RESTAURANTSTATION' AND COLUMN_NAME='FONTSIZE')
Begin
	Alter Table RESTAURANTSTATION
	ADD FONTSIZE decimal(24,6) NULL 
End
GO



