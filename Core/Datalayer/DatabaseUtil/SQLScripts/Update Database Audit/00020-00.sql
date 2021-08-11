/*

	Incident No.	: 9287
	Responsible		: Björn Eiríksson
	Sprint			: 
	Date created	: 30.03.2011

	Description		: New fields added to table POSMENULINELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMENULINELog
						
*/

USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMENULINELog') AND NAME='USEHEADERFONT')
begin
	ALTER TABLE POSMENULINELog ADD USEHEADERFONT tinyint NULL
	ALTER TABLE POSMENULINELog ADD USEHEADERATTRIBUTES tinyint NULL
end

GO