/*

	Incident No.	: xxx
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 09.09.2012

	Description		: Add table KMLOG
	
	
	Tables affected	: KMLOG
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMLOG'))
BEGIN
	CREATE TABLE [dbo].KMLOG(
	[ID] uniqueidentifier NOT NULL default newID(),
	[DEVICETYPE] int NOT NULL,
	[DEVICEID] nvarchar(20) NOT NULL,
	[ACTION] int NOT NULL,
	[ORDERID] nvarchar(20) NOT NULL,
	[TIME] datetime NOT NULL,
	[DATAAREAID] varchar(4) NOT NULL
	)

	Alter table KMLOG Add CONSTRAINT  PK_KMLOG Primary Key (ID,DATAAREAID)
END

GO
