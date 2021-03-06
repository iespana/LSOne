
/*

	Incident No.	: 16056
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .Net 2012\Hel
	Date created	: 22.03.2012

	Description		: Update primary key field

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboTransactionEftInfoTrans - field type updated
						
*/


USE LSPOSNET
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='RBOTRANSACTIONEFTINFOTRANS' AND COLUMN_NAME='TRANSACTIONID' AND CHARACTER_MAXIMUM_LENGTH = 10)
BEGIN

	CREATE TABLE dbo.Tmp_RBOTRANSACTIONEFTINFOTRANS
		(
		TRANSACTIONID nvarchar(20) NOT NULL,
		LINENUM numeric(28, 12) NOT NULL,
		STORE nvarchar(10) NOT NULL,
		TERMINAL nvarchar(10) NOT NULL,
		TIMEOUT int NULL,
		MERCHANTNUMBER nvarchar(60) NULL,
		TERMINALNUMBER nvarchar(60) NULL,
		TRACK2 nvarchar(60) NULL,
		CARDNUMBER nvarchar(60) NULL,
		CARDNUMBERHIDDEN tinyint NULL,
		EXPDATE varchar(10) NULL,
		CARDENTRYTYPE tinyint NULL,
		CARDNAME nvarchar(60) NULL,
		AMOUNT numeric(28, 12) NULL,
		AMOUNTINCENTS int NULL,
		STAFFID nvarchar(60) NULL,
		TRANSACTIONNUMBER int NULL,
		AUTHORIZED tinyint NULL,
		AUTHCODE nvarchar(60) NULL,
		CARDTYPE int NULL,
		ACQUIRERNAME nvarchar(60) NULL,
		TRANSACTIONDATETIME datetime NULL,
		ISSUERNUMBER int NULL,
		BATCHNUMBER bigint NULL,
		RESPONSECODE nvarchar(60) NULL,
		AUTHSOURCECODE nvarchar(60) NULL,
		ENTRYSOURCECODE nvarchar(60) NULL,
		PROCESSLOCALLY tinyint NULL,
		AUTHSTRING nvarchar(60) NULL,
		VISAAUTHCODE nvarchar(60) NULL,
		EUROPAYAUTHCODE nvarchar(60) NULL,
		BATCHCODE nvarchar(60) NULL,
		SEQUENCECODE nvarchar(60) NULL,
		NOTAUTHORIZEDTEXT nvarchar(60) NULL,
		AUTHORIZEDTEXT nvarchar(60) NULL,
		TRANSACTIONTYPE int NULL,
		MESSAGE nvarchar(60) NULL,
		ISSUERNAME nvarchar(60) NULL,
		ERRORCODE nvarchar(60) NULL,
		REFERRALTELNUMBER nvarchar(60) NULL,
		TRACKSEPERATOR nvarchar(60) NULL,
		PREVIOUSSEQUENCECODE nvarchar(60) NULL,
		EXTERNALCARDRECEIPTS text NULL,
		DATAAREAID nvarchar(3) NOT NULL,
		RECID int NULL,
		TENDERTYPE nvarchar(20) NULL,
		REPLICATIONCOUNTER int NOT NULL IDENTITY (1, 1)
		)  ON [PRIMARY]
		 TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE dbo.Tmp_RBOTRANSACTIONEFTINFOTRANS SET (LOCK_ESCALATION = TABLE)

	SET IDENTITY_INSERT dbo.Tmp_RBOTRANSACTIONEFTINFOTRANS ON

	IF EXISTS(SELECT * FROM dbo.RBOTRANSACTIONEFTINFOTRANS)
		 EXEC('INSERT INTO dbo.Tmp_RBOTRANSACTIONEFTINFOTRANS (TRANSACTIONID, LINENUM, STORE, TERMINAL, TIMEOUT, MERCHANTNUMBER, TERMINALNUMBER, TRACK2, CARDNUMBER, CARDNUMBERHIDDEN, EXPDATE, CARDENTRYTYPE, CARDNAME, AMOUNT, AMOUNTINCENTS, STAFFID, TRANSACTIONNUMBER, AUTHORIZED, AUTHCODE, CARDTYPE, ACQUIRERNAME, TRANSACTIONDATETIME, ISSUERNUMBER, BATCHNUMBER, RESPONSECODE, AUTHSOURCECODE, ENTRYSOURCECODE, PROCESSLOCALLY, AUTHSTRING, VISAAUTHCODE, EUROPAYAUTHCODE, BATCHCODE, SEQUENCECODE, NOTAUTHORIZEDTEXT, AUTHORIZEDTEXT, TRANSACTIONTYPE, MESSAGE, ISSUERNAME, ERRORCODE, REFERRALTELNUMBER, TRACKSEPERATOR, PREVIOUSSEQUENCECODE, EXTERNALCARDRECEIPTS, DATAAREAID, RECID, TENDERTYPE, REPLICATIONCOUNTER)
			SELECT TRANSACTIONID, LINENUM, STORE, TERMINAL, TIMEOUT, MERCHANTNUMBER, TERMINALNUMBER, TRACK2, CARDNUMBER, CARDNUMBERHIDDEN, EXPDATE, CARDENTRYTYPE, CARDNAME, AMOUNT, AMOUNTINCENTS, STAFFID, TRANSACTIONNUMBER, AUTHORIZED, AUTHCODE, CARDTYPE, ACQUIRERNAME, TRANSACTIONDATETIME, ISSUERNUMBER, BATCHNUMBER, RESPONSECODE, AUTHSOURCECODE, ENTRYSOURCECODE, PROCESSLOCALLY, AUTHSTRING, VISAAUTHCODE, EUROPAYAUTHCODE, BATCHCODE, SEQUENCECODE, NOTAUTHORIZEDTEXT, AUTHORIZEDTEXT, TRANSACTIONTYPE, MESSAGE, ISSUERNAME, ERRORCODE, REFERRALTELNUMBER, TRACKSEPERATOR, PREVIOUSSEQUENCECODE, EXTERNALCARDRECEIPTS, DATAAREAID, RECID, TENDERTYPE, REPLICATIONCOUNTER FROM dbo.RBOTRANSACTIONEFTINFOTRANS WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT dbo.Tmp_RBOTRANSACTIONEFTINFOTRANS OFF

	DROP TABLE dbo.RBOTRANSACTIONEFTINFOTRANS

	EXECUTE sp_rename N'dbo.Tmp_RBOTRANSACTIONEFTINFOTRANS', N'RBOTRANSACTIONEFTINFOTRANS', 'OBJECT' 

	ALTER TABLE dbo.RBOTRANSACTIONEFTINFOTRANS ADD CONSTRAINT
		TRANSACTIONEFTINFOIDX PRIMARY KEY CLUSTERED 
		(
		TRANSACTIONID,
		LINENUM,
		STORE,
		TERMINAL,
		DATAAREAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


END
GO






