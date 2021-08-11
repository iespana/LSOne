
/*

	Incident No.	: 9287
	Responsible		: Björn Eiríksson
	Sprint			: 
	Date created	: 30.03.2011

	Description		: New fields added to table POSMENULINE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMENULINE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMENULINE') AND NAME='USEHEADERFONT')
begin
	ALTER TABLE POSMENULINE ADD USEHEADERFONT tinyint NULL
	ALTER TABLE POSMENULINE ADD USEHEADERATTRIBUTES tinyint NULL
end