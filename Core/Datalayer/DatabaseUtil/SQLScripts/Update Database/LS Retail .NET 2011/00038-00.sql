﻿/*

	Incident No.	: 
	Responsible		: Björn Eiríksson
	Sprint			: LS Retail .NET v 2010 - Sprint 4
	Date created	:4.1.2011
	
	Description		: Change tables to use 20 letter keys for the new sequence manager.

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- Most
						
*/

USE LSPOSNET

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'RBOSTYLES' AND COLUMN_NAME = 'STYLE') = 4)
begin
	ALTER TABLE RBOSTYLES ALTER COLUMN STYLE [nvarchar](20) NOT NULL
end

GO







