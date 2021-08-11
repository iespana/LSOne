/*

	Incident No.	: N/A
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 11.10.13

	Description		: Added FORMLAYOUTTYPEID to POSISFORMLAYOUT
*/

USE LSPOSNET

GO

if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSISFORMLAYOUT' and COLUMN_NAME = 'FORMLAYOUTTYPEID')
begin
	alter table POSISFORMLAYOUT add FORMLAYOUTTYPEID nvarchar(50) not null default '9662E62D-55DF-40CF-B7E2-907FB770D60B'
end

go
