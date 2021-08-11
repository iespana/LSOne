
/*

	Incident No.	: 9450
	Responsible		: Björn Eiríksson
	Sprint			: SP3
	Date created	: 06.04.2011

	Description		: Expanded the Unit ID in the INVENTITEMBARCODE table

	Logic scripts   : Audit Logig
	
	Tables affected	: INVENTITEMBARCODE
						
*/

USE LSPOSNET_Audit

GO

if((SELECT CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns
WHERE TABLE_NAME = 'INVENTITEMBARCODELog' AND COLUMN_NAME = 'UNITID') = 10)
begin
	ALTER TABLE INVENTITEMBARCODELog ALTER COLUMN UNITID [nvarchar](20) NULL
end

GO