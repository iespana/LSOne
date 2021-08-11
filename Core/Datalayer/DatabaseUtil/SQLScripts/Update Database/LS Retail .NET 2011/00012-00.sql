
/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 03\Dot Net Team
	Date created	: 15.11.2010
	
	Description		: Adding a table structure for contacts

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:  CONTACTTABLE, VENDTABLE
						
*/

Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CONTACTTABLE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE dbo.CONTACTTABLE
		(
		[CONTACTID] nvarchar(20) NOT NULL,
		[OWNERID] nvarchar(20) NOT NULL,
		[OWNERTYPE] int NOT NULL, -- 1 = Vendor
		[CONTACTTYPE] int NOT NULL, -- 1 = Person, 2 Company 
		[COMPANYNAME] nvarchar(60) NOT NULL,
		[FirstName] [nvarchar](31) NOT NULL,
		[MiddleName] [nvarchar](15) NOT NULL,
		[LastName] [nvarchar](20) NOT NULL,
		[NamePrefix] [nvarchar](8) NOT NULL,
		[NameSuffix] [nvarchar](8) NOT NULL,
		[ADDRESS] [nvarchar](250) NULL,
		[STREET] [nvarchar](250) NULL,
		[ZIPCODE] [nvarchar](10) NULL,
		[CITY] [nvarchar](60) NULL,
		[COUNTY] [nvarchar](10) NULL,
		[STATE] [nvarchar](10) NULL,
		[COUNTRY] [nvarchar](10) NULL,
		[PHONE] nvarchar(20) NULL,
		[PHONE2] nvarchar(20) NULL,
		[FAX] nvarchar(20),
		[EMAIL] nvarchar(80) NULL,
		DATAAREAID nvarchar(4) NOT NULL
		)  ON [PRIMARY]

	ALTER TABLE [dbo].CONTACTTABLE ADD  CONSTRAINT [DF__CONTACTTABLE__DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]


	ALTER TABLE dbo.CONTACTTABLE ADD CONSTRAINT PK_CONTACTTABLE
	PRIMARY KEY CLUSTERED ([CONTACTID],DATAAREAID) ON [PRIMARY]

	CREATE INDEX IX_CONTACTTABLE 
	ON dbo.CONTACTTABLE (OWNERID)

END

GO