
/*

	Incident No.	: N/A
	Responsible		: N/A
	Sprint			: N/A
	Date created	: 15.07.2010

	Description		: Creates the Data Director user for a specific database. The "USE [LSPOSNET]" will be replaced with the current database name

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: N/A
						
*/


USE [master]
IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DataDir') 
BEGIN		
	CREATE LOGIN [DataDir] WITH PASSWORD=N'DataDir.2008', DEFAULT_DATABASE=[LSPOSNET], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
END

USE [LSPOSNET]
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'DataDir') 
BEGIN
	CREATE USER [DataDir] FOR LOGIN [DataDir]	
END

USE [LSPOSNET]
EXEC sp_addrolemember N'db_owner', N'DataDir' 
GO


