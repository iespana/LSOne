/*

	Incident No.	: 18492
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 10.09.2012

	Description		: Add column ONVOID to table KMINTERFACEPROFILE
	
	
	Tables affected	: KMINTERFACEPROFILE
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='ONVOID')
BEGIN
	ALTER TABLE KMINTERFACEPROFILE ADD ONVOID int NULL
END	
GO
