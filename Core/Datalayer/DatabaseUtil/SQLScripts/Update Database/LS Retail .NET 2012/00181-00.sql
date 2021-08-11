/*

	Incident No.	: [TFS incident no]
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS Retail .NET 2012\Loki
	Date created	: 19.4.2012

	Description		: Add new field to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOTRANSACTIONDISCOUNTTRANS - new field added
						
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='RBOTRANSACTIONDISCOUNTTRANS' AND COLUMN_NAME='ORIGIN')
BEGIN
	ALTER TABLE RBOTRANSACTIONDISCOUNTTRANS ADD ORIGIN int NULL	
END
GO