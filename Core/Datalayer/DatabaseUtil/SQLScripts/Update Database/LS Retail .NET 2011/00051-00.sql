/*

	Incident No.	: 8276
	Responsible		: Hörður Kristjánsson
	Sprint			: 2011 - Sprint 5
	Date created	: 24.01.2011

	Description		: Add new table REMOTEHOSTS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: REMOTEHOSTS	-	Table added
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[REMOTEHOSTS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[REMOTEHOSTS](
		[ID] [nvarchar](20) NOT NULL,
		[DESCRIPTION] [nvarchar](60) NULL,
		[ADDRESS] [nvarchar](100) NULL,
		[PORT] [int] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_REMOTEHOSTS] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO