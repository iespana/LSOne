
/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 2011 - Sprint 5
	Date created	: 07.01.2011

	Description		: Add a field to COMPANYINFO

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	COMPANYINFO	-	COMPANYLOGO field added	
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('COMPANYINFO') AND NAME='COMPANYLOGO')
Begin
	ALTER TABLE dbo.COMPANYINFO ADD COMPANYLOGO varbinary(MAX) NULL
End


