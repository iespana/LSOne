﻿
/*

	Incident No.	: 15806
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012/Embla
	Date created	: 14.03.2012
	
	Description		: alter table POSPERIODICDISCOUNTLINELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- POSPERIODICDISCOUNTLINELog - Removed lineid and added POSPERIODICDISCOUNTLINEGUID
						
*/


USE LSPOSNET_Audit

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSPERIODICDISCOUNTLINELog') AND NAME='LINEID')
ALTER TABLE dbo.POSPERIODICDISCOUNTLINELog DROP COLUMN LINEID

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSPERIODICDISCOUNTLINELog') AND NAME='POSPERIODICDISCOUNTLINEGUID')
ALTER TABLE dbo.POSPERIODICDISCOUNTLINELog Add POSPERIODICDISCOUNTLINEGUID uniqueidentifier NOT NULL default NEWID() 

GO