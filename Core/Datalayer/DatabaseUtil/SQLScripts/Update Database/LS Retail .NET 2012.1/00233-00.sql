/*

	Incident No.	: xxx
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 03.09.2012

	Description		: Add table KMTIMESTYLE
	
	
	Tables affected	: KMTIMESTYLE
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMTIMESTYLE'))
BEGIN
	CREATE TABLE [dbo].[KMTIMESTYLE](
	[ID] [uniqueidentifier] NOT NULL,
	[KDSID] [nvarchar](20) NOT NULL,
	[SECONDSPASSED] int NOT NULL,
	[STYLEID] [nvarchar](20) NOT NULL,
	[DATAAREAID] [varchar](4) NOT NULL
	)

	Alter table KMTIMESTYLE Add CONSTRAINT  PK_KMTIMESTYLE Primary Key (ID,DATAAREAID)
END

GO
