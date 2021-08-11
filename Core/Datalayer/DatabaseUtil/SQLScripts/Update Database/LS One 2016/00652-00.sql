
/*

	Incident No.	: 
	Responsible		: Hörður Kristjánsson
	Sprint			: Kuala Lumpur

	Description		: Adding new retail item tables
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

USE LSPOSNET
GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DIMENSIONATTRIBUTE')
begin
	CREATE TABLE [dbo].[DIMENSIONATTRIBUTE](
		[ID] [uniqueidentifier] NOT NULL,
		[DIMENSIONID] [uniqueidentifier] NOT NULL,
		[DESCRIPTION] [nvarchar](30) NOT NULL,
		[CODE] [nvarchar](20) NOT NULL,
		[SEQUENCE] [int] NOT NULL,
		[DELETED] [bit] NOT NULL,
	 CONSTRAINT [PK_DIMENSIONATTRIBUTE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[DIMENSIONATTRIBUTE] ADD  CONSTRAINT [DF_DIMENSIONATTRIBUTE_ID]  DEFAULT (newid()) FOR [ID]

	ALTER TABLE [dbo].[DIMENSIONATTRIBUTE] ADD  CONSTRAINT [DF_DIMENSIONATTRIBUTE_DESCRIPTION]  DEFAULT ('') FOR [DESCRIPTION]

	ALTER TABLE [dbo].[DIMENSIONATTRIBUTE] ADD  CONSTRAINT [DF_DIMENSIONATTRIBUTE_CODE]  DEFAULT ('') FOR [CODE]

	ALTER TABLE [dbo].[DIMENSIONATTRIBUTE] ADD  CONSTRAINT [DF_DIMENSIONATTRIBUTE_SEQUENCE]  DEFAULT ((0)) FOR [SEQUENCE]

	ALTER TABLE [dbo].[DIMENSIONATTRIBUTE] ADD  CONSTRAINT [DF_DIMENSIONATTRIBUTE_DELETED]  DEFAULT ((0)) FOR [DELETED]

	CREATE NONCLUSTERED INDEX [IX_DIMENSIONATTRIBUTE_CODE] ON [dbo].[DIMENSIONATTRIBUTE]
	(
		[CODE] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_DIMENSIONATTRIBUTE_DESCRIPTION] ON [dbo].[DIMENSIONATTRIBUTE]
	(
		[DESCRIPTION] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_DIMENSIONATTRIBUTE_DIMENSIONID] ON [dbo].[DIMENSIONATTRIBUTE]
	(
		[DIMENSIONID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

end


if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DIMENSIONTEMPLATE')
begin
	CREATE TABLE [dbo].[DIMENSIONTEMPLATE](
		[ID] [uniqueidentifier] NOT NULL,
		[DESCRIPTION] [nvarchar](30) NOT NULL,
		[DELETED] [bit] NOT NULL,
	 CONSTRAINT [PK_DIMENSIONTEMPLATE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[DIMENSIONTEMPLATE] ADD  CONSTRAINT [DF_DIMENSIONTEMPLATE_ID]  DEFAULT (newid()) FOR [ID]

	ALTER TABLE [dbo].[DIMENSIONTEMPLATE] ADD  CONSTRAINT [DF_DIMENSIONTEMPLATE_DELETED]  DEFAULT ((0)) FOR [DELETED]

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEM')
begin
	CREATE TABLE [dbo].[RETAILITEM](
		[MASTERID] [uniqueidentifier] NOT NULL,
		[ITEMID] [nvarchar](20) NOT NULL,
		[HEADERITEMID] [uniqueidentifier] NULL,
		[ITEMNAME] [nvarchar](60) NOT NULL,
		[VARIANTNAME] [nvarchar](60) NOT NULL,
		[ITEMTYPE] [tinyint] NOT NULL,
		[DEFAULTVENDORID] [nvarchar](20) NOT NULL,
		[NAMEALIAS] [nvarchar](60) NOT NULL,
		[EXTENDEDDESCRIPTION] [nvarchar](max) NULL,
		[RETAILGROUPMASTERID] [uniqueidentifier]  NULL,
		[RETAILDEPARTMENTMASTERID] [uniqueidentifier] NULL,
		[RETAILDIVISIONMASTERID] [uniqueidentifier] NULL,
		[ZEROPRICEVALID] [bit] NOT NULL,
		[QTYBECOMESNEGATIVE] [bit] NOT NULL,
		[NODISCOUNTALLOWED] [bit] NOT NULL,
		[KEYINPRICE] [tinyint] NOT NULL,
		[SCALEITEM] [bit] NOT NULL,
		[KEYINQTY] [tinyint] NOT NULL, 
		[BLOCKEDONPOS] [bit] NOT NULL,
		[BARCODESETUPID] [nvarchar](20) NOT NULL,
		[PRINTVARIANTSSHELFLABELS] [bit] NOT NULL,
		[FUELITEM] [bit] NOT NULL,
		[GRADEID] [nvarchar](20) NOT NULL,
		[MUSTKEYINCOMMENT] [bit] NOT NULL,
		[DATETOBEBLOCKED] [datetime] NOT NULL,
		[DATETOACTIVATEITEM] [datetime] NOT NULL,
		[PROFITMARGIN] [numeric](28, 12) NOT NULL,
		[VALIDATIONPERIODID] [nvarchar](20) NOT NULL,
		[MUSTSELECTUOM] [bit] NOT NULL,
		[INVENTORYUNITID] [nvarchar](20) NOT NULL,
		[PURCHASEUNITID] [nvarchar](20) NOT NULL,
		[SALESUNITID] [nvarchar](20) NOT NULL,
		[PURCHASEPRICE] [numeric](28, 12) NOT NULL,
		[PURCHASEPRICEINCLTAX] [numeric](28, 12) NOT NULL,
		[PURCHASEPRICEUNIT] [numeric](28, 12) NOT NULL,
		[PURCHASEMARKUP] [numeric](28, 12) NOT NULL,
		[SALESPRICE] [numeric](28, 12) NOT NULL,
		[SALESPRICEINCLTAX] [numeric](28, 12) NOT NULL,
		[SALESPRICEUNIT] [numeric](28, 12) NOT NULL,
		[SALESMARKUP] [numeric](28, 12) NOT NULL,
		[SALESLINEDISC] [nvarchar](20) NOT NULL,
		[SALESMULTILINEDISC] [nvarchar](20) NOT NULL,
		[SALESALLOWTOTALDISCOUNT] [bit] NOT NULL,
		[SALESTAXITEMGROUPID] [nvarchar](20) NOT NULL,
		[DELETED] [bit] NOT NULL,
	 CONSTRAINT [PK_RETAILITEM] PRIMARY KEY CLUSTERED 
	(
		[MASTERID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_ID]  DEFAULT (newid()) FOR [MASTERID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_ITEMNAME]  DEFAULT ('') FOR [ITEMNAME]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_VARIANTNAME]  DEFAULT ('') FOR [VARIANTNAME]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_ITEMTYPE]  DEFAULT ((-1)) FOR [ITEMTYPE]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_DEFAULTVENDORID]  DEFAULT ('') FOR [DEFAULTVENDORID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_NAMEALIAS]  DEFAULT ('') FOR [NAMEALIAS]

	--ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_ITEMDEPARTMENT]  DEFAULT ('') FOR [RETAILDEPARTMENTID]

	--ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_DIVISIONID]  DEFAULT ('') FOR [RETAILDIVISIONID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_ZEROPRICEVALID]  DEFAULT ((0)) FOR [ZEROPRICEVALID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_QTYBECOMESNEGATIVE]  DEFAULT ((0)) FOR [QTYBECOMESNEGATIVE]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_NODISCOUNTALLOWED]  DEFAULT ((0)) FOR [NODISCOUNTALLOWED]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_KEYINGINPRICE]  DEFAULT ((0)) FOR [KEYINPRICE]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SCALEITEM]  DEFAULT ((0)) FOR [SCALEITEM]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_KEYINGINQTY]  DEFAULT ((0)) FOR [KEYINQTY]
	
	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_BLOCKEDONPOS]  DEFAULT ((0)) FOR [BLOCKEDONPOS]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_BARCODESETUPID]  DEFAULT ('') FOR [BARCODESETUPID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_PRINTVARIANTSSHELFLABELS]  DEFAULT ((0)) FOR [PRINTVARIANTSSHELFLABELS]
	
	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_FUELITEM]  DEFAULT ((0)) FOR [FUELITEM]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_GRADEID]  DEFAULT ('') FOR [GRADEID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_MUSTKEYINCOMMENT]  DEFAULT ((0)) FOR [MUSTKEYINCOMMENT]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_DATETOBEBLOCKED]  DEFAULT ('1900-01-01 00:00:00.000') FOR [DATETOBEBLOCKED]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_DATETOACTIVATEITEM]  DEFAULT ('1900-01-01 00:00:00.000') FOR [DATETOACTIVATEITEM]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_DEFAULTPROFIT]  DEFAULT ((0)) FOR [PROFITMARGIN]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_POSPERIODICID]  DEFAULT ('') FOR [VALIDATIONPERIODID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_MUSTSELECTUOM]  DEFAULT ((0)) FOR [MUSTSELECTUOM]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_INVENTORYUNITID]  DEFAULT ('') FOR [INVENTORYUNITID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_PURCHASEUNITID]  DEFAULT ('') FOR [PURCHASEUNITID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESUNITID]  DEFAULT ('') FOR [SALESUNITID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_INVENTORYPRICE]  DEFAULT ((0)) FOR [PURCHASEPRICE]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_PURCHASEPRICEINCLTAX]  DEFAULT ((0)) FOR [PURCHASEPRICEINCLTAX]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_PURCHASEPRICEUNIT]  DEFAULT ((0)) FOR [PURCHASEPRICEUNIT]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_PURCHASEMARKUP]  DEFAULT ((0)) FOR [PURCHASEMARKUP]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESPRICE]  DEFAULT ((0)) FOR [SALESPRICE]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESPRICEINCLTAX]  DEFAULT ((0)) FOR [SALESPRICEINCLTAX]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESPRICEUNIT]  DEFAULT ((0)) FOR [SALESPRICEUNIT]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESMARKUP]  DEFAULT ((0)) FOR [SALESMARKUP]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESLINEDISC]  DEFAULT ('') FOR [SALESLINEDISC]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESMULTILINEDISC]  DEFAULT ('') FOR [SALESMULTILINEDISC]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESALLOWTOTALDISCOUNT]  DEFAULT ((0)) FOR [SALESALLOWTOTALDISCOUNT]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_SALESTAXITEMGROUPID]  DEFAULT ('') FOR [SALESTAXITEMGROUPID]

	ALTER TABLE [dbo].[RETAILITEM] ADD  CONSTRAINT [DF_RETAILITEM_DELETED]  DEFAULT ((0)) FOR [DELETED]

	CREATE NONCLUSTERED INDEX [IX_RETAILITEM_HEADERITEMID] ON [dbo].[RETAILITEM]
	(
		[HEADERITEMID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_RETAILITEM_ITEMNAME] ON [dbo].[RETAILITEM]
	(
		[ITEMNAME] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_RETAILITEM_VARIANTNAME] ON [dbo].[RETAILITEM]
	(
		[VARIANTNAME] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEMDIMENSION')
begin
	CREATE TABLE [dbo].[RETAILITEMDIMENSION](
		[ID] [uniqueidentifier] NOT NULL,
		[RETAILITEMID] [uniqueidentifier] NOT NULL,
		[DESCRIPTION] [nvarchar](30) NOT NULL,
		[SEQUENCE] [int] NOT NULL,
		[DELETED] [bit] NOT NULL,
	 CONSTRAINT [PK_RETAILITEMDIMENSION] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RETAILITEMDIMENSION] ADD  CONSTRAINT [DF_RETAILITEMDIMENSION_ID]  DEFAULT (newid()) FOR [ID]

	ALTER TABLE [dbo].[RETAILITEMDIMENSION] ADD  CONSTRAINT [DF_RETAILITEMDIMENSION_DESCRIPTION]  DEFAULT ('') FOR [DESCRIPTION]

	ALTER TABLE [dbo].[RETAILITEMDIMENSION] ADD  CONSTRAINT [DF_RETAILITEMDIMENSION_SEQUENCE]  DEFAULT ((0)) FOR [SEQUENCE]

	ALTER TABLE [dbo].[RETAILITEMDIMENSION] ADD  CONSTRAINT [DF_RETAILITEMDIMENSION_DELETED]  DEFAULT ((0)) FOR [DELETED]

	CREATE NONCLUSTERED INDEX [IX_RETAILITEMDIMENSION_RETAILITEMID] ON [dbo].[RETAILITEMDIMENSION]
	(
		[RETAILITEMID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEMDIMENSIONATTRIBUTE')
begin
	CREATE TABLE [dbo].[RETAILITEMDIMENSIONATTRIBUTE](
		[RETAILITEMID] [uniqueidentifier] NOT NULL,
		[DIMENSIONATTRIBUTEID] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_RETAILITEMDIMENSIONATTRIBUTE] PRIMARY KEY CLUSTERED 
	(
		[RETAILITEMID] ASC,
		[DIMENSIONATTRIBUTEID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_RETAILITEMDIMENSIONATTRIBUTE_RETAILITEMID] ON [dbo].[RETAILITEMDIMENSIONATTRIBUTE]
	(
		[RETAILITEMID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_RETAILITEMDIMENSIONATTRIBUTE_DIMENSIONATTRIBUTEID] ON [dbo].[RETAILITEMDIMENSIONATTRIBUTE]
	(
		[DIMENSIONATTRIBUTEID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'PERIODICDISCOUNT')
begin
CREATE TABLE [dbo].[PERIODICDISCOUNT](
	[MASTERID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[OFFERID] [nvarchar](20) NOT NULL,
	[DESCRIPTION] [nvarchar](60) NOT NULL,
	[STATUS] [tinyint] NULL,
	[PDTYPE] [int] NULL,
	[PRIORITY] [int] NULL,
	[DISCVALIDPERIODID] [nvarchar](20) NULL,
	[DISCOUNTTYPE] [int] NULL,
	[NOOFLINESTOTRIGGER] [int] NULL,
	[DEALPRICEVALUE] [numeric](28, 12) NULL CONSTRAINT [DF_PERIODICDISCOUNT_DEALPRICEVALUE]  DEFAULT ((0)),
	[DISCOUNTPCTVALUE] [numeric](28, 12) NULL CONSTRAINT [DF_PERIODICDISCOUNT_DISCOUNTPCTVALUE]  DEFAULT ((0)),
	[DISCOUNTAMOUNTVALUE] [numeric](28, 12) NULL CONSTRAINT [DF_PERIODICDISCOUNT_DISCOUNTAMOUNTVALUE]  DEFAULT ((0)),
	[NOOFLEASTEXPITEMS] [int] NULL,
	[PRICEGROUP] [nvarchar](20) NULL,
	[ACCOUNTCODE] [int] NULL,
	[ACCOUNTRELATION] [nvarchar](20) NULL,
	[TRIGGERED] [int] NULL,
	[DELETED] [bit] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_PERIODICDISCOUNT] PRIMARY KEY CLUSTERED 
(
	[MASTERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end


if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'PERIODICDISCOUNTLINE')
begin
CREATE TABLE [dbo].[PERIODICDISCOUNTLINE](
	[OFFERID] [nvarchar](20) NOT NULL,
	[OFFERMASTERID] [uniqueidentifier] NOT NULL,
	[LINEID] [int] NOT NULL,
	[PRODUCTTYPE] [int] NULL,
	[TARGETID] [nvarchar](30) NOT NULL,
	[TARGETMASTERID] [uniqueidentifier] NOT NULL,
	[DEALPRICEORDISCPCT] [numeric](28, 12) NULL CONSTRAINT [DF_PERIODICDISCOUNTLINE_DEALPRICEORDISCPCT]  DEFAULT ((0)),
	[LINEGROUP] [nvarchar](60) NOT NULL,
	[DISCTYPE] [int] NULL CONSTRAINT [DF_PERIODICDISCOUNTLINE_DISCTYPE]  DEFAULT ((0)),
	[POSPERIODICDISCOUNTLINEGUID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[DISCPCT] [numeric](28, 12) NULL DEFAULT ((0)),
	[DISCAMOUNT] [numeric](28, 12) NULL DEFAULT ((0)),
	[DISCAMOUNTINCLTAX] [numeric](28, 12) NULL DEFAULT ((0)),
	[OFFERPRICE] [numeric](28, 12) NULL DEFAULT ((0)),
	[OFFERPRICEINCLTAX] [numeric](28, 12) NULL DEFAULT ((0)),
	[DELETED] [bit] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_PERIODICDISCOUNTLINE] PRIMARY KEY CLUSTERED 
(
	[POSPERIODICDISCOUNTLINEGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILDIVISION')
begin
CREATE TABLE [dbo].[RETAILDIVISION](
	[DIVISIONID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL,
	[MASTERID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[DELETED] [bit] NOT NULL DEFAULT 0,
 CONSTRAINT [RETAILDIVISIO_PK] PRIMARY KEY CLUSTERED 
(
	[MASTERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILDEPARTMENT')
begin
CREATE TABLE [dbo].[RETAILDEPARTMENT](
	[DEPARTMENTID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL DEFAULT (''),
	[NAMEALIAS] [nvarchar](20) NULL DEFAULT (''),
	[DIVISIONID] [nvarchar](20) NULL,
	[DIVISIONMASTERID] [uniqueidentifier] NULL,
	[MASTERID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[DELETED] [bit] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_RETAILDEPARTMENT] PRIMARY KEY CLUSTERED 
(
	[MASTERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILGROUP')
begin
CREATE TABLE [dbo].[RETAILGROUP](
	[MASTERID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[GROUPID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL,
	[DEPARTMENTID] [nvarchar](20) NULL,
	[SALESTAXITEMGROUP] [nvarchar](20) NULL,
	[DEFAULTPROFIT] [numeric](28, 12) NULL,
	[POSPERIODICID] [nvarchar](20) NULL,
	[DIVISIONMASTERID] [uniqueidentifier] NULL,
	[DELETED] [bit] NOT NULL DEFAULT 0,
	[DEPARTMENTMASTERID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RETAILGROUP] PRIMARY KEY CLUSTERED 
(
	[MASTERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SPECIALGROUP')
begin
CREATE TABLE [dbo].[SPECIALGROUP](
	[GROUPID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL,
	[DELETED] [bit] NOT NULL DEFAULT 0,
	[MASTERID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
 CONSTRAINT [PK_SPECIALGROUP] PRIMARY KEY CLUSTERED 
(
	[MASTERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SPECIALGROUPITEMS')
begin
CREATE TABLE [dbo].[SPECIALGROUPITEMS](
	[GROUPID] [nvarchar](20) NOT NULL,
	[ITEMID] [nvarchar](20) NOT NULL,
	[MEMBERMASTERID] [uniqueidentifier] NOT NULL,
	[GROUPMASTERID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SPECIALGROUPITEMS] PRIMARY KEY CLUSTERED 
(
	[MEMBERMASTERID] ASC,
	[GROUPMASTERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end



if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEMIMAGE')
begin
CREATE TABLE [dbo].[RETAILITEMIMAGE](
	[ITEMMASTERID] [uniqueidentifier] not null,
	[ITEMID] [nvarchar](20) NOT NULL,
	[IMAGE] [varbinary](max) NULL,
	[IMAGEINDEX] [tinyint] NOT NULL,
 CONSTRAINT [PK_RETAILITEMIMAGE] PRIMARY KEY CLUSTERED 
(
	[ITEMMASTERID] ASC,
	[IMAGEINDEX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


end


IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_RETAILITEM_ITEMID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE UNIQUE INDEX LS_RETAILITEM_ITEMID 
   ON RETAILITEM(ITEMID); 

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_PERIODICDISCOUNT_OFFERID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE UNIQUE INDEX LS_PERIODICDISCOUNT_OFFERID 
   ON PERIODICDISCOUNT(OFFERID); 

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_RETAILDIVISION_DIVISIONID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE UNIQUE INDEX LS_RETAILDIVISION_DIVISIONID 
   ON RETAILDIVISION(DIVISIONID); 

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_RETAILDEPARTMENT_DEPARTMENTID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE UNIQUE INDEX LS_RETAILDEPARTMENT_DEPARTMENTID 
   ON RETAILDEPARTMENT(DEPARTMENTID); 

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_RETAILGROUP_GROUPID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE UNIQUE INDEX LS_RETAILGROUP_GROUPID 
   ON RETAILGROUP(GROUPID); 

IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_SPECIALGROUP_GROUPID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE UNIQUE INDEX LS_SPECIALGROUP_GROUPID 
   ON SPECIALGROUP(GROUPID); 


   IF NOT EXISTS (SELECT name from sys.indexes
           WHERE name = N'LS_RETAILITEMIMAGE_ITEMID') 
-- Create a unique index called AK_UnitMeasure_Name
-- on the Production.UnitMeasure table using the Name column.
CREATE INDEX LS_RETAILITEMIMAGE_ITEMID 
   ON RETAILITEMIMAGE(ITEMID); 


   
            IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'INVENTSUM'  AND CONSTRAINT_NAME = 'PK_INVENTSUM')
BEGIN
    ALTER TABLE INVENTSUM

        DROP CONSTRAINT PK_INVENTSUM

END


  IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTSUM' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
   ALTER TABLE INVENTSUM
    
        DROP COLUMN DATAAREAID
END

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[INSERT_INVENTTRANS]'))
begin
   drop trigger dbo.INSERT_INVENTTRANS
end

GO

create trigger INSERT_INVENTTRANS
on INVENTTRANS after insert as

declare @itemID nvarchar(20)
declare @storeID nvarchar(20)
declare @adjusted [numeric](28, 12)
declare @oldAdjustment [numeric](28, 12)
declare @sql nvarchar(max)

select @itemID = ITEMID,  @storeID = STOREID, @adjusted = ADJUSTMENTININVENTORYUNIT
from inserted;

select @oldAdjustment = QUANTITY from dbo.INVENTSUM
where ITEMID = @itemID and STOREID = @storeID 

if @oldAdjustment is NULL
	begin
		-- We need to insert a new INVENTSUM record
		-- The @sql string manipulation is needed since the SQL server pre-compiler will give an error on a missing VARANTID
		-- column even if we encapsulate it with the "if - else" statement
		if exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'INVENTSUM' and COLUMN_NAME = 'VARIANTID')
		begin		
			set @sql = 
			'insert into INVENTSUM ([ITEMID],[STOREID],[QUANTITY],[VARIANTID])
			 values (' + @itemID + ' ,' + @storeID + ' ,' + @adjusted + ','')'
		end else begin
			set @sql =
			'insert into INVENTSUM ([ITEMID],[STOREID],[QUANTITY])
			 values (' + @itemID + ' ,' + @storeID + ' ,' + @adjusted + ')'
		end

		exec(@sql)
	end
else 
	begin
		-- We update the existing INVENTSUM record
		update INVENTSUM
		set [QUANTITY] = (@oldAdjustment + @adjusted)
		where [ITEMID] = @itemID and [STOREID] = @storeID 
	end

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTJOURNALTRANS]'))
begin
   drop trigger dbo.Update_INVENTJOURNALTRANS
end

GO

create trigger Update_INVENTJOURNALTRANS
on INVENTJOURNALTRANS after insert,update, delete as

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int
Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

if @writeAudit = 1
begin
    
	set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

	if @connectionUser IS null
		begin
			set @sessionUser = SYSTEM_USER
			set @connectionUser = NewID()
		end
	else
		set @sessionUser = ''
		
	declare @DeletedCount int
	declare @InsertedCount int
	
	select @DeletedCount = COUNT(*) FROM DELETED
	select @InsertedCount = COUNT(*) FROM inserted
	
	begin try
		if @DeletedCount > 0 and @InsertedCount = 0
		begin
			insert into LSPOSNET_Audit.dbo.INVENTJOURNALTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				JOURNALID,
				LINENUM,
				TRANSDATE,
				ITEMID,
				ADJUSTMENT,
				COSTPRICE,
				PRICEUNIT,
				COSTMARKUP,
				COSTAMOUNT,
				SALESAMOUNT,
				INVENTONHAND,
				COUNTED,
				REASONREFRECID,
				VARIANTID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.JOURNALID,
				ins.LINENUM,
				ins.TRANSDATE,
				ins.ITEMID,
				ins.ADJUSTMENT,
				ins.COSTPRICE,
				ins.PRICEUNIT,
				ins.COSTMARKUP,
				ins.COSTAMOUNT,
				ins.SALESAMOUNT,
				ins.INVENTONHAND,
				ins.COUNTED,
				ins.REASONREFRECID,
				'',
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTJOURNALTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				JOURNALID,
				LINENUM,
				TRANSDATE,
				ITEMID,
				ADJUSTMENT,
				COSTPRICE,
				PRICEUNIT,
				COSTMARKUP,
				COSTAMOUNT,
				SALESAMOUNT,
				INVENTONHAND,
				COUNTED,
				REASONREFRECID,
				VARIANTID,	
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.JOURNALID,
				ins.LINENUM,
				ins.TRANSDATE,
				ins.ITEMID,
				ins.ADJUSTMENT,
				ins.COSTPRICE,
				ins.PRICEUNIT,
				ins.COSTMARKUP,
				ins.COSTAMOUNT,
				ins.SALESAMOUNT,
				ins.INVENTONHAND,
				ins.COUNTED,
				ins.REASONREFRECID,
				'',
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PURCHASEORDERLINE]'))
begin
   drop trigger dbo.Update_PURCHASEORDERLINE
end

GO

create trigger Update_PURCHASEORDERLINE
on PURCHASEORDERLINE after insert,update, delete as

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int
Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

if @writeAudit = 1
begin
    
	set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

	if @connectionUser IS null
		begin
			set @sessionUser = SYSTEM_USER
			set @connectionUser = NewID()
		end
	else
		set @sessionUser = ''
		
	declare @DeletedCount int
	declare @InsertedCount int
	
	select @DeletedCount = COUNT(*) FROM DELETED
	select @InsertedCount = COUNT(*) FROM inserted
	
	begin try
		if @DeletedCount > 0 and @InsertedCount = 0
		begin
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				LINENUMBER,
				RETAILITEMID,
				VENDORITEMID,
				VARIANTID,
				UNITID,
				QUANTITY,
				PRICE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.LINENUMBER,
				ins.RETAILITEMID,
				ins.VENDORITEMID,
				'',
				ins.UNITID,
				ins.QUANTITY,
				ins.PRICE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				LINENUMBER,
				RETAILITEMID,
				VENDORITEMID,
				VARIANTID,
				UNITID,
				QUANTITY,
				PRICE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.LINENUMBER,
				ins.RETAILITEMID,
				ins.VENDORITEMID,
				'',
				ins.UNITID,
				ins.QUANTITY,
				ins.PRICE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTRANSFERORDERLINE]'))
begin
   drop trigger dbo.Update_INVENTORYTRANSFERORDERLINE
end

GO

create trigger Update_INVENTORYTRANSFERORDERLINE
on INVENTORYTRANSFERORDERLINE after insert,update, delete as

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int
Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

if @writeAudit = 1
begin

    set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

    if @connectionUser IS null
        begin
            set @sessionUser = SYSTEM_USER
            set @connectionUser = NewID()
        end
    else
        set @sessionUser = ''

    declare @DeletedCount int
    declare @InsertedCount int

    select @DeletedCount = COUNT(*) FROM DELETED
    select @InsertedCount = COUNT(*) FROM inserted

    begin try
        if @DeletedCount > 0 and @InsertedCount = 0
        begin
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERORDERLINELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERORDERID,
				ITEMID,
				VARIANTID,
				UNITID,
				QUANTITYSENT,
				QUANTITYRECEIVED,
				SENT,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERORDERID,
				ins.ITEMID,
				'',
				ins.UNITID,
				ins.QUANTITYSENT,
				ins.QUANTITYRECEIVED,
				ins.SENT,
				ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERORDERLINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERORDERID,
				ITEMID,
				VARIANTID,
				UNITID,
				QUANTITYSENT,
				QUANTITYRECEIVED,
				SENT,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERORDERID,
				ins.ITEMID,
				'',
				ins.UNITID,
				ins.QUANTITYSENT,
				ins.QUANTITYRECEIVED,
				ins.SENT,
				ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTRANSFERREQUESTLINE]'))
begin
   drop trigger dbo.Update_INVENTORYTRANSFERREQUESTLINE
end

GO


create trigger Update_INVENTORYTRANSFERREQUESTLINE
on INVENTORYTRANSFERREQUESTLINE after insert,update, delete as

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int
Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

if @writeAudit = 1
begin

    set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

    if @connectionUser IS null
        begin
            set @sessionUser = SYSTEM_USER
            set @connectionUser = NewID()
        end
    else
        set @sessionUser = ''

    declare @DeletedCount int
    declare @InsertedCount int

    select @DeletedCount = COUNT(*) FROM DELETED
    select @InsertedCount = COUNT(*) FROM inserted

    begin try
        if @DeletedCount > 0 and @InsertedCount = 0
        begin
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERREQUESTLINELog (
                AuditUserGUID,
                AuditUserLogin,
                 AuditDate,
                ID,
				INVENTORYTRANSFERREQUESTID,
				ITEMID,
				VARIANTID,
				UNITID,
				QUANTITYREQUESTED,
				SENT,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERREQUESTID,
				ins.ITEMID,
				'',
				ins.UNITID,
				ins.QUANTITYREQUESTED,
				ins.SENT,
				ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERREQUESTLINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERREQUESTID,
				ITEMID,
				VARIANTID,
				UNITID,
				QUANTITYREQUESTED,
				SENT,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERREQUESTID,
				ins.ITEMID,
				'',
				ins.UNITID,
				ins.QUANTITYREQUESTED,
				ins.SENT,
				ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
