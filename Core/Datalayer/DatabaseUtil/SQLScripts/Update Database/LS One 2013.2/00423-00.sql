/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 4.10.2013

	Description		: Added a new column for print options
*/

USE LSPOSNET

GO

if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'RBOTRANSACTIONEFTINFOTRANS' and COLUMN_NAME = 'RECEIPTPRINTING')
begin
	alter table RBOTRANSACTIONEFTINFOTRANS add RECEIPTPRINTING int
end
