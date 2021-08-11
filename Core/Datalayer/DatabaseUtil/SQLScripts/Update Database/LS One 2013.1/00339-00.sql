USE LSPOSNET
GO
  
if not exists(select 'x' from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = N'USERSETTINGS' and COLUMN_NAME = N'Type')
begin
	alter table USERSETTINGS add Type uniqueidentifier not null Default('C79AE480-7EE1-11DB-9FE1-0800200C9A66')
end

GO
