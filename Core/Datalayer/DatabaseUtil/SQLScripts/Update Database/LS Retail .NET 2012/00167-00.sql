
/*

	Incident No.	: 
	Responsible		: Björn Eiríksson
	Sprint			: LS Retail .NET 2012
	Date created	: 08.03.2012

	Description		: Altering row in table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: New tables for Reporting
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('REPORTS'))
BEGIN
	CREATE TABLE [dbo].[REPORTS](
		[REPORTGUID] uniqueidentifier NOT NULL,
		[NAME] [nvarchar](60) NOT NULL,
		[DESCRIPTION] [nvarchar](255) NOT NULL,
		[REPORTBLOB] [varbinary](max) NULL,
		[SQLBLOB] [varbinary](max) NULL,
		[SQLBLOBINSTALLED] bit NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)  ON [PRIMARY]
		
	alter table dbo.REPORTS add constraint
	PK_REPORTS primary key clustered (REPORTGUID,DATAAREAID) 
END

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('REPORTLOCALIZATION'))
BEGIN
	CREATE TABLE [dbo].[REPORTLOCALIZATION](
		[REPORTGUID] uniqueidentifier NOT NULL,
		[LANGUAGEID] nvarchar(10) NOT NULL,
		[NAME] [nvarchar](60) NOT NULL,
		[DESCRIPTION] [nvarchar](255) NOT NULL,
		[REPORTBLOB] [varbinary](max) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)  ON [PRIMARY]
		
	alter table dbo.REPORTLOCALIZATION add constraint
	PK_REPORTLOCALIZATION primary key clustered (REPORTGUID,LANGUAGEID,DATAAREAID) 
END

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('REPORTCONTEXTS'))
BEGIN
	CREATE TABLE [dbo].[REPORTCONTEXTS](
		[CONTEXTGUID] uniqueidentifier NOT NULL,
		[REPORTGUID] uniqueidentifier NOT NULL,
		[CONTEXT] nvarchar(20),
		[ACTIVE] bit NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)  ON [PRIMARY]
		
	alter table dbo.REPORTCONTEXTS add constraint
	PK_REPORTCONTEXTS primary key clustered (CONTEXTGUID,DATAAREAID) 

	CREATE INDEX IX_REPORTCONTEXTS_REPORTGUID
	ON dbo.REPORTCONTEXTS (REPORTGUID)

	CREATE INDEX IX_REPORTCONTEXTS_CONTEXT
	ON dbo.REPORTCONTEXTS (CONTEXT)
END

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('REPORTENUMS'))
BEGIN
	CREATE TABLE [dbo].[REPORTENUMS](
		[REPORTGUID] uniqueidentifier NOT NULL,
		[LANGUAGEID] nvarchar(10) NOT NULL,
		[ENUMNAME] nvarchar(40) NOT NULL,
		[ENUMVALUE] int NOT NULL,
		[ENUMTEXT] nvarchar(40) NOT NULL,
		[LABEL] nvarchar(40) NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)  ON [PRIMARY]
		
	alter table dbo.REPORTENUMS add constraint
	PK_REPORTENUMS primary key clustered (REPORTGUID,LANGUAGEID,ENUMNAME,ENUMVALUE,DATAAREAID) 
END

GO
