/*

	Incident No.	: [TFS incident no]
	Responsible		: Sigfús Jóhannesson
	Sprint			: LS Retail .NET 2012\Mercury
	Date created	: 17.01.2013

	Description		: Adding indexes to PRICEDISCTABLE. This script was moved from 2012 branch to dev line

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PRICEDISCTABLE 
						
*/

USE LSPOSNET

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_ITEMCODE')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_ITEMCODE
 ON dbo.PRICEDISCTABLE (ITEMCODE)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_ACCOUNTCODE')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_ACCOUNTCODE
 ON dbo.PRICEDISCTABLE (ACCOUNTCODE)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_ITEMRELATION')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_ITEMRELATION
 ON dbo.PRICEDISCTABLE (ITEMRELATION)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_ACCOUNTRELATION')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_ACCOUNTRELATION
 ON dbo.PRICEDISCTABLE (ACCOUNTRELATION)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_QUANTITYAMOUNT')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_QUANTITYAMOUNT
 ON dbo.PRICEDISCTABLE (QUANTITYAMOUNT)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_FROMDATE')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_FROMDATE
 ON dbo.PRICEDISCTABLE (FROMDATE)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_CURRENCY')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_CURRENCY
 ON dbo.PRICEDISCTABLE (CURRENCY)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_RELATION')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_RELATION
 ON dbo.PRICEDISCTABLE (RELATION)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_UNITID')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_UNITID
 ON dbo.PRICEDISCTABLE (UNITID)
END

if not exists(SELECT * FROM [sys].[indexes] where object_id = object_id(N'[dbo].[PRICEDISCTABLE]') AND NAME = N'IDX_PRICEDISCTABLE_INVENTDIMID')
BEGIN
 CREATE INDEX IDX_PRICEDISCTABLE_INVENTDIMID
 ON dbo.PRICEDISCTABLE (INVENTDIMID)
END
