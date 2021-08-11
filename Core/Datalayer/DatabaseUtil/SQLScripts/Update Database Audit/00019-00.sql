/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2011 - Store Controller 2.0.2 - Sprint 1
	Date created	: 17.03.2011
					  06.07.2013 - modified (STATE is not part of CUSTTABLE/CUSTTABLELog anymore)

	Description		: Made the State field longer to prevent crashes

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: CUSTTABLELog, RBOSTAFFTABLELog, RBOSTORETABLELog, VENDTABLELog, COMPANYINFOLog
						
*/

USE LSPOSNET_Audit

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'RBOSTAFFTABLELog' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE RBOSTAFFTABLELog ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'RBOSTORETABLELog' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE RBOSTORETABLELog ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'VENDTABLELog' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE VENDTABLELog ALTER COLUMN STATE [nvarchar](30) NULL
end

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'COMPANYINFOLog' AND COLUMN_NAME = 'STATE') = 10)
begin
	ALTER TABLE COMPANYINFOLog ALTER COLUMN STATE [nvarchar](30) NULL
end

GO