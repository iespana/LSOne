
/*

	Incident No.	: 15292
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012/Hel
	Date created	: 23.03.2012
	
	Description		: alter table POSMMLINEGROUPSLog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : POSMMLINEGROUPSLog - Added description column
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMMLINEGROUPSLog') AND NAME='DESCRIPTION')
ALTER TABLE dbo.POSMMLINEGROUPSLog add DESCRIPTION  nvarchar (30) NULL

GO
