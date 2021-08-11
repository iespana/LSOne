
/*

	Incident No.	: 17099
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 15.06.2012

	Description		: Adding the RBOINVENTTRANSLATIONS table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: 	RBOINVENTTRANSLATIONS			- table added
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTTRANSLATIONS]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[RBOINVENTTRANSLATIONS](
		ID uniqueidentifier NOT NULL,
		ITEMID nvarchar(20) NOT NULL,
		CULTURENAME nvarchar(20) NOT NULL,
		DESCRIPTION nvarchar(250) NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)

	Alter table [RBOINVENTTRANSLATIONS] Add CONSTRAINT  PK_RBOINVENTTRANSLATIONS Primary Key (ID, DATAAREAID)
END
GO