/*

	Incident No.	: 17879
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 20.07.2012

	Description		: Add KMSETTINGS table
	
	
	Tables affected	: None
						
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KMSETTINGS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[KMSETTINGS](
		CURRENTORDERNUMBER int NULL,
		MAXORDERNUMBER int NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL 
		)

	Alter table KMSETTINGS Add CONSTRAINT  PK_KMSETTINGS Primary Key (DATAAREAID)
END
GO