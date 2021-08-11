/*

	Incident No.	: 9336
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS Retail.Net 2011.1\Sprint 1 - Convergence Atlanta
	Date created	: 30.03.2011

	Description		: Change the RBOTRANSACTIONEFTINFOTRANS so that the TenderType field is an nvarchar field

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RboTransactionEftInfoTrans	-	Changed TenderType to nvarchar
						
*/

USE LSPOSNET
GO

IF 'int' IN (SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' AND COLUMN_NAME = 'TENDERTYPE')
BEGIN
	alter table RBOTRANSACTIONEFTINFOTRANS
	drop column TENDERTYPE
	
	alter table RBOTRANSACTIONEFTINFOTRANS
	add TENDERTYPE nvarchar(20) 
END
GO