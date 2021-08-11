/*

	Incident No.	: 
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: 
	Date created	: 30.04.2018

	Description		: Add new tables for the stock counting functionality in the OMNI server
*/

USE LSPOSNET
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTJOURNALTRANS' AND COLUMN_NAME = 'LINESTATUS')
BEGIN
	ALTER TABLE INVENTJOURNALTRANS ADD [LINESTATUS] INT NOT NULL DEFAULT 0
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OMNIJOURNAL')
BEGIN
	CREATE TABLE [dbo].[OMNIJOURNAL](
		[JOURNALID] [nvarchar](100) NOT NULL,
		[JOURNALTYPE] [int] NULL,
		[STOREID] [nvarchar](40) NULL,
		[TEMPLATEID] [nvarchar](100) NULL,
		[STAFFID] [nvarchar](100) NULL,
		[STATUS] [int] NULL,
		[CREATEDDATE] [datetime] NULL,
	 CONSTRAINT [PK_OMNIJOURNAL] PRIMARY KEY CLUSTERED 
	(
		[JOURNALID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[OMNIJOURNAL] ADD  CONSTRAINT [DF_OMNIJOURNAL_CREATEDDATE]  DEFAULT (getdate()) FOR [CREATEDDATE]
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OMNIJOURNAL')
BEGIN
	EXECUTE spDB_SetTableDescription_1_0 'OMNIJOURNAL', 'Information about a journal that is being sent from OMNI server to the One OMNI plugin'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'JOURNALID', 'Unique ID of the journal'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'JOURNALTYPE', 'The journal type i.e. stock counting, purchase order and etc.'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'STOREID', 'The store ID the device is configured to be at';
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'TEMPLATEID', 'The template that is associated with this journal';
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'STAFFID', 'The ID of the user that is logged onto the inventory app on the device'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'STATUS', 'Set while managing the journal in the One Plugin for the OMNI server - is not set by the device or the inventory app';
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNAL', 'CREATEDDATE', 'The date and time when the journal was created in the database'	
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OMNIJOURNALLINE')
BEGIN
	CREATE TABLE [dbo].[OMNIJOURNALLINE](
		[JOURNALID] [nvarchar](100) NOT NULL,
		[ITEMID] [nvarchar](100) NOT NULL,
		[UNITID] [nvarchar](100) NOT NULL,
		[COUNTED] [numeric](28, 12) NOT NULL,
		[STAFFID] [nvarchar](100) NOT NULL,
		[AREAID] [uniqueidentifier] NOT NULL,
		[BATCHID] [uniqueidentifier] NULL,
		[STATUS] [int] NOT NULL,
		[CREATEDDATE] [datetime] NULL,
	 CONSTRAINT [PK_OMNIJOURNALLINE] PRIMARY KEY CLUSTERED 
	(
		[JOURNALID] ASC,
		[ITEMID] ASC,
		[UNITID] ASC,
		[AREAID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[OMNIJOURNALLINE] ADD  CONSTRAINT [DF_OMNIJOURNALLINE_STATUS]  DEFAULT ((0)) FOR [STATUS]


	ALTER TABLE [dbo].[OMNIJOURNALLINE] ADD  CONSTRAINT [DF_OMNIJOURNALLINE_CREATEDDATE]  DEFAULT (getdate()) FOR [CREATEDDATE]

END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OMNIJOURNALLINE')
BEGIN
	EXECUTE spDB_SetTableDescription_1_0 'OMNIJOURNALLINE', 'Each item line created in the inventory app on the device is saved here'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'JOURNALID', 'Unique ID of the journal'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'ITEMID', 'The item ID'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'UNITID', 'The unit selected in the device'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'COUNTED', 'The counted number for the item'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'STAFFID', 'The ID of the user that is logged onto the inventory app on the device'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'AREAID', 'The ID of the area selected when doing stock counting'
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'STATUS', 'Set while managing the journal line in the One Plugin for the OMNI server - is not set by the device or the inventory app';
	EXECUTE spDB_SetFieldDescription_1_0 'OMNIJOURNALLINE', 'CREATEDDATE', 'The date and time when the journal line was created in the database'	
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTORYTEMPLATE' AND COLUMN_NAME = 'BATCHSIZE')
BEGIN
	ALTER TABLE INVENTORYTEMPLATE ADD [BATCHSIZE] INT NOT NULL DEFAULT 0
END
GO