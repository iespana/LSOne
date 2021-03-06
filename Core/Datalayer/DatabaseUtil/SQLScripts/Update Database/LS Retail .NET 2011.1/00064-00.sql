/*

	Incident No.	: N/A
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2011 - Store Controller 2.0.2 - Sprint 1
	Date created	: 29.03.2011

	Description		: Add a field to TAXTABLE so that the POS can have a display code for taxcodes now that the ID is autogenerated. 
					  This field can also be used f.x. by ERP systems who rely on tax code names to function.

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: TAXTABLE (field PRINTCODE)
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('TAXTABLE') AND NAME='PRINTCODE')
begin
	ALTER TABLE TAXTABLE ADD PRINTCODE nvarchar(20) NULL
end

