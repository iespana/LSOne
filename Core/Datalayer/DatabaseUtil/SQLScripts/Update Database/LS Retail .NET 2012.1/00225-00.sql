/*

	Incident No.	: 18003
	Responsible		: Olga Kristjánsdóttir
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 10.08.2012

	Description		: Add RETAILGROUPREPLENISHMENT
	
	
	Tables affected	:	RETAILGROUPREPLENISHMENT
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RETAILGROUPREPLENISHMENT]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RETAILGROUPREPLENISHMENT](
	[ID] [uniqueidentifier] NOT NULL,
	[RETAILGROUPID] [varchar](20) NULL,
	[STOREID] [varchar](20) NULL,
	[REORDERPOINT] [INT] NULL,
	[REORDERQUANTITY] [INT] NULL,
	[DATAAREAID] [varchar](4) NOT NULL
	)

	Alter table [RETAILGROUPREPLENISHMENT] Add CONSTRAINT  PK_RETAILGROUPREPLENISHMENT Primary Key (ID,DATAAREAID)
	END

GO
