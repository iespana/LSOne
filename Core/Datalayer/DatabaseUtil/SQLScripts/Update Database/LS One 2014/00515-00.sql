
/*
	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 18.02.2014

	Description		: Adding table for the new table layout designer
						
*/
USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LSHFLOORLAYOUT')
BEGIN
	CREATE TABLE [dbo].[LSHFLOORLAYOUT](
	[FLOORLAYOUTID] [nvarchar](20) NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[DESCRIPTION] [nvarchar](60) NOT NULL,
	[FLOORLAYOUTDESIGN] [varbinary](max) NULL,
 CONSTRAINT [PK_LSHFLOORLAYOUT] PRIMARY KEY CLUSTERED 
	(
		[FLOORLAYOUTID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]


END
GO
