﻿/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 2011 - Sprint 5
	Date created	: 15.02.2011

	Description		: Getting rid of table POSISUPDATEFILES; re-using POSISUPDATESFILES instead

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	POSISUPDATEFILES,  
						POSISUPDATESFILES
						
*/


USE LSPOSNET
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[DBO].[POSISUPDATEFILES]') AND TYPE IN (N'U'))
      DROP TABLE dbo.POSISUPDATEFILES
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[DBO].[POSISUPDATESFILES]') AND TYPE IN (N'U'))

CREATE TABLE [dbo].[POSISUPDATESFILES](
	[DLLID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
	[UPDATEID] [int] NULL,
	[FILEVERSION] [nvarchar](30) NULL,
	[FILEINSERTDATE] [datetime] NULL,
	[DATA] [varbinary](max) NULL,
	[COMPANY] [nvarchar](40) NULL,
	[DESCRIPTION] [nvarchar](255) NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[FOLDERPATH] [nvarchar](2) NULL,
	[FILEMODIFIEDDATE] [datetime] NULL,
	[DELETED] [int] NULL,
	[ADDEDTOUPDATE] [int] NULL,
 CONSTRAINT [PK_POSISUPDATESFILES] PRIMARY KEY CLUSTERED 
(
	[DLLID] ASC,
	[DATAAREAID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

