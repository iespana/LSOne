/*

	Incident No.	: N/A
	Responsible		: Tobias Helmer		
	Sprint			: N/A
	Date created	: 14.12.2010

	Description		: INVENTTRANS - column added

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: INVENTTRANS - column added

						
*/
--  Table was recreated in 00030  - HG.
--IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('INVENTTRANS') AND NAME='UNITID')
--ALTER TABLE dbo.INVENTTRANS ADD UNITID nvarchar(20) NOT NULL CONSTRAINT DF_INVENTTRANS_UNITID DEFAULT N'';

