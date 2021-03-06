
/*

	Incident No.	: N/A
	Responsible		: Marý B Steingrímsdóttir
	Sprint			: LS Retail .NET\Loki
	Date created	: 7.5.2012

	Description		: Creates temporary tables for all price data so that after recalculations the original price data still exists

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PriceDiscTableOrgData - new table
					  InventTableModuleOrgData - new table

						
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('ORGDATA_PRICEDISCTABLE'))
BEGIN
	SELECT * 
	INTO ORGDATA_PRICEDISCTABLE
	FROM PRICEDISCTABLE	
END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('ORGDATA_INVENTTABLEMODULE'))
BEGIN
	SELECT * 
	INTO ORGDATA_INVENTTABLEMODULE
	FROM INVENTTABLEMODULE	
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ORGDATA_PRICEDISCTABLE' AND COLUMN_NAME='CALCULATED')
BEGIN
	ALTER TABLE ORGDATA_PRICEDISCTABLE ADD CALCULATED TINYINT NULL	
END
GO

UPDATE ORGDATA_PRICEDISCTABLE
SET CALCULATED = 0
WHERE CALCULATED IS NULL
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ORGDATA_INVENTTABLEMODULE' AND COLUMN_NAME='CALCULATED')
BEGIN
	ALTER TABLE ORGDATA_INVENTTABLEMODULE ADD CALCULATED TINYINT NULL	
END
GO

UPDATE ORGDATA_INVENTTABLEMODULE
SET CALCULATED = 0
WHERE CALCULATED IS NULL
GO



