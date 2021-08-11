/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer
	Sprint			: 2011 - Store Controller 2.0.2 - Sprint 1
	Date created	: 20.05.2011

	Description		: column TAXCOLLECTLIMIT.TAXCODE was too short

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: TAXCOLLECTLIMIT
						
*/

USE LSPOSNET

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'TAXCOLLECTLIMIT' AND COLUMN_NAME = 'TAXCODE') = 10)
begin
	ALTER TABLE TAXCOLLECTLIMIT ALTER COLUMN TAXCODE [nvarchar](20) NOT NULL
end
GO
