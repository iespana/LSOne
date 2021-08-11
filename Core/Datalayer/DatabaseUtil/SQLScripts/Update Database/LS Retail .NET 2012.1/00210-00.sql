/*

	Incident No.	:
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 17.07.2012

	Description		: Add Kitchen manager profile table
	
	
	Tables affected	: KMTRANSACTIONPROFILE
						
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KMTRANSACTIONPROFILE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[KMTRANSACTIONPROFILE](
		ID uniqueidentifier NOT NULL,
		NAME nvarchar(60) NOT NULL,
		KITCHENMANAGERSERVER nvarchar(20) NOT NULL,
		KITCHENMANAGERPORT nvarchar(10) NOT NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL
		)

	Alter table KMTRANSACTIONPROFILE Add CONSTRAINT  PK_KMTRANSACTIONPROFILE Primary Key (ID, DATAAREAID)
END
GO