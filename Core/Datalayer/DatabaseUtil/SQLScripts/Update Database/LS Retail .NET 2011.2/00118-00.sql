
/*

	Incident No.	: 12062	
	Responsible		: Óskar Bjarnason	
	Sprint			: LS Retail .NET 2012/Freyr
	Date created	: 04.10.2011

	Description		: Add TransactionID, Linenum, Description, ALLOWEOD, Active 

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISSUSPENDEDTRANSACTIONS	-	Add TRANSACTIONID
														add DESCRIPTION
														Add ALLOWEOD
														add ACTIVE
														Add SUSPENSIONTYPEID
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME='TRANSACTIONID')
Begin
	Alter table POSISSUSPENDEDTRANSACTIONS 
	Add TRANSACTIONID nvarchar(40) NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME='DESCRIPTION')
Begin
	Alter table POSISSUSPENDEDTRANSACTIONS 
	Add DESCRIPTION nvarchar(60) NULL 
END
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME='ALLOWEOD')
Begin
	Alter table POSISSUSPENDEDTRANSACTIONS 
	Add ALLOWEOD int NULL
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME='ACTIVE')
Begin
	Alter table POSISSUSPENDEDTRANSACTIONS 
	Add ACTIVE Tinyint Null
	
	CREATE INDEX IX_POSISSUSPENDEDTRANSACTIONSACTIVE
	ON dbo.POSISSUSPENDEDTRANSACTIONS (ACTIVE)
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME='SUSPENSIONTYPEID')
Begin
	Alter table POSISSUSPENDEDTRANSACTIONS 
	Add SUSPENSIONTYPEID nvarchar(40) Null

	CREATE INDEX IX_POSISSUSPENDEDTRANSACTIONSSUSPENSIONTYPE
	ON dbo.POSISSUSPENDEDTRANSACTIONS (SUSPENSIONTYPEID)
END
GO

--exec spDB_SetTableDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','Table that stores header informaton on suspended transactions'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','ID','The ID of the suspended transaction'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','Transactiondata','Data of the transactions '
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','Staff','Staffmember who sent the transaction '
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','NETAMOUNT','Netamount of the transaction '
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','StoreID','Id of the store '
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','TERMINALID','Id of the terminal'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','DATAAREAID','Id of the Dataarea'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','TRANSACTIONXML','XML data of the transaction'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','RECALLEDBY','Who recalled the transaction'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','TRANSACTIONID','ID of the TRANSACTION'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','DESCRIPTION','Description of the suspended transaction'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','ALLOWEOD','Flag that specifies if its allowed to do end of day operation when there are suspended transactions ( 0 = Store Default, 1 = Terminal Default, 2=Yes, 3=No) '
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','ACTIVE','Flag that specifies if the transaction is active( 0  = No, 1 = yes'
--exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','SUSPENSIONTYPEID','ID of the SuspensionType'











