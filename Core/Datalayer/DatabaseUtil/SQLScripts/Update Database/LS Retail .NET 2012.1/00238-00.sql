/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 11.09.2012

	Description		: Add column Description to table KMINTERFACEPROFILE
	
	
	Tables affected	: KMINTERFACEPROFILE
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='Description')
BEGIN
	ALTER TABLE KMINTERFACEPROFILE ADD [Description] nvarchar(100) NULL
END	
GO
