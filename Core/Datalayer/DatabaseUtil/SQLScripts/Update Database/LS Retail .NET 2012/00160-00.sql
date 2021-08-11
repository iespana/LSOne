
/*

	Incident No.	: 14213
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012\Askur
	Date created	: 31.01.2012

	Description		: Adding fields to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosTransactionServiceProfile - Added field UseCreditVouchers					  
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSTRANSACTIONSERVICEPROFILE') AND NAME='USECREDITVOUCHERS')
ALTER TABLE dbo.POSTRANSACTIONSERVICEPROFILE ADD USECREDITVOUCHERS TINYINT NULL

GO