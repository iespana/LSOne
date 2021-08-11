
/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2012 - Sprint Frigg
	Date created	: 29.08.2011

	Description		: Tables for Credit voucers

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOCREDITVOUCHERTABLE, RBOCREDITVOUCHERTRANSACTIONS
						
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOCREDITVOUCHERTABLE'))
BEGIN
CREATE TABLE dbo.RBOCREDITVOUCHERTABLE
	(
	VOUCHERID nvarchar(20) NOT NULL,
	BALANCE numeric(28, 12) NULL,
	CURRENCY nvarchar(20) NOT NULL,
	DATAAREAID nvarchar(4) NOT NULL
	)  ON [PRIMARY]

	ALTER TABLE dbo.RBOCREDITVOUCHERTABLE ADD CONSTRAINT
		PK_RBOCREDITVOUCHERTABLE PRIMARY KEY CLUSTERED 
		(
		VOUCHERID,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE INDEX IX_RBOCREDITVOUCHERTABLE 
	ON dbo.RBOCREDITVOUCHERTABLE (BALANCE)
end

GO

exec spDB_SetTableDescription_1_0 'RBOCREDITVOUCHERTABLE','Table that stores header informaton on credit vouchers'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTABLE','VOUCHERID','The ID of the credit voucher'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTABLE','BALANCE','Current balance of the credit voucher'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTABLE','CURRENCY','Identifies the currency that the credit voucher is published in'


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOCREDITVOUCHERTRANSACTIONS'))
BEGIN
CREATE TABLE dbo.RBOCREDITVOUCHERTRANSACTIONS
	(
	VOUCHERID nvarchar(20) NOT NULL,
	VOUCHERLINEID uniqueidentifier NOT NULL,
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
	DATAAREAID nvarchar(4) NOT NULL
	)  ON [PRIMARY]


ALTER TABLE dbo.RBOCREDITVOUCHERTRANSACTIONS ADD CONSTRAINT
	PK_RBOCREDITVOUCHERTRANSACTIONS PRIMARY KEY CLUSTERED 
	(
	VOUCHERID,
	VOUCHERLINEID,
	DATAAREAID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


END

GO

exec spDB_SetTableDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','Table that stores transaction lines for credit vouchers'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','VOUCHERID','The ID of the credit voucher card'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','VOUCHERLINEID','UUID that identifies the line'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','STOREID','ID that identifies the store that triggered the creation of the line'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','TERMINALID','ID that identifies the terminal that triggered the creation of the line'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','TRANSACTIONNUMBER','ID that identifies the transaction that triggered the creation of the line (if any)'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','RECEIPTID','ID that identifies the receipt on the transaction that triggered the creation of the line (if any)'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','STAFFID','ID that identifies id of the staff that created the line if any'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','USERID','UUID that identifies id of the Store Controller user that created the line if any'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','TRANSACTIONDATE','The date when the line was created'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','TRANSACTIONTIME','The time when the line was created'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','AMOUNT','The amount of the line record'
exec spDB_SetFieldDescription_1_0 'RBOCREDITVOUCHERTRANSACTIONS','OPERATION','Type of line, 0 = create, 2 = add to credit voucher, 3 = take from credit voucher'

GO




