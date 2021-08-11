/*
	Incident No.	: ONE-11308
	Responsible		: Adrian Chiorean
	Sprint			: Leu
	Date created	: 18.12.2020

	Description		: Create history table for retail item cost prices
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RETAILITEMCOSTHISTORY')
BEGIN
	CREATE TABLE [dbo].[RETAILITEMCOSTHISTORY](
		[ID] [uniqueidentifier] NOT NULL,
		[ITEMID] [nvarchar](40) NOT NULL,
		[STOREID] [nvarchar](40) NOT NULL,
		[COST] decimal(28, 12) NOT NULL,
		[UNITID] [nvarchar](40) NOT NULL,
		[ENTRYDATE] [datetime] NOT NULL
	 CONSTRAINT [PK_RETAILITEMCOSTHISTORY] PRIMARY KEY CLUSTERED
	(
		[ID] ASC, [ITEMID] ASC, [STOREID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	EXEC spDB_SetTableDescription_1_0 'RETAILITEMCOSTHISTORY','Contains archived weighted cost prices per store for retail item';

	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOSTHISTORY', 'ID', 'Unique ID of the entry';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOSTHISTORY', 'ITEMID', 'ID of the item';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOSTHISTORY', 'STOREID', 'ID of the store';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOSTHISTORY', 'COST', 'Calculated cost price';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOSTHISTORY', 'UNITID', 'Inventory unit ID in which the cost was calculated';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOSTHISTORY', 'ENTRYDATE', 'Date of the calculated price';
END
GO