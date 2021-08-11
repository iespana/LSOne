/*

	Incident No.	: 19839
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 14.11.2012

	Description		: Changes column name KDSID to KDSPROFILEID

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: KMTIMESTYLE
						
*/

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMTIMESTYLE') AND NAME='KDSID')
BEGIN
	EXEC sp_RENAME 'KMTIMESTYLE.KDSID', 'KDSPROFILEID', 'COLUMN'
END
GO

