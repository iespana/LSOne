/*
	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 05.06.2013

	Description		: Create tables for customer addresses
*/

USE LSPOSNET
GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CUSTOMERADDRESSTYPE')
BEGIN
	CREATE TABLE [dbo].[CUSTOMERADDRESSTYPE]
	(
		[ADDRESSTYPE] [int] NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		[NAME] [nvarchar](30)
		CONSTRAINT [PK_CUSTOMERADDRESSTYPE] PRIMARY KEY CLUSTERED 
		(
			[ADDRESSTYPE] ASC,
			[DATAAREAID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF (0 = (SELECT count(*) FROM [dbo].[CUSTOMERADDRESSTYPE] WHERE [DATAAREAID] = 'LSR'))
BEGIN
	INSERT INTO [dbo].[CUSTOMERADDRESSTYPE] ([ADDRESSTYPE],[DATAAREAID],[NAME]) values(0,'LSR','Default');
	INSERT INTO [dbo].[CUSTOMERADDRESSTYPE] ([ADDRESSTYPE],[DATAAREAID],[NAME]) values(1,'LSR','Shipping');
	INSERT INTO [dbo].[CUSTOMERADDRESSTYPE] ([ADDRESSTYPE],[DATAAREAID],[NAME]) values(2,'LSR','Billing');
END

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CUSTOMERADDRESS')
BEGIN
	CREATE TABLE [dbo].[CUSTOMERADDRESS]
	(
		[ACCOUNTNUM] [nvarchar](20) NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		[ADDRESSTYPE] [int] NOT NULL,
		[STREET] [nvarchar](250) NULL,
		[ADDRESS] [nvarchar](250) NULL,
		[ZIPCODE] [nvarchar](10) NULL,
		[CITY] [nvarchar](60) NULL,
		[STATE] [nvarchar](30) NULL,
		[COUNTY] [nvarchar](10) NULL,
		[COUNTRY] [nvarchar](20) NULL,
		[ADDRESSFORMAT] [int] NULL,

		[TAXGROUP] [nvarchar](20) NULL,

		[PHONE] [nvarchar](20) NULL,
		[CELLULARPHONE] [nvarchar](20) NULL,
		[EMAIL] [nvarchar](80) NULL
		CONSTRAINT [PK_CUSTOMERADDRESS] PRIMARY KEY CLUSTERED 
		(
			[ACCOUNTNUM] ASC,
			[DATAAREAID] ASC,
			[ADDRESSTYPE] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[CUSTOMERADDRESS]  WITH CHECK ADD FOREIGN KEY([ADDRESSTYPE],[DATAAREAID])
	REFERENCES [dbo].[CUSTOMERADDRESSTYPE] ([ADDRESSTYPE],[DATAAREAID])
END
