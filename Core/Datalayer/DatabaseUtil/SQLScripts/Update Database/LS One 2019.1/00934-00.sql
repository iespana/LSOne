/*
	Incident No.	: ONE-7633 Store transfers/purchase orders - create documents from templates in mobile inventory
					  ONE-7668 Remove inv. app operations from the list of available operations
	Responsible		: Adrian Chiorean
	Sprint			: Sun
	Date created	: 05.07.2019

	Description		: Remove operations OmniPriceCheck, OmniUpdatePrice
					  Rename OmniStockCounting to OmniStockCountingTemplate
					  Add operations OmniWhoAmI, OmniUnsentWorksheets, OmniUnsentDocument, OmniPurchaseOrderTemplate, OmniTransferTemplate
*/

USE LSPOSNET

IF  EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2107') -- Update stock counting description
BEGIN
	UPDATE OPERATIONS SET [DESCRIPTION] = 'Stock counting template' WHERE ID = '2107'
END
GO

IF  EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2113') -- Rename PriceCheck
BEGIN
	UPDATE OPERATIONS SET [DESCRIPTION] = 'Unsent templates' WHERE ID = '2113'
END
GO

IF  EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2114') -- Rename UpdatePrice
BEGIN
	UPDATE OPERATIONS SET [DESCRIPTION] = 'Unsent documents' WHERE ID = '2114'
END
GO

IF NOT EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2117')
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, [DESCRIPTION], [TYPE], [LOOKUPTYPE], [AUDIT])
	VALUES (NEWID(), '2117', 'Who am I', 3, 0, NULL)
END
GO

IF NOT EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2118')
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, [DESCRIPTION], [TYPE], [LOOKUPTYPE], [AUDIT])
	VALUES (NEWID(), '2118', 'Purchase order template', 3, 0, NULL)
END
GO

IF NOT EXISTS(SELECT 1 FROM OPERATIONS WHERE ID = '2119')
BEGIN
	INSERT INTO OPERATIONS (MASTERID, ID, [DESCRIPTION], [TYPE], [LOOKUPTYPE], [AUDIT])
	VALUES (NEWID(), '2119', 'Store transfer template', 3, 0, NULL)
END
GO