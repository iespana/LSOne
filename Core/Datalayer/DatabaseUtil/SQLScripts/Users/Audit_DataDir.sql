
/*

	Incident No.	: N/A
	Responsible		: N/A
	Sprint			: N/A
	Date created	: 21.0.2011

	Description		: Creates the Data Director user for the Audit database - is given db_datawrite role

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: N/A
						
*/


USE [master]
IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DataDir') 
BEGIN	
	CREATE LOGIN [DataDir] WITH PASSWORD=N'DataDir.2008', DEFAULT_DATABASE=[LSPOSNET], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
END

USE [LSPOSNET_Audit]
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DataDir') 
BEGIN
	CREATE USER [DataDir] FOR LOGIN [DataDir]	
END

USE [LSPOSNET_Audit]
EXEC sp_addrolemember N'db_datawriter', N'DataDir'
GO


