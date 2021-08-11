/*

	Incident No.	: 19838
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 16.11.2012

	Description		: Add column KITCHENMANAGERPROFILEID to RBOSTORETABLE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOSTORETABLE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTORETABLE') AND NAME='KITCHENMANAGERPROFILEID')
BEGIN
	ALTER TABLE RBOSTORETABLE ADD KITCHENMANAGERPROFILEID uniqueidentifier NULL
END
GO