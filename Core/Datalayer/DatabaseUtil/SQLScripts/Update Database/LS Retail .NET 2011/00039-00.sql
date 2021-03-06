
/*

	Incident No.	: N/A
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2011 - Sprint 5
	Date created	: 04.01.2011

	Description		: Add a field to PURCHASEORDERS and remove some fields as well

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	PURCHASEORDERS	-	STOREID field added	
						PURCHASEORDERS	-	DELIVERYNAME field removed
						PURCHASEORDERS	-	DELIVERYADDRESS field removed
						PURCHASEORDERS	-	DELIVERYSTREET field removed
						PURCHASEORDERS	-	DELIVERYZIPCODE field removed
						PURCHASEORDERS	-	DELIVERYCITY field removed
						PURCHASEORDERS	-	DELIVERYCOUNTY field removed
						PURCHASEORDERS	-	DELIVERYSTATE field removed
						PURCHASEORDERS	-	DELIVERYCOUNTRY field removed		  
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='STOREID')
Begin
	ALTER TABLE PURCHASEORDERS ADD STOREID [nvarchar](20) NULL
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYADDRESS')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYADDRESS
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYSTREET')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYSTREET
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYZIPCODE')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYZIPCODE
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYCITY')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYCITY
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYCOUNTY')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYCOUNTY
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYSTATE')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYSTATE
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYCOUNTRY')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYCOUNTRY
End

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='DELIVERYNAME')
Begin
	ALTER TABLE PURCHASEORDERS DROP COLUMN DELIVERYNAME
End