#include "..\Common\constants.iss"
#include "..\Common\utils.iss"

#define APPNAME "LS One to SAP Business One Integration Package"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{79564676-1875-4CC4-8970-A7A158E19046}
AppName={#APPNAME}
DefaultDirName={pf}\LS Retail\Site Manager
DefaultGroupName=LS Retail\Site Manager
SetupIconFile={#SourcePath}\..\Common\images\SiteManager.ico
UninstallDisplayIcon={app}\Site Manager.exe

AppPublisher={#LSOneAppPublisher}
AppPublisherURL={#LSOneAppPublisherURL}
AppUpdatesURL={#LSOneAppPublisherURL}
AppSupportURL={#LSOneAppPublisherURL}
LicenseFile={#LSOneLicenseFile}
Compression={#LSOneCompression}
SolidCompression=no
MinVersion={#LSOneMinVersion}
WizardSmallImageFile={#LSOneWizardSmallImageFile}
WizardImageFile={#LSOneWizardImageFile}
OutputDir={#LSOneOutputDir}
OutputBaseFilename={#LSOneOutputBaseFilename}

#define ApplicationVersion GetFileVersion(SourceDir + '\SiteManager\bin\LSOne.Connectors.SAPBusinessOne.Core.dll')
AppVerName={#APPNAME} {#ApplicationVersion}
AppVersion={#ApplicationVersion}
VersionInfoVersion={#ApplicationVersion}
VersionInfoProductVersion={#ApplicationVersion}
VersionInfoTextVersion={#ApplicationVersion}

#ifdef BuildServerBuildNumber
  VersionInfoProductTextVersion={#ApplicationVersion} - Build {#BuildServerBuildNumber}
#endif

CloseApplications=no

; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64

;starting with InnoSetup 5.5.7, As recommended by Microsoft's desktop applications guideline, 
;DisableWelcomePage now defaults to yes. 
;Additionally DisableDirPage and DisableProgramGroupPage now default to auto. The defaults in all previous versions were no.
;see http://www.jrsoftware.org/files/is5.5-whatsnew.htm#5.5.7
DisableWelcomePage={#LSOneDisableUI}
DisableDirPage=yes
DisableProgramGroupPage=yes

#define DATAPACKAGESFOLDER "{commonappdata}\LS Retail\Default Data"

#define DataPackSourceDir "..\..\..\Connectors\SAPB1Connector\LSOne.Connectors.SAPBusinessOne.Core\DataLayer\ImportData"
#define SourceDir "..\..\..\Connectors\SAPB1Connector\Build"
#define SMSourceDir "..\..\SM\Build"
#define SSSourceDir "..\..\SiteService\Build"

[CustomMessages]
SelectionFolderPage_Caption=LS One plugins for SAP Business One
SelectionFolderPage_Description=
SelectionFolderPage_Subcaption=Select which plugins to install.
FoldersPage_Caption=LS One Location
FoldersPage_Description=
FoldersPage_Subcaption=Select location where LS One applications are installed.
Message_EmptyFolders=You have not selected any installation path for one or more applications.

[Dirs]
Name: "{code:GetSiteManagerInstallDir}"; Check: SiteManagerSelected()
Name: "{code:GetDD3InstallDir}"; Check: DD3Selected()
Name: "{code:GetSSInstallDir}"; Check: SiteServiceSelected()


[Files]
Source: "UpdateSAPOverride.ps1"; Flags: dontcopy

; Plugins
; ----------------------------------------------------------------------------------------------------------------------------------
;SiteManager
Source: "{#SourceDir}\SiteManager\LSOne.Connectors.SAPBusinessOne.Plugin.dll"; DestDir:"{code:GetSiteManagerInstallDir}\Plugins"; DestName:"LSOne.Connectors.SAPBusinessOne.Plugin.scplug"; Flags: ignoreversion; Check: CanInstallSiteManager()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.PluginComponents.dll"; DestDir:"{code:GetSiteManagerInstallDir}\bin"; Flags: ignoreversion; Check: CanInstallSiteManager()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.Core.dll"; DestDir:"{code:GetSiteManagerInstallDir}\bin"; Flags: ignoreversion; Check: CanInstallSiteManager()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.SAPService.dll"; DestDir:"{code:GetSiteManagerInstallDir}\bin"; Flags: ignoreversion; Check: CanInstallSiteManager()

; All language resource files
 Source: "{#SourceDir}\SiteManager\*.resources.dll"; DestDir: "{code:GetSiteManagerInstallDir}"; Flags: ignoreversion recursesubdirs skipifsourcedoesntexist

; DATA PACKAGES
Source: "{#DataPackSourceDir}\SAP Business One Connector demo data.xml"; DestDir:{#DATAPACKAGESFOLDER}; Flags: ignoreversion;
Source: "{#DataPackSourceDir}\SAP Business One Connector US demo data.xml"; DestDir:{#DATAPACKAGESFOLDER}; Flags: ignoreversion;

;SiteService
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.SiteServicePlugin.dll"; DestDir: "{code:GetSSInstallDir}\Plugins"; Flags: ignoreversion; Check: CanInstallSiteService()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.IntegrationFrameworkSAPPlugin.dll"; DestDir: "{code:GetSSInstallDir}\Plugins"; Flags: ignoreversion; Check: CanInstallSiteService()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.Core.dll"; DestDir: "{code:GetSSInstallDir}\bin"; Flags: ignoreversion; Check: CanInstallSiteService()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.SAPService.dll"; DestDir: "{code:GetSSInstallDir}\bin"; Flags: ignoreversion; Check: CanInstallSiteService()

;DD3
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.DataDirectorPlugin.dll"; DestDir: "{code:GetDD3InstallDir}\bin\plugins"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.Core.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SourceDir}\LSOne.Connectors.SAPBusinessOne.SAPService.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.Utilities.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.Services.Interfaces.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.SqlDataProviders.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.SqlConnector.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.GenericConnector.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.DataProviders.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.BusinessObjects.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.SqlTransactionDataProviders.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.TransactionDataProviders.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\LSOne.DataLayer.TransactionObjects.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SMSourceDir}\bin\LSOne.DataLayer.KDSBusinessObjects.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()
;DD3 - required by ServiceLayer
Source: "{#SSSourceDir}\Services\LSOne.Services.Tax.dll"; DestDir: "{code:GetDD3InstallDir}\bin\Services"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SSSourceDir}\Services\LSOne.Services.Rounding.dll"; DestDir: "{code:GetDD3InstallDir}\bin\Services"; Flags: ignoreversion; Check: CanInstallDD3()
Source: "{#SSSourceDir}\LSRetail.SiteService.IntegrationFrameworkBaseInterface.dll"; DestDir: "{code:GetDD3InstallDir}\bin"; Flags: ignoreversion; Check: CanInstallDD3()

[Code]
var
	SelectionFolderPage: TInputOptionWizardPage;
	FoldersPage: TInputDirWizardPage;
	SiteManagerInstallDir: String;
	DD3InstallDir: String;
	SiteServiceInstallDir: String;
	Old_WizardForm_NextButton_OnClick: TNotifyEvent;
	IsDD3PluginSelected: Boolean;
	IsSiteServicePluginSelected: Boolean;
	IsSiteManagerPluginSelected: Boolean;

procedure UpdateSAPOverride();
var
	ErrorCode: Integer;
begin
	ExtractTemporaryFile('UpdateSAPOverride.ps1');
	ShellExec('open', 'PowerShell', 'PowerShell Set-ExecutionPolicy Bypass -Scope CurrentUser -Force', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
	ShellExec('open', 'PowerShell', ExpandConstant('-File "{tmp}\UpdateSAPOverride.ps1"'), '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
end;

procedure StartDDService();
var
	ErrorCode: Integer;
begin
	StartService('DDService', ErrorCode);
end;

procedure StartLSOneSiteService();
var
	ErrorCode: Integer;
begin
	UpdateSAPOverride();
	StartService('LSOneSiteService', ErrorCode);
end;

//
// Installer Functions
//
function InitializeSetup(): Boolean;
begin
	DD3InstallDir := '';
	SiteManagerInstallDir := '';
	SiteServiceInstallDir := '';
	
	Result := True;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
	hDDService: HANDLE;
	ddServiceStatus: SERVICE_STATUS;
	hLSOneSiteService: HANDLE;
	lsOneSiteServiceStatus: SERVICE_STATUS;
	needToStopServices: Boolean;
begin
	if (CurStep = ssInstall) and (IsDD3PluginSelected or IsSiteServicePluginSelected) then
	begin
		hDDService := GetServiceHandle('DDService');
		hLSOneSiteService := GetServiceHandle('LSOneSiteService');

		needToStopServices := false;

		if (hDDService <> 0) and IsDD3PluginSelected then
		begin
			QueryServiceStatus(hDDService, ddServiceStatus);

			needToStopServices := needToStopServices or (ddServiceStatus.dwCurrentState <> SERVICE_STOPPED);
		end;
		
		if (hLSOneSiteService <> 0) and IsSiteServicePluginSelected then
		begin
			QueryServiceStatus(hLSOneSiteService, lsOneSiteServiceStatus);

			needToStopServices := needToStopServices or (lsOneSiteServiceStatus.dwCurrentState <> SERVICE_STOPPED);
		end;

		if needToStopServices then
		begin
			if MsgBox('In order to continue the installation, we need to stop the LS One services and all the jobs that might be running. Do you want to continue the installation?', mbConfirmation, MB_YESNO) = IDYES then
			begin
				if IsDD3PluginSelected then
				begin
					WizardForm.StatusLabel.Caption := 'Stopping LS Data Director 3 Service...';

					StopService(hDDService);
				end;

				if IsSiteServicePluginSelected then
				begin
					WizardForm.StatusLabel.Caption := 'Stopping LS One Site Service...';

					StopService(hLSOneSiteService);
				end;
			end
			else
			begin
				WizardForm.StatusLabel.Caption := 'Stopping the installation...';

				CloseServiceHandle(hDDService);
				CloseServiceHandle(hLSOneSiteService);
				Abort();
				exit;
			end;
		end;

		CloseServiceHandle(hDDService);
		CloseServiceHandle(hLSOneSiteService);
	end;

	if (CurStep = ssPostInstall) and IsDD3PluginSelected then
	begin
		WizardForm.StatusLabel.Caption := 'Starting LS Data Director 3 Service...';
		StartDDService();
	end;

	if (CurStep = ssPostInstall) and IsSiteServicePluginSelected then
	begin
		WizardForm.StatusLabel.Caption := 'Starting LS One Site Service...';
		StartLSOneSiteService();
	end;
end;

procedure DeinitializeSetup();
begin
	if IsDD3PluginSelected then
		StartDDService();

	if IsSiteServicePluginSelected then
		StartLSOneSiteService();
end;

procedure DisableNextButton();
begin
  WizardForm.NextButton.Enabled := False;
  WizardForm.Update;
end;

procedure EnableNextButton();
begin
  WizardForm.NextButton.Enabled := True;
  WizardForm.Update;
end;

procedure ClickCheckEvent(Sender : TObject);
begin
	if SelectionFolderPage.Values[0] or SelectionFolderPage.Values[1] or SelectionFolderPage.Values[2] then
	begin
		EnableNextButton();
	end
	else
	begin
		DisableNextButton();
	end;

	IsSiteManagerPluginSelected := SelectionFolderPage.Values[0];
	IsSiteServicePluginSelected := SelectionFolderPage.Values[1];
	IsDD3PluginSelected := SelectionFolderPage.Values[2];
end;

procedure WizardForm_NextButton_OnClick(Sender: TObject);
var
	AnyDirEmpty: Boolean;
begin
	Case WizardForm.CurPageID of
		SelectionFolderPage.ID:
			begin
				FoldersPage.Buttons[0].Enabled := SelectionFolderPage.Values[0];
				FoldersPage.Buttons[1].Enabled := SelectionFolderPage.Values[1];
				FoldersPage.Buttons[2].Enabled := SelectionFolderPage.Values[2];

				Old_WizardForm_NextButton_OnClick(Sender);
			end;
		FoldersPage.ID:
			begin
				SiteManagerInstallDir := FoldersPage.Edits[0].Text;
				SiteServiceInstallDir := FoldersPage.Edits[1].Text;
				DD3InstallDir := FoldersPage.Edits[2].Text;

				if FoldersPage.Buttons[0].Enabled and (SiteManagerInstallDir = '') then
				begin
					AnyDirEmpty := True;
				end;
				
				if FoldersPage.Buttons[1].Enabled and (SiteServiceInstallDir = '') then
				begin
					AnyDirEmpty := True;
				end;
				
				if FoldersPage.Buttons[2].Enabled and (DD3InstallDir = '') then
				begin
					AnyDirEmpty := True;
				end;

				if AnyDirEmpty then
				begin
					MsgBox(CustomMessage('Message_EmptyFolders'), mbInformation, MB_OK);
				end
				else
				begin
					if not FoldersPage.Buttons[0].Enabled then
					begin
						FoldersPage.Edits[0].Text := 'C:\';
					end;
					
					if not FoldersPage.Buttons[1].Enabled then
					begin
						FoldersPage.Edits[1].Text := 'C:\';
					end;
					
					if not FoldersPage.Buttons[2].Enabled then
					begin
						FoldersPage.Edits[2].Text := 'C:\';
					end;

					Old_WizardForm_NextButton_OnClick(Sender);
				end;
			end;
		else
			begin
				Old_WizardForm_NextButton_OnClick(Sender);
				ClickCheckEvent(Sender);
			end;
	end;
end;

procedure InitializeWizard();
begin
	SelectionFolderPage := CreateInputOptionPage(wpLicense,
							CustomMessage('SelectionFolderPage_Caption'),
							CustomMessage('SelectionFolderPage_Description'),
							CustomMessage('SelectionFolderPage_Subcaption'),
							False,
							False);

	SelectionFolderPage.Add('Site Manager');
	SelectionFolderPage.Add('Site Service');
	SelectionFolderPage.Add('Data Director');

	SelectionFolderPage.CheckListBox.OnClickCheck := @ClickCheckEvent;

	FoldersPage := CreateInputDirPage(SelectionFolderPage.ID,
							CustomMessage('FoldersPage_Caption'),
							CustomMessage('FoldersPage_Description'),
							CustomMessage('FoldersPage_Subcaption'),
							False,
							'New Folder');

	FoldersPage.Add('Site Manager Location');
	FoldersPage.Values[0] := GetPreviousData('SMPath', SiteManagerInstallDir);
	FoldersPage.Add('Site Service Location');
	FoldersPage.Values[1] := GetPreviousData('SSPath', SiteServiceInstallDir);
	FoldersPage.Add('Data Director Location');
	FoldersPage.Values[2] := GetPreviousData('DDPath', DD3InstallDir);
	
	// disable text fields, only allow user to browse to existing folders
	FoldersPage.Edits[0].Enabled := False;
	FoldersPage.Edits[1].Enabled := False;
	FoldersPage.Edits[2].Enabled := False;

	// override wizard NextButton click
	Old_WizardForm_NextButton_OnClick := WizardForm.NextButton.OnClick;
	WizardForm.NextButton.OnClick := @WizardForm_NextButton_OnClick;
end;

function GetSiteManagerInstallDir(Param: String): String;
begin
	Result := SiteManagerInstallDir;
end;

function GetDD3InstallDir(Param: String): String;
begin
	Result := DD3InstallDir;
end;

function GetSSInstallDir(Param: String): String;
begin
	Result := SiteServiceInstallDir;
end;

function CanInstallSiteManager(): Boolean;
begin
	Result := (SelectionFolderPage.Values[0] and DirExists(ExpandConstant('{code:GetSiteManagerInstallDir}')));
end;

function CanInstallSiteService(): Boolean;
begin
	Result := (SelectionFolderPage.Values[1] and DirExists(ExpandConstant('{code:GetSSInstallDir}')));
end;

function CanInstallDD3(): Boolean;
begin
	Result := (SelectionFolderPage.Values[2] and DirExists(ExpandConstant('{code:GetDD3InstallDir}')));
end;

function SiteManagerSelected(): Boolean;
begin
	Result := IsSiteManagerPluginSelected;
end;

function DD3Selected(): Boolean;
begin
	Result := IsDD3PluginSelected;
end;

function SiteServiceSelected(): Boolean;
begin
	Result := IsSiteServicePluginSelected;
end;

procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
  // store chosen directories for the next run of the setup 
  SetPreviousData(PreviousDataKey, 'SMPath', SiteManagerInstallDir);
  SetPreviousData(PreviousDataKey, 'SSPath', SiteServiceInstallDir);
  SetPreviousData(PreviousDataKey, 'DDPath', DD3InstallDir);
end;