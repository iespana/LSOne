/*

	Incident No.	: ONE-8299
	Responsible		: Adrian Chiorean
	Sprint			: Barot
	Date created	: 16.01.2018

	Description		: Alter spCUSTOMER_RecreateCustomerLedger
					  Change hardcoded condition (P.TENDERTYPE <> 4) to check if the tender type (which can have any generated ID) has a default function different than Customer.

					  ...
					  FROM RBOTRANSACTIONPAYMENTTRANS P	
					  LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
					  WHERE
					  ...
					  AND te.DEFAULTFUNCTION <> 3
*/

USE LSPOSNET
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCUSTOMER_RecreateCustomerLedger]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[spCUSTOMER_RecreateCustomerLedger]
GO

/****** Object:  StoredProcedure [dbo].[spCUSTOMER_RecreateCustomerLedger]    Script Date: 16/01/2018 3:21:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCUSTOMER_RecreateCustomerLedger]
@customerID as varchar(20), @statementID as varchar(20), @DataAreaID as varchar(4), @UserID as uniqueidentifier

AS
BEGIN

	--print 'Begin Customer Ledger Entry creation'

	-- PARAMETERS testing values
	--SET @DATAAREAID = 'LSR';
	--SET @customerID = '';
	--SET @statementID = '';

	--Stored proc variables
	DECLARE @transType int
	DECLARE @transactionID nvarchar(20)
	DECLARE @receiptID nvarchar(20)
	DECLARE @storeID nvarchar(20)
	DECLARE @terminalID nvarchar(20)
	DECLARE @amountToAccount numeric(28,12)
	DECLARE @customerAccount nvarchar(30)
	DECLARE @saleIsReturn tinyint
	DECLARE @TransDate datetime

	DECLARE @AMOUNTTENDERED numeric(28,12)
	DECLARE @TENDERCOUNT int
	DECLARE @WholeDiscountAmount numeric(28,12)
	DECLARE @err as int
	DECLARE @LocalCurrency as nvarchar(3)

	SET @LocalCurrency = (SELECT CURRENCYCODE FROM COMPANYINFO WHERE KEY_=0 AND DATAAREAID = @DataAreaID)

	BEGIN TRAN
	
	IF @statementID = ''
	BEGIN
		IF @customerID = ''
		BEGIN
			-- Recreate whole CusotmerLedgerEntries table
			-- TODO check
			DELETE FROM CUSTOMERLEDGERENTRIES
			WHERE TERMINALID <> ''
			
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT <> ''
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
		END
		ELSE
		BEGIN
			-- Recreate one customer's customer ledger entries
			-- TODO check
			DELETE FROM CUSTOMERLEDGERENTRIES 
			WHERE CUSTOMER = @customerID
			AND TERMINALID <> ''
			
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT = @customerID
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
		END
	END
	ELSE
	BEGIN
		IF @customerID = ''
		BEGIN
			-- Create customer ledger entries by statement ID
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT <> ''
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
			AND STATEMENTID = @statementID
			AND [TYPE] <> 3
		END
		ELSE
		BEGIN
			-- Create customer ledger entries by statement ID for one customer only
			DECLARE TRANSACTIONLIST CURSOR FOR
			SELECT [TYPE], TRANSACTIONID, RECEIPTID, STORE, TERMINAL, AMOUNTTOACCOUNT, CUSTACCOUNT, SALEISRETURNSALE, TRANSDATE
			FROM RBOTRANSACTIONTABLE T
			WHERE DATAAREAID = @DataAreaID
			AND CUSTACCOUNT = @customerID
			AND ENTRYSTATUS = 0  -- we don't want voided transactions
			AND STATEMENTID = @statementID
			AND [TYPE] <> 3
		END
	END

	OPEN TRANSACTIONLIST
	FETCH FROM TRANSACTIONLIST INTO @transType, @transactionID, @receiptID, @storeID, @terminalID, @amountToAccount, @customerAccount, @saleIsReturn, @transDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @transType = 2 
		BEGIN
			-- Get the number of valid payments on the transactions
			SELECT @TENDERCOUNT = COALESCE(COUNT(tt.TRANSACTIONID), 0)
			FROM RBOTRANSACTIONPAYMENTTRANS tt
			LEFT JOIN RBOTENDERTYPETABLE te ON tt.TENDERTYPE = te.TENDERTYPEID
			WHERE tt.TRANSACTIONSTATUS = 0 -- we don't want voided payments
			AND tt.TRANSACTIONID = @transactionID
			AND tt.STORE = @storeID
			AND tt.TERMINAL = @terminalID	
			AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')

			-- Check if there are any customer account payments
			SELECT @AMOUNTTENDERED = COALESCE(SUM(AMOUNTMST), 0)
			FROM RBOTRANSACTIONPAYMENTTRANS P	
			JOIN RBOSTORETENDERTYPETABLE T ON 
				P.TENDERTYPE = T.TENDERTYPEID 
				AND T.POSOPERATION = 202 -- is customer account tender
				AND T.STOREID = P.STORE 
				AND T.DATAAREAID = P.DATAAREAID
			LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
			WHERE P.DATAAREAID =  @DataAreaID
			AND P.TRANSACTIONSTATUS = 0 -- we don't want voided payments
			AND P.TRANSACTIONID = @transactionID
			AND P.STORE = @storeID
			AND P.TERMINAL = @terminalID
			AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')			
			
			IF @AMOUNTTENDERED <> 0
			BEGIN
				SET @TENDERCOUNT = @TENDERCOUNT - 1 
				IF @AMOUNTTENDERED < 0 SET @saleIsReturn = 1
				IF @saleIsReturn = 1
				BEGIN
					PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = CreditMemo, Status = Open, Amount = '+ CAST(-@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
					
					INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
					 VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,2						--Credit Memo
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,1						--Open
						   ,@UserID)
					
					SET @err = @@ERROR
					IF @err != 0 GOTO HANDLE_ERROR
				END
				ELSE
				BEGIN
					PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Invoice, Status = Open, Amount = '+ CAST(-@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
					INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
					 VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,1						--Invoice
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,1						--Open
						   ,@UserID)
						   
					SET @err = @@ERROR
					IF @err != 0 GOTO HANDLE_ERROR
				END
			END
			
			IF @TENDERCOUNT > 0
			BEGIN
				-- Get the sum of non-customer account payments to save to ledger		
				SELECT @AMOUNTTENDERED = COALESCE(SUM(AMOUNTMST), 0)
				FROM RBOTRANSACTIONPAYMENTTRANS P	
				JOIN RBOSTORETENDERTYPETABLE T ON 
					P.TENDERTYPE = T.TENDERTYPEID 
					AND T.POSOPERATION <> 202 -- is NOT customer account tender
					AND T.STOREID = P.STORE 
					AND T.DATAAREAID = P.DATAAREAID
				LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
				WHERE P.DATAAREAID =  @DataAreaID
				AND P.TRANSACTIONSTATUS = 0 -- we don't want voided payments
				AND P.TRANSACTIONID = @transactionID
				AND P.STORE = @storeID
				AND P.TERMINAL = @terminalID	
				AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')
				
				-- There are either other payments on the transaction or it wasn't paid with a customer account
				IF @AMOUNTTENDERED <> 0
				BEGIN
					IF @AMOUNTTENDERED < 0 SET @saleIsReturn = 1
					PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Sale, Status = Closed, Amount = '+ CAST(-@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
					INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
					 VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,3						--Sale
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,-@AMOUNTTENDERED
						   ,-@AMOUNTTENDERED
						   ,0
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,0						--Closed
						   ,@UserID)
						   
					SET @err = @@ERROR
					IF @err != 0 GOTO HANDLE_ERROR
				END
			END
			
			--IF @saleIsReturn = 0
			BEGIN
				SET @WholeDiscountAmount = (SELECT SUM(ISNULL(WHOLEDISCAMOUNTWITHTAX,0)) 
											FROM RBOTRANSACTIONSALESTRANS 
											WHERE DATAAREAID = @DataAreaID 
											AND TRANSACTIONSTATUS = 0 -- we don't want voided payments
											AND TRANSACTIONID = @transactionID
											AND STORE = @storeID
											AND TERMINALID = @terminalID)
				IF @WholeDiscountAmount <> 0 
				BEGIN
					IF @saleIsReturn = 0
					BEGIN
						PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Discount, Status = Closed, Amount = '+ CAST(ABS(@WholeDiscountAmount) as varchar)+' is return: '+cast(@saleIsReturn as varchar)
						INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
						VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,4						--Discount
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,@WholeDiscountAmount
						   ,@WholeDiscountAmount
						   ,0
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,0						--Closed
						   ,@UserID)
						SET @err = @@ERROR
						IF @err != 0 GOTO HANDLE_ERROR
					END
					ELSE
					BEGIN
						PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Discount, Status = Closed, Amount = '+ CAST(-ABS(@WholeDiscountAmount) as varchar)+' is return: '+cast(@saleIsReturn as varchar)
						INSERT INTO [CUSTOMERLEDGERENTRIES]
						   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
						VALUES
						   (@DataAreaID
						   ,@TransDate
						   ,@customerAccount
						   ,4						--Discount
						   ,@receiptID
						   ,''
						   ,''
						   ,@LocalCurrency
						   ,@WholeDiscountAmount
						   ,@WholeDiscountAmount
						   ,0
						   ,@storeID
						   ,@terminalID
						   ,@transactionID
						   ,@receiptID
						   ,0						--Closed
						   ,@UserID)
						SET @err = @@ERROR
						IF @err != 0 GOTO HANDLE_ERROR
					END						
					
				END
			END
		END
		
		ELSE
		IF @transType = 3
		BEGIN 
			-- Get the sum of non-customer account payments to save to ledger		
			SELECT @AMOUNTTENDERED = COALESCE(SUM(AMOUNTMST), 0)
			FROM RBOTRANSACTIONPAYMENTTRANS P	
			JOIN RBOSTORETENDERTYPETABLE T ON 
				P.TENDERTYPE = T.TENDERTYPEID 
				AND T.POSOPERATION <> 202 -- is NOT customer account tender
				AND T.STOREID = P.STORE 
				AND T.DATAAREAID = P.DATAAREAID
			LEFT JOIN RBOTENDERTYPETABLE te ON P.TENDERTYPE = te.TENDERTYPEID
			WHERE P.DATAAREAID =  @DataAreaID
			AND P.TRANSACTIONSTATUS = 0 -- we don't want voided payments
			AND P.TRANSACTIONID = @transactionID
			AND P.STORE = @storeID
			AND P.TERMINAL = @terminalID
			AND (te.DEFAULTFUNCTION <> 3 OR @statementID = '')			
			
			-- There are either other payments on the transaction or it wasn't paid with a customer account
			IF @AMOUNTTENDERED <> 0
			BEGIN
				PRINT @customerAccount + ' - ' + @transactionID + ': Customer Ledger Entry created - Type = Payment, Status = Open, Amount = '+ CAST(@AMOUNTTENDERED as varchar)+' is return: '+cast(@saleIsReturn as varchar)
				INSERT INTO [CUSTOMERLEDGERENTRIES]
				   ([DATAAREAID],[POSTINGDATE],[CUSTOMER],[TYPE],[DOCUMENTNO],[DESCRIPTION],[REASONCODE],[CURRENCY],[CURRENCYAMOUNT],[AMOUNT],[REMAININGAMOUNT],[STOREID],[TERMINALID],[TRANSACTIONID],[RECEIPTID],[STATUS],[USERID])
				VALUES
				   (@DataAreaID
				   ,@TransDate
				   ,@customerAccount
				   ,0						--Payment
				   ,@receiptID
				   ,''
				   ,''
				   ,@LocalCurrency
				   ,@AMOUNTTENDERED
				   ,@AMOUNTTENDERED
				   ,@AMOUNTTENDERED
				   ,@storeID
				   ,@terminalID
				   ,@transactionID
				   ,@receiptID
				   ,1						--Open
				   ,@UserID)
				SET @err = @@ERROR
				IF @err != 0 GOTO HANDLE_ERROR
			END
		END 
	    
	    
		FETCH NEXT FROM TRANSACTIONLIST INTO @transType, @transactionID, @receiptID, @storeID, @terminalID, @amountToAccount, @customerAccount, @saleIsReturn, @TransDate

	END

	COMMIT TRAN
	--print '------------------------ DONE';

	close TRANSACTIONLIST
	deallocate TRANSACTIONLIST
	RETURN 0;

	HANDLE_ERROR:
		BEGIN
			ROLLBACK TRAN;
			CLOSE Expired_cursor;
			DEALLOCATE Expired_cursor;
			RETURN 3;				--Insert error
		END
END

GO