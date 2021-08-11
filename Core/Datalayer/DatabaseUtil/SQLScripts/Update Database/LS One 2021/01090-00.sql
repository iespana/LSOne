/*
	Incident No.	: ONE-11308
	Responsible		: Adrian Chiorean
	Sprint			: Renminbi
	Date created	: 03.12.2020

	Description		: Create table for retail item cost prices
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RETAILITEMCOST')
BEGIN
	CREATE TABLE [dbo].[RETAILITEMCOST](
		[ID] [uniqueidentifier] NOT NULL,
		[ITEMID] [nvarchar](40) NOT NULL,
		[STOREID] [nvarchar](40) NOT NULL,
		[COST] decimal(28, 12) NOT NULL,
		[UNITID] [nvarchar](40) NOT NULL,
		[ENTRYDATE] [datetime] NOT NULL
	 CONSTRAINT [PK_RETAILITEMCOST] PRIMARY KEY CLUSTERED
	(
		[ID] ASC, [ITEMID] ASC, [STOREID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	EXEC spDB_SetTableDescription_1_0 'RETAILITEMCOST','Contains weighted cost prices per store for retail item';

	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOST', 'ID', 'Unique ID of the entry';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOST', 'ITEMID', 'ID of the item';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOST', 'STOREID', 'ID of the store';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOST', 'COST', 'Calculated cost price';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOST', 'UNITID', 'Inventory unit ID in which the cost was calculated';
	EXECUTE spDB_SetFieldDescription_1_0 'RETAILITEMCOST', 'ENTRYDATE', 'Date of the calculated price';
END

GO