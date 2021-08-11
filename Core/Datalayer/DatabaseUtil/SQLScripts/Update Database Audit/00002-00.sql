
/*

	Incident No.	: 6602
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET v 2010 - Sprint 2
	Date created	: 15.11.2010
	
	Description		: Added fields to RBOTREMINALTABLELog and POSFUNCTIONALITYPROFILELog to audit extra fields for hospitality

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	RboTerminalTableLog			- added NVarChar field "SalesTypeFilter".
						PosFunctionalityProfileLog 	- added TinyInt field "IsHospitalityProfile".
						
*/


USE LSPOSNET_Audit

GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOTERMINALTABLELog]') AND TYPE IN ('U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[DBO].[RBOTERMINALTABLELog]') AND NAME='SALESTYPEFILTER')
	BEGIN
		alter table dbo.RBOTERMINALTABLELog 
		add SALESTYPEFILTER nvarchar (250) NULL
	END
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSFUNCTIONALITYPROFILELog]') AND TYPE IN ('U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[DBO].[POSFUNCTIONALITYPROFILELog]') AND NAME='ISHOSPITALITYPROFILE')
	BEGIN
		alter table dbo.POSFUNCTIONALITYPROFILELog 
		add ISHOSPITALITYPROFILE tinyint NULL
	END
END
GO
