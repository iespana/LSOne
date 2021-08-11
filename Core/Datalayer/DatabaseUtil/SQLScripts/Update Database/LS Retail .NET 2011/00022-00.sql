
/*

	Incident No.	: 7138
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 03\SC Team
	Date created	: 07.12.2010

	Description		: Adding RESTAURANTMENUTYPE table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	RestaurantMenuType	- table created
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RESTAURANTMENUTYPE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RESTAURANTMENUTYPE](
		[RESTAURANTID] [nvarchar](20) NOT NULL,
		[MENUORDER] [int] NOT NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[CODEONPOS] [nvarchar](1) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_RESTAURANTMENUTYPE] PRIMARY KEY CLUSTERED 
	(
		[RESTAURANTID] ASC,
		[MENUORDER] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


