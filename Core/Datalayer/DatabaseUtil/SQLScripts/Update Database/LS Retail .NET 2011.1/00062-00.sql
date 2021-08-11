/*

	Incident No.	: N/A
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2011 - Store Controller 2.0.2 - Sprint 1
	Date created	: 21.03.2011

	Description		: Add a field to RBOSTATEMENTTABLE to indicate whether en ERP system has processed the statement

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOSTATEMENTTABLE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RBOSTATEMENTTABLE') AND NAME='ERPPROCESSED')
begin
	ALTER TABLE RBOSTATEMENTTABLE ADD ERPPROCESSED tinyint NULL
end


GO