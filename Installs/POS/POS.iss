#include "..\Installer Scripts\products.iss"

#include "..\Installer Scripts\products\stringversion.iss"
#include "..\Installer Scripts\products\winversion.iss"
#include "..\Installer Scripts\products\fileversion.iss"
#include "..\Installer Scripts\products\dotnetfxversion.iss"

#include "..\Common\constants.iss"
#include "..\Common\utils.iss"

#define APPNAME "LS One POS"
#define APPEXENAME "LS One POS.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{9E9C3C4D-EFB2-41A1-B217-587D046BC953}
AppName={#APPNAME}
DefaultDirName={pf}\LS Retail\{#APPNAME}
DefaultGroupName=LS Retail\{#APPNAME}
SetupIconFile={#SourcePath}\..\Common\images\POS.ico
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

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "{#SourceDir}\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs; AfterInstall: DeleteObsoleteDLLs

;;;;;;;   POS EXECUTABLES
Source: "{#SourceDir}\*.exe"; DestDir:{app}; Flags: ignoreversion recursesubdirs; AfterInstall: CopyBusinessDayFiles
Source: "{#SourceDir}\*.config"; DestDir: "{app}"; Flags: ignoreversion

;;;;;;;   INSTALL FILES
Source: "{#SourcePath}\..\Common\images\lsposlogo.jpg"; DestDir:{app}\Install; Flags: ignoreversion
Source: "{#SourcePath}\..\Common\libs\OPOS_CCOs_1.14.001.msi"; DestDir:{app}\Install; Flags: ignoreversion

#if "yes" == SQL
	Source: "{#SQLServerDir}\{#SQLServerSubDir}\{#SQLServerExe}"; DestDir:{app}\Install; Flags: ignoreversion nocompression;
#endif

[Icons]
#if "EXPRESS" == Type
  Name: "{group}\LS One POS"; Filename: "{app}\LS One POS.exe"; Parameters: "-cloud"
  Name: {commondesktop}\LS One POS; Filename: {app}\LS One POS.exe; Tasks: desktopicon; Parameters: "-cloud"
#else
	Name: "{group}\LS One POS"; Filename: "{app}\LS One POS.exe"
	Name: {commondesktop}\LS One POS; Filename: {app}\LS One POS.exe; Tasks: desktopicon
#endif
Name: "{group}\LS One POS (Maintenance Mode)"; Filename: "{app}\LS One POS.exe"; Parameters: "-se"
Name: "{group}\{cm:UninstallProgram,LS One POS}"; Filename: "{uninstallexe}"

[Run]
#if "EXPRESS" == Type
  Filename: "{app}\LS One POS.exe"; Description: "{cm:LaunchProgram,LS One POS}";Parameters: "-cloud"; Flags: nowait postinstall skipifsilent unchecked
#else
	Filename: "{app}\LS One POS.exe"; Description: "{cm:LaunchProgram,LS One POS}"; Flags: nowait postinstall skipifsilent unchecked
#endif


[InstallDelete]
;Delete Printing Station files from parent folder before install
;Installs after version 2016 have these files moved to the bin folder
Type: files; Name: {app}\PrintingStationClient.dll
Type: files; Name: {app}\PrintingStationUtils.dll
Type: files; Name: {app}\LSOne.DevUtilities.dll

[Code]
procedure CopyBusinessDayFiles();
var
businessDayPath: String;
businessSystemDayPath: String;
destinationFolder: String;
begin
  businessDayPath := ExpandConstant('{commonappdata}') + '\LS Retail\LS One POS\BusinessDay';
  businessSystemDayPath := ExpandConstant('{commonappdata}') + '\LS Retail\LS One POS\BusinessSystemDay';
  destinationFolder := ExpandConstant('{commonappdata}') + '\LS Retail\LS POS'; 

  if FileExists(businessDayPath) then begin
    FileCopy(businessDayPath,  destinationFolder + '\BusinessDay', False);
  end;

  if FileExists(businessSystemDayPath) then begin
    FileCopy(businessSystemDayPath,  destinationFolder + '\BusinessSystemDay', False);
  end;
end;

procedure DeleteObsoleteDLLs();
begin
  // Unload the DLL
  UnloadDLL(ExpandConstant('{app}\LSOne.DevUtilities.dll'));

  // Now we can delete the DLL
  DeleteFile(ExpandConstant('{app}\LSOne.DevUtilities.dll'));
end;