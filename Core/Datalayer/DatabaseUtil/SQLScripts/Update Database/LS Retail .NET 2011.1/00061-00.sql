/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2011 - Store Controller 2.0.2 - Sprint 1
	Date created	: 17.03.2011

	Description		: Made the State field longer to prevent crashes

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: CUSTTABLE, RBOSTAFFTABLE, RBOSTORETABLE, VENDTABLE, COMPANYINFO
						
*/

USE LSPOSNET

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'CUSTTABLE' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE CUSTTABLE ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'RBOSTAFFTABLE' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE RBOSTAFFTABLE ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE RBOSTORETABLE ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'VENDTABLE' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE VENDTABLE ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'COMPANYINFO' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE COMPANYINFO ALTER COLUMN STATE [nvarchar](30) NULL
end