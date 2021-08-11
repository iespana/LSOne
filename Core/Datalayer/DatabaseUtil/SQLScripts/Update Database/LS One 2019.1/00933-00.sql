/*

	Incident No.	: ONE-10270 Omni ManagerPrivileges in SiteManager
	Responsible		: Simona Avornicesei
	Sprint			: Sun
	Date created	: 04.07.2019

	Description		: Add Omni manager permission to LSOne
	
						
*/
USE LSPOSNET

DECLARE @DATAAREAID NVarChar(10)
DECLARE @permissionGroupGuid UniqueIdentifier

SET @DATAAREAID = 'LSR'
SET @permissionGroupGuid = '{815CD310-EDF2-43E4-A5BC-CD28F8199F97}'

IF NOT EXISTS (SELECT * FROM [PERMISSIONGROUP] WHERE [GUID] = @permissionGroupGuid)
BEGIN
	INSERT INTO [PERMISSIONGROUP] ([GUID],[Name],[DATAAREAID]) 
		VALUES (@permissionGroupGuid, 'Omni', @DATAAREAID)

	-- add localization
	INSERT INTO PERMISSIONGROUPLOCALIZATION ([Locale], [GUID],[Name],[DATAAREAID]) 
		SELECT DISTINCT [Locale], @permissionGroupGuid, 'Omni', @DATAAREAID FROM PERMISSIONGROUPLOCALIZATION
END