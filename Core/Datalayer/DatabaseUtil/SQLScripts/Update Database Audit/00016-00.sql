/*

	Incident No.	: 
	Responsible		: GuÃ°bjÃ¶rn Einarsson
	Sprint			: LS Retail .NET v 2011 - Sprint 5
	Date created	: 12.1.2011
	
	Description		: Change CUSTTABLELog columns to use 20 letter key instead of a 10 letter key.

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	CUSTTABLELog - TAXGROUP set to nvarchar(20)
						CUSTTABLELog - LINEDISC set to nvarchar(20)
						CUSTTABLELog - PRICEGROUP set to nvarchar(20)
						CUSTTABLELog - MULTILINEDISC set to nvarchar(20)
						CUSTTABLELog - ENDDISC set to nvarchar(20)
							
*/


USE LSPOSNET_Audit

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'TAXGROUP') = 10)
begin
	ALTER TABLE CUSTTABLELog ALTER COLUMN TAXGROUP [nvarchar](20) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'LINEDISC') = 10)
begin
	ALTER TABLE CUSTTABLELog ALTER COLUMN LINEDISC [nvarchar](20) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'PRICEGROUP') = 10)
begin
	ALTER TABLE CUSTTABLELog ALTER COLUMN PRICEGROUP [nvarchar](20) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'MULTILINEDISC') = 10)
begin
	ALTER TABLE CUSTTABLELog ALTER COLUMN MULTILINEDISC [nvarchar](20) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'ENDDISC') = 10)
begin
	ALTER TABLE CUSTTABLELog ALTER COLUMN ENDDISC [nvarchar](20) NULL
end

GO