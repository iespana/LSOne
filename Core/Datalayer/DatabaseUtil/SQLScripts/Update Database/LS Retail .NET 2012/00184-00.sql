/*

	Incident No.	: [TFS incident no]
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012\Loki
	Date created	: 8.5.2012

	Description		: Add new field to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOTRANSACTIONDISCOUNTTRANS - new field added
						
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='RESTAURANTSTATION' AND COLUMN_NAME='PRINTERDEVICENAME')
BEGIN
	ALTER TABLE RESTAURANTSTATION ADD PRINTERDEVICENAME nvarchar(30) NULL	
END
GO