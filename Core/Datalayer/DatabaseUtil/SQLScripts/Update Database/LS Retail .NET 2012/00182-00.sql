/*

	Incident No.	: 16146
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS Retail .NET 2012\Loki
	Date created	: 27.4.2012

	Description		: Add new field to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOTRANSACTIONDISCOUNTTRANS - new field added
						
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='RBOTRANSACTIONINFOCODETRANS' AND COLUMN_NAME='LINEID')
BEGIN
	ALTER TABLE RBOTRANSACTIONINFOCODETRANS ADD LINEID numeric(28,12) NULL	
END
GO