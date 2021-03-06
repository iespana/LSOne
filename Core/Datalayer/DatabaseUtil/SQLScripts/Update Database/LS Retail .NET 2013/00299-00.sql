USE LSPOSNET
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT

GO
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('CUSTOMERLEDGERENTRIES'))
BEGIN
	BEGIN TRANSACTION
	CREATE TABLE CUSTOMERLEDGERENTRIES
		(
		ENTRYNO int NOT NULL IDENTITY (1, 1),
		DATAAREAID nvarchar(4) NOT NULL,
		POSTINGDATE datetime NULL,
		CUSTOMER nvarchar(20) NULL,
		TYPE int NULL,
		DOCUMENTNO nvarchar(20) NULL,
		DESCRIPTION nvarchar(60) NULL,
		REASONCODE nvarchar(20) NULL,
		CURRENCY nvarchar(3) NULL,
		CURRENCYAMOUNT numeric(28, 12) NULL,
		AMOUNT numeric(28, 12) NULL,
		REMAININGAMOUNT numeric(28, 12) NULL,
		STOREID nvarchar(20) NULL,
		TERMINALID nvarchar(20) NULL,
		TRANSACTIONID nvarchar(20) NULL,
		RECEIPTID nvarchar(20) NULL,
		STATUS int NULL,
		USERID uniqueidentifier NULL
		)  ON [PRIMARY]
	ALTER TABLE CUSTOMERLEDGERENTRIES ADD CONSTRAINT
		PK_Table_1_1 PRIMARY KEY CLUSTERED 
		(
		ENTRYNO,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	ALTER TABLE CUSTOMERLEDGERENTRIES SET (LOCK_ESCALATION = TABLE)
	COMMIT
END

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'CUSTOMERLEDGERDATASOURCE')
	ALTER TABLE RBOPARAMETERS ADD CUSTOMERLEDGERDATASOURCE int NULL
GO
