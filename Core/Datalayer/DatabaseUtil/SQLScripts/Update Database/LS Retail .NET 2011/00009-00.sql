
/*

	Incident No.	: 6606
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 03\Dot Net Team
	Date created	: 10.11.2010

	Description		: Added fields for hospitality profiles

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: 
						RboTerminalTable			- added NVarChar field "SalesTypeFilter".
						PosFunctionalityProfile 	- added TinyInt field "IsHospitalityProfile".
						
						
*/

use LSPOSNET

go


IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOTERMINALTABLE]') AND TYPE IN ('U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[DBO].[RBOTERMINALTABLE]') AND NAME='SALESTYPEFILTER')
	BEGIN
		alter table dbo.RBOTERMINALTABLE 
		add SALESTYPEFILTER nvarchar (250) NULL
	END
END
GO

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSFUNCTIONALITYPROFILE]') AND TYPE IN ('U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('[DBO].[POSFUNCTIONALITYPROFILE]') AND NAME='ISHOSPITALITYPROFILE')
	BEGIN
		alter table dbo.POSFUNCTIONALITYPROFILE 
		add ISHOSPITALITYPROFILE tinyint NULL
	END
END
GO
