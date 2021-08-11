﻿
/*

	Incident No.	: [TFS incident no]
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET v 2010 - Sprint 1
	Date created	: 29.06.2010
	
	Description		: Database changes needed for Infocode changes and partial redesign

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	RboInfocodeTable			- fields added, default values added to fields
						RboInfocodeTableSpecific	- fields added, default values added to fields
						RboInformationSubcodeTable	- fields added, default values added to fields					  
						RboTransactionInfocodeTable - PRIMARY KEY changed, fields added
						
*/

-- RBOINFOCODETABLE
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='USAGECATEGORY')
ALTER TABLE dbo.RBOINFOCODETABLE ADD USAGECATEGORY INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='DISPLAYOPTION')
ALTER TABLE dbo.RBOINFOCODETABLE ADD DISPLAYOPTION INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='LINKITEMLINESTOTRIGGERLINE')
ALTER TABLE dbo.RBOINFOCODETABLE ADD LINKITEMLINESTOTRIGGERLINE TINYINT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='MULTIPLESELECTION')
ALTER TABLE dbo.RBOINFOCODETABLE ADD MULTIPLESELECTION TINYINT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='TRIGGERING')
ALTER TABLE dbo.RBOINFOCODETABLE ADD TRIGGERING TINYINT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='MINSELECTION')
ALTER TABLE dbo.RBOINFOCODETABLE ADD MINSELECTION INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='MAXSELECTION')
ALTER TABLE dbo.RBOINFOCODETABLE ADD MAXSELECTION INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='CREATEINFOCODETRANSENTRIES')
ALTER TABLE dbo.RBOINFOCODETABLE ADD CREATEINFOCODETRANSENTRIES TINYINT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='EXPLANATORYHEADERTEXT')
ALTER TABLE dbo.RBOINFOCODETABLE ADD EXPLANATORYHEADERTEXT NVARCHAR(30) NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLE') AND NAME='OKPRESSEDACTION')
ALTER TABLE dbo.RBOINFOCODETABLE ADD OKPRESSEDACTION INT NULL;

--RBOINFORMATIONSUBCODETABLE
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='USAGECATEGORY')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD USAGECATEGORY INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='VARIANTCODE')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD VARIANTCODE NVARCHAR(10) NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='VARIANTNEEDED')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD VARIANTNEEDED TINYINT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='QTYLINKEDTOTRIGGERLINE')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD QTYLINKEDTOTRIGGERLINE TINYINT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='PRICEHANDLING')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD PRICEHANDLING INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='LINKEDINFOCODE')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD LINKEDINFOCODE NVARCHAR(10) NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='UNITOFMEASURE')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD UNITOFMEASURE NVARCHAR(10) NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='QTYPERUNITOFMEASURE')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD QTYPERUNITOFMEASURE NUMERIC(28,12) NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='INFOCODEPROMPT')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD INFOCODEPROMPT NVARCHAR(30) NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='MAXSELECTION')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD MAXSELECTION INT NULL;
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFORMATIONSUBCODETABLE') AND NAME='SERIALLOTNEEDED')
ALTER TABLE dbo.RBOINFORMATIONSUBCODETABLE ADD SERIALLOTNEEDED TINYINT NULL;

--RBOINFOCODETABLESPECIFIC
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLESPECIFIC') AND NAME='TRIGGERING')
ALTER TABLE dbo.RBOINFOCODETABLESPECIFIC ADD TRIGGERING TINYINT NULL
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLESPECIFIC') AND NAME='UNITOFMEASURE')
ALTER TABLE dbo.RBOINFOCODETABLESPECIFIC ADD UNITOFMEASURE NVARCHAR(10) NULL
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLESPECIFIC') AND NAME='SALESTYPEFILTER')
ALTER TABLE dbo.RBOINFOCODETABLESPECIFIC ADD SALESTYPEFILTER NVARCHAR(250) NULL
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOINFOCODETABLESPECIFIC') AND NAME='USAGECATEGORY')
ALTER TABLE dbo.RBOINFOCODETABLESPECIFIC ADD USAGECATEGORY INT NULL;

-- Add new POS operation
IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 129 and [DATAAREAID] = 'LSR')
INSERT INTO [POSISOPERATIONS] ([OPERATIONID], [OPERATIONNAME], [PERMISSIONID], [PERMISSIONID2], [CHECKUSERACCESS],[USEROPERATION],[DATAAREAID]) VALUES (129,'Infocode On Request',NULL,NULL,1,1,'LSR')