
/*
	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 21.02.2014

	Description		: Added a few indexes for performance reasons
						
*/
USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM sys.indexes where [object_id] = object_id('INVENTDIMCOMBINATION') and name = 'IDX_INVENTDIMCOMINATION_ITEM')
BEGIN
	CREATE NONCLUSTERED INDEX [IDX_INVENTDIMCOMINATION_ITEM]
	ON [dbo].[INVENTDIMCOMBINATION] ([ITEMID])
	INCLUDE ([RBOVARIANTID],[DATAAREAID])
END
GO
