
/*

	Incident No.	: 
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: 2014
	Date created	: 07.01.2014

	Description		: Created table for discount calculations
	
						
*/
USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DISCOUNTPARAMETERS')
BEGIN
	CREATE TABLE [dbo].[DISCOUNTPARAMETERS](
		[PERIODICLINE] [int] DEFAULT 3,
		[PERIODICTOTAL] [int] DEFAULT 4,	
		[LINETOTAL] [int] DEFAULT 4,	
		[KEY_] [int] NOT NULL,	
		[DATAAREAID] [nvarchar](4) NOT NULL,	
		CONSTRAINT [PK_DISCOUNTPARAMETERS] PRIMARY KEY CLUSTERED 
		(
			[DATAAREAID] ASC,
			[KEY_] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]
END
GO