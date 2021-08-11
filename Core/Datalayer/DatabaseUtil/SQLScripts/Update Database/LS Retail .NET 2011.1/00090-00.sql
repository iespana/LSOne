/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2011.1\Sprint 2
	Date created	: 25.05.2011

	Description		: Update purchase orders table information

	Logic scripts   : 
	
	Tables affected	: PURCHASEORDERS
					  PURCHASEORDERLINE
					  PURCHASEORDERMISCCHARGES
						
*/

USE LSPOSNET
GO

exec spDB_SetTableDescription_1_0 'PURCHASEORDERS','Table with informations regarding purchase orders. Both PURCHASEORDERLINE table and PURCHASEORDERMISCCHARGES table contain more information regarding the purchase order'

exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','PURCHASEORDERID','The ID of the purchase order'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','VENDORID','The ID of the vendor that the purchase order is linked to'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','CONTACTID','Thd ID of the contact that the purchase order is linked to'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','PURCHASESTATUS','The status of the purchase order'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','DELIVERYDATE','The delivery date of the purchase order'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','CURRENCYCODE','The currency code ID of the purchase order'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','CREATEDDATE','The purchase order´s date of creation'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','STOREID','The ID of the store that we are ordering to'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','ORDERER','The Guid of the user who made the purchase order'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','ORDERINGDATE','The ordering date of the purchase order'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERS','DATAAREAID','DATAAREAID'

exec spDB_SetTableDescription_1_0 'PURCHASEORDERLINE','Table with informations regarding purchase orders lines. A purchase order line contains information regarding the ordered items. The purchase order line is linked to a purchase order from the PURCHASEORDERS table'

exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','PURCHASEORDERID','The ID of the purchase order that this line references'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','LINENUMBER','The line number of the purchase order line'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','RETAILITEMID','The ID of the retail item that is referenced from table INVENTTABLE'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','VENDORITEMID','The ID of the retail item from the vendor´s perspective. Referenced from table VENDORITEMS'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','VARIANTID','The variant ID of the retail item. Referenced from table INVENTDIMCOMBINATION'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','UNITID','The unit ID of the retail item. Referenced from table UNIT'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','QUANTITY','The quantity of retail items that was ordered'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','PRICE','The unit price of the retail item that the RETAILITEMID defines'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','DISCOUNTAMOUNT','The discount amount per item'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','DISCOUNTPERCENTAGE','The discount percentage.'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','TAXAMOUNT','The tax amount per item'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERLINE','DATAAREAID','DATAAREAID'

exec spDB_SetTableDescription_1_0 'PURCHASEORDERMISCCHARGES','Table with informations about misc charges for a purchase order. The purchase order misc charge is linked to a purchase order from the PURCHASEORDERS table'

exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','PURCHASEORDERID','The ID of the purchase order that this misc charge references'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','LINENUMBER','The line number of the purchase order misc charge'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','TYPE','The type of the misc charge'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','REASON','The reason string for this misc charge'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','AMOUNT','The amount of the misc charge'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','TAXAMOUNT','The tax amount of the misc charge'
exec spDB_SetFieldDescription_1_0 'PURCHASEORDERMISCCHARGES','DATAAREAID','DATAAREAID'
