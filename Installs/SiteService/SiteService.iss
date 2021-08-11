#include "..\Common\constants.iss"
#include "..\Common\utils.iss"

#define APPNAME "LS One Site Service"
#define APPEXENAME "LSOneSiteService.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{ADD76A99-BDFD-4BD4-95D4-FE4CEA2A8869}
AppName={#APPNAME}
DefaultDirName={pf}\LS Retail\{#APPNAME}
DefaultGroupName=LS Retail\{#APPNAME}
SetupIconFile={#SourcePath}\..\Common\images\SiteService.ico
UninstallDisplayIcon={app}\{#APPEXENAME}

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

#define ApplicationVersion GetFileVersion(SourceDir + '\' + APPEXENAME)
AppVersion={#ApplicationVersion}
AppVerName={#APPNAME} {#ApplicationVersion}
VersionInfoVersion={#ApplicationVersion}
VersionInfoProductVersion={#ApplicationVersion}
VersionInfoTextVersion={#ApplicationVersion}

#ifdef BuildServerBuildNumber
  VersionInfoProductTextVersion={#ApplicationVersion} - Build {#BuildServerBuildNumber}
#endif


;starting with InnoSetup 5.5.7, As recommended by Microsoft's desktop applications guideline, 
;DisableWelcomePage now defaults to yes. 
;Additionally DisableDirPage and DisableProgramGroupPage now default to auto. The defaults in all previous versions were no.
;see http://www.jrsoftware.org/files/is5.5-whatsnew.htm#5.5.7
DisableWelcomePage={#LSOneDisableUI}
DisableDirPage={#LSOneDisableUI}
DisableProgramGroupPage={#LSOneDisableUI}

[Files]
; the 32-bit version of LSOne.Utilities.dll is required for the hashing algorithm during the install process
Source: "{#SourceDir}\x86\LSOne.Utilities.dll"; DestName: "External.LSOne.Utilities.dll"; Flags: dontcopy

Source: "{#SourceDir}\{#APPEXENAME}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\{#APPEXENAME}.config"; DestDir: "{app}"; Flags: ignoreversion

Source: "{#SourceDir}\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\plugins\*.dll"; DestDir: "{app}\Plugins"; Flags: ignoreversion

; Services
source: "{#SourceDir}\services\*.dll"; DestDir: "{app}\Services"; Flags: ignoreversion

Source: "{#SourceDir}\bin\Quartz.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "{#SourceDir}\bin\LSOne.DataLayer.KDSBusinessObjects.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "{#SourceDir}\bin\Common.Logging.dll"; DestDir: "{app}\bin"; Flags: ignoreversion

;configuration file
Source: "..\Common\configs\SS\LS One Site Service.config"; DestDir: "{commonappdata}\LS Retail\LS One Site Service"; Flags: onlyifdoesntexist uninsneveruninstall; Permissions: everyone-full

[InstallDelete]
Type: files; Name: "{app}\DevUtilities.dll";

[UninstallDelete]
Type: dirifempty; Name: "{commonappdata}\LS Retail\LS One Site Service"

[Run]
Filename: "{app}\{#APPEXENAME}"; Parameters: "/install"

[UninstallRun]
Filename: "{app}\{#APPEXENAME}"; Parameters: "/uninstall"

[CustomMessages]
PasswordPage_Caption=LSOne Site Service administration password
PasswordPage_Description=
PasswordPage_Text=Please enter administration password for LS One Site Service that will be required when remote configuring it from LS One Site Manager

[Code]
(* Description: Exposes the hash function LSOneComputePassword from LSOne.Utilities.dll to the installer *)
procedure ComputePassword(plainText: WideString; hashAlgorithm: WideString; hashSalt: WideString; out hashResult: WideString);
external 'LSOneComputePassword@files:External.LSOne.Utilities.dll stdcall setuponly';

const
  HASH_SALT = '{#HashSalt}';
  HASH_ALGORITHM = '{#Crypto}';

var
  PasswordPage: TInputQueryWizardPage;
  administrativeHash: WideString;

procedure UpdateSSConfig();
var
  configFilePath: String;
begin
  configFilePath := ExpandConstant('{commonappdata}') + '\LS Retail\LS One Site Service\LS One Site Service.config';
  Log('Updating Site Service configuration file: ' + configFilePath);

  UpdateConfigFile(configFilePath, 'Administration', administrativeHash);
end;

procedure InitializeWizard();
begin
  PasswordPage := CreateInputQueryPage(wpReady,
                                      CustomMessage('PasswordPage_Caption'),
                                      CustomMessage('PasswordPage_Description'),
                                      CustomMessage('PasswordPage_Text'));
  PasswordPage.Add('Password:', True);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  plainText: WideString;
begin
  // Validate certain pages before allowing the user to proceed
  if (CurPageID = PasswordPage.ID) and not (WizardSilent) then begin
    plainText := Trim(PasswordPage.Values[0]);
    if plainText = '' then begin
      MsgBox('You must enter a password to be able to configure LS One Site Service.', mbError, MB_OK);
      Result := False;
    end else begin
        Log('HashSalt: ' + HASH_SALT + ', crypto: ' + HASH_ALGORITHM);
        try
          //hash the password and save it to LSOne Site Service config file
          ComputePassword(plainText, HASH_ALGORITHM, HASH_SALT, administrativeHash);
          Result := True;
        except
          ShowExceptionMessage();
          Result := False;
        end;
    end;
  end else
    Result := True;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  hLSOneSiteService: HANDLE;
	lsOneSiteServiceStatus: SERVICE_STATUS;
	needToStopServices: Boolean;
  ErrorCode: Integer;
begin
  if CurStep = ssInstall then
	begin
		hLSOneSiteService := GetServiceHandle('LSOneSiteService');
		needToStopServices := false;
		
		if hLSOneSiteService <> 0 then
		begin
      Log('Service LSOneSiteService exists. Need to check its state...');
			QueryServiceStatus(hLSOneSiteService, lsOneSiteServiceStatus);

			needToStopServices := needToStopServices or (lsOneSiteServiceStatus.dwCurrentState <> SERVICE_STOPPED);
    end
    else
    begin
      Log('Service LSOneSiteService does not exist.');
		end;

		if needToStopServices then
		begin
      Log('Service LSOneSiteService exists. Need to stop it...');
			if MsgBox('In order to continue the installation, we need to stop the LS One Site Service. Do you want to continue the installation?', mbConfirmation, MB_YESNO) = IDYES then
			begin
        WizardForm.StatusLabel.Caption := 'Stopping LS One Site Service...';
        StopService(hLSOneSiteService);
			end
			else
			begin
				WizardForm.StatusLabel.Caption := 'Stopping the installation...';
				CloseServiceHandle(hLSOneSiteService);
				Abort();
				exit;
			end;
		end;

		CloseServiceHandle(hLSOneSiteService);
	end;

  if CurStep = ssPostInstall then begin
    try
      Log('Post Install: Unload LSOne.Utilities');
      UnloadDLL(ExpandConstant('{tmp}\External.LSOne.Utilities.dll'));
      DeleteFile(ExpandConstant('{tmp}\External.LSOne.Utilities.dll'));
      
      Log('Post Install: Update SiteService configuration file');
      UpdateSSConfig();

      Log('Start LSOneSiteService service');
      WizardForm.StatusLabel.Caption := 'Starting LS One Site Service...';
		  StartService('LSOneSiteService', ErrorCode);
    except
      ShowExceptionMessage();
    end;
  end;
end;

procedure DeinitializeSetup();
var
  ErrorCode: Integer;
begin
		StartService('LSOneSiteService', ErrorCode);
end;