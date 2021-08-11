/*

	Incident No.	: N/A
	Responsible		: GuÃ°bjÃ¶rn Einarsson
	Sprint			: Sprint 4 - 2010
	Date created	: 21.12.2010

	Description		: VENDORITEMS - column added

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: VENDORITEMS - column added

						
*/

Use LSPOSNET

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('VENDORITEMS') AND NAME='ITEMPRICE')
ALTER TABLE dbo.VENDORITEMS ADD ITEMPRICE numeric(28,12) NOT NULL DEFAULT 0

