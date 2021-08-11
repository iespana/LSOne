/*

	Incident No.	: 12062
	Responsible		: Óskar Bjarnason
	Sprint			: 2012 - Sprint Sif
	Date created	: 15.09.2011

	Description		: Tables for Suspended Transactions

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISSUSPENSIONTYPE, POSISSUSPENSIONADDINFO
						
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISSUSPENSIONTYPE'))
BEGIN
CREATE TABLE dbo.POSISSUSPENSIONTYPE
	(
	ID nvarchar(40) NOT NULL,
	DESCRIPTION nvarchar(60) NULL,
	ALLOWEOD int NOT NULL,
	DATAAREAID nvarchar(4) NOT NULL
	)  ON [PRIMARY]

	ALTER TABLE dbo.POSISSUSPENSIONTYPE ADD CONSTRAINT
		PK_POSISSUSPENSIONTYPE PRIMARY KEY CLUSTERED 
		(
		ID,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

end

GO

exec spDB_SetTableDescription_1_0 'POSISSUSPENSIONTYPE','Table that stores header informaton on suspended transactions'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONTYPE','ID','The ID of the suspended transaction'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONTYPE','ALLOWEOD','Flag that specifies if its allowed to do end of day operation when there are suspended transactions ( 0 = Store Default, 1 = Terminal Default, 2=Yes, 3=No) '

GO

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSISSUSPENSIONADDINFO'))
BEGIN
CREATE TABLE dbo.POSISSUSPENSIONADDINFO
	(
	ID nvarchar(40) NOT NULL,
	SUSPENSIONTYPEID nvarchar(40) NOT NULL,
	PROMPT nvarchar(60),
	FIELDORDER int,
	INFOTYPE int,
	INFOTYPESELECTION nvarchar(20) NOT NULL,
	REQUIRED tinyint,
	DATAAREAID nvarchar(4) NOT NULL	
	)  ON [PRIMARY]

	ALTER TABLE dbo.POSISSUSPENSIONADDINFO ADD CONSTRAINT
		PK_POSISSUSPENSIONADDINFO PRIMARY KEY CLUSTERED 
		(
		ID,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

end

GO
exec spDB_SetTableDescription_1_0 'POSISSUSPENSIONADDINFO','Table that stores additional  informaton on suspended transactions'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONADDINFO','SUSPENSIONTYPEID','The ID of the suspended transaction'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONADDINFO','PROMPT','The text used to prompt the user for the information '
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONADDINFO','FIELDORDER','The order in which the information should be asked for'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONADDINFO','INFOTYPE','Enum for information Types (Text, customer, name, address, infocode, date, other)'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONADDINFO','INFOTYPESELECTION','If the Infotype is infocode this field will have the name of the infocode to be displayed. Can be either a single infocode or an infocode group'
exec spDB_SetFieldDescription_1_0 'POSISSUSPENSIONADDINFO','REQUIRED','If true then the user has to enter information, if false then a cancel button wil be available to the user'

GO
