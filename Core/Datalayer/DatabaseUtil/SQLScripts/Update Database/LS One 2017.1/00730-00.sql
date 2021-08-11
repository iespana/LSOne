/*

	Incident No.	: ONE-5941
	Responsible		: Simona Avornicesei
	Sprint			: Bestå
	Date created	: 07.03.2017

	Description		: Change Logo width configuration on Store card-Form settings to be a selection
	
	
	Tables affected	: RBOSTORETABLE
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'RECEIPTLOGOWIDTH')
	EXECUTE spDB_SetFieldDescription_1_0 'RBOSTORETABLE', 'RECEIPTLOGOWIDTH', '[Obsolete, ONE-5941] Store logo width in pixels.';
	--EXECUTE sys.sp_addextendedproperty 'MS_Description','[Obsolete, ONE-5941] Store logo width in pixels.', 
	--									'schema', 'dbo', 
	--									'table', 'RBOSTORETABLE', 
	--									'column', 'RECEIPTLOGOWIDTH';
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'RECEIPTLOGOSIZE')
	ALTER TABLE RBOSTORETABLE
		ADD RECEIPTLOGOSIZE tinyint NOT NULL DEFAULT 1;

	-- add definitions
	EXECUTE spDB_SetTableDescription_1_0 'RBOSTORETABLE', 'Contains defined stores and their settings';
	EXECUTE spDB_SetFieldDescription_1_0 'RBOSTORETABLE', 'RECEIPTLOGOSIZE', 'WinPrinter printing mode for store logo. Can be 1 - Normal, 2 - Double'

	--EXECUTE sys.sp_addextendedproperty 'MS_Description','Contains defined stores and their settings', 
	--									'schema', 'dbo', 
	--									'table', 'RBOSTORETABLE';
	--EXECUTE sys.sp_addextendedproperty 'MS_Description','WinPrinter printing mode for store logo. Can be 1 - Normal, 2 - Double', 
	--									'schema', 'dbo', 
	--									'table', 'RBOSTORETABLE', 
	--									'column', 'RECEIPTLOGOSIZE';
GO