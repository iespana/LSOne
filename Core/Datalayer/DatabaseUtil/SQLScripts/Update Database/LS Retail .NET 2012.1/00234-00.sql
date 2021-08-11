﻿/*

	Incident No.	: xxx
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 08.09.2012

	Description		: Add columns NUMBEROFCOLUMNS and NUMBEROFROWS to table KMINTERFACEPROFILE
	
	
	Tables affected	: NUMBEROFCOLUMNS
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='NUMBEROFCOLUMNS')
BEGIN
	ALTER TABLE KMINTERFACEPROFILE ADD NUMBEROFCOLUMNS int NULL
END	
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILE') AND NAME='NUMBEROFROWS')
BEGIN
	ALTER TABLE KMINTERFACEPROFILE ADD NUMBEROFROWS int NULL
END	
GO
