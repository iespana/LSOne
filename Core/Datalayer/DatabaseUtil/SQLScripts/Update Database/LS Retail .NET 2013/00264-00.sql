﻿/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 05.12.2012

	Description		: Remove column MODIFIEDITEMS and ROUTEID from table STATIONSELECTION
	
	
	Tables affected	: STATIONSELECTION
						
*/
USE LSPOSNET
GO
  
IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('STATIONSELECTION') AND NAME='MODIFIEDITEMS')
BEGIN
	ALTER TABLE STATIONSELECTION Drop Column MODIFIEDITEMS 
END	


IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('STATIONSELECTION') AND NAME='ROUTEID')
BEGIN
	ALTER TABLE STATIONSELECTION Drop Column ROUTEID 
END	