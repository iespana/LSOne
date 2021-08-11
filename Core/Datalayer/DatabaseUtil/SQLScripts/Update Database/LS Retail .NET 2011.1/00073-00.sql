
/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: N/A
	Date created	: 12.04.2011

	Description		: Created the RBOINVENTITEMIMAGE table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOINVENTITEMIMAGE - new fields
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTITEMIMAGE]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOINVENTITEMIMAGE](
                [ITEMID] [nvarchar](20) NOT NULL,
                [IMAGE] [varbinary](max) NULL,
                [DATAAREAID] [nvarchar](4) NOT NULL,
CONSTRAINT [PK_RBOINVENTITEMIMAGE] PRIMARY KEY CLUSTERED 
(
                [ITEMID] ASC,
                [DATAAREAID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO


