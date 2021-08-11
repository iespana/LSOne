﻿
/*
	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 14.02.2014

	Description		: Added a few indexes for performance reasons
						
*/
USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM sys.indexes where [object_id] = object_id('INVENTTABLEMODULE') and name = 'IDX_INVENTTABLEMODULE_MODULETYPE')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_INVENTTABLEMODULE_MODULETYPE
	ON [dbo].[INVENTTABLEMODULE] ([MODULETYPE])
	INCLUDE ([ITEMID],[PRICE])
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes where [object_id] = object_id('POSPERIODICDISCOUNTLINE') and name = 'IDX_PERIODICDISCOUNTLINE_OFFER')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_PERIODICDISCOUNTLINE_OFFER
	ON [dbo].[POSPERIODICDISCOUNTLINE] ([OFFERID],[DATAAREAID])
	INCLUDE ([LINEID],[PRODUCTTYPE],[ID],[DEALPRICEORDISCPCT],[LINEGROUP],[DISCTYPE],[POSPERIODICDISCOUNTLINEGUID])
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes where [object_id] = object_id('RBOINVENTTABLE') and name = 'IDX_RBOINVENTTABLE_DIMGROUP')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_RBOINVENTTABLE_DIMGROUP
	ON [dbo].[RBOINVENTTABLE] ([ITEMID])
	INCLUDE ([COLORGROUP],[SIZEGROUP],[STYLEGROUP])
END
GO
