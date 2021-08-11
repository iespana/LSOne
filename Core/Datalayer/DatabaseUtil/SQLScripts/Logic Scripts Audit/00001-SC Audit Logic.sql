
/*

	Incident No.	: 5703
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 01\Dot Net Team
	Date created	: 06.10.2010

	Description		: This is the "Audit Logic Script.sql" file from the Store Controller

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: n/a
						
*/

Use LSPOSNET 

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAuditing_ReadLogs_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAuditing_ReadLogs_1_0]

GO

create procedure dbo.spAuditing_ReadLogs_1_0
(@cmd Nvarchar(255), @dataareaID nvarchar(10), @context uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

set nocount on

if @user = ''
begin
	set @user = '%%'
end

begin try
	if @context is NULL
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + ''',NULL,''' + @user + ''',''' + @from + ''',''' + @to + '''')
	else
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + ''',''' + @context + ''',''' + @user + ''',''' + @from + ''',''' + @to + '''')
end try
begin catch

end catch

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAuditing_ReadLogsII_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAuditing_ReadLogsII_1_0]

GO

create procedure dbo.spAuditing_ReadLogsII_1_0
(@cmd Nvarchar(255), @dataareaID nvarchar(10), @context Nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

set nocount on

if @user = ''
begin
	set @user = '%%'
end

begin try
	if @context is NULL
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + ',NULL,''' + @user + ''',''' + @from + ''',''' + @to + '''')
	else
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + ''',''' + @context + ''',''' + @user + ''',''' + @from + ''',''' + @to + '''')
end try
begin catch

end catch


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAuditing_ReadLogsIII_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAuditing_ReadLogsIII_1_0]

GO

create procedure dbo.spAuditing_ReadLogsIII_1_0
(@cmd Nvarchar(255), @dataareaID nvarchar(10), @context Nvarchar(255),@context2 Nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

set nocount on

if @user = ''
begin
	set @user = '%%'
end

begin try
	if @context is NULL
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + 'NULL,''' + @context2 + ''',''' + @user + ''',''' + @from + ''',''' + @to + '''')
	else
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + ''',''' + @context + ''',''' + @context2 + ''',''' + @user + ''',''' + @from + ''',''' + @to + '''')
end try
begin catch

end catch


GO

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAuditing_ReadLogsIV_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAuditing_ReadLogsIV_1_0]

GO

create procedure dbo.spAuditing_ReadLogsIV_1_0
(@cmd Nvarchar(255), @dataareaID nvarchar(10), @context Nvarchar(255),@context2 Nvarchar(255),@context3 Nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

set nocount on

if @user = ''
begin
	set @user = '%%'
end

begin try
	if @context is NULL
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + 'NULL,''' + @context2 + ''',''' +@context3 + ''',''' + @user + ''',''' + @from + ''',''' + @to + '''')
	else
		exec ('LSPOSNET_Audit.dbo.' + @cmd + ' ''' + @dataareaID + ''',''' + @context + ''',''' + @context2 + ''',''' + @context3 + ''',''' + @user + ''',''' + @from + ''',''' + @to + '''')
end try
begin catch

end catch


GO

-- ---------------------------------------------------------------------------------------------------
use LSPOSNET_Audit

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_SignAction_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_SignAction_1_0]

GO

create procedure dbo.spAUDIT_SignAction_1_0
(@dataareaID nvarchar(10),
 @contextGuid uniqueidentifier,
 @actionGuid uniqueidentifier,
 @userGuid uniqueidentifier,
 @reason ntext) 
as

set nocount on 

insert into SIGNEDACTIONS (GUID,DATAAREAID,ContextGuid,ActionGuid,Reason,UserGuid,CreatedOn)
values (NewID(),@dataareaID,@contextGUID,@actionGuid,@reason,@userGuid,GETDATE())		

Go

Use LSPOSNET

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_SignAction_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_SignAction_1_0]

GO

create procedure dbo.spSECURITY_SignAction_1_0
(@dataareaID nvarchar(10),
 @contextGuid uniqueidentifier,
 @actionGuid uniqueidentifier,
 @reason ntext) 
as

set nocount on

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int

set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

if not @connectionUser IS null
begin
	insert into SIGNEDACTIONS (GUID,DATAAREAID,ContextGuid,ActionGuid,Reason,UserGuid,CreatedOn)
	values (NewID(),@dataareaID,@contextGUID,@actionGuid,@reason,@connectionUser,GETDATE())
	
	Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

	if @writeAudit = 1
	begin
		begin try
			exec LSPOSNET_Audit.dbo.spAUDIT_SignAction_1_0 @dataareaID,@contextGuid,@actionGuid,@connectionUser,@reason
		end try
		begin catch
		end catch
	end		
end

GO


-- ---------------------------------------------------------------------------------------------------
use LSPOSNET_Audit

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_DeleteLogs_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_DeleteLogs_1_0]

GO

create procedure dbo.spAUDIT_DeleteLogs_1_0
	(@toDate datetime,@dataareaID nvarchar(10))
as

set nocount on

Delete from SYSTEMSETTINGSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERGROUPPERMISSIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERSLOG where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERLOGINLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERPERMISSIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERSINGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSVISUALPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSHARDWAREPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTORETABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTORETENDERTYPECARDTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTORETENDERTYPETABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOTERMINALTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOTENDERTYPECARDTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOTENDERTYPECARDNUMBERSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOTENDERTYPETABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISTENDERRESTRICTIONSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PAYMENTLIMITATIONSLog where AuditDate < @toDate
Delete from RBOSTAFFTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from OPERATIONSLog where AuditDate < @toDate
Delete from POSISTILLLAYOUTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CUSTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CUSTOMERADDRESSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSFUNCTIONALITYPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSTRANSACTIONSERVICEPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISFORMLAYOUTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISUPDATESMASTERLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOCOLORGROUPTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOCOLORGROUPTRANSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTYLEGROUPTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTYLEGROUPTRANSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSIZEGROUPTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSIZEGROUPTRANSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from BARCODESETUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOBARCODEMASKTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOBARCODEMASKSEGMENTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTITEMBARCODELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTDIMCOMBINATIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PRICEDISCTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PRICEDISCGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from TAXTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from TAXDATALog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from TAXITEMGROUPHEADINGLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTDIMGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTDIMSETUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from TAXGROUPHEADINGLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from TAXGROUPDATALog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from DECIMALSETTINGSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISLICENSELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISPARAMETERSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from COMPANYINFOLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from SALESPARAMETERSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PRICEPARAMETERSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSPERIODICDISCOUNTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSPERIODICDISCOUNTLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSDISCVALIDATIONPERIODLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSMULTIBUYDISCOUNTLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from HOSPITALITYSETUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINVENTITEMRETAILGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINVENTITEMDEPARTMENTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSPECIALGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSMMLINEGROUPSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CURRENCYLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from UNITLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PRICEGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOLOCATIONPRICEGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from NUMBERSEQUENCETABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from UNITCONVERTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from EXCHRATESLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTORECASHDECLARATIONTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from SALESTYPELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTATEMENTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTATEMENTLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CONTACTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from VENDTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTJOURNALTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTJOURNALTRANSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTTRANSREASONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from VENDORITEMSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from GOODSRECEIVINGLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from GOODSRECEIVINGLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PURCHASEORDERSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from PURCHASEORDERLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTATEMENTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOSTATEMENTLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from DININGTABLELAYOUTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from HOSPITALITYTYPELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSLOOKUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSMENUHEADERLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSMENULINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RESTAURANTSTATIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from DININGTABLELAYOUTSCREENLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSCOLORLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RESTAURANTDININGTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RESTAURANTMENUTYPELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from STATIONPRINTINGROUTESLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from STATIONSELECTIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINFORMATIONSUBCODETABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINFOCODETABLESPECIFICLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINFOCODETABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOCUSTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINVENTITEMIMAGELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINVENTLINKEDITEMLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOGIFTCARDTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOGIFTCARDTRANSACTIONSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOCREDITVOUCHERTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOCREDITVOUCHERTRANSACTIONSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISSUSPENSIONADDINFOLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISSUSPENSIONTYPELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISSUSPENDEDTRANSACTIONSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSISSUSPENDTRANSADDINFOLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOMSRCARDTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBODISCOUNTOFFERLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINVENTTABLELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOINVENTTRANSLATIONSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYTRANSACTIONPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOTERMINALGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOTERMINALGROUPCONNECTIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSSTYLEPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSSTYLEPROFILELINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from POSCONTEXTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYSTATIONSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYITEMCONNECTIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYVISUALPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYTERMINALCONNECTIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYHOSPITALITYTYPECONNECTIONLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from KITCHENDISPLAYFUNCTIONALPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from USERLOGINTOKENSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTORYTRANSFERORDERLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTORYTRANSFERORDERLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTORYTRANSFERREQUESTLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTORYTRANSFERREQUESTLINELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CUSTGROUPLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CUSTGROUPCATEGORYLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from RBOPARAMETERSLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from DIMENSIONATTRIBUTELog where AuditDate < @toDate 
Delete from DIMENSIONTEMPLATELog where AuditDate < @toDate 
Delete from RETAILITEMLog where AuditDate < @toDate 
Delete from CUSTOMERORDERSETTINGSLog where AuditDate < @toDate
Delete from IMPORTPROFILELog where AuditDate < @toDate
Delete from IMPORTPROFILELINESLog where AuditDate < @toDate
Delete from POSUSERPROFILELog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from CUSTOMERLog where AuditDate < @toDate and DATAAREAID = @dataareaID
Delete from INVENTORYTEMPLATELog where AuditDate < @toDate and DATAAREAID = @dataareaID


Go

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_AddLoginLog_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_AddLoginLog_1_0]

GO

create procedure dbo.spSECURITY_AddLoginLog_1_0
(@dataareaID nvarchar(10),@Login nvarchar(32), @AuditUserGUID uniqueidentifier, @AuditFunction nvarchar(25))
as

set nocount on

insert into USERLOGINLog (DATAAREAID,Login, AuditDate,AuditFunction,AuditUserGUID)
values (@dataareaID,@Login,GETDATE(),@AuditFunction,@AuditUserGUID)

GO

Use LSPOSNET

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_DeleteAuditLogs_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSECURITY_DeleteAuditLogs_1_0]

GO

create procedure dbo.spSECURITY_DeleteAuditLogs_1_0
	(@dataAreaID nvarchar(10),@toDate datetime)
as

set nocount on

declare @connectionUser uniqueidentifier
declare @sessionUser nvarchar(32)
declare @writeAudit int

set @connectionUser = CAST(CONTEXT_INFO() as uniqueidentifier)

if not @connectionUser IS null
begin
	Select @writeAudit = Value from SYSTEMSETTINGS where GUID = '17e851c0-3037-11df-9aae-0800200c9a66'

	if @writeAudit = 1
	begin
		begin try
			exec LSPOSNET_Audit.dbo.spAUDIT_DeleteLogs_1_0 @toDate,@dataAreaID
		end try
		begin catch
		
		end catch
	end
end

GO




-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAuditing_AddLoginLog_1_0]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAuditing_AddLoginLog_1_0]

GO


create procedure dbo.spAuditing_AddLoginLog_1_0
(@dataareaID nvarchar(10),@Login nvarchar(32), @AuditUserGUID uniqueidentifier, @AuditFunction nvarchar(25))
as

set nocount on

begin try
	exec LSPOSNET_Audit.dbo.spSECURITY_AddLoginLog_1_0 @dataareaID,@Login,@AuditUserGUID,@AuditFunction
end try
begin catch

end catch

GO

-- audit triggers
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_tblUser]'))
begin
   drop trigger dbo.Update_tblUser
end

GO

create trigger Update_tblUser 
on USERS after update,insert as

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
		
	-- since the user table is special case in the system then
    -- we need to do some poking around
    
    declare @Count int
    declare @audit int
    
    set @audit = 1
	
	SELECT @Count = COUNT(*) FROM DELETED

	if @Count > 0
	begin
	   -- This was a update so we need to compare LastChangeTime of new and old
	   -- to get idea if this was user change or just system changing security structures.
	   
	   
	   if Exists(Select 'x' from Deleted d join Inserted i on d.GUID = i.GUID
	   where d.FirstName = i.FirstName and d.MiddleName = i.MiddleName and d.LastName = i.LastName and
	   d.NamePrefix = i.NamePrefix and d.NameSuffix = i.NameSuffix and d.Deleted = i.Deleted and i.STAFFID = d.STAFFID) 
	   begin
			set @audit = 0
	   end
	end
	
	begin try
		if @audit = 1
		begin
			insert into LSPOSNET_Audit.dbo.USERSLOG 
				(AuditUserGUID, 
				 AuditUserLogin,
				 AuditDate,
				 GUID,
				 DATAAREAID,
  				 Login,
				 IsDomainUser,
				 FirstName,
 				 MiddleName,
				 LastName,
				 NamePrefix,
				 NameSuffix,
				 STAFFID,
				 Deleted)
				 Select 
					@connectionUser, @sessionUser as AuditUserLogin, 
					GETDATE() as AuditDate,
					ins.GUID,
					ins.DATAAREAID,
					ins.Login,
					ins.IsDomainUser,
					ins.FirstName,
					ins.MiddleName,
					ins.LastName,
					ins.NamePrefix,
					ins.NameSuffix,
					ins.STAFFID,
					ins.Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
end

GO



-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_tblUserGroup]'))
begin
   drop trigger dbo.Update_tblUserGroup
end

GO

create trigger Update_tblUserGroup 
on USERGROUPS after update,insert as

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
	
	begin try	
		insert into LSPOSNET_Audit.dbo.USERGROUPLog (
			AuditUserGUID, 
			AuditUserLogin,
			AuditDate,
			GUID,
			DATAAREAID,
			[Name],
			IsAdminGroup,
			Locked,
			Deleted)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GUID,
				ins.DATAAREAID,
				ins.Name,
				ins.IsAdminGroup,
				ins.Locked,
				ins.Deleted
			From inserted ins
	end try
	begin catch
	
	end catch
end

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_tblUsersInGroup]'))
begin
   drop trigger dbo.Update_tblUsersInGroup
end

GO

create trigger Update_tblUsersInGroup
on USERSINGROUP after insert,delete as

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
		
	declare @Count int
	
	SELECT @Count = COUNT(*) FROM DELETED
	
	begin try
		IF @Count > 0
		begin
			insert into LSPOSNET_Audit.dbo.USERSINGROUPLog 
				(AuditUserGUID, 
				 AuditUserLogin,
				 AuditDate,
				 UserGUID,
				 DATAAREAID,
				 UserGroupGUID,
				 AuditFunction)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UserGUID,
				ins.DATAAREAID,
				ins.UserGroupGUID,
				'Remove' as [Action]
				From deleted ins
		end
		else
		begin
			insert into LSPOSNET_Audit.dbo.USERSINGROUPLog 
				(AuditUserGUID, 
				 AuditUserLogin,
				 AuditDate,
				 UserGUID,
				 DATAAREAID,
				 UserGroupGUID,
				 AuditFunction)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UserGUID,
				ins.DATAAREAID,
				ins.UserGroupGUID,
				'Add' as [Action]
				From inserted ins
		end
	end try
	begin catch
	end catch
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_tblUserPermission]'))
begin
   drop trigger dbo.Update_tblUserPermission
end

GO

create trigger Update_tblUserPermission
on USERPERMISSION after insert,update, delete as

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
			-- If we got here then we set permission to inherit
			insert into LSPOSNET_Audit.dbo.USERPERMISSIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				UserGUID,
				PermissionGUID,
				DATAAREAID,
				AuditFunction)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UserGUID,
				ins.PermissionGUID,
				ins.DATAAREAID,
				'Inherit'
				From deleted ins
		end
		else
		begin
			-- If we got here then we have grant or deny
			insert into LSPOSNET_Audit.dbo.USERPERMISSIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				UserGUID,
				PermissionGUID,
				DATAAREAID,
				AuditFunction)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UserGUID,
				ins.PermissionGUID,
				ins.DATAAREAID,
				Auditfunuction = Case
					when ins.[Grant] = 1 then 'Grant'
					else 'Deny'
				end
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_tblUserGroupPermission]'))
begin
   drop trigger dbo.Update_tblUserGroupPermission
end

GO

create trigger Update_tblUserGroupPermission
on USERGROUPPERMISSIONS after insert, delete as

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
	
	select @DeletedCount = COUNT(*) FROM DELETED
	
	begin try
		if @DeletedCount > 0
		begin
			-- If we got here then we set permission to deny
			insert into LSPOSNET_Audit.dbo.USERGROUPPERMISSIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				UserGroupGUID,
				DATAAREAID,
				PermissionGUID,
				AuditFunction)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UserGroupGUID,
				ins.DATAAREAID,
				ins.PermissionGUID,
				'Deny'
			From deleted ins
		end
		else
		begin
			-- If we got here then we have grant
			insert into LSPOSNET_Audit.dbo.USERGROUPPERMISSIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				UserGroupGUID,
				DATAAREAID,
				PermissionGUID,
				AuditFunction)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UserGroupGUID,
				ins.DATAAREAID,
				ins.PermissionGUID,
				'Grant'
			From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_tblUserSystemSettings]'))
begin
   drop trigger dbo.Update_tblUserSystemSettings
end

GO

create trigger Update_tblUserSystemSettings
on SYSTEMSETTINGS after update as

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
	
	begin try
		-- If we got here then we set permission to deny
		insert into LSPOSNET_Audit.dbo.SYSTEMSETTINGSLog (
			AuditUserGUID, 
			AuditUserLogin,
			AuditDate,
			SettingGUID,
			DATAAREAID,
			Value)
		Select 
			@connectionUser, @sessionUser as AuditUserLogin, 
			GETDATE() as AuditDate,
			ins.GUID,
			ins.DATAAREAID,
			ins.Value
		From inserted ins
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSVISUALPROFILE]'))
begin
   drop trigger dbo.Update_POSVISUALPROFILE
end

GO

create trigger Update_POSVISUALPROFILE
on POSVISUALPROFILE after insert,update, delete as

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
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSVISUALPROFILELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				PROFILEID,
				NAME,
				RESOLUTION,
				TERMINALTYPE,
				HIDECURSOR,
				DESIGNALLOWEDONPOS,
				OPAQUEBACKGROUNDFORM,
				OPACITY,
				USEFORMBACKGROUNDIMAGE,
				SCREENINDEX,
				RECEIPTPAYMENTLINESSIZE,
				CONFIRMBUTTONSTYLEID,
				CANCELBUTTONSTYLEID,
				ACTIONBUTTONSTYLEID,
				NORMALBUTTONSTYLEID,
				OTHERBUTTONSTYLEID,
				OVERRIDEPOSCONTROLBORDERCOLOR,
				POSCONTROLBORDERCOLOR,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.PROFILEID,
				ins.NAME,
				ins.RESOLUTION,
				ins.TERMINALTYPE,
				ins.HIDECURSOR,
				ins.DESIGNALLOWEDONPOS,
				ins.OPAQUEBACKGROUNDFORM,
				ins.OPACITY,
				ins.USEFORMBACKGROUNDIMAGE,
				ins.SCREENINDEX,
				ins.RECEIPTPAYMENTLINESSIZE,
				ins.CONFIRMBUTTONSTYLEID,
				ins.CANCELBUTTONSTYLEID,
				ins.ACTIONBUTTONSTYLEID,
				ins.NORMALBUTTONSTYLEID,
				ins.OTHERBUTTONSTYLEID,
				ins.OVERRIDEPOSCONTROLBORDERCOLOR,
				ins.POSCONTROLBORDERCOLOR,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSVISUALPROFILELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				PROFILEID,
				NAME,
				RESOLUTION,
				TERMINALTYPE,
				HIDECURSOR,
				DESIGNALLOWEDONPOS,
				OPAQUEBACKGROUNDFORM,
				OPACITY,
				USEFORMBACKGROUNDIMAGE,
				SCREENINDEX,
				RECEIPTPAYMENTLINESSIZE,
				CONFIRMBUTTONSTYLEID,
				CANCELBUTTONSTYLEID,
				ACTIONBUTTONSTYLEID,
				NORMALBUTTONSTYLEID,
				OTHERBUTTONSTYLEID,
				OVERRIDEPOSCONTROLBORDERCOLOR,
				POSCONTROLBORDERCOLOR,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.PROFILEID,
				ins.NAME,
				ins.RESOLUTION,
				ins.TERMINALTYPE,
				ins.HIDECURSOR,
				ins.DESIGNALLOWEDONPOS,
				ins.OPAQUEBACKGROUNDFORM,
				ins.OPACITY,
				ins.USEFORMBACKGROUNDIMAGE,
				ins.SCREENINDEX,
				ins.RECEIPTPAYMENTLINESSIZE,
				ins.CONFIRMBUTTONSTYLEID,
				ins.CANCELBUTTONSTYLEID,
				ins.ACTIONBUTTONSTYLEID,
				ins.NORMALBUTTONSTYLEID,
				ins.OTHERBUTTONSTYLEID,
				ins.OVERRIDEPOSCONTROLBORDERCOLOR,
				ins.POSCONTROLBORDERCOLOR,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISTILLLAYOUT]'))
begin
   drop trigger dbo.Update_POSISTILLLAYOUT
end

GO

create trigger Update_POSISTILLLAYOUT
on POSISTILLLAYOUT after insert,update, delete as

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
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSISTILLLAYOUTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				LAYOUTID,
				NAME,
				WIDTH,
				HEIGHT,
				BUTTONGRID1,
				BUTTONGRID2,
				BUTTONGRID3,
				BUTTONGRID4,
				BUTTONGRID5,
				RECEIPTID,
				TOTALID,
				CUSTOMERLAYOUTID,
				LOGOPICTUREID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.LAYOUTID,
				ins.NAME,
				ins.WIDTH,
				ins.HEIGHT,
				ins.BUTTONGRID1,
				ins.BUTTONGRID2,
				ins.BUTTONGRID3,
				ins.BUTTONGRID4,
				ins.BUTTONGRID5,
				ins.RECEIPTID,
				ins.TOTALID,
				ins.CUSTOMERLAYOUTID,
				ins.LOGOPICTUREID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSISTILLLAYOUTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				LAYOUTID,
				NAME,
				WIDTH,
				HEIGHT,
				BUTTONGRID1,
				BUTTONGRID2,
				BUTTONGRID3,
				BUTTONGRID4,
				BUTTONGRID5,
				RECEIPTID,
				TOTALID,
				CUSTOMERLAYOUTID,
				LOGOPICTUREID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.LAYOUTID,
				ins.NAME,
				ins.WIDTH,
				ins.HEIGHT,
				ins.BUTTONGRID1,
				ins.BUTTONGRID2,
				ins.BUTTONGRID3,
				ins.BUTTONGRID4,
				ins.BUTTONGRID5,
				ins.RECEIPTID,
				ins.TOTALID,
				ins.CUSTOMERLAYOUTID,
				ins.LOGOPICTUREID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSFUNCTIONALITYPROFILE]'))
begin
   drop trigger dbo.Update_POSFUNCTIONALITYPROFILE
end

GO

create trigger Update_POSFUNCTIONALITYPROFILE
on POSFUNCTIONALITYPROFILE after insert,update, delete as

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
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSFUNCTIONALITYPROFILELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				PROFILEID,
				NAME,
				AGGREGATEITEMS,
				LOGLEVEL,
				AGGREGATEPAYMENTS,
				AGGREGATEITEMSFORPRINTING,
				ALLOWSALESIFDRAWERISOPEN,
				SHOWSTAFFLISTATLOGON,
				LIMITSTAFFLISTTOSTORE,
				STAFFBARCODELOGON,
				STAFFCARDLOGON,
				MUSTKEYINPRICEIFZERO,
				MINIMUMPASSWORDLENGTH,
				ISHOSPITALITYPROFILE,
				ALWAYSASKFORPASSWORD,								
                NUMPADENTRYSTARTSINDECIMALS,
                NUMPADAMOUNTOFDECIMALS,
                SAFEDROPUSESDENOMINATION,
                SAFEDROPREVUSESDENOMINATION,
                BANKDROPUSESDENOMINATION,
                BANKDROPREVUSESDENOMINATION,
                TENDERDECLUSESDENOMINATION,
                POLLINGINTERVAL,				
                MAXIMUMPRICE,
                MAXIMUMQTY,
				OMNISUSPENSIONTYPE,
				OMNIITEMIMAGELOOKUPGROUP,
				OPENDRAWER,
				ZRPTPRINTGRANDTOTALS,
				LIMITATIONDISPLAYTYPE,
				DISPLAYLIMITATIONSTOTALSINPOS,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.PROFILEID,
				ins.NAME,
				ins.AGGREGATEITEMS,
				ins.LOGLEVEL,
				ins.AGGREGATEPAYMENTS,
				ins.AGGREGATEITEMSFORPRINTING,
				ins.ALLOWSALESIFDRAWERISOPEN,
				ins.SHOWSTAFFLISTATLOGON,
				ins.LIMITSTAFFLISTTOSTORE,
				ins.STAFFBARCODELOGON,
				ins.STAFFCARDLOGON,
				ins.MUSTKEYINPRICEIFZERO,
				ins.MINIMUMPASSWORDLENGTH,
				ins.ISHOSPITALITYPROFILE,
				ins.ALWAYSASKFORPASSWORD,
				
                ins.NUMPADENTRYSTARTSINDECIMALS,
                ins.NUMPADAMOUNTOFDECIMALS,
                ins.SAFEDROPUSESDENOMINATION,
                ins.SAFEDROPREVUSESDENOMINATION,
                ins.BANKDROPUSESDENOMINATION,
                ins.BANKDROPREVUSESDENOMINATION,
                ins.TENDERDECLUSESDENOMINATION,
                ins.POLLINGINTERVAL,
				ins.MAXIMUMPRICE,
                ins.MAXIMUMQTY,
				ins.OMNISUSPENSIONTYPE,
				ins.OMNIITEMIMAGELOOKUPGROUP,
				ins.OPENDRAWER,
				ins.ZRPTPRINTGRANDTOTALS,
				ins.LIMITATIONDISPLAYTYPE,
				ins.DISPLAYLIMITATIONSTOTALSINPOS,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSFUNCTIONALITYPROFILELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				PROFILEID,
				NAME,
				AGGREGATEITEMS,
				LOGLEVEL,
				AGGREGATEPAYMENTS,
				AGGREGATEITEMSFORPRINTING,
				ALLOWSALESIFDRAWERISOPEN,
				SHOWSTAFFLISTATLOGON,
				LIMITSTAFFLISTTOSTORE,
				STAFFBARCODELOGON,
				STAFFCARDLOGON,
				MUSTKEYINPRICEIFZERO,
				MINIMUMPASSWORDLENGTH,
				ISHOSPITALITYPROFILE,
				ALWAYSASKFORPASSWORD,
                NUMPADENTRYSTARTSINDECIMALS,
                NUMPADAMOUNTOFDECIMALS,
                SAFEDROPUSESDENOMINATION,
                SAFEDROPREVUSESDENOMINATION,
                BANKDROPUSESDENOMINATION,
                BANKDROPREVUSESDENOMINATION,
                TENDERDECLUSESDENOMINATION,
                POLLINGINTERVAL,
				MAXIMUMPRICE,
                MAXIMUMQTY,
				OMNISUSPENSIONTYPE,
				OMNIITEMIMAGELOOKUPGROUP,
				OPENDRAWER,
				ZRPTPRINTGRANDTOTALS,
				LIMITATIONDISPLAYTYPE,
				DISPLAYLIMITATIONSTOTALSINPOS,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.PROFILEID,
				ins.NAME,
				ins.AGGREGATEITEMS,
				ins.LOGLEVEL,
				ins.AGGREGATEPAYMENTS,
				ins.AGGREGATEITEMSFORPRINTING,
				ins.ALLOWSALESIFDRAWERISOPEN,
				ins.SHOWSTAFFLISTATLOGON,
				ins.LIMITSTAFFLISTTOSTORE,
				ins.STAFFBARCODELOGON,
				ins.STAFFCARDLOGON,
				ins.MUSTKEYINPRICEIFZERO,
				ins.MINIMUMPASSWORDLENGTH,
				ins.ISHOSPITALITYPROFILE,
				ins.ALWAYSASKFORPASSWORD,
                ins.NUMPADENTRYSTARTSINDECIMALS,
                ins.NUMPADAMOUNTOFDECIMALS,
                ins.SAFEDROPUSESDENOMINATION,
                ins.SAFEDROPREVUSESDENOMINATION,
                ins.BANKDROPUSESDENOMINATION,
                ins.BANKDROPREVUSESDENOMINATION,
                ins.TENDERDECLUSESDENOMINATION,
                ins.POLLINGINTERVAL,				
                ins.MAXIMUMPRICE,
                ins.MAXIMUMQTY,
				ins.OMNISUSPENSIONTYPE,
				ins.OMNIITEMIMAGELOOKUPGROUP,
				ins.OPENDRAWER,
				ins.ZRPTPRINTGRANDTOTALS,
				ins.LIMITATIONDISPLAYTYPE,
				ins.DISPLAYLIMITATIONSTOTALSINPOS,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOTENDERTYPECARDTABLE]'))
begin
   drop trigger dbo.Update_RBOTENDERTYPECARDTABLE
end

GO

create trigger Update_RBOTENDERTYPECARDTABLE
on RBOTENDERTYPECARDTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOTENDERTYPECARDTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				CARDTYPEID,
				NAME,
				CARDTYPES,
				CARDISSUER,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.CARDTYPEID,
				ins.NAME,
				ins.CARDTYPES,
				ins.CARDISSUER,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOTENDERTYPECARDTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				CARDTYPEID,
				NAME,
				CARDTYPES,
				CARDISSUER,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.CARDTYPEID,
				ins.NAME,
				ins.CARDTYPES,
				ins.CARDISSUER,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOTENDERTYPECARDNUMBERS]'))
begin
   drop trigger dbo.Update_RBOTENDERTYPECARDNUMBERS
end

GO

create trigger Update_RBOTENDERTYPECARDNUMBERS
on RBOTENDERTYPECARDNUMBERS after insert,update, delete as

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
	
	declare @action nvarchar(10)
	
	select @DeletedCount = COUNT(*) FROM DELETED
	select @InsertedCount = COUNT(*) FROM inserted
	
	begin try
		if @DeletedCount > 0 and @InsertedCount = 0
		begin
			insert into LSPOSNET_Audit.dbo.RBOTENDERTYPECARDNUMBERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				CARDTYPEID,
				CARDNUMBERFROM,
				CARDNUMBERTO,
				CARDNUMBERLENGTH,
				Action
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.CARDTYPEID,
				ins.CARDNUMBERFROM,
				ins.CARDNUMBERTO,
				ins.CARDNUMBERLENGTH,
				'Delete'
				From DELETED ins
		end
		else
		begin
			if @DeletedCount > 0
			begin
				set @action = 'Update'
			end
			else
			begin
				set @action = 'Insert'
			end
		
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOTENDERTYPECARDNUMBERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				CARDTYPEID,
				CARDNUMBERFROM,
				CARDNUMBERTO,
				CARDNUMBERLENGTH,
				Action
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.CARDTYPEID,
				ins.CARDNUMBERFROM,
				ins.CARDNUMBERTO,
				ins.CARDNUMBERLENGTH,
				@action
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_OPERATIONS]'))
begin
   drop trigger dbo.Update_OPERATIONS
end

GO

create trigger Update_OPERATIONS
on OPERATIONS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.OPERATIONSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ID,
                DESCRIPTION,
                TYPE,
                LOOKUPTYPE,
                AUDIT,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ID,
                ins.DESCRIPTION,
                ins.TYPE,
                ins.LOOKUPTYPE,
                ins.AUDIT,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.OPERATIONSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ID,
                DESCRIPTION,
                TYPE,
                LOOKUPTYPE,
                AUDIT,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ID,
                ins.DESCRIPTION,
                ins.TYPE,
                ins.LOOKUPTYPE,
                ins.AUDIT,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


-- ---------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSTRANSACTIONSERVICEPROFILE]'))
begin
   drop trigger dbo.Update_POSTRANSACTIONSERVICEPROFILE
end

GO

create trigger Update_POSTRANSACTIONSERVICEPROFILE
on POSTRANSACTIONSERVICEPROFILE after insert, update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSTRANSACTIONSERVICEPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                PROFILEID,
                NAME,
                USERNAME,
                PASSWORD,
                COMPANY,
                DOMAIN,
                AOSSERVER,
                AOSINSTANCE,
                AOSPORT,
                CONFIGURATION,
                TSCUSTOMER,
                TSSTAFF,
                TSINVENTORYLOOKUP,
                LANGUAGE,
                AXVERSION,
                CENTRALTABLESERVER,
                CENTRALTABLESERVERPORT,                
                DATAAREAID,
                ISSUEGIFTCARDOPTION,
                USEGIFTCARDS,
				USECENTRALSUSPENSION,
				TIMEOUT,
				MAXMESSAGESIZE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.PROFILEID,
                ins.NAME,
                ins.USERNAME,
                ins.PASSWORD,
                ins.COMPANY,
                ins.DOMAIN,
                ins.AOSSERVER,
                ins.AOSINSTANCE,
                ins.AOSPORT,
                ins.CONFIGURATION, 
                ins.TSCUSTOMER,
                ins.TSSTAFF,
                ins.TSINVENTORYLOOKUP,
                ins.LANGUAGE,
                ins.AXVERSION,
                ins.CENTRALTABLESERVER,
                ins.CENTRALTABLESERVERPORT,
                ins.DATAAREAID,
                ins.ISSUEGIFTCARDOPTION,
                ins.USEGIFTCARDS,
				ins.USECENTRALSUSPENSION,
				ins.TIMEOUT,
				ins.MAXMESSAGESIZE,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSTRANSACTIONSERVICEPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                PROFILEID,
                NAME,
                USERNAME,
                PASSWORD,
                COMPANY,
                DOMAIN,
                AOSSERVER,
                AOSINSTANCE,
                AOSPORT,
                CONFIGURATION,
                TSCUSTOMER,
                TSSTAFF,
                TSINVENTORYLOOKUP,
                LANGUAGE,
                AXVERSION,
                CENTRALTABLESERVER,
                CENTRALTABLESERVERPORT,
                DATAAREAID,
                ISSUEGIFTCARDOPTION,
                USEGIFTCARDS,
				USECENTRALSUSPENSION,
				TIMEOUT,
				MAXMESSAGESIZE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.PROFILEID,
                ins.NAME,
                ins.USERNAME,
                ins.PASSWORD,
                ins.COMPANY,
                ins.DOMAIN,
                ins.AOSSERVER,
                ins.AOSINSTANCE,
                ins.AOSPORT,
                ins.CONFIGURATION,     
                ins.TSCUSTOMER,
                ins.TSSTAFF,
                ins.TSINVENTORYLOOKUP,
                ins.LANGUAGE,
                ins.AXVERSION,
                ins.CENTRALTABLESERVER,
                ins.CENTRALTABLESERVERPORT,
                ins.DATAAREAID,
                ins.ISSUEGIFTCARDOPTION,
                ins.USEGIFTCARDS,				
                ins.USECENTRALSUSPENSION,
				ins.TIMEOUT,
				ins.MAXMESSAGESIZE,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
-- ---------------------------------------------------------------------------------------------------

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOTENDERTYPETABLE]'))
begin
   drop trigger dbo.Update_RBOTENDERTYPETABLE
end

GO

create trigger Update_RBOTENDERTYPETABLE
on RBOTENDERTYPETABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOTENDERTYPETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TENDERTYPEID,
				NAME,
				DEFAULTFUNCTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TENDERTYPEID,
				ins.NAME,
				ins.DEFAULTFUNCTION,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOTENDERTYPETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TENDERTYPEID,
				NAME,
				DEFAULTFUNCTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TENDERTYPEID,
				ins.NAME,
				ins.DEFAULTFUNCTION,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISFORMLAYOUT]'))
begin
   drop trigger dbo.Update_POSISFORMLAYOUT
end

GO

create trigger Update_POSISFORMLAYOUT
on POSISFORMLAYOUT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSISFORMLAYOUTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ID,
				TITLE,
				DESCRIPTION,
				UPPERCASE,
				LINECOUNTPRPAGE,
				PRINTASSLIP,
				USEWINDOWSPRINTER,
				WINDOWSPRINTERNAME,
				PRINTBEHAVIOUR,
				PROMPTQUESTION,
				WINDOWSPRINTERCONFIGURATIONID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ID,
				ins.TITLE,
				ins.DESCRIPTION,
				ins.UPPERCASE,
				ins.LINECOUNTPRPAGE,
				ins.PRINTASSLIP,
				ins.USEWINDOWSPRINTER,
				ins.WINDOWSPRINTERNAME,
				ins.PRINTBEHAVIOUR,
				ins.PROMPTQUESTION,
				ins.WINDOWSPRINTERCONFIGURATIONID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin

				insert into LSPOSNET_Audit.dbo.POSISFORMLAYOUTLog (
					AuditUserGUID, 
					AuditUserLogin,
					AuditDate,
					DATAAREAID,
					ID,
					TITLE,
					DESCRIPTION,
					UPPERCASE,
					LINECOUNTPRPAGE,
					PRINTASSLIP,
					USEWINDOWSPRINTER,
					WINDOWSPRINTERNAME,
					PRINTBEHAVIOUR,
					PROMPTQUESTION,
					WINDOWSPRINTERCONFIGURATIONID,
					Deleted
					)
				Select 
					@connectionUser, @sessionUser as AuditUserLogin, 
					GETDATE() as AuditDate,
					ins.DATAAREAID,
					ins.ID,
					ins.TITLE,
					ins.DESCRIPTION,
					ins.UPPERCASE,
					ins.LINECOUNTPRPAGE,
					ins.PRINTASSLIP,
					ins.USEWINDOWSPRINTER,
					ins.WINDOWSPRINTERNAME,
					ins.PRINTBEHAVIOUR,
					ins.PROMPTQUESTION,
					ins.WINDOWSPRINTERCONFIGURATIONID,
					0 as Deleted
					From inserted ins,deleted del

			
		end
	end try
	begin catch
	
	end catch
	
	
	--AuditUserLogin = Case 
--           when pl.AuditUserLogin = '' then u.Login
--           else pl.AuditUserLogin
--       end,
end

GO

-- ---------------------------------------------------------------------------------------------------

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISUPDATESMASTER]'))
begin
   drop trigger dbo.Update_POSISUPDATESMASTER
end

GO

create trigger Update_POSISUPDATESMASTER
on POSISUPDATESMASTER after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSISUPDATESMASTERLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				UPDATEID,
				STOREID,
				POSID,
				FILEID,
				FILECREATEDATE,
				STATUS,
				TEXT,
				HIGHPRIORITY,
				FOLDERPATH,
				POSAPPLICATIONPATH,
				USERNAME,
				NAME,
				FILEVERSION,
				FILEMODIFIEDDATE,
				COMPANY,
				DESCRIPTION,
				SCHEDULED
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.UPDATEID,
				ins.STOREID,
				ins.POSID,
				ins.FILEID,
				ins.FILECREATEDATE,
				ins.STATUS,
				ins.TEXT,
				ins.HIGHPRIORITY,
				ins.FOLDERPATH,
				ins.POSAPPLICATIONPATH,
				ins.USERNAME,
				ins.NAME,
				ins.FILEVERSION,
				ins.FILEMODIFIEDDATE,
				ins.COMPANY,
				ins.DESCRIPTION,
				ins.SCHEDULED
				From DELETED ins
		end
		else
		begin

				insert into LSPOSNET_Audit.dbo.POSISUPDATESMASTERLog (
					AuditUserGUID, 
					AuditUserLogin,
					AuditDate,
					DATAAREAID,
					UPDATEID,
					STOREID,
					POSID,
					FILEID,
					FILECREATEDATE,
					STATUS,
					TEXT,
					HIGHPRIORITY,
					FOLDERPATH,
					POSAPPLICATIONPATH,
					USERNAME,
					NAME,
					FILEVERSION,
					FILEMODIFIEDDATE,
					COMPANY,
					DESCRIPTION,
					SCHEDULED
					)
				Select 
					@connectionUser, @sessionUser as AuditUserLogin, 
					GETDATE() as AuditDate,
					ins.DATAAREAID,
					ins.UPDATEID,
					ins.STOREID,
					ins.POSID,
					ins.FILEID,
					ins.FILECREATEDATE,
					ins.STATUS,
					ins.TEXT,
					ins.HIGHPRIORITY,
					ins.FOLDERPATH,
					ins.POSAPPLICATIONPATH,
					ins.USERNAME,
					ins.NAME,
					ins.FILEVERSION,
					ins.FILEMODIFIEDDATE,
					ins.COMPANY,
					ins.DESCRIPTION,
					ins.SCHEDULED
					From inserted ins

			
		end
	end try
	begin catch
	
	end catch
	
	
	--AuditUserLogin = Case 
--           when pl.AuditUserLogin = '' then u.Login
--           else pl.AuditUserLogin
--       end,
end

GO

-- ---------------------------------------------------------------------------------------------------

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSHARDWAREPROFILE]'))
begin
   drop trigger dbo.Update_POSHARDWAREPROFILE
end

GO

create trigger Update_POSHARDWAREPROFILE
on POSHARDWAREPROFILE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSHARDWAREPROFILELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				PROFILEID,
				NAME,
				SCREENKEYBOARD,
				DRAWERREMOTE,
				DRAWERSTATUS,
				DRAWER,
				DRAWERDEVICENAME,
				DRAWEROPENTEXT,
				DRAWERDESCRIPTION,
				DISPLAYBINCONVERSION,
				DISPLAYDEVICE,
				DISPLAYDEVICENAME,
				DISPLAYDESCRIPTION,
				DISPLAYREMOTE,
				DISPLAYTOTALTEXT,
				DISPLAYBALANCETEXT,
				DISPLAYCLOSEDLINE1,
				DISPLAYCLOSEDLINE2,
				DISPLAYCHARACTERSET,
				DELAYFORLINKEDITEMS,
				SHOWPICTURE,
				MSR,
				MSRDEVICENAME,
				MSRDESCRIPTION,
				MSRAUTODISABLE,
				MSRDISABLEEVENTS,
				MSRREMOTE,
				ENTERREPEATSLASTITEM,
				STARTTRACK1,
				SEPARATOR1,
				ENDTRACK1,
				PRINTER,
				PRINTERDEVICENAME,
				PRINTBINARYCONVERSION,
				PRINTERCHARACTERSET,
				PRINTERDESCRIPTION,
				SCANNER,
				SCANNERDEVICENAME,
				SCANNERDESCRIPTION,
				SCALE,
				SCALEDEVICENAME,
				SCALEDESCRIPTION,
				MANUALINPUTALLOWED,
				KEYLOCK,
				KEYLOCKDEVICENAME,
				KEYLOCKDESCRIPTION,
				EFT,
				EFTDESCRIPTION,
				EFTSERVERNAME,
				EFTSERVERPORT,
				EFTCOMPANYID,
				EFTUSERID,
				EFTPASSWORD,
				FORECOURT,
				HOSTNAME,
				CCTV,
				CCTVPORT,
				CCTVHOSTNAME,
				CCTVCAMERA,
				EFTBATCHINCREMENTATEOS,
				RFIDSCANNERTYPE,
				RFIDDEVICENAME,
				RFIDDESCRIPTION,
				DUALDISPLAY,
				DUALDISPLAYDEVICENAME,
				DUALDISPLAYDESCRIPTION,
				DUALDISPLAYTYPE,
				DUALDISPLAYPORT,
				DUALDISPLAYRECEIPTPERCENTAGE,
				DUALDISPLAYIMAGEPATH,
				DUALDISPLAYIMAGEINTERVAL,
				DUALDISPLAYBROWSERURL,
				CASHCHANGER,
				CASHCHANGERPORTSETTINGS,
				CASHCHANGERINITSETTINGS,
				FORECOURTMANAGER,
				FORECOURTMANAGERHOSTNAME,
				FORECOURTMANAGERPORT,
				FORECOURTMANAGERPOSPORT,
				FORECOURTMANAGERScreenHeightPercentage,
				FORECOURTMANAGERControllerHostName,
				FORECOURTMANAGERLogLevel,
				FORECOURTMANAGERImplFileName,
				FORECOURTMANAGERImplFileType,
				FORECOURTMANAGERScreenExtHeightPercentage,
				FORECOURTMANAGERVolumeUnit,
				FORECOURTMANAGERCallingSound,
				FORECOURTMANAGERCallingBlink,
				FORECOURTMANAGERFuellingPointColumns,
				FISCALPRINTER,
				FISCALPRINTERCONNECTION,
				FISCALPRINTERDESCRIPTION,
				STATIONPRINTINGHOSTID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.PROFILEID,
				ins.NAME,
				ins.SCREENKEYBOARD,
				ins.DRAWERREMOTE,
				ins.DRAWERSTATUS,
				ins.DRAWER,
				ins.DRAWERDEVICENAME,
				ins.DRAWEROPENTEXT,
				ins.DRAWERDESCRIPTION,
				ins.DISPLAYBINCONVERSION,
				ins.DISPLAYDEVICE,
				ins.DISPLAYDEVICENAME,
				ins.DISPLAYDESCRIPTION,
				ins.DISPLAYREMOTE,
				ins.DISPLAYTOTALTEXT,
				ins.DISPLAYBALANCETEXT,
				ins.DISPLAYCLOSEDLINE1,
				ins.DISPLAYCLOSEDLINE2,
				ins.DISPLAYCHARACTERSET,
				ins.DELAYFORLINKEDITEMS,
				ins.SHOWPICTURE,
				ins.MSR,
				ins.MSRDEVICENAME,
				ins.MSRDESCRIPTION,
				ins.MSRAUTODISABLE,
				ins.MSRDISABLEEVENTS,
				ins.MSRREMOTE,
				ins.ENTERREPEATSLASTITEM,
				ins.STARTTRACK1,
				ins.SEPARATOR1,
				ins.ENDTRACK1,
				ins.PRINTER,
				ins.PRINTERDEVICENAME,
				ins.PRINTBINARYCONVERSION,
				ins.PRINTERCHARACTERSET,
				ins.PRINTERDESCRIPTION,
				ins.SCANNER,
				ins.SCANNERDEVICENAME,
				ins.SCANNERDESCRIPTION,
				ins.SCALE,
				ins.SCALEDEVICENAME,
				ins.SCALEDESCRIPTION,
				ins.MANUALINPUTALLOWED,
				ins.KEYLOCK,
				ins.KEYLOCKDEVICENAME,
				ins.KEYLOCKDESCRIPTION,
				ins.EFT,
				ins.EFTDESCRIPTION,
				ins.EFTSERVERNAME,
				ins.EFTSERVERPORT,
				ins.EFTCOMPANYID,
				ins.EFTUSERID,
				ins.EFTPASSWORD,
				ins.FORECOURT,
				ins.HOSTNAME,
				ins.CCTV,
				ins.CCTVPORT,
				ins.CCTVHOSTNAME,
				ins.CCTVCAMERA,
				ins.EFTBATCHINCREMENTATEOS,
				ins.RFIDSCANNERTYPE,
				ins.RFIDDEVICENAME,
				ins.RFIDDESCRIPTION,
				ins.DUALDISPLAY,
				ins.DUALDISPLAYDEVICENAME,
				ins.DUALDISPLAYDESCRIPTION,
				ins.DUALDISPLAYTYPE,
				ins.DUALDISPLAYPORT,
				ins.DUALDISPLAYRECEIPTPERCENTAGE,
				ins.DUALDISPLAYIMAGEPATH,
				ins.DUALDISPLAYIMAGEINTERVAL,
				ins.DUALDISPLAYBROWSERURL,
				ins.CASHCHANGER,
				ins.CASHCHANGERPORTSETTINGS,
				ins.CASHCHANGERINITSETTINGS,
				ins.FORECOURTMANAGER,
				ins.FORECOURTMANAGERHOSTNAME,
				ins.FORECOURTMANAGERPORT,
				ins.FORECOURTMANAGERPOSPORT,
				ins.FORECOURTMANAGERScreenHeightPercentage,
				ins.FORECOURTMANAGERControllerHostName,
				ins.FORECOURTMANAGERLogLevel,
				ins.FORECOURTMANAGERImplFileName,
				ins.FORECOURTMANAGERImplFileType,
				ins.FORECOURTMANAGERScreenExtHeightPercentage,
				ins.FORECOURTMANAGERVolumeUnit,
				ins.FORECOURTMANAGERCallingSound,
				ins.FORECOURTMANAGERCallingBlink,
				ins.FORECOURTMANAGERFuellingPointColumns,
				ins.FISCALPRINTER,
				ins.FISCALPRINTERCONNECTION,
				ins.FISCALPRINTERDESCRIPTION,
				STATIONPRINTINGHOSTID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSHARDWAREPROFILELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				PROFILEID,
				NAME,
				SCREENKEYBOARD,
				DRAWERREMOTE,
				DRAWERSTATUS,
				DRAWER,
				DRAWERDEVICENAME,
				DRAWEROPENTEXT,
				DRAWERDESCRIPTION,
				DISPLAYBINCONVERSION,
				DISPLAYDEVICE,
				DISPLAYDEVICENAME,
				DISPLAYDESCRIPTION,
				DISPLAYREMOTE,
				DISPLAYTOTALTEXT,
				DISPLAYBALANCETEXT,
				DISPLAYCLOSEDLINE1,
				DISPLAYCLOSEDLINE2,
				DISPLAYCHARACTERSET,
				DELAYFORLINKEDITEMS,
				SHOWPICTURE,
				MSR,
				MSRDEVICENAME,
				MSRDESCRIPTION,
				MSRAUTODISABLE,
				MSRDISABLEEVENTS,
				MSRREMOTE,
				ENTERREPEATSLASTITEM,
				STARTTRACK1,
				SEPARATOR1,
				ENDTRACK1,
				PRINTER,
				PRINTERDEVICENAME,
				PRINTBINARYCONVERSION,
				PRINTERCHARACTERSET,				
				PRINTERDESCRIPTION,
				SCANNER,
				SCANNERDEVICENAME,
				SCANNERDESCRIPTION,
				SCALE,
				SCALEDEVICENAME,
				SCALEDESCRIPTION,
				MANUALINPUTALLOWED,
				KEYLOCK,
				KEYLOCKDEVICENAME,
				KEYLOCKDESCRIPTION,
				EFT,
				EFTDESCRIPTION,
				EFTSERVERNAME,
				EFTSERVERPORT,
				EFTCOMPANYID,
				EFTUSERID,
				EFTPASSWORD,
				FORECOURT,
				HOSTNAME,
				CCTV,
				CCTVPORT,
				CCTVHOSTNAME,
				CCTVCAMERA,
				EFTBATCHINCREMENTATEOS,
				RFIDSCANNERTYPE,
				RFIDDEVICENAME,
				RFIDDESCRIPTION,
				DUALDISPLAY,
				DUALDISPLAYDEVICENAME,
				DUALDISPLAYDESCRIPTION,
				DUALDISPLAYTYPE,
				DUALDISPLAYPORT,
				DUALDISPLAYRECEIPTPERCENTAGE,
				DUALDISPLAYIMAGEPATH,
				DUALDISPLAYIMAGEINTERVAL,
				DUALDISPLAYBROWSERURL,
				CASHCHANGER,
				CASHCHANGERPORTSETTINGS,
				CASHCHANGERINITSETTINGS,
				FORECOURTMANAGER,
				FORECOURTMANAGERHOSTNAME,
				FORECOURTMANAGERPORT,
				FORECOURTMANAGERPOSPORT,
				FORECOURTMANAGERScreenHeightPercentage,
				FORECOURTMANAGERControllerHostName,
				FORECOURTMANAGERLogLevel,
				FORECOURTMANAGERImplFileName,
				FORECOURTMANAGERImplFileType,
				FORECOURTMANAGERScreenExtHeightPercentage,
				FORECOURTMANAGERVolumeUnit,
				FORECOURTMANAGERCallingSound,
				FORECOURTMANAGERCallingBlink,
				FORECOURTMANAGERFuellingPointColumns,
				FISCALPRINTER,
				FISCALPRINTERCONNECTION,
				FISCALPRINTERDESCRIPTION,
				STATIONPRINTINGHOSTID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.PROFILEID,
				ins.NAME,
				ins.SCREENKEYBOARD,
				ins.DRAWERREMOTE,
				ins.DRAWERSTATUS,
				ins.DRAWER,
				ins.DRAWERDEVICENAME,
				ins.DRAWEROPENTEXT,
				ins.DRAWERDESCRIPTION,
				ins.DISPLAYBINCONVERSION,
				ins.DISPLAYDEVICE,
				ins.DISPLAYDEVICENAME,
				ins.DISPLAYDESCRIPTION,
				ins.DISPLAYREMOTE,
				ins.DISPLAYTOTALTEXT,
				ins.DISPLAYBALANCETEXT,
				ins.DISPLAYCLOSEDLINE1,
				ins.DISPLAYCLOSEDLINE2,
				ins.DISPLAYCHARACTERSET,
				ins.DELAYFORLINKEDITEMS,
				ins.SHOWPICTURE,
				ins.MSR,
				ins.MSRDEVICENAME,
				ins.MSRDESCRIPTION,
				ins.MSRAUTODISABLE,
				ins.MSRDISABLEEVENTS,
				ins.MSRREMOTE,
				ins.ENTERREPEATSLASTITEM,
				ins.STARTTRACK1,
				ins.SEPARATOR1,
				ins.ENDTRACK1,
				ins.PRINTER,
				ins.PRINTERDEVICENAME,
				ins.PRINTBINARYCONVERSION,
				ins.PRINTERCHARACTERSET,
				ins.PRINTERDESCRIPTION,
				ins.SCANNER,
				ins.SCANNERDEVICENAME,
				ins.SCANNERDESCRIPTION,
				ins.SCALE,
				ins.SCALEDEVICENAME,
				ins.SCALEDESCRIPTION,
				ins.MANUALINPUTALLOWED,
				ins.KEYLOCK,
				ins.KEYLOCKDEVICENAME,
				ins.KEYLOCKDESCRIPTION,
				ins.EFT,
				ins.EFTDESCRIPTION,
				ins.EFTSERVERNAME,
				ins.EFTSERVERPORT,
				ins.EFTCOMPANYID,
				ins.EFTUSERID,
				ins.EFTPASSWORD,
				ins.FORECOURT,
				ins.HOSTNAME,
				ins.CCTV,
				ins.CCTVPORT,
				ins.CCTVHOSTNAME,
				ins.CCTVCAMERA,
				ins.EFTBATCHINCREMENTATEOS,
				ins.RFIDSCANNERTYPE,
				ins.RFIDDEVICENAME,
				ins.RFIDDESCRIPTION,
				ins.DUALDISPLAY,
				ins.DUALDISPLAYDEVICENAME,
				ins.DUALDISPLAYDESCRIPTION,
				ins.DUALDISPLAYTYPE,
				ins.DUALDISPLAYPORT,
				ins.DUALDISPLAYRECEIPTPERCENTAGE,
				ins.DUALDISPLAYIMAGEPATH,
				ins.DUALDISPLAYIMAGEINTERVAL,
				ins.DUALDISPLAYBROWSERURL,
				ins.CASHCHANGER,
				ins.CASHCHANGERPORTSETTINGS,
				ins.CASHCHANGERINITSETTINGS,
				ins.FORECOURTMANAGER,
				ins.FORECOURTMANAGERHOSTNAME,
				ins.FORECOURTMANAGERPORT,
				ins.FORECOURTMANAGERPOSPORT,
				ins.FORECOURTMANAGERScreenHeightPercentage,
				ins.FORECOURTMANAGERControllerHostName,
				ins.FORECOURTMANAGERLogLevel,
				ins.FORECOURTMANAGERImplFileName,
				ins.FORECOURTMANAGERImplFileType,
				ins.FORECOURTMANAGERScreenExtHeightPercentage,
				ins.FORECOURTMANAGERVolumeUnit,
				ins.FORECOURTMANAGERCallingSound,
				ins.FORECOURTMANAGERCallingBlink,
				ins.FORECOURTMANAGERFuellingPointColumns,
				ins.FISCALPRINTER,
				ins.FISCALPRINTERCONNECTION,
				ins.FISCALPRINTERDESCRIPTION,
				ins.STATIONPRINTINGHOSTID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CUSTTABLE]'))
begin
   drop trigger dbo.Update_CUSTTABLE
end

GO

create trigger Update_CUSTTABLE
on CUSTTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.CUSTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ACCOUNTNUM,
				NAME,
				INVOICEACCOUNT,
				CURRENCY,
				BLOCKED,
				CREDITMAX,
				LANGUAGEID,
				TAXGROUP,
				PRICEGROUP,
				LINEDISC,
				MULTILINEDISC,
				ENDDISC,
				ORGID,
				NONCHARGABLEACCOUNT,
				FIRSTNAME,
				MIDDLENAME,
				LASTNAME,
				NAMEPREFIX,
				NAMESUFFIX,
				RECEIPTOPTION,
				RECEIPTEMAIL,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ACCOUNTNUM,
				ins.NAME,
				ins.INVOICEACCOUNT,
				ins.CURRENCY,
				ins.BLOCKED,
				ins.CREDITMAX,
				ins.LANGUAGEID,
				ins.TAXGROUP,
				ins.PRICEGROUP,
				ins.LINEDISC,
				ins.MULTILINEDISC,
				ins.ENDDISC,
				ins.ORGID,
				ins.NONCHARGABLEACCOUNT,
				ins.FIRSTNAME,
				ins.MIDDLENAME,
				ins.LASTNAME,
				ins.NAMEPREFIX,
				ins.NAMESUFFIX,
				ins.RECEIPTOPTION,
				ins.RECEIPTEMAIL,
				1 as Deleted
				From DELETED ins

		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.CUSTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ACCOUNTNUM,
				NAME,
				INVOICEACCOUNT,
				CURRENCY,
				BLOCKED,
				CREDITMAX,
				LANGUAGEID,
				TAXGROUP,
				PRICEGROUP,
				LINEDISC,
				MULTILINEDISC,
				ENDDISC,
				ORGID,
				NONCHARGABLEACCOUNT,
				FIRSTNAME,
				MIDDLENAME,
				LASTNAME,
				NAMEPREFIX,
				NAMESUFFIX,
				RECEIPTOPTION,
				RECEIPTEMAIL,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ACCOUNTNUM,
				ins.NAME,
				ins.INVOICEACCOUNT,
				ins.CURRENCY,
				ins.BLOCKED,
				ins.CREDITMAX,
				ins.LANGUAGEID,
				ins.TAXGROUP,
				ins.PRICEGROUP,
				ins.LINEDISC,
				ins.MULTILINEDISC,
				ins.ENDDISC,
				ins.ORGID,
				ins.NONCHARGABLEACCOUNT,
				ins.FIRSTNAME,
				ins.MIDDLENAME,
				ins.LASTNAME,
				ins.NAMEPREFIX,
				ins.NAMESUFFIX,
				ins.RECEIPTOPTION,
				ins.RECEIPTEMAIL,
				ins.Deleted
				From inserted ins

		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CUSTOMERADDRESS]'))
begin
   drop trigger dbo.Update_CUSTOMERADDRESS
end

GO

create trigger Update_CUSTOMERADDRESS
on CUSTOMERADDRESS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.CUSTOMERADDRESSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ACCOUNTNUM,
				ADDRESS,
				COUNTRY,
				ZIPCODE,
				STATE,
				COUNTY,
				CITY,
				STREET,
				TAXGROUP,
				ADDRESSTYPE,
				ADDRESSFORMAT,
				PHONE,
				CELLULARPHONE,
				EMAIL,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ACCOUNTNUM,
				ins.ADDRESS,
				ins.COUNTRY,
				ins.ZIPCODE,
				ins.STATE,
				ins.COUNTY,
				ins.CITY,
				ins.STREET,
				ins.TAXGROUP,
				ins.ADDRESSTYPE,
				ins.ADDRESSFORMAT,
				ins.PHONE,
				ins.CELLULARPHONE,
				ins.EMAIL,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.CUSTOMERADDRESSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ACCOUNTNUM,
				ADDRESS,
				COUNTRY,
				ZIPCODE,
				STATE,
				COUNTY,
				CITY,
				STREET,
				TAXGROUP,
				ADDRESSTYPE,
				ADDRESSFORMAT,
				PHONE,
				CELLULARPHONE,
				EMAIL,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ACCOUNTNUM,
				ins.ADDRESS,
				ins.COUNTRY,
				ins.ZIPCODE,
				ins.STATE,
				ins.COUNTY,
				ins.CITY,
				ins.STREET,
				ins.TAXGROUP,
				ins.ADDRESSTYPE,
				ins.ADDRESSFORMAT,
				ins.PHONE,
				ins.CELLULARPHONE,
				ins.EMAIL,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO
-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTAFFTABLE]'))
begin
   drop trigger dbo.Update_RBOSTAFFTABLE
end

GO

create trigger Update_RBOSTAFFTABLE
on RBOSTAFFTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTAFFTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				STAFFID,
				NAME,
				STOREID,
				PASSWORD,
				CHANGEPASSWORD,
				ALLOWTRANSACTIONVOIDING,
				MANAGERPRIVILEGES,
				ALLOWXREPORTPRINTING,
				ALLOWTENDERDECLARATION,
				ALLOWFLOATINGDECLARATION,
				PRICEOVERRIDE,
				MAXDISCOUNTPCT,
				ALLOWCHANGENOVOID,
				ALLOWTRANSACTIONSUSPENSION,
				ALLOWOPENDRAWERONLY,
				FIRSTNAME,
				LASTNAME,
				EMPLOYMENTTYPE,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				DEL_PHONELOCAL,
				PHONEWORK,
				BLOCKED,
				BLOCKINGDATE,
				LEFTHANDED,
				PERMISSIONGROUPID,
				ZREPORTID,
				NAMEONRECEIPT,
				PAYROLLNUMBER,
				CONTINUEONTSERRORS,
				PHONEHOME,
				MAXTOTALDISCOUNTPCT,
				LAYOUTID,
				VISUALPROFILE,
				OPERATORCULTURE,
				MAXLINEDISCOUNTAMOUNT,
				MAXTOTALDISCOUNTAMOUNT,
				MAXLINERETURNAMOUNT,
				MAXTOTALRETURNAMOUNT,
				KEYBOARDCODE,
				LAYOUTNAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.STAFFID,
				ins.NAME,
				ins.STOREID,
				ins.PASSWORD,
				ins.CHANGEPASSWORD,
				ins.ALLOWTRANSACTIONVOIDING,
				ins.MANAGERPRIVILEGES,
				ins.ALLOWXREPORTPRINTING,
				ins.ALLOWTENDERDECLARATION,
				ins.ALLOWFLOATINGDECLARATION,
				ins.PRICEOVERRIDE,
				ins.MAXDISCOUNTPCT,
				ins.ALLOWCHANGENOVOID,
				ins.ALLOWTRANSACTIONSUSPENSION,
				ins.ALLOWOPENDRAWERONLY,
				ins.FIRSTNAME,
				ins.LASTNAME,
				ins.EMPLOYMENTTYPE,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.DEL_PHONELOCAL,
				ins.PHONEWORK,
				ins.BLOCKED,
				ins.BLOCKINGDATE,
				ins.LEFTHANDED,
				ins.PERMISSIONGROUPID,
				ins.ZREPORTID,
				ins.NAMEONRECEIPT,
				ins.PAYROLLNUMBER,
				ins.CONTINUEONTSERRORS,
				ins.PHONEHOME,
				ins.MAXTOTALDISCOUNTPCT,
				ins.LAYOUTID,
				ins.VISUALPROFILE,
				ins.OPERATORCULTURE,
				ins.MAXLINEDISCOUNTAMOUNT,
				ins.MAXTOTALDISCOUNTAMOUNT,
				ins.MAXLINERETURNAMOUNT,
				ins.MAXTOTALRETURNAMOUNT,
				ins.KEYBOARDCODE,
				ins.LAYOUTNAME,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTAFFTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				STAFFID,
				NAME,
				STOREID,
				PASSWORD,
				CHANGEPASSWORD,
				ALLOWTRANSACTIONVOIDING,
				MANAGERPRIVILEGES,
				ALLOWXREPORTPRINTING,
				ALLOWTENDERDECLARATION,
				ALLOWFLOATINGDECLARATION,
				PRICEOVERRIDE,
				MAXDISCOUNTPCT,
				ALLOWCHANGENOVOID,
				ALLOWTRANSACTIONSUSPENSION,
				ALLOWOPENDRAWERONLY,
				FIRSTNAME,
				LASTNAME,
				EMPLOYMENTTYPE,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				DEL_PHONELOCAL,
				PHONEWORK,
				BLOCKED,
				BLOCKINGDATE,
				LEFTHANDED,
				PERMISSIONGROUPID,
				ZREPORTID,
				NAMEONRECEIPT,
				PAYROLLNUMBER,
				CONTINUEONTSERRORS,
				PHONEHOME,
				MAXTOTALDISCOUNTPCT,
				LAYOUTID,
				VISUALPROFILE,
				OPERATORCULTURE,
				MAXLINEDISCOUNTAMOUNT,
				MAXTOTALDISCOUNTAMOUNT,
				MAXLINERETURNAMOUNT,
				MAXTOTALRETURNAMOUNT,
				KEYBOARDCODE,
				LAYOUTNAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.STAFFID,
				ins.NAME,
				ins.STOREID,
				ins.PASSWORD,
				ins.CHANGEPASSWORD,
				ins.ALLOWTRANSACTIONVOIDING,
				ins.MANAGERPRIVILEGES,
				ins.ALLOWXREPORTPRINTING,
				ins.ALLOWTENDERDECLARATION,
				ins.ALLOWFLOATINGDECLARATION,
				ins.PRICEOVERRIDE,
				ins.MAXDISCOUNTPCT,
				ins.ALLOWCHANGENOVOID,
				ins.ALLOWTRANSACTIONSUSPENSION,
				ins.ALLOWOPENDRAWERONLY,
				ins.FIRSTNAME,
				ins.LASTNAME,
				ins.EMPLOYMENTTYPE,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.DEL_PHONELOCAL,
				ins.PHONEWORK,
				ins.BLOCKED,
				ins.BLOCKINGDATE,
				ins.LEFTHANDED,
				ins.PERMISSIONGROUPID,
				ins.ZREPORTID,
				ins.NAMEONRECEIPT,
				ins.PAYROLLNUMBER,
				ins.CONTINUEONTSERRORS,
				ins.PHONEHOME,
				ins.MAXTOTALDISCOUNTPCT,
				ins.LAYOUTID,
				ins.VISUALPROFILE,
				ins.OPERATORCULTURE,
				ins.MAXLINEDISCOUNTAMOUNT,
				ins.MAXTOTALDISCOUNTAMOUNT,
				ins.MAXLINERETURNAMOUNT,
				ins.MAXTOTALRETURNAMOUNT,
				ins.KEYBOARDCODE,
				ins.LAYOUTNAME,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISTENDERRESTRICTIONS]'))
begin
   drop trigger dbo.Update_POSISTENDERRESTRICTIONS
end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PAYMENTLIMITATIONS]'))
begin
   drop trigger dbo.Update_PAYMENTLIMITATIONS
end

GO

create trigger Update_PAYMENTLIMITATIONS
on PAYMENTLIMITATIONS after insert,update, delete as

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
	
	declare @action nvarchar(10)
	
	select @DeletedCount = COUNT(*) FROM DELETED
	select @InsertedCount = COUNT(*) FROM inserted
	
	begin try
		if @DeletedCount > 0 and @InsertedCount = 0
		begin
			insert into LSPOSNET_Audit.dbo.PAYMENTLIMITATIONSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				MASTERID,				
				TENDERID,
				RESTRICTIONCODE,
				RELATIONMASTERID,
				TYPE,
				INCLUDE,
				TAXEXEMPT,
				Action
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,				
				ins.MASTERID,
				ins.TENDERID,
				ins.RESTRICTIONCODE,
				ins.RELATIONMASTERID,
				ins.TYPE,
				ins.INCLUDE,
				ins.TAXEXEMPT,
				'Delete'
				From DELETED ins
		end
		else
		begin
			if @DeletedCount > 0
			begin
				set @action = 'Update'
			end
			else
			begin
				set @action = 'Insert'
			end
		
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PAYMENTLIMITATIONSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				MASTERID,
				TENDERID,
				RESTRICTIONCODE,
				RELATIONMASTERID,
				TYPE,
				INCLUDE,
				TAXEXEMPT,
				Action
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.MASTERID,
				ins.TENDERID,
				ins.RESTRICTIONCODE,
				ins.RELATIONMASTERID,
				ins.TYPE,
				ins.INCLUDE,
				ins.TAXEXEMPT,
				@action
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTORETENDERTYPETABLE]'))
begin
   drop trigger dbo.Update_RBOSTORETENDERTYPETABLE
end

GO

create trigger Update_RBOSTORETENDERTYPETABLE
on RBOSTORETENDERTYPETABLE after insert,update, delete as

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
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTORETENDERTYPETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				TENDERTYPEID,
				NAME,
				FUNCTION_,
				COUNTINGREQUIRED,
				CARDORACCOUNTNUM,
				LINENUMINTRANSACTION,
				CHANGETENDERID,
				ABOVEMINIMUMTENDERID,
				MINIMUMCHANGEAMOUNT,
				ROUNDINGMETHOD,
				ROUNDING,
				MINIMUMAMOUNTENTERED,
				MAXIMUMAMOUNTENTERED,
				MINIMUMAMOUNTALLOWED,
				MAXIMUMAMOUNTALLOWED,
				MANAGERKEYCONTROL, -- 20
				KEYBOARDENTRYALLOWED,
				ALLOWOVERTENDER,
				MAXIMUMOVERTENDERAMOUNT,
				ALLOWUNDERTENDER,
				UNDERTENDERAMOUNT,
				OPENDRAWER,
				ENDORSECHECK,
				ASKFORDATE,
				SEEKAUTHORIZATION,
				PRINTSEPARATEINVOICE,
				FRONTOFCHECK,
				KEYBOARDENTRYREQUIRED,
				PAYACCOUNTBILL,
				MARKINGONLY,
				FOREIGNCURRENCY,
				ACCOUNTTYPE,
				ACCOUNTRELATION,
				DIFFERENCEACCOUNT,
				ALLOWFLOAT,
				ENDORSMENTLINE1,
				ENDORSMENTLINE2,
				CHECKPAYEE,
				SLIPBACKINPRINTER,
				SLIPFRONTINPRINTER,
				ASKFORCARDORACCOUNT,
				INVOICEINPRINTER,
				CHANGELINEONRECEIPT,
				POSCOUNTENTRIES,
				TAKENTOBANK,
				MULTIPLYINTENDEROPERATIONS,
				COMPRESSPAYMENTENTRIES,
				MAYBEUSED,
				ALLOWRETURNNEGATIVE,
				DATAAREAID,
				POSOPERATION,
				TAKENTOSAFE,
				MAXRECOUNT,
				MAXCOUNTINGDIFFERENCE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.TENDERTYPEID,
				ins.NAME,
				ins.FUNCTION_,
				ins.COUNTINGREQUIRED,
				ins.CARDORACCOUNTNUM,
				ins.LINENUMINTRANSACTION,
				ins.CHANGETENDERID,
				ins.ABOVEMINIMUMTENDERID,
				ins.MINIMUMCHANGEAMOUNT,
				ins.ROUNDINGMETHOD,
				ins.ROUNDING,
				ins.MINIMUMAMOUNTENTERED,
				ins.MAXIMUMAMOUNTENTERED,
				ins.MINIMUMAMOUNTALLOWED,
				ins.MAXIMUMAMOUNTALLOWED,
				ins.MANAGERKEYCONTROL, --20
				ins.KEYBOARDENTRYALLOWED,
				ins.ALLOWOVERTENDER,
				ins.MAXIMUMOVERTENDERAMOUNT,
				ins.ALLOWUNDERTENDER,
				ins.UNDERTENDERAMOUNT,
				ins.OPENDRAWER,
				ins.ENDORSECHECK,
				ins.ASKFORDATE,
				ins.SEEKAUTHORIZATION,
				ins.PRINTSEPARATEINVOICE,
				ins.FRONTOFCHECK,
				ins.KEYBOARDENTRYREQUIRED,
				ins.PAYACCOUNTBILL,
				ins.MARKINGONLY,
				ins.FOREIGNCURRENCY,
				ins.ACCOUNTTYPE,
				ins.ACCOUNTRELATION,
				ins.DIFFERENCEACCOUNT,
				ins.ALLOWFLOAT,
				ins.ENDORSMENTLINE1,
				ins.ENDORSMENTLINE2,
				ins.CHECKPAYEE,
				ins.SLIPBACKINPRINTER,
				ins.SLIPFRONTINPRINTER,
				ins.ASKFORCARDORACCOUNT,
				ins.INVOICEINPRINTER,
				ins.CHANGELINEONRECEIPT,
				ins.POSCOUNTENTRIES,
				ins.TAKENTOBANK,
				ins.MULTIPLYINTENDEROPERATIONS,
				ins.COMPRESSPAYMENTENTRIES,
				ins.MAYBEUSED,
				ins.ALLOWRETURNNEGATIVE,
				ins.DATAAREAID,
				ins.POSOPERATION,
				ins.TAKENTOSAFE,
				ins.MAXRECOUNT,
				ins.MAXCOUNTINGDIFFERENCE,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTORETENDERTYPETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				TENDERTYPEID,
				NAME,
				FUNCTION_,
				COUNTINGREQUIRED,
				CARDORACCOUNTNUM,
				LINENUMINTRANSACTION,
				CHANGETENDERID,
				ABOVEMINIMUMTENDERID,
				MINIMUMCHANGEAMOUNT,
				ROUNDINGMETHOD,
				ROUNDING,
				MINIMUMAMOUNTENTERED,
				MAXIMUMAMOUNTENTERED,
				MINIMUMAMOUNTALLOWED,
				MAXIMUMAMOUNTALLOWED,
				MANAGERKEYCONTROL, -- 20
				KEYBOARDENTRYALLOWED,
				ALLOWOVERTENDER,
				MAXIMUMOVERTENDERAMOUNT,
				ALLOWUNDERTENDER,
				UNDERTENDERAMOUNT,
				OPENDRAWER,
				ENDORSECHECK,
				ASKFORDATE,
				SEEKAUTHORIZATION,
				PRINTSEPARATEINVOICE,
				FRONTOFCHECK, --30
				KEYBOARDENTRYREQUIRED,
				PAYACCOUNTBILL,
				MARKINGONLY,
				FOREIGNCURRENCY,
				ACCOUNTTYPE,
				ACCOUNTRELATION,
				DIFFERENCEACCOUNT,
				ALLOWFLOAT,
				ENDORSMENTLINE1,
				ENDORSMENTLINE2,
				CHECKPAYEE,
				SLIPBACKINPRINTER,
				SLIPFRONTINPRINTER,
				ASKFORCARDORACCOUNT,
				INVOICEINPRINTER,
				CHANGELINEONRECEIPT,
				POSCOUNTENTRIES,
				TAKENTOBANK,
				MULTIPLYINTENDEROPERATIONS,
				COMPRESSPAYMENTENTRIES,
				MAYBEUSED,
				ALLOWRETURNNEGATIVE,
				DATAAREAID,
				POSOPERATION,
				TAKENTOSAFE,
				MAXRECOUNT,
				MAXCOUNTINGDIFFERENCE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.TENDERTYPEID,
				ins.NAME,
				ins.FUNCTION_,
				ins.COUNTINGREQUIRED,
				ins.CARDORACCOUNTNUM,
				ins.LINENUMINTRANSACTION,
				ins.CHANGETENDERID,
				ins.ABOVEMINIMUMTENDERID,
				ins.MINIMUMCHANGEAMOUNT,
				ins.ROUNDINGMETHOD,
				ins.ROUNDING,
				ins.MINIMUMAMOUNTENTERED,
				ins.MAXIMUMAMOUNTENTERED,
				ins.MINIMUMAMOUNTALLOWED,
				ins.MAXIMUMAMOUNTALLOWED,
				ins.MANAGERKEYCONTROL, -- 20
				ins.KEYBOARDENTRYALLOWED,
				ins.ALLOWOVERTENDER,
				ins.MAXIMUMOVERTENDERAMOUNT,
				ins.ALLOWUNDERTENDER,
				ins.UNDERTENDERAMOUNT,
				ins.OPENDRAWER,
				ins.ENDORSECHECK,
				ins.ASKFORDATE,
				ins.SEEKAUTHORIZATION,
				ins.PRINTSEPARATEINVOICE, --30
				ins.FRONTOFCHECK,
				ins.KEYBOARDENTRYREQUIRED,
				ins.PAYACCOUNTBILL,
				ins.MARKINGONLY,
				ins.FOREIGNCURRENCY,
				ins.ACCOUNTTYPE,
				ins.ACCOUNTRELATION,
				ins.DIFFERENCEACCOUNT,
				ins.ALLOWFLOAT,
				ins.ENDORSMENTLINE1,
				ins.ENDORSMENTLINE2,
				ins.CHECKPAYEE,
				ins.SLIPBACKINPRINTER,
				ins.SLIPFRONTINPRINTER,
				ins.ASKFORCARDORACCOUNT,
				ins.INVOICEINPRINTER,
				ins.CHANGELINEONRECEIPT,
				ins.POSCOUNTENTRIES,
				ins.TAKENTOBANK,
				ins.MULTIPLYINTENDEROPERATIONS,
				ins.COMPRESSPAYMENTENTRIES,
				ins.MAYBEUSED,
				ins.ALLOWRETURNNEGATIVE,
				ins.DATAAREAID,
				ins.POSOPERATION,
				ins.TAKENTOSAFE,
				ins.MAXRECOUNT,
				ins.MAXCOUNTINGDIFFERENCE,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTORETENDERTYPECARDTABLE]'))
begin
   drop trigger dbo.Update_RBOSTORETENDERTYPECARDTABLE
end

GO

create trigger Update_RBOSTORETENDERTYPECARDTABLE
on RBOSTORETENDERTYPECARDTABLE after insert,update, delete as

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
	
	declare @action nvarchar(10)
	
	
	
	begin try
		if @DeletedCount > 0 and @InsertedCount = 0
		begin
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTORETENDERTYPECARDTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				TENDERTYPEID,
				CARDTYPEID,
				ACTION,
				NAME,
				ACCOUNTTYPE,
				ACCOUNTRELATION,
				COUNTINGREQUIRED,
				DIFFERENCEACCOUNT,
				TAKENTOBANK,
				ALLOWCHANGE,
				MANUALAUTHORIZATION,
				CARDNUMBERSWIPED,
				SAMECARDALLOWED,
				MARKINGONLY,
				CURRENCYONTOTALSCODE,
				CURRENCYCODE,
				DATAAREAID,
				DIFFERENCEACCOUNTFORBIGDI20017,
				MAXNORMALDIFFERENCEAMOUNT,
				ENTERFLEETINFO,
				CARDFEE,
				CARDFEEACCOUNT,
				CHECKMODULUS,
				CHECKEXPIREDDATE,
				PROCESSLOCALLY,
				ALLOWMANUALINPUT,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.TENDERTYPEID,
				ins.CARDTYPEID,
				'Delete',
				ins.NAME,
				ins.ACCOUNTTYPE,
				ins.ACCOUNTRELATION,
				ins.COUNTINGREQUIRED,
				ins.DIFFERENCEACCOUNT,
				ins.TAKENTOBANK,
				ins.ALLOWCHANGE,
				ins.MANUALAUTHORIZATION,
				ins.CARDNUMBERSWIPED,
				ins.SAMECARDALLOWED,
				ins.MARKINGONLY,
				ins.CURRENCYONTOTALSCODE,
				ins.CURRENCYCODE,
				ins.DATAAREAID,
				ins.DIFFERENCEACCOUNTFORBIGDI20017,
				ins.MAXNORMALDIFFERENCEAMOUNT,
				ins.ENTERFLEETINFO,
				ins.CARDFEE,
				ins.CARDFEEACCOUNT,
				ins.CHECKMODULUS,
				ins.CHECKEXPIREDDATE,
				ins.PROCESSLOCALLY,
				ins.ALLOWMANUALINPUT,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			if @DeletedCount > 0
			begin
				set @action = 'Update'
			end
			else
			begin
				set @action = 'Insert'
			end
		
			-- If we got here then we are inserting new or updating existing
			insert into LSPOSNET_Audit.dbo.RBOSTORETENDERTYPECARDTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				TENDERTYPEID,
				CARDTYPEID,
				ACTION,
				NAME,
				ACCOUNTTYPE,
				ACCOUNTRELATION,
				COUNTINGREQUIRED,
				DIFFERENCEACCOUNT,
				TAKENTOBANK,
				ALLOWCHANGE,
				MANUALAUTHORIZATION,
				CARDNUMBERSWIPED,
				SAMECARDALLOWED,
				MARKINGONLY,
				CURRENCYONTOTALSCODE,
				CURRENCYCODE,
				DATAAREAID,
				DIFFERENCEACCOUNTFORBIGDI20017,
				MAXNORMALDIFFERENCEAMOUNT,
				ENTERFLEETINFO,
				CARDFEE,
				CARDFEEACCOUNT,
				CHECKMODULUS,
				CHECKEXPIREDDATE,
				PROCESSLOCALLY,
				ALLOWMANUALINPUT,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.TENDERTYPEID,
				ins.CARDTYPEID,
				@action,
				ins.NAME,
				ins.ACCOUNTTYPE,
				ins.ACCOUNTRELATION,
				ins.COUNTINGREQUIRED,
				ins.DIFFERENCEACCOUNT,
				ins.TAKENTOBANK,
				ins.ALLOWCHANGE,
				ins.MANUALAUTHORIZATION,
				ins.CARDNUMBERSWIPED,
				ins.SAMECARDALLOWED,
				ins.MARKINGONLY,
				ins.CURRENCYONTOTALSCODE,
				ins.CURRENCYCODE,
				ins.DATAAREAID,
				ins.DIFFERENCEACCOUNTFORBIGDI20017,
				ins.MAXNORMALDIFFERENCEAMOUNT,
				ins.ENTERFLEETINFO,
				ins.CARDFEE,
				ins.CARDFEEACCOUNT,
				ins.CHECKMODULUS,
				ins.CHECKEXPIREDDATE,
				ins.PROCESSLOCALLY,
				ins.ALLOWMANUALINPUT,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTORETABLE]'))
begin
   drop trigger dbo.Update_RBOSTORETABLE
end

GO

create trigger Update_RBOSTORETABLE
on RBOSTORETABLE after insert,update, delete as

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
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTORETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				NAME,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				PHONE,
				MANAGERID,
				OPENFROM,
				OPENTO,
				DIMENSION,
				DIMENSION2_,
				DIMENSION3_,
				INVENTLOCATION,
				STATEMENTMETHOD,
				ONESTATEMENTPERDAY,
				CURRENCY,
				CLOSINGMETHOD,
				ROUNDINGACCOUNT,
				MAXIMUMPOSTINGDIFFERENCE,
				MAXROUNDINGAMOUNT,
				MAXSHIFTDIFFERENCEAMOUNT,
				MAXTRANSACTIONDIFFERENCEAMOUNT,
				STATEMENTNUMSEQ,
				FUNCTIONALITYPROFILE,
				CREATELABELSFORZEROPRICE,
				TERMINALNUMSEQ,
				STAFFNUMSEQ,
				ITEMNUMSEQ,
				DISCOUNTOFFERNUMSEQ,
				MIXANDMATCHNUMSEQ,
				MULTIBUYDISCOUNTNUMSEQ,
				INVENTORYLOOKUP,
				REMOVEADDTENDER,
				TENDERDECLARATIONCALCULATION,
				MAXIMUMTEXTLENGTHONRECEIPT,
				NUMBEROFTOPORBOTTOMLINES,
				ITEMIDONRECEIPT,
				SERVICECHARGEPCT,
				INCOMEEXEPENSEACCOUNT,
				SERVICECHARGEPROMPT,
				STATEMENTVOUCHERNUMSEQ,
				TAXGROUP,
				ROUNDINGTAXACCOUNT,
				MAXROUNDINGTAXAMOUNT,
				CULTURENAME,
				LAYOUTID,
				DATAAREAID,
				SQLSERVERNAME,
				DATABASENAME,
				USERNAME,
				PASSWORD,
				DEFAULTCUSTACCOUNT,
				USEDEFAULTCUSTACCOUNT,
				WINDOWSAUTHENTICATION,
				HIDETRAININGMODE,
				USETAXGROUPFROM,
				SUSPENDALLOWEOD,
				KEYBOARDCODE,
				LAYOUTNAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.NAME,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.PHONE,
				ins.MANAGERID,
				ins.OPENFROM,
				ins.OPENTO,
				ins.DIMENSION,
				ins.DIMENSION2_,
				ins.DIMENSION3_,
				ins.INVENTLOCATION,
				ins.STATEMENTMETHOD,
				ins.ONESTATEMENTPERDAY,
				ins.CURRENCY,
				ins.CLOSINGMETHOD,
				ins.ROUNDINGACCOUNT,
				ins.MAXIMUMPOSTINGDIFFERENCE,
				ins.MAXROUNDINGAMOUNT,
				ins.MAXSHIFTDIFFERENCEAMOUNT,
				ins.MAXTRANSACTIONDIFFERENCEAMOUNT,
				ins.STATEMENTNUMSEQ,
				ins.FUNCTIONALITYPROFILE,
				ins.CREATELABELSFORZEROPRICE,
				ins.TERMINALNUMSEQ,
				ins.STAFFNUMSEQ,
				ins.ITEMNUMSEQ,
				ins.DISCOUNTOFFERNUMSEQ,
				ins.MIXANDMATCHNUMSEQ,
				ins.MULTIBUYDISCOUNTNUMSEQ,
				ins.INVENTORYLOOKUP,
				ins.REMOVEADDTENDER,
				ins.TENDERDECLARATIONCALCULATION,
				ins.MAXIMUMTEXTLENGTHONRECEIPT,
				ins.NUMBEROFTOPORBOTTOMLINES,
				ins.ITEMIDONRECEIPT,
				ins.SERVICECHARGEPCT,
				ins.INCOMEEXEPENSEACCOUNT,
				ins.SERVICECHARGEPROMPT,
				ins.STATEMENTVOUCHERNUMSEQ,
				ins.TAXGROUP,
				ins.ROUNDINGTAXACCOUNT,
				ins.MAXROUNDINGTAXAMOUNT,
				ins.CULTURENAME,
				ins.LAYOUTID,
				ins.DATAAREAID,
				ins.SQLSERVERNAME,
				ins.DATABASENAME,
				ins.USERNAME,
				ins.PASSWORD,
				ins.DEFAULTCUSTACCOUNT,
				ins.USEDEFAULTCUSTACCOUNT,
				ins.WINDOWSAUTHENTICATION,
				ins.HIDETRAININGMODE,
				ins.USETAXGROUPFROM,
				ins.SUSPENDALLOWEOD,
				ins.KEYBOARDCODE,
				ins.LAYOUTNAME,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTORETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				NAME,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				PHONE,
				MANAGERID,
				OPENFROM,
				OPENTO,
				DIMENSION,
				DIMENSION2_,
				DIMENSION3_,
				INVENTLOCATION,
				STATEMENTMETHOD,
				ONESTATEMENTPERDAY,
				CURRENCY,
				CLOSINGMETHOD,
				ROUNDINGACCOUNT,
				MAXIMUMPOSTINGDIFFERENCE,
				MAXROUNDINGAMOUNT,
				MAXSHIFTDIFFERENCEAMOUNT,
				MAXTRANSACTIONDIFFERENCEAMOUNT,
				STATEMENTNUMSEQ,
				FUNCTIONALITYPROFILE,
				CREATELABELSFORZEROPRICE,
				TERMINALNUMSEQ,
				STAFFNUMSEQ,
				ITEMNUMSEQ,
				DISCOUNTOFFERNUMSEQ,
				MIXANDMATCHNUMSEQ,
				MULTIBUYDISCOUNTNUMSEQ,
				INVENTORYLOOKUP,
				REMOVEADDTENDER,
				TENDERDECLARATIONCALCULATION,
				MAXIMUMTEXTLENGTHONRECEIPT,
				NUMBEROFTOPORBOTTOMLINES,
				ITEMIDONRECEIPT,
				SERVICECHARGEPCT,
				INCOMEEXEPENSEACCOUNT,
				SERVICECHARGEPROMPT,
				STATEMENTVOUCHERNUMSEQ,
				TAXGROUP,
				ROUNDINGTAXACCOUNT,
				MAXROUNDINGTAXAMOUNT,
				CULTURENAME,
				LAYOUTID,
				DATAAREAID,
				SQLSERVERNAME,
				DATABASENAME,
				USERNAME,
				PASSWORD,
				DEFAULTCUSTACCOUNT,
				USEDEFAULTCUSTACCOUNT,
				WINDOWSAUTHENTICATION,
				HIDETRAININGMODE,
				USETAXGROUPFROM,
				SUSPENDALLOWEOD,
				KEYBOARDCODE,
				LAYOUTNAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.NAME,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.PHONE,
				ins.MANAGERID,
				ins.OPENFROM,
				ins.OPENTO,
				ins.DIMENSION,
				ins.DIMENSION2_,
				ins.DIMENSION3_,
				ins.INVENTLOCATION,
				ins.STATEMENTMETHOD,
				ins.ONESTATEMENTPERDAY,
				ins.CURRENCY,
				ins.CLOSINGMETHOD,
				ins.ROUNDINGACCOUNT,
				ins.MAXIMUMPOSTINGDIFFERENCE,
				ins.MAXROUNDINGAMOUNT,
				ins.MAXSHIFTDIFFERENCEAMOUNT,
				ins.MAXTRANSACTIONDIFFERENCEAMOUNT,
				ins.STATEMENTNUMSEQ,
				ins.FUNCTIONALITYPROFILE,
				ins.CREATELABELSFORZEROPRICE,
				ins.TERMINALNUMSEQ,
				ins.STAFFNUMSEQ,
				ins.ITEMNUMSEQ,
				ins.DISCOUNTOFFERNUMSEQ,
				ins.MIXANDMATCHNUMSEQ,
				ins.MULTIBUYDISCOUNTNUMSEQ,
				ins.INVENTORYLOOKUP,
				ins.REMOVEADDTENDER,
				ins.TENDERDECLARATIONCALCULATION,
				ins.MAXIMUMTEXTLENGTHONRECEIPT,
				ins.NUMBEROFTOPORBOTTOMLINES,
				ins.ITEMIDONRECEIPT,
				ins.SERVICECHARGEPCT,
				ins.INCOMEEXEPENSEACCOUNT,
				ins.SERVICECHARGEPROMPT,
				ins.STATEMENTVOUCHERNUMSEQ,
				ins.TAXGROUP,
				ins.ROUNDINGTAXACCOUNT,
				ins.MAXROUNDINGTAXAMOUNT,
				ins.CULTURENAME,
				ins.LAYOUTID,
				ins.DATAAREAID,
				ins.SQLSERVERNAME,
				ins.DATABASENAME,
				ins.USERNAME,
				ins.PASSWORD,
				ins.DEFAULTCUSTACCOUNT,
				ins.USEDEFAULTCUSTACCOUNT,
				ins.WINDOWSAUTHENTICATION,
				ins.HIDETRAININGMODE,
				ins.USETAXGROUPFROM,
				ins.SUSPENDALLOWEOD,
				ins.KEYBOARDCODE,
				ins.LAYOUTNAME,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOTERMINALTABLE]'))
begin
   drop trigger dbo.Update_RBOTERMINALTABLE
end

GO

create trigger Update_RBOTERMINALTABLE
on RBOTERMINALTABLE after insert,update, delete as

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
			-- If we got here then we are deleting
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOTERMINALTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				TERMINALID,
				NAME,--5
				STOREID,
				LOCATION,
				STATEMENTMETHOD,
				TERMINALSTATEMENT,
				NOTACTIVE,--10
				CLOSINGSTATUS,
				DISPLAYTERMINALCLOSED,
				DISPLAYLINKEDITEM,
				MANAGERKEYONRETURN,
				SLIPIFRETURN,
				OPENDRAWERATLILO,
				ONLYTOTALINSUSPENDEDTRANS20015,
				EXITAFTEREACHTRANSACTION,
				AUTOLOGOFFTIMEOUT,
				RETURNINTRANSACTION,--20
				ITEMIDONRECEIPT,
				EFTSTOREID,
				EFTTERMINALID,
				MAXRECEIPTTEXTLENGTH,
				NUMBEROFTOPBOTTOMLINES,
				RECEIPTSETUPLOCATION,
				MAXDISPLAYTEXTLENGTH,
				CUSTOMERDISPLAYTEXT1,
				CUSTOMERDISPLAYTEXT2,
				HARDWAREPROFILE,--30
				VISUALPROFILE,
				PRINTVATREFUNDCHECKS,
				RECEIPTPRINTINGDEFAULTOFF,
				RECEIPTBARCODE,
				LASTZREPORTID,
				LAYOUTID,
				UPDATESERVICEPORT,
				IPADDRESS,
				DATAAREAID,
				FUNCTIONALITYPROFILE,
				RECEIPTIDNUMBERSEQUENCE,
				STANDALONE,
				TRANSACTIONIDNUMBERSEQUENCE,
				TRANSACTIONSERVICEPROFILE,
				SALESTYPEFILTER,
				SUSPENDALLOWEOD,
				LSPAYUSELOCALSERVER,
				LSPAYSERVERNAME,
				LSPAYSERVERPORT,
				LSPAYPLUGINID,
				LSPAYPLUGINNAME,
				LSPAYSUPPORTREFREFUND,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.TERMINALID,
				ins.NAME,--5
				ins.STOREID,
				ins.LOCATION,
				ins.STATEMENTMETHOD,
				ins.TERMINALSTATEMENT,
				ins.NOTACTIVE, -- 10
				ins.CLOSINGSTATUS,
				ins.DISPLAYTERMINALCLOSED,
				ins.DISPLAYLINKEDITEM,
				ins.MANAGERKEYONRETURN,
				ins.SLIPIFRETURN,
				ins.OPENDRAWERATLILO,
				ins.ONLYTOTALINSUSPENDEDTRANS20015,
				ins.EXITAFTEREACHTRANSACTION,
				ins.AUTOLOGOFFTIMEOUT,
				ins.RETURNINTRANSACTION,--20
				ins.ITEMIDONRECEIPT,
				ins.EFTSTOREID,
				ins.EFTTERMINALID,
				ins.MAXRECEIPTTEXTLENGTH,
				ins.NUMBEROFTOPBOTTOMLINES,
				ins.RECEIPTSETUPLOCATION,
				ins.MAXDISPLAYTEXTLENGTH,
				ins.CUSTOMERDISPLAYTEXT1,
				ins.CUSTOMERDISPLAYTEXT2,
				ins.HARDWAREPROFILE,--30
				ins.VISUALPROFILE,
				ins.PRINTVATREFUNDCHECKS,
				ins.RECEIPTPRINTINGDEFAULTOFF,
				ins.RECEIPTBARCODE,
				ins.LASTZREPORTID,
				ins.LAYOUTID,
				ins.UPDATESERVICEPORT,
				ins.IPADDRESS,
				ins.DATAAREAID,
				ins.FUNCTIONALITYPROFILE,
				ins.RECEIPTIDNUMBERSEQUENCE,
				ins.STANDALONE,
				ins.TRANSACTIONIDNUMBERSEQUENCE,
				ins.TRANSACTIONSERVICEPROFILE,
				ins.SALESTYPEFILTER,
				ins.SUSPENDALLOWEOD,
				ins.LSPAYUSELOCALSERVER,
				ins.LSPAYSERVERNAME,
				ins.LSPAYSERVERPORT,
				ins.LSPAYPLUGINID,
				ins.LSPAYPLUGINNAME,
				ins.LSPAYSUPPORTREFREFUND,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOTERMINALTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				TERMINALID,
				NAME,
				STOREID,
				LOCATION,
				STATEMENTMETHOD,
				TERMINALSTATEMENT,
				NOTACTIVE,
				CLOSINGSTATUS,
				DISPLAYTERMINALCLOSED,
				DISPLAYLINKEDITEM,
				MANAGERKEYONRETURN,
				SLIPIFRETURN,
				OPENDRAWERATLILO,
				ONLYTOTALINSUSPENDEDTRANS20015,
				EXITAFTEREACHTRANSACTION,
				AUTOLOGOFFTIMEOUT,
				RETURNINTRANSACTION,
				ITEMIDONRECEIPT,
				EFTSTOREID,
				EFTTERMINALID,
				MAXRECEIPTTEXTLENGTH,
				NUMBEROFTOPBOTTOMLINES,
				RECEIPTSETUPLOCATION,
				MAXDISPLAYTEXTLENGTH,
				CUSTOMERDISPLAYTEXT1,
				CUSTOMERDISPLAYTEXT2,
				HARDWAREPROFILE,
				VISUALPROFILE,
				PRINTVATREFUNDCHECKS,
				RECEIPTPRINTINGDEFAULTOFF,
				RECEIPTBARCODE,
				LASTZREPORTID,
				LAYOUTID,
				UPDATESERVICEPORT,
				IPADDRESS,
				DATAAREAID,
				FUNCTIONALITYPROFILE,
				RECEIPTIDNUMBERSEQUENCE,
				STANDALONE,
				TRANSACTIONIDNUMBERSEQUENCE,
				TRANSACTIONSERVICEPROFILE,
				SALESTYPEFILTER,
				SUSPENDALLOWEOD,
				LSPAYUSELOCALSERVER,
				LSPAYSERVERNAME,
				LSPAYSERVERPORT,
				LSPAYPLUGINID,
				LSPAYPLUGINNAME,
				LSPAYSUPPORTREFREFUND,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.TERMINALID,
				ins.NAME,
				ins.STOREID,
				ins.LOCATION,
				ins.STATEMENTMETHOD,
				ins.TERMINALSTATEMENT,
				ins.NOTACTIVE,
				ins.CLOSINGSTATUS,
				ins.DISPLAYTERMINALCLOSED,
				ins.DISPLAYLINKEDITEM,
				ins.MANAGERKEYONRETURN,
				ins.SLIPIFRETURN,
				ins.OPENDRAWERATLILO,
				ins.ONLYTOTALINSUSPENDEDTRANS20015,
				ins.EXITAFTEREACHTRANSACTION,
				ins.AUTOLOGOFFTIMEOUT,
				ins.RETURNINTRANSACTION,
				ins.ITEMIDONRECEIPT,
				ins.EFTSTOREID,
				ins.EFTTERMINALID,
				ins.MAXRECEIPTTEXTLENGTH,
				ins.NUMBEROFTOPBOTTOMLINES,
				ins.RECEIPTSETUPLOCATION,
				ins.MAXDISPLAYTEXTLENGTH,
				ins.CUSTOMERDISPLAYTEXT1,
				ins.CUSTOMERDISPLAYTEXT2,
				ins.HARDWAREPROFILE,
				ins.VISUALPROFILE,
				ins.PRINTVATREFUNDCHECKS,
				ins.RECEIPTPRINTINGDEFAULTOFF,
				ins.RECEIPTBARCODE,
				ins.LASTZREPORTID,
				ins.LAYOUTID,
				ins.UPDATESERVICEPORT,
				ins.IPADDRESS,
				ins.DATAAREAID,
				ins.FUNCTIONALITYPROFILE,
				ins.RECEIPTIDNUMBERSEQUENCE,
				ins.STANDALONE,
				ins.TRANSACTIONIDNUMBERSEQUENCE,
				ins.TRANSACTIONSERVICEPROFILE,
				ins.SALESTYPEFILTER,
				SUSPENDALLOWEOD,
				ins.LSPAYUSELOCALSERVER,
				ins.LSPAYSERVERNAME,
				ins.LSPAYSERVERPORT,
				ins.LSPAYPLUGINID,
				ins.LSPAYPLUGINNAME,
				ins.LSPAYSUPPORTREFREFUND,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOCOLORGROUPTABLE]'))
begin
   drop trigger dbo.Update_RBOCOLORGROUPTABLE
end

GO

create trigger Update_RBOCOLORGROUPTABLE
on RBOCOLORGROUPTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOCOLORGROUPTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				COLORGROUP,
				DESCRIPTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.COLORGROUP,
				ins.DESCRIPTION,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOCOLORGROUPTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				COLORGROUP,
				DESCRIPTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.COLORGROUP,
				ins.DESCRIPTION,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOCOLORGROUPTRANS]'))
begin
   drop trigger dbo.Update_RBOCOLORGROUPTRANS
end

GO

create trigger Update_RBOCOLORGROUPTRANS
on RBOCOLORGROUPTRANS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOCOLORGROUPTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				COLORGROUP,
				Color,
				DESCRIPTION,
				Name,
				Weight,
				NOINBARCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.COLORGROUP,
				ins.COLOR,
				'N/A',
				ins.NAME,
				ins.WEIGHT,
				ins.NOINBARCODE,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOCOLORGROUPTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				COLORGROUP,
				Color,
				DESCRIPTION,
				Name,
				Weight,
				NOINBARCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.COLORGROUP,
				ins.COLOR,
				'N/A',
				ins.NAME,
				ins.WEIGHT,
				ins.NOINBARCODE,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTYLEGROUPTABLE]'))
begin
   drop trigger dbo.Update_RBOSTYLEGROUPTABLE
end

GO

create trigger Update_RBOSTYLEGROUPTABLE
on RBOSTYLEGROUPTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTYLEGROUPTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				STYLEGROUP,
				DESCRIPTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.STYLEGROUP,
				ins.DESCRIPTION,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTYLEGROUPTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				STYLEGROUP,
				DESCRIPTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.STYLEGROUP,
				ins.DESCRIPTION,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTYLEGROUPTRANS]'))
begin
   drop trigger dbo.Update_RBOSTYLEGROUPTRANS
end

GO

create trigger Update_RBOSTYLEGROUPTRANS
on RBOSTYLEGROUPTRANS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTYLEGROUPTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				STYLEGROUP,
				Style,
				DESCRIPTION,
				Name,
				Weight,
				NOINBARCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.STYLEGROUP,
				ins.STYLE,
				'N/A',
				ins.NAME,
				ins.WEIGHT,
				ins.NOINBARCODE,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTYLEGROUPTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				STYLEGROUP,
				Style,
				DESCRIPTION,
				Name,
				Weight,
				NOINBARCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.STYLEGROUP,
				ins.STYLE,
				'N/A',
				ins.NAME,
				ins.WEIGHT,
				ins.NOINBARCODE,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------


GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSIZEGROUPTABLE]'))
begin
   drop trigger dbo.Update_RBOSIZEGROUPTABLE
end

GO

create trigger Update_RBOSIZEGROUPTABLE
on RBOSIZEGROUPTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSIZEGROUPTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				SIZEGROUP,
				DESCRIPTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.SIZEGROUP,
				ins.DESCRIPTION,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSIZEGROUPTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				SIZEGROUP,
				DESCRIPTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.SIZEGROUP,
				ins.DESCRIPTION,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSIZEGROUPTRANS]'))
begin
   drop trigger dbo.Update_RBOSIZEGROUPTRANS
end

GO

create trigger Update_RBOSIZEGROUPTRANS
on RBOSIZEGROUPTRANS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSIZEGROUPTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				SIZEGROUP,
				Size_,
				DESCRIPTION,
				Name,
				Weight,
				NOINBARCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.SIZEGROUP,
				ins.SIZE_,
				'N/A',
				ins.NAME,
				ins.WEIGHT,
				ins.NOINBARCODE,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSIZEGROUPTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				SIZEGROUP,
				Size_,
				DESCRIPTION,
				Name,
				Weight,
				NOINBARCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.SIZEGROUP,
				ins.SIZE_,
				'N/A',
				ins.NAME,
				ins.WEIGHT,
				ins.NOINBARCODE,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_BARCODESETUP]'))
begin
   drop trigger dbo.Update_BARCODESETUP
end

GO

create trigger Update_BARCODESETUP
on BARCODESETUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.BARCODESETUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				BARCODESETUPID,
				BARCODETYPE,
				FONTNAME,
				FONTSIZE,
				DESCRIPTION,
				MINIMUMLENGTH,
				MAXIMUMLENGTH,
				RBOBARCODEMASK,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.BARCODESETUPID,
				ins.BARCODETYPE,
				ins.FONTNAME,
				ins.FONTSIZE,
				ins.DESCRIPTION,
				ins.MINIMUMLENGTH,
				ins.MAXIMUMLENGTH,
				ins.RBOBARCODEMASK,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.BARCODESETUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				BARCODESETUPID,
				BARCODETYPE,
				FONTNAME,
				FONTSIZE,
				DESCRIPTION,
				MINIMUMLENGTH,
				MAXIMUMLENGTH,
				RBOBARCODEMASK,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.BARCODESETUPID,
				ins.BARCODETYPE,
				ins.FONTNAME,
				ins.FONTSIZE,
				ins.DESCRIPTION,
				ins.MINIMUMLENGTH,
				ins.MAXIMUMLENGTH,
				ins.RBOBARCODEMASK,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO



-- ---------------------------------------------------------------------------------------------------
GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOBARCODEMASKTABLE]'))
begin
   drop trigger dbo.Update_RBOBARCODEMASKTABLE
end

GO

create trigger Update_RBOBARCODEMASKTABLE
on RBOBARCODEMASKTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOBARCODEMASKTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				DESCRIPTION,
				MASK,
				PREFIX,
				SYMBOLOGY,
				TYPE,
				MASKID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.DESCRIPTION,
				ins.MASK,
				ins.PREFIX,
				ins.SYMBOLOGY,
				ins.TYPE,
				ins.MASKID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOBARCODEMASKTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				DESCRIPTION,
				MASK,
				PREFIX,
				SYMBOLOGY,
				TYPE,
				MASKID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.DESCRIPTION,
				ins.MASK,
				ins.PREFIX,
				ins.SYMBOLOGY,
				ins.TYPE,
				ins.MASKID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOBARCODEMASKSEGMENT]'))
begin
   drop trigger dbo.Update_RBOBARCODEMASKSEGMENT
end

GO

create trigger Update_RBOBARCODEMASKSEGMENT
on RBOBARCODEMASKSEGMENT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOBARCODEMASKSEGMENTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				SEGMENTNUM,
				LENGTH,
				TYPE,
				DECIMALS,
				CHAR,
				MASKID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.SEGMENTNUM,
				ins.LENGTH,
				ins.TYPE,
				ins.DECIMALS,
				ins.CHAR,
				ins.MASKID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOBARCODEMASKSEGMENTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				SEGMENTNUM,
				LENGTH,
				TYPE,
				DECIMALS,
				CHAR,
				MASKID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.SEGMENTNUM,
				ins.LENGTH,
				ins.TYPE,
				ins.DECIMALS,
				ins.CHAR,
				ins.MASKID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTITEMBARCODE]'))
begin
   drop trigger dbo.Update_INVENTITEMBARCODE
end

GO

create trigger Update_INVENTITEMBARCODE
on INVENTITEMBARCODE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTITEMBARCODELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ITEMBARCODE,
				ITEMID,
				INVENTDIMID,
				BARCODESETUPID,
				USEFORPRINTING,
				USEFORINPUT,
				DESCRIPTION,
				QTY,
				UNITID,
				RBOVARIANTID,
				RBOSHOWFORITEM,
				BLOCKED,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ITEMBARCODE,
				ins.ITEMID,
				ins.INVENTDIMID,
				ins.BARCODESETUPID,
				ins.USEFORPRINTING,
				ins.USEFORINPUT,
				ins.DESCRIPTION,
				ins.QTY,
				ins.UNITID,
				ins.RBOVARIANTID,
				ins.RBOSHOWFORITEM,
				ins.BLOCKED,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTITEMBARCODELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ITEMBARCODE,
				ITEMID,
				INVENTDIMID,
				BARCODESETUPID,
				USEFORPRINTING,
				USEFORINPUT,
				DESCRIPTION,
				QTY,
				UNITID,
				RBOVARIANTID,
				RBOSHOWFORITEM,
				BLOCKED,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ITEMBARCODE,
				ins.ITEMID,
				ins.INVENTDIMID,
				ins.BARCODESETUPID,
				ins.USEFORPRINTING,
				ins.USEFORINPUT,
				ins.DESCRIPTION,
				ins.QTY,
				ins.UNITID,
				ins.RBOVARIANTID,
				ins.RBOSHOWFORITEM,
				ins.BLOCKED,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTDIMCOMBINATION]'))
begin
   drop trigger dbo.Update_INVENTDIMCOMBINATION
end

GO

create trigger Update_INVENTDIMCOMBINATION
on INVENTDIMCOMBINATION after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTDIMCOMBINATIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ITEMID,
				NAME,
				AUTOCREATED,
				COSTPRICE,
				PRICEUNIT,
				MARKUP,
				PRICEQTY,
				PRICEDATE,
				ALLOCATEMARKUP,
				ITEMIDCOMPANY,
				RBOVARIANTID,
				INVENTSIZEID,
				INVENTCOLORID,
				INVENTSTYLEID,
				CONFIGID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ITEMID,
				ins.NAME,
				ins.AUTOCREATED,
				ins.COSTPRICE,
				ins.PRICEUNIT,
				ins.MARKUP,
				ins.PRICEQTY,
				ins.PRICEDATE,
				ins.ALLOCATEMARKUP,
				ins.ITEMIDCOMPANY,
				ins.RBOVARIANTID,
				ins.INVENTSIZEID,
				ins.INVENTCOLORID,
				ins.INVENTSTYLEID,
				'',
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTDIMCOMBINATIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ITEMID,
				NAME,
				AUTOCREATED,
				COSTPRICE,
				PRICEUNIT,
				MARKUP,
				PRICEQTY,
				PRICEDATE,
				ALLOCATEMARKUP,
				ITEMIDCOMPANY,
				RBOVARIANTID,
				INVENTSIZEID,
				INVENTCOLORID,
				INVENTSTYLEID,
				CONFIGID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ITEMID,
				ins.NAME,
				ins.AUTOCREATED,
				ins.COSTPRICE,
				ins.PRICEUNIT,
				ins.MARKUP,
				ins.PRICEQTY,
				ins.PRICEDATE,
				ins.ALLOCATEMARKUP,
				ins.ITEMIDCOMPANY, 
				ins.RBOVARIANTID,
				ins.INVENTSIZEID,
				ins.INVENTCOLORID,
				ins.INVENTSTYLEID,
				'',
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PRICEDISCTABLE]'))
begin
   drop trigger dbo.Update_PRICEDISCTABLE
end

GO

create trigger Update_PRICEDISCTABLE
on PRICEDISCTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PRICEDISCTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				AGREEMENT,
				ITEMCODE,
				ACCOUNTCODE,
				ITEMRELATION,
				ACCOUNTRELATION,
				QUANTITYAMOUNT,
				FROMDATE,
				TODATE,
				AMOUNT,
				AMOUNTINCLTAX,
				CURRENCY,
				PERCENT1,
				PERCENT2,
				DELIVERYTIME,
				SEARCHAGAIN,
				PRICEUNIT,
				RELATION,
				UNITID,
				MARKUP,
				ALLOCATEMARKUP,
				MODULE,
				INVENTDIMID,
				RECID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.AGREEMENT,
				ins.ITEMCODE,
				ins.ACCOUNTCODE,
				ins.ITEMRELATION,
				ins.ACCOUNTRELATION,
				ins.QUANTITYAMOUNT,
				ins.FROMDATE,
				ins.TODATE,
				ins.AMOUNT,
				ins.AMOUNTINCLTAX,
				ins.CURRENCY,
				ins.PERCENT1,
				ins.PERCENT2,
				ins.DELIVERYTIME,
				ins.SEARCHAGAIN,
				ins.PRICEUNIT,
				ins.RELATION,
				ins.UNITID,
				ins.MARKUP,
				ins.ALLOCATEMARKUP,
				ins.MODULE,
				ins.INVENTDIMID,
				ins.RECID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PRICEDISCTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				AGREEMENT,
				ITEMCODE,
				ACCOUNTCODE,
				ITEMRELATION,
				ACCOUNTRELATION,
				QUANTITYAMOUNT,
				FROMDATE,
				TODATE,
				AMOUNT,
				AMOUNTINCLTAX,
				CURRENCY,
				PERCENT1,
				PERCENT2,
				DELIVERYTIME,
				SEARCHAGAIN,
				PRICEUNIT,
				RELATION,
				UNITID,
				MARKUP,
				ALLOCATEMARKUP,
				MODULE,
				INVENTDIMID,
				RECID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.AGREEMENT,
				ins.ITEMCODE,
				ins.ACCOUNTCODE,
				ins.ITEMRELATION,
				ins.ACCOUNTRELATION,
				ins.QUANTITYAMOUNT,
				ins.FROMDATE,
				ins.TODATE,
				ins.AMOUNT,
				ins.AMOUNTINCLTAX,
				ins.CURRENCY,
				ins.PERCENT1,
				ins.PERCENT2,
				ins.DELIVERYTIME,
				ins.SEARCHAGAIN,
				ins.PRICEUNIT,
				ins.RELATION,
				ins.UNITID,
				ins.MARKUP,
				ins.ALLOCATEMARKUP,
				ins.MODULE,
				ins.INVENTDIMID,
				ins.RECID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PRICEDISCGROUP]'))
begin
   drop trigger dbo.Update_PRICEDISCGROUP
end

GO

create trigger Update_PRICEDISCGROUP
on PRICEDISCGROUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PRICEDISCGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				MODULE,
				TYPE,
				GROUPID,
				NAME,
				INCLTAX,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.MODULE,
				ins.TYPE,
				ins.GROUPID,
				ins.NAME,
				ins.INCLTAX,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PRICEDISCGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				MODULE,
				TYPE,
				GROUPID,
				NAME,
				INCLTAX,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.MODULE,
				ins.TYPE,
				ins.GROUPID,
				ins.NAME,
				ins.INCLTAX,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTTABLEMODULE]'))
begin
   drop trigger dbo.Update_INVENTTABLEMODULE
end

GO

create trigger Update_INVENTTABLEMODULE
on INVENTTABLEMODULE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTTABLEMODULELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ITEMID,
				MODULETYPE,
				UNITID,
				PRICE,
				PRICEUNIT,
				PRICEINCLTAX,
				MARKUP,
				QUANTITY,
				LOWESTQTY,
				HIGHESTQTY,
				BLOCKED,
				DELIVERYTIME,
				INVENTLOCATIONID,
				MANDATORYINVENTLOCATION,
				STANDARDQTY,
				PRICEDATE,
				PRICEQTY,
				ALLOCATEMARKUP,
				CALENDARDAYS,
				INTERCOMPANYBLOCKED,
				TAXITEMGROUPID,
				LINEDISC,
				MULTILINEDISC,
				ENDDISC,
				MARKUPGROUPID,
				OVERDELIVERYPCT,
				UNDERDELIVERYPCT,
				SUPPITEMGROUPID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ITEMID,
				ins.MODULETYPE,
				ins.UNITID,
				ins.PRICE,
				ins.PRICEUNIT,
				ins.PRICEINCLTAX,
				ins.MARKUP,
				ins.QUANTITY,
				ins.LOWESTQTY,
				ins.HIGHESTQTY,
				ins.BLOCKED,
				ins.DELIVERYTIME,
				ins.INVENTLOCATIONID,
				ins.MANDATORYINVENTLOCATION,
				ins.STANDARDQTY,
				ins.PRICEDATE,
				ins.PRICEQTY,
				ins.ALLOCATEMARKUP,
				ins.CALENDARDAYS,
				ins.INTERCOMPANYBLOCKED,
				ins.TAXITEMGROUPID,
				ins.LINEDISC,
				ins.MULTILINEDISC,
				ins.ENDDISC,
				ins.MARKUPGROUPID,
				ins.OVERDELIVERYPCT,
				ins.UNDERDELIVERYPCT,
				ins.SUPPITEMGROUPID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTTABLEMODULELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				ITEMID,
				MODULETYPE,
				UNITID,
				PRICE,
				PRICEUNIT,
				PRICEINCLTAX,
				MARKUP,
				QUANTITY,
				LOWESTQTY,
				HIGHESTQTY,
				BLOCKED,
				DELIVERYTIME,
				INVENTLOCATIONID,
				MANDATORYINVENTLOCATION,
				STANDARDQTY,
				PRICEDATE,
				PRICEQTY,
				ALLOCATEMARKUP,
				CALENDARDAYS,
				INTERCOMPANYBLOCKED,
				TAXITEMGROUPID,
				LINEDISC,
				MULTILINEDISC,
				ENDDISC,
				MARKUPGROUPID,
				OVERDELIVERYPCT,
				UNDERDELIVERYPCT,
				SUPPITEMGROUPID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.ITEMID,
				ins.MODULETYPE,
				ins.UNITID,
				ins.PRICE,
				ins.PRICEUNIT,
				ins.PRICEINCLTAX,
				ins.MARKUP,
				ins.QUANTITY,
				ins.LOWESTQTY,
				ins.HIGHESTQTY,
				ins.BLOCKED,
				ins.DELIVERYTIME,
				ins.INVENTLOCATIONID,
				ins.MANDATORYINVENTLOCATION,
				ins.STANDARDQTY,
				ins.PRICEDATE,
				ins.PRICEQTY,
				ins.ALLOCATEMARKUP,
				ins.CALENDARDAYS,
				ins.INTERCOMPANYBLOCKED,
				ins.TAXITEMGROUPID,
				ins.LINEDISC,
				ins.MULTILINEDISC,
				ins.ENDDISC,
				ins.MARKUPGROUPID,
				ins.OVERDELIVERYPCT,
				ins.UNDERDELIVERYPCT,
				ins.SUPPITEMGROUPID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOPARAMETERS]'))
begin
   drop trigger dbo.Update_RBOPARAMETERS
end

GO

create trigger Update_RBOPARAMETERS
on RBOPARAMETERS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				KEY_,
				RECEIPTOPTION,
				LOCALSTOREID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.KEY_,
				ins.RECEIPTOPTION,
				ins.LOCALSTOREID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				KEY_,
				RECEIPTOPTION,
				LOCALSTOREID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.KEY_,
				ins.RECEIPTOPTION,
				ins.LOCALSTOREID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_TAXTABLE]'))
begin
   drop trigger dbo.Update_TAXTABLE
end

GO

create trigger Update_TAXTABLE
on TAXTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.TAXTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXCODE,
				TAXROUNDOFF,
				TAXROUNDOFFTYPE,
				TAXNAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXCODE,
				ins.TAXROUNDOFF,
				ins.TAXROUNDOFFTYPE,
				ins.TAXNAME,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.TAXTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXCODE,
				TAXROUNDOFF,
				TAXROUNDOFFTYPE,
				TAXNAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXCODE,
				ins.TAXROUNDOFF,
				ins.TAXROUNDOFFTYPE,
				ins.TAXNAME,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_TAXDATA]'))
begin
   drop trigger dbo.Update_TAXDATA
end

GO

create trigger Update_TAXDATA
on TAXDATA after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.TAXDATALog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXCODE,
				TAXVALUE,
				TAXLIMITMIN,
				TAXLIMITMAX,
				VATEXEMPTPCT,
				TAXFROMDATE,
				TAXTODATE,
				INVESTMENTTAXPCT,
				POSTAXCODEID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXCODE,
				ins.TAXVALUE,
				ins.TAXLIMITMIN,
				ins.TAXLIMITMAX,
				ins.VATEXEMPTPCT,
				ins.TAXFROMDATE,
				ins.TAXTODATE,
				ins.INVESTMENTTAXPCT,
				ins.POSTAXCODEID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.TAXDATALog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXCODE,
				TAXVALUE,
				TAXLIMITMIN,
				TAXLIMITMAX,
				VATEXEMPTPCT,
				TAXFROMDATE,
				TAXTODATE,
				INVESTMENTTAXPCT,
				POSTAXCODEID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXCODE,
				ins.TAXVALUE,
				ins.TAXLIMITMIN,
				ins.TAXLIMITMAX,
				ins.VATEXEMPTPCT,
				ins.TAXFROMDATE,
				ins.TAXTODATE,
				ins.INVESTMENTTAXPCT,
				ins.POSTAXCODEID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_TAXITEMGROUPHEADING]'))
begin
   drop trigger dbo.Update_TAXITEMGROUPHEADING
end

GO

create trigger Update_TAXITEMGROUPHEADING
on TAXITEMGROUPHEADING after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.TAXITEMGROUPHEADINGLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXITEMGROUP,
				NAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXITEMGROUP,
				ins.NAME,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.TAXITEMGROUPHEADINGLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXITEMGROUP,
				NAME,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXITEMGROUP,
				ins.NAME,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_TAXONITEM]'))
begin
   drop trigger dbo.Update_TAXONITEM
end

GO

create trigger Update_TAXONITEM
on TAXONITEM after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.TAXONITEMLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXITEMGROUP,
				TAXCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXITEMGROUP,
				ins.TAXCODE,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.TAXONITEMLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				TAXITEMGROUP,
				TAXCODE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.TAXITEMGROUP,
				ins.TAXCODE,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTDIMGROUP]'))
begin
   drop trigger dbo.Update_INVENTDIMGROUP
end

GO

create trigger Update_INVENTDIMGROUP
on INVENTDIMGROUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTDIMGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				DIMGROUPID,
				NAME,
				POSDISPLAYSETTING,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.DIMGROUPID,
				ins.NAME,
				ins.POSDISPLAYSETTING,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTDIMGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				DIMGROUPID,
				NAME,
				POSDISPLAYSETTING,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.DIMGROUPID,
				ins.NAME,
				ins.POSDISPLAYSETTING,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTDIMSETUP]'))
begin
   drop trigger dbo.Update_INVENTDIMSETUP
end

GO

create trigger Update_INVENTDIMSETUP
on INVENTDIMSETUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTDIMSETUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				DIMGROUPID,
				DIMFIELDID,
				LINENUM,
				ALLOWBLANKRECEIPT,
				ALLOWBLANKISSUE,
				ACTIVE,
				FINANCIALINVENT,
				PHYSICALINVENT,
				PRIMARYSTOCKING,
				EDITINVENTTRANSACTION,
				EDITPURCHLINE,
				EDITSALESLINE,
				EDITPRODUCTION,
				EDITPRODLINE,
				EDITINVENTTRANSFER,
				EDITINVENTLOSSPROFIT,
				EDITINVENTCOUNTING,
				ITEMDIMENSION,
				EDITINVENTQUARANTINEORDER,
				EDITPROJECT,
				EDITASSET,
				SERIALNUMBERCONTROL,
				COVPRDIMENSION,
				EDITSMMQUOTATIONLINE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.DIMGROUPID,
				ins.DIMFIELDID,
				ins.LINENUM,
				ins.ALLOWBLANKRECEIPT,
				ins.ALLOWBLANKISSUE,
				ins.ACTIVE,
				ins.FINANCIALINVENT,
				ins.PHYSICALINVENT,
				ins.PRIMARYSTOCKING,
				ins.EDITINVENTTRANSACTION,
				ins.EDITPURCHLINE,
				ins.EDITSALESLINE,
				ins.EDITPRODUCTION,
				ins.EDITPRODLINE,
				ins.EDITINVENTTRANSFER,
				ins.EDITINVENTLOSSPROFIT,
				ins.EDITINVENTCOUNTING,
				ins.ITEMDIMENSION,
				ins.EDITINVENTQUARANTINEORDER,
				ins.EDITPROJECT,
				ins.EDITASSET,
				ins.SERIALNUMBERCONTROL,
				ins.COVPRDIMENSION,
				ins.EDITSMMQUOTATIONLINE,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTDIMSETUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DATAAREAID,
				DIMGROUPID,
				DIMFIELDID,
				LINENUM,
				ALLOWBLANKRECEIPT,
				ALLOWBLANKISSUE,
				ACTIVE,
				FINANCIALINVENT,
				PHYSICALINVENT,
				PRIMARYSTOCKING,
				EDITINVENTTRANSACTION,
				EDITPURCHLINE,
				EDITSALESLINE,
				EDITPRODUCTION,
				EDITPRODLINE,
				EDITINVENTTRANSFER,
				EDITINVENTLOSSPROFIT,
				EDITINVENTCOUNTING,
				ITEMDIMENSION,
				EDITINVENTQUARANTINEORDER,
				EDITPROJECT,
				EDITASSET,
				SERIALNUMBERCONTROL,
				COVPRDIMENSION,
				EDITSMMQUOTATIONLINE,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DATAAREAID,
				ins.DIMGROUPID,
				ins.DIMFIELDID,
				ins.LINENUM,
				ins.ALLOWBLANKRECEIPT,
				ins.ALLOWBLANKISSUE,
				ins.ACTIVE,
				ins.FINANCIALINVENT,
				ins.PHYSICALINVENT,
				ins.PRIMARYSTOCKING,
				ins.EDITINVENTTRANSACTION,
				ins.EDITPURCHLINE,
				ins.EDITSALESLINE,
				ins.EDITPRODUCTION,
				ins.EDITPRODLINE,
				ins.EDITINVENTTRANSFER,
				ins.EDITINVENTLOSSPROFIT,
				ins.EDITINVENTCOUNTING,
				ins.ITEMDIMENSION,
				ins.EDITINVENTQUARANTINEORDER,
				ins.EDITPROJECT,
				ins.EDITASSET,
				ins.SERIALNUMBERCONTROL,
				ins.COVPRDIMENSION,
				ins.EDITSMMQUOTATIONLINE,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_TAXGROUPHEADING]'))
begin
   drop trigger dbo.Update_TAXGROUPHEADING
end

GO

create trigger Update_TAXGROUPHEADING
on TAXGROUPHEADING after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.TAXGROUPHEADINGLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				TAXGROUP,
				TAXGROUPNAME,
				SEARCHFIELD1,
				SEARCHFIELD2,
				TAXREVERSEONCASHDISC,
				TAXDIRECTION,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.TAXGROUP,
				ins.TAXGROUPNAME,
				ins.SEARCHFIELD1,
				ins.SEARCHFIELD2,
				ins.TAXREVERSEONCASHDISC,
				ins.TAXDIRECTION,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.TAXGROUPHEADINGLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				TAXGROUP,
				TAXGROUPNAME,
				SEARCHFIELD1,
				SEARCHFIELD2,
				TAXREVERSEONCASHDISC,
				TAXDIRECTION,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.TAXGROUP,
				ins.TAXGROUPNAME,
				ins.SEARCHFIELD1,
				ins.SEARCHFIELD2,
				ins.TAXREVERSEONCASHDISC,
				ins.TAXDIRECTION,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_TAXGROUPDATA]'))
begin
   drop trigger dbo.Update_TAXGROUPDATA
end

GO

create trigger Update_TAXGROUPDATA
on TAXGROUPDATA after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.TAXGROUPDATALog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				TAXGROUP,
				TAXCODE,
				EXEMPTTAX,
				USETAX,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.TAXGROUP,
				ins.TAXCODE,
				ins.EXEMPTTAX,
				ins.USETAX,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.TAXGROUPDATALog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				TAXGROUP,
				TAXCODE,
				EXEMPTTAX,
				USETAX,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.TAXGROUP,
				ins.TAXCODE,
				ins.EXEMPTTAX,
				ins.USETAX,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_DECIMALSETTINGS]'))
begin
   drop trigger dbo.Update_DECIMALSETTINGS
end

GO

create trigger Update_DECIMALSETTINGS
on DECIMALSETTINGS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.DECIMALSETTINGSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ID,
				NAME,
				MINDECIMALS,
				MAXDECIMALS,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ID,
				ins.NAME,
				ins.MINDECIMALS,
				ins.MAXDECIMALS,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.DECIMALSETTINGSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ID,
				NAME,
				MINDECIMALS,
				MAXDECIMALS,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ID,
				ins.NAME,
				ins.MINDECIMALS,
				ins.MAXDECIMALS,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISLICENSE]'))
begin
   drop trigger dbo.Update_POSISLICENSE
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISPARAMETERS]'))
begin
   drop trigger dbo.Update_POSISPARAMETERS
end

GO

create trigger Update_POSISPARAMETERS
on POSISPARAMETERS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSISPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				WEBSERVICEURL,
				WEBSERVICEUSERNAME,
				WEBSERVICEPASSWORD,
				KEY_,
				MODIFIEDDATETIME,
				DEL_MODIFIED,
				MODIFIEDBY,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.WEBSERVICEURL,
				ins.WEBSERVICEUSERNAME,
				ins.WEBSERVICEPASSWORD,
				ins.KEY_,
				ins.MODIFIEDDATETIME,
				ins.DEL_MODIFIED,
				ins.MODIFIEDBY,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSISPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				WEBSERVICEURL,
				WEBSERVICEUSERNAME,
				WEBSERVICEPASSWORD,
				KEY_,
				MODIFIEDDATETIME,
				DEL_MODIFIED,
				MODIFIEDBY,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.WEBSERVICEURL,
				ins.WEBSERVICEUSERNAME,
				ins.WEBSERVICEPASSWORD,
				ins.KEY_,
				ins.MODIFIEDDATETIME,
				ins.DEL_MODIFIED,
				ins.MODIFIEDBY,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_COMPANYINFO]'))
begin
   drop trigger dbo.Update_COMPANYINFO
end

GO

create trigger Update_COMPANYINFO
on COMPANYINFO after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.COMPANYINFOLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				NAME,
				ADDRESS,
				PHONE,
				TELEFAX,
				BANK,
				GIRO,
				REGNUM,
				COREGNUM,
				VATNUM,
				CURRENCYCODE,
				IMPORTVATNUM,
				ZIPCODE,
				STATE,
				COUNTY,
				COUNTRYREGIONID,
				TELEX,
				URL,
				EMAIL,
				CELLULARPHONE,
				PHONELOCAL,
				UPSNUM,
				TAX1099REGNUM,
				NAMECONTROL,
				TCC,
				EUROCURRENCYCODE,
				KEY_,
				SECONDARYCURRENCYCODE,
				DVRID,
				LANGUAGEID,
				INTRASTATCODE,
				GIROCONTRACT,
				GIROCONTRACTACCOUNT,
				BRANCHID,
				VATNUMBRANCHID,
				IMPORTVATNUMBRANCHID,
				ACTIVITYCODE,
				STREET,
				CITY,
				CONVERSIONDATE,
				PAGER,
				SMS,
				ADDRFORMAT,
				COMPANYREGCOMFR,
				PACKMATERIALFEELICENSENUM,
				COMPANYIDNAF,
				PAYMROUTINGDNB,
				PAYMTRADERNUMBER,
				PAYMINSTRUCTIONID1,
				PAYMINSTRUCTIONID2,
				PAYMINSTRUCTIONID3,
				PAYMINSTRUCTIONID4,
				ISSUINGSIGNATURE,
				SIACODE,
				BANKCENTRALBANKPURPOSECODE,
				BANKCENTRALBANKPURPOSETEXT,
				DBA,
				FOREIGNENTITYINDICATOR,
				COMBINEDFEDSTATEFILER,
				LASTFILINGINDICATOR,
				VALIDATE1099ONENTRY,
				LEGALFORMFR,
				SHIPPINGCALENDARID,
				ENTERPRISENUMBER,
				BRANCHNUMBER,
				CUSTOMSCUSTOMERNUMBER_FI,
				CUSTOMSLICENSENUMBER_FI,
				PLANNINGCOMPANY,
				TAXREPRESENTATIVE,
				FALLBACKINVENTLOCATIONID,
				ENABLENORWAY,
				ORGID,
				BANKACCTUSEDFOR1099,
				MODIFIEDDATETIME,
				DEL_MODIFIEDTIME,
				MODIFIEDBY,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.NAME,
				ins.ADDRESS,
				ins.PHONE,
				ins.TELEFAX,
				ins.BANK,
				ins.GIRO,
				ins.REGNUM,
				ins.COREGNUM,
				ins.VATNUM,
				ins.CURRENCYCODE,
				ins.IMPORTVATNUM,
				ins.ZIPCODE,
				ins.STATE,
				ins.COUNTY,
				ins.COUNTRYREGIONID,
				ins.TELEX,
				ins.URL,
				ins.EMAIL,
				ins.CELLULARPHONE,
				ins.PHONELOCAL,
				ins.UPSNUM,
				ins.TAX1099REGNUM,
				ins.NAMECONTROL,
				ins.TCC,
				ins.EUROCURRENCYCODE,
				ins.KEY_,
				ins.SECONDARYCURRENCYCODE,
				ins.DVRID,
				ins.LANGUAGEID,
				ins.INTRASTATCODE,
				ins.GIROCONTRACT,
				ins.GIROCONTRACTACCOUNT,
				ins.BRANCHID,
				ins.VATNUMBRANCHID,
				ins.IMPORTVATNUMBRANCHID,
				ins.ACTIVITYCODE,
				ins.STREET,
				ins.CITY,
				ins.CONVERSIONDATE,
				ins.PAGER,
				ins.SMS,
				ins.ADDRFORMAT,
				ins.COMPANYREGCOMFR,
				ins.PACKMATERIALFEELICENSENUM,
				ins.COMPANYIDNAF,
				ins.PAYMROUTINGDNB,
				ins.PAYMTRADERNUMBER,
				ins.PAYMINSTRUCTIONID1,
				ins.PAYMINSTRUCTIONID2,
				ins.PAYMINSTRUCTIONID3,
				ins.PAYMINSTRUCTIONID4,
				ins.ISSUINGSIGNATURE,
				ins.SIACODE,
				ins.BANKCENTRALBANKPURPOSECODE,
				ins.BANKCENTRALBANKPURPOSETEXT,
				ins.DBA,
				ins.FOREIGNENTITYINDICATOR,
				ins.COMBINEDFEDSTATEFILER,
				ins.LASTFILINGINDICATOR,
				ins.VALIDATE1099ONENTRY,
				ins.LEGALFORMFR,
				ins.SHIPPINGCALENDARID,
				ins.ENTERPRISENUMBER,
				ins.BRANCHNUMBER,
				ins.CUSTOMSCUSTOMERNUMBER_FI,
				ins.CUSTOMSLICENSENUMBER_FI,
				ins.PLANNINGCOMPANY,
				ins.TAXREPRESENTATIVE,
				ins.FALLBACKINVENTLOCATIONID,
				ins.ENABLENORWAY,
				ins.ORGID,
				ins.BANKACCTUSEDFOR1099,
				ins.MODIFIEDDATETIME,
				ins.DEL_MODIFIEDTIME,
				ins.MODIFIEDBY,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.COMPANYINFOLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				NAME,
				ADDRESS,
				PHONE,
				TELEFAX,
				BANK,
				GIRO,
				REGNUM,
				COREGNUM,
				VATNUM,
				CURRENCYCODE,
				IMPORTVATNUM,
				ZIPCODE,
				STATE,
				COUNTY,
				COUNTRYREGIONID,
				TELEX,
				URL,
				EMAIL,
				CELLULARPHONE,
				PHONELOCAL,
				UPSNUM,
				TAX1099REGNUM,
				NAMECONTROL,
				TCC,
				EUROCURRENCYCODE,
				KEY_,
				SECONDARYCURRENCYCODE,
				DVRID,
				LANGUAGEID,
				INTRASTATCODE,
				GIROCONTRACT,
				GIROCONTRACTACCOUNT,
				BRANCHID,
				VATNUMBRANCHID,
				IMPORTVATNUMBRANCHID,
				ACTIVITYCODE,
				STREET,
				CITY,
				CONVERSIONDATE,
				PAGER,
				SMS,
				ADDRFORMAT,
				COMPANYREGCOMFR,
				PACKMATERIALFEELICENSENUM,
				COMPANYIDNAF,
				PAYMROUTINGDNB,
				PAYMTRADERNUMBER,
				PAYMINSTRUCTIONID1,
				PAYMINSTRUCTIONID2,
				PAYMINSTRUCTIONID3,
				PAYMINSTRUCTIONID4,
				ISSUINGSIGNATURE,
				SIACODE,
				BANKCENTRALBANKPURPOSECODE,
				BANKCENTRALBANKPURPOSETEXT,
				DBA,
				FOREIGNENTITYINDICATOR,
				COMBINEDFEDSTATEFILER,
				LASTFILINGINDICATOR,
				VALIDATE1099ONENTRY,
				LEGALFORMFR,
				SHIPPINGCALENDARID,
				ENTERPRISENUMBER,
				BRANCHNUMBER,
				CUSTOMSCUSTOMERNUMBER_FI,
				CUSTOMSLICENSENUMBER_FI,
				PLANNINGCOMPANY,
				TAXREPRESENTATIVE,
				FALLBACKINVENTLOCATIONID,
				ENABLENORWAY,
				ORGID,
				BANKACCTUSEDFOR1099,
				MODIFIEDDATETIME,
				DEL_MODIFIEDTIME,
				MODIFIEDBY,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.NAME,
				ins.ADDRESS,
				ins.PHONE,
				ins.TELEFAX,
				ins.BANK,
				ins.GIRO,
				ins.REGNUM,
				ins.COREGNUM,
				ins.VATNUM,
				ins.CURRENCYCODE,
				ins.IMPORTVATNUM,
				ins.ZIPCODE,
				ins.STATE,
				ins.COUNTY,
				ins.COUNTRYREGIONID,
				ins.TELEX,
				ins.URL,
				ins.EMAIL,
				ins.CELLULARPHONE,
				ins.PHONELOCAL,
				ins.UPSNUM,
				ins.TAX1099REGNUM,
				ins.NAMECONTROL,
				ins.TCC,
				ins.EUROCURRENCYCODE,
				ins.KEY_,
				ins.SECONDARYCURRENCYCODE,
				ins.DVRID,
				ins.LANGUAGEID,
				ins.INTRASTATCODE,
				ins.GIROCONTRACT,
				ins.GIROCONTRACTACCOUNT,
				ins.BRANCHID,
				ins.VATNUMBRANCHID,
				ins.IMPORTVATNUMBRANCHID,
				ins.ACTIVITYCODE,
				ins.STREET,
				ins.CITY,
				ins.CONVERSIONDATE,
				ins.PAGER,
				ins.SMS,
				ins.ADDRFORMAT,
				ins.COMPANYREGCOMFR,
				ins.PACKMATERIALFEELICENSENUM,
				ins.COMPANYIDNAF,
				ins.PAYMROUTINGDNB,
				ins.PAYMTRADERNUMBER,
				ins.PAYMINSTRUCTIONID1,
				ins.PAYMINSTRUCTIONID2,
				ins.PAYMINSTRUCTIONID3,
				ins.PAYMINSTRUCTIONID4,
				ins.ISSUINGSIGNATURE,
				ins.SIACODE,
				ins.BANKCENTRALBANKPURPOSECODE,
				ins.BANKCENTRALBANKPURPOSETEXT,
				ins.DBA,
				ins.FOREIGNENTITYINDICATOR,
				ins.COMBINEDFEDSTATEFILER,
				ins.LASTFILINGINDICATOR,
				ins.VALIDATE1099ONENTRY,
				ins.LEGALFORMFR,
				ins.SHIPPINGCALENDARID,
				ins.ENTERPRISENUMBER,
				ins.BRANCHNUMBER,
				ins.CUSTOMSCUSTOMERNUMBER_FI,
				ins.CUSTOMSLICENSENUMBER_FI,
				ins.PLANNINGCOMPANY,
				ins.TAXREPRESENTATIVE,
				ins.FALLBACKINVENTLOCATIONID,
				ins.ENABLENORWAY,
				ins.ORGID,
				ins.BANKACCTUSEDFOR1099,
				ins.MODIFIEDDATETIME,
				ins.DEL_MODIFIEDTIME,
				ins.MODIFIEDBY,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_SALESPARAMETERS]'))
begin
   drop trigger dbo.Update_SALESPARAMETERS
end

GO

create trigger Update_SALESPARAMETERS
on SALESPARAMETERS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.SALESPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DISC,
				KEY_,
				DATAAREAID,
				CALCPERIODICDISCS,
				CALCCUSTOMERDISCS,
				CLEARPERIODICDISCOUNTCACHE,
				CLEARPERIODICDISCOUNTCACHEMINUTES,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DISC,
				ins.KEY_,
				ins.DATAAREAID,
				ins.CALCPERIODICDISCS,
				ins.CALCCUSTOMERDISCS,
				ins.CLEARPERIODICDISCOUNTCACHE,
				ins.CLEARPERIODICDISCOUNTCACHEMINUTES,			
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.SALESPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DISC,
				KEY_,
				DATAAREAID,
				CALCPERIODICDISCS,
				CALCCUSTOMERDISCS,
				CLEARPERIODICDISCOUNTCACHE,
				CLEARPERIODICDISCOUNTCACHEMINUTES,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DISC,
				ins.KEY_,
				ins.DATAAREAID,
				ins.CALCPERIODICDISCS,
				ins.CALCCUSTOMERDISCS,
				ins.CLEARPERIODICDISCOUNTCACHE,
				ins.CLEARPERIODICDISCOUNTCACHEMINUTES,	
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PRICEPARAMETERS]'))
begin
   drop trigger dbo.Update_PRICEPARAMETERS
end

GO

create trigger Update_PRICEPARAMETERS
on PRICEPARAMETERS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PRICEPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				SALESPRICEACCOUNTITEM ,
				SALESLINEACCOUNTITEM ,
				SALESLINEACCOUNTGROUP ,
				SALESLINEACCOUNTALL ,
				SALESMULTILNACCOUNTGROUP ,
				SALESMULTILNACCOUNTALL ,
				SALESENDACCOUNTALL ,
				SALESPRICEGROUPITEM ,
				SALESLINEGROUPITEM ,
				SALESLINEGROUPGROUP ,
				SALESLINEGROUPALL ,
				SALESMULTILNGROUPGROUP ,
				SALESMULTILNGROUPALL ,
				SALESENDGROUPALL ,
				SALESPRICEALLITEM ,
				SALESLINEALLITEM ,
				SALESLINEALLGROUP ,
				SALESLINEALLALL ,
				SALESMULTILNALLGROUP ,
				SALESMULTILNALLALL ,
				SALESENDALLALL ,
				PURCHPRICEACCOUNTITEM ,
				PURCHLINEACCOUNTITEM ,
				PURCHLINEACCOUNTGROUP ,
				PURCHLINEACCOUNTALL ,
				PURCHMULTILNACCOUNTGROUP ,
				PURCHMULTILNACCOUNTALL ,
				PURCHENDACCOUNTALL ,
				PURCHPRICEGROUPITEM ,
				PURCHLINEGROUPITEM ,
				PURCHLINEGROUPGROUP ,
				PURCHLINEGROUPALL ,
				PURCHMULTILNGROUPGROUP ,
				PURCHMULTILNGROUPALL ,
				PURCHENDGROUPALL ,
				PURCHPRICEALLITEM ,
				PURCHLINEALLITEM ,
				PURCHLINEALLGROUP ,
				PURCHLINEALLALL ,
				PURCHMULTILNALLGROUP ,
				PURCHMULTILNALLALL ,
				PURCHENDALLALL ,
				KEY_ ,
				MODIFIEDDATE ,
				MODIFIEDTIME ,
				MODIFIEDBY ,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.SALESPRICEACCOUNTITEM ,
				ins.SALESLINEACCOUNTITEM ,
				ins.SALESLINEACCOUNTGROUP ,
				ins.SALESLINEACCOUNTALL ,
				ins.SALESMULTILNACCOUNTGROUP ,
				ins.SALESMULTILNACCOUNTALL ,
				ins.SALESENDACCOUNTALL ,
				ins.SALESPRICEGROUPITEM ,
				ins.SALESLINEGROUPITEM ,
				ins.SALESLINEGROUPGROUP ,
				ins.SALESLINEGROUPALL ,
				ins.SALESMULTILNGROUPGROUP ,
				ins.SALESMULTILNGROUPALL ,
				ins.SALESENDGROUPALL ,
				ins.SALESPRICEALLITEM ,
				ins.SALESLINEALLITEM ,
				ins.SALESLINEALLGROUP ,
				ins.SALESLINEALLALL ,
				ins.SALESMULTILNALLGROUP ,
				ins.SALESMULTILNALLALL ,
				ins.SALESENDALLALL ,
				ins.PURCHPRICEACCOUNTITEM ,
				ins.PURCHLINEACCOUNTITEM ,
				ins.PURCHLINEACCOUNTGROUP ,
				ins.PURCHLINEACCOUNTALL ,
				ins.PURCHMULTILNACCOUNTGROUP ,
				ins.PURCHMULTILNACCOUNTALL ,
				ins.PURCHENDACCOUNTALL ,
				ins.PURCHPRICEGROUPITEM ,
				ins.PURCHLINEGROUPITEM ,
				ins.PURCHLINEGROUPGROUP ,
				ins.PURCHLINEGROUPALL ,
				ins.PURCHMULTILNGROUPGROUP ,
				ins.PURCHMULTILNGROUPALL ,
				ins.PURCHENDGROUPALL ,
				ins.PURCHPRICEALLITEM ,
				ins.PURCHLINEALLITEM ,
				ins.PURCHLINEALLGROUP ,
				ins.PURCHLINEALLALL ,
				ins.PURCHMULTILNALLGROUP ,
				ins.PURCHMULTILNALLALL ,
				ins.PURCHENDALLALL ,
				ins.KEY_ ,
				ins.MODIFIEDDATE ,
				ins.MODIFIEDTIME ,
				ins.MODIFIEDBY ,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PRICEPARAMETERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				SALESPRICEACCOUNTITEM ,
				SALESLINEACCOUNTITEM ,
				SALESLINEACCOUNTGROUP ,
				SALESLINEACCOUNTALL ,
				SALESMULTILNACCOUNTGROUP ,
				SALESMULTILNACCOUNTALL ,
				SALESENDACCOUNTALL ,
				SALESPRICEGROUPITEM ,
				SALESLINEGROUPITEM ,
				SALESLINEGROUPGROUP ,
				SALESLINEGROUPALL ,
				SALESMULTILNGROUPGROUP ,
				SALESMULTILNGROUPALL ,
				SALESENDGROUPALL ,
				SALESPRICEALLITEM ,
				SALESLINEALLITEM ,
				SALESLINEALLGROUP ,
				SALESLINEALLALL ,
				SALESMULTILNALLGROUP ,
				SALESMULTILNALLALL ,
				SALESENDALLALL ,
				PURCHPRICEACCOUNTITEM ,
				PURCHLINEACCOUNTITEM ,
				PURCHLINEACCOUNTGROUP ,
				PURCHLINEACCOUNTALL ,
				PURCHMULTILNACCOUNTGROUP ,
				PURCHMULTILNACCOUNTALL ,
				PURCHENDACCOUNTALL ,
				PURCHPRICEGROUPITEM ,
				PURCHLINEGROUPITEM ,
				PURCHLINEGROUPGROUP ,
				PURCHLINEGROUPALL ,
				PURCHMULTILNGROUPGROUP ,
				PURCHMULTILNGROUPALL ,
				PURCHENDGROUPALL ,
				PURCHPRICEALLITEM ,
				PURCHLINEALLITEM ,
				PURCHLINEALLGROUP ,
				PURCHLINEALLALL ,
				PURCHMULTILNALLGROUP ,
				PURCHMULTILNALLALL ,
				PURCHENDALLALL ,
				KEY_ ,
				MODIFIEDDATE ,
				MODIFIEDTIME ,
				MODIFIEDBY ,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.SALESPRICEACCOUNTITEM ,
				ins.SALESLINEACCOUNTITEM ,
				ins.SALESLINEACCOUNTGROUP ,
				ins.SALESLINEACCOUNTALL ,
				ins.SALESMULTILNACCOUNTGROUP ,
				ins.SALESMULTILNACCOUNTALL ,
				ins.SALESENDACCOUNTALL ,
				ins.SALESPRICEGROUPITEM ,
				ins.SALESLINEGROUPITEM ,
				ins.SALESLINEGROUPGROUP ,
				ins.SALESLINEGROUPALL ,
				ins.SALESMULTILNGROUPGROUP ,
				ins.SALESMULTILNGROUPALL ,
				ins.SALESENDGROUPALL ,
				ins.SALESPRICEALLITEM ,
				ins.SALESLINEALLITEM ,
				ins.SALESLINEALLGROUP ,
				ins.SALESLINEALLALL ,
				ins.SALESMULTILNALLGROUP ,
				ins.SALESMULTILNALLALL ,
				ins.SALESENDALLALL ,
				ins.PURCHPRICEACCOUNTITEM ,
				ins.PURCHLINEACCOUNTITEM ,
				ins.PURCHLINEACCOUNTGROUP ,
				ins.PURCHLINEACCOUNTALL ,
				ins.PURCHMULTILNACCOUNTGROUP ,
				ins.PURCHMULTILNACCOUNTALL ,
				ins.PURCHENDACCOUNTALL ,
				ins.PURCHPRICEGROUPITEM ,
				ins.PURCHLINEGROUPITEM ,
				ins.PURCHLINEGROUPGROUP ,
				ins.PURCHLINEGROUPALL ,
				ins.PURCHMULTILNGROUPGROUP ,
				ins.PURCHMULTILNGROUPALL ,
				ins.PURCHENDGROUPALL ,
				ins.PURCHPRICEALLITEM ,
				ins.PURCHLINEALLITEM ,
				ins.PURCHLINEALLGROUP ,
				ins.PURCHLINEALLALL ,
				ins.PURCHMULTILNALLGROUP ,
				ins.PURCHMULTILNALLALL ,
				ins.PURCHENDALLALL ,
				ins.KEY_ ,
				ins.MODIFIEDDATE ,
				ins.MODIFIEDTIME ,
				ins.MODIFIEDBY ,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTTABLE]'))
begin
   drop trigger dbo.Update_INVENTTABLE
end

GO

create trigger Update_INVENTTABLE
on INVENTTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ITEMGROUPID,
				ITEMID,
				ITEMNAME,
				ITEMTYPE,
				PURCHMODEL,
				HEIGHT,
				WIDTH,
				SALESMODEL,
				REQGROUPID,
				PRIMARYVENDORID,
				NETWEIGHT,
				DEPTH,
				UNITVOLUME,
				DENSITY,
				DIMENSION,
				DIMENSION2_,
				DIMENSION3_,
				COSTMODEL,
				USEALTITEMID,
				ALTITEMID,
				BOMMANUALCONSUMP,
				BOMMANUALRECEIPT,
				STOPEXPLODE,
				BATCHNUMGROUPID,
				PRODPOOLID,
				PROPERTYID,
				ABCTIEUP,
				ABCREVENUE,
				ABCVALUE,
				ABCCONTRIBUTIONMARGIN,
				SALESPERCENTMARKUP,
				SALESCONTRIBUTIONRATIO,
				SALESPRICEMODELBASIC,
				MINAVERAGESETTLE,
				NAMEALIAS,
				PRODGROUPID,
				PROJCATEGORYID,
				GROSSDEPTH,
				GROSSWIDTH,
				GROSSHEIGHT,
				SORTCODE,
				SERIALNUMGROUPID,
				DIMGROUPID,
				MODELGROUPID,
				ITEMBUYERGROUPID,
				WMSPICKINGQTYTIME,
				TARAWEIGHT,
				ITEMDIMCOMBINATIONAUTOCREATE,
				ITEMDIMCOSTPRICE,
				ITEMIDCOMPANY,
				PBAITEMCONFIGURABLE,
				PBAINVENTITEMGROUPID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ITEMGROUPID,
				ins.ITEMID,
				ins.ITEMNAME,
				ins.ITEMTYPE,
				ins.PURCHMODEL,
				ins.HEIGHT,
				ins.WIDTH,
				ins.SALESMODEL,
				ins.REQGROUPID,
				ins.PRIMARYVENDORID,
				ins.NETWEIGHT,
				ins.DEPTH,
				ins.UNITVOLUME,
				ins.DENSITY,
				ins.DIMENSION,
				ins.DIMENSION2_,
				ins.DIMENSION3_,
				ins.COSTMODEL,
				ins.USEALTITEMID,
				ins.ALTITEMID,
				ins.BOMMANUALCONSUMP,
				ins.BOMMANUALRECEIPT,
				ins.STOPEXPLODE,
				ins.BATCHNUMGROUPID,
				ins.PRODPOOLID,
				ins.PROPERTYID,
				ins.ABCTIEUP,
				ins.ABCREVENUE,
				ins.ABCVALUE,
				ins.ABCCONTRIBUTIONMARGIN,
				ins.SALESPERCENTMARKUP,
				ins.SALESCONTRIBUTIONRATIO,
				ins.SALESPRICEMODELBASIC,
				ins.MINAVERAGESETTLE,
				ins.NAMEALIAS,
				ins.PRODGROUPID,
				ins.PROJCATEGORYID,
				ins.GROSSDEPTH,
				ins.GROSSWIDTH,
				ins.GROSSHEIGHT,
				ins.SORTCODE,
				ins.SERIALNUMGROUPID,
				ins.DIMGROUPID,
				ins.MODELGROUPID,
				ins.ITEMBUYERGROUPID,
				ins.WMSPICKINGQTYTIME,
				ins.TARAWEIGHT,
				ins.ITEMDIMCOMBINATIONAUTOCREATE,
				ins.ITEMDIMCOSTPRICE,
				ins.ITEMIDCOMPANY,
				ins.PBAITEMCONFIGURABLE,
				ins.PBAINVENTITEMGROUPID,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ITEMGROUPID,
				ITEMID,
				ITEMNAME,
				ITEMTYPE,
				PURCHMODEL,
				HEIGHT,
				WIDTH,
				SALESMODEL,
				REQGROUPID,
				PRIMARYVENDORID,
				NETWEIGHT,
				DEPTH,
				UNITVOLUME,
				DENSITY,
				DIMENSION,
				DIMENSION2_,
				DIMENSION3_,
				COSTMODEL,
				USEALTITEMID,
				ALTITEMID,
				BOMMANUALCONSUMP,
				BOMMANUALRECEIPT,
				STOPEXPLODE,
				BATCHNUMGROUPID,
				PRODPOOLID,
				PROPERTYID,
				ABCTIEUP,
				ABCREVENUE,
				ABCVALUE,
				ABCCONTRIBUTIONMARGIN,
				SALESPERCENTMARKUP,
				SALESCONTRIBUTIONRATIO,
				SALESPRICEMODELBASIC,
				MINAVERAGESETTLE,
				NAMEALIAS,
				PRODGROUPID,
				PROJCATEGORYID,
				GROSSDEPTH,
				GROSSWIDTH,
				GROSSHEIGHT,
				SORTCODE,
				SERIALNUMGROUPID,
				DIMGROUPID,
				MODELGROUPID,
				ITEMBUYERGROUPID,
				WMSPICKINGQTYTIME,
				TARAWEIGHT,
				ITEMDIMCOMBINATIONAUTOCREATE,
				ITEMDIMCOSTPRICE,
				ITEMIDCOMPANY,
				PBAITEMCONFIGURABLE,
				PBAINVENTITEMGROUPID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ITEMGROUPID,
				ins.ITEMID,
				ins.ITEMNAME,
				ins.ITEMTYPE,
				ins.PURCHMODEL,
				ins.HEIGHT,
				ins.WIDTH,
				ins.SALESMODEL,
				ins.REQGROUPID,
				ins.PRIMARYVENDORID,
				ins.NETWEIGHT,
				ins.DEPTH,
				ins.UNITVOLUME,
				ins.DENSITY,
				ins.DIMENSION,
				ins.DIMENSION2_,
				ins.DIMENSION3_,
				ins.COSTMODEL,
				ins.USEALTITEMID,
				ins.ALTITEMID,
				ins.BOMMANUALCONSUMP,
				ins.BOMMANUALRECEIPT,
				ins.STOPEXPLODE,
				ins.BATCHNUMGROUPID,
				ins.PRODPOOLID,
				ins.PROPERTYID,
				ins.ABCTIEUP,
				ins.ABCREVENUE,
				ins.ABCVALUE,
				ins.ABCCONTRIBUTIONMARGIN,
				ins.SALESPERCENTMARKUP,
				ins.SALESCONTRIBUTIONRATIO,
				ins.SALESPRICEMODELBASIC,
				ins.MINAVERAGESETTLE,
				ins.NAMEALIAS,
				ins.PRODGROUPID,
				ins.PROJCATEGORYID,
				ins.GROSSDEPTH,
				ins.GROSSWIDTH,
				ins.GROSSHEIGHT,
				ins.SORTCODE,
				ins.SERIALNUMGROUPID,
				ins.DIMGROUPID,
				ins.MODELGROUPID,
				ins.ITEMBUYERGROUPID,
				ins.WMSPICKINGQTYTIME,
				ins.TARAWEIGHT,
				ins.ITEMDIMCOMBINATIONAUTOCREATE,
				ins.ITEMDIMCOSTPRICE,
				ins.ITEMIDCOMPANY,
				ins.PBAITEMCONFIGURABLE,
				ins.PBAINVENTITEMGROUPID,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSPERIODICDISCOUNT]'))
begin
   drop trigger dbo.Update_POSPERIODICDISCOUNT
end

GO

create trigger Update_POSPERIODICDISCOUNT
on POSPERIODICDISCOUNT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSPERIODICDISCOUNTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				OFFERID,
				DESCRIPTION,
				STATUS,
				PDTYPE,
				PRIORITY,
				DISCVALIDPERIODID,
				DISCOUNTTYPE,
				SAMEDIFFMMLINES,
				NOOFLINESTOTRIGGER,
				DEALPRICEVALUE,
				DISCOUNTPCTVALUE,
				DISCOUNTAMOUNTVALUE,
				NOOFLEASTEXPITEMS,
				PRICEGROUP,
				NOOFTIMESAPPLICABLE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.OFFERID,
				ins.DESCRIPTION,
				ins.STATUS,
				ins.PDTYPE,
				ins.PRIORITY,
				ins.DISCVALIDPERIODID,
				ins.DISCOUNTTYPE,
				ins.SAMEDIFFMMLINES,
				ins.NOOFLINESTOTRIGGER,
				ins.DEALPRICEVALUE,
				ins.DISCOUNTPCTVALUE,
				ins.DISCOUNTAMOUNTVALUE,
				ins.NOOFLEASTEXPITEMS,
				ins.PRICEGROUP,
				ins.NOOFTIMESAPPLICABLE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSPERIODICDISCOUNTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				OFFERID,
				DESCRIPTION,
				STATUS,
				PDTYPE,
				PRIORITY,
				DISCVALIDPERIODID,
				DISCOUNTTYPE,
				SAMEDIFFMMLINES,
				NOOFLINESTOTRIGGER,
				DEALPRICEVALUE,
				DISCOUNTPCTVALUE,
				DISCOUNTAMOUNTVALUE,
				NOOFLEASTEXPITEMS,
				PRICEGROUP,
				NOOFTIMESAPPLICABLE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.OFFERID,
				ins.DESCRIPTION,
				ins.STATUS,
				ins.PDTYPE,
				ins.PRIORITY,
				ins.DISCVALIDPERIODID,
				ins.DISCOUNTTYPE,
				ins.SAMEDIFFMMLINES,
				ins.NOOFLINESTOTRIGGER,
				ins.DEALPRICEVALUE,
				ins.DISCOUNTPCTVALUE,
				ins.DISCOUNTAMOUNTVALUE,
				ins.NOOFLEASTEXPITEMS,
				ins.PRICEGROUP,
				ins.NOOFTIMESAPPLICABLE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSPERIODICDISCOUNTLINE]'))
begin
   drop trigger dbo.Update_POSPERIODICDISCOUNTLINE
end

GO

create trigger Update_POSPERIODICDISCOUNTLINE
on POSPERIODICDISCOUNTLINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSPERIODICDISCOUNTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				OFFERID,
				PRODUCTTYPE,
				ID,
				DEALPRICEORDISCPCT,
				LINEGROUP,
				DISCTYPE,
				DATAAREAID,
				POSPERIODICDISCOUNTLINEGUID,
				Deleted				
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.OFFERID,
				ins.PRODUCTTYPE,
				ins.ID,
				ins.DEALPRICEORDISCPCT,
				ins.LINEGROUP,
				ins.DISCTYPE,
				ins.DATAAREAID,
				ins.POSPERIODICDISCOUNTLINEGUID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSPERIODICDISCOUNTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				OFFERID,
				PRODUCTTYPE,
				ID,
				DEALPRICEORDISCPCT,
				LINEGROUP,
				DISCTYPE,
				DATAAREAID,
				POSPERIODICDISCOUNTLINEGUID,
				Deleted				
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.OFFERID,
				ins.PRODUCTTYPE,
				ins.ID,
				ins.DEALPRICEORDISCPCT,
				ins.LINEGROUP,
				ins.DISCTYPE,
				ins.DATAAREAID,
				ins.POSPERIODICDISCOUNTLINEGUID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSDISCVALIDATIONPERIOD]'))
begin
   drop trigger dbo.Update_POSDISCVALIDATIONPERIOD
end

GO

create trigger Update_POSDISCVALIDATIONPERIOD
on POSDISCVALIDATIONPERIOD after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSDISCVALIDATIONPERIODLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ID ,
				DESCRIPTION ,
				STARTINGDATE  ,
				ENDINGDATE  ,
				STARTINGTIME  ,
				ENDINGTIME  ,
				TIMEWITHINBOUNDS  ,
				MONSTARTINGTIME  ,
				MONENDINGTIME  ,
				MONWITHINBOUNDS  ,
				TUESTARTINGTIME  ,
				TUEENDINGTIME  ,
				TUEWITHINBOUNDS  ,
				WEDSTARTINGTIME  ,
				WEDENDINGTIME  ,
				WEDWITHINBOUNDS  ,
				THUSTARTINGTIME  ,
				THUENDINGTIME  ,
				THUWITHINBOUNDS  ,
				FRISTARTINGTIME  ,
				FRIENDINGTIME  ,
				FRIWITHINBOUNDS  ,
				SATSTARTINGTIME  ,
				SATENDINGTIME  ,
				SATWITHINBOUNDS  ,
				SUNSTARTINGTIME  ,
				SUNENDINGTIME  ,
				SUNWITHINBOUNDS  ,
				ENDTIMEAFTERMID  ,
				MONAFTERMIDNIGHT  ,
				TUEAFTERMIDNIGHT  ,
				WEDAFTERMIDNIGHT  ,
				THUAFTERMIDNIGHT  ,
				FRIAFTERMIDNIGHT  ,
				SATAFTERMIDNIGHT  ,
				SUNAFTERMIDNIGHT  ,
				DATAAREAID ,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ID ,
				ins.DESCRIPTION ,
				ins.STARTINGDATE  ,
				ins.ENDINGDATE  ,
				ins.STARTINGTIME  ,
				ins.ENDINGTIME  ,
				ins.TIMEWITHINBOUNDS  ,
				ins.MONSTARTINGTIME  ,
				ins.MONENDINGTIME  ,
				ins.MONWITHINBOUNDS  ,
				ins.TUESTARTINGTIME  ,
				ins.TUEENDINGTIME  ,
				ins.TUEWITHINBOUNDS  ,
				ins.WEDSTARTINGTIME  ,
				ins.WEDENDINGTIME  ,
				ins.WEDWITHINBOUNDS  ,
				ins.THUSTARTINGTIME  ,
				ins.THUENDINGTIME  ,
				ins.THUWITHINBOUNDS  ,
				ins.FRISTARTINGTIME  ,
				ins.FRIENDINGTIME  ,
				ins.FRIWITHINBOUNDS  ,
				ins.SATSTARTINGTIME  ,
				ins.SATENDINGTIME  ,
				ins.SATWITHINBOUNDS  ,
				ins.SUNSTARTINGTIME  ,
				ins.SUNENDINGTIME  ,
				ins.SUNWITHINBOUNDS  ,
				ins.ENDTIMEAFTERMID  ,
				ins.MONAFTERMIDNIGHT  ,
				ins.TUEAFTERMIDNIGHT  ,
				ins.WEDAFTERMIDNIGHT  ,
				ins.THUAFTERMIDNIGHT  ,
				ins.FRIAFTERMIDNIGHT  ,
				ins.SATAFTERMIDNIGHT  ,
				ins.SUNAFTERMIDNIGHT  ,
				ins.DATAAREAID ,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSDISCVALIDATIONPERIODLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ID ,
				DESCRIPTION ,
				STARTINGDATE  ,
				ENDINGDATE  ,
				STARTINGTIME  ,
				ENDINGTIME  ,
				TIMEWITHINBOUNDS  ,
				MONSTARTINGTIME  ,
				MONENDINGTIME  ,
				MONWITHINBOUNDS  ,
				TUESTARTINGTIME  ,
				TUEENDINGTIME  ,
				TUEWITHINBOUNDS  ,
				WEDSTARTINGTIME  ,
				WEDENDINGTIME  ,
				WEDWITHINBOUNDS  ,
				THUSTARTINGTIME  ,
				THUENDINGTIME  ,
				THUWITHINBOUNDS  ,
				FRISTARTINGTIME  ,
				FRIENDINGTIME  ,
				FRIWITHINBOUNDS  ,
				SATSTARTINGTIME  ,
				SATENDINGTIME  ,
				SATWITHINBOUNDS  ,
				SUNSTARTINGTIME  ,
				SUNENDINGTIME  ,
				SUNWITHINBOUNDS  ,
				ENDTIMEAFTERMID  ,
				MONAFTERMIDNIGHT  ,
				TUEAFTERMIDNIGHT  ,
				WEDAFTERMIDNIGHT  ,
				THUAFTERMIDNIGHT  ,
				FRIAFTERMIDNIGHT  ,
				SATAFTERMIDNIGHT  ,
				SUNAFTERMIDNIGHT  ,
				DATAAREAID ,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ID ,
				ins.DESCRIPTION ,
				ins.STARTINGDATE  ,
				ins.ENDINGDATE  ,
				ins.STARTINGTIME  ,
				ins.ENDINGTIME  ,
				ins.TIMEWITHINBOUNDS  ,
				ins.MONSTARTINGTIME  ,
				ins.MONENDINGTIME  ,
				ins.MONWITHINBOUNDS  ,
				ins.TUESTARTINGTIME  ,
				ins.TUEENDINGTIME  ,
				ins.TUEWITHINBOUNDS  ,
				ins.WEDSTARTINGTIME  ,
				ins.WEDENDINGTIME  ,
				ins.WEDWITHINBOUNDS  ,
				ins.THUSTARTINGTIME  ,
				ins.THUENDINGTIME  ,
				ins.THUWITHINBOUNDS  ,
				ins.FRISTARTINGTIME  ,
				ins.FRIENDINGTIME  ,
				ins.FRIWITHINBOUNDS  ,
				ins.SATSTARTINGTIME  ,
				ins.SATENDINGTIME  ,
				ins.SATWITHINBOUNDS  ,
				ins.SUNSTARTINGTIME  ,
				ins.SUNENDINGTIME  ,
				ins.SUNWITHINBOUNDS  ,
				ins.ENDTIMEAFTERMID  ,
				ins.MONAFTERMIDNIGHT  ,
				ins.TUEAFTERMIDNIGHT  ,
				ins.WEDAFTERMIDNIGHT  ,
				ins.THUAFTERMIDNIGHT  ,
				ins.FRIAFTERMIDNIGHT  ,
				ins.SATAFTERMIDNIGHT  ,
				ins.SUNAFTERMIDNIGHT  ,
				ins.DATAAREAID ,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSMULTIBUYDISCOUNTLINE]'))
begin
   drop trigger dbo.Update_POSMULTIBUYDISCOUNTLINE
end

GO

create trigger Update_POSMULTIBUYDISCOUNTLINE
on POSMULTIBUYDISCOUNTLINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSMULTIBUYDISCOUNTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				OFFERID,
				MINQUANTITY,
				UNITPRICEORDISCPCT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.OFFERID,
				ins.MINQUANTITY,
				ins.UNITPRICEORDISCPCT,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSMULTIBUYDISCOUNTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				OFFERID,
				MINQUANTITY,
				UNITPRICEORDISCPCT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.OFFERID,
				ins.MINQUANTITY,
				ins.UNITPRICEORDISCPCT,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_HOSPITALITYSETUP]'))
begin
   drop trigger dbo.Update_HOSPITALITYSETUP
end

GO

create trigger Update_HOSPITALITYSETUP
on HOSPITALITYSETUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.HOSPITALITYSETUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				SETUP,
				DELIVERYSALESTYPE,
				DINEINSALESTYPE,
				ORDERPROCESSTIMEMIN,
				TABLEFREECOLORB,
				TABLENOTAVAILCOLORB,
				TABLELOCKEDCOLORB,
				ORDERNOTPRINTEDCOLORB,
				ORDERPRINTEDCOLORB,
				ORDERSTARTEDCOLORB,
				ORDERFINISHEDCOLORB,
				ORDERCONFIRMEDCOLORB,
				TABLEFREECOLORF,
				TABLENOTAVAILCOLORF,
				TABLELOCKEDCOLORF,
				ORDERNOTPRINTEDCOLORF,
				ORDERPRINTEDCOLORF,
				ORDERSTARTEDCOLORF,
				ORDERFINISHEDCOLORF,
				ORDERCONFIRMEDCOLORF,
				CONFIRMSTATIONPRINTING,
				REQUESTNOOFGUESTS,
				NOOFDINEINTABLESCOL,
				NOOFDINEINTABLESROWS,
				STATIONPRINTING,
				DINEINTABLELOCKING,
				DINEINTABLESELECTION,
				PERIOD1TIMEFROM,
				PERIOD1TIMETO,
				PERIOD2TIMEFROM,
				PERIOD2TIMETO,
				PERIOD3TIMEFROM,
				PERIOD3TIMETO,
				PERIOD4TIMEFROM,
				PERIOD4TIMETO,
				AUTOLOGOFFATPOSEXIT,
				TAKEOUTSALESTYPE,
				PREORDERSALESTYPE,
				LOGSTATIONPRINTING,
				POPULATEDELIVERYINFOCODES,
				ALLOWPREORDERS,
				TAKEOUTNONAMENO,
				ADVPREORDPRINTMIN,
				CLOSETRIPONDEPART,
				DELPROGRESSSTATUSINUSE,
				DAYSBOMPRINTEXIST,
				DAYSBOMMONITOREXIST,
				DAYSDRIVERTRIPSEXIST,
				POSTERMINALPRINTPREORDERS,
				DISPLAYTIMEATORDERTAKING,
				NORMALPOSSALESTYPE,
				ORDLISTSCROLLPAGESIZE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.SETUP,
				ins.DELIVERYSALESTYPE,
				ins.DINEINSALESTYPE,
				ins.ORDERPROCESSTIMEMIN,
				ins.TABLEFREECOLORB,
				ins.TABLENOTAVAILCOLORB,
				ins.TABLELOCKEDCOLORB,
				ins.ORDERNOTPRINTEDCOLORB,
				ins.ORDERPRINTEDCOLORB,
				ins.ORDERSTARTEDCOLORB,
				ins.ORDERFINISHEDCOLORB,
				ins.ORDERCONFIRMEDCOLORB,
				ins.TABLEFREECOLORF,
				ins.TABLENOTAVAILCOLORF,
				ins.TABLELOCKEDCOLORF,
				ins.ORDERNOTPRINTEDCOLORF,
				ins.ORDERPRINTEDCOLORF,
				ins.ORDERSTARTEDCOLORF,
				ins.ORDERFINISHEDCOLORF,
				ins.ORDERCONFIRMEDCOLORF,
				ins.CONFIRMSTATIONPRINTING,
				ins.REQUESTNOOFGUESTS,
				ins.NOOFDINEINTABLESCOL,
				ins.NOOFDINEINTABLESROWS,
				ins.STATIONPRINTING,
				ins.DINEINTABLELOCKING,
				ins.DINEINTABLESELECTION,
				ins.PERIOD1TIMEFROM,
				ins.PERIOD1TIMETO,
				ins.PERIOD2TIMEFROM,
				ins.PERIOD2TIMETO,
				ins.PERIOD3TIMEFROM,
				ins.PERIOD3TIMETO,
				ins.PERIOD4TIMEFROM,
				ins.PERIOD4TIMETO,
				ins.AUTOLOGOFFATPOSEXIT,
				ins.TAKEOUTSALESTYPE,
				ins.PREORDERSALESTYPE,
				ins.LOGSTATIONPRINTING,
				ins.POPULATEDELIVERYINFOCODES,
				ins.ALLOWPREORDERS,
				ins.TAKEOUTNONAMENO,
				ins.ADVPREORDPRINTMIN,
				ins.CLOSETRIPONDEPART,
				ins.DELPROGRESSSTATUSINUSE,
				ins.DAYSBOMPRINTEXIST,
				ins.DAYSBOMMONITOREXIST,
				ins.DAYSDRIVERTRIPSEXIST,
				ins.POSTERMINALPRINTPREORDERS,
				ins.DISPLAYTIMEATORDERTAKING,
				ins.NORMALPOSSALESTYPE,
				ins.ORDLISTSCROLLPAGESIZE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.HOSPITALITYSETUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				SETUP,
				DELIVERYSALESTYPE,
				DINEINSALESTYPE,
				ORDERPROCESSTIMEMIN,
				TABLEFREECOLORB,
				TABLENOTAVAILCOLORB,
				TABLELOCKEDCOLORB,
				ORDERNOTPRINTEDCOLORB,
				ORDERPRINTEDCOLORB,
				ORDERSTARTEDCOLORB,
				ORDERFINISHEDCOLORB,
				ORDERCONFIRMEDCOLORB,
				TABLEFREECOLORF,
				TABLENOTAVAILCOLORF,
				TABLELOCKEDCOLORF,
				ORDERNOTPRINTEDCOLORF,
				ORDERPRINTEDCOLORF,
				ORDERSTARTEDCOLORF,
				ORDERFINISHEDCOLORF,
				ORDERCONFIRMEDCOLORF,
				CONFIRMSTATIONPRINTING,
				REQUESTNOOFGUESTS,
				NOOFDINEINTABLESCOL,
				NOOFDINEINTABLESROWS,
				STATIONPRINTING,
				DINEINTABLELOCKING,
				DINEINTABLESELECTION,
				PERIOD1TIMEFROM,
				PERIOD1TIMETO,
				PERIOD2TIMEFROM,
				PERIOD2TIMETO,
				PERIOD3TIMEFROM,
				PERIOD3TIMETO,
				PERIOD4TIMEFROM,
				PERIOD4TIMETO,
				AUTOLOGOFFATPOSEXIT,
				TAKEOUTSALESTYPE,
				PREORDERSALESTYPE,
				LOGSTATIONPRINTING,
				POPULATEDELIVERYINFOCODES,
				ALLOWPREORDERS,
				TAKEOUTNONAMENO,
				ADVPREORDPRINTMIN,
				CLOSETRIPONDEPART,
				DELPROGRESSSTATUSINUSE,
				DAYSBOMPRINTEXIST,
				DAYSBOMMONITOREXIST,
				DAYSDRIVERTRIPSEXIST,
				POSTERMINALPRINTPREORDERS,
				DISPLAYTIMEATORDERTAKING,
				NORMALPOSSALESTYPE,
				ORDLISTSCROLLPAGESIZE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.SETUP,
				ins.DELIVERYSALESTYPE,
				ins.DINEINSALESTYPE,
				ins.ORDERPROCESSTIMEMIN,
				ins.TABLEFREECOLORB,
				ins.TABLENOTAVAILCOLORB,
				ins.TABLELOCKEDCOLORB,
				ins.ORDERNOTPRINTEDCOLORB,
				ins.ORDERPRINTEDCOLORB,
				ins.ORDERSTARTEDCOLORB,
				ins.ORDERFINISHEDCOLORB,
				ins.ORDERCONFIRMEDCOLORB,
				ins.TABLEFREECOLORF,
				ins.TABLENOTAVAILCOLORF,
				ins.TABLELOCKEDCOLORF,
				ins.ORDERNOTPRINTEDCOLORF,
				ins.ORDERPRINTEDCOLORF,
				ins.ORDERSTARTEDCOLORF,
				ins.ORDERFINISHEDCOLORF,
				ins.ORDERCONFIRMEDCOLORF,
				ins.CONFIRMSTATIONPRINTING,
				ins.REQUESTNOOFGUESTS,
				ins.NOOFDINEINTABLESCOL,
				ins.NOOFDINEINTABLESROWS,
				ins.STATIONPRINTING,
				ins.DINEINTABLELOCKING,
				ins.DINEINTABLESELECTION,
				ins.PERIOD1TIMEFROM,
				ins.PERIOD1TIMETO,
				ins.PERIOD2TIMEFROM,
				ins.PERIOD2TIMETO,
				ins.PERIOD3TIMEFROM,
				ins.PERIOD3TIMETO,
				ins.PERIOD4TIMEFROM,
				ins.PERIOD4TIMETO,
				ins.AUTOLOGOFFATPOSEXIT,
				ins.TAKEOUTSALESTYPE,
				ins.PREORDERSALESTYPE,
				ins.LOGSTATIONPRINTING,
				ins.POPULATEDELIVERYINFOCODES,
				ins.ALLOWPREORDERS,
				ins.TAKEOUTNONAMENO,
				ins.ADVPREORDPRINTMIN,
				ins.CLOSETRIPONDEPART,
				ins.DELPROGRESSSTATUSINUSE,
				ins.DAYSBOMPRINTEXIST,
				ins.DAYSBOMMONITOREXIST,
				ins.DAYSDRIVERTRIPSEXIST,
				ins.POSTERMINALPRINTPREORDERS,
				ins.DISPLAYTIMEATORDERTAKING,
				ins.NORMALPOSSALESTYPE,
				ins.ORDLISTSCROLLPAGESIZE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINVENTITEMRETAILGROUP]'))
begin
   drop trigger dbo.Update_RBOINVENTITEMRETAILGROUP
end

GO

create trigger Update_RBOINVENTITEMRETAILGROUP
on RBOINVENTITEMRETAILGROUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOINVENTITEMRETAILGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GROUPID,
				NAME,
				NAMEALIAS,
				DEPARTMENTID,
				SIZEGROUPID,
				COLORGROUPID,
				ITEMREPORTNAME,
				SHELFREPORTNAME,
				DISPENSEPRINTERGROUPID,
				BARCODESETUPID,
				DISPENSEPRINTINGDISABLED,
				USEEANSTANDARDBARCODE,
				POSINVENTORYLOOKUP,
				STYLEGROUPID,
				FSHREPLENISHMENTRULEID,
				ITEMGROUPID,
				INVENTLOCATIONIDFORPURCHA16016,
				INVENTLOCATIONIDFORINVENTORY,
				INVENTLOCATIONIDFORSALESORDER,
				INVENTMODELGROUPID,
				INVENTDIMGROUPID,
				SALESTAXITEMGROUP,
				PURCHASETAXITEMGROUP,
				BASECOMPARISONUNITCODE,
				KEYINGINPRICE,
				DATAAREAID,
				LSRCOMMISSIONGROUPID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.NAMEALIAS,
				ins.DEPARTMENTID,
				ins.SIZEGROUPID,
				ins.COLORGROUPID,
				ins.ITEMREPORTNAME,
				ins.SHELFREPORTNAME,
				ins.DISPENSEPRINTERGROUPID,
				ins.BARCODESETUPID,
				ins.DISPENSEPRINTINGDISABLED,
				ins.USEEANSTANDARDBARCODE,
				ins.POSINVENTORYLOOKUP,
				ins.STYLEGROUPID,
				ins.FSHREPLENISHMENTRULEID,
				ins.ITEMGROUPID,
				ins.INVENTLOCATIONIDFORPURCHA16016,
				ins.INVENTLOCATIONIDFORINVENTORY,
				ins.INVENTLOCATIONIDFORSALESORDER,
				ins.INVENTMODELGROUPID,
				ins.INVENTDIMGROUPID,
				ins.SALESTAXITEMGROUP,
				ins.PURCHASETAXITEMGROUP,
				ins.BASECOMPARISONUNITCODE,
				ins.KEYINGINPRICE,
				ins.DATAAREAID,
				ins.LSRCOMMISSIONGROUPID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOINVENTITEMRETAILGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GROUPID,
				NAME,
				NAMEALIAS,
				DEPARTMENTID,
				SIZEGROUPID,
				COLORGROUPID,
				ITEMREPORTNAME,
				SHELFREPORTNAME,
				DISPENSEPRINTERGROUPID,
				BARCODESETUPID,
				DISPENSEPRINTINGDISABLED,
				USEEANSTANDARDBARCODE,
				POSINVENTORYLOOKUP,
				STYLEGROUPID,
				FSHREPLENISHMENTRULEID,
				ITEMGROUPID,
				INVENTLOCATIONIDFORPURCHA16016,
				INVENTLOCATIONIDFORINVENTORY,
				INVENTLOCATIONIDFORSALESORDER,
				INVENTMODELGROUPID,
				INVENTDIMGROUPID,
				SALESTAXITEMGROUP,
				PURCHASETAXITEMGROUP,
				BASECOMPARISONUNITCODE,
				KEYINGINPRICE,
				DATAAREAID,
				LSRCOMMISSIONGROUPID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.NAMEALIAS,
				ins.DEPARTMENTID,
				ins.SIZEGROUPID,
				ins.COLORGROUPID,
				ins.ITEMREPORTNAME,
				ins.SHELFREPORTNAME,
				ins.DISPENSEPRINTERGROUPID,
				ins.BARCODESETUPID,
				ins.DISPENSEPRINTINGDISABLED,
				ins.USEEANSTANDARDBARCODE,
				ins.POSINVENTORYLOOKUP,
				ins.STYLEGROUPID,
				ins.FSHREPLENISHMENTRULEID,
				ins.ITEMGROUPID,
				ins.INVENTLOCATIONIDFORPURCHA16016,
				ins.INVENTLOCATIONIDFORINVENTORY,
				ins.INVENTLOCATIONIDFORSALESORDER,
				ins.INVENTMODELGROUPID,
				ins.INVENTDIMGROUPID,
				ins.SALESTAXITEMGROUP,
				ins.PURCHASETAXITEMGROUP,
				ins.BASECOMPARISONUNITCODE,
				ins.KEYINGINPRICE,
				ins.DATAAREAID,
				ins.LSRCOMMISSIONGROUPID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINVENTITEMDEPARTMENT]'))
begin
   drop trigger dbo.Update_RBOINVENTITEMDEPARTMENT
end

GO

create trigger Update_RBOINVENTITEMDEPARTMENT
on RBOINVENTITEMDEPARTMENT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOINVENTITEMDEPARTMENTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DEPARTMENTID,
				NAME,
				DEFAULTPROFIT,
				DISPENSEPRINTERGROUPID,
				DISPENSEPRINTERSEQNUM,
				NAMEALIAS,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DEPARTMENTID,
				ins.NAME,
				ins.DEFAULTPROFIT,
				ins.DISPENSEPRINTERGROUPID,
				ins.DISPENSEPRINTERSEQNUM,
				ins.NAMEALIAS,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOINVENTITEMDEPARTMENTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				DEPARTMENTID,
				NAME,
				DEFAULTPROFIT,
				DISPENSEPRINTERGROUPID,
				DISPENSEPRINTERSEQNUM,
				NAMEALIAS,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.DEPARTMENTID,
				ins.NAME,
				ins.DEFAULTPROFIT,
				ins.DISPENSEPRINTERGROUPID,
				ins.DISPENSEPRINTERSEQNUM,
				ins.NAMEALIAS,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSPECIALGROUP]'))
begin
   drop trigger dbo.Update_RBOSPECIALGROUP
end

GO

create trigger Update_RBOSPECIALGROUP
on RBOSPECIALGROUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSPECIALGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GROUPID,
				NAME,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSPECIALGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GROUPID,
				NAME,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSMMLINEGROUPS]'))
begin
   drop trigger dbo.Update_POSMMLINEGROUPS
end

GO

create trigger Update_POSMMLINEGROUPS
on POSMMLINEGROUPS after insert,update, delete as

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
    select @InsertedCount   = COUNT(*) FROM inserted

    begin try
        if @DeletedCount > 0 and @InsertedCount = 0
        begin
            insert into LSPOSNET_Audit.dbo.POSMMLINEGROUPSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                OFFERID,
                LINEGROUP,
                NOOFITEMSNEEDED,
                DATAAREAID,
                COLOR,
                DESCRIPTION,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.OFFERID,
                ins.LINEGROUP,
                ins.NOOFITEMSNEEDED,
                ins.DATAAREAID,
                ins.COLOR,
                ins.DESCRIPTION,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSMMLINEGROUPSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                OFFERID,
                LINEGROUP,
                NOOFITEMSNEEDED,
                DATAAREAID,
                COLOR,
                DESCRIPTION,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.OFFERID,
                ins.LINEGROUP,
                ins.NOOFITEMSNEEDED,
                ins.DATAAREAID,
                ins.COLOR,
                ins.DESCRIPTION,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINVENTITEMIMAGE]'))
begin
   drop trigger dbo.Update_RBOINVENTITEMIMAGE
end

GO

create trigger Update_RBOINVENTITEMIMAGE
on RBOINVENTITEMIMAGE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOINVENTITEMIMAGELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ITEMID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ITEMID,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOINVENTITEMIMAGELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ITEMID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ITEMID,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CURRENCY]'))
begin
   drop trigger dbo.Update_CURRENCY
end

GO

create trigger Update_CURRENCY
on CURRENCY after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.CURRENCYLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				CURRENCYCODE,
				TXT,
				LEDGERACCOUNTPROFIT,
				LEDGERACCOUNTLOSS,
				ROUNDOFFSALES,
				ROUNDOFFPURCH,
				ROUNDOFFAMOUNT,
				CONSEXCHRATEMONETARY,
				CONSEXCHRATENONMONETARY,
				LEDGERACCOUNTNONREALLOSS,
				LEDGERACCOUNTNONREALPROFIT,
				ROUNDOFFTYPEPURCH,
				ROUNDOFFTYPESALES,
				ROUNDOFFTYPEAMOUNT,
				ROUNDOFFPROJECT,
				ROUNDOFFTYPEPROJECT,
				ROUNDOFFTYPEPRICE,
				ROUNDOFFPRICE,
				SYMBOL,
				CURRENCYPREFIX,
				CURRENCYSUFFIX,
				ONLINECONVERSIONTOOL,
				CURRENCYCODEISO,
				GENDERMALEFEMALE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.CURRENCYCODE,
				ins.TXT,
				ins.LEDGERACCOUNTPROFIT,
				ins.LEDGERACCOUNTLOSS,
				ins.ROUNDOFFSALES,
				ins.ROUNDOFFPURCH,
				ins.ROUNDOFFAMOUNT,
				ins.CONSEXCHRATEMONETARY,
				ins.CONSEXCHRATENONMONETARY,
				ins.LEDGERACCOUNTNONREALLOSS,
				ins.LEDGERACCOUNTNONREALPROFIT,
				ins.ROUNDOFFTYPEPURCH,
				ins.ROUNDOFFTYPESALES,
				ins.ROUNDOFFTYPEAMOUNT,
				ins.ROUNDOFFPROJECT,
				ins.ROUNDOFFTYPEPROJECT,
				ins.ROUNDOFFTYPEPRICE,
				ins.ROUNDOFFPRICE,
				ins.SYMBOL,
				ins.CURRENCYPREFIX,
				ins.CURRENCYSUFFIX,
				ins.ONLINECONVERSIONTOOL,
				ins.CURRENCYCODEISO,
				ins.GENDERMALEFEMALE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.CURRENCYLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				CURRENCYCODE,
				TXT,
				LEDGERACCOUNTPROFIT,
				LEDGERACCOUNTLOSS,
				ROUNDOFFSALES,
				ROUNDOFFPURCH,
				ROUNDOFFAMOUNT,
				CONSEXCHRATEMONETARY,
				CONSEXCHRATENONMONETARY,
				LEDGERACCOUNTNONREALLOSS,
				LEDGERACCOUNTNONREALPROFIT,
				ROUNDOFFTYPEPURCH,
				ROUNDOFFTYPESALES,
				ROUNDOFFTYPEAMOUNT,
				ROUNDOFFPROJECT,
				ROUNDOFFTYPEPROJECT,
				ROUNDOFFTYPEPRICE,
				ROUNDOFFPRICE,
				SYMBOL,
				CURRENCYPREFIX,
				CURRENCYSUFFIX,
				ONLINECONVERSIONTOOL,
				CURRENCYCODEISO,
				GENDERMALEFEMALE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.CURRENCYCODE,
				ins.TXT,
				ins.LEDGERACCOUNTPROFIT,
				ins.LEDGERACCOUNTLOSS,
				ins.ROUNDOFFSALES,
				ins.ROUNDOFFPURCH,
				ins.ROUNDOFFAMOUNT,
				ins.CONSEXCHRATEMONETARY,
				ins.CONSEXCHRATENONMONETARY,
				ins.LEDGERACCOUNTNONREALLOSS,
				ins.LEDGERACCOUNTNONREALPROFIT,
				ins.ROUNDOFFTYPEPURCH,
				ins.ROUNDOFFTYPESALES,
				ins.ROUNDOFFTYPEAMOUNT,
				ins.ROUNDOFFPROJECT,
				ins.ROUNDOFFTYPEPROJECT,
				ins.ROUNDOFFTYPEPRICE,
				ins.ROUNDOFFPRICE,
				ins.SYMBOL,
				ins.CURRENCYPREFIX,
				ins.CURRENCYSUFFIX,
				ins.ONLINECONVERSIONTOOL,
				ins.CURRENCYCODEISO,
				ins.GENDERMALEFEMALE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_UNIT]'))
begin
   drop trigger dbo.Update_UNIT
end

GO

create trigger Update_UNIT
on UNIT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.UNITLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				UNITID,
				TXT,
				UNITDECIMALS,
				UNITSYSTEM,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UNITID,
				ins.TXT,
				ins.UNITDECIMALS,
				ins.UNITSYSTEM,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.UNITLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				UNITID,
				TXT,
				UNITDECIMALS,
				UNITSYSTEM,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.UNITID,
				ins.TXT,
				ins.UNITDECIMALS,
				ins.UNITSYSTEM,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PRICEGROUP]'))
begin
   drop trigger dbo.Update_PRICEGROUP
end

GO

create trigger Update_PRICEGROUP
on PRICEGROUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PRICEGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GROUPID,
				NAME,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PRICEGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GROUPID,
				NAME,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOLOCATIONPRICEGROUP]'))
begin
   drop trigger dbo.Update_RBOLOCATIONPRICEGROUP
end

GO

create trigger Update_RBOLOCATIONPRICEGROUP
on RBOLOCATIONPRICEGROUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOLOCATIONPRICEGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				PRICEGROUPID,
				LEVEL_,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.PRICEGROUPID,
				ins.LEVEL_,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOLOCATIONPRICEGROUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				PRICEGROUPID,
				LEVEL_,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.PRICEGROUPID,
				ins.LEVEL_,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
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

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_UNITCONVERT]'))
begin
   drop trigger dbo.Update_UNITCONVERT
end

GO

create trigger Update_UNITCONVERT
on UNITCONVERT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.UNITCONVERTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				FROMUNIT,
				TOUNIT,
				FACTOR,
				ROUNDOFF,
				ITEMID,
				DATAAREAID,
				MARKUP,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.FROMUNIT,
				ins.TOUNIT,
				ins.FACTOR,
				ins.ROUNDOFF,
				ins.ITEMID,
				ins.DATAAREAID,
				ins.MARKUP,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.UNITCONVERTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				FROMUNIT,
				TOUNIT,
				FACTOR,
				ROUNDOFF,
				ITEMID,
				DATAAREAID,
				MARKUP,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.FROMUNIT,
				ins.TOUNIT,
				ins.FACTOR,
				ins.ROUNDOFF,
				ins.ITEMID,
				ins.DATAAREAID,
				ins.MARKUP,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_EXCHRATES]'))
begin
   drop trigger dbo.Update_EXCHRATES
end

GO

create trigger Update_EXCHRATES
on EXCHRATES after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.EXCHRATESLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				FROMDATE,
				EXCHRATE,
				TODATE,
				CURRENCYCODE,
				POSEXCHRATE,
				DATAAREAID,
				RECID,
				GOVERNMENTEXCHRATE,
				TRIANGULATION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.FROMDATE,
				ins.EXCHRATE,
				ins.TODATE,
				ins.CURRENCYCODE,
				ins.POSEXCHRATE,
				ins.DATAAREAID,
				ins.RECID,
				ins.GOVERNMENTEXCHRATE,
				ins.TRIANGULATION,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.EXCHRATESLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				FROMDATE,
				EXCHRATE,
				TODATE,
				CURRENCYCODE,
				POSEXCHRATE,
				DATAAREAID,
				RECID,
				GOVERNMENTEXCHRATE,
				TRIANGULATION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.FROMDATE,
				ins.EXCHRATE,
				ins.TODATE,
				ins.CURRENCYCODE,
				ins.POSEXCHRATE,
				ins.DATAAREAID,
				ins.RECID,
				ins.GOVERNMENTEXCHRATE,
				ins.TRIANGULATION,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTORECASHDECLARATIONTABLE]'))
begin
   drop trigger dbo.Update_RBOSTORECASHDECLARATIONTABLE
end

GO

create trigger Update_RBOSTORECASHDECLARATIONTABLE
on RBOSTORECASHDECLARATIONTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTORECASHDECLARATIONTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				TYPE,
				CURRENCY,
				AMOUNT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.TYPE,
				ins.CURRENCY,
				ins.AMOUNT,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTORECASHDECLARATIONTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STOREID,
				TYPE,
				CURRENCY,
				AMOUNT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STOREID,
				ins.TYPE,
				ins.CURRENCY,
				ins.AMOUNT,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO
---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_SALESTYPE]'))
begin
   drop trigger dbo.Update_SALESTYPE
end

GO

create trigger Update_SALESTYPE
on SALESTYPE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.SALESTYPELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				CODE,
				DESCRIPTION,
				REQUESTSALESPERSON,
				REQUESTDEPOSITPERC,
				REQUESTCHARGEACCOUNT,
				PURCHASINGCODE,
				DEFAULTORDERLIMIT,
				LIMITSETTING,
				REQUESTCONFIRMATION,
				REQUESTDESCRIPTION,
				NEWGLOBALDIMENSION2,
				SUSPENDPRINTING,
				SUSPENDTYPE,
				PREPAYMENTACCOUNTNO,
				MINIMUMDEPOSIT,
				PRINTITEMLINESONPOSSLIP,
				VOIDEDPREPAYMENTACCOUNTNO,
				DAYSOPENTRANSEXIST,
				TAXGROUPID,
				PRICEGROUP,
				TRANSDELETEREMINDER,
				LOCATIONCODE,
				PAYMENTISPREPAYMENT,
				CALCPRICEFROMVATPRICE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.CODE,
				ins.DESCRIPTION,
				ins.REQUESTSALESPERSON,
				ins.REQUESTDEPOSITPERC,
				ins.REQUESTCHARGEACCOUNT,
				ins.PURCHASINGCODE,
				ins.DEFAULTORDERLIMIT,
				ins.LIMITSETTING,
				ins.REQUESTCONFIRMATION,
				ins.REQUESTDESCRIPTION,
				ins.NEWGLOBALDIMENSION2,
				ins.SUSPENDPRINTING,
				ins.SUSPENDTYPE,
				ins.PREPAYMENTACCOUNTNO,
				ins.MINIMUMDEPOSIT,
				ins.PRINTITEMLINESONPOSSLIP,
				ins.VOIDEDPREPAYMENTACCOUNTNO,
				ins.DAYSOPENTRANSEXIST,
				ins.TAXGROUPID,
				ins.PRICEGROUP,
				ins.TRANSDELETEREMINDER,
				ins.LOCATIONCODE,
				ins.PAYMENTISPREPAYMENT,
				ins.CALCPRICEFROMVATPRICE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.SALESTYPELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				CODE,
				DESCRIPTION,
				REQUESTSALESPERSON,
				REQUESTDEPOSITPERC,
				REQUESTCHARGEACCOUNT,
				PURCHASINGCODE,
				DEFAULTORDERLIMIT,
				LIMITSETTING,
				REQUESTCONFIRMATION,
				REQUESTDESCRIPTION,
				NEWGLOBALDIMENSION2,
				SUSPENDPRINTING,
				SUSPENDTYPE,
				PREPAYMENTACCOUNTNO,
				MINIMUMDEPOSIT,
				PRINTITEMLINESONPOSSLIP,
				VOIDEDPREPAYMENTACCOUNTNO,
				DAYSOPENTRANSEXIST,
				TAXGROUPID,
				PRICEGROUP,
				TRANSDELETEREMINDER,
				LOCATIONCODE,
				PAYMENTISPREPAYMENT,
				CALCPRICEFROMVATPRICE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.CODE,
				ins.DESCRIPTION,
				ins.REQUESTSALESPERSON,
				ins.REQUESTDEPOSITPERC,
				ins.REQUESTCHARGEACCOUNT,
				ins.PURCHASINGCODE,
				ins.DEFAULTORDERLIMIT,
				ins.LIMITSETTING,
				ins.REQUESTCONFIRMATION,
				ins.REQUESTDESCRIPTION,
				ins.NEWGLOBALDIMENSION2,
				ins.SUSPENDPRINTING,
				ins.SUSPENDTYPE,
				ins.PREPAYMENTACCOUNTNO,
				ins.MINIMUMDEPOSIT,
				ins.PRINTITEMLINESONPOSSLIP,
				ins.VOIDEDPREPAYMENTACCOUNTNO,
				ins.DAYSOPENTRANSEXIST,
				ins.TAXGROUPID,
				ins.PRICEGROUP,
				ins.TRANSDELETEREMINDER,
				ins.LOCATIONCODE,
				ins.PAYMENTISPREPAYMENT,
				ins.CALCPRICEFROMVATPRICE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTATEMENTTABLE]'))
begin
   drop trigger dbo.Update_RBOSTATEMENTTABLE
end

GO

create trigger Update_RBOSTATEMENTTABLE
on RBOSTATEMENTTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				STOREID,
				CALCULATEDTIME,
				POSTINGDATE,
				PERIODSTARTINGTIME,
				PERIODENDINGTIME,
				POSTED,
				CALCULATED,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.STOREID,
				ins.CALCULATEDTIME,
				ins.POSTINGDATE,
				ins.PERIODSTARTINGTIME,
				ins.PERIODENDINGTIME,
				ins.POSTED,
				ins.CALCULATED,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				STOREID,
				CALCULATEDTIME,
				POSTINGDATE,
				PERIODSTARTINGTIME,
				PERIODENDINGTIME,
				POSTED,
				CALCULATED,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.STOREID,
				ins.CALCULATEDTIME,
				ins.POSTINGDATE,
				ins.PERIODSTARTINGTIME,
				ins.PERIODENDINGTIME,
				ins.POSTED,
				ins.CALCULATED,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTATEMENTLINE]'))
begin
   drop trigger dbo.Update_RBOSTATEMENTLINE
end

GO

create trigger Update_RBOSTATEMENTLINE
on RBOSTATEMENTLINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				LINENUMBER,
				STAFFID,
				TERMINALID,
				CURRENCYCODE,
				TENDERID,
				TRANSACTIONAMOUNT,
				BANKEDAMOUNT,
				SAFEAMOUNT,
				COUNTEDAMOUNT,
				DIFFERENCE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.LINENUMBER,
				ins.STAFFID,
				ins.TERMINALID,
				ins.CURRENCYCODE,
				ins.TENDERID,
				ins.TRANSACTIONAMOUNT,
				ins.BANKEDAMOUNT,
				ins.SAFEAMOUNT,
				ins.COUNTEDAMOUNT,
				ins.DIFFERENCE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				LINENUMBER,
				STAFFID,
				TERMINALID,
				CURRENCYCODE,
				TENDERID,
				TRANSACTIONAMOUNT,
				BANKEDAMOUNT,
				SAFEAMOUNT,
				COUNTEDAMOUNT,
				DIFFERENCE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.LINENUMBER,
				ins.STAFFID,
				ins.TERMINALID,
				ins.CURRENCYCODE,
				ins.TENDERID,
				ins.TRANSACTIONAMOUNT,
				ins.BANKEDAMOUNT,
				ins.SAFEAMOUNT,
				ins.COUNTEDAMOUNT,
				ins.DIFFERENCE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CONTACTTABLE]'))
begin
   drop trigger dbo.Update_CONTACTTABLE
end

GO

create trigger Update_CONTACTTABLE
on CONTACTTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.CONTACTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				CONTACTID,
				OWNERID,
				OWNERTYPE,
				CONTACTTYPE,
				COMPANYNAME,
				FirstName,
				MiddleName,
				LastName,
				NamePrefix,
				NameSuffix,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				PHONE,
				PHONE2,
				FAX,
				EMAIL,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.CONTACTID,
				ins.OWNERID,
				ins.OWNERTYPE,
				ins.CONTACTTYPE,
				ins.COMPANYNAME,
				ins.FirstName,
				ins.MiddleName,
				ins.LastName,
				ins.NamePrefix,
				ins.NameSuffix,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.PHONE,
				ins.PHONE2,
				ins.FAX,
				ins.EMAIL,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.CONTACTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				CONTACTID,
				OWNERID,
				OWNERTYPE,
				CONTACTTYPE,
				COMPANYNAME,
				FirstName,
				MiddleName,
				LastName,
				NamePrefix,
				NameSuffix,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				PHONE,
				PHONE2,
				FAX,
				EMAIL,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.CONTACTID,
				ins.OWNERID,
				ins.OWNERTYPE,
				ins.CONTACTTYPE,
				ins.COMPANYNAME,
				ins.FirstName,
				ins.MiddleName,
				ins.LastName,
				ins.NamePrefix,
				ins.NameSuffix,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.PHONE,
				ins.PHONE2,
				ins.FAX,
				ins.EMAIL,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_VENDTABLE]'))
begin
   drop trigger dbo.Update_VENDTABLE
end

GO

create trigger Update_VENDTABLE
on VENDTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.VENDTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ACCOUNTNUM,
				NAME,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				PHONE,
				FAX,
				EMAIL,
				DEFAULTCONTACTID,
				LANGUAGEID,
				CURRENCY,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ACCOUNTNUM,
				ins.NAME,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.PHONE,
				ins.FAX,
				ins.EMAIL,
				ins.DEFAULTCONTACTID,
				ins.LANGUAGEID,
				ins.CURRENCY,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.VENDTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ACCOUNTNUM,
				NAME,
				ADDRESS,
				STREET,
				ZIPCODE,
				CITY,
				COUNTY,
				STATE,
				COUNTRY,
				PHONE,
				FAX,
				EMAIL,
				DEFAULTCONTACTID,
				LANGUAGEID,
				CURRENCY,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ACCOUNTNUM,
				ins.NAME,
				ins.ADDRESS,
				ins.STREET,
				ins.ZIPCODE,
				ins.CITY,
				ins.COUNTY,
				ins.STATE,
				ins.COUNTRY,
				ins.PHONE,
				ins.FAX,
				ins.EMAIL,
				ins.DEFAULTCONTACTID,
				ins.LANGUAGEID,
				ins.CURRENCY,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_VENDORITEMS]'))
begin
   drop trigger dbo.Update_VENDORITEMS
end

GO

create trigger Update_VENDORITEMS
on VENDORITEMS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.VENDORITEMSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				INTERNALID,
				VENDORITEMID,
				RETAILITEMID,
				UNITID,
				VENDORID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.INTERNALID,
				ins.VENDORITEMID,
				ins.RETAILITEMID,
				ins.UNITID,
				ins.VENDORID,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.VENDORITEMSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				INTERNALID,
				VENDORITEMID,
				RETAILITEMID,
				UNITID,
				VENDORID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.INTERNALID,
				ins.VENDORITEMID,
				ins.RETAILITEMID,
				ins.UNITID,
				ins.VENDORID,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTJOURNALTABLE]'))
begin
   drop trigger dbo.Update_INVENTJOURNALTABLE
end

GO

create trigger Update_INVENTJOURNALTABLE
on INVENTJOURNALTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTJOURNALTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				JOURNALID,
				DESCRIPTION,
				POSTED,
				POSTEDDATETIME,
				JOURNALTYPE,
				DELETEPOSTEDLINES,
				CREATEDDATETIME,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.JOURNALID,
				ins.DESCRIPTION,
				ins.POSTED,
				ins.POSTEDDATETIME,
				ins.JOURNALTYPE,
				ins.DELETEPOSTEDLINES,
				ins.CREATEDDATETIME,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTJOURNALTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				JOURNALID,
				DESCRIPTION,
				POSTED,
				POSTEDDATETIME,
				JOURNALTYPE,
				DELETEPOSTEDLINES,
				CREATEDDATETIME,				
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.JOURNALID,
				ins.DESCRIPTION,
				ins.POSTED,
				ins.POSTEDDATETIME,
				ins.JOURNALTYPE,
				ins.DELETEPOSTEDLINES,
				ins.CREATEDDATETIME,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTJOURNALTRANS]'))
begin
   drop trigger dbo.Update_INVENTJOURNALTRANS
end

GO

create trigger Update_INVENTJOURNALTRANS
on INVENTJOURNALTRANS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.INVENTJOURNALTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				JOURNALID,
				LINENUM,
				TRANSDATE,
				ITEMID,
				ADJUSTMENT,
				COSTPRICE,
				PRICEUNIT,
				COSTMARKUP,
				COSTAMOUNT,
				SALESAMOUNT,
				INVENTONHAND,
				COUNTED,
				REASONREFRECID,
				DATAAREAID,
				PICTUREID,
				OMNITRANSACTIONID,
				OMNILINEID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.JOURNALID,
				ins.LINENUM,
				ins.TRANSDATE,
				ins.ITEMID,
				ins.ADJUSTMENT,
				ins.COSTPRICE,
				ins.PRICEUNIT,
				ins.COSTMARKUP,
				ins.COSTAMOUNT,
				ins.SALESAMOUNT,
				ins.INVENTONHAND,
				ins.COUNTED,
				ins.REASONREFRECID,
				ins.DATAAREAID,
				ins.PICTUREID,
				ins.OMNITRANSACTIONID,
				ins.OMNILINEID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.INVENTJOURNALTRANSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				JOURNALID,
				LINENUM,
				TRANSDATE,
				ITEMID,
				ADJUSTMENT,
				COSTPRICE,
				PRICEUNIT,
				COSTMARKUP,
				COSTAMOUNT,
				SALESAMOUNT,
				INVENTONHAND,
				COUNTED,
				REASONREFRECID,
				DATAAREAID,
				PICTUREID,
				OMNITRANSACTIONID,
				OMNILINEID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.JOURNALID,
				ins.LINENUM,
				ins.TRANSDATE,
				ins.ITEMID,
				ins.ADJUSTMENT,
				ins.COSTPRICE,
				ins.PRICEUNIT,
				ins.COSTMARKUP,
				ins.COSTAMOUNT,
				ins.SALESAMOUNT,
				ins.INVENTONHAND,
				ins.COUNTED,
				ins.REASONREFRECID,
				ins.DATAAREAID,
				ins.PICTUREID,
				ins.OMNITRANSACTIONID,
				ins.OMNILINEID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTTRANSREASON]'))
begin
   drop trigger dbo.Update_INVENTTRANSREASON
end

GO

create trigger Update_INVENTTRANSREASON
on INVENTTRANSREASON after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.INVENTTRANSREASONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                REASONID,
                REASONTEXT,
                DATAAREAID,
                ACTION,
                BEGINDATE,
                ENDDATE,
                ISSYSTEMREASONCODE,
                SHOWONPOS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.REASONID,
                ins.REASONTEXT,
                ins.DATAAREAID,
                ins.ACTION,
                ins.BEGINDATE,
                ins.ENDDATE,
                ins.ISSYSTEMREASONCODE,
                ins.SHOWONPOS,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTTRANSREASONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                REASONID,
                REASONTEXT,
                DATAAREAID,
                ACTION,
                BEGINDATE,
                ENDDATE,
                ISSYSTEMREASONCODE,
                SHOWONPOS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.REASONID,
                ins.REASONTEXT,
                ins.DATAAREAID,
                ins.ACTION,
                ins.BEGINDATE,
                ins.ENDDATE,
                ins.ISSYSTEMREASONCODE,
                ins.SHOWONPOS,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINFOCODETABLE]'))
begin
   drop trigger dbo.Update_RBOINFOCODETABLE
end

GO

create trigger Update_RBOINFOCODETABLE
on RBOINFOCODETABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOINFOCODETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				INFOCODEID,
				DESCRIPTION,
				PROMPT,
				ONCEPERTRANSACTION,
				VALUEISAMOUNTORQUANTITY,
				PRINTPROMPTONRECEIPT,
				PRINTINPUTONRECEIPT,
				PRINTINPUTNAMEONRECEIPT,
				INPUTTYPE,
				MINIMUMVALUE,
				MAXIMUMVALUE,
				MINIMUMLENGTH,
				MAXIMUMLENGTH,
				INPUTREQUIRED,
				STD1INVALUE,
				LINKEDINFOCODEID,
				RANDOMFACTOR,
				RANDOMCOUNTER,
				DATATYPEID,
				MODIFIEDDATE,
				MODIFIEDTIME,
				MODIFIEDBY,
				DATAAREAID,
				ADDITIONALCHECK,
				USAGECATEGORY,
				DISPLAYOPTION,
				LINKITEMLINESTOTRIGGERLINE,
				MULTIPLESELECTION,
				TRIGGERING,
				MINSELECTION,
				MAXSELECTION,
				CREATEINFOCODETRANSENTRIES,
				EXPLANATORYHEADERTEXT,
				OKPRESSEDACTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.INFOCODEID,
				ins.DESCRIPTION,
				ins.PROMPT,
				ins.ONCEPERTRANSACTION,
				ins.VALUEISAMOUNTORQUANTITY,
				ins.PRINTPROMPTONRECEIPT,
				ins.PRINTINPUTONRECEIPT,
				ins.PRINTINPUTNAMEONRECEIPT,
				ins.INPUTTYPE,
				ins.MINIMUMVALUE,
				ins.MAXIMUMVALUE,
				ins.MINIMUMLENGTH,
				ins.MAXIMUMLENGTH,
				ins.INPUTREQUIRED,
				ins.STD1INVALUE,
				ins.LINKEDINFOCODEID,
				ins.RANDOMFACTOR,
				ins.RANDOMCOUNTER,
				ins.DATATYPEID,
				ins.MODIFIEDDATE,
				ins.MODIFIEDTIME,
				ins.MODIFIEDBY,
				ins.DATAAREAID,
				ins.ADDITIONALCHECK,
				ins.USAGECATEGORY,
				ins.DISPLAYOPTION,
				ins.LINKITEMLINESTOTRIGGERLINE,
				ins.MULTIPLESELECTION,
				ins.TRIGGERING,
				ins.MINSELECTION,
				ins.MAXSELECTION,
				ins.CREATEINFOCODETRANSENTRIES,
				ins.EXPLANATORYHEADERTEXT,
				ins.OKPRESSEDACTION,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOINFOCODETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				INFOCODEID,
				DESCRIPTION,
				PROMPT,
				ONCEPERTRANSACTION,
				VALUEISAMOUNTORQUANTITY,
				PRINTPROMPTONRECEIPT,
				PRINTINPUTONRECEIPT,
				PRINTINPUTNAMEONRECEIPT,
				INPUTTYPE,
				MINIMUMVALUE,
				MAXIMUMVALUE,
				MINIMUMLENGTH,
				MAXIMUMLENGTH,
				INPUTREQUIRED,
				STD1INVALUE,
				LINKEDINFOCODEID,
				RANDOMFACTOR,
				RANDOMCOUNTER,
				DATATYPEID,
				MODIFIEDDATE,
				MODIFIEDTIME,
				MODIFIEDBY,
				DATAAREAID,
				ADDITIONALCHECK,
				USAGECATEGORY,
				DISPLAYOPTION,
				LINKITEMLINESTOTRIGGERLINE,
				MULTIPLESELECTION,
				TRIGGERING,
				MINSELECTION,
				MAXSELECTION,
				CREATEINFOCODETRANSENTRIES,
				EXPLANATORYHEADERTEXT,
				OKPRESSEDACTION,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.INFOCODEID,
				ins.DESCRIPTION,
				ins.PROMPT,
				ins.ONCEPERTRANSACTION,
				ins.VALUEISAMOUNTORQUANTITY,
				ins.PRINTPROMPTONRECEIPT,
				ins.PRINTINPUTONRECEIPT,
				ins.PRINTINPUTNAMEONRECEIPT,
				ins.INPUTTYPE,
				ins.MINIMUMVALUE,
				ins.MAXIMUMVALUE,
				ins.MINIMUMLENGTH,
				ins.MAXIMUMLENGTH,
				ins.INPUTREQUIRED,
				ins.STD1INVALUE,
				ins.LINKEDINFOCODEID,
				ins.RANDOMFACTOR,
				ins.RANDOMCOUNTER,
				ins.DATATYPEID,
				ins.MODIFIEDDATE,
				ins.MODIFIEDTIME,
				ins.MODIFIEDBY,
				ins.DATAAREAID,
				ins.ADDITIONALCHECK,
				ins.USAGECATEGORY,
				ins.DISPLAYOPTION,
				ins.LINKITEMLINESTOTRIGGERLINE,
				ins.MULTIPLESELECTION,
				ins.TRIGGERING,
				ins.MINSELECTION,
				ins.MAXSELECTION,
				ins.CREATEINFOCODETRANSENTRIES,
				ins.EXPLANATORYHEADERTEXT,
				ins.OKPRESSEDACTION,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINFORMATIONSUBCODETABLE]'))
begin
   drop trigger dbo.Update_RBOINFORMATIONSUBCODETABLE
end

GO

create trigger Update_RBOINFORMATIONSUBCODETABLE
on RBOINFORMATIONSUBCODETABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOINFORMATIONSUBCODETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				INFOCODEID,
				SUBCODEID,
				DESCRIPTION,
				TRIGGERFUNCTION,
				TRIGGERCODE,
				NEWSALESLINE,
				PRICETYPE,
				AMOUNTPERCENT,
				DATAAREAID,
				USAGECATEGORY,
				VARIANTCODE,
				VARIANTNEEDED,
				QTYLINKEDTOTRIGGERLINE,
				PRICEHANDLING,
				LINKEDINFOCODE,
				UNITOFMEASURE,
				QTYPERUNITOFMEASURE,
				INFOCODEPROMPT,
				MAXSELECTION,
				SERIALLOTNEEDED,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.INFOCODEID,
				ins.SUBCODEID,
				ins.DESCRIPTION,
				ins.TRIGGERFUNCTION,
				ins.TRIGGERCODE,
				ins.NEWSALESLINE,
				ins.PRICETYPE,
				ins.AMOUNTPERCENT,
				ins.DATAAREAID,
				ins.USAGECATEGORY,
				ins.VARIANTCODE,
				ins.VARIANTNEEDED,
				ins.QTYLINKEDTOTRIGGERLINE,
				ins.PRICEHANDLING,
				ins.LINKEDINFOCODE,
				ins.UNITOFMEASURE,
				ins.QTYPERUNITOFMEASURE,
				ins.INFOCODEPROMPT,
				ins.MAXSELECTION,
				ins.SERIALLOTNEEDED,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOINFORMATIONSUBCODETABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				INFOCODEID,
				SUBCODEID,
				DESCRIPTION,
				TRIGGERFUNCTION,
				TRIGGERCODE,
				NEWSALESLINE,
				PRICETYPE,
				AMOUNTPERCENT,
				DATAAREAID,
				USAGECATEGORY,
				VARIANTCODE,
				VARIANTNEEDED,
				QTYLINKEDTOTRIGGERLINE,
				PRICEHANDLING,
				LINKEDINFOCODE,
				UNITOFMEASURE,
				QTYPERUNITOFMEASURE,
				INFOCODEPROMPT,
				MAXSELECTION,
				SERIALLOTNEEDED,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.INFOCODEID,
				ins.SUBCODEID,
				ins.DESCRIPTION,
				ins.TRIGGERFUNCTION,
				ins.TRIGGERCODE,
				ins.NEWSALESLINE,
				ins.PRICETYPE,
				ins.AMOUNTPERCENT,
				ins.DATAAREAID,
				ins.USAGECATEGORY,
				ins.VARIANTCODE,
				ins.VARIANTNEEDED,
				ins.QTYLINKEDTOTRIGGERLINE,
				ins.PRICEHANDLING,
				ins.LINKEDINFOCODE,
				ins.UNITOFMEASURE,
				ins.QTYPERUNITOFMEASURE,
				ins.INFOCODEPROMPT,
				ins.MAXSELECTION,
				ins.SERIALLOTNEEDED,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINFOCODETABLESPECIFIC]'))
begin
   drop trigger dbo.Update_RBOINFOCODETABLESPECIFIC
end

GO

create trigger Update_RBOINFOCODETABLESPECIFIC
on RBOINFOCODETABLESPECIFIC after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOINFOCODETABLESPECIFICLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				REFRELATION,
				SEQUENCE,
				INFOCODEID,
				INPUTREQUIRED,
				WHENREQUIRED,
				REFRELATION2,
				REFRELATION3,
				REFTABLEID,
				DATAAREAID,
				TRIGGERING,
				UNITOFMEASURE,
				SALESTYPEFILTER,
				USAGECATEGORY,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.REFRELATION,
				ins.SEQUENCE,
				ins.INFOCODEID,
				ins.INPUTREQUIRED,
				ins.WHENREQUIRED,
				ins.REFRELATION2,
				ins.REFRELATION3,
				ins.REFTABLEID,
				ins.DATAAREAID,
				ins.TRIGGERING,
				ins.UNITOFMEASURE,
				ins.SALESTYPEFILTER,
				ins.USAGECATEGORY,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOINFOCODETABLESPECIFICLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				REFRELATION,
				SEQUENCE,
				INFOCODEID,
				INPUTREQUIRED,
				WHENREQUIRED,
				REFRELATION2,
				REFRELATION3,
				REFTABLEID,
				DATAAREAID,
				TRIGGERING,
				UNITOFMEASURE,
				SALESTYPEFILTER,
				USAGECATEGORY,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.REFRELATION,
				ins.SEQUENCE,
				ins.INFOCODEID,
				ins.INPUTREQUIRED,
				ins.WHENREQUIRED,
				ins.REFRELATION2,
				ins.REFRELATION3,
				ins.REFTABLEID,
				ins.DATAAREAID,
				ins.TRIGGERING,
				ins.UNITOFMEASURE,
				ins.SALESTYPEFILTER,
				ins.USAGECATEGORY,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_GOODSRECEIVING]'))
begin
   drop trigger dbo.Update_GOODSRECEIVING
end

GO

create trigger Update_GOODSRECEIVING
on GOODSRECEIVING after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.GOODSRECEIVINGLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GOODSRECEIVINGID,
				PURCHASEORDERID,
				STATUS,
				CREATEDDATE,
				POSTEDDATE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GOODSRECEIVINGID,
				ins.PURCHASEORDERID,
				ins.STATUS,
				ins.CREATEDDATE,
				ins.POSTEDDATE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.GOODSRECEIVINGLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GOODSRECEIVINGID,
				PURCHASEORDERID,
				STATUS,
				CREATEDDATE,
				POSTEDDATE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GOODSRECEIVINGID,
				ins.PURCHASEORDERID,
				ins.STATUS,
				ins.CREATEDDATE,
				ins.POSTEDDATE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_GOODSRECEIVINGLINE]'))
begin
   drop trigger dbo.Update_GOODSRECEIVINGLINE
end

GO

create trigger Update_GOODSRECEIVINGLINE
on GOODSRECEIVINGLINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.GOODSRECEIVINGLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GOODSRECEIVINGID,
				PURCHASEORDERLINENUMBER,
				LINENUMBER,
				RECEIVEDQUANTITY,
				RECEIVEDDATE,
				POSTED,
				STOREID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GOODSRECEIVINGID,
				ins.PURCHASEORDERLINENUMBER,
				ins.LINENUMBER,
				ins.RECEIVEDQUANTITY,
				ins.RECEIVEDDATE,
				ins.POSTED,
				ins.STOREID,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.GOODSRECEIVINGLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				GOODSRECEIVINGID,
				PURCHASEORDERLINENUMBER,
				LINENUMBER,
				RECEIVEDQUANTITY,
				RECEIVEDDATE,
				POSTED,
				STOREID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.GOODSRECEIVINGID,
				ins.PURCHASEORDERLINENUMBER,
				ins.LINENUMBER,
				ins.RECEIVEDQUANTITY,
				ins.RECEIVEDDATE,
				ins.POSTED,
				ins.STOREID,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PURCHASEORDERS]'))
begin
   drop trigger dbo.Update_PURCHASEORDERS
end

GO

create trigger Update_PURCHASEORDERS
on PURCHASEORDERS after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				VENDORID,
				CONTACTID,
				PURCHASESTATUS,
				DELIVERYDATE,
				CURRENCYCODE,
				CREATEDDATE,
				ORDERER,
				STOREID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.VENDORID,
				ins.CONTACTID,
				ins.PURCHASESTATUS,
				ins.DELIVERYDATE,
				ins.CURRENCYCODE,
				ins.CREATEDDATE,
				ins.ORDERER,
				ins.STOREID,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERSLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				VENDORID,
				CONTACTID,
				PURCHASESTATUS,
				DELIVERYDATE,
				CURRENCYCODE,
				CREATEDDATE,
				ORDERER,
				STOREID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.VENDORID,
				ins.CONTACTID,
				ins.PURCHASESTATUS,
				ins.DELIVERYDATE,
				ins.CURRENCYCODE,
				ins.CREATEDDATE,
				ins.ORDERER,
				ins.STOREID,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PURCHASEORDERLINE]'))
begin
   drop trigger dbo.Update_PURCHASEORDERLINE
end

GO

create trigger Update_PURCHASEORDERLINE
on PURCHASEORDERLINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				LINENUMBER,
				RETAILITEMID,
				VENDORITEMID,
				UNITID,
				QUANTITY,
				PRICE,
				DATAAREAID,
				PICTUREID,
				OMNITRANSACTIONID,
				OMNILINEID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.LINENUMBER,
				ins.RETAILITEMID,
				ins.VENDORITEMID,
				ins.UNITID,
				ins.QUANTITY,
				ins.PRICE,
				ins.DATAAREAID,
				ins.PICTUREID,
				ins.OMNITRANSACTIONID,
				ins.OMNILINEID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				LINENUMBER,
				RETAILITEMID,
				VENDORITEMID,
				UNITID,
				QUANTITY,
				PRICE,
				DATAAREAID,
				PICTUREID,
				OMNITRANSACTIONID,
				OMNILINEID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.LINENUMBER,
				ins.RETAILITEMID,
				ins.VENDORITEMID,
				ins.UNITID,
				ins.QUANTITY,
				ins.PRICE,
				ins.DATAAREAID,
				ins.PICTUREID,
				ins.OMNITRANSACTIONID,
				ins.OMNILINEID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PURCHASEORDERMISCCHARGES]'))
begin
   drop trigger dbo.Update_PURCHASEORDERMISCCHARGES
end

GO

create trigger Update_PURCHASEORDERMISCCHARGES
on PURCHASEORDERMISCCHARGES after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERMISCCHARGESLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				LINENUMBER,
				TYPE,
				REASON,
				AMOUNT,
				TAXAMOUNT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.LINENUMBER,
				ins.TYPE,
				ins.REASON,
				ins.AMOUNT,
				ins.TAXAMOUNT,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.PURCHASEORDERMISCCHARGESLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				PURCHASEORDERID,
				LINENUMBER,
				TYPE,
				REASON,
				AMOUNT,
				TAXAMOUNT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.PURCHASEORDERID,
				ins.LINENUMBER,
				ins.TYPE,
				ins.REASON,
				ins.AMOUNT,
				ins.TAXAMOUNT,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO
---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTATEMENTTABLE]'))
begin
   drop trigger dbo.Update_RBOSTATEMENTTABLE
end

GO

create trigger Update_RBOSTATEMENTTABLE
on RBOSTATEMENTTABLE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				STOREID,
				CALCULATEDTIME,
				POSTINGDATE,
				PERIODSTARTINGTIME,
				PERIODENDINGTIME,
				POSTED,
				CALCULATED,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.STOREID,
				ins.CALCULATEDTIME,
				ins.POSTINGDATE,
				ins.PERIODSTARTINGTIME,
				ins.PERIODENDINGTIME,
				ins.POSTED,
				ins.CALCULATED,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTTABLELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				STOREID,
				CALCULATEDTIME,
				POSTINGDATE,
				PERIODSTARTINGTIME,
				PERIODENDINGTIME,
				POSTED,
				CALCULATED,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.STOREID,
				ins.CALCULATEDTIME,
				ins.POSTINGDATE,
				ins.PERIODSTARTINGTIME,
				ins.PERIODENDINGTIME,
				ins.POSTED,
				ins.CALCULATED,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOSTATEMENTLINE]'))
begin
   drop trigger dbo.Update_RBOSTATEMENTLINE
end

GO

create trigger Update_RBOSTATEMENTLINE
on RBOSTATEMENTLINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				LINENUMBER,
				STAFFID,
				TERMINALID,
				CURRENCYCODE,
				TENDERID,
				TRANSACTIONAMOUNT,
				BANKEDAMOUNT,
				SAFEAMOUNT,
				COUNTEDAMOUNT,
				DIFFERENCE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.LINENUMBER,
				ins.STAFFID,
				ins.TERMINALID,
				ins.CURRENCYCODE,
				ins.TENDERID,
				ins.TRANSACTIONAMOUNT,
				ins.BANKEDAMOUNT,
				ins.SAFEAMOUNT,
				ins.COUNTEDAMOUNT,
				ins.DIFFERENCE,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RBOSTATEMENTLINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				STATEMENTID,
				LINENUMBER,
				STAFFID,
				TERMINALID,
				CURRENCYCODE,
				TENDERID,
				TRANSACTIONAMOUNT,
				BANKEDAMOUNT,
				SAFEAMOUNT,
				COUNTEDAMOUNT,
				DIFFERENCE,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.STATEMENTID,
				ins.LINENUMBER,
				ins.STAFFID,
				ins.TERMINALID,
				ins.CURRENCYCODE,
				ins.TENDERID,
				ins.TRANSACTIONAMOUNT,
				ins.BANKEDAMOUNT,
				ins.SAFEAMOUNT,
				ins.COUNTEDAMOUNT,
				DIFFERENCE,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_DININGTABLELAYOUT]'))
begin
   drop trigger dbo.Update_DININGTABLELAYOUT
end

GO

create trigger Update_DININGTABLELAYOUT
on DININGTABLELAYOUT after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.DININGTABLELAYOUTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				RESTAURANTID,
				SEQUENCE,
				SALESTYPE,
				LAYOUTID,
				DESCRIPTION,
				NOOFSCREENS,
				STARTINGTABLENO,
				NOOFDININGTABLES,
				ENDINGTABLENO,
				DININGTABLEROWS,
				DININGTABLECOLUMNS,
				CURRENTLAYOUT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.RESTAURANTID,
				ins.SEQUENCE,
				ins.SALESTYPE,
				ins.LAYOUTID,
				ins.DESCRIPTION,
				ins.NOOFSCREENS,
				ins.STARTINGTABLENO,
				ins.NOOFDININGTABLES,
				ins.ENDINGTABLENO,
				ins.DININGTABLEROWS,
				ins.DININGTABLECOLUMNS,
				ins.CURRENTLAYOUT,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.DININGTABLELAYOUTLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				RESTAURANTID,
				SEQUENCE,
				SALESTYPE,
				LAYOUTID,
				DESCRIPTION,
				NOOFSCREENS,
				STARTINGTABLENO,
				NOOFDININGTABLES,
				ENDINGTABLENO,
				DININGTABLEROWS,
				DININGTABLECOLUMNS,
				CURRENTLAYOUT,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.RESTAURANTID,
				ins.SEQUENCE,
				ins.SALESTYPE,
				ins.LAYOUTID,
				ins.DESCRIPTION,
				ins.NOOFSCREENS,
				ins.STARTINGTABLENO,
				ins.NOOFDININGTABLES,
				ins.ENDINGTABLENO,
				ins.DININGTABLEROWS,
				ins.DININGTABLECOLUMNS,
				ins.CURRENTLAYOUT,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO
---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_HOSPITALITYTYPE]'))
begin
   drop trigger dbo.Update_HOSPITALITYTYPE
end

GO

create trigger Update_HOSPITALITYTYPE
on HOSPITALITYTYPE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.HOSPITALITYTYPELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				RESTAURANTID,
				SEQUENCE,
				DESCRIPTION,
				OVERVIEW,
				SALESTYPE,
				UPDATETABLEFROMPOS,
				REQUESTNOOFGUESTS,
				STATIONPRINTING,
				ACCESSTOOTHERRESTAURANT,
				POSLOGONMENUID,
				ALLOWNEWENTRIES,
				TIPSAMTLINE1,
				TIPSAMTLINE2,
				TIPSTOTALLINE,
				STAYINPOSAFTERTRANS,
				TIPSINCOMEACC1,
				TIPSINCOMEACC2,
				NOOFDINEINTABLES,
				TABLEBUTTONPOSMENUID,
				TABLEBUTTONDESCRIPTION,
				TABLEBUTTONSTAFFDESCRIPTION,
				STAFFTAKEOVERINTRANS,
				MANAGERTAKEOVERINTRANS,
				VIEWSALESSTAFF,
				VIEWTRANSDATE,
				VIEWTRANSTIME,
				VIEWDELIVERYADDRESS,
				VIEWLISTTOTALS,
				ORDERBY,
				VIEWRESTAURANT,
				VIEWGRID,
				VIEWCOUNTDOWN,
				VIEWPROGRESSSTATUS,
				DIRECTEDITOPERATION,
				SETTINGSFROMHOSPTYPE,
				SETTINGSFROMSEQUENCE,
				SHARINGSALESTYPEFILTER,
				SETTINGSFROMRESTAURANT,
				GUESTBUTTONS,
				MAXGUESTBUTTONSSHOWN,
				MAXGUESTSPERTABLE,
				SPLITBILLLOOKUPID,
				SELECTGUESTONSPLITTING,
				COMBINESPLITLINESACTION,
				TRANSFERLINESLOOKUPID,
				PRINTTRAININGTRANSACTIONS,
				LAYOUTID,
				TOPPOSMENUID,
				DININGTABLELAYOUTID,
				AUTOMATICJOININGCHECK,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.RESTAURANTID,
				ins.SEQUENCE,
				ins.DESCRIPTION,
				ins.OVERVIEW,
				ins.SALESTYPE,
				ins.UPDATETABLEFROMPOS,
				ins.REQUESTNOOFGUESTS,
				ins.STATIONPRINTING,
				ins.ACCESSTOOTHERRESTAURANT,
				ins.POSLOGONMENUID,
				ins.ALLOWNEWENTRIES,
				ins.TIPSAMTLINE1,
				ins.TIPSAMTLINE2,
				ins.TIPSTOTALLINE,
				ins.STAYINPOSAFTERTRANS,
				ins.TIPSINCOMEACC1,
				ins.TIPSINCOMEACC2,
				ins.NOOFDINEINTABLES,
				ins.TABLEBUTTONPOSMENUID,
				ins.TABLEBUTTONDESCRIPTION,
				ins.TABLEBUTTONSTAFFDESCRIPTION,
				ins.STAFFTAKEOVERINTRANS,
				ins.MANAGERTAKEOVERINTRANS,
				ins.VIEWSALESSTAFF,
				ins.VIEWTRANSDATE,
				ins.VIEWTRANSTIME,
				ins.VIEWDELIVERYADDRESS,
				ins.VIEWLISTTOTALS,
				ins.ORDERBY,
				ins.VIEWRESTAURANT,
				ins.VIEWGRID,
				ins.VIEWCOUNTDOWN,
				ins.VIEWPROGRESSSTATUS,
				ins.DIRECTEDITOPERATION,
				ins.SETTINGSFROMHOSPTYPE,
				ins.SETTINGSFROMSEQUENCE,
				ins.SHARINGSALESTYPEFILTER,
				ins.SETTINGSFROMRESTAURANT,
				ins.GUESTBUTTONS,
				ins.MAXGUESTBUTTONSSHOWN,
				ins.MAXGUESTSPERTABLE,
				ins.SPLITBILLLOOKUPID,
				ins.SELECTGUESTONSPLITTING,
				ins.COMBINESPLITLINESACTION,
				ins.TRANSFERLINESLOOKUPID,
				ins.PRINTTRAININGTRANSACTIONS,
				ins.LAYOUTID,
				ins.TOPPOSMENUID,
				ins.DININGTABLELAYOUTID,
				ins.AUTOMATICJOININGCHECK,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.HOSPITALITYTYPELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				RESTAURANTID,
				SEQUENCE,
				DESCRIPTION,
				OVERVIEW,
				SALESTYPE,
				UPDATETABLEFROMPOS,
				REQUESTNOOFGUESTS,
				STATIONPRINTING,
				ACCESSTOOTHERRESTAURANT,
				POSLOGONMENUID,
				ALLOWNEWENTRIES,
				TIPSAMTLINE1,
				TIPSAMTLINE2,
				TIPSTOTALLINE,
				STAYINPOSAFTERTRANS,
				TIPSINCOMEACC1,
				TIPSINCOMEACC2,
				NOOFDINEINTABLES,
				TABLEBUTTONPOSMENUID,
				TABLEBUTTONDESCRIPTION,
				TABLEBUTTONSTAFFDESCRIPTION,
				STAFFTAKEOVERINTRANS,
				MANAGERTAKEOVERINTRANS,
				VIEWSALESSTAFF,
				VIEWTRANSDATE,
				VIEWTRANSTIME,
				VIEWDELIVERYADDRESS,
				VIEWLISTTOTALS,
				ORDERBY,
				VIEWRESTAURANT,
				VIEWGRID,
				VIEWCOUNTDOWN,
				VIEWPROGRESSSTATUS,
				DIRECTEDITOPERATION,
				SETTINGSFROMHOSPTYPE,
				SETTINGSFROMSEQUENCE,
				SHARINGSALESTYPEFILTER,
				SETTINGSFROMRESTAURANT,
				GUESTBUTTONS,
				MAXGUESTBUTTONSSHOWN,
				MAXGUESTSPERTABLE,
				SPLITBILLLOOKUPID,
				SELECTGUESTONSPLITTING,
				COMBINESPLITLINESACTION,
				TRANSFERLINESLOOKUPID,
				PRINTTRAININGTRANSACTIONS,
				LAYOUTID,
				TOPPOSMENUID,
				DININGTABLELAYOUTID,
				AUTOMATICJOININGCHECK,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.RESTAURANTID,
				ins.SEQUENCE,
				ins.DESCRIPTION,
				ins.OVERVIEW,
				ins.SALESTYPE,
				ins.UPDATETABLEFROMPOS,
				ins.REQUESTNOOFGUESTS,
				ins.STATIONPRINTING,
				ins.ACCESSTOOTHERRESTAURANT,
				ins.POSLOGONMENUID,
				ins.ALLOWNEWENTRIES,
				ins.TIPSAMTLINE1,
				ins.TIPSAMTLINE2,
				ins.TIPSTOTALLINE,
				ins.STAYINPOSAFTERTRANS,
				ins.TIPSINCOMEACC1,
				ins.TIPSINCOMEACC2,
				ins.NOOFDINEINTABLES,
				ins.TABLEBUTTONPOSMENUID,
				ins.TABLEBUTTONDESCRIPTION,
				ins.TABLEBUTTONSTAFFDESCRIPTION,
				ins.STAFFTAKEOVERINTRANS,
				ins.MANAGERTAKEOVERINTRANS,
				ins.VIEWSALESSTAFF,
				ins.VIEWTRANSDATE,
				ins.VIEWTRANSTIME,
				ins.VIEWDELIVERYADDRESS,
				ins.VIEWLISTTOTALS,
				ins.ORDERBY,
				ins.VIEWRESTAURANT,
				ins.VIEWGRID,
				ins.VIEWCOUNTDOWN,
				ins.VIEWPROGRESSSTATUS,
				ins.DIRECTEDITOPERATION,
				ins.SETTINGSFROMHOSPTYPE,
				ins.SETTINGSFROMSEQUENCE,
				ins.SHARINGSALESTYPEFILTER,
				ins.SETTINGSFROMRESTAURANT,
				ins.GUESTBUTTONS,
				ins.MAXGUESTBUTTONSSHOWN,
				ins.MAXGUESTSPERTABLE,
				ins.SPLITBILLLOOKUPID,
				ins.SELECTGUESTONSPLITTING,
				ins.COMBINESPLITLINESACTION,
				ins.TRANSFERLINESLOOKUPID,
				ins.PRINTTRAININGTRANSACTIONS,
				ins.LAYOUTID,
				ins.TOPPOSMENUID,
				ins.DININGTABLELAYOUTID,
				ins.AUTOMATICJOININGCHECK,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOCUSTTABLE]'))
begin
   drop trigger dbo.Update_RBOCUSTTABLE
end

GO

create trigger Update_RBOCUSTTABLE
on RBOCUSTTABLE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOCUSTTABLELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ACCOUNTNUM,
                OTHERTENDERINFINALIZING,
                POSTASSHIPMENT,
                DATAAREAID,
                USEORDERNUMBERREFERENCE,
                RECEIPTOPTION,
                RECEIPTEMAIL,
                NONCHARGABLEACCOUNT,
                REQUIRESAPPROVAL,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ACCOUNTNUM,
                ins.OTHERTENDERINFINALIZING,
                ins.POSTASSHIPMENT,
                ins.DATAAREAID,
                ins.USEORDERNUMBERREFERENCE,
                ins.RECEIPTOPTION,
                ins.RECEIPTEMAIL,
                ins.NONCHARGABLEACCOUNT,
                ins.REQUIRESAPPROVAL,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOCUSTTABLELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ACCOUNTNUM,
                OTHERTENDERINFINALIZING,
                POSTASSHIPMENT,
                DATAAREAID,
                USEORDERNUMBERREFERENCE,
                RECEIPTOPTION,
                RECEIPTEMAIL,
                NONCHARGABLEACCOUNT,
                REQUIRESAPPROVAL,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ACCOUNTNUM,
                ins.OTHERTENDERINFINALIZING,
                ins.POSTASSHIPMENT,
                ins.DATAAREAID,
                ins.USEORDERNUMBERREFERENCE,
                ins.RECEIPTOPTION,
                ins.RECEIPTEMAIL,
                ins.NONCHARGABLEACCOUNT,
                ins.REQUIRESAPPROVAL,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSLOOKUP]'))
begin
   drop trigger dbo.Update_POSLOOKUP
end

GO

create trigger Update_POSLOOKUP
on POSLOOKUP after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSLOOKUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				LOOKUPID,
				DESCRIPTION,
				DYNAMICMENUID,
				DYNAMICMENU2ID,
				GRID1MENUID,
				GRID2MENUID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.LOOKUPID,
				ins.DESCRIPTION,
				ins.DYNAMICMENUID,
				ins.DYNAMICMENU2ID,
				ins.GRID1MENUID,
				ins.GRID2MENUID,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSLOOKUPLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				LOOKUPID,
				DESCRIPTION,
				DYNAMICMENUID,
				DYNAMICMENU2ID,
				GRID1MENUID,
				GRID2MENUID,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.LOOKUPID,
				ins.DESCRIPTION,
				ins.DYNAMICMENUID,
				ins.DYNAMICMENU2ID,
				ins.GRID1MENUID,
				ins.GRID2MENUID,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSMENUHEADER]'))
begin
   drop trigger dbo.Update_POSMENUHEADER
end

GO

create trigger Update_POSMENUHEADER
on POSMENUHEADER after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSMENUHEADERLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                MENUID,
                DESCRIPTION,
                COLUMNS,
                ROWS,
                MENUCOLOR,
                FONTNAME,
                FONTSIZE,
                FONTBOLD,
                FORECOLOR,
                BACKCOLOR,
                FONTITALIC,
                FONTCHARSET,
                USENAVOPERATION,
                DATAAREAID,
                APPLIESTO,
                BACKCOLOR2,
                GRADIENTMODE,
                SHAPE,
                MENUTYPE,
                STYLEID,
				DEVICETYPE,
				MAINMENU,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MENUID,
                ins.DESCRIPTION,
                ins.COLUMNS,
                ins.ROWS,
                ins.MENUCOLOR,
                ins.FONTNAME,
                ins.FONTSIZE,
                ins.FONTBOLD,
                ins.FORECOLOR,
                ins.BACKCOLOR,
                ins.FONTITALIC,
                ins.FONTCHARSET,
                ins.USENAVOPERATION,
                ins.DATAAREAID,
                ins.APPLIESTO,
                ins.BACKCOLOR2,
                ins.GRADIENTMODE,
                ins.SHAPE,
                ins.MENUTYPE,
                ins.STYLEID,
				ins.DEVICETYPE,
				ins.MAINMENU,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSMENUHEADERLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                MENUID,
                DESCRIPTION,
                COLUMNS,
                ROWS,
                MENUCOLOR,
                FONTNAME,
                FONTSIZE,
                FONTBOLD,
                FORECOLOR,
                BACKCOLOR,
                FONTITALIC,
                FONTCHARSET,
                USENAVOPERATION,
                DATAAREAID,
                APPLIESTO,
                BACKCOLOR2,
                GRADIENTMODE,
                SHAPE,
                MENUTYPE,
                STYLEID,
				DEVICETYPE,
				MAINMENU,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MENUID,
                ins.DESCRIPTION,
                ins.COLUMNS,
                ins.ROWS,
                ins.MENUCOLOR,
                ins.FONTNAME,
                ins.FONTSIZE,
                ins.FONTBOLD,
                ins.FORECOLOR,
                ins.BACKCOLOR,
                ins.FONTITALIC,
                ins.FONTCHARSET,
                ins.USENAVOPERATION,
                ins.DATAAREAID,
                ins.APPLIESTO,
                ins.BACKCOLOR2,
                ins.GRADIENTMODE,
                ins.SHAPE,
                ins.MENUTYPE,
                ins.STYLEID,
				ins.DEVICETYPE,
				ins.MAINMENU,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_USERLOGINTOKENS]'))
begin
   drop trigger dbo.Update_USERLOGINTOKENS
end

GO

create trigger Update_USERLOGINTOKENS
on USERLOGINTOKENS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.USERLOGINTOKENSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                GUID,
                USERGUID,
                DESCRIPTION,
                HASH,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.GUID,
                ins.USERGUID,
                ins.DESCRIPTION,
                ins.HASH,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.USERLOGINTOKENSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                GUID,
                USERGUID,
                DESCRIPTION,
                HASH,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.GUID,
                ins.USERGUID,
                ins.DESCRIPTION,
                ins.HASH,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSMENULINE]'))
begin
   drop trigger dbo.Update_POSMENULINE
end

GO

create trigger Update_POSMENULINE
on POSMENULINE after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.POSMENULINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				MENUID,
				SEQUENCE,
				KEYNO,
				DESCRIPTION,
				OPERATION,
				PARAMETER,
				PARAMETERTYPE,
				FONTNAME,
				FONTSIZE,
				FONTBOLD,
				FORECOLOR,
				BACKCOLOR,
				FONTITALIC,
				FONTCHARSET,
				DISABLED,
				PICTUREFILE,
				HIDEDESCRONPICTURE,
				FONTSTRIKETHROUGH,
				FONTUNDERLINE,
				COLUMNSPAN,
				ROWSPAN,
				NAVOPERATION,
				DATAAREAID,
				HIDDEN,
				SHADEWHENDISABLED,
				BACKGROUNDHIDDEN,
				TRANSPARENT,
				GLYPH,
				GLYPH2,
				GLYPH3,
				GLYPH4,
				GLYPHTEXT,
				GLYPHTEXT2,
				GLYPHTEXT3,
				GLYPHTEXT4,
				GLYPHTEXTFONT,
				GLYPHTEXT2FONT,
				GLYPHTEXT3FONT,
				GLYPHTEXT4FONT,
				GLYPHTEXTFONTSIZE,
				GLYPHTEXT2FONTSIZE,
				GLYPHTEXT3FONTSIZE,
				GLYPHTEXT4FONTSIZE,
				GLYPHTEXTFORECOLOR,
				GLYPHTEXT2FORECOLOR,
				GLYPHTEXT3FORECOLOR,
				GLYPHTEXT4FORECOLOR,
				GLYPHOFFSET,
				GLYPH2OFFSET,
				GLYPH3OFFSET,
				GLYPH4OFFSET,
				BACKCOLOR2,
				GRADIENTMODE,
				SHAPE,
				USEHEADERFONT,
				USEHEADERATTRIBUTES,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.MENUID,
				ins.SEQUENCE,
				ins.KEYNO,
				ins.DESCRIPTION,
				ins.OPERATION,
				ins.PARAMETER,
				ins.PARAMETERTYPE,
				ins.FONTNAME,
				ins.FONTSIZE,
				ins.FONTBOLD,
				ins.FORECOLOR,
				ins.BACKCOLOR,
				ins.FONTITALIC,
				ins.FONTCHARSET,
				ins.DISABLED,
				ins.PICTUREFILE,
				ins.HIDEDESCRONPICTURE,
				ins.FONTSTRIKETHROUGH,
				ins.FONTUNDERLINE,
				ins.COLUMNSPAN,
				ins.ROWSPAN,
				ins.NAVOPERATION,
				ins.DATAAREAID,
				ins.HIDDEN,
				ins.SHADEWHENDISABLED,
				ins.BACKGROUNDHIDDEN,
				ins.TRANSPARENT,
				ins.GLYPH,
				ins.GLYPH2,
				ins.GLYPH3,
				ins.GLYPH4,
				ins.GLYPHTEXT,
				ins.GLYPHTEXT2,
				ins.GLYPHTEXT3,
				ins.GLYPHTEXT4,
				ins.GLYPHTEXTFONT,
				ins.GLYPHTEXT2FONT,
				ins.GLYPHTEXT3FONT,
				ins.GLYPHTEXT4FONT,
				ins.GLYPHTEXTFONTSIZE,
				ins.GLYPHTEXT2FONTSIZE,
				ins.GLYPHTEXT3FONTSIZE,
				ins.GLYPHTEXT4FONTSIZE,
				ins.GLYPHTEXTFORECOLOR,
				ins.GLYPHTEXT2FORECOLOR,
				ins.GLYPHTEXT3FORECOLOR,
				ins.GLYPHTEXT4FORECOLOR,
				ins.GLYPHOFFSET,
				ins.GLYPH2OFFSET,
				ins.GLYPH3OFFSET,
				ins.GLYPH4OFFSET,
				ins.BACKCOLOR2,
				ins.GRADIENTMODE,
				ins.SHAPE,
				ins.USEHEADERFONT,
				ins.USEHEADERATTRIBUTES,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.POSMENULINELog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				MENUID,
				SEQUENCE,
				KEYNO,
				DESCRIPTION,
				OPERATION,
				PARAMETER,
				PARAMETERTYPE,
				FONTNAME,
				FONTSIZE,
				FONTBOLD,
				FORECOLOR,
				BACKCOLOR,
				FONTITALIC,
				FONTCHARSET,
				DISABLED,
				PICTUREFILE,
				HIDEDESCRONPICTURE,
				FONTSTRIKETHROUGH,
				FONTUNDERLINE,
				COLUMNSPAN,
				ROWSPAN,
				NAVOPERATION,
				DATAAREAID,
				HIDDEN,
				SHADEWHENDISABLED,
				BACKGROUNDHIDDEN,
				TRANSPARENT,
				GLYPH,
				GLYPH2,
				GLYPH3,
				GLYPH4,
				GLYPHTEXT,
				GLYPHTEXT2,
				GLYPHTEXT3,
				GLYPHTEXT4,
				GLYPHTEXTFONT,
				GLYPHTEXT2FONT,
				GLYPHTEXT3FONT,
				GLYPHTEXT4FONT,
				GLYPHTEXTFONTSIZE,
				GLYPHTEXT2FONTSIZE,
				GLYPHTEXT3FONTSIZE,
				GLYPHTEXT4FONTSIZE,
				GLYPHTEXTFORECOLOR,
				GLYPHTEXT2FORECOLOR,
				GLYPHTEXT3FORECOLOR,
				GLYPHTEXT4FORECOLOR,
				GLYPHOFFSET,
				GLYPH2OFFSET,
				GLYPH3OFFSET,
				GLYPH4OFFSET,
				BACKCOLOR2,
				GRADIENTMODE,
				SHAPE,
				USEHEADERFONT,
				USEHEADERATTRIBUTES,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.MENUID,
				ins.SEQUENCE,
				ins.KEYNO,
				ins.DESCRIPTION,
				ins.OPERATION,
				ins.PARAMETER,
				ins.PARAMETERTYPE,
				ins.FONTNAME,
				ins.FONTSIZE,
				ins.FONTBOLD,
				ins.FORECOLOR,
				ins.BACKCOLOR,
				ins.FONTITALIC,
				ins.FONTCHARSET,
				ins.DISABLED,
				ins.PICTUREFILE,
				ins.HIDEDESCRONPICTURE,
				ins.FONTSTRIKETHROUGH,
				ins.FONTUNDERLINE,
				ins.COLUMNSPAN,
				ins.ROWSPAN,
				ins.NAVOPERATION,
				ins.DATAAREAID,
				ins.HIDDEN,
				ins.SHADEWHENDISABLED,
				ins.BACKGROUNDHIDDEN,
				ins.TRANSPARENT,
				ins.GLYPH,
				ins.GLYPH2,
				ins.GLYPH3,
				ins.GLYPH4,
				ins.GLYPHTEXT,
				ins.GLYPHTEXT2,
				ins.GLYPHTEXT3,
				ins.GLYPHTEXT4,
				ins.GLYPHTEXTFONT,
				ins.GLYPHTEXT2FONT,
				ins.GLYPHTEXT3FONT,
				ins.GLYPHTEXT4FONT,
				ins.GLYPHTEXTFONTSIZE,
				ins.GLYPHTEXT2FONTSIZE,
				ins.GLYPHTEXT3FONTSIZE,
				ins.GLYPHTEXT4FONTSIZE,
				ins.GLYPHTEXTFORECOLOR,
				ins.GLYPHTEXT2FORECOLOR,
				ins.GLYPHTEXT3FORECOLOR,
				ins.GLYPHTEXT4FORECOLOR,
				ins.GLYPHOFFSET,
				ins.GLYPH2OFFSET,
				ins.GLYPH3OFFSET,
				ins.GLYPH4OFFSET,
				ins.BACKCOLOR2,
				ins.GRADIENTMODE,
				ins.SHAPE,
				ins.USEHEADERFONT,
				ins.USEHEADERATTRIBUTES,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RESTAURANTSTATION]'))
begin
   drop trigger dbo.Update_RESTAURANTSTATION
end

GO

create trigger Update_RESTAURANTSTATION
on RESTAURANTSTATION after insert,update, delete as

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
			insert into LSPOSNET_Audit.dbo.RESTAURANTSTATIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ID,
				STATIONNAME,
				WINDOWSPRINTER,
				OUTPUTLINES,
				CHECKPRINTING,
				POSEXTERNALPRINTERID,
				PRINTING,
				STATIONFILTER,
				STATIONCHECKMINUTES,
				COMPRESSBOMRECEIPT,
				EXCLUDEFROMCOMPRESSION,
				STATIONTYPE,
				PRINTINGPRIORITY,
				ENDTURNSREDAFTERMIN,
				SCREENALIGNMENT,
				SCREENNUMBER,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ID,
				ins.STATIONNAME,
				ins.WINDOWSPRINTER,
				ins.OUTPUTLINES,
				ins.CHECKPRINTING,
				ins.POSEXTERNALPRINTERID,
				ins.PRINTING,
				ins.STATIONFILTER,
				ins.STATIONCHECKMINUTES,
				ins.COMPRESSBOMRECEIPT,
				ins.EXCLUDEFROMCOMPRESSION,
				ins.STATIONTYPE,
				ins.PRINTINGPRIORITY,
				ins.ENDTURNSREDAFTERMIN,
				ins.SCREENALIGNMENT,
				ins.SCREENNUMBER,
				ins.DATAAREAID,
				1 as Deleted
				From DELETED ins
		end
		else
		begin
			-- If we got here then we are inserting new or deleting existing
			insert into LSPOSNET_Audit.dbo.RESTAURANTSTATIONLog (
				AuditUserGUID, 
				AuditUserLogin,
				AuditDate,
				ID,
				STATIONNAME,
				WINDOWSPRINTER,
				OUTPUTLINES,
				CHECKPRINTING,
				POSEXTERNALPRINTERID,
				PRINTING,
				STATIONFILTER,
				STATIONCHECKMINUTES,
				COMPRESSBOMRECEIPT,
				EXCLUDEFROMCOMPRESSION,
				STATIONTYPE,
				PRINTINGPRIORITY,
				ENDTURNSREDAFTERMIN,
				SCREENALIGNMENT,
				SCREENNUMBER,
				DATAAREAID,
				Deleted
				)
			Select 
				@connectionUser, @sessionUser as AuditUserLogin, 
				GETDATE() as AuditDate,
				ins.ID,
				ins.STATIONNAME,
				ins.WINDOWSPRINTER,
				ins.OUTPUTLINES,
				ins.CHECKPRINTING,
				ins.POSEXTERNALPRINTERID,
				ins.PRINTING,
				ins.STATIONFILTER,
				ins.STATIONCHECKMINUTES,
				ins.COMPRESSBOMRECEIPT,
				ins.EXCLUDEFROMCOMPRESSION,
				ins.STATIONTYPE,
				ins.PRINTINGPRIORITY,
				ins.ENDTURNSREDAFTERMIN,
				ins.SCREENALIGNMENT,
				ins.SCREENNUMBER,
				ins.DATAAREAID,
				0 as Deleted
				From inserted ins
		end
	end try
	begin catch
	
	end catch
	
end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_DININGTABLELAYOUTSCREEN]'))
begin
   drop trigger dbo.Update_DININGTABLELAYOUTSCREEN
end

GO

create trigger Update_DININGTABLELAYOUTSCREEN
on DININGTABLELAYOUTSCREEN after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.DININGTABLELAYOUTSCREENLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                RESTAURANTID,
                SEQUENCE,
                SALESTYPE,
                LAYOUTID,
                SCREENNO,
                SCREENDESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.RESTAURANTID,
                ins.SEQUENCE,
                ins.SALESTYPE,
                ins.LAYOUTID,
                ins.SCREENNO,
                ins.SCREENDESCRIPTION,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.DININGTABLELAYOUTSCREENLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                RESTAURANTID,
                SEQUENCE,
                SALESTYPE,
                LAYOUTID,
                SCREENNO,
                SCREENDESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.RESTAURANTID,
                ins.SEQUENCE,
                ins.SALESTYPE,
                ins.LAYOUTID,
                ins.SCREENNO,
                ins.SCREENDESCRIPTION,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSCOLOR]'))
begin
   drop trigger dbo.Update_POSCOLOR
end

GO

create trigger Update_POSCOLOR
on POSCOLOR after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSCOLORLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                COLORCODE,
                DESCRIPTION,
                COLOR,
                BOLD,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.COLORCODE,
                ins.DESCRIPTION,
                ins.COLOR,
                ins.BOLD,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSCOLORLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                COLORCODE,
                DESCRIPTION,
                COLOR,
                BOLD,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.COLORCODE,
                ins.DESCRIPTION,
                ins.COLOR,
                ins.BOLD,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RESTAURANTDININGTABLE]'))
begin
   drop trigger dbo.Update_RESTAURANTDININGTABLE
end

GO

create trigger Update_RESTAURANTDININGTABLE
on RESTAURANTDININGTABLE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RESTAURANTDININGTABLELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                RESTAURANTID,
                SALESTYPE,
                DINEINTABLENO,
                DESCRIPTION,
                TYPE,
                NONSMOKING,
                NOOFGUESTS,
                JOINEDTABLE,
                X1POSITION,
                X2POSITION,
                Y1POSITION,
                Y2POSITION,
                LINKEDTODINEINTABLE,
                DININGTABLESJOINED,
                SEQUENCE,
                AVAILABILITY,
                DININGTABLELAYOUTID,
                LAYOUTSCREENNO,
                DESCRIPTIONONBUTTON,
                SHAPE,
                X1POSITIONDESIGN,
                X2POSITIONDESIGN,
                Y1POSITIONDESIGN,
                Y2POSITIONDESIGN,
                LAYOUTSCREENNODESIGN,
                JOINEDTABLEDESIGN,
                DININGTABLESJOINEDDESIGN,
                DELETEINOTHERLAYOUTS,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.RESTAURANTID,
                ins.SALESTYPE,
                ins.DINEINTABLENO,
                ins.DESCRIPTION,
                ins.TYPE,
                ins.NONSMOKING,
                ins.NOOFGUESTS,
                ins.JOINEDTABLE,
                ins.X1POSITION,
                ins.X2POSITION,
                ins.Y1POSITION,
                ins.Y2POSITION,
                ins.LINKEDTODINEINTABLE,
                ins.DININGTABLESJOINED,
                ins.SEQUENCE,
                ins.AVAILABILITY,
                ins.DININGTABLELAYOUTID,
                ins.LAYOUTSCREENNO,
                ins.DESCRIPTIONONBUTTON,
                ins.SHAPE,
                ins.X1POSITIONDESIGN,
                ins.X2POSITIONDESIGN,
                ins.Y1POSITIONDESIGN,
                ins.Y2POSITIONDESIGN,
                ins.LAYOUTSCREENNODESIGN,
                ins.JOINEDTABLEDESIGN,
                ins.DININGTABLESJOINEDDESIGN,
                ins.DELETEINOTHERLAYOUTS,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RESTAURANTDININGTABLELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                RESTAURANTID,
                SALESTYPE,
                DINEINTABLENO,
                DESCRIPTION,
                TYPE,
                NONSMOKING,
                NOOFGUESTS,
                JOINEDTABLE,
                X1POSITION,
                X2POSITION,
                Y1POSITION,
                Y2POSITION,
                LINKEDTODINEINTABLE,
                DININGTABLESJOINED,
                SEQUENCE,
                AVAILABILITY,
                DININGTABLELAYOUTID,
                LAYOUTSCREENNO,
                DESCRIPTIONONBUTTON,
                SHAPE,
                X1POSITIONDESIGN,
                X2POSITIONDESIGN,
                Y1POSITIONDESIGN,
                Y2POSITIONDESIGN,
                LAYOUTSCREENNODESIGN,
                JOINEDTABLEDESIGN,
                DININGTABLESJOINEDDESIGN,
                DELETEINOTHERLAYOUTS,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.RESTAURANTID,
                ins.SALESTYPE,
                ins.DINEINTABLENO,
                ins.DESCRIPTION,
                ins.TYPE,
                ins.NONSMOKING,
                ins.NOOFGUESTS,
                ins.JOINEDTABLE,
                ins.X1POSITION,
                ins.X2POSITION,
                ins.Y1POSITION,
                ins.Y2POSITION,
                ins.LINKEDTODINEINTABLE,
                ins.DININGTABLESJOINED,
                ins.SEQUENCE,
                ins.AVAILABILITY,
                ins.DININGTABLELAYOUTID,
                ins.LAYOUTSCREENNO,
                ins.DESCRIPTIONONBUTTON,
                ins.SHAPE,
                ins.X1POSITIONDESIGN,
                ins.X2POSITIONDESIGN,
                ins.Y1POSITIONDESIGN,
                ins.Y2POSITIONDESIGN,
                ins.LAYOUTSCREENNODESIGN,
                ins.JOINEDTABLEDESIGN,
                ins.DININGTABLESJOINEDDESIGN,
                ins.DELETEINOTHERLAYOUTS,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RESTAURANTMENUTYPE]'))
begin
   drop trigger dbo.Update_RESTAURANTMENUTYPE
end

GO

create trigger Update_RESTAURANTMENUTYPE
on RESTAURANTMENUTYPE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RESTAURANTMENUTYPELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                RESTAURANTID,
                MENUORDER,
                DESCRIPTION,
                CODEONPOS,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.RESTAURANTID,
                ins.MENUORDER,
                ins.DESCRIPTION,
                ins.CODEONPOS,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RESTAURANTMENUTYPELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                RESTAURANTID,
                MENUORDER,
                DESCRIPTION,
                CODEONPOS,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.RESTAURANTID,
                ins.MENUORDER,
                ins.DESCRIPTION,
                ins.CODEONPOS,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_STATIONPRINTINGROUTES]'))
begin
   drop trigger dbo.Update_STATIONPRINTINGROUTES
end

GO

create trigger Update_STATIONPRINTINGROUTES
on STATIONPRINTINGROUTES after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.STATIONPRINTINGROUTESLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                ROUTEDESCRIPTION,
                RESTAURANTID,
                PASSWORD,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.ROUTEDESCRIPTION,
                ins.RESTAURANTID,
                ins.PASSWORD,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.STATIONPRINTINGROUTESLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                ROUTEDESCRIPTION,
                RESTAURANTID,
                PASSWORD,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.ROUTEDESCRIPTION,
                ins.RESTAURANTID,
                ins.PASSWORD,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_STATIONSELECTION]'))
begin
   drop trigger dbo.Update_STATIONSELECTION
end

GO

create trigger Update_STATIONSELECTION
on STATIONSELECTION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.STATIONSELECTIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                TYPE,
                CODE,
                STATIONID,
                SALESTYPE,
                RESTAURANTID,
                DATAAREAID,
                DESCRIPTION,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.TYPE,
                ins.CODE,
                ins.STATIONID,
                ins.SALESTYPE,
                ins.RESTAURANTID,
                ins.DATAAREAID,
                ins.DESCRIPTION,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.STATIONSELECTIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                TYPE,
                CODE,
                STATIONID,
                SALESTYPE,
                RESTAURANTID,
                DATAAREAID,
                DESCRIPTION,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.TYPE,
                ins.CODE,
                ins.STATIONID,
                ins.SALESTYPE,
                ins.RESTAURANTID,
                ins.DATAAREAID,
                ins.DESCRIPTION,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOGIFTCARDTRANSACTIONS]'))
begin
   drop trigger dbo.Update_RBOGIFTCARDTRANSACTIONS
end

GO

create trigger Update_RBOGIFTCARDTRANSACTIONS
on RBOGIFTCARDTRANSACTIONS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOGIFTCARDTRANSACTIONSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                GIFTCARDID,
                GIFTCARDLINEID,
                STOREID,
                TERMINALID,
                TRANSACTIONNUMBER,
                RECEIPTID,
                STAFFID,
                USERID,
                TRANSACTIONDATE,
                TRANSACTIONTIME,
                AMOUNT,
                OPERATION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.GIFTCARDID,
                ins.GIFTCARDLINEID,
                ins.STOREID,
                ins.TERMINALID,
                ins.TRANSACTIONNUMBER,
                ins.RECEIPTID,
                ins.STAFFID,
                ins.USERID,
                ins.TRANSACTIONDATE,
                ins.TRANSACTIONTIME,
                ins.AMOUNT,
                ins.OPERATION,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOGIFTCARDTRANSACTIONSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                GIFTCARDID,
                GIFTCARDLINEID,
                STOREID,
                TERMINALID,
                TRANSACTIONNUMBER,
                RECEIPTID,
                STAFFID,
                USERID,
                TRANSACTIONDATE,
                TRANSACTIONTIME,
                AMOUNT,
                OPERATION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.GIFTCARDID,
                ins.GIFTCARDLINEID,
                ins.STOREID,
                ins.TERMINALID,
                ins.TRANSACTIONNUMBER,
                ins.RECEIPTID,
                ins.STAFFID,
                ins.USERID,
                ins.TRANSACTIONDATE,
                ins.TRANSACTIONTIME,
                ins.AMOUNT,
                ins.OPERATION,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOGIFTCARDTABLE]'))
begin
   drop trigger dbo.Update_RBOGIFTCARDTABLE
end

GO

create trigger Update_RBOGIFTCARDTABLE
on RBOGIFTCARDTABLE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOGIFTCARDTABLELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                GIFTCARDID,
                BALANCE,
                CURRENCY,
                ACTIVE,
                DATAAREAID,
                REFILLABLE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.GIFTCARDID,
                ins.BALANCE,
                ins.CURRENCY,
                ins.ACTIVE,
                ins.DATAAREAID,
                ins.REFILLABLE,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOGIFTCARDTABLELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                GIFTCARDID,
                BALANCE,
                CURRENCY,
                ACTIVE,
                DATAAREAID,
                REFILLABLE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.GIFTCARDID,
                ins.BALANCE,
                ins.CURRENCY,
                ins.ACTIVE,
                ins.DATAAREAID,
                ins.REFILLABLE,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINVENTLINKEDITEM]'))
begin
   drop trigger dbo.Update_RBOINVENTLINKEDITEM
end

GO

create trigger Update_RBOINVENTLINKEDITEM
on RBOINVENTLINKEDITEM after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOINVENTLINKEDITEMLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ITEMID,
				UNIT,
				LINKEDITEMID,
				QTY,
				BLOCKED,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ITEMID,
				ins.UNIT,
				ins.LINKEDITEMID,
				ins.QTY,
				ins.BLOCKED,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOINVENTLINKEDITEMLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ITEMID,
				UNIT,
				LINKEDITEMID,
				QTY,
				BLOCKED,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ITEMID,
				ins.UNIT,
				ins.LINKEDITEMID,
				ins.QTY,
				ins.BLOCKED,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOCREDITVOUCHERTABLE]'))
begin
   drop trigger dbo.Update_RBOCREDITVOUCHERTABLE
end

GO

create trigger Update_RBOCREDITVOUCHERTABLE
on RBOCREDITVOUCHERTABLE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOCREDITVOUCHERTABLELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                VOUCHERID,
                BALANCE,
                CURRENCY,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.VOUCHERID,
                ins.BALANCE,
                ins.CURRENCY,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOCREDITVOUCHERTABLELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                VOUCHERID,
                BALANCE,
                CURRENCY,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.VOUCHERID,
                ins.BALANCE,
                ins.CURRENCY,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOCREDITVOUCHERTRANSACTIONS]'))
begin
   drop trigger dbo.Update_RBOCREDITVOUCHERTRANSACTIONS
end

GO

create trigger Update_RBOCREDITVOUCHERTRANSACTIONS
on RBOCREDITVOUCHERTRANSACTIONS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOCREDITVOUCHERTRANSACTIONSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                VOUCHERID,
                VOUCHERLINEID,
                STOREID,
                TERMINALID,
                TRANSACTIONNUMBER,
                RECEIPTID,
                STAFFID,
                USERID,
                TRANSACTIONDATE,
                TRANSACTIONTIME,
                AMOUNT,
                OPERATION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.VOUCHERID,
                ins.VOUCHERLINEID,
                ins.STOREID,
                ins.TERMINALID,
                ins.TRANSACTIONNUMBER,
                ins.RECEIPTID,
                ins.STAFFID,
                ins.USERID,
                ins.TRANSACTIONDATE,
                ins.TRANSACTIONTIME,
                ins.AMOUNT,
                ins.OPERATION,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOCREDITVOUCHERTRANSACTIONSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                VOUCHERID,
                VOUCHERLINEID,
                STOREID,
                TERMINALID,
                TRANSACTIONNUMBER,
                RECEIPTID,
                STAFFID,
                USERID,
                TRANSACTIONDATE,
                TRANSACTIONTIME,
                AMOUNT,
                OPERATION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.VOUCHERID,
                ins.VOUCHERLINEID,
                ins.STOREID,
                ins.TERMINALID,
                ins.TRANSACTIONNUMBER,
                ins.RECEIPTID,
                ins.STAFFID,
                ins.USERID,
                ins.TRANSACTIONDATE,
                ins.TRANSACTIONTIME,
                ins.AMOUNT,
                ins.OPERATION,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
------------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISSUSPENSIONTYPE]'))
begin
   drop trigger dbo.Update_POSISSUSPENSIONTYPE
end

GO

create trigger Update_POSISSUSPENSIONTYPE
on POSISSUSPENSIONTYPE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSISSUSPENSIONTYPELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                ALLOWEOD,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.ALLOWEOD,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSISSUSPENSIONTYPELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                ALLOWEOD,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.ALLOWEOD,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISSUSPENSIONADDINFO]'))
begin
   drop trigger dbo.Update_POSISSUSPENSIONADDINFO
end

GO

create trigger Update_POSISSUSPENSIONADDINFO
on POSISSUSPENSIONADDINFO after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSISSUSPENSIONADDINFOLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                SUSPENSIONTYPEID,
                PROMPT,
                FIELDORDER,
                INFOTYPE,
                INFOTYPESELECTION,
                REQUIRED,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.SUSPENSIONTYPEID,
                ins.PROMPT,
                ins.FIELDORDER,
                ins.INFOTYPE,
                ins.INFOTYPESELECTION,
                ins.REQUIRED,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSISSUSPENSIONADDINFOLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                SUSPENSIONTYPEID,
                PROMPT,
                FIELDORDER,
                INFOTYPE,
                INFOTYPESELECTION,
                REQUIRED,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.SUSPENSIONTYPEID,
                ins.PROMPT,
                ins.FIELDORDER,
                ins.INFOTYPE,
                ins.INFOTYPESELECTION,
                ins.REQUIRED,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO



-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISSUSPENDEDTRANSACTIONS]'))
begin
   drop trigger dbo.Update_POSISSUSPENDEDTRANSACTIONS
end

GO

create trigger Update_POSISSUSPENDEDTRANSACTIONS
on POSISSUSPENDEDTRANSACTIONS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSISSUSPENDEDTRANSACTIONSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                TRANSACTIONID,
                BYTELENGTH,
                TRANSDATE,
                STAFF,
                BALANCE,
                STOREID,
                TERMINALID,
                DATAAREAID,
                RECALLEDBY,
                DESCRIPTION,
                ALLOWEOD,
                ACTIVE,
                SUSPENSIONTYPEID,
                BALANCEWITHTAX,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.TRANSACTIONID,
                ins.BYTELENGTH,
                ins.TRANSDATE,
                ins.STAFF,
                ins.BALANCE,
                ins.STOREID,
                ins.TERMINALID,
                ins.DATAAREAID,
                ins.RECALLEDBY,
                ins.DESCRIPTION,
                ins.ALLOWEOD,
                ins.ACTIVE,
                ins.SUSPENSIONTYPEID,
                ins.BALANCEWITHTAX,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSISSUSPENDEDTRANSACTIONSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                TRANSACTIONID,
                BYTELENGTH,
                TRANSDATE,
                STAFF,
                BALANCE,
                STOREID,
                TERMINALID,
                DATAAREAID,
                RECALLEDBY,
                DESCRIPTION,
                ALLOWEOD,
                ACTIVE,
                SUSPENSIONTYPEID,
                BALANCEWITHTAX,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.TRANSACTIONID,
                ins.BYTELENGTH,
                ins.TRANSDATE,
                ins.STAFF,
                ins.BALANCE,
                ins.STOREID,
                ins.TERMINALID,
                ins.DATAAREAID,
                ins.RECALLEDBY,
                ins.DESCRIPTION,
                ins.ALLOWEOD,
                ins.ACTIVE,
                ins.SUSPENSIONTYPEID,
                ins.BALANCEWITHTAX,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSISSUSPENDTRANSADDINFO]'))
begin
   drop trigger dbo.Update_POSISSUSPENDTRANSADDINFO
end

GO

create trigger Update_POSISSUSPENDTRANSADDINFO
on POSISSUSPENDTRANSADDINFO after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSISSUSPENDTRANSADDINFOLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                TRANSACTIONID,
                PROMPT,
                FIELDORDER,
                INFOTYPE,
                INFOTYPESELECTION,
                TEXTRESULT1,
                TEXTRESULT2,
                TEXTRESULT3,
                TEXTRESULT4,
                TEXTRESULT5,
                TEXTRESULT6,
                DATERESULT,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.TRANSACTIONID,
                ins.PROMPT,
                ins.FIELDORDER,
                ins.INFOTYPE,
                ins.INFOTYPESELECTION,
                ins.TEXTRESULT1,
                ins.TEXTRESULT2,
                ins.TEXTRESULT3,
                ins.TEXTRESULT4,
                ins.TEXTRESULT5,
                ins.TEXTRESULT6,
                ins.DATERESULT,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSISSUSPENDTRANSADDINFOLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                TRANSACTIONID,
                PROMPT,
                FIELDORDER,
                INFOTYPE,
                INFOTYPESELECTION,
                TEXTRESULT1,
                TEXTRESULT2,
                TEXTRESULT3,
                TEXTRESULT4,
                TEXTRESULT5,
                TEXTRESULT6,
                DATERESULT,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.TRANSACTIONID,
                ins.PROMPT,
                ins.FIELDORDER,
                ins.INFOTYPE,
                ins.INFOTYPESELECTION,
                ins.TEXTRESULT1,
                ins.TEXTRESULT2,
                ins.TEXTRESULT3,
                ins.TEXTRESULT4,
                ins.TEXTRESULT5,
                ins.TEXTRESULT6,
                ins.DATERESULT,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO



-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOMSRCARDTABLE]'))
begin
   drop trigger dbo.Update_RBOMSRCARDTABLE
end

GO

create trigger Update_RBOMSRCARDTABLE
on RBOMSRCARDTABLE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOMSRCARDTABLELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                LINKTYPE,
                LINKID,
                CARDNUMBER,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.LINKTYPE,
                ins.LINKID,
                ins.CARDNUMBER,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOMSRCARDTABLELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                LINKTYPE,
                LINKID,
                CARDNUMBER,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.LINKTYPE,
                ins.LINKID,
                ins.CARDNUMBER,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBODISCOUNTOFFERLINE]'))
begin
   drop trigger dbo.Update_RBODISCOUNTOFFERLINE
end

GO

create trigger Update_RBODISCOUNTOFFERLINE
on RBODISCOUNTOFFERLINE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBODISCOUNTOFFERLINELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                OFFERID,
                STATUS,
                ITEMRELATION,
                NAME,
                STANDARDPRICEINCLTAX_DEL,
                STANDARDPRICE_DEL,
                DISCPCT,
                DISCAMOUNT,
                OFFERPRICE,
                OFFERPRICEINCLTAX,
                UNIT,
                TYPE,
                CURRENCY_DEL,
                DISCONPOS_DEL,
                DISCAMOUNTINCLTAX,
                MODIFIEDDATE,
                MODIFIEDTIME,
                MODIFIEDBY,
                MODIFIEDTRANSACTIONID,
                DATAAREAID,
                ID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.OFFERID,
                ins.STATUS,
                ins.ITEMRELATION,
                ins.NAME,
                ins.STANDARDPRICEINCLTAX_DEL,
                ins.STANDARDPRICE_DEL,
                ins.DISCPCT,
                ins.DISCAMOUNT,
                ins.OFFERPRICE,
                ins.OFFERPRICEINCLTAX,
                ins.UNIT,
                ins.TYPE,
                ins.CURRENCY_DEL,
                ins.DISCONPOS_DEL,
                ins.DISCAMOUNTINCLTAX,
                ins.MODIFIEDDATE,
                ins.MODIFIEDTIME,
                ins.MODIFIEDBY,
                ins.MODIFIEDTRANSACTIONID,
                ins.DATAAREAID,
                ins.ID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBODISCOUNTOFFERLINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                OFFERID,
                STATUS,
                ITEMRELATION,
                NAME,
                STANDARDPRICEINCLTAX_DEL,
                STANDARDPRICE_DEL,
                DISCPCT,
                DISCAMOUNT,
                OFFERPRICE,
                OFFERPRICEINCLTAX,
                UNIT,
                TYPE,
                CURRENCY_DEL,
                DISCONPOS_DEL,
                DISCAMOUNTINCLTAX,
                MODIFIEDDATE,
                MODIFIEDTIME,
                MODIFIEDBY,
                MODIFIEDTRANSACTIONID,
                DATAAREAID,
                ID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.OFFERID,
                ins.STATUS,
                ins.ITEMRELATION,
                ins.NAME,
                ins.STANDARDPRICEINCLTAX_DEL,
                ins.STANDARDPRICE_DEL,
                ins.DISCPCT,
                ins.DISCAMOUNT,
                ins.OFFERPRICE,
                ins.OFFERPRICEINCLTAX,
                ins.UNIT,
                ins.TYPE,
                ins.CURRENCY_DEL,
                ins.DISCONPOS_DEL,
                ins.DISCAMOUNTINCLTAX,
                ins.MODIFIEDDATE,
                ins.MODIFIEDTIME,
                ins.MODIFIEDBY,
                ins.MODIFIEDTRANSACTIONID,
                ins.DATAAREAID,
                ins.ID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


------------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINVENTTABLE]'))
begin
   drop trigger dbo.Update_RBOINVENTTABLE
end

GO

create trigger Update_RBOINVENTTABLE
on RBOINVENTTABLE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOINVENTTABLELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ITEMID,
                ITEMTYPE,
                ITEMGROUP,
                ITEMDEPARTMENT,
                ITEMFAMILY,
                UNITPRICEINCLUDINGTAX,
                COSTCALCULATIONONPOS,
                NOINVENTPOSTING,
                ZEROPRICEVALID,
                QTYBECOMESNEGATIVE,
                NODISCOUNTALLOWED,
                KEYINGINPRICE,
                SCALEITEM,
                KEYINGINQTY,
                DATEBLOCKED,
                DATETOBEBLOCKED,
                BLOCKEDONPOS,
                DISPENSEPRINTINGDISABLED,
                --DISPENSEPRINTERGROUPID,
                --BASECOMPARISONUNITCODE,
                --BARCODESETUPID,
                PRINTVARIANTSSHELFLABELS,
                --COLORGROUP,
                --SIZEGROUP,
                USEEANSTANDARDBARCODE,
                STYLEGROUP,
                DATAAREAID,
                FUELITEM,
                GRADEID,
                MUSTKEYINCOMMENT,
                DATETOACTIVATEITEM,
                --BUSINESSGROUP,
                --DIVISIONGROUP,
                DEFAULTPROFIT,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ITEMID,
                ins.ITEMTYPE,
                ins.ITEMGROUP,
                ins.ITEMDEPARTMENT,
                ins.ITEMFAMILY,
                ins.UNITPRICEINCLUDINGTAX,
                ins.COSTCALCULATIONONPOS,
                ins.NOINVENTPOSTING,
                ins.ZEROPRICEVALID,
                ins.QTYBECOMESNEGATIVE,
                ins.NODISCOUNTALLOWED,
                ins.KEYINGINPRICE,
                ins.SCALEITEM,
                ins.KEYINGINQTY,
                ins.DATEBLOCKED,
                ins.DATETOBEBLOCKED,
                ins.BLOCKEDONPOS,
                ins.DISPENSEPRINTINGDISABLED,
                --ins.DISPENSEPRINTERGROUPID,
                --ins.BASECOMPARISONUNITCODE,
                --ins.BARCODESETUPID,
                ins.PRINTVARIANTSSHELFLABELS,
                --ins.COLORGROUP,
                --ins.SIZEGROUP,
                ins.USEEANSTANDARDBARCODE,
                ins.STYLEGROUP,
                ins.DATAAREAID,
                ins.FUELITEM,
                ins.GRADEID,
                ins.MUSTKEYINCOMMENT,
                ins.DATETOACTIVATEITEM,
                --ins.BUSINESSGROUP,
                --ins.DIVISIONGROUP,
                ins.DEFAULTPROFIT,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOINVENTTABLELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ITEMID,
                ITEMTYPE,
                ITEMGROUP,
                ITEMDEPARTMENT,
                ITEMFAMILY,
                UNITPRICEINCLUDINGTAX,
                COSTCALCULATIONONPOS,
                NOINVENTPOSTING,
                ZEROPRICEVALID,
                QTYBECOMESNEGATIVE,
                NODISCOUNTALLOWED,
                KEYINGINPRICE,
                SCALEITEM,
                KEYINGINQTY,
                DATEBLOCKED,
                DATETOBEBLOCKED,
                BLOCKEDONPOS,
                DISPENSEPRINTINGDISABLED,
                --DISPENSEPRINTERGROUPID,
                --BASECOMPARISONUNITCODE,
                --BARCODESETUPID,
                PRINTVARIANTSSHELFLABELS,
                --COLORGROUP,
                --SIZEGROUP,
                USEEANSTANDARDBARCODE,
                STYLEGROUP,
                DATAAREAID,
                FUELITEM,
                GRADEID,
                MUSTKEYINCOMMENT,
                DATETOACTIVATEITEM,
                --BUSINESSGROUP,
                --DIVISIONGROUP,
                DEFAULTPROFIT,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ITEMID,
                ins.ITEMTYPE,
                ins.ITEMGROUP,
                ins.ITEMDEPARTMENT,
                ins.ITEMFAMILY,
                ins.UNITPRICEINCLUDINGTAX,
                ins.COSTCALCULATIONONPOS,
                ins.NOINVENTPOSTING,
                ins.ZEROPRICEVALID,
                ins.QTYBECOMESNEGATIVE,
                ins.NODISCOUNTALLOWED,
                ins.KEYINGINPRICE,
                ins.SCALEITEM,
                ins.KEYINGINQTY,
                ins.DATEBLOCKED,
                ins.DATETOBEBLOCKED,
                ins.BLOCKEDONPOS,
                ins.DISPENSEPRINTINGDISABLED,
                --ins.DISPENSEPRINTERGROUPID,
                --ins.BASECOMPARISONUNITCODE,
                --ins.BARCODESETUPID,
                ins.PRINTVARIANTSSHELFLABELS,
                --ins.COLORGROUP,
                --ins.SIZEGROUP,
                ins.USEEANSTANDARDBARCODE,
                ins.STYLEGROUP,
                ins.DATAAREAID,
                ins.FUELITEM,
                ins.GRADEID,
                ins.MUSTKEYINCOMMENT,
                ins.DATETOACTIVATEITEM,
                --ins.BUSINESSGROUP,
                --ins.DIVISIONGROUP,
                ins.DEFAULTPROFIT,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOINVENTTRANSLATIONS]'))
begin
   drop trigger dbo.Update_RBOINVENTTRANSLATIONS
end

GO

create trigger Update_RBOINVENTTRANSLATIONS
on RBOINVENTTRANSLATIONS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOINVENTTRANSLATIONSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                ITEMID,
                CULTURENAME,
                DESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.ITEMID,
                ins.CULTURENAME,
                ins.DESCRIPTION,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOINVENTTRANSLATIONSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                ITEMID,
                CULTURENAME,
                DESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.ITEMID,
                ins.CULTURENAME,
                ins.DESCRIPTION,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KMPROFILE]'))
begin
   drop trigger dbo.Update_KMPROFILE
end

GO

create trigger Update_KMPROFILE
on KITCHENDISPLAYTRANSACTIONPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYTRANSACTIONPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                KITCHENMANAGERSERVER,
                KITCHENMANAGERPORT,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.KITCHENMANAGERSERVER,
                ins.KITCHENMANAGERPORT,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYTRANSACTIONPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                KITCHENMANAGERSERVER,
                KITCHENMANAGERPORT,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.KITCHENMANAGERSERVER,
                ins.KITCHENMANAGERPORT,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOTERMINALGROUP]'))
begin
   drop trigger dbo.Update_RBOTERMINALGROUP
end

GO
create trigger Update_RBOTERMINALGROUP
on RBOTERMINALGROUP after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOTERMINALGROUPLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOTERMINALGROUPLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RBOTERMINALGROUPCONNECTION]'))
begin
   drop trigger dbo.Update_RBOTERMINALGROUPCONNECTION
end

GO

create trigger Update_RBOTERMINALGROUPCONNECTION
on RBOTERMINALGROUPCONNECTION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RBOTERMINALGROUPCONNECTIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                TERMINALGROUPID,
                TERMINALID,
				STOREID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.TERMINALGROUPID,
                ins.TERMINALID,
				ins.STOREID,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RBOTERMINALGROUPCONNECTIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                TERMINALGROUPID,
                TERMINALID,
				STOREID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.TERMINALGROUPID,
                ins.TERMINALID,
				ins.STOREID,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSSTYLEPROFILE]'))
begin
   drop trigger dbo.Update_POSSTYLEPROFILE
end

GO

create trigger Update_POSSTYLEPROFILE
on POSSTYLEPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSSTYLEPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSSTYLEPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSSTYLEPROFILELINE]'))
begin
   drop trigger dbo.Update_POSSTYLEPROFILELINE
end

GO

create trigger Update_POSSTYLEPROFILELINE
on POSSTYLEPROFILELINE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSSTYLEPROFILELINELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                PROFILEID,
                MENUID,
                CONTEXTID,
                STYLEID,
                SYSTEM,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.PROFILEID,
                ins.MENUID,
                ins.CONTEXTID,
                ins.STYLEID,
                ins.SYSTEM,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSSTYLEPROFILELINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                PROFILEID,
                MENUID,
                CONTEXTID,
                STYLEID,
                SYSTEM,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.PROFILEID,
                ins.MENUID,
                ins.CONTEXTID,
                ins.STYLEID,
                ins.SYSTEM,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSCONTEXT]'))
begin
   drop trigger dbo.Update_POSCONTEXT
end

GO

create trigger Update_POSCONTEXT
on POSCONTEXT after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSCONTEXTLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                MENUREQUIRED,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.MENUREQUIRED,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSCONTEXTLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                MENUREQUIRED,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.MENUREQUIRED,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYSTATION]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYSTATION
end

GO

create trigger Update_KITCHENDISPLAYSTATION
on KITCHENDISPLAYSTATION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYSTATIONSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                SCREENNUMBER,
                KITCHENDISPLAYFUNCTIONALPROFILEID,
                KITCHENDISPLAYSTYLEPROFILEID,
                KITCHENDISPLAYVISUALPROFILEID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.SCREENNUMBER,
                ins.KITCHENDISPLAYFUNCTIONALPROFILEID,
                ins.KITCHENDISPLAYSTYLEPROFILEID,
                ins.KITCHENDISPLAYVISUALPROFILEID,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYSTATIONSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                SCREENNUMBER,
                KITCHENDISPLAYFUNCTIONALPROFILEID,
                KITCHENDISPLAYSTYLEPROFILEID,
                KITCHENDISPLAYVISUALPROFILEID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.SCREENNUMBER,
                ins.KITCHENDISPLAYFUNCTIONALPROFILEID,
                ins.KITCHENDISPLAYSTYLEPROFILEID,
                ins.KITCHENDISPLAYVISUALPROFILEID,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYITEMCONNECTION]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYITEMCONNECTION
end

GO

create trigger Update_KITCHENDISPLAYITEMCONNECTION
on KITCHENDISPLAYITEMCONNECTION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYITEMCONNECTIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                STATIONID,
                TYPE,
                CONNECTIONVALUE,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.STATIONID,
                ins.TYPE,
                ins.CONNECTIONVALUE,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYITEMCONNECTIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                STATIONID,
                TYPE,
                CONNECTIONVALUE,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.STATIONID,
                ins.TYPE,
                ins.CONNECTIONVALUE,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYVISUALPROFILE]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYVISUALPROFILE
end

GO

create trigger Update_KITCHENDISPLAYVISUALPROFILE
on KITCHENDISPLAYVISUALPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYVISUALPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                ORDERPANEWIDTH,
                ORDERPANEHEIGHT,
                ORDERPANEX,
                ORDERPANEY,
                ORDERPANEVISIBLE,
                BUTTONPANEWIDTH,
                BUTTONPANEHEIGHT,
                BUTTONPANEX,
                BUTTONPANEY,
                BUTTONPANEVISIBLE,
                NUMBEROFCOLUMNS,
                NUMBEROFROWS,
                ITEMMODIFIERINCREASEPREFIX,
                ITEMMODIFIERDECREASEPREFIX,
                ITEMMODIFIERNORMALPREFIX,
                DATAAREAID,
                SHOWDEALS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.ORDERPANEWIDTH,
                ins.ORDERPANEHEIGHT,
                ins.ORDERPANEX,
                ins.ORDERPANEY,
                ins.ORDERPANEVISIBLE,
                ins.BUTTONPANEWIDTH,
                ins.BUTTONPANEHEIGHT,
                ins.BUTTONPANEX,
                ins.BUTTONPANEY,
                ins.BUTTONPANEVISIBLE,
                ins.NUMBEROFCOLUMNS,
                ins.NUMBEROFROWS,
                ins.ITEMMODIFIERINCREASEPREFIX,
                ins.ITEMMODIFIERDECREASEPREFIX,
                ins.ITEMMODIFIERNORMALPREFIX,
                ins.DATAAREAID,
                ins.SHOWDEALS,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYVISUALPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                ORDERPANEWIDTH,
                ORDERPANEHEIGHT,
                ORDERPANEX,
                ORDERPANEY,
                ORDERPANEVISIBLE,
                BUTTONPANEWIDTH,
                BUTTONPANEHEIGHT,
                BUTTONPANEX,
                BUTTONPANEY,
                BUTTONPANEVISIBLE,
                NUMBEROFCOLUMNS,
                NUMBEROFROWS,
                ITEMMODIFIERINCREASEPREFIX,
                ITEMMODIFIERDECREASEPREFIX,
                ITEMMODIFIERNORMALPREFIX,
                DATAAREAID,
                SHOWDEALS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.ORDERPANEWIDTH,
                ins.ORDERPANEHEIGHT,
                ins.ORDERPANEX,
                ins.ORDERPANEY,
                ins.ORDERPANEVISIBLE,
                ins.BUTTONPANEWIDTH,
                ins.BUTTONPANEHEIGHT,
                ins.BUTTONPANEX,
                ins.BUTTONPANEY,
                ins.BUTTONPANEVISIBLE,          
                ins.NUMBEROFCOLUMNS,
                ins.NUMBEROFROWS,
                ins.ITEMMODIFIERINCREASEPREFIX,
                ins.ITEMMODIFIERDECREASEPREFIX,
                ins.ITEMMODIFIERNORMALPREFIX,
                ins.DATAAREAID,
                ins.SHOWDEALS,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYSTYLEPROFILE]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYSTYLEPROFILE
end

GO

create trigger Update_KITCHENDISPLAYSTYLEPROFILE
on KITCHENDISPLAYSTYLEPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYSTYLEPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYSTYLEPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYTERMINALCONNECTION]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYTERMINALCONNECTION
end

GO

create trigger Update_KITCHENDISPLAYTERMINALCONNECTION
on KITCHENDISPLAYTERMINALCONNECTION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYTERMINALCONNECTIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                STATIONID,
                TYPE,
                CONNECTIONVALUE,
                DATAAREAID,
				TERMINALID,
				STOREID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.STATIONID,
                ins.TYPE,
                ins.CONNECTIONVALUE,
                ins.DATAAREAID,
				ins.TERMINALID,
				ins.STOREID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYTERMINALCONNECTIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                STATIONID,
                TYPE,
                CONNECTIONVALUE,
                DATAAREAID,
				TERMINALID,
				STOREID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.STATIONID,
                ins.TYPE,
                ins.CONNECTIONVALUE,
                ins.DATAAREAID,
				ins.TERMINALID,
				ins.STOREID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYHOSPITALITYTYPECONNECTION]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYHOSPITALITYTYPECONNECTION
end

GO

create trigger Update_KITCHENDISPLAYHOSPITALITYTYPECONNECTION
on KITCHENDISPLAYHOSPITALITYTYPECONNECTION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYHOSPITALITYTYPECONNECTIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                STATIONID,
                TYPE,
                CONNECTIONVALUE,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.STATIONID,
                ins.TYPE,
                ins.CONNECTIONVALUE,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYHOSPITALITYTYPECONNECTIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                STATIONID,
                TYPE,
                CONNECTIONVALUE,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.STATIONID,
                ins.TYPE,
                ins.CONNECTIONVALUE,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_KITCHENDISPLAYFUNCTIONALPROFILE]'))
begin
   drop trigger dbo.Update_KITCHENDISPLAYFUNCTIONALPROFILE
end

GO

create trigger Update_KITCHENDISPLAYFUNCTIONALPROFILE
on KITCHENDISPLAYFUNCTIONALPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYFUNCTIONALPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
				BUMPPOSSIBLE,
                BUTTONSMENUID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
				ins.BUMPPOSSIBLE,
                ins.BUTTONSMENUID,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.KITCHENDISPLAYFUNCTIONALPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
				BUMPPOSSIBLE,
                BUTTONSMENUID,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
				ins.BUMPPOSSIBLE,
                ins.BUTTONSMENUID,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTRANSFERORDER]'))
begin
   drop trigger dbo.Update_INVENTORYTRANSFERORDER
end

GO

create trigger Update_INVENTORYTRANSFERORDER
on INVENTORYTRANSFERORDER after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERORDERLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERREQUESTID,
				SENDINGSTOREID,
				RECEIVINGSTOREID,
				CREATIONDATE,
				RECEIVINGDATE,
				SENTDATE,
				RECEIVED,
				SENT,
				FETCHEDBYRECEIVINGSTORE,
				CREATEDBY,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERREQUESTID,
				ins.SENDINGSTOREID,
				ins.RECEIVINGSTOREID,
				ins.CREATIONDATE,
				ins.RECEIVINGDATE,
				ins.SENTDATE,
				ins.RECEIVED,
				ins.SENT,
				ins.FETCHEDBYRECEIVINGSTORE,
				ins.CREATEDBY,
				ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERORDERLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERREQUESTID,
				SENDINGSTOREID,
				RECEIVINGSTOREID,
				CREATIONDATE,
				RECEIVINGDATE,
				SENTDATE,
				RECEIVED,
				SENT,
				FETCHEDBYRECEIVINGSTORE,
				CREATEDBY,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERREQUESTID,
				ins.SENDINGSTOREID,
				ins.RECEIVINGSTOREID,
				ins.CREATIONDATE,
				ins.RECEIVINGDATE,
				ins.SENTDATE,
				ins.RECEIVED,
				ins.SENT,
				ins.FETCHEDBYRECEIVINGSTORE,
				ins.CREATEDBY,
				ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTRANSFERORDERLINE]'))
begin
   drop trigger dbo.Update_INVENTORYTRANSFERORDERLINE
end

GO

create trigger Update_INVENTORYTRANSFERORDERLINE
on INVENTORYTRANSFERORDERLINE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERORDERLINELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERORDERID,
				ITEMID,
				UNITID,
				QUANTITYSENT,
				QUANTITYRECEIVED,
				SENT,
				DATAAREAID,
				PICTUREID,
				OMNITRANSACTIONID,
				OMNILINEID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERORDERID,
				ins.ITEMID,
				ins.UNITID,
				ins.QUANTITYSENT,
				ins.QUANTITYRECEIVED,
				ins.SENT,
				ins.DATAAREAID,
				ins.PICTUREID,
				ins.OMNITRANSACTIONID,
				ins.OMNILINEID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERORDERLINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERORDERID,
				ITEMID,
				UNITID,
				QUANTITYSENT,
				QUANTITYRECEIVED,
				SENT,
				DATAAREAID,
				PICTUREID,
				OMNITRANSACTIONID,
				OMNILINEID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERORDERID,
				ins.ITEMID,
				ins.UNITID,
				ins.QUANTITYSENT,
				ins.QUANTITYRECEIVED,
				ins.SENT,
				ins.DATAAREAID,
				ins.PICTUREID,
				ins.OMNITRANSACTIONID,
				ins.OMNILINEID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTRANSFERREQUEST]'))
begin
   drop trigger dbo.Update_INVENTORYTRANSFERREQUEST
end

GO


create trigger Update_INVENTORYTRANSFERREQUEST
on INVENTORYTRANSFERREQUEST after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERREQUESTLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
				SENDINGSTOREID,
				RECEIVINGSTOREID,
				CREATIONDATE,
				SENTDATE,
				SENT,
				FETCHEDBYRECEIVINGSTORE,
				INVENTORYTRANSFERCREATED,
				CREATEDBY,
				INVENTORYTRANSFERORDERID,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.SENDINGSTOREID,
				ins.RECEIVINGSTOREID,
				ins.CREATIONDATE,
				ins.SENTDATE,
				ins.SENT,
				ins.FETCHEDBYRECEIVINGSTORE,
				ins.INVENTORYTRANSFERCREATED,
				ins.CREATEDBY,
				ins.INVENTORYTRANSFERORDERID,
				ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERREQUESTLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
				SENDINGSTOREID,
				RECEIVINGSTOREID,
				CREATIONDATE,
				SENTDATE,
				SENT,
				FETCHEDBYRECEIVINGSTORE,
				INVENTORYTRANSFERCREATED,
				CREATEDBY,
				INVENTORYTRANSFERORDERID,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.SENDINGSTOREID,
				ins.RECEIVINGSTOREID,
				ins.CREATIONDATE,
				ins.SENTDATE,
				ins.SENT,
				ins.FETCHEDBYRECEIVINGSTORE,
				ins.INVENTORYTRANSFERCREATED,
				ins.CREATEDBY,
				ins.INVENTORYTRANSFERORDERID,
				ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTRANSFERREQUESTLINE]'))
begin
   drop trigger dbo.Update_INVENTORYTRANSFERREQUESTLINE
end

GO


create trigger Update_INVENTORYTRANSFERREQUESTLINE
on INVENTORYTRANSFERREQUESTLINE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERREQUESTLINELog (
                AuditUserGUID,
                AuditUserLogin,
                 AuditDate,
                ID,
				INVENTORYTRANSFERREQUESTID,
				ITEMID,
				UNITID,
				QUANTITYREQUESTED,
				SENT,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERREQUESTID,
				ins.ITEMID,
				ins.UNITID,
				ins.QUANTITYREQUESTED,
				ins.SENT,
				ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTRANSFERREQUESTLINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
				INVENTORYTRANSFERREQUESTID,
				ITEMID,
				UNITID,
				QUANTITYREQUESTED,
				SENT,
				DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
				ins.INVENTORYTRANSFERREQUESTID,
				ins.ITEMID,
				ins.UNITID,
				ins.QUANTITYREQUESTED,
				ins.SENT,
				ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CUSTGROUP]'))
begin
   drop trigger dbo.Update_CUSTGROUP
end

GO

create trigger Update_CUSTGROUP
on CUSTGROUP after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.CUSTGROUPLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                DATAAREAID,
                EXCLUSIVE,
                CATEGORY,
                PURCHASEAMOUNT,
				USEPURCHASELIMIT,
				PURCHASEPERIOD,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.DATAAREAID,
                ins.EXCLUSIVE,
                ins.CATEGORY,
                ins.PURCHASEAMOUNT,
				ins.USEPURCHASELIMIT,
				ins.PURCHASEPERIOD,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.CUSTGROUPLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                DATAAREAID,
                EXCLUSIVE,
                CATEGORY,
                PURCHASEAMOUNT,
				USEPURCHASELIMIT,
				PURCHASEPERIOD,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.DATAAREAID,
                ins.EXCLUSIVE,
                ins.CATEGORY,
                ins.PURCHASEAMOUNT,
				ins.USEPURCHASELIMIT,
				ins.PURCHASEPERIOD,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CUSTGROUPCATEGORY]'))
begin
   drop trigger dbo.Update_CUSTGROUPCATEGORY
end

GO

create trigger Update_CUSTGROUPCATEGORY
on CUSTGROUPCATEGORY after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.CUSTGROUPCATEGORYLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.CUSTGROUPCATEGORYLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_DIMENSIONATTRIBUTE]'))
begin
   drop trigger dbo.Update_DIMENSIONATTRIBUTE
end

GO

create trigger Update_DIMENSIONATTRIBUTE
on DIMENSIONATTRIBUTE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.DIMENSIONATTRIBUTELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                DIMENSIONID,
                DESCRIPTION,
                CODE,
                SEQUENCE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DIMENSIONID,
                ins.DESCRIPTION,
                ins.CODE,
                ins.SEQUENCE,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.DIMENSIONATTRIBUTELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                DIMENSIONID,
                DESCRIPTION,
                CODE,
                SEQUENCE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DIMENSIONID,
                ins.DESCRIPTION,
                ins.CODE,
                ins.SEQUENCE,
                ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_DIMENSIONTEMPLATE]'))
begin
   drop trigger dbo.Update_DIMENSIONTEMPLATE
end

GO

create trigger Update_DIMENSIONTEMPLATE
on DIMENSIONTEMPLATE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.DIMENSIONTEMPLATELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.DIMENSIONTEMPLATELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                DESCRIPTION,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.DESCRIPTION,
                ins.Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RETAILITEM]'))
begin
   drop trigger dbo.Update_RETAILITEM
end

GO

create trigger Update_RETAILITEM
on RETAILITEM after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RETAILITEMLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ITEMID,
                HEADERITEMID,
                ITEMNAME,
                VARIANTNAME,
                ITEMTYPE,
                DEFAULTVENDORID,
                NAMEALIAS,
                RETAILGROUPMASTERID,
                ZEROPRICEVALID,
                QTYBECOMESNEGATIVE,
                NODISCOUNTALLOWED,
                KEYINPRICE,
                SCALEITEM,
                KEYINQTY,
                BLOCKEDONPOS,
                BARCODESETUPID,
                PRINTVARIANTSSHELFLABELS,
                FUELITEM,
                GRADEID,
                MUSTKEYINCOMMENT,
                DATETOBEBLOCKED,
                DATETOACTIVATEITEM,
                PROFITMARGIN,
                VALIDATIONPERIODID,
                MUSTSELECTUOM,
                INVENTORYUNITID,
                PURCHASEUNITID,
                SALESUNITID,
                PURCHASEPRICE,
                SALESPRICE,
                SALESPRICEINCLTAX,
                SALESMARKUP,
                SALESLINEDISC,
                SALESMULTILINEDISC,
                SALESALLOWTOTALDISCOUNT,
                SALESTAXITEMGROUPID,
				RETURNABLE,
				EXTENDEDDESCRIPTION,
				SEARCHKEYWORDS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ITEMID,
                ins.HEADERITEMID,
                ins.ITEMNAME,
                ins.VARIANTNAME,
                ins.ITEMTYPE,
                ins.DEFAULTVENDORID,
                ins.NAMEALIAS,
                ins.RETAILGROUPMASTERID,
                ins.ZEROPRICEVALID,
                ins.QTYBECOMESNEGATIVE,
                ins.NODISCOUNTALLOWED,
                ins.KEYINPRICE,
                ins.SCALEITEM,
                ins.KEYINQTY,
                ins.BLOCKEDONPOS,
                ins.BARCODESETUPID,
                ins.PRINTVARIANTSSHELFLABELS,
                ins.FUELITEM,
                ins.GRADEID,
                ins.MUSTKEYINCOMMENT,
                ins.DATETOBEBLOCKED,
                ins.DATETOACTIVATEITEM,
                ins.PROFITMARGIN,
                ins.VALIDATIONPERIODID,
                ins.MUSTSELECTUOM,
                ins.INVENTORYUNITID,
                ins.PURCHASEUNITID,
                ins.SALESUNITID,
                ins.PURCHASEPRICE,
                ins.SALESPRICE,
                ins.SALESPRICEINCLTAX,
                ins.SALESMARKUP,
                ins.SALESLINEDISC,
                ins.SALESMULTILINEDISC,
                ins.SALESALLOWTOTALDISCOUNT,
                ins.SALESTAXITEMGROUPID,
				ins.RETURNABLE,
				ins.EXTENDEDDESCRIPTION,
				ins.SEARCHKEYWORDS,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RETAILITEMLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ITEMID,
                HEADERITEMID,
                ITEMNAME,
                VARIANTNAME,
                ITEMTYPE,
                DEFAULTVENDORID,
                NAMEALIAS,
                RETAILGROUPMASTERID,
                ZEROPRICEVALID,
                QTYBECOMESNEGATIVE,
                NODISCOUNTALLOWED,
                KEYINPRICE,
                SCALEITEM,
                KEYINQTY,
                BLOCKEDONPOS,
                BARCODESETUPID,
                PRINTVARIANTSSHELFLABELS,
                FUELITEM,
                GRADEID,
                MUSTKEYINCOMMENT,
                DATETOBEBLOCKED,
                DATETOACTIVATEITEM,
                PROFITMARGIN,
                VALIDATIONPERIODID,
                MUSTSELECTUOM,
                INVENTORYUNITID,
                PURCHASEUNITID,
                SALESUNITID,
                PURCHASEPRICE,
                SALESPRICE,
                SALESPRICEINCLTAX,
                SALESMARKUP,
                SALESLINEDISC,
                SALESMULTILINEDISC,
                SALESALLOWTOTALDISCOUNT,
                SALESTAXITEMGROUPID,
				RETURNABLE,
				EXTENDEDDESCRIPTION,
				SEARCHKEYWORDS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ITEMID,
                ins.HEADERITEMID,
                ins.ITEMNAME,
                ins.VARIANTNAME,
                ins.ITEMTYPE,
                ins.DEFAULTVENDORID,
                ins.NAMEALIAS,
                ins.RETAILGROUPMASTERID,
                ins.ZEROPRICEVALID,
                ins.QTYBECOMESNEGATIVE,
                ins.NODISCOUNTALLOWED,
                ins.KEYINPRICE,
                ins.SCALEITEM,
                ins.KEYINQTY,
                ins.BLOCKEDONPOS,
                ins.BARCODESETUPID,
                ins.PRINTVARIANTSSHELFLABELS,
                ins.FUELITEM,
                ins.GRADEID,
                ins.MUSTKEYINCOMMENT,
                ins.DATETOBEBLOCKED,
                ins.DATETOACTIVATEITEM,
                ins.PROFITMARGIN,
                ins.VALIDATIONPERIODID,
                ins.MUSTSELECTUOM,
                ins.INVENTORYUNITID,
                ins.PURCHASEUNITID,
                ins.SALESUNITID,
                ins.PURCHASEPRICE,
                ins.SALESPRICE,
                ins.SALESPRICEINCLTAX,
                ins.SALESMARKUP,
                ins.SALESLINEDISC,
                ins.SALESMULTILINEDISC,
                ins.SALESALLOWTOTALDISCOUNT,
                ins.SALESTAXITEMGROUPID,
				ins.RETURNABLE,
				ins.EXTENDEDDESCRIPTION,
				ins.SEARCHKEYWORDS,
                ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CUSTOMERORDERSETTINGS]'))
begin
   drop trigger dbo.Update_CUSTOMERORDERSETTINGS
end

GO

create trigger Update_CUSTOMERORDERSETTINGS
on CUSTOMERORDERSETTINGS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.CUSTOMERORDERSETTINGSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ORDERTYPE,
                ACCEPTSDEPOSITS,
                MINIMUMDEPOSITS,
                SOURCE,
                DELIVERY,                
                EXPIRATIONTIMEVALUE,
                EXPIRATIONTIMEUNIT,
                NUMBERSERIES,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ORDERTYPE,
                ins.ACCEPTSDEPOSITS,
                ins.MINIMUMDEPOSITS,
                ins.SOURCE,
                ins.DELIVERY,                
                ins.EXPIRATIONTIMEVALUE,
                ins.EXPIRATIONTIMEUNIT,
                ins.NUMBERSERIES,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.CUSTOMERORDERSETTINGSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ORDERTYPE,
                ACCEPTSDEPOSITS,
                MINIMUMDEPOSITS,
                SOURCE,
                DELIVERY,
                EXPIRATIONTIMEVALUE,
                EXPIRATIONTIMEUNIT,
                NUMBERSERIES,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ORDERTYPE,
                ins.ACCEPTSDEPOSITS,
                ins.MINIMUMDEPOSITS,
                ins.SOURCE,
                ins.DELIVERY,                
                ins.EXPIRATIONTIMEVALUE,
                ins.EXPIRATIONTIMEUNIT,
                ins.NUMBERSERIES,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO 

---------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_IMPORTPROFILE]'))
begin
   drop trigger dbo.Update_IMPORTPROFILE
end

GO

create trigger Update_IMPORTPROFILE
on IMPORTPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.IMPORTPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ID,
                DESCRIPTION,
                IMPORTTYPE,
                [DEFAULT],
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ID,
                ins.DESCRIPTION,
                ins.IMPORTTYPE,
                ins.[DEFAULT],
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.IMPORTPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ID,
                DESCRIPTION,
                IMPORTTYPE,
                [DEFAULT],
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ID,
                ins.DESCRIPTION,
                ins.IMPORTTYPE,
                ins.[DEFAULT],
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_IMPORTPROFILELINES]'))
begin
   drop trigger dbo.Update_IMPORTPROFILELINES
end

GO

create trigger Update_IMPORTPROFILELINES
on IMPORTPROFILELINES after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.IMPORTPROFILELINESLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                MASTERID,
                IMPORTPROFILEMASTERID,
                FIELD,
                FIELDTYPE,
                SEQUENCE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.IMPORTPROFILEMASTERID,
                ins.FIELD,
                ins.FIELDTYPE,
                ins.SEQUENCE,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.IMPORTPROFILELINESLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                MASTERID,
                IMPORTPROFILEMASTERID,
                FIELD,
                FIELDTYPE,
                SEQUENCE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.IMPORTPROFILEMASTERID,
                ins.FIELD,
                ins.FIELDTYPE,
                ins.SEQUENCE,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

---------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_POSUSERPROFILE]'))
begin
   drop trigger dbo.Update_POSUSERPROFILE
end

GO

create trigger Update_POSUSERPROFILE
on POSUSERPROFILE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.POSUSERPROFILELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                PROFILEID,
                DATAAREAID,
                DESCRIPTION,
                MAXLINEDISCOUNTAMOUNT,
                MAXDISCOUNTPCT,
                MAXTOTALDISCOUNTAMOUNT,
                MAXTOTALDISCOUNTPCT,
                MAXLINERETURNAMOUNT,
                MAXTOTALRETURNAMOUNT,
                STOREID,
                VISUALPROFILE,
                LAYOUTID,
                KEYBOARDCODE,
                LAYOUTNAME,
                OPERATORCULTURE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.PROFILEID,
                ins.DATAAREAID,
                ins.DESCRIPTION,
                ins.MAXLINEDISCOUNTAMOUNT,
                ins.MAXDISCOUNTPCT,
                ins.MAXTOTALDISCOUNTAMOUNT,
                ins.MAXTOTALDISCOUNTPCT,
                ins.MAXLINERETURNAMOUNT,
                ins.MAXTOTALRETURNAMOUNT,
                ins.STOREID,
                ins.VISUALPROFILE,
                ins.LAYOUTID,
                ins.KEYBOARDCODE,
                ins.LAYOUTNAME,
                ins.OPERATORCULTURE,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.POSUSERPROFILELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                PROFILEID,
                DATAAREAID,
                DESCRIPTION,
                MAXLINEDISCOUNTAMOUNT,
                MAXDISCOUNTPCT,
                MAXTOTALDISCOUNTAMOUNT,
                MAXTOTALDISCOUNTPCT,
                MAXLINERETURNAMOUNT,
                MAXTOTALRETURNAMOUNT,
                STOREID,
                VISUALPROFILE,
                LAYOUTID,
                KEYBOARDCODE,
                LAYOUTNAME,
                OPERATORCULTURE,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.PROFILEID,
                ins.DATAAREAID,
                ins.DESCRIPTION,
                ins.MAXLINEDISCOUNTAMOUNT,
                ins.MAXDISCOUNTPCT,
                ins.MAXTOTALDISCOUNTAMOUNT,
                ins.MAXTOTALDISCOUNTPCT,
                ins.MAXLINERETURNAMOUNT,
                ins.MAXTOTALRETURNAMOUNT,
                ins.STOREID,
                ins.VISUALPROFILE,
                ins.LAYOUTID,
                ins.KEYBOARDCODE,
                ins.LAYOUTNAME,
                ins.OPERATORCULTURE,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

--------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_CUSTOMER]'))
begin
   drop trigger dbo.Update_CUSTOMER
end

GO

create trigger Update_CUSTOMER
on CUSTOMER after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.CUSTOMERLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ACCOUNTNUM,
                NAME,
                INVOICEACCOUNT,
                FIRSTNAME,
                MIDDLENAME,
                LASTNAME,
                NAMEPREFIX,
                NAMESUFFIX,
                CURRENCY,
                LANGUAGEID,
                TAXGROUP,
                PRICEGROUP,
                LINEDISC,
                MULTILINEDISC,
                ENDDISC,
                CREDITMAX,
                ORGID,
                BLOCKED,
                NONCHARGABLEACCOUNT,
                INCLTAX,
                PHONE,
                CELLULARPHONE,
                NAMEALIAS,
                CUSTGROUP,
                VATNUM,
                EMAIL,
                URL,
                TAXOFFICE,
                USEPURCHREQUEST,
                LOCALLYSAVED,
                GENDER,
                DATEOFBIRTH,
                RECEIPTOPTION,
                RECEIPTEMAIL,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ACCOUNTNUM,
                ins.NAME,
                ins.INVOICEACCOUNT,
                ins.FIRSTNAME,
                ins.MIDDLENAME,
                ins.LASTNAME,
                ins.NAMEPREFIX,
                ins.NAMESUFFIX,
                ins.CURRENCY,
                ins.LANGUAGEID,
                ins.TAXGROUP,
                ins.PRICEGROUP,
                ins.LINEDISC,
                ins.MULTILINEDISC,
                ins.ENDDISC,
                ins.CREDITMAX,
                ins.ORGID,
                ins.BLOCKED,
                ins.NONCHARGABLEACCOUNT,
                ins.INCLTAX,
                ins.PHONE,
                ins.CELLULARPHONE,
                ins.NAMEALIAS,
                ins.CUSTGROUP,
                ins.VATNUM,
                ins.EMAIL,
                ins.URL,
                ins.TAXOFFICE,
                ins.USEPURCHREQUEST,
                ins.LOCALLYSAVED,
                ins.GENDER,
                ins.DATEOFBIRTH,
                ins.RECEIPTOPTION,
                ins.RECEIPTEMAIL,
                ins.DATAAREAID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.CUSTOMERLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                MASTERID,
                ACCOUNTNUM,
                NAME,
                INVOICEACCOUNT,
                FIRSTNAME,
                MIDDLENAME,
                LASTNAME,
                NAMEPREFIX,
                NAMESUFFIX,
                CURRENCY,
                LANGUAGEID,
                TAXGROUP,
                PRICEGROUP,
                LINEDISC,
                MULTILINEDISC,
                ENDDISC,
                CREDITMAX,
                ORGID,
                BLOCKED,
                NONCHARGABLEACCOUNT,
                INCLTAX,
                PHONE,
                CELLULARPHONE,
                NAMEALIAS,
                CUSTGROUP,
                VATNUM,
                EMAIL,
                URL,
                TAXOFFICE,
                USEPURCHREQUEST,
                LOCALLYSAVED,
                GENDER,
                DATEOFBIRTH,
                RECEIPTOPTION,
                RECEIPTEMAIL,
                DATAAREAID,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.MASTERID,
                ins.ACCOUNTNUM,
                ins.NAME,
                ins.INVOICEACCOUNT,
                ins.FIRSTNAME,
                ins.MIDDLENAME,
                ins.LASTNAME,
                ins.NAMEPREFIX,
                ins.NAMESUFFIX,
                ins.CURRENCY,
                ins.LANGUAGEID,
                ins.TAXGROUP,
                ins.PRICEGROUP,
                ins.LINEDISC,
                ins.MULTILINEDISC,
                ins.ENDDISC,
                ins.CREDITMAX,
                ins.ORGID,
                ins.BLOCKED,
                ins.NONCHARGABLEACCOUNT,
                ins.INCLTAX,
                ins.PHONE,
                ins.CELLULARPHONE,
                ins.NAMEALIAS,
                ins.CUSTGROUP,
                ins.VATNUM,
                ins.EMAIL,
                ins.URL,
                ins.TAXOFFICE,
                ins.USEPURCHREQUEST,
                ins.LOCALLYSAVED,
                ins.GENDER,
                ins.DATEOFBIRTH,
                ins.RECEIPTOPTION,
                ins.RECEIPTEMAIL,
                ins.DATAAREAID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

--------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RETAILITEMDIMENSION]'))
begin
   drop trigger dbo.Update_RETAILITEMDIMENSION
end


GO


create trigger Update_RETAILITEMDIMENSION
on RETAILITEMDIMENSION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RETAILITEMDIMENSIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				ID,
				RETAILITEMID,
				DESCRIPTION,
				SEQUENCE,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.ID,
				ins.RETAILITEMID,
				ins.DESCRIPTION,
				ins.SEQUENCE,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RETAILITEMDIMENSIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				ID,
				RETAILITEMID,
				DESCRIPTION,
				SEQUENCE,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.ID,
				ins.RETAILITEMID,
				ins.DESCRIPTION,
				ins.SEQUENCE,
				ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end



GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RETAILITEMDIMENSIONATTRIBUTE]'))
begin
   drop trigger dbo.Update_RETAILITEMDIMENSIONATTRIBUTE
end

GO

create trigger Update_RETAILITEMDIMENSIONATTRIBUTE
on RETAILITEMDIMENSIONATTRIBUTE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RETAILITEMDIMENSIONATTRIBUTELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				RETAILITEMID,
				DIMENSIONATTRIBUTEID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.RETAILITEMID,
				ins.DIMENSIONATTRIBUTEID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RETAILITEMDIMENSIONATTRIBUTELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				RETAILITEMID,
				DIMENSIONATTRIBUTEID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.RETAILITEMID,
				ins.DIMENSIONATTRIBUTEID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end



GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PERIODICDISCOUNT]'))
begin
   drop trigger dbo.Update_PERIODICDISCOUNT
end

GO

create trigger Update_PERIODICDISCOUNT
on PERIODICDISCOUNT after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.PERIODICDISCOUNTLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				MASTERID,
				OFFERID,
				DESCRIPTION,
				STATUS,
				PDTYPE,
				PRIORITY,
				DISCVALIDPERIODID,
				DISCOUNTTYPE,
				NOOFLINESTOTRIGGER,
				DEALPRICEVALUE,
				DISCOUNTPCTVALUE,
				DISCOUNTAMOUNTVALUE,
				NOOFLEASTEXPITEMS,
				PRICEGROUP,
				ACCOUNTCODE,
				ACCOUNTRELATION,
				TRIGGERED,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.MASTERID,
				ins.OFFERID,
				ins.DESCRIPTION,
				ins.STATUS,
				ins.PDTYPE,
				ins.PRIORITY,
				ins.DISCVALIDPERIODID,
				ins.DISCOUNTTYPE,
				ins.NOOFLINESTOTRIGGER,
				ins.DEALPRICEVALUE,
				ins.DISCOUNTPCTVALUE,
				ins.DISCOUNTAMOUNTVALUE,
				ins.NOOFLEASTEXPITEMS,
				ins.PRICEGROUP,
				ins.ACCOUNTCODE,
				ins.ACCOUNTRELATION,
				ins.TRIGGERED,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.PERIODICDISCOUNTLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				MASTERID,
				OFFERID,
				DESCRIPTION,
				STATUS,
				PDTYPE,
				PRIORITY,
				DISCVALIDPERIODID,
				DISCOUNTTYPE,
				NOOFLINESTOTRIGGER,
				DEALPRICEVALUE,
				DISCOUNTPCTVALUE,
				DISCOUNTAMOUNTVALUE,
				NOOFLEASTEXPITEMS,
				PRICEGROUP,
				ACCOUNTCODE,
				ACCOUNTRELATION,
				TRIGGERED,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.MASTERID,
				ins.OFFERID,
				ins.DESCRIPTION,
				ins.STATUS,
				ins.PDTYPE,
				ins.PRIORITY,
				ins.DISCVALIDPERIODID,
				ins.DISCOUNTTYPE,
				ins.NOOFLINESTOTRIGGER,
				ins.DEALPRICEVALUE,
				ins.DISCOUNTPCTVALUE,
				ins.DISCOUNTAMOUNTVALUE,
				ins.NOOFLEASTEXPITEMS,
				ins.PRICEGROUP,
				ins.ACCOUNTCODE,
				ins.ACCOUNTRELATION,
				ins.TRIGGERED,
				ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_PERIODICDISCOUNTLINE]'))
begin
   drop trigger dbo.Update_PERIODICDISCOUNTLINE
end

GO

create trigger Update_PERIODICDISCOUNTLINE
on PERIODICDISCOUNTLINE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.PERIODICDISCOUNTLINELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				OFFERID,
				OFFERMASTERID,
				LINEID,
				PRODUCTTYPE,
				TARGETID,
				TARGETMASTERID,
				DEALPRICEORDISCPCT,
				LINEGROUP,
				DISCTYPE,
				POSPERIODICDISCOUNTLINEGUID,
				DISCPCT,
				DISCAMOUNT,
				DISCAMOUNTINCLTAX,
				OFFERPRICE,
				OFFERPRICEINCLTAX,
				UNIT,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.OFFERID,
				ins.OFFERMASTERID,
				ins.LINEID,
				ins.PRODUCTTYPE,
				ins.TARGETID,
				ins.TARGETMASTERID,
				ins.DEALPRICEORDISCPCT,
				ins.LINEGROUP,
				ins.DISCTYPE,
				ins.POSPERIODICDISCOUNTLINEGUID,
				ins.DISCPCT,
				ins.DISCAMOUNT,
				ins.DISCAMOUNTINCLTAX,
				ins.OFFERPRICE,
				ins.OFFERPRICEINCLTAX,
				ins.UNIT,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.PERIODICDISCOUNTLINELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				OFFERID,
				OFFERMASTERID,
				LINEID,
				PRODUCTTYPE,
				TARGETID,
				TARGETMASTERID,
				DEALPRICEORDISCPCT,
				LINEGROUP,
				DISCTYPE,
				POSPERIODICDISCOUNTLINEGUID,
				DISCPCT,
				DISCAMOUNT,
				DISCAMOUNTINCLTAX,
				OFFERPRICE,
				OFFERPRICEINCLTAX,
				UNIT,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.OFFERID,
				ins.OFFERMASTERID,
				ins.LINEID,
				ins.PRODUCTTYPE,
				ins.TARGETID,
				ins.TARGETMASTERID,
				ins.DEALPRICEORDISCPCT,
				ins.LINEGROUP,
				ins.DISCTYPE,
				ins.POSPERIODICDISCOUNTLINEGUID,
				ins.DISCPCT,
				ins.DISCAMOUNT,
				ins.DISCAMOUNTINCLTAX,
				ins.OFFERPRICE,
				ins.OFFERPRICEINCLTAX,
				ins.UNIT,
                ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end


GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RETAILDIVISION]'))
begin
   drop trigger dbo.Update_RETAILDIVISION
end

GO

create trigger Update_RETAILDIVISION
on RETAILDIVISION after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RETAILDIVISIONLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				DIVISIONID,
				NAME,
				MASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.DIVISIONID,
				ins.NAME,
				ins.MASTERID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RETAILDIVISIONLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				DIVISIONID,
				NAME,
				MASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.DIVISIONID,
				ins.NAME,
				ins.MASTERID,
				ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RETAILDEPARTMENT]'))
begin
   drop trigger dbo.Update_RETAILDEPARTMENT
end

GO

create trigger Update_RETAILDEPARTMENT
on RETAILDEPARTMENT after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RETAILDEPARTMENTLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				DEPARTMENTID,
				NAME,
				NAMEALIAS,
				DIVISIONID,
				DIVISIONMASTERID,
				MASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.DEPARTMENTID,
				ins.NAME,
				ins.NAMEALIAS,
				ins.DIVISIONID,
				ins.DIVISIONMASTERID,
				ins.MASTERID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RETAILDEPARTMENTLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				DEPARTMENTID,
				NAME,
				NAMEALIAS,
				DIVISIONID,
				DIVISIONMASTERID,
				MASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.DEPARTMENTID,
				ins.NAME,
				ins.NAMEALIAS,
				ins.DIVISIONID,
				ins.DIVISIONMASTERID,
				ins.MASTERID,
				ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_RETAILGROUP]'))
begin
   drop trigger dbo.Update_RETAILGROUP
end

GO

create trigger Update_RETAILGROUP
on RETAILGROUP after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.RETAILGROUPLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				MASTERID,
				GROUPID,
				NAME,
				DEPARTMENTID,
				SALESTAXITEMGROUP,
				DEFAULTPROFIT,
				POSPERIODICID,
				DIVISIONMASTERID,
				DEPARTMENTMASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.MASTERID,
				ins.GROUPID,
				ins.NAME,
				ins.DEPARTMENTID,
				ins.SALESTAXITEMGROUP,
				ins.DEFAULTPROFIT,
				ins.POSPERIODICID,
				ins.DIVISIONMASTERID,
				ins.DEPARTMENTMASTERID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.RETAILGROUPLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				MASTERID,
				GROUPID,
				NAME,
				DEPARTMENTID,
				SALESTAXITEMGROUP,
				DEFAULTPROFIT,
				POSPERIODICID,
				DIVISIONMASTERID,
				DEPARTMENTMASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.MASTERID,
				ins.GROUPID,
				ins.NAME,
				ins.DEPARTMENTID,
				ins.SALESTAXITEMGROUP,
				ins.DEFAULTPROFIT,
				ins.POSPERIODICID,
				ins.DIVISIONMASTERID,
				ins.DEPARTMENTMASTERID,
				ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_SPECIALGROUP]'))
begin
   drop trigger dbo.Update_SPECIALGROUP
end

GO

create trigger Update_SPECIALGROUP
on SPECIALGROUP after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.SPECIALGROUPLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				GROUPID,
				NAME,
				MASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.MASTERID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.SPECIALGROUPLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				GROUPID,
				NAME,
				MASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.GROUPID,
				ins.NAME,
				ins.MASTERID,
				ins.DELETED
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_SPECIALGROUPITEMS]'))
begin
   drop trigger dbo.Update_SPECIALGROUPITEMS
end

GO

create trigger Update_SPECIALGROUPITEMS
on SPECIALGROUPITEMS after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.SPECIALGROUPITEMSLog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
				GROUPID,
				ITEMID,
				MEMBERMASTERID,
				GROUPMASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.GROUPID,
				ins.ITEMID,
				ins.MEMBERMASTERID,
				ins.GROUPMASTERID,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.SPECIALGROUPITEMSLog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
				GROUPID,
				ITEMID,
				MEMBERMASTERID,
				GROUPMASTERID,
				DELETED
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
				ins.GROUPID,
				ins.ITEMID,
				ins.MEMBERMASTERID,
				ins.GROUPMASTERID,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO
-- ---------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Update_INVENTORYTEMPLATE]'))
begin
   drop trigger dbo.Update_INVENTORYTEMPLATE
end

GO

create trigger Update_INVENTORYTEMPLATE
on INVENTORYTEMPLATE after insert,update, delete as

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
            insert into LSPOSNET_Audit.dbo.INVENTORYTEMPLATELog (
                AuditUserGUID,
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                CHANGEVENDORINLINE,
                CHANGEUOMINLINE,
                CALCULATESUGGESTEDQUANTITY,
                SETQUANTITYTOSUGGESTEDQUANTITY,
                DISPLAYREORDERPOINT,
                DISPLAYMAXIMUMINVENTORY,
                DISPLAYBARCODE,
                ALLSTORES,
                DATAAREAID,
                ADDLINESWITHZEROSUGGESTEDQTY,
                TEMPLATEENTRYTYPE,
                UNITSELECTION,
                ENTERINGTYPE,
                QUANTITYMETHOD,
                DEFAULTQUANTITY,
                AREAID,
                DEFAULTSTORE,
                AUTOPOPULATEITEMS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.CHANGEVENDORINLINE,
                ins.CHANGEUOMINLINE,
                ins.CALCULATESUGGESTEDQUANTITY,
                ins.SETQUANTITYTOSUGGESTEDQUANTITY,
                ins.DISPLAYREORDERPOINT,
                ins.DISPLAYMAXIMUMINVENTORY,
                ins.DISPLAYBARCODE,
                ins.ALLSTORES,
                ins.DATAAREAID,
                ins.ADDLINESWITHZEROSUGGESTEDQTY,
                ins.TEMPLATEENTRYTYPE,
                ins.UNITSELECTION,
                ins.ENTERINGTYPE,
                ins.QUANTITYMETHOD,
                ins.DEFAULTQUANTITY,
                ins.AREAID,
                ins.DEFAULTSTORE,
                ins.AUTOPOPULATEITEMS,
                1 as Deleted
                From DELETED ins
        end
        else
        begin
            -- If we got here then we are inserting new or deleting existing
            insert into LSPOSNET_Audit.dbo.INVENTORYTEMPLATELog (
                AuditUserGUID, 
                AuditUserLogin,
                AuditDate,
                ID,
                NAME,
                CHANGEVENDORINLINE,
                CHANGEUOMINLINE,
                CALCULATESUGGESTEDQUANTITY,
                SETQUANTITYTOSUGGESTEDQUANTITY,
                DISPLAYREORDERPOINT,
                DISPLAYMAXIMUMINVENTORY,
                DISPLAYBARCODE,
                ALLSTORES,
                DATAAREAID,
                ADDLINESWITHZEROSUGGESTEDQTY,
                TEMPLATEENTRYTYPE,
                UNITSELECTION,
                ENTERINGTYPE,
                QUANTITYMETHOD,
                DEFAULTQUANTITY,
                AREAID,
                DEFAULTSTORE,
                AUTOPOPULATEITEMS,
                Deleted
                )
            Select 
                @connectionUser, @sessionUser as AuditUserLogin, 
                GETDATE() as AuditDate,
                ins.ID,
                ins.NAME,
                ins.CHANGEVENDORINLINE,
                ins.CHANGEUOMINLINE,
                ins.CALCULATESUGGESTEDQUANTITY,
                ins.SETQUANTITYTOSUGGESTEDQUANTITY,
                ins.DISPLAYREORDERPOINT,
                ins.DISPLAYMAXIMUMINVENTORY,
                ins.DISPLAYBARCODE,
                ins.ALLSTORES,
                ins.DATAAREAID,
                ins.ADDLINESWITHZEROSUGGESTEDQTY,
                ins.TEMPLATEENTRYTYPE,
                ins.UNITSELECTION,
                ins.ENTERINGTYPE,
                ins.QUANTITYMETHOD,
                ins.DEFAULTQUANTITY,
                ins.AREAID,
                ins.DEFAULTSTORE,
                ins.AUTOPOPULATEITEMS,
                0 as Deleted
                From inserted ins
        end
    end try
    begin catch

    end catch

end

GO




---END OF AUDIT TRIGGERS 
-- ---------------------------------------------------------------------------------------------------



-- ---------------------------------------------------------------------------------------------------
-- -----------------------------------
----------------------------------------------------------------
Use LSPOSNET_Audit

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UserPermissions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_UserPermissions]

GO

create procedure dbo.spAUDIT_ViewLog_UserPermissions
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

set nocount on

--AuditUserLogin = Case 
--           when pl.AuditUserLogin = '' then u.Login
--           else pl.AuditUserLogin
--       end,

select pl.AuditID, 
       pl.AuditUserGUID, 
       u.Login as AuditUserLogin,
       pl.AuditUserLogin as AuditUserLogin2,
       pl.AuditDate,
       p.Description, 
       pl.AuditFunction as [Action] 
from LSPOSNET.dbo.PERMISSIONS p inner 
join dbo.USERPERMISSIONLog pl on p.GUID = pl.PermissionGUID
left outer join LSPOSNET.dbo.USERS u on u.GUID = pl.AuditUserGUID
where pl.DATAAREAID = @dataAreaID and pl.UserGuid = @contextIdentifier and (pl.AuditUserLogin Like @user or u.Login Like @user) and pl.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_User]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_User]

GO

create procedure dbo.spAUDIT_ViewLog_User
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

--AuditUserLogin = Case 
--           when u.AuditUserLogin = '' then m.Login
--           else u.AuditUserLogin
--       end,

select u.AuditID, 
       u.AuditUserGUID,
       m.Login as AuditUserLogin,
       u.AuditUserLogin as AuditUserLogin2,
       u.AuditDate, 
       u.AuditSpecialOperation,
       u.Login as AuditUserLogin,
       u.FirstName, 
       u.MiddleName, 
       u.LastName, 
       u.NamePrefix, 
       u.NameSuffix, 
       u.IsDomainUser,
       u.STAFFID, 
       u.Enabled, 
       u.Deleted
from   dbo.USERSLOG u
       left outer join LSPOSNET.dbo.USERS m on m.GUID = u.AuditUserGUID
where  u.DATAAREAID = @dataAreaID and u.GUID = @contextIdentifier and (u.AuditUserLogin Like @user or m.Login Like @user) and u.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Users]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_Users]

GO

create procedure dbo.spAUDIT_ViewLog_Users
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

--AuditUserLogin = Case 
--           when u.AuditUserLogin = '' then m.Login
--           else u.AuditUserLogin
--       end,

select u.AuditID, 
       u.AuditUserGUID,
       m.Login as AuditUserLogin,
       u.AuditUserLogin as AuditUserLogin2,
       u.AuditDate, 
       u.AuditSpecialOperation,
       u.Login as AuditUserLogin,
       u.FirstName, 
       u.MiddleName, 
       u.LastName, 
       u.NamePrefix, 
       u.NameSuffix, 
       u.IsDomainUser,
       u.STAFFID, 
       u.Enabled, 
       u.Deleted
from   dbo.USERSLOG u
       left outer join LSPOSNET.dbo.USERS m on m.GUID = u.AuditUserGUID
where  u.DATAAREAID = @dataAreaID and (u.AuditUserLogin Like @user or m.Login Like @user) and u.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UsersInGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_UsersInGroup]

GO

create procedure dbo.spAUDIT_ViewLog_UsersInGroup
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

--AuditUserLogin = Case 
--           when ug.AuditUserLogin = '' then m.Login
--           else ug.AuditUserLogin
--       end,

select ug.AuditID, 
       ug.AuditUserGUID, 
       m.Login as AuditUserLogin,
       ug.AuditUserLogin as AuditUserLogin2,
       ug.AuditDate, 
       g.Name, 
       ug.AuditFunction as [Action]
from   dbo.USERSINGROUPLog ug
       inner join LSPOSNET.dbo.USERGROUPS g on ug.UserGroupGUID = g.GUID and ug.DATAAREAID COLLATE DATABASE_DEFAULT = g.DATAAREAID
       left outer join LSPOSNET.dbo.USERS m on m.GUID = ug.AuditUserGUID
       
where  ug.DATAAREAID = @dataAreaID and ug.UserGuid = @contextIdentifier and (ug.AuditUserLogin Like @user or m.Login Like @user) and ug.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_GroupPermissions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_GroupPermissions]

GO

--AuditUserLogin = Case 
--           when ugp.AuditUserLogin = '' then m.Login
--           else ugp.AuditUserLogin
--       end,  

create procedure dbo.spAUDIT_ViewLog_GroupPermissions
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as
select ugp.AuditID,
       ugp.AuditUserGUID,
       m.Login as AuditUserLogin,
       ugp.AuditUserLogin as AuditUserLogin2,
       ugp.AuditDate, 
       ug.Name, 
       p.Description, 
       ugp.AuditFunction as [Action]
from   dbo.USERGROUPPERMISSIONLog ugp 
       inner join LSPOSNET.dbo.USERGROUPS ug ON ugp.UserGroupGUID = ug.GUID and ugp.DATAAREAID COLLATE DATABASE_DEFAULT = ug.DATAAREAID
       inner join LSPOSNET.dbo.PERMISSIONS p on p.GUID = ugp.PermissionGUID
       left outer join LSPOSNET.dbo.USERS m on m.GUID = ugp.AuditUserGUID
where  ugp.DATAAREAID = @dataAreaID and (ugp.AuditUserLogin Like @user or m.Login Like @user) and ugp.AuditDate Between @from and @to   
GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UserGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_UserGroups]

GO



create procedure dbo.spAUDIT_ViewLog_UserGroups
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when g.AuditUserLogin = '' then m.Login
 --          else g.AuditUserLogin
 --      end,  

select g.AuditID, 
	   g.AuditUserGUID, 
	   m.Login as AuditUserLogin,
	   g.AuditUserLogin as AuditUserLogin2,
	   g.AuditDate, 
	   g.Name, 
	   g.Deleted
from dbo.USERGROUPLog g
left outer join LSPOSNET.dbo.USERS m on m.GUID = g.AuditUserGUID
where g.DATAAREAID = @dataAreaID and (g.AuditUserLogin Like @user or m.Login Like @user) and g.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SystemSettings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_SystemSettings]

GO

create procedure dbo.spAUDIT_ViewLog_SystemSettings
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

-- AuditUserLogin = Case 
--           when s.AuditUserLogin = '' then m.Login
--           else s.AuditUserLogin
--       end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate, 
       setting.Name,
       s.Value
from  dbo.SYSTEMSETTINGSLog s
left outer join LSPOSNET.dbo.SYSTEMSETTINGS setting on s.SettingGUID = setting.GUID
left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where setting.DATAAREAID = @dataAreaID and setting.Type = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UserLogins]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_UserLogins]

GO



create procedure dbo.spAUDIT_ViewLog_UserLogins
(@dataAreaID nvarchar(10),@contextIdentifier uniqueidentifier,@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.Login as AuditUserLogin2,
       s.AuditDate,
       s.Login,
       s.Auditfunction as [Action]
from  dbo.USERLOGINLog s
left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where s.DATAAREAID = @dataAreaID and (s.Login Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_VisualProfile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_VisualProfile]

GO

create procedure dbo.spAUDIT_ViewLog_VisualProfile
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.RESOLUTION,
       s.TERMINALTYPE,
       s.HIDECURSOR,
       s.DESIGNALLOWEDONPOS,
       s.OPAQUEBACKGROUNDFORM,
       s.OPACITY,
       s.USEFORMBACKGROUNDIMAGE,
	   s.SCREENINDEX,
	   s.RECEIPTPAYMENTLINESSIZE,
	   s.CONFIRMBUTTONSTYLEID,
	   s.CANCELBUTTONSTYLEID,
	   s.ACTIONBUTTONSTYLEID,
	   s.NORMALBUTTONSTYLEID,
	   s.OTHERBUTTONSTYLEID,
	   s.OVERRIDEPOSCONTROLBORDERCOLOR,
	   s.POSCONTROLBORDERCOLOR
from   dbo.POSVISUALPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.PROFILEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_VisualProfiles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_VisualProfiles]

GO

create procedure dbo.spAUDIT_ViewLog_VisualProfiles
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.RESOLUTION,
       s.TERMINALTYPE,
       s.HIDECURSOR,
       s.DESIGNALLOWEDONPOS,
       s.OPAQUEBACKGROUNDFORM,
       s.OPACITY,
       s.USEFORMBACKGROUNDIMAGE,
	   s.SCREENINDEX,
	   s.RECEIPTPAYMENTLINESSIZE,
	   s.CONFIRMBUTTONSTYLEID,
	   s.CANCELBUTTONSTYLEID,
	   s.ACTIONBUTTONSTYLEID,
	   s.NORMALBUTTONSTYLEID,
	   s.OTHERBUTTONSTYLEID,
	   s.OVERRIDEPOSCONTROLBORDERCOLOR,
	   s.POSCONTROLBORDERCOLOR
from   dbo.POSVISUALPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Terminal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_Terminal]

GO

create procedure dbo.spAUDIT_ViewLog_Terminal
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TerminalID,
       s.NAME,
       s.STOREID,
       s.AutoLogOffTimeout,
       s.HardwareProfile,
	   s.FunctionalityProfile,
       s.VisualProfile,
       s.CustomerDisplayText1,
       s.CustomerDisplayText2,
       s.OpenDrawerAtLilo,
       s.LayoutID,
       s.StandAlone,
       s.ExitAfterEachTransaction,
	   s.UpdateServicePort,
	   s.TRANSACTIONSERVICEPROFILE,
	   s.transactionIDNumberSequence,
	   s.IPAddress,
	   s.EFTTERMINALID,
	   s.EFTSTOREID,
	   s.SALESTYPEFILTER,
	   s.SUSPENDALLOWEOD,
	   s.LSPAYUSELOCALSERVER,
	   s.LSPAYSERVERNAME,
	   s.LSPAYSERVERPORT,
	   s.LSPAYPLUGINID,
	   s.LSPAYPLUGINNAME,
	   s.LSPAYSUPPORTREFREFUND
from   dbo.RBOTERMINALTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.TERMINALID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Terminals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spAUDIT_ViewLog_Terminals]

GO

create procedure dbo.spAUDIT_ViewLog_Terminals
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TerminalID,
       s.NAME,
       s.STOREID,
       s.AutoLogOffTimeout,
       s.HardwareProfile,
	   s.FunctionalityProfile,
       s.VisualProfile,
       s.CustomerDisplayText1,
       s.CustomerDisplayText2,
       s.OpenDrawerAtLilo,
       s.LayoutID,
       s.StandAlone,
       s.ExitAfterEachTransaction,
	   s.UpdateServicePort,
	   s.TRANSACTIONSERVICEPROFILE,
	   s.transactionIDNumberSequence,
	   s.IPAddress,
	   s.EFTTERMINALID,
	   s.EFTSTOREID,
	   s.SALESTYPEFILTER,
	   s.SUSPENDALLOWEOD,
	   s.LSPAYUSELOCALSERVER,
	   s.LSPAYSERVERNAME,
	   s.LSPAYSERVERPORT,
	   s.LSPAYPLUGINID,
	   s.LSPAYPLUGINNAME,
	   s.LSPAYSUPPORTREFREFUND
from   dbo.RBOTERMINALTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Store]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Store

GO

create procedure dbo.spAUDIT_ViewLog_Store
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.StoreID,
       s.NAME,
       s.ADDRESS,
       s.STREET,
       s.CITY,
       s.ZIPCODE,
       s.STATE,
       s.COUNTRY,
       s.FUNCTIONALITYPROFILE,
	   s.LAYOUTID,
       s.CURRENCY,
       s.CULTURENAME,
       s.TAXGROUP,
       s.SqlServerName,
	   s.DatabaseName,
	   s.WindowsAuthentication,
	   s.UserName,
	   s.Password,
	   s.DefaultCustAccount,
	   s.UseDefaultCustAccount,
	   s.HIDETRAININGMODE,
	   s.STATEMENTMETHOD,
	   s.TENDERDECLARATIONCALCULATION,
	   s.MAXIMUMPOSTINGDIFFERENCE,
	   s.MAXTRANSACTIONDIFFERENCEAMOUNT,
	   s.USETAXGROUPFROM,
	   s.SUSPENDALLOWEOD,
	   s.KEYBOARDCODE,
	   s.LAYOUTNAME
from   dbo.RBOSTORETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STOREID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Stores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Stores

GO

create procedure dbo.spAUDIT_ViewLog_Stores
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.StoreID,
       s.NAME,
       s.ADDRESS,
       s.STREET,
       s.CITY,
       s.ZIPCODE,
       s.STATE,
       s.COUNTRY,
       s.FUNCTIONALITYPROFILE,
	   s.LAYOUTID,
       s.CURRENCY,
       s.CULTURENAME,
       s.TAXGROUP,
       s.SqlServerName,
	   s.DatabaseName,
	   s.WindowsAuthentication,
	   s.UserName,
	   s.Password,
	   s.DefaultCustAccount,
	   s.UseDefaultCustAccount,
	   s.HIDETRAININGMODE,
	   s.USETAXGROUPFROM,
	   s.SUSPENDALLOWEOD,
	   s.KEYBOARDCODE,
	   s.LAYOUTNAME
from   dbo.RBOSTORETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StorePaymentType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StorePaymentType

GO

create procedure dbo.spAUDIT_ViewLog_StorePaymentType
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.StoreID,
       s.Name,
       s.ChangeTenderID,
       s.AboveMinimumTenderID,
       s.MinimumChangeAmount,
       s.POSOperation,
       s.Rounding,
       s.RoundingMethod,
       s.OPENDRAWER,
       s.ALLOWOVERTENDER,
       s.ALLOWUNDERTENDER,
	   s.MAXIMUMOVERTENDERAMOUNT,
	   s.UNDERTENDERAMOUNT,
	   s.MinimumAmountAllowed,
	   s.MinimumAmountEntered,
	   s.MaximumAmountAllowed,
	   s.MaximumAmountEntered
from   dbo.RBOSTORETENDERTYPETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STOREID = @contextIdentifier and s.TENDERTYPEID = @contextIdentifier2 and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StorePaymentTypeCards]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StorePaymentTypeCards

GO

create procedure dbo.spAUDIT_ViewLog_StorePaymentTypeCards
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.Action,
       s.StoreID,
       s.CARDTYPEID,
       s.Name,
       s.CHECKMODULUS,
       s.CHECKEXPIREDDATE,
       s.PROCESSLOCALLY,
       s.ALLOWMANUALINPUT
from   dbo.RBOSTORETENDERTYPECARDTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STOREID = @contextIdentifier and s.TENDERTYPEID = @contextIdentifier2 and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CardTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CardTypes

GO

create procedure dbo.spAUDIT_ViewLog_CardTypes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.CARDTYPEID,
       s.Name,
       s.CARDTYPES,
       s.CARDISSUER,
       s.Deleted
from   dbo.RBOTENDERTYPECARDTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.CARDTYPEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_AllCardTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_AllCardTypes

GO

create procedure dbo.spAUDIT_ViewLog_AllCardTypes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.CARDTYPEID,
       s.Name,
       s.CARDTYPES,
       s.CARDISSUER,
       s.Deleted
from   dbo.RBOTENDERTYPECARDTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CardTypeNumbers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CardTypeNumbers

GO

create procedure dbo.spAUDIT_ViewLog_CardTypeNumbers
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.Action,
       s.CARDTYPEID,
       s.CARDNUMBERFROM,
       s.CARDNUMBERTO,
       s.CARDNUMBERLENGTH
from   dbo.RBOTENDERTYPECARDNUMBERSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.CARDTYPEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PaymentMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PaymentMethod

GO

create procedure dbo.spAUDIT_ViewLog_PaymentMethod
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TENDERTYPEID,
       s.Name,
       s.DEFAULTFUNCTION,
       s.Deleted
from   dbo.RBOTENDERTYPETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.TENDERTYPEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PaymentMethods]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PaymentMethods

GO

create procedure dbo.spAUDIT_ViewLog_PaymentMethods
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TENDERTYPEID,
       s.Name,
       s.DEFAULTFUNCTION,
       s.Deleted
from   dbo.RBOTENDERTYPETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PaymentMethodLimitations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PaymentMethodLimitations

GO

create procedure dbo.spAUDIT_ViewLog_PaymentMethodLimitations
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.Action,
	   s.MASTERID,
       s.RESTRICTIONCODE,
       s.RELATIONMASTERID,
       s.TYPE,
       s.INCLUDE,
	   s.TAXEXEMPT
from   dbo.PAYMENTLIMITATIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.TENDERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_POSUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_POSUsers

GO

create procedure dbo.spAUDIT_ViewLog_POSUsers
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.STAFFID,
       s.NAME,
       s.STOREID,
       s.PASSWORD,
       s.CHANGEPASSWORD,
       s.MaxDiscountPct as MAXLINEDISCOUNTPCT,
       s.MaxTotalDiscountPct,
	   s.MAXLINEDISCOUNTAMOUNT,
	   s.MAXTOTALDISCOUNTAMOUNT,
	   s.MAXLINERETURNAMOUNT,
	   s.MAXTOTALRETURNAMOUNT,
       s.LayoutID,
       s.VISUALPROFILE,
       s.MANAGERPRIVILEGES,
       s.ALLOWTRANSACTIONVOIDING,
       s.ALLOWXREPORTPRINTING,
       s.ALLOWTENDERDECLARATION,
       s.ALLOWFLOATINGDECLARATION,
       s.ALLOWCHANGENOVOID,
       s.ALLOWTRANSACTIONSUSPENSION,
       s.ALLOWOPENDRAWERONLY,
	   s.PRICEOVERRIDE AS ALLOWPRICEOVERRIDE,
	   s.NAMEONRECEIPT,
	   s.CONTINUEONTSERRORS,
       s.OPERATORCULTURE,
	   s.KEYBOARDCODE,
	   s.LAYOUTNAME,
       s.Deleted
from   dbo.RBOSTAFFTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STAFFID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_OPERATIONS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_OPERATIONS

GO

create procedure dbo.spAUDIT_ViewLog_OPERATIONS
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.MASTERID,
        s.ID,
        s.DESCRIPTION,
        s.TYPE,
        s.LOOKUPTYPE,
        s.AUDIT,
        s.Deleted
from   dbo.OPERATIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TouchButtonLayout]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TouchButtonLayout

GO

create procedure dbo.spAUDIT_ViewLog_TouchButtonLayout
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.LAYOUTID,
       s.NAME,
       s.WIDTH,
       s.HEIGHT,
       s.BUTTONGRID1,
       s.BUTTONGRID2,
       s.BUTTONGRID3,
       s.BUTTONGRID4,
       s.BUTTONGRID5,
       s.RECEIPTID,
       s.TOTALID,
       s.CUSTOMERLAYOUTID,
       s.LOGOPICTUREID,
       s.Deleted
from   dbo.POSISTILLLAYOUTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.LAYOUTID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_AllTouchButtonLayouts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_AllTouchButtonLayouts

GO

create procedure dbo.spAUDIT_ViewLog_AllTouchButtonLayouts
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.LAYOUTID,
       s.NAME,
       s.WIDTH,
       s.HEIGHT,
       s.BUTTONGRID1,
       s.BUTTONGRID2,
       s.BUTTONGRID3,
       s.BUTTONGRID4,
       s.BUTTONGRID5,
       s.RECEIPTID,
       s.TOTALID,
       s.CUSTOMERLAYOUTID,
       s.LOGOPICTUREID,
       s.Deleted
from   dbo.POSISTILLLAYOUTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Customer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Customer

GO

create procedure dbo.spAUDIT_ViewLog_Customer
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ACCOUNTNUM,
       s.NAME,
       s.INVOICEACCOUNT,
       s.ORGID,
       s.CURRENCY,
       s.CREDITMAX,
       s.LANGUAGEID,
       s.TAXGROUP,
       s.PRICEGROUP,
       s.LINEDISC,
       s.MULTILINEDISC,
       s.ENDDISC,
       s.BLOCKED,
       s.NONCHARGABLEACCOUNT,
	   s.FIRSTNAME,
	   s.MIDDLENAME,
	   s.LASTNAME,
	   s.NAMEPREFIX,
	   s.NAMESUFFIX,
	   s.RECEIPTOPTION,
       s.RECEIPTEMAIL,
       s.Deleted
from   dbo.CUSTTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ACCOUNTNUM = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CustomerAddress]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CustomerAddress

GO

create procedure dbo.spAUDIT_ViewLog_CustomerAddress
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ACCOUNTNUM,
       s.STREET,
       s.ADDRESS,
       s.ZIPCODE,
       s.STATE,
       s.CITY,
       s.COUNTY,
       s.COUNTRY,
       s.TAXGROUP,
       s.ADDRESSTYPE,
       s.ADDRESSFORMAT,
       s.PHONE,
       s.CELLULARPHONE,
       s.EMAIL,
       s.Deleted
from   dbo.CUSTOMERADDRESSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ACCOUNTNUM = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_FunctionalityProfile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_FunctionalityProfile

GO

create procedure dbo.spAUDIT_ViewLog_FunctionalityProfile
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.LOGLEVEL,
       s.AGGREGATEITEMS,
       s.AGGREGATEPAYMENTS,
       s.AGGREGATEITEMSFORPRINTING,
       s.ALLOWSALESIFDRAWERISOPEN,
       s.SHOWSTAFFLISTATLOGON,
       s.LIMITSTAFFLISTTOSTORE,
	   s.STAFFBARCODELOGON,
	   s.STAFFCARDLOGON,
	   s.MUSTKEYINPRICEIFZERO,
	   s.MINIMUMPASSWORDLENGTH,
	   s.ISHOSPITALITYPROFILE,
	   s.ALWAYSASKFORPASSWORD,	   
       s.NUMPADENTRYSTARTSINDECIMALS,
       s.NUMPADAMOUNTOFDECIMALS,
       s.SAFEDROPUSESDENOMINATION,
       s.SAFEDROPREVUSESDENOMINATION,
       s.BANKDROPUSESDENOMINATION,
       s.BANKDROPREVUSESDENOMINATION,
       s.TENDERDECLUSESDENOMINATION,
       s.POLLINGINTERVAL,	   
       s.MAXIMUMPRICE,
       s.MAXIMUMQTY,
	   s.OMNISUSPENSIONTYPE,
	   s.OMNIITEMIMAGELOOKUPGROUP,
	   s.OPENDRAWER,
	   s.ZRPTPRINTGRANDTOTALS,
	   s.LIMITATIONDISPLAYTYPE,
	   s.DISPLAYLIMITATIONSTOTALSINPOS,
       s.Deleted
from   dbo.POSFUNCTIONALITYPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.PROFILEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_FunctionalityProfiles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_FunctionalityProfiles

GO

create procedure dbo.spAUDIT_ViewLog_FunctionalityProfiles
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.LOGLEVEL,
       s.AGGREGATEITEMS,
       s.AGGREGATEPAYMENTS,
       s.AGGREGATEITEMSFORPRINTING,
       s.ALLOWSALESIFDRAWERISOPEN,
       s.SHOWSTAFFLISTATLOGON,
       s.LIMITSTAFFLISTTOSTORE,
	   s.LIMITATIONDISPLAYTYPE,
	   s.DISPLAYLIMITATIONSTOTALSINPOS,
       s.Deleted
from   dbo.POSFUNCTIONALITYPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TransactionServiceProfile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TransactionServiceProfile

GO

create procedure dbo.spAUDIT_ViewLog_TransactionServiceProfile
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.USERNAME,
       s.PASSWORD,
       s.COMPANY,
       s.DOMAIN,
       s.AOSSERVER,
       s.AOSINSTANCE,
       s.AOSPORT,
       s.CONFIGURATION,
       s.TSCUSTOMER,
       s.TSSTAFF,
       s.TSINVENTORYLOOKUP,
       s.LANGUAGE,
       s.AXVERSION,
       s.CENTRALTABLESERVER,
       s.CENTRALTABLESERVERPORT,
	   s.ISSUEGIFTCARDOPTION,
       s.USEGIFTCARDS,
	   s.USECENTRALSUSPENSION,
       s.Deleted
from   dbo.POSTRANSACTIONSERVICEPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.PROFILEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TransactionServiceProfiles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TransactionServiceProfiles

GO

create procedure dbo.spAUDIT_ViewLog_TransactionServiceProfiles
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.USERNAME,
       s.PASSWORD,
       s.COMPANY,
       s.DOMAIN,
       s.AOSSERVER,
       s.AOSINSTANCE,
       s.AOSPORT,
       s.CONFIGURATION,
       s.TSCUSTOMER,
       s.TSSTAFF,
       s.TSINVENTORYLOOKUP,
       s.LANGUAGE,
       s.AXVERSION,
       s.CENTRALTABLESERVER,
       s.CENTRALTABLESERVERPORT,
	   s.ISSUEGIFTCARDOPTION,
       s.USEGIFTCARDS,
	   s.USECENTRALSUSPENSION,
       s.Deleted
from   dbo.POSTRANSACTIONSERVICEPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_HardwareProfile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_HardwareProfile

GO

create procedure dbo.spAUDIT_ViewLog_HardwareProfile
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.DRAWER,
       s.DRAWERDEVICENAME,
       s.DRAWERDESCRIPTION,
       s.DRAWEROPENTEXT,
       s.DISPLAYDEVICE,
       s.DISPLAYDEVICENAME,
       s.DISPLAYDESCRIPTION,
	   s.DISPLAYTOTALTEXT,
	   s.DISPLAYBALANCETEXT,
       s.DISPLAYCLOSEDLINE1,
       s.DISPLAYCLOSEDLINE2,
       s.DISPLAYCHARACTERSET,
       s.MSR,
       s.MSRDEVICENAME,
       s.MSRDESCRIPTION,
       s.STARTTRACK1,
       s.SEPARATOR1,
       s.ENDTRACK1,
       s.PRINTER,
       s.PRINTERDEVICENAME,
       s.PRINTERDESCRIPTION,
	   s.PRINTBINARYCONVERSION,
       s.PRINTERCHARACTERSET,
       s.SCANNER,
       s.SCANNERDEVICENAME,
       s.SCANNERDESCRIPTION,
       s.SCALE,
       s.SCALEDEVICENAME,
       s.SCALEDESCRIPTION,
       s.MANUALINPUTALLOWED,
       s.KEYLOCK,
       s.KEYLOCKDEVICENAME,
       s.KEYLOCKDESCRIPTION,
       s.EFT,
       s.EFTSERVERNAME,
       s.EFTDESCRIPTION,
       s.EFTSERVERPORT,
       s.EFTCOMPANYID,
       s.EFTUSERID,
       s.EFTPASSWORD,
       s.EftBatchIncrementAtEOS,
       s.CCTV,
       s.CCTVCAMERA,
       s.CCTVHOSTNAME,
       s.CCTVPORT,
       s.FORECOURT,
       s.HOSTNAME,
       s.RFIDSCANNERTYPE,
       s.RFIDDEVICENAME,
       s.RFIDDESCRIPTION,
       s.CASHCHANGER,
       s.CASHCHANGERPORTSETTINGS,
       s.CASHCHANGERINITSETTINGS,
       s.DUALDISPLAY,
       s.DUALDISPLAYDEVICENAME,
       s.DUALDISPLAYDESCRIPTION,
       s.DUALDISPLAYTYPE,
       s.DUALDISPLAYPORT,
       s.DUALDISPLAYRECEIPTPERCENTAGE,
       s.DUALDISPLAYIMAGEPATH,
       s.DUALDISPLAYIMAGEINTERVAL,
       s.DUALDISPLAYBROWSERURL,
	   s.FORECOURTMANAGER as FORECOURTMANAGERActive,
	   s.FORECOURTMANAGERHOSTNAME as FORECOURTMANAGERServer,
	   s.FORECOURTMANAGERPORT,
	   s.FORECOURTMANAGERPOSPORT,
	   s.FORECOURTMANAGERScreenHeightPercentage,
	   s.FORECOURTMANAGERControllerHostName,
	   s.FORECOURTMANAGERLogLevel,
	   s.FORECOURTMANAGERImplFileName,
	   s.FORECOURTMANAGERImplFileType,
	   s.FORECOURTMANAGERScreenExtHeightPercentage,
	   s.FORECOURTMANAGERVolumeUnit,
	   s.FORECOURTMANAGERCallingSound,
	   s.FORECOURTMANAGERCallingBlink,
	   s.FORECOURTMANAGERFuellingPointColumns,
	   s.FISCALPRINTER,
	   s.FISCALPRINTERCONNECTION,
       s.FISCALPRINTERDESCRIPTION,
	   s.STATIONPRINTINGHOSTID,
       s.Deleted
from   dbo.POSHARDWAREPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.PROFILEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_HardwareProfiles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_HardwareProfiles

GO

create procedure dbo.spAUDIT_ViewLog_HardwareProfiles
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.PROFILEID,
       s.NAME,
       s.DRAWER,
       s.DRAWERDEVICENAME,
       s.DRAWERDESCRIPTION,
       s.DRAWEROPENTEXT,
       s.DISPLAYDEVICE,
       s.DISPLAYDEVICENAME,
       s.DISPLAYDESCRIPTION,
	   s.DISPLAYTOTALTEXT,
	   s.DISPLAYBALANCETEXT,
       s.DISPLAYCLOSEDLINE1,
       s.DISPLAYCLOSEDLINE2,
       s.DISPLAYCHARACTERSET,
       s.MSR,
       s.MSRDEVICENAME,
       s.MSRDESCRIPTION,
       s.STARTTRACK1,
       s.SEPARATOR1,
       s.ENDTRACK1,
       s.PRINTER,
       s.PRINTERDEVICENAME,
       s.PRINTERDESCRIPTION,
	   s.PRINTBINARYCONVERSION,
       s.PRINTERCHARACTERSET,
       s.SCANNER,
       s.SCANNERDEVICENAME,
       s.SCANNERDESCRIPTION,
       s.SCALE,
       s.SCALEDEVICENAME,
       s.SCALEDESCRIPTION,
       s.MANUALINPUTALLOWED,
       s.KEYLOCK,
       s.KEYLOCKDEVICENAME,
       s.KEYLOCKDESCRIPTION,
       s.EFT,
       s.EFTSERVERNAME,
       s.EFTDESCRIPTION,
       s.EFTSERVERPORT,
       s.EFTCOMPANYID,
       s.EFTUSERID,
       s.EFTPASSWORD,
       s.EftBatchIncrementAtEOS,
       s.CCTV,
       s.CCTVCAMERA,
       s.CCTVHOSTNAME,
       s.CCTVPORT,
       s.FORECOURT,
       s.HOSTNAME,
       s.RFIDSCANNERTYPE,
       s.RFIDDEVICENAME,
       s.RFIDDESCRIPTION,
       s.CASHCHANGER,
       s.CASHCHANGERPORTSETTINGS,
       s.CASHCHANGERINITSETTINGS,
       s.DUALDISPLAY,
       s.DUALDISPLAYDEVICENAME,
       s.DUALDISPLAYDESCRIPTION,
       s.DUALDISPLAYTYPE,
       s.DUALDISPLAYPORT,
       s.DUALDISPLAYRECEIPTPERCENTAGE,
       s.DUALDISPLAYIMAGEPATH,
       s.DUALDISPLAYIMAGEINTERVAL,
       s.DUALDISPLAYBROWSERURL,
	   s.FISCALPRINTER,
       s.FISCALPRINTERCONNECTION,
       s.FISCALPRINTERDESCRIPTION,
	   s.STATIONPRINTINGHOSTID,
       s.Deleted
from   dbo.POSHARDWAREPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Form]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Form

GO

create procedure dbo.spAUDIT_ViewLog_Form
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ID,
       s.TITLE,
       s.DESCRIPTION,
       s.UPPERCASE,
       s.LINECOUNTPRPAGE,
       s.PRINTASSLIP,
       s.USEWINDOWSPRINTER,
       s.WINDOWSPRINTERNAME,
       s.PRINTBEHAVIOUR,
       s.PROMPTQUESTION,
	   s.WINDOWSPRINTERCONFIGURATIONID,
       s.Deleted
from   dbo.POSISFORMLAYOUTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Forms]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Forms

GO

create procedure dbo.spAUDIT_ViewLog_Forms
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ID,
       s.TITLE,
       s.DESCRIPTION,
       s.UPPERCASE,
       s.LINECOUNTPRPAGE,
       s.PRINTASSLIP,
       s.USEWINDOWSPRINTER,
       s.WINDOWSPRINTERNAME,
       s.PRINTBEHAVIOUR,
       s.PROMPTQUESTION,
	   s.WINDOWSPRINTERCONFIGURATIONID,
       s.Deleted
from   dbo.POSISFORMLAYOUTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UpdateSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_UpdateSchedule

GO

create procedure dbo.spAUDIT_ViewLog_UpdateSchedule
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.UPDATEID,
       s.STOREID,
       s.POSID,
       s.FILEID,
       s.FILECREATEDATE,
       s.STATUS,
       s.TEXT,
       s.HIGHPRIORITY,
       s.FOLDERPATH,
       s.POSAPPLICATIONPATH,
       s.USERNAME,
       s.NAME,
       s.FILEVERSION,
       s.FILEMODIFIEDDATE,
       s.COMPANY,
       s.DESCRIPTION,
       s.SCHEDULED
from   dbo.POSISUPDATESMASTERLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ColorGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ColorGroups

GO

create procedure dbo.spAUDIT_ViewLog_ColorGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.COLORGROUP,
	   s.DESCRIPTION,
       s.Deleted
from   dbo.RBOCOLORGROUPTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ColorGroupLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ColorGroupLines

GO

create procedure dbo.spAUDIT_ViewLog_ColorGroupLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.COLORGROUP,
       s.COLOR,
	   s.NAME,
       s.WEIGHT,
	   s.NOINBARCODE,
	   s.DESCRIPTION,
       s.Deleted
from   dbo.RBOCOLORGROUPTRANSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StyleGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StyleGroups

GO

create procedure dbo.spAUDIT_ViewLog_StyleGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.STYLEGROUP,
	   s.DESCRIPTION,
       s.Deleted
from   dbo.RBOSTYLEGROUPTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StyleGroupLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StyleGroupLines

GO

create procedure dbo.spAUDIT_ViewLog_StyleGroupLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.STYLEGROUP,
       s.STYLE,
	   s.NAME,
       s.WEIGHT,
	   s.NOINBARCODE,
	   s.DESCRIPTION,
       s.Deleted
from   dbo.RBOSTYLEGROUPTRANSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SizeGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SizeGroups

GO

create procedure dbo.spAUDIT_ViewLog_SizeGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.SIZEGROUP,
	   s.DESCRIPTION,
       s.Deleted
from   dbo.RBOSIZEGROUPTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SizeGroupLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SizeGroupLines

GO

create procedure dbo.spAUDIT_ViewLog_SizeGroupLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.SIZEGROUP,
       s.SIZE_,
	   s.NAME,
       s.WEIGHT,
	   s.NOINBARCODE,
	   s.DESCRIPTION,
       s.Deleted
from   dbo.RBOSIZEGROUPTRANSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_BarCodeSetup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_BarCodeSetup

GO

create procedure dbo.spAUDIT_ViewLog_BarCodeSetup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.BARCODESETUPID,
       s.BARCODETYPE,
       s.FONTNAME,
       s.FONTSIZE,
       s.DESCRIPTION,
       s.MINIMUMLENGTH,
       s.MAXIMUMLENGTH,
       s.RBOBARCODEMASK,
       s.Deleted
from   dbo.BARCODESETUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.BARCODESETUPID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_BarCodeSetups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_BarCodeSetups

GO

create procedure dbo.spAUDIT_ViewLog_BarCodeSetups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.BARCODESETUPID,
       s.BARCODETYPE,
       s.FONTNAME,
       s.FONTSIZE,
       s.DESCRIPTION,
       s.MINIMUMLENGTH,
       s.MAXIMUMLENGTH,
       s.RBOBARCODEMASK,
       s.Deleted
from   dbo.BARCODESETUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_BarCodeMasks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_BarCodeMasks

GO

create procedure dbo.spAUDIT_ViewLog_BarCodeMasks
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.MASKID,
       s.DESCRIPTION,
       s.MASK,
       s.PREFIX,
       s.SYMBOLOGY,
       s.TYPE,
       s.Deleted
from   dbo.RBOBARCODEMASKTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_BarCodeMaskSegments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_BarCodeMaskSegments

GO

create procedure dbo.spAUDIT_ViewLog_BarCodeMaskSegments
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.MASKID,
       s.SEGMENTNUM,
       s.LENGTH,
       s.TYPE,
       s.DECIMALS,
       s.CHAR,
       s.Deleted
from   dbo.RBOBARCODEMASKSEGMENTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemBarCodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemBarCodes

GO

create procedure dbo.spAUDIT_ViewLog_ItemBarCodes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ITEMID,
       s.ITEMBARCODE,
       s.RBOVARIANTID,
       s.BARCODESETUPID,
	   s.QTY,
	   s.UNITID,
       s.RBOSHOWFORITEM,
       s.USEFORINPUT,
       s.USEFORPRINTING,
       s.Deleted
from   dbo.INVENTITEMBARCODELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemDimensions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemDimensions

GO

create procedure dbo.spAUDIT_ViewLog_ItemDimensions
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ITEMID,
       s.RBOVARIANTID,
       s.NAME,
       s.INVENTSIZEID,
       s.INVENTCOLORID,
       s.INVENTSTYLEID,
       s.Deleted
from   dbo.INVENTDIMCOMBINATIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TradeAgreementsSalesPriceCustomers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TradeAgreementsSalesPriceCustomers

GO

create procedure dbo.spAUDIT_ViewLog_TradeAgreementsSalesPriceCustomers
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@contextIdentifier3 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.AGREEMENT,
	   s.ITEMCODE,
	   s.ACCOUNTCODE,
	   s.ITEMRELATION,
	   s.ACCOUNTRELATION,
	   s.QUANTITYAMOUNT,
	   s.FROMDATE,
	   s.TODATE,
	   s.AMOUNT,
	   s.AMOUNTINCLTAX,
	   s.CURRENCY,
	   s.PERCENT1,
	   s.PERCENT2,
	   s.DELIVERYTIME,
	   s.SEARCHAGAIN,
	   s.PRICEUNIT,
	   s.RELATION,
	   s.UNITID,
	   s.MARKUP,
	   s.ALLOCATEMARKUP,
	   s.MODULE,
	   s.INVENTDIMID,
	   s.RECID,
       s.Deleted
from   dbo.PRICEDISCTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ACCOUNTCODE = @contextIdentifier and s.ACCOUNTRELATION = @contextIdentifier2 and s.RELATION = CAST(@contextIdentifier3 as int) and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TradeAgreementsSalesPriceItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TradeAgreementsSalesPriceItems

GO

create procedure dbo.spAUDIT_ViewLog_TradeAgreementsSalesPriceItems
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@contextIdentifier3 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.AGREEMENT,
	   s.ITEMCODE,
	   s.ACCOUNTCODE,
	   s.ITEMRELATION,
	   s.ACCOUNTRELATION,
	   s.QUANTITYAMOUNT,
	   s.FROMDATE,
	   s.TODATE,
	   s.AMOUNT,
	   s.AMOUNTINCLTAX,
	   s.CURRENCY,
	   s.PERCENT1,
	   s.PERCENT2,
	   s.DELIVERYTIME,
	   s.SEARCHAGAIN,
	   s.PRICEUNIT,
	   s.RELATION,
	   s.UNITID,
	   s.MARKUP,
	   s.ALLOCATEMARKUP,
	   s.MODULE,
	   s.INVENTDIMID,
	   s.RECID,
       s.Deleted
from   dbo.PRICEDISCTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ITEMCODE = @contextIdentifier and s.ITEMRELATION = @contextIdentifier2 and s.RELATION = CAST(@contextIdentifier3 as int) and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PriceDiscountGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PriceDiscountGroups

GO

create procedure dbo.spAUDIT_ViewLog_PriceDiscountGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
	   s.TYPE,
	   s.GROUPID,
	   s.NAME,
	   s.INCLTAX,
       s.Deleted
from   dbo.PRICEDISCGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.MODULE = CAST(@contextIdentifier as int) and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventTableModule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventTableModule

GO

create procedure dbo.spAUDIT_ViewLog_InventTableModule
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
	   s.ITEMID,
	   s.UNITID,
	   s.PRICE,
	   s.PRICEINCLTAX,
	   --s.PRICEUNIT,
	   s.MARKUP,
	   s.LINEDISC,
	   s.MULTILINEDISC,
	   s.ENDDISC,
	   s.PRICEDATE,
	  -- s.PRICEQTY,
	   s.ALLOCATEMARKUP,
	   s.TAXITEMGROUPID,
       s.Deleted
from   dbo.INVENTTABLEMODULELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ITEMID = @contextIdentifier and s.MODULETYPE = CAST(@contextIdentifier2 as int) and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Parameters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Parameters

GO

create procedure dbo.spAUDIT_ViewLog_Parameters
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
	   s.LOCALSTOREID,
       s.Deleted
from   dbo.RBOPARAMETERSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.KEY_ = CAST(@contextIdentifier as int) and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TaxCodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TaxCodes

GO

create procedure dbo.spAUDIT_ViewLog_TaxCodes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TAXCODE,
	   s.TAXNAME,
	   s.TAXROUNDOFF,
	   s.TAXROUNDOFFTYPE,
       s.Deleted
from   dbo.TAXTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TaxCodesLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TaxCodesLines

GO

create procedure dbo.spAUDIT_ViewLog_TaxCodesLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TAXCODE,
       s.TAXFROMDATE,
	   s.TAXTODATE,
	   s.TAXLIMITMIN,
	   s.TAXLIMITMAX,
	   s.TAXVALUE,
       s.Deleted
from   dbo.TAXDATALog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemSalesTaxGroupCodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemSalesTaxGroupCodes

GO

create procedure dbo.spAUDIT_ViewLog_ItemSalesTaxGroupCodes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TAXITEMGROUP,
       s.TAXCODE,
       s.Deleted
from   dbo.TAXONITEMLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemSalesTaxGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemSalesTaxGroups

GO

create procedure dbo.spAUDIT_ViewLog_ItemSalesTaxGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TAXITEMGROUP,
       s.NAME,
       s.Deleted
from   dbo.TAXITEMGROUPHEADINGLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DimensionGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DimensionGroups

GO

create procedure dbo.spAUDIT_ViewLog_DimensionGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.DIMGROUPID,
       s.NAME,
	   s.POSDISPLAYSETTING,
       s.Deleted
from   dbo.INVENTDIMGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DimensionGroupSetup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DimensionGroupSetup

GO

create procedure dbo.spAUDIT_ViewLog_DimensionGroupSetup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.DIMGROUPID,
	   [Type] = Case
			when s.DIMFIELDID = 8 then 'Size'
			when s.DIMFIELDID = 9 then 'Color'
			when s.DIMFIELDID = 20001 then 'Style'
			when s.DIMFIELDID = 5 then 'Serial'
			else 'Unknown'
	   end,
	   s.ACTIVE,
	   s.ALLOWBLANKISSUE,
       s.Deleted
from   dbo.INVENTDIMSETUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SalesTaxGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SalesTaxGroup

GO

create procedure dbo.spAUDIT_ViewLog_SalesTaxGroup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TAXGROUP,
	   s.TAXGROUPNAME,
	   s.SEARCHFIELD1,
	   s.SEARCHFIELD2,
       s.Deleted
from   dbo.TAXGROUPHEADINGLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SalesTaxCodeInGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SalesTaxCodeInGroup

GO

create procedure dbo.spAUDIT_ViewLog_SalesTaxCodeInGroup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.TAXGROUP,
	   s.TAXCODE,
	   s.EXEMPTTAX,
	   s.USETAX,
       s.Deleted
from   dbo.TAXGROUPDATALog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DecimalSettings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DecimalSettings

GO

create procedure dbo.spAUDIT_ViewLog_DecimalSettings
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.ID,
	   s.NAME,
	   s.MINDECIMALS,
	   s.MAXDECIMALS,
       s.Deleted
from   dbo.DECIMALSETTINGSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_POSLicenseInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_POSLicenseInformation

GO

create procedure dbo.spAUDIT_ViewLog_POSLicenseInformation
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.STOREID,
	   s.TERMINALID,
	   s.VOLUMENO,
	   s.PASSWORD,
	   s.ERRORTXT,
	   s.ERROROCCURED,
       s.Deleted
from   dbo.POSISLICENSELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_POSWebServiceInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_POSWebServiceInformation

GO

create procedure dbo.spAUDIT_ViewLog_POSWebServiceInformation
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,		
	   s.WEBSERVICEUSERNAME,
	   s.WEBSERVICEPASSWORD,
       s.Deleted
from   dbo.POSISPARAMETERSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StoreManagementCurrency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StoreManagementCurrency

GO

create procedure dbo.spAUDIT_ViewLog_StoreManagementCurrency
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
	   s.AuditDate,	
	   s.CURRENCYCODE,
	   s.NAME,
	   s.Address,
	   s.PHONE,
	   s.TELEFAX as FAX,
	   s.ZIPCODE,
	   s.COUNTRYREGIONID,
	   s.EMAIL,
	   s.STREET,
	   s.CITY,
       s.Deleted
from   dbo.COMPANYINFOLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.KEY_ = CAST(@contextIdentifier as int) and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StoreManagementDiscounts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StoreManagementDiscounts

GO

create procedure dbo.spAUDIT_ViewLog_StoreManagementDiscounts
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
	   s.AuditDate,	
	   s.DISC,       
	   s.CALCPERIODICDISCS,
	   s.CALCCUSTOMERDISCS,
	   s.CLEARPERIODICDISCOUNTCACHE,
	   s.CLEARPERIODICDISCOUNTCACHEMINUTES,	   
	   s.Deleted
from   dbo.SALESPARAMETERSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.KEY_ = CAST(@contextIdentifier as int) and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_AdministrationDiscountAndPriceActivationPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_AdministrationDiscountAndPriceActivationPage

GO

create procedure dbo.spAUDIT_ViewLog_AdministrationDiscountAndPriceActivationPage
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
	   s.AuditDate,	
	    s.SALESPRICEACCOUNTITEM ,
		s.SALESLINEACCOUNTITEM ,
		s.SALESLINEACCOUNTGROUP ,
		s.SALESLINEACCOUNTALL ,
		s.SALESMULTILNACCOUNTGROUP ,
		s.SALESMULTILNACCOUNTALL ,
		s.SALESENDACCOUNTALL ,
		s.SALESPRICEGROUPITEM ,
		s.SALESLINEGROUPITEM ,
		s.SALESLINEGROUPGROUP ,
		s.SALESLINEGROUPALL ,
		s.SALESMULTILNGROUPGROUP ,
		s.SALESMULTILNGROUPALL ,
		s.SALESENDGROUPALL ,
		s.SALESPRICEALLITEM ,
		s.SALESLINEALLITEM ,
		s.SALESLINEALLGROUP ,
		s.SALESLINEALLALL ,
		s.SALESMULTILNALLGROUP ,
		s.SALESMULTILNALLALL ,
		s.SALESENDALLALL ,
       s.Deleted
from   dbo.PRICEPARAMETERSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.KEY_ = CAST(@contextIdentifier as int) and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemGroups

GO

create procedure dbo.spAUDIT_ViewLog_ItemGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.ITEMGROUPID ,
		s.Deleted
from   dbo.INVENTTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DiscountOffers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DiscountOffers

GO

create procedure dbo.spAUDIT_ViewLog_DiscountOffers
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.OFFERID,
		s.DESCRIPTION,
		s.DISCOUNTPCTVALUE,
		s.STATUS,
		s.PRIORITY,
		s.PDTYPE,
		s.DISCVALIDPERIODID,
		s.PRICEGROUP,
		s.Deleted
from   dbo.POSPERIODICDISCOUNTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.PDTYPE = CAST(@contextIdentifier as int)  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_MixAndMatchOffers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_MixAndMatchOffers

GO

create procedure dbo.spAUDIT_ViewLog_MixAndMatchOffers
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.OFFERID,
		s.DESCRIPTION,
		s.DEALPRICEVALUE,
		s.DISCOUNTPCTVALUE,
		s.DISCOUNTAMOUNTVALUE,
		s.NOOFLEASTEXPITEMS,
		s.STATUS,
		s.PRIORITY,
		s.PDTYPE,
		s.DISCVALIDPERIODID,
		s.PRICEGROUP,
		s.Deleted
from   dbo.POSPERIODICDISCOUNTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.PDTYPE = CAST(@contextIdentifier as int)  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DiscountOfferLine]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DiscountOfferLine

GO

create procedure dbo.spAUDIT_ViewLog_DiscountOfferLine
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select distinct s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.OFFERID,
		s.PRODUCTTYPE,
		s.LINEGROUP,
		s.ID,
		s.DEALPRICEORDISCPCT,
		s.Deleted
from dbo.POSPERIODICDISCOUNTLog k join dbo.POSPERIODICDISCOUNTLINELog s on k.OFFERID = s.OFFERID and k.DATAAREAID = s.DATAAREAID
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and k.PDTYPE = CAST(@contextIdentifier as int) and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DiscountPeriods]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DiscountPeriods

GO

create procedure dbo.spAUDIT_ViewLog_DiscountPeriods
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.ID ,
		s.DESCRIPTION ,
		s.STARTINGDATE  ,
		s.ENDINGDATE  ,
		s.STARTINGTIME  ,
		s.ENDINGTIME  ,
		s.TIMEWITHINBOUNDS  ,
		s.MONSTARTINGTIME  ,
		s.MONENDINGTIME  ,
		s.MONWITHINBOUNDS  ,
		s.TUESTARTINGTIME  ,
		s.TUEENDINGTIME  ,
		s.TUEWITHINBOUNDS  ,
		s.WEDSTARTINGTIME  ,
		s.WEDENDINGTIME  ,
		s.WEDWITHINBOUNDS  ,
		s.THUSTARTINGTIME  ,
		s.THUENDINGTIME  ,
		s.THUWITHINBOUNDS  ,
		s.FRISTARTINGTIME  ,
		s.FRIENDINGTIME  ,
		s.FRIWITHINBOUNDS  ,
		s.SATSTARTINGTIME  ,
		s.SATENDINGTIME  ,
		s.SATWITHINBOUNDS  ,
		s.SUNSTARTINGTIME  ,
		s.SUNENDINGTIME  ,
		s.SUNWITHINBOUNDS  ,
		s.ENDTIMEAFTERMID  ,
		s.MONAFTERMIDNIGHT  ,
		s.TUEAFTERMIDNIGHT  ,
		s.WEDAFTERMIDNIGHT  ,
		s.THUAFTERMIDNIGHT  ,
		s.FRIAFTERMIDNIGHT  ,
		s.SATAFTERMIDNIGHT  ,
		s.SUNAFTERMIDNIGHT  ,
		s.Deleted
from   dbo.POSDISCVALIDATIONPERIODLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_MultibuyDiscountLine]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_MultibuyDiscountLine

GO

create procedure dbo.spAUDIT_ViewLog_MultibuyDiscountLine
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.OFFERID,
		s.MINQUANTITY,
		s.UNITPRICEORDISCPCT,
		s.Deleted
from dbo.POSMULTIBUYDISCOUNTLINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_HospitalitySetup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_HospitalitySetup

GO

create procedure dbo.spAUDIT_ViewLog_HospitalitySetup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.SETUP,
        s.DELIVERYSALESTYPE,
        s.DINEINSALESTYPE,
        s.ORDERPROCESSTIMEMIN,
        s.TABLEFREECOLORB,
        s.TABLENOTAVAILCOLORB,
        s.TABLELOCKEDCOLORB,
        s.ORDERNOTPRINTEDCOLORB,
        s.ORDERPRINTEDCOLORB,
        s.ORDERSTARTEDCOLORB,
        s.ORDERFINISHEDCOLORB,
        s.ORDERCONFIRMEDCOLORB,
        s.TABLEFREECOLORF,
        s.TABLENOTAVAILCOLORF,
        s.TABLELOCKEDCOLORF,
        s.ORDERNOTPRINTEDCOLORF,
        s.ORDERPRINTEDCOLORF,
        s.ORDERSTARTEDCOLORF,
        s.ORDERFINISHEDCOLORF,
        s.ORDERCONFIRMEDCOLORF,
        s.CONFIRMSTATIONPRINTING,
        s.REQUESTNOOFGUESTS,
        s.NOOFDINEINTABLESCOL,
        s.NOOFDINEINTABLESROWS,
        s.STATIONPRINTING,
        s.DINEINTABLELOCKING,
        s.DINEINTABLESELECTION,
        s.PERIOD1TIMEFROM,
        s.PERIOD1TIMETO,
        s.PERIOD2TIMEFROM,
        s.PERIOD2TIMETO,
        s.PERIOD3TIMEFROM,
        s.PERIOD3TIMETO,
        s.PERIOD4TIMEFROM,
        s.PERIOD4TIMETO,
        s.AUTOLOGOFFATPOSEXIT,
        s.TAKEOUTSALESTYPE,
        s.PREORDERSALESTYPE,
        s.LOGSTATIONPRINTING,
        s.POPULATEDELIVERYINFOCODES,
        s.ALLOWPREORDERS,
        s.TAKEOUTNONAMENO,
        s.ADVPREORDPRINTMIN,
        s.CLOSETRIPONDEPART,
        s.DELPROGRESSSTATUSINUSE,
        s.DAYSBOMPRINTEXIST,
        s.DAYSBOMMONITOREXIST,
        s.DAYSDRIVERTRIPSEXIST,
        s.POSTERMINALPRINTPREORDERS,
        s.DISPLAYTIMEATORDERTAKING,
        s.NORMALPOSSALESTYPE,
        s.ORDLISTSCROLLPAGESIZE,
		s.Deleted
from dbo.HOSPITALITYSETUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RetailGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RetailGroups

GO

create procedure dbo.spAUDIT_ViewLog_RetailGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.GROUPID,
		s.NAME,
		s.NAMEALIAS,
		s.DEPARTMENTID AS RETAILDEPARTMENTID,
		s.SIZEGROUPID,
		s.COLORGROUPID,
		s.STYLEGROUPID,
		s.ITEMGROUPID AS RETAILGROUPID,
		s.SALESTAXITEMGROUP,
		s.Deleted
from dbo.RBOINVENTITEMRETAILGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RetailGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RetailGroup

GO

create procedure dbo.spAUDIT_ViewLog_RetailGroup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.NAME,
		s.NAMEALIAS,
		s.DEPARTMENTID AS RETAILDEPARTMENTID,
		s.SIZEGROUPID,
		s.COLORGROUPID,
		s.STYLEGROUPID,
		s.ITEMGROUPID AS RETAILGROUPID,
		s.SALESTAXITEMGROUP,
		s.Deleted
from dbo.RBOINVENTITEMRETAILGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.GROUPID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

--blabla

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RetailDepartments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RetailDepartments

GO

create procedure dbo.spAUDIT_ViewLog_RetailDepartments
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.DEPARTMENTID,
		s.NAME,
		s.NAMEALIAS,
		s.Deleted
from dbo.RBOINVENTITEMDEPARTMENTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RetailDepartment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RetailDepartment

GO

create procedure dbo.spAUDIT_ViewLog_RetailDepartment
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.NAME,
		s.NAMEALIAS,
		s.Deleted
from dbo.RBOINVENTITEMDEPARTMENTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.DEPARTMENTID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SpecialGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SpecialGroups

GO

create procedure dbo.spAUDIT_ViewLog_SpecialGroups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,	
		s.GROUPID,
		s.NAME,
		s.Deleted
from dbo.RBOSPECIALGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SpecialGroupsItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SpecialGroupsItem

GO

create procedure dbo.spAUDIT_ViewLog_SpecialGroupsItem
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ITEMID,	
		s.GROUPID,
		s.Deleted
from dbo.RBOSPECIALGROUPITEMSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.ITEMID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CURRENCIES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CURRENCIES

GO

create procedure dbo.spAUDIT_ViewLog_CURRENCIES
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.CURRENCYCODE,
		s.TXT,
		s.LEDGERACCOUNTPROFIT,
		s.LEDGERACCOUNTLOSS,
		s.ROUNDOFFSALES,
		s.ROUNDOFFPURCH,
		s.ROUNDOFFAMOUNT,
		s.CONSEXCHRATEMONETARY,
		s.CONSEXCHRATENONMONETARY,
		s.LEDGERACCOUNTNONREALLOSS,
		s.LEDGERACCOUNTNONREALPROFIT,
		s.ROUNDOFFTYPEPURCH,
		s.ROUNDOFFTYPESALES,
		s.ROUNDOFFTYPEAMOUNT,
		s.ROUNDOFFPROJECT,
		s.ROUNDOFFTYPEPROJECT,
		s.ROUNDOFFTYPEPRICE,
		s.ROUNDOFFPRICE,
		s.SYMBOL,
		s.CURRENCYPREFIX,
		s.CURRENCYSUFFIX,
		s.ONLINECONVERSIONTOOL,
		s.CURRENCYCODEISO,
		s.GENDERMALEFEMALE,
		s.Deleted
from dbo.CURRENCYLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UNITS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_UNITS

GO

create procedure dbo.spAUDIT_ViewLog_UNITS
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.UNITID,
		s.TXT,
		s.UNITDECIMALS,
		s.Deleted
from dbo.UnitLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PRICEGROUPS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PRICEGROUPS

GO

create procedure dbo.spAUDIT_ViewLog_PRICEGROUPS
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.GROUPID,
		s.NAME,
		s.Deleted
from dbo.PRICEGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RBOLOCATIONPRICEGROUP_Stores_and_groups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RBOLOCATIONPRICEGROUP_Stores_and_groups

GO

create procedure dbo.spAUDIT_ViewLog_RBOLOCATIONPRICEGROUP_Stores_and_groups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.STOREID,
		s.PRICEGROUPID,
		s.LEVEL_ as LEVEL,
		s.Deleted
from dbo.RBOLOCATIONPRICEGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_AdministrationNumberSequencesPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_AdministrationNumberSequencesPage

GO

create procedure dbo.spAUDIT_ViewLog_AdministrationNumberSequencesPage
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
       s.NUMBERSEQUENCE,
	   s.TXT,
	   s.HIGHEST,
	   s.FORMAT,
       s.Deleted
from   dbo.NUMBERSEQUENCETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UnitConversion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_UnitConversion

GO

create procedure dbo.spAUDIT_ViewLog_UnitConversion
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ITEMID,
		s.FROMUNIT,
		s.TOUNIT,
		s.FACTOR,
		s.Deleted
from dbo.UNITCONVERTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ExchangeRates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ExchangeRates

GO

create procedure dbo.spAUDIT_ViewLog_ExchangeRates
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		FROMDATE,
		EXCHRATE,
		CURRENCYCODE,
		POSEXCHRATE,
		s.Deleted
from dbo.EXCHRATESLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RetailItemGeneral]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RetailItemGeneral

GO

create procedure dbo.spAUDIT_ViewLog_RetailItemGeneral
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ITEMID,
		s.ITEMNAME,
		s.NAMEALIAS,
		s.ITEMTYPE,
		s.NOTES,
		s.ITEMGROUPID,
		s.DIMGROUPID,
		s.PRIMARYVENDORID as DEFAULTVENDOR,
		s.Deleted
from   dbo.INVENTTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.ITEMID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-- ---------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CashDeclaration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CashDeclaration

GO

create procedure dbo.spAUDIT_ViewLog_CashDeclaration
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.TYPE,
		s.CURRENCY,
		s.AMOUNT,
		s.Deleted
from dbo.RBOSTORECASHDECLARATIONTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SalesTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SalesTypes

GO

create procedure dbo.spAUDIT_ViewLog_SalesTypes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
	   s.DESCRIPTION,
	   s.REQUESTSALESPERSON,
	   s.REQUESTDEPOSITPERC,
	   s.REQUESTCHARGEACCOUNT,
	   s.PURCHASINGCODE,
	   s.DEFAULTORDERLIMIT,
	   s.LIMITSETTING,
	   s.REQUESTCONFIRMATION,
	   s.REQUESTDESCRIPTION,
	   s.NEWGLOBALDIMENSION2,
	   s.SUSPENDPRINTING,
	   s.SUSPENDTYPE,
	   s.PREPAYMENTACCOUNTNO,
	   s.MINIMUMDEPOSIT,
	   s.PRINTITEMLINESONPOSSLIP,
	   s.VOIDEDPREPAYMENTACCOUNTNO,
	   s.DAYSOPENTRANSEXIST,
	   s.TAXGROUPID,
	   s.PRICEGROUP,
	   s.TRANSDELETEREMINDER,
	   s.LOCATIONCODE,
	   s.PAYMENTISPREPAYMENT,
	   s.CALCPRICEFROMVATPRICE,
       s.Deleted
from   dbo.SALESTYPELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SalesType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SalesType

GO

create procedure dbo.spAUDIT_ViewLog_SalesType
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select s.AuditID, 
       s.AuditUserGUID,
       m.Login as AuditUserLogin,
       s.AuditUserLogin as AuditUserLogin2,
       s.AuditDate,
	   s.DESCRIPTION,
	   s.REQUESTSALESPERSON,
	   s.REQUESTDEPOSITPERC,
	   s.REQUESTCHARGEACCOUNT,
	   s.PURCHASINGCODE,
	   s.DEFAULTORDERLIMIT,
	   s.LIMITSETTING,
	   s.REQUESTCONFIRMATION,
	   s.REQUESTDESCRIPTION,
	   s.NEWGLOBALDIMENSION2,
	   s.SUSPENDPRINTING,
	   s.SUSPENDTYPE,
	   s.PREPAYMENTACCOUNTNO,
	   s.MINIMUMDEPOSIT,
	   s.PRINTITEMLINESONPOSSLIP,
	   s.VOIDEDPREPAYMENTACCOUNTNO,
	   s.DAYSOPENTRANSEXIST,
	   s.TAXGROUPID,
	   s.PRICEGROUP,
	   s.TRANSDELETEREMINDER,
	   s.LOCATIONCODE,
	   s.PAYMENTISPREPAYMENT,
	   s.CALCPRICEFROMVATPRICE,
       s.Deleted
from   dbo.SALESTYPELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.CODE = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Statements]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Statements

GO

create procedure dbo.spAUDIT_ViewLog_Statements
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.STATEMENTID,
		s.STOREID,
		s.CALCULATEDTIME,
		s.POSTINGDATE,
		s.PERIODSTARTINGTIME,
		s.PERIODENDINGTIME,
        s.Deleted
from    dbo.RBOSTATEMENTTABLELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StatementLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StatementLines

GO

create procedure dbo.spAUDIT_ViewLog_StatementLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.STATEMENTID,
		s.LINENUMBER,
		s.STAFFID,
		s.TERMINALID,
		s.CURRENCYCODE,
		s.TENDERID,
		s.TRANSACTIONAMOUNT,
		s.BANKEDAMOUNT,
		s.SAFEAMOUNT,
		s.COUNTEDAMOUNT,
		s.DIFFERENCE,
        s.Deleted
from    dbo.RBOSTATEMENLINELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_VendorItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_VendorItems

GO

create procedure dbo.spAUDIT_ViewLog_VendorItems
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.INTERNALID,
		s.VENDORITEMID,
		s.RETAILITEMID,
		s.VARIANTID,
		s.UNITID,
		s.VENDORID,
        s.Deleted
from    dbo.VENDORITEMSLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.VENDORID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Vendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Vendors

GO

create procedure dbo.spAUDIT_ViewLog_Vendors
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ACCOUNTNUM,
		s.NAME,
		s.ADDRESS,
		s.STREET,
		s.ZIPCODE,
		s.CITY,
		s.COUNTY,
		s.STATE,
		s.COUNTRY,
		s.PHONE,
		s.EMAIL,
		s.FAX,
		s.DEFAULTCONTACTID,
		s.LANGUAGEID,
		s.CURRENCY,
        s.Deleted
from    dbo.VENDTABLELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Vendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Vendor

GO

create procedure dbo.spAUDIT_ViewLog_Vendor
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ACCOUNTNUM,
		s.NAME,
		s.ADDRESS,
		s.STREET,
		s.ZIPCODE,
		s.CITY,
		s.COUNTY,
		s.STATE,
		s.COUNTRY,
		s.PHONE,
		s.EMAIL,
		s.FAX,
		s.DEFAULTCONTACTID,
		s.LANGUAGEID,
		s.CURRENCY,
        s.Deleted
from    dbo.VENDTABLELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.ACCOUNTNUM = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Contacts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Contacts

GO

create procedure dbo.spAUDIT_ViewLog_Contacts
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.CONTACTID,
		s.OWNERID,
		s.OWNERTYPE,
		s.CONTACTTYPE,
		s.COMPANYNAME,
		s.FirstName,
		s.MiddleName,
		s.LastName,
		s.NamePrefix,
		s.NameSuffix,
		s.ADDRESS,
		s.STREET,
		s.ZIPCODE,
		s.CITY,
		s.COUNTY,
		s.STATE,
		s.COUNTRY,
		s.PHONE,
		s.PHONE2,
		s.EMAIL,
		s.FAX,
        s.Deleted
from    dbo.CONTACTTABLELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.OWNERID = @contextIdentifier and s.OWNERTYPE = CAST(@contextIdentifier2 as int) and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventJPLHeaderLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventJPLHeaderLines

GO

create procedure dbo.spAUDIT_ViewLog_InventJPLHeaderLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255), @user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.JOURNALID,
		s.DESCRIPTION,
		s.POSTED,
		s.POSTEDDATETIME,
		s.JOURNALTYPE,
		s.DELETEPOSTEDLINES,
		s.CREATEDDATETIME,
        s.Deleted
from    dbo.INVENTJOURNALTABLELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventoryJPLLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventoryJPLLines

GO

create procedure dbo.spAUDIT_ViewLog_InventoryJPLLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255), @user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.JOURNALID,
		s.LINENUM,
		s.TRANSDATE,
		s.ITEMID,
		s.ADJUSTMENT,
		s.COSTPRICE,
		s.PRICEUNIT,
		s.COSTMARKUP,
		s.COSTAMOUNT,
		s.SALESAMOUNT,
		s.INVENTONHAND,
		s.COUNTED,
		s.REASONREFRECID,
		s.VARIANTID,
		s.PICTUREID,
		s.OMNITRANSACTIONID,
		s.OMNILINEID,
        s.Deleted
from    dbo.INVENTJOURNALTRANSLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_INVENTTRANSREASON]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_INVENTTRANSREASON

GO

create procedure dbo.spAUDIT_ViewLog_INVENTTRANSREASON
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.REASONID,
        s.REASONTEXT,
        s.ACTION,
        s.BEGINDATE,
        s.ENDDATE,
        s.ISSYSTEMREASONCODE,
        s.SHOWONPOS,
        s.Deleted
from    dbo.INVENTTRANSREASONLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Infocode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Infocode

GO

create procedure dbo.spAUDIT_ViewLog_Infocode
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select 	s.AuditID,
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.INFOCODEID,
		s.DESCRIPTION,
		s.PROMPT,
		s.ONCEPERTRANSACTION,
		s.VALUEISAMOUNTORQUANTITY,
		s.PRINTPROMPTONRECEIPT,
		s.PRINTINPUTONRECEIPT,
		s.PRINTINPUTNAMEONRECEIPT,
		s.INPUTTYPE,
		s.MINIMUMVALUE,
		s.MAXIMUMVALUE,
		s.MINIMUMLENGTH,
		s.MAXIMUMLENGTH,
		s.INPUTREQUIRED,
		s.STD1INVALUE,
		s.LINKEDINFOCODEID,
		s.RANDOMFACTOR,
		s.RANDOMCOUNTER,
		s.DATATYPEID,
		s.MODIFIEDDATE,
		s.MODIFIEDTIME,
		s.MODIFIEDBY,
		s.DATAAREAID,
		s.ADDITIONALCHECK,
		s.USAGECATEGORY,
		s.DISPLAYOPTION,
		s.LINKITEMLINESTOTRIGGERLINE,
		s.MULTIPLESELECTION,
		s.TRIGGERING,
		s.MINSELECTION,
		s.MAXSELECTION,
		s.CREATEINFOCODETRANSENTRIES,
		s.EXPLANATORYHEADERTEXT,
		s.OKPRESSEDACTION,
		s.Deleted
from dbo.RBOINFOCODETABLElog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.INFOCODEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Infocodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Infocodes

GO

create procedure dbo.spAUDIT_ViewLog_Infocodes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
        s.INFOCODEID,
        s.DESCRIPTION,
        s.PROMPT,
        s.ONCEPERTRANSACTION,
        s.VALUEISAMOUNTORQUANTITY,
        s.PRINTPROMPTONRECEIPT,
        s.PRINTINPUTONRECEIPT,
        s.PRINTINPUTNAMEONRECEIPT,
        s.INPUTTYPE,
        s.MINIMUMVALUE,
        s.MAXIMUMVALUE,
        s.MINIMUMLENGTH,
        s.MAXIMUMLENGTH,
        s.INPUTREQUIRED,
        s.STD1INVALUE,
        s.LINKEDINFOCODEID,
        s.RANDOMFACTOR,
        s.RANDOMCOUNTER,
        s.DATATYPEID,
        s.MODIFIEDDATE,
        s.MODIFIEDTIME,
        s.MODIFIEDBY,
        s.ADDITIONALCHECK,
        s.USAGECATEGORY,
        s.DISPLAYOPTION,
        s.LINKITEMLINESTOTRIGGERLINE,
        s.MULTIPLESELECTION,
        s.TRIGGERING,
        s.MINSELECTION,
        s.MAXSELECTION,
        s.CREATEINFOCODETRANSENTRIES,
        s.EXPLANATORYHEADERTEXT,
        s.OKPRESSEDACTION,
        s.Deleted
from   dbo.RBOINFOCODETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.INPUTTYPE <> 12 and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InfocodeGroupsByUsageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InfocodeGroupsByUsageCategory

GO

create procedure dbo.spAUDIT_ViewLog_InfocodeGroupsByUsageCategory
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
        s.INFOCODEID,
        s.DESCRIPTION,
        s.PROMPT,
        s.ONCEPERTRANSACTION,
        s.VALUEISAMOUNTORQUANTITY,
        s.PRINTPROMPTONRECEIPT,
        s.PRINTINPUTONRECEIPT,
        s.PRINTINPUTNAMEONRECEIPT,
        s.INPUTTYPE,
        s.MINIMUMVALUE,
        s.MAXIMUMVALUE,
        s.MINIMUMLENGTH,
        s.MAXIMUMLENGTH,
        s.INPUTREQUIRED,
        s.STD1INVALUE,
        s.LINKEDINFOCODEID,
        s.RANDOMFACTOR,
        s.RANDOMCOUNTER,
        s.DATATYPEID,
        s.MODIFIEDDATE,
        s.MODIFIEDTIME,
        s.MODIFIEDBY,
        s.ADDITIONALCHECK,
        s.USAGECATEGORY,
        s.DISPLAYOPTION,
        s.LINKITEMLINESTOTRIGGERLINE,
        s.MULTIPLESELECTION,
        s.TRIGGERING,
        s.MINSELECTION,
        s.MAXSELECTION,
        s.CREATEINFOCODETRANSENTRIES,
        s.EXPLANATORYHEADERTEXT,
        s.OKPRESSEDACTION,
        s.Deleted
from   dbo.RBOINFOCODETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.INPUTTYPE = 12 and s.USAGECATEGORY = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InfocodeTableSpecific]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InfocodeTableSpecific

GO

create procedure dbo.spAUDIT_ViewLog_InfocodeTableSpecific
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select	s.AuditID,
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.REFRELATION,
		s.SEQUENCE,
		s.INFOCODEID,
		s.INPUTREQUIRED,
		s.WHENREQUIRED,
		s.REFRELATION2,
		s.REFRELATION3,
		s.REFTABLEID,
		s.DATAAREAID,
		s.TRIGGERING,
		s.UNITOFMEASURE,
		s.SALESTYPEFILTER,
		s.USAGECATEGORY,
		s.Deleted
from dbo.RBOINFOCODETABLESPECIFICLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InformationSubCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InformationSubCode

GO

create procedure dbo.spAUDIT_ViewLog_InformationSubCode
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.INFOCODEID,
		s.SUBCODEID,
		s.DESCRIPTION,
		s.TRIGGERFUNCTION,
		s.TRIGGERCODE,
		s.NEWSALESLINE,
		s.PRICETYPE,
		s.AMOUNTPERCENT,
		s.DATAAREAID,
		s.USAGECATEGORY,
		s.VARIANTCODE,
		s.VARIANTNEEDED,
		s.QTYLINKEDTOTRIGGERLINE,
		s.PRICEHANDLING,
		s.LINKEDINFOCODE,
		s.UNITOFMEASURE,
		s.QTYPERUNITOFMEASURE,
		s.INFOCODEPROMPT,
		s.MAXSELECTION,
		s.SERIALLOTNEEDED,
		s.Deleted
from dbo.RBOINFORMATIONSUBCODETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.SUBCODEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InformationSubCodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InformationSubCodes

GO

create procedure dbo.spAUDIT_ViewLog_InformationSubCodes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select	s.AuditID,
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.INFOCODEID,
		s.SUBCODEID,
		s.DESCRIPTION,
		s.TRIGGERFUNCTION,
		s.TRIGGERCODE,
		s.NEWSALESLINE,
		s.PRICETYPE,
		s.AMOUNTPERCENT,
		s.USAGECATEGORY,
		s.VARIANTCODE,
		s.VARIANTNEEDED,
		s.QTYLINKEDTOTRIGGERLINE,
		s.PRICEHANDLING,
		s.LINKEDINFOCODE,
		s.UNITOFMEASURE,
		s.QTYPERUNITOFMEASURE,
		s.INFOCODEPROMPT,
		s.MAXSELECTION,
		s.SERIALLOTNEEDED,
		s.Deleted
from   dbo.RBOINFORMATIONSUBCODETABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_GoodsReceivingDocument]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_GoodsReceivingDocument

GO

create procedure dbo.spAUDIT_ViewLog_GoodsReceivingDocument
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.GOODSRECEIVINGID,
		s.PURCHASEORDERID,
		s.STATUS,
		s.CREATEDDATE,
		s.POSTEDDATE,
        s.Deleted
from    dbo.GOODSRECEIVINGLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.GOODSRECEIVINGID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_GoodsReceivingDocuments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_GoodsReceivingDocuments

GO

create procedure dbo.spAUDIT_ViewLog_GoodsReceivingDocuments
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.GOODSRECEIVINGID,
		s.PURCHASEORDERID,
		s.STATUS,
		s.CREATEDDATE,
		s.POSTEDDATE,
        s.Deleted
from    dbo.GOODSRECEIVINGLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_GoodsReceivingDocumentLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_GoodsReceivingDocumentLines

GO

create procedure dbo.spAUDIT_ViewLog_GoodsReceivingDocumentLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.GOODSRECEIVINGID,
		s.PURCHASEORDERLINENUMBER,
		s.LINENUMBER,
		s.RECEIVEDQUANTITY,
		s.RECEIVEDDATE,
		s.POSTED,
		s.STOREID,
        s.Deleted
from    dbo.GOODSRECEIVINGLINELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.GOODSRECEIVINGID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PurchaseOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PurchaseOrder

GO

create procedure dbo.spAUDIT_ViewLog_PurchaseOrder
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.PURCHASEORDERID,
		s.VENDORID,
		s.CONTACTID,
		s.PURCHASESTATUS,
		s.DELIVERYDATE,
		s.CURRENCYCODE,
		s.CREATEDDATE,
		s.ORDERER,
		s.STOREID,
        s.Deleted
from    dbo.PURCHASEORDERSLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.PURCHASEORDERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PurchaseOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PurchaseOrders

GO

create procedure dbo.spAUDIT_ViewLog_PurchaseOrders
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.PURCHASEORDERID,
		s.VENDORID,
		s.CONTACTID,
		s.PURCHASESTATUS,
		s.DELIVERYDATE,
		s.CURRENCYCODE,
		s.CREATEDDATE,
		s.ORDERER,
        s.Deleted
from    dbo.PURCHASEORDERSLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PurchaseOrderMiscCharges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PurchaseOrderMiscCharges

GO

create procedure dbo.spAUDIT_ViewLog_PurchaseOrderMiscCharges
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.PURCHASEORDERID,
		s.LINENUMBER,
		s.TYPE,
		s.REASON,
		s.AMOUNT,
		s.TAXAMOUNT,
        s.Deleted
from    dbo.PURCHASEORDERMISCCHARGESLog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.PURCHASEORDERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PurchaseOrderLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PurchaseOrderLines

GO

create procedure dbo.spAUDIT_ViewLog_PurchaseOrderLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.PURCHASEORDERID,
		s.LINENUMBER,
		s.RETAILITEMID,
		s.VENDORITEMID,
		s.VARIANTID,
		s.UNITID,
		s.QUANTITY,
		s.PRICE,
		s.PICTUREID,
		s.OMNITRANSACTIONID,
		s.OMNILINEID,
        s.Deleted
from    dbo.PURCHASEORDERLINELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.PURCHASEORDERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Statements]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Statements

GO

create procedure dbo.spAUDIT_ViewLog_Statements
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.STATEMENTID,
		s.STOREID,
		s.CALCULATEDTIME,
		s.POSTINGDATE,
		s.PERIODSTARTINGTIME,
		s.PERIODENDINGTIME,
		s.POSTED,
		s.CALCULATED,
        s.Deleted
from    dbo.RBOSTATEMENTTABLELog s
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and s.STOREID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StatementLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StatementLines

GO

create procedure dbo.spAUDIT_ViewLog_StatementLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.STATEMENTID,
		s.LINENUMBER,
		s.STAFFID,
		s.TERMINALID,
		s.CURRENCYCODE,
		s.TENDERID,
		s.TRANSACTIONAMOUNT,
		s.BANKEDAMOUNT,
		s.SAFEAMOUNT,
		s.COUNTEDAMOUNT,
		s.DIFFERENCE,
        s.Deleted
from    dbo.RBOSTATEMENTLINELog s
		join RBOSTATEMENTTABLELog t on s.STATEMENTID = t.STATEMENTID and s.DATAAREAID = t.DATAAREAID
        left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where   s.DATAAREAID = @dataAreaID and t.STOREID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DiningTableLayout]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DiningTableLayout

GO

create procedure dbo.spAUDIT_ViewLog_DiningTableLayout
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.RESTAURANTID,
		s.SEQUENCE,
		s.SALESTYPE,
		s.LAYOUTID,
		s.DESCRIPTION,
		s.NOOFSCREENS,
		s.STARTINGTABLENO,
		s.NOOFDININGTABLES,
		s.ENDINGTABLENO,
		s.DININGTABLEROWS,
		s.DININGTABLECOLUMNS,
		s.CURRENTLAYOUT,
        s.Deleted
from   dbo.DININGTABLELAYOUTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to


GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_HospitalityTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_HospitalityTypes

GO

create procedure dbo.spAUDIT_ViewLog_HospitalityTypes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.RESTAURANTID,
		s.SEQUENCE,
		s.DESCRIPTION,
		s.OVERVIEW,
		s.SALESTYPE,
		s.UPDATETABLEFROMPOS,
		s.REQUESTNOOFGUESTS,
		s.STATIONPRINTING,
		s.ACCESSTOOTHERRESTAURANT,
		s.POSLOGONMENUID,
		s.ALLOWNEWENTRIES,
		s.TIPSAMTLINE1,
		s.TIPSAMTLINE2,
		s.TIPSTOTALLINE,
		s.STAYINPOSAFTERTRANS,
		s.TIPSINCOMEACC1,
		s.TIPSINCOMEACC2,
		s.NOOFDINEINTABLES,
		s.TABLEBUTTONPOSMENUID,
		s.TABLEBUTTONDESCRIPTION,
		s.TABLEBUTTONSTAFFDESCRIPTION,
		s.STAFFTAKEOVERINTRANS,
		s.MANAGERTAKEOVERINTRANS,
		s.VIEWSALESSTAFF,
		s.VIEWTRANSDATE,
		s.VIEWTRANSTIME,
		s.VIEWDELIVERYADDRESS,
		s.VIEWLISTTOTALS,
		s.ORDERBY,
		s.VIEWRESTAURANT,
		s.VIEWGRID,
		s.VIEWCOUNTDOWN,
		s.VIEWPROGRESSSTATUS,
		s.DIRECTEDITOPERATION,
		s.SETTINGSFROMHOSPTYPE,
		s.SETTINGSFROMSEQUENCE,
		s.SHARINGSALESTYPEFILTER,
		s.SETTINGSFROMRESTAURANT,
		s.GUESTBUTTONS,
		s.MAXGUESTBUTTONSSHOWN,
		s.MAXGUESTSPERTABLE,
		s.SPLITBILLLOOKUPID,
		s.SELECTGUESTONSPLITTING,
		s.COMBINESPLITLINESACTION,
		s.TRANSFERLINESLOOKUPID,
		s.PRINTTRAININGTRANSACTIONS,
		s.LAYOUTID,
		s.TOPPOSMENUID,
		s.DININGTABLELAYOUTID,
		s.AUTOMATICJOININGCHECK,
        s.Deleted
from   dbo.HOSPITALITYTYPELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to


GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_HospitalityType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_HospitalityType

GO

create procedure dbo.spAUDIT_ViewLog_HospitalityType
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.RESTAURANTID,
		s.SEQUENCE,
		s.DESCRIPTION,
		s.OVERVIEW,
		s.SALESTYPE,
		s.UPDATETABLEFROMPOS,
		s.REQUESTNOOFGUESTS,
		s.STATIONPRINTING,
		s.ACCESSTOOTHERRESTAURANT,
		s.POSLOGONMENUID,
		s.ALLOWNEWENTRIES,
		s.TIPSAMTLINE1,
		s.TIPSAMTLINE2,
		s.TIPSTOTALLINE,
		s.STAYINPOSAFTERTRANS,
		s.TIPSINCOMEACC1,
		s.TIPSINCOMEACC2,
		s.NOOFDINEINTABLES,
		s.TABLEBUTTONPOSMENUID,
		s.TABLEBUTTONDESCRIPTION,
		s.TABLEBUTTONSTAFFDESCRIPTION,
		s.STAFFTAKEOVERINTRANS,
		s.MANAGERTAKEOVERINTRANS,
		s.VIEWSALESSTAFF,
		s.VIEWTRANSDATE,
		s.VIEWTRANSTIME,
		s.VIEWDELIVERYADDRESS,
		s.VIEWLISTTOTALS,
		s.ORDERBY,
		s.VIEWRESTAURANT,
		s.VIEWGRID,
		s.VIEWCOUNTDOWN,
		s.VIEWPROGRESSSTATUS,
		s.DIRECTEDITOPERATION,
		s.SETTINGSFROMHOSPTYPE,
		s.SETTINGSFROMSEQUENCE,
		s.SHARINGSALESTYPEFILTER,
		s.SETTINGSFROMRESTAURANT,
		s.GUESTBUTTONS,
		s.MAXGUESTBUTTONSSHOWN,
		s.MAXGUESTSPERTABLE,
		s.SPLITBILLLOOKUPID,
		s.SELECTGUESTONSPLITTING,
		s.COMBINESPLITLINESACTION,
		s.TRANSFERLINESLOOKUPID,
		s.PRINTTRAININGTRANSACTIONS,
		s.LAYOUTID,
		s.TOPPOSMENUID,
		s.DININGTABLELAYOUTID,
		s.AUTOMATICJOININGCHECK,
        s.Deleted
from   dbo.HOSPITALITYTYPELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.RESTAURANTID = @contextIdentifier and s.SALESTYPE = @contextIdentifier2 and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to


GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PosLookups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PosLookups

GO

create procedure dbo.spAUDIT_ViewLog_PosLookups
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.LOOKUPID,
		s.DESCRIPTION,
		s.DYNAMICMENUID,
		s.DYNAMICMENU2ID,
		s.GRID1MENUID,
		s.GRID2MENUID,
        s.Deleted
from   dbo.POSLOOKUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to


GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PosMenuHeaders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PosMenuHeaders

GO

create procedure dbo.spAUDIT_ViewLog_PosMenuHeaders
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.MENUID,
		s.DESCRIPTION,
		s.COLUMNS,
		s.ROWS,
		s.MENUCOLOR,
		s.FONTNAME,
		s.FONTSIZE,
		s.FONTBOLD,
		s.FORECOLOR,
		s.BACKCOLOR,
		s.FONTITALIC,
		s.FONTCHARSET,
		s.USENAVOPERATION,
		s.APPLIESTO,
		s.DEVICETYPE,
		s.MAINMENU,
        s.Deleted
from   dbo.POSMENUHEADERLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PosMenuLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PosMenuLines

GO

create procedure dbo.spAUDIT_ViewLog_PosMenuLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.MENUID,
		s.SEQUENCE,
		s.KEYNO,
		s.DESCRIPTION,
		s.OPERATION,
		s.PARAMETER,
		s.PARAMETERTYPE,
		s.FONTNAME,
		s.FONTSIZE,
		s.FONTBOLD,
		s.FORECOLOR,
		s.BACKCOLOR,
		s.FONTITALIC,
		s.FONTCHARSET,
		s.DISABLED,
		s.PICTUREFILE,
		s.HIDEDESCRONPICTURE,
		s.USEHEADERFONT,
		s.FONTSTRIKETHROUGH,
		s.FONTUNDERLINE,
		s.USEHEADERATTRIBUTES,
		s.COLUMNSPAN,
		s.ROWSPAN,
		s.NAVOPERATION,
		s.HIDDEN,
		s.SHADEWHENDISABLED,
		s.BACKGROUNDHIDDEN,
		s.TRANSPARENT,
		s.GLYPH,
		s.GLYPH2,
		s.GLYPH3,
		s.GLYPH4,
		s.GLYPHTEXT,
		s.GLYPHTEXT2,
		s.GLYPHTEXT3,
		s.GLYPHTEXT4,
		s.GLYPHTEXTFONT,
		s.GLYPHTEXT2FONT,
		s.GLYPHTEXT3FONT,
		s.GLYPHTEXT4FONT,
		s.GLYPHTEXTFONTSIZE,
		s.GLYPHTEXT2FONTSIZE,
		s.GLYPHTEXT3FONTSIZE,
		s.GLYPHTEXT4FONTSIZE,
		s.GLYPHTEXTFORECOLOR,
		s.GLYPHTEXT2FORECOLOR,
		s.GLYPHTEXT3FORECOLOR,
		s.GLYPHTEXT4FORECOLOR,
		s.GLYPHOFFSET,
		s.GLYPH2OFFSET,
		s.GLYPH3OFFSET,
		s.GLYPH4OFFSET,
		s.BACKCOLOR2,
		s.GRADIENTMODE,
		s.SHAPE,
        s.Deleted
from   dbo.POSMENULINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PosMenuLine]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PosMenuLine

GO

create procedure dbo.spAUDIT_ViewLog_PosMenuLine
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@contextIdentifier2 nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.MENUID,
		s.SEQUENCE,
		s.KEYNO,
		s.DESCRIPTION,
		s.OPERATION,
		s.PARAMETER,
		s.PARAMETERTYPE,
		s.FONTNAME,
		s.FONTSIZE,
		s.FONTBOLD,
		s.FORECOLOR,
		s.BACKCOLOR,
		s.FONTITALIC,
		s.FONTCHARSET,
		s.USEHEADERFONT,
		s.USEHEADERATTRIBUTES,
		s.DISABLED,
		s.PICTUREFILE,
		s.HIDEDESCRONPICTURE,
		s.FONTSTRIKETHROUGH,
		s.FONTUNDERLINE,
		s.COLUMNSPAN,
		s.ROWSPAN,
		s.NAVOPERATION,
		s.HIDDEN,
		s.SHADEWHENDISABLED,
		s.BACKGROUNDHIDDEN,
		s.TRANSPARENT,
		s.GLYPH,
		s.GLYPH2,
		s.GLYPH3,
		s.GLYPH4,
		s.GLYPHTEXT,
		s.GLYPHTEXT2,
		s.GLYPHTEXT3,
		s.GLYPHTEXT4,
		s.GLYPHTEXTFONT,
		s.GLYPHTEXT2FONT,
		s.GLYPHTEXT3FONT,
		s.GLYPHTEXT4FONT,
		s.GLYPHTEXTFONTSIZE,
		s.GLYPHTEXT2FONTSIZE,
		s.GLYPHTEXT3FONTSIZE,
		s.GLYPHTEXT4FONTSIZE,
		s.GLYPHTEXTFORECOLOR,
		s.GLYPHTEXT2FORECOLOR,
		s.GLYPHTEXT3FORECOLOR,
		s.GLYPHTEXT4FORECOLOR,
		s.GLYPHOFFSET,
		s.GLYPH2OFFSET,
		s.GLYPH3OFFSET,
		s.GLYPH4OFFSET,
		s.BACKCOLOR2,
		s.GRADIENTMODE,
		s.SHAPE,
        s.Deleted
from   dbo.POSMENULINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.MENUID = @contextIdentifier and s.SEQUENCE = @contextIdentifier2 and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RestaurantStations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RestaurantStations

GO

create procedure dbo.spAUDIT_ViewLog_RestaurantStations
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ID,
		s.STATIONNAME,
		s.WINDOWSPRINTER,
		s.OUTPUTLINES,
		s.CHECKPRINTING,
		s.POSEXTERNALPRINTERID,
		s.PRINTING,
		s.STATIONFILTER,
		s.STATIONCHECKMINUTES,
		s.COMPRESSBOMRECEIPT,
		s.EXCLUDEFROMCOMPRESSION,
		s.STATIONTYPE,
		s.PRINTINGPRIORITY,
		s.ENDTURNSREDAFTERMIN,
		s.SCREENALIGNMENT,
		s.SCREENNUMBER,
        s.Deleted
from   dbo.RESTAURANTSTATIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STATIONTYPE <> 3 and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RestaurantStation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RestaurantStation

GO

create procedure dbo.spAUDIT_ViewLog_RestaurantStation
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case 
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID, 
		s.AuditUserGUID,
		m.Login as AuditUserLogin,
		s.AuditUserLogin as AuditUserLogin2,
		s.AuditDate,
		s.ID,
		s.STATIONNAME,
		s.WINDOWSPRINTER,
		s.OUTPUTLINES,
		s.CHECKPRINTING,
		s.POSEXTERNALPRINTERID,
		s.PRINTING,
		s.STATIONFILTER,
		s.STATIONCHECKMINUTES,
		s.COMPRESSBOMRECEIPT,
		s.EXCLUDEFROMCOMPRESSION,
		s.STATIONTYPE,
		s.PRINTINGPRIORITY,
		s.ENDTURNSREDAFTERMIN,
		s.SCREENALIGNMENT,
		s.SCREENNUMBER,
        s.Deleted
from   dbo.RESTAURANTSTATIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RBOCustomer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RBOCustomer

GO

create procedure dbo.spAUDIT_ViewLog_RBOCustomer
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ACCOUNTNUM,
        s.OTHERTENDERINFINALIZING,
        s.POSTASSHIPMENT,
        s.USEORDERNUMBERREFERENCE,
        s.RECEIPTOPTION,
        s.RECEIPTEMAIL,
        s.NONCHARGABLEACCOUNT,
        s.REQUIRESAPPROVAL,
        s.Deleted
from   dbo.RBOCUSTTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ACCOUNTNUM = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StationPrinting]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StationPrinting

GO

create procedure dbo.spAUDIT_ViewLog_StationPrinting
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.TYPE,
        s.CODE,
        s.STATIONID,
        s.SALESTYPE,
        s.RESTAURANTID,
        s.DESCRIPTION,
        s.Deleted
from   dbo.STATIONSELECTIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemImages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemImages

GO

create procedure dbo.spAUDIT_ViewLog_ItemImages
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ITEMID,
        s.Deleted
from   dbo.RBOINVENTITEMIMAGELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.ITEMID = @contextIdentifier  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_LinkedItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_LinkedItems

GO

create procedure dbo.spAUDIT_ViewLog_LinkedItems
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ITEMID as ORIGINALITEMID,
		s.UNIT,
		s.LINKEDITEMID,
		s.QTY,
		s.BLOCKED,
        s.Deleted
from   dbo.RBOINVENTLINKEDITEMLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.ITEMID = @contextIdentifier  and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_GiftCard]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_GiftCard

GO

create procedure dbo.spAUDIT_ViewLog_GiftCard
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.GIFTCARDID,
        s.BALANCE,
        s.CURRENCY,
        s.ACTIVE,
		s.Refillable,
        s.Deleted
		
from   dbo.RBOGIFTCARDTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.GIFTCARDID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_GiftCardLine]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_GiftCardLine

GO

create procedure dbo.spAUDIT_ViewLog_GiftCardLine
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.GIFTCARDID,
        s.GIFTCARDLINEID,
        s.STOREID,
        s.TERMINALID,
        s.TRANSACTIONNUMBER,
        s.RECEIPTID,
        s.STAFFID,
        s.USERID,
        s.TRANSACTIONDATE,
        s.TRANSACTIONTIME,
        s.AMOUNT,
        s.OPERATION,
        s.Deleted
from   dbo.RBOGIFTCARDTRANSACTIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.GIFTCARDID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CreditVoucher]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CreditVoucher

GO

create procedure dbo.spAUDIT_ViewLog_CreditVoucher
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.VOUCHERID,
        s.BALANCE,
        s.CURRENCY,
        s.Deleted
from   dbo.RBOCREDITVOUCHERTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.VOUCHERID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CreditVoucherLine]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CreditVoucherLine

GO

create procedure dbo.spAUDIT_ViewLog_CreditVoucherLine
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.VOUCHERID,
        s.VOUCHERLINEID,
        s.STOREID,
        s.TERMINALID,
        s.TRANSACTIONNUMBER,
        s.RECEIPTID,
        s.STAFFID,
        s.USERID,
        s.TRANSACTIONDATE,
        s.TRANSACTIONTIME,
        s.AMOUNT,
        s.OPERATION,
        s.Deleted
from   dbo.RBOCREDITVOUCHERTRANSACTIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.VOUCHERID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SuspendedTransactionTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SuspendedTransactionTypes

GO

create procedure dbo.spAUDIT_ViewLog_SuspendedTransactionTypes
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.DESCRIPTION,
        s.ALLOWEOD,
        s.Deleted
from   dbo.POSISSUSPENSIONTYPELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
-----------------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SuspendedTransactionAddInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SuspendedTransactionAddInfo

GO

create procedure dbo.spAUDIT_ViewLog_SuspendedTransactionAddInfo
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.SUSPENSIONTYPEID,
        s.PROMPT,
        s.FIELDORDER,
        s.INFOTYPE,
        s.INFOTYPESELECTION,
        s.REQUIRED,
        s.Deleted
from   dbo.POSISSUSPENSIONADDINFOLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.SUSPENSIONTYPEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_SuspendTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_SuspendTransaction

GO

create procedure dbo.spAUDIT_ViewLog_SuspendTransaction
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.TRANSACTIONID,
        s.BYTELENGTH,
        s.TRANSDATE,
        s.STAFF,
        s.BALANCE,
        s.STOREID,
        s.TERMINALID,
        s.RECALLEDBY,
        s.DESCRIPTION,
        s.ALLOWEOD,
        s.ACTIVE,
        s.SUSPENSIONTYPEID,
        s.BALANCEWITHTAX,
        s.Deleted
from   dbo.POSISSUSPENDEDTRANSACTIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


-----------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_POSSUSPENDTRANSADDINFO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_POSSUSPENDTRANSADDINFO

GO

create procedure dbo.spAUDIT_ViewLog_POSSUSPENDTRANSADDINFO
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.TRANSACTIONID,
        s.PROMPT,
        s.FIELDORDER,
        s.INFOTYPE,
        s.INFOTYPESELECTION,
        s.TEXTRESULT1,
        s.TEXTRESULT2,
        s.TEXTRESULT3,
        s.TEXTRESULT4,
        s.TEXTRESULT5,
        s.TEXTRESULT6,
        s.DATERESULT,
        s.Deleted
from   dbo.POSISSUSPENDTRANSADDINFOLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_MsrCardLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_MsrCardLinks

GO

create procedure dbo.spAUDIT_ViewLog_MsrCardLinks
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.LINKTYPE,
        s.LINKID,
        s.CARDNUMBER,
        s.Deleted
from   dbo.RBOMSRCARDTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.LINKTYPE = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_PromotionLine]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_PromotionLine

GO

create procedure dbo.spAUDIT_ViewLog_PromotionLine
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.OFFERID,
        s.STATUS,
        s.ITEMRELATION,
        s.NAME,
        s.DISCPCT,
        s.DISCAMOUNT,
        s.OFFERPRICE,
        s.OFFERPRICEINCLTAX,
        s.ID,
        s.Deleted
from   dbo.RBODISCOUNTOFFERLINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

-----------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_POSMMLINEGROUPS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_POSMMLINEGROUPS

GO

create procedure dbo.spAUDIT_ViewLog_POSMMLINEGROUPS
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.OFFERID,
        s.LINEGROUP,
        s.NOOFITEMSNEEDED,
        s.COLOR,
        s.DESCRIPTION,
        s.Deleted
from   dbo.POSMMLINEGROUPSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to


GO

---------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RBOINVENTTABLE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RBOINVENTTABLE

GO

create procedure dbo.spAUDIT_ViewLog_RBOINVENTTABLE
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ITEMID,
        s.ITEMTYPE,
        s.ITEMGROUP,
        s.ITEMDEPARTMENT,
        s.ITEMFAMILY,
        s.UNITPRICEINCLUDINGTAX,
        s.COSTCALCULATIONONPOS,
        s.NOINVENTPOSTING,
        s.ZEROPRICEVALID,
        s.QTYBECOMESNEGATIVE,
        s.NODISCOUNTALLOWED,
        s.KEYINGINPRICE,
        s.SCALEITEM,
        s.KEYINGINQTY,
        s.DATEBLOCKED,
        s.DATETOBEBLOCKED,
        s.BLOCKEDONPOS,
        s.DISPENSEPRINTINGDISABLED,
        --s.DISPENSEPRINTERGROUPID,
        --s.BASECOMPARISONUNITCODE,
        --s.BARCODESETUPID,
        s.PRINTVARIANTSSHELFLABELS,
        --s.COLORGROUP,
        --s.SIZEGROUP,
        s.USEEANSTANDARDBARCODE,
        s.STYLEGROUP,
        s.FUELITEM,
        s.GRADEID,
        s.MUSTKEYINCOMMENT,
        s.DATETOACTIVATEITEM,
        --s.BUSINESSGROUP,
        --s.DIVISIONGROUP,
        s.DEFAULTPROFIT,
        s.Deleted
from   dbo.RBOINVENTTABLELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and s.ITEMID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO
------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ItemTranslation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ItemTranslation

GO

create procedure dbo.spAUDIT_ViewLog_ItemTranslation
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.ITEMID,
        s.CULTURENAME,
        s.DESCRIPTION,
        s.Deleted
from   dbo.RBOINVENTTRANSLATIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ITEMID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KMPROFILES]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KMPROFILES

GO

create procedure dbo.spAUDIT_ViewLog_KMPROFILES
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.KITCHENMANAGERSERVER,
        s.KITCHENMANAGERPORT,
        s.Deleted
from   dbo.KITCHENDISPLAYTRANSACTIONPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KMPROFILE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KMPROFILE

GO

create procedure dbo.spAUDIT_ViewLog_KMPROFILE
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.KITCHENMANAGERSERVER,
        s.KITCHENMANAGERPORT,
        s.Deleted
from   dbo.KITCHENDISPLAYTRANSACTIONPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------------------------------------------------


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RBOTERMINALGROUP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RBOTERMINALGROUP

GO

create procedure dbo.spAUDIT_ViewLog_RBOTERMINALGROUP
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.DESCRIPTION,
        s.Deleted
from   dbo.RBOTERMINALGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_TERMINALGROUPCONNECTION]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_TERMINALGROUPCONNECTION

GO

create procedure dbo.spAUDIT_ViewLog_TERMINALGROUPCONNECTION
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.TERMINALGROUPID,
        s.TERMINALID,
        s.Deleted
from   dbo.RBOTERMINALGROUPCONNECTIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StyleProfiles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StyleProfiles

GO

create procedure dbo.spAUDIT_ViewLog_StyleProfiles
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.NAME,
        s.Deleted
from   dbo.POSSTYLEPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_StyleProfileLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_StyleProfileLines

GO

create procedure dbo.spAUDIT_ViewLog_StyleProfileLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.MENUID,
        s.CONTEXTID,
        s.STYLEID,
        s.SYSTEM,
        s.Deleted
from   dbo.POSSTYLEPROFILELINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.PROFILEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Context]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Context

GO

create procedure dbo.spAUDIT_ViewLog_Context
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.NAME,
        s.MENUREQUIRED,
        s.Deleted
from   dbo.POSCONTEXTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYSTATIONSLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYSTATIONSLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYSTATIONSLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.SCREENNUMBER,
        s.KITCHENDISPLAYFUNCTIONALPROFILEID,
        s.KITCHENDISPLAYSTYLEPROFILEID,
        s.KITCHENDISPLAYVISUALPROFILEID,
        s.Deleted
from   dbo.KITCHENDISPLAYSTATIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


--------------------------------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYSTATIONLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYSTATIONLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYSTATIONLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.SCREENNUMBER,
        s.KITCHENDISPLAYFUNCTIONALPROFILEID,
        s.KITCHENDISPLAYSTYLEPROFILEID,
        s.KITCHENDISPLAYVISUALPROFILEID,
        s.Deleted
from   dbo.KITCHENDISPLAYSTATIONSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


----------------------------------------------------------------------------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYITEMCONNECTIONLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYITEMCONNECTIONLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYITEMCONNECTIONLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.STATIONID,
        s.TYPE,
        s.CONNECTIONVALUE,
        s.Deleted
from   dbo.KITCHENDISPLAYITEMCONNECTIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STATIONID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYVISUALPROFILESLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYVISUALPROFILESLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYVISUALPROFILESLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.ORDERPANEWIDTH,
        s.ORDERPANEHEIGHT,
        s.ORDERPANEX,
        s.ORDERPANEY,
        s.ORDERPANEVISIBLE,
        s.BUTTONPANEWIDTH,
        s.BUTTONPANEHEIGHT,
        s.BUTTONPANEX,
        s.BUTTONPANEY,
        s.BUTTONPANEVISIBLE,
        s.NUMBEROFCOLUMNS,
        s.NUMBEROFROWS,
        s.ITEMMODIFIERINCREASEPREFIX,
        s.ITEMMODIFIERDECREASEPREFIX,
        s.ITEMMODIFIERNORMALPREFIX,
        s.SHOWDEALS,
        s.Deleted
from   dbo.KITCHENDISPLAYVISUALPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

---------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYVISUALPROFILELog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYVISUALPROFILELog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYVISUALPROFILELog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.ORDERPANEWIDTH,
        s.ORDERPANEHEIGHT,
        s.ORDERPANEX,
        s.ORDERPANEY,
        s.ORDERPANEVISIBLE,
        s.BUTTONPANEWIDTH,
        s.BUTTONPANEHEIGHT,
        s.BUTTONPANEX,
        s.BUTTONPANEY,
        s.BUTTONPANEVISIBLE,
        s.NUMBEROFCOLUMNS,
        s.NUMBEROFROWS,
        s.ITEMMODIFIERINCREASEPREFIX,
        s.ITEMMODIFIERDECREASEPREFIX,
        s.ITEMMODIFIERNORMALPREFIX,
        s.SHOWDEALS,
        s.Deleted
from   dbo.KITCHENDISPLAYVISUALPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYTERMINALCONNECTIONLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYTERMINALCONNECTIONLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYTERMINALCONNECTIONLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.STATIONID,
        s.TYPE,
        s.CONNECTIONVALUE,
		s.TERMINALID,
		s.STOREID,
        s.Deleted
from   dbo.KITCHENDISPLAYTERMINALCONNECTIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STATIONID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYHOSPITALITYCONNECTIONLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYHOSPITALITYCONNECTIONLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYHOSPITALITYCONNECTIONLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.STATIONID,
        s.TYPE,
        s.CONNECTIONVALUE,
        s.Deleted
from   dbo.KITCHENDISPLAYHOSPITALITYTYPECONNECTIONLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.STATIONID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYFUNCTIONALPROFILESLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYFUNCTIONALPROFILESLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYFUNCTIONALPROFILESLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
		s.BUMPPOSSIBLE,
        s.BUTTONSMENUID,
        s.Deleted
from   dbo.KITCHENDISPLAYFUNCTIONALPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYFUNCTIONALPROFILELog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYFUNCTIONALPROFILELog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYFUNCTIONALPROFILELog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
		s.BUMPPOSSIBLE,
        s.BUTTONSMENUID,
        s.Deleted
from   dbo.KITCHENDISPLAYFUNCTIONALPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYSTYLESPROFILESLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYSTYLESPROFILESLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYSTYLESPROFILESLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.Deleted
from   dbo.KITCHENDISPLAYSTYLEPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYSTYLESPROFILELog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYSTYLESPROFILELog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYSTYLESPROFILELog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.NAME,
        s.Deleted
from   dbo.KITCHENDISPLAYSTYLEPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_KITCHENDISPLAYMENUHEADERLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_KITCHENDISPLAYMENUHEADERLog

GO

create procedure dbo.spAUDIT_ViewLog_KITCHENDISPLAYMENUHEADERLog
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.MENUID,
        s.DESCRIPTION,
        s.COLUMNS,
        s.ROWS,
        s.MENUCOLOR,
        s.FONTNAME,
        s.FONTSIZE,
        s.FONTBOLD,
        s.FORECOLOR,
        s.BACKCOLOR,
        s.FONTITALIC,
        s.FONTCHARSET,
        s.USENAVOPERATION,
        s.APPLIESTO,
        s.BACKCOLOR2,
        s.GRADIENTMODE,
        s.SHAPE,
        s.MENUTYPE,
        s.STYLEID,
		s.DEVICETYPE,
		s.MAINMENU,
        s.Deleted
from   dbo.POSMENUHEADERLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.MENUTYPE = '2' and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UserLoginTokens]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_UserLoginTokens

GO

create procedure dbo.spAUDIT_ViewLog_UserLoginTokens
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.GUID,
        s.USERGUID,
        s.DESCRIPTION,
        s.HASH,
        s.Deleted
from   dbo.USERLOGINTOKENSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.USERGUID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventoryTransferOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventoryTransferOrders

GO
create procedure dbo.spAUDIT_ViewLog_InventoryTransferOrders
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
		s.INVENTORYTRANSFERREQUESTID,
		s.SENDINGSTOREID,
		s.RECEIVINGSTOREID,
		s.CREATIONDATE,
		s.RECEIVINGDATE,
		s.SENTDATE,
		s.RECEIVED,
		s.SENT,
		s.FETCHEDBYRECEIVINGSTORE,
		s.CREATEDBY,
        s.Deleted
from   dbo.INVENTORYTRANSFERORDERLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventoryTransferOrderLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventoryTransferOrderLines

GO
create procedure dbo.spAUDIT_ViewLog_InventoryTransferOrderLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
		s.INVENTORYTRANSFERORDERID,
		s.ITEMID,
		s.VARIANTID,
		s.UNITID,
		s.QUANTITYSENT,
		s.QUANTITYRECEIVED,
		s.SENT,
		s.PICTUREID,
		s.OMNITRANSACTIONID,
		s.OMNILINEID,
        s.Deleted
from   dbo.INVENTORYTRANSFERORDERLINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventoryTransferRequests]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventoryTransferRequests

GO
create procedure dbo.spAUDIT_ViewLog_InventoryTransferRequests
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
		s.SENDINGSTOREID,
		s.RECEIVINGSTOREID,
		s.CREATIONDATE,
		s.SENTDATE,
		s.SENT,
		s.FETCHEDBYRECEIVINGSTORE,
		s.INVENTORYTRANSFERCREATED,
		s.CREATEDBY,
		s.INVENTORYTRANSFERORDERID,
        s.Deleted
from   dbo.INVENTORYTRANSFERREQUESTLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventoryTransferRequestLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventoryTransferRequestLines

GO
create procedure dbo.spAUDIT_ViewLog_InventoryTransferRequestLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
		s.INVENTORYTRANSFERREQUESTID,
		s.ITEMID,
		s.VARIANTID,
		s.UNITID,
		s.QUANTITYREQUESTED,
		s.SENT,
        s.Deleted
from   dbo.INVENTORYTRANSFERREQUESTLINELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CustomerGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CustomerGroup

GO

create procedure dbo.spAUDIT_ViewLog_CustomerGroup
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.NAME,
        s.EXCLUSIVE,
        s.CATEGORY,
        s.PURCHASEAMOUNT,
		s.USEPURCHASELIMIT,
		s.PURCHASEPERIOD,
        s.Deleted
from   dbo.CUSTGROUPLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Category]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Category

GO

create procedure dbo.spAUDIT_ViewLog_Category
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.DESCRIPTION,
        s.Deleted
from   dbo.CUSTGROUPCATEGORYLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.DESCRIPTION = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DimensionAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DimensionAttribute

GO

create procedure dbo.spAUDIT_ViewLog_DimensionAttribute
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.DIMENSIONID,
        s.DESCRIPTION,
        s.CODE,
        s.SEQUENCE,
        s.Deleted
from   dbo.DIMENSIONATTRIBUTELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_DimensionTemplate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_DimensionTemplate

GO

create procedure dbo.spAUDIT_ViewLog_DimensionTemplate
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.DESCRIPTION,
        s.Deleted
from   dbo.DIMENSIONTEMPLATELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_RetailItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_RetailItem

GO

create procedure dbo.spAUDIT_ViewLog_RetailItem
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.MASTERID,
        s.ITEMID,
        s.HEADERITEMID,
        s.ITEMNAME,
        s.VARIANTNAME,
        s.ITEMTYPE,
        s.DEFAULTVENDORID,
        s.NAMEALIAS,
        s.RETAILGROUPMASTERID,
	    s.ZEROPRICEVALID,
        s.QTYBECOMESNEGATIVE,
        s.NODISCOUNTALLOWED,
		s.KEYINPRICE,
		s.SCALEITEM,
		s.KEYINQTY,
        s.BLOCKEDONPOS,
        s.BARCODESETUPID,
        s.PRINTVARIANTSSHELFLABELS,
        s.FUELITEM,
        s.GRADEID,
        s.MUSTKEYINCOMMENT,
        s.DATETOBEBLOCKED,
        s.DATETOACTIVATEITEM,
		s.PROFITMARGIN,
		s.VALIDATIONPERIODID,
		s.MUSTSELECTUOM,
        s.INVENTORYUNITID,
        s.PURCHASEUNITID,
        s.SALESUNITID,
        s.PURCHASEPRICE,
        s.SALESPRICE,
        s.SALESPRICEINCLTAX,
        s.SALESMARKUP,
        s.SALESLINEDISC,
        s.SALESMULTILINEDISC,
        s.SALESALLOWTOTALDISCOUNT,
        s.SALESTAXITEMGROUPID,
		s.RETURNABLE,
		s.EXTENDEDDESCRIPTION,
		s.SEARCHKEYWORDS,
        s.Deleted
from   dbo.RETAILITEMLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.MASTERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_CustomerOrderSettings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_CustomerOrderSettings

GO

create procedure dbo.spAUDIT_ViewLog_CustomerOrderSettings
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ACCEPTSDEPOSITS,
        s.MINIMUMDEPOSITS,
        s.SOURCE,
        s.DELIVERY,        
        s.EXPIRATIONTIMEVALUE,
        s.EXPIRATIONTIMEUNIT,
        s.NUMBERSERIES,
        s.Deleted
from   dbo.CUSTOMERORDERSETTINGSLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ORDERTYPE = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ImportProfile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ImportProfile

GO

create procedure dbo.spAUDIT_ViewLog_ImportProfile
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.ID,
        s.DESCRIPTION,
        s.IMPORTTYPE,
        s.[DEFAULT],
        s.Deleted
from   dbo.IMPORTPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.MASTERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_ImportProfileLines]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_ImportProfileLines

GO

create procedure dbo.spAUDIT_ViewLog_ImportProfileLines
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.IMPORTPROFILEMASTERID,
        s.FIELD,
        s.FIELDTYPE,
        s.SEQUENCE,
        s.Deleted
from   dbo.IMPORTPROFILELINESLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.MASTERID = @contextIdentifier and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_UserProfile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_UserProfile

GO

create procedure dbo.spAUDIT_ViewLog_UserProfile
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.PROFILEID,
        s.DESCRIPTION,
        s.MAXLINEDISCOUNTAMOUNT,
        s.MAXDISCOUNTPCT,
        s.MAXTOTALDISCOUNTAMOUNT,
        s.MAXTOTALDISCOUNTPCT,
        s.MAXLINERETURNAMOUNT,
        s.MAXTOTALRETURNAMOUNT,
        s.STOREID,
        s.VISUALPROFILE,
        s.LAYOUTID,
        s.KEYBOARDCODE,
        s.LAYOUTNAME,
        s.OPERATORCULTURE,
        s.Deleted
from   dbo.POSUSERPROFILELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.PROFILEID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_Customer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_Customer

GO

create procedure dbo.spAUDIT_ViewLog_Customer
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.MASTERID,
        s.ACCOUNTNUM,
        s.NAME,
        s.INVOICEACCOUNT,
        s.FIRSTNAME,
        s.MIDDLENAME,
        s.LASTNAME,
        s.NAMEPREFIX,
        s.NAMESUFFIX,
        s.CURRENCY,
        s.LANGUAGEID,
        s.TAXGROUP,
        s.PRICEGROUP,
        s.LINEDISC,
        s.MULTILINEDISC,
        s.ENDDISC,
        s.CREDITMAX,
        s.ORGID,
        s.BLOCKED,
        s.NONCHARGABLEACCOUNT,
        s.INCLTAX,
        s.PHONE,
        s.CELLULARPHONE,
        s.NAMEALIAS,
        s.CUSTGROUP,
        s.VATNUM,
        s.EMAIL,
        s.URL,
        s.TAXOFFICE,
        s.USEPURCHREQUEST,
        s.LOCALLYSAVED,
        s.GENDER,
        s.DATEOFBIRTH,
        s.RECEIPTOPTION,
        s.RECEIPTEMAIL,
        s.Deleted
from   dbo.CUSTOMERLog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ACCOUNTNUM = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spAUDIT_ViewLog_InventoryTemplate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].spAUDIT_ViewLog_InventoryTemplate

GO

create procedure dbo.spAUDIT_ViewLog_InventoryTemplate
(@dataAreaID nvarchar(10),@contextIdentifier nvarchar(255),@user nvarchar(50),@from datetime,@to datetime)
as

 --AuditUserLogin = Case
 --          when s.Login = '' then m.Login
 --          else s.Login
 --      end,

select  s.AuditID,
        s.AuditUserGUID,
        m.Login as AuditUserLogin,
        s.AuditUserLogin as AuditUserLogin2,
        s.AuditDate,
        s.NAME,
        s.CHANGEVENDORINLINE,
        s.CHANGEUOMINLINE,
        s.CALCULATESUGGESTEDQUANTITY,
        s.SETQUANTITYTOSUGGESTEDQUANTITY,
        s.DISPLAYREORDERPOINT,
        s.DISPLAYMAXIMUMINVENTORY,
        s.DISPLAYBARCODE,
        s.ALLSTORES,
        s.ADDLINESWITHZEROSUGGESTEDQTY,
        s.TEMPLATEENTRYTYPE,
        s.UNITSELECTION,
        s.ENTERINGTYPE,
        s.QUANTITYMETHOD,
        s.DEFAULTQUANTITY,
        s.DEFAULTSTORE,
        s.AUTOPOPULATEITEMS,
        s.Deleted
from   dbo.INVENTORYTEMPLATELog s
       left outer join LSPOSNET.dbo.USERS m on m.GUID = s.AuditUserGUID
where  s.ID = @contextIdentifier and s.DATAAREAID = @dataAreaID and (s.AuditUserLogin Like @user or m.Login Like @user) and s.AuditDate Between @from and @to

GO


