
/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2012 - Sprint Baldur
	Date created	: 28.06.2011

	Description		: Tables for Gift cards

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOGIFTCARDTABLE, RBOGIFTCARDTRANSACTIONS
						
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOGIFTCARDTABLE'))
BEGIN
CREATE TABLE dbo.RBOGIFTCARDTABLE
	(
	GIFTCARDID nvarchar(20) NOT NULL,
	BALANCE numeric(28, 12) NULL,
	CURRENCY nvarchar(20) NOT NULL,
	ACTIVE tinyint NOT NULL,
	DATAAREAID nvarchar(4) NOT NULL
	)  ON [PRIMARY]

	ALTER TABLE dbo.RBOGIFTCARDTABLE ADD CONSTRAINT
		PK_RBOGIFTCARDTABLE PRIMARY KEY CLUSTERED 
		(
		GIFTCARDID,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE INDEX IX_RBOGIFTCARDTABLE 
	ON dbo.RBOGIFTCARDTABLE (BALANCE)
end

GO

exec spDB_SetTableDescription_1_0 'RBOGIFTCARDTABLE','Table that stores header informaton on gift cards'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTABLE','GIFTCARDID','The ID of the gift card'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTABLE','BALANCE','Current balance of the gift card'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTABLE','CURRENCY','Identifies the currency that the gift card is published in'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTABLE','ACTIVE','If the gift card has been purchased then it is active'


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOGIFTCARDTRANSACTIONS'))
BEGIN
CREATE TABLE dbo.RBOGIFTCARDTRANSACTIONS
	(
	GIFTCARDID nvarchar(20) NOT NULL,
	GIFTCARDLINEID uniqueidentifier NOT NULL,
	STOREID nvarchar(20) NULL,
	TERMINALID nvarchar(20) NULL,
	TRANSACTIONNUMBER nvarchar(20) NULL,
	RECEIPTID nvarchar(20) NULL,
	STAFFID nvarchar(20) NULL,
	USERID uniqueidentifier NULL,
	TRANSACTIONDATE date NULL,
	TRANSACTIONTIME time(7) NULL,
	AMOUNT numeric(28, 12) NULL,
	OPERATION int NOT NULL,
	DATAAREAID nvarchar(40) NOT NULL
	)  ON [PRIMARY]


ALTER TABLE dbo.RBOGIFTCARDTRANSACTIONS ADD CONSTRAINT
	PK_Table_1 PRIMARY KEY CLUSTERED 
	(
	GIFTCARDID,
	GIFTCARDLINEID,
	DATAAREAID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


END

GO

exec spDB_SetTableDescription_1_0 'RBOGIFTCARDTRANSACTIONS','Table that stores transaction lines for gift cards'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','GIFTCARDID','The ID of the gift card'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','GIFTCARDLINEID','UUID that identifies the line'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','STOREID','ID that identifies the store that triggered the creation of the line'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','TERMINALID','ID that identifies the terminal that triggered the creation of the line'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','TRANSACTIONNUMBER','ID that identifies the transaction that triggered the creation of the line (if any)'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','RECEIPTID','ID that identifies the receipt on the transaction that triggered the creation of the line (if any)'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','STAFFID','ID that identifies id of the staff that created the line if any'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','USERID','UUID that identifies id of the Store Controller user that created the line if any'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','TRANSACTIONDATE','The date when the line was created'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','TRANSACTIONTIME','The time when the line was created'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','AMOUNT','The amount of the line record'
exec spDB_SetFieldDescription_1_0 'RBOGIFTCARDTRANSACTIONS','OPERATION','Type of line, 0 = create, 1 = activate, 2 = add to Gift card, 3 = take from gift card, 4 = deactivate'

GO




