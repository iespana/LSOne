/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 10.12.2012

	Description		: Remove table KMINTERFACEPROFILE
	
	
	Tables affected	: KMINTERFACEPROFILE
						
*/
USE LSPOSNET
GO
  
IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE'))
BEGIN
	Drop Table KMINTERFACEPROFILE
END	