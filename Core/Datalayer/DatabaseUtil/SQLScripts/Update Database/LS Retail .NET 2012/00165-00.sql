
/*

	Incident No.	: 15498
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 02.03.2012

	Description		: Adding data to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATIONS - updated data in the LOOKUPTYPE field
						
*/

USE LSPOSNET

GO


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISOPERATIONS' AND COLUMN_NAME='LOOKUPTYPE')
Begin
    -- Retail items
	update POSISOPERATIONS set LOOKUPTYPE = 1 where OPERATIONID = 100 -- Item Sale 

	-- StorePaymentTypes	
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 206 -- Pay Cash Quick
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 213 -- Pay Credit Memo
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 214 -- Pay Gift Card
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 203 -- Pay Currency
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 202 -- Pay Customer Account
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 208 -- Pay Corporate Card
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 204 -- Pay Check
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 511 -- Issue Credit Memo
	update POSISOPERATIONS set LOOKUPTYPE = 2 where OPERATIONID = 512 -- Issue Credit Gift Certificate

	-- PosMenu
	update POSISOPERATIONS set LOOKUPTYPE = 3 where OPERATIONID = 400 -- Popup Menu
	update POSISOPERATIONS set LOOKUPTYPE = 3 where OPERATIONID = 401 -- Sub menu

	-- InfocodeTaxGroup
	update POSISOPERATIONS set LOOKUPTYPE = 4 where OPERATIONID = 1405 -- Infocode Tax Group Change

	-- PosMenuAndButtonGrid
	update POSISOPERATIONS set LOOKUPTYPE = 5 where OPERATIONID = 1500 -- Open menu

	-- SuspendedTransactionTypes
	update POSISOPERATIONS set LOOKUPTYPE = 6 where OPERATIONID = 504 -- Recall transaction
	update POSISOPERATIONS set LOOKUPTYPE = 6 where OPERATIONID = 503 -- SSuspend transaction

	-- BlankOperations
	update POSISOPERATIONS set LOOKUPTYPE = 7 where OPERATIONID = 915 -- Blank operation

	-- IncomeAccounts
	update POSISOPERATIONS set LOOKUPTYPE = 8 where OPERATIONID = 517 -- Income accounts

	-- ExpenseAccounts
	update POSISOPERATIONS set LOOKUPTYPE = 9 where OPERATIONID = 518 -- Expense accounts

	-- TextInput
	update POSISOPERATIONS set LOOKUPTYPE = 10 where OPERATIONID = 103 -- Item Comment

	-- NumericInput	
	update POSISOPERATIONS set LOOKUPTYPE = 11 where OPERATIONID = 301 -- Line Discount Percent	
	update POSISOPERATIONS set LOOKUPTYPE = 11 where OPERATIONID = 303 -- Total Discount Percent

	-- StorePaymentTypeAndAmount
	update POSISOPERATIONS set LOOKUPTYPE = 12 where OPERATIONID = 200 -- Pay Cash

	-- Amount
	update POSISOPERATIONS set LOOKUPTYPE = 13 where OPERATIONID = 300 -- Line Discount Amount
	update POSISOPERATIONS set LOOKUPTYPE = 13 where OPERATIONID = 302 -- Total Discount Amount
END
GO