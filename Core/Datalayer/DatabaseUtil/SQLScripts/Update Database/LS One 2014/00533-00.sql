
/*

	Incident No.	: N/A
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: N/A
	Date created	: 24.5.2014

	Description		: Because a column was deleted from NumberSequenceTable and the trigger changed we need to run the trigger here so that the 
					  logic scripts can run in the order they should be running - otherwise an error will come up.
	
						
*/

Use LSPOSNET


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_NUMBERSEQUENCETABLE]'))
begin
   drop trigger dbo.Update_NUMBERSEQUENCETABLE
end

GO

create trigger Update_NUMBERSEQUENCETABLE
on NUMBERSEQUENCETABLE after insert,update, delete as

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int
Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

if @writeAudit = 1
begin
    
	set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

	if @connectionUser IS null
		begin
			set @sessionUser = SYSTEM_USER
			set @connectionUser = NewID()
		end
	else
		set @sessionUser = ''
		
	declare @DeletedCount int
	declare @InsertedCount int
	
	select @DeletedCount = COUNT(*) FROM DELETED
	select @InsertedCount = COUNT(*) FROM inserted
	
	begin try
		if @DeletedCount > 0 and @InsertedCount = 0
		begin
			insert into LSPOSNET_Audit.dbo.NUMBERSEQUENCETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				NUMBERSEQUENCE,
				TXT,
				HIGHEST,
				FORMAT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.NUMBERSEQUENCE,
				ins.TXT,
				ins.HIGHEST,
				ins.FORMAT,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.NUMBERSEQUENCETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				NUMBERSEQUENCE,
				TXT,
				HIGHEST,
				FORMAT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.NUMBERSEQUENCE,
				ins.TXT,
				ins.HIGHEST,
				ins.FORMAT,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO





