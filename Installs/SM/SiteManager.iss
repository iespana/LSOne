#include "..\Installer Scripts\products.iss"
#include "..\Installer Scripts\products\dotnetfxversion.iss"
#include "..\Installer Scripts\products\dotnetfx47.iss"

#include "..\Common\constants.iss"

#define APPNAME "LS One Site Manager"
#define APPEXENAME "Site Manager.exe"

[Setup]
AppId={{937076EA-ACAC-42B7-8045-B20BF4A31152}
AppName={#APPNAME}
DefaultDirName={pf}\LS Retail\Site Manager
DefaultGroupName=LS Retail\Site Manager
SetupIconFile={#SourcePath}\..\Common\images\SiteManager.ico
UninstallDisplayIcon={app}\{#APPEXENAME}

AppPublisher={#LSOneAppPublisher}
AppPublisherURL={#LSOneAppPublisherURL}
AppUpdatesURL={#LSOneAppPublisherURL}
AppSupportURL={#LSOneAppPublisherURL}
LicenseFile={#LSOneLicenseFile}
Compression={#LSOneCompression}
SolidCompression={#LSOneSolidCompression}
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


#define DATAPACKAGESFOLDER "{commonappdata}\LS Retail\Default Data"

; This one ensures that Site Manager is not running while we turn on the installer.
AppMutex=StoreController

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
DisableDirPage={#LSOneDisableUI}
DisableProgramGroupPage={#LSOneDisableUI}

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Registry]
Root: HKCR; Subkey: ".rpdsc"; ValueType: string; ValueName: ""; ValueData: "StoreControllerReportManifest"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "StoreControllerReportManifest"; ValueType: string; ValueName: ""; ValueData: "SC Report manifest"; Flags: uninsdeletekey
Root: HKCR; Subkey: "StoreControllerReportManifest\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\Site Manager.exe,1"

Root: HKCR; Subkey: ".scplug"; ValueType: string; ValueName: ""; ValueData: "StoreControllerPlugin"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "StoreControllerPlugin"; ValueType: string; ValueName: ""; ValueData: "SC Plugin"; Flags: uninsdeletekey
Root: HKCR; Subkey: "StoreControllerPlugin\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\Site Manager.exe,2"

[InstallDelete]
; Delete old style named plugins that may have been previously installed
Type: files; Name: "{app}\Plugins\LSRetail.*";
Type: files; Name: "{app}\Services\LSRetail.*";
; Deleted/unused plugins
Type: files; Name: "{app}\Plugins\LSOne.ViewPlugins.LSLicensing.*";
Type: files; Name: "{app}\Plugins\LSOne.ViewPlugins.Omni.*";

;Clean up old dlls in root.
Type: files; Name: "{app}\LSRetail.*";
Type: files; Name: "{app}\DevExpress.*";

;remove Report Viewer 10 dlls
Type: files; Name: "{app}\bin\Microsoft.ReportViewer.*.dll";

Type: files; Name: "{app}\ButtonGrid.dll";
Type: files; Name: "{app}\CCTVInterface.dll";
Type: files; Name: "{app}\EFTInterface.dll";
Type: files; Name: "{app}\TillLayoutDesigner.dll";
Type: files; Name: "{app}\TransAutomClient.dll";
Type: files; Name: "{app}\LSOne.DevUtilities.dll";
Type: files; Name: "{app}\EFTInterface.dll";
Type: files; Name: "{app}\LSMenuButtonControl.dll";
Type: files; Name: "{app}\POSProcesses.dll";
Type: files; Name: "{app}\PosSkins.dll";
Type: files; Name: "{app}\SharedControls.dll";
Type: files; Name: "{app}\SystemFramework.dll";
Type: files; Name: "{app}\SystemSettings.dll";
Type: files; Name: "{app}\ICSharpCode.SharpZipLib.dll";
Type: files; Name: "{app}\PrintingStationUtils.dll";
Type: files; Name: "{app}\TillLayoutDesigner.dll";
Type: files; Name: "{app}\PrintingStationClient.dll";

; Delete obsolete plugins and DLLs that may have been previously installed
Type: files; Name: "{app}\Services\LSRetail.Services.ExcelImporterService.dll";
Type: files; Name: "{app}\DTG.Spreadsheet.dll";
Type: files; Name: "{app}\Plugins\LSOne.ViewPlugins.ExcelImporter.scplug";
Type: files; Name: "{app}\Plugins\LSOne.ViewPlugins.Dimensions.scplug";


[Files]

; Plugins
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Administration.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Administration.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.BarCodes.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.BarCodes.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Forms.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Forms.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.LookupValues.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.LookupValues.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.POSUser.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.POSUser.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Profiles.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Profiles.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.ReceiptBrowser.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.ReceiptBrowser.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.ReportViewer.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.ReportViewer.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.RetailItems.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.RetailItems.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.SalesTax.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.SalesTax.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Store.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Store.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.TouchButtons.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.TouchButtons.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.User.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.User.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.XtraReportsViewer.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.XtraReportsViewer.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.ExcelFiles.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.ExcelFiles.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Terminals.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Terminals.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.UserInterfaceStyles.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.UserInterfaceStyles.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Backup.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Backup.scplug"; Flags: ignoreversion
Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.VariantFramework.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.VariantFramework.scplug"; Flags: ignoreversion

; POS Plugins
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\POSPlugins\LSPay.POSPlugin.dll"; DestDir:{app}\POSPlugins; DestName:"LSPay.POSPlugin.dll"; Flags: ignoreversion

#if "BASIC" != Type
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.CentralSuspensions.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.CentralSuspensions.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.CreditVouchers.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.CreditVouchers.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Customer.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Customer.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.CustomerOrders.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.CustomerOrders.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.CustomerLedger.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.CustomerLedger.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.CustomerLoyalty.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.CustomerLoyalty.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.GiftCards.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.GiftCards.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Hospitality.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Hospitality.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Infocodes.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Infocodes.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Inventory.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Inventory.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.LabelPrinting.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.LabelPrinting.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.PeriodicDiscounts.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.PeriodicDiscounts.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.PrintingStation.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.PrintingStation.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Replenishment.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Replenishment.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.Scheduler.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.Scheduler.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.SiteService.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.SiteService.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.TaxRefund.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.TaxRefund.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.TradeAgreements.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.TradeAgreements.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.USConfigurations.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.USConfigurations.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.EndOfDay.dll"; DestDir:{app}\Plugins; DestName:"LSRetail.SiteManage.Plugins.EndOfDay.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.SaleByPaymentMediaReport.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.SaleByPaymentMediaReport.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.AppExchange.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.AppExchange.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.SerialNumbers.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.SerialNumbers.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.LSCommerce.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.LSCommerce.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.PaymentLimitations.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.PaymentLimitations.scplug"; Flags: ignoreversion
  Source: "{#SourceDir}\Plugins\LSOne.ViewPlugins.RetailItemAssemblies.dll"; DestDir:{app}\Plugins; DestName:"LSOne.ViewPlugins.RetailItemAssemblies.scplug"; Flags: ignoreversion
#endif

; Templates
Source: "{#SourceDir}\Templates\*.xlsx"; DestDir:{app}\Templates; Flags: ignoreversion

; SVGs
Source: "{#SourceDir}\SVGs\*.svg"; DestDir:{app}\SVGs; Flags: ignoreversion

; All language resource files
Source: "{#SourceDir}\*.resources.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; Support dll's from POS
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\LSOne.Controls.ButtonGrid.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Services.Interfaces.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.ReportDesigner.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.POS.Processes.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\SystemFramework.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.SiteManager.Utilities.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.TillLayoutDesigner.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.MenuButton.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSRetail.SiteService.SiteServiceInterface.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.POS.Core.dll"; DestDir:{app}; Flags: ignoreversion

; Database management
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\LSOne.DataLayer.DatabaseUtil.dll"; DestDir:{app}; Flags: ignoreversion

; Site Manager data layer
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\LSOne.DataLayer.GenericConnector.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.SqlConnector.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.BusinessObjects.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.TransactionObjects.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.DDBusinessObjects.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.DataProviders.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.TransactionDataProviders.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.DDDataProviders.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.SqlDataProviders.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.SqlTransactionDataProviders.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.DataLayer.SqlDDDataProviders.dll"; DestDir:{app}; Flags: ignoreversion

; Site Manager shared components
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\LSOne.Controls.ContextBar.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.DropDownForm.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.OperationPanel.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.Shared.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.WizardOptionButton.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.DataControls.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.ViewCore.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.ViewCore.Dialogs.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Utilities.dll"; DestDir:{app}; Flags: ignoreversion; AfterInstall: DeleteObsoleteDLLs
Source: "{#SourceDir}\LSOne.Controls.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.SearchBar.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.ScrollBar.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.ListView.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.ListViewExtensions.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSOne.Controls.POS.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\System.Drawing.Html.dll"; DestDir:{app}; Flags: ignoreversion

; Site Manager services
Source: "{#SourceDir}\LSOne.Services.Interfaces.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\LSRetail.SiteService.IntegrationFrameworkBaseInterface.dll"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Rounding.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Calculation.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Tax.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Excel.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.SiteService.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.EndOfDayBackOffice.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Inventory.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Barcode.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Label.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.DD.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.License.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Migration.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Backup.dll"; DestDir:{app}\Services; Flags: ignoreversion
Source: "{#SourceDir}\Services\LSOne.Services.Application.dll"; DestDir:{app}\Services; Flags: ignoreversion

; Licensing
Source: "{#SourceDir}\LSRetail.Licensing.Common.dll"; DestDir:{app}; Flags: ignoreversion

; 3rd party components
Source: "{#SourceDir}\bin\*.*"; DestDir:{app}\bin; Flags: ignoreversion

; Site Manager executables
; ----------------------------------------------------------------------------------------------------------------------------------
Source: "{#SourceDir}\Site Manager.exe"; DestDir:{app}; Flags: ignoreversion
Source: "{#SourceDir}\Site Manager.exe.config"; DestDir:{app}; Flags: ignoreversion

; Help files
;ignore if DocDir does not exist or in DEBUG mode and source files do not exist
; ----------------------------------------------------------------------------------------------------------------------------------
#if DirExists(DocDir)
  #if RPos("-DEBUG", SetupSuffix) == 0
    Source: "{#DocDir}\*.*"; DestDir: "{app}\Help"; Flags: ignoreversion recursesubdirs
  #else
    Source: "{#DocDir}\*.*"; DestDir: "{app}\Help"; Flags: ignoreversion recursesubdirs skipifsourcedoesntexist
  #endif

  Source: "{#ExtraDir}\SetupMadCapHelpViewer0630.msi"; DestDir:{app}\Install; Flags: ignoreversion;
#else
  #pragma message "DocDir: " + SQL + " was not found. Skipping help files..."
#endif

; INSTALL FILES
#if "yes" == SQL
	Source: "{#SQLServerDir}\{#SQLServerSubDir}\{#SQLServerExe}"; DestDir:{app}\Install; Flags: ignoreversion nocompression;
#endif

; DATA PACKAGES
; Here you can add your own data packages that will be installed with Site Manager, which can later be imported into Site Manager
; Example:
;
; Source: "MyDataPackages\package1.xml"; DestDir:{#DATAPACKAGESFOLDER}; Flags: ignoreversion;
[Dirs]
Name: "{#DATAPACKAGESFOLDER}"; Permissions: users-modify;

[Icons]
#if "EXPRESS" == Type
	Name: "{group}\Site Manager"; Filename: "{app}\Site Manager.exe"; Parameters: "-cloud"
  Name: "{commondesktop}\Site Manager"; Filename: "{app}\Site Manager.exe" ; Tasks: desktopicon; Parameters: "-cloud"
#else
	Name: "{group}\Site Manager"; Filename: "{app}\Site Manager.exe"
  Name: "{commondesktop}\Site Manager"; Filename: "{app}\Site Manager.exe" ; Tasks: desktopicon
#endif
Name: "{group}\{cm:UninstallProgram,Site Manager}"; Filename: "{uninstallexe}"

[Run] 
#if "EXPRESS" == Type
	Filename: "{app}\Site Manager.exe"; Description: "{cm:LaunchProgram,Site Manager}"; Flags: nowait postinstall skipifsilent unchecked; Parameters: "-cloud"
#else
	Filename: "{app}\Site Manager.exe"; Description: "{cm:LaunchProgram,Site Manager}"; Flags: nowait postinstall skipifsilent unchecked
#endif

[Code]

procedure DeleteObsoleteDLLs();
begin
  // Unload the DLL
  UnloadDLL(ExpandConstant('{app}\DevUtilities.dll'));
  // Now we can delete the DLL
  DeleteFile(ExpandConstant('{app}\DevUtilities.dll'));
end;
