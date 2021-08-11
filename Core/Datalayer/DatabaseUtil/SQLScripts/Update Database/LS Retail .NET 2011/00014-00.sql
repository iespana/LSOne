/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 
	Date created	: 17.11.2010
	
	Description		:	Adding table REASONS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:  REASONS
						
*/

--IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[REASONS]') AND TYPE IN ('U'))
--BEGIN
--	CREATE TABLE dbo.REASONS
--		(
--		[REASONID] nvarchar(20) NOT NULL,
--		[REASONTEXT] nvarchar(60) NOT NULL,
--		DATAAREAID nvarchar(4) NOT NULL
--		)  ON [PRIMARY]

--	ALTER TABLE [dbo].REASONS ADD  CONSTRAINT [DF__REASONS__DATAAREAID]  DEFAULT ('LSR') FOR [DATAAREAID]


--	ALTER TABLE dbo.REASONS ADD CONSTRAINT PK_REASONS
--	PRIMARY KEY CLUSTERED ([REASONID],DATAAREAID) ON [PRIMARY]

--END
--GO

