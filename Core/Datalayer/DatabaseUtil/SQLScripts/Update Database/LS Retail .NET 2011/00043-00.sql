
/*

	Incident No.	: N/A
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2011 - Sprint 5
	Date created	: 06.01.2011

	Description		: Add a field to PURCHASEORDERS 

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	PURCHASEORDERS	-	ORDERINGDATE field added	
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('PURCHASEORDERS') AND NAME='ORDERINGDATE')
Begin
	ALTER TABLE PURCHASEORDERS ADD ORDERINGDATE DATETIME NULL
End


