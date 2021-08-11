﻿
/*

	Incident No.	: 17313
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 19.6.2012

	Description		: Style tables added to the database

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSContext - new table
					  POSStyleProfile - new table
					  POSStyleProfileLine - new table
					  POSStyle - new table
					  POSMenuHeader - field added
					  RBOStoreTable - field added
						
*/

Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSCONTEXT]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSCONTEXT](
		[ID] [nvarchar](20) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[MENUREQUIRED] [tinyint] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_POSCONTEXT] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[POSCONTEXT] ADD  CONSTRAINT [DF_POSCONTEXT_DATAAREAID]  DEFAULT (N'LSR') FOR [DATAAREAID]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSSTYLEPROFILE]') AND TYPE IN ('U'))
BEGIN

	CREATE TABLE [dbo].[POSSTYLEPROFILE](
		[ID] [nvarchar](20) NOT NULL,
		[NAME] [nvarchar](60) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_POSSTYLEPROFILE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[POSSTYLEPROFILE] ADD  CONSTRAINT [DF_POSSTYLEPROFILE_DATAAREAID]  DEFAULT (N'LSR') FOR [DATAAREAID]

END

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSSTYLEPROFILELINE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSSTYLEPROFILELINE](
		[ID] [nvarchar](20) NOT NULL,
		[PROFILEID] [nvarchar](20) NOT NULL,
		[MENUID] [nvarchar](20) NULL,
		[CONTEXTID] [nvarchar](20) NULL,
		[STYLEID] [nvarchar](20) NULL,
		[SYSTEM] [tinyint] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_POSSTYLEPROFILELINE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[PROFILEID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[POSSTYLEPROFILELINE] ADD  CONSTRAINT [DF_POSSTYLEPROFILELINE_DATAAREAID]  DEFAULT (N'LSR') FOR [DATAAREAID]

END

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSSTYLE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[POSSTYLE](
		[ID] [nvarchar](20) NOT NULL,
		[NAME] [nvarchar](60) NULL,		
		[FONTNAME] [nvarchar](32) NULL,
		[FONTSIZE] [int] NULL,
		[FONTBOLD] [tinyint] NULL,
		[FORECOLOR] [int] NULL,
		[BACKCOLOR] [int] NULL,
		[FONTITALIC] [tinyint] NULL,
		[FONTCHARSET] [int] NULL,	
		[DATAAREAID] [nvarchar](4) NOT NULL,	
		[BACKCOLOR2] [int] NULL,
		[GRADIENTMODE] [int] NULL,
		[SHAPE] [int] NULL	
	 CONSTRAINT [PK_POSSTYLE] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]	

	ALTER TABLE [dbo].[POSSTYLE] ADD  CONSTRAINT [DF_POSSTYLE_DATAAREAID]  DEFAULT (N'LSR') FOR [DATAAREAID]
	
END

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMENUHEADER') AND NAME='STYLEID')
BEGIN
	ALTER TABLE dbo.POSMENUHEADER ADD STYLEID nvarchar(20) NULL
END

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='STYLEPROFILE')
BEGIN
	ALTER TABLE dbo.RBOSTORETABLE ADD STYLEPROFILE nvarchar(20) NULL
END

GO
