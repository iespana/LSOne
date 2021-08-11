﻿/*

	Incident No.	: 23976
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2013.1\S1
	Date created	: 30.05.2013

	Description		: Primary key changed
	
	Tables affected	: WIZARDTEMPLATETAX
						
*/
USE LSPOSNET

GO
IF EXISTS (select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = N'PK_WIZARDTEMPLATETAX')
BEGIN
	ALTER TABLE WIZARDTEMPLATETAX DROP CONSTRAINT [PK_WIZARDTEMPLATETAX]
	ALTER TABLE WIZARDTEMPLATETAX ALTER COLUMN TAXGROUPTYPE INT NOT NULL
	ALTER TABLE WIZARDTEMPLATETAX ADD CONSTRAINT [PK_WIZARDTEMPLATETAX_NEW] PRIMARY KEY CLUSTERED(
			[ID] ASC,
			[DATAAREAID] ASC,
			[TAXGROUP] ASC,
			[TAXGROUPTYPE] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 
END
GO