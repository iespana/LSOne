
/*

	Incident No.	: 
	Responsible		: GuÃ°bjÃ¶rn Einarsson
	Sprint			: LS Retail .NET v 2011 - Sprint 5
	Date created	: 10.1.2011
	
	Description		: Change POSISTENDERRESTRICTIONSLog to use 20 letter key instead of a 10 letter key.

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:  POSISTENDERRESTRICTIONSLog - TENDERID set to nvarchar(20)
						
*/


USE LSPOSNET_Audit

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'POSISTENDERRESTRICTIONSLog' AND COLUMN_NAME = 'TENDERID') = 10)
begin
	ALTER TABLE POSISTENDERRESTRICTIONSLog ALTER COLUMN TENDERID [nvarchar](20) NOT NULL
end

GO




