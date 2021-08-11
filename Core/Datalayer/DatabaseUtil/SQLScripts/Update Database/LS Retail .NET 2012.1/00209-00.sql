/*

	Incident No.	:
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 04.07.2012

	Description		: Add KitchenDisplayProfileId to RestaurantStation
	
	
	Tables affected	: RestaurantStation
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RESTAURANTSTATION') AND NAME='KITCHENDISPLAYPROFILEID')
BEGIN
	ALTER TABLE dbo.RESTAURANTSTATION ADD KITCHENDISPLAYPROFILEID varchar(20) NULL
END
GO
