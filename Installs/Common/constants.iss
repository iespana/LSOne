;common settings for all LSOne installers
;expected settings:
;   Type               - type of the installer: Basic or Express
;   ReleaseNumber      - LSOne release number, like '2017.1' or '2019'
;   SourceDir          - the directory where the source files will be retrieved
;   SQLServerDir       - the directory where the SQL Server 2014 or later will be retrieved
;   ExtraDir           - the directory where additional files or installers will be retrieved
;   ForecourtDir       - the directory where the Forecourt Manager installers/files will be retrieved
;   OutputDir          - the destination folder for the created installer
;   SetupName          - name of the created installer
;   SQL                - if it should include SQL Server installers or not: 'yes' or 'no'
;                      - default: n (no)
;   SQLVersion         - version of the SQL Server to be included (the installer will look into a 'SQL2014' folder to get the SQL Server installers if SQLVersion is '2014')
;                      - default: 2014

;Minimum Windows version on which LSOne applications run is Windows 7 SP1, see https://docs.microsoft.com/en-us/dotnet/framework/get-started/system-requirements

;for debugging purposes
#pragma option -v+
#pragma verboselevel 9

;define constants here because overriding directives from included files is not supported
#define LSOneAppPublisher "LS Retail"
#define LSOneAppPublisherURL "www.lsretail.com"
#define LSOneCompression "lzma"
#define LSOneSolidCompression "yes"
#define LSOneMinVersion "6.1.7601"
#define HashSalt "One retail POS software to excel in all your retail challenges"
#define Crypto "SHA512"

#pragma message "SourcePath: " + SourcePath

;folders with additional files
#ifndef SQLServerDir
    #define SQLServerDir "..\SQLServer"
#endif
#pragma message "SQLServerDir: " + SQLServerDir
#ifndef ExtraDir
    #define ExtraDir "..\Extra"
#endif
#pragma message "ExtraDir: " + ExtraDir
#ifndef DocDir
    #define DocDir "..\Help"
#endif
#pragma message "DocDir: " + DocDir

;type of the installer: (none), BASIC, EXPRESS. Default is (none)
#ifndef Type
    #define Type ''
#endif
#pragma message "Type: " + Type
;text that will be appended to the setup name
#ifndef SetupSuffix
    #define SetupSuffix ''
#endif
#pragma message "SetupSuffix: " + SetupSuffix

[Setup]
#define LSOneLicenseFile SourcePath + "\..\Common\licenses\LSR SLT 2020.rtf"
#define LSOneWizardSmallImageFile SourcePath + "\..\Common\images\icon.bmp"
#define LSOneWizardImageFile SourcePath + "\..\Common\images\wizard image.bmp"
#define LSOneOutputDir ".\Setup"
#define LSOneDisableUI 'no'

;determne release number
#define private FindHandle
#define FindHandle = FindFirst("..\..\SM\Build\Site Manager.exe", 0)
#if FindHandle
    #define Version GetFileProductVersion("..\..\SM\Build\Site Manager.exe")
    #pragma message  "Version: " + Version
    #if Len(Version) > 0
        #define ReleaseNumber Trim(StringChange(Version, "LS One", ""))
    #else
        #define ReleaseNumber "no version found"
    #endif
#else
    #define ReleaseNumber ""
#endif
#pragma message "ReleaseNumber: " + ReleaseNumber

;determine application name
#define private ProjectName = StringChange(ExtractFileDir(SourcePath), ExtractFileDir(ExtractFileDir(SourcePath)) + "\", "")
#pragma message "ProjectName: " + ProjectName

;determine source dir
#if "SiteManager" == ProjectName
    #ifexist StringChange(ExtractFilePath(SourcePath), "Installs\", "") + "\Build"
        #define SourceDir StringChange(ExtractFilePath(SourcePath), "Installs\", "") + "\Build"
    #else
        #define SourceDir StringChange(ExtractFilePath(SourcePath), "Installs\SiteManager", "SM") + "\Build"
    #endif
#else
    #define SourceDir StringChange(ExtractFilePath(SourcePath), "Installs\", "") + "\Build"
#endif
#pragma message "SourceDir: " + SourceDir

;if there is a SQL installer in SQLServerDir then the LSOne installer will contain the SQL Server bundle
#if "SiteService" == ProjectName
    #define SQL "no"
#endif

#ifndef SQL
    #define FindHandle = FindFirst(SQLServerDir + "\SQL*.exe", faAnyFile)
    #if FindHandle
        #define SQL "yes"
        #define SQLServerSubDir ""
        #define SQLServerExe FindGetFileName(FindHandle)
        #pragma message "SQLServerSubDir: " + SQLServerSubDir + ", SQLServerExe: " + SQLServerExe
    #else
        #define SQL "no"
    #endif
#endif
#pragma message "SQL: " + SQL

;Determine the OutputBaseFilename using preprocesor
;scripting constant for the OutputBaseFilename directive cannot be used because this directive specifies the name for the resulting setup file and thus has to be known at compilation time
#define SetupName 'LSOne.' + ProjectName + '.Setup'
#pragma message "SetupName: " + SetupName

#if "yes" == SQL
    #if "BASIC" == Type
        #define LSOneOutputBaseFilename SetupName + '.SQL.Basic' + SetupSuffix
    #elif "EXPRESS" == Type
        #define LSOneOutputBaseFilename SetupName + '.SQL.Express' + SetupSuffix
    #else
        #define LSOneOutputBaseFilename SetupName + '.SQL' + SetupSuffix
    #endif
#else
    #if "BASIC" == Type
        #define LSOneOutputBaseFilename SetupName + '.Basic' + SetupSuffix
    #elif "EXPRESS" == Type
        #define LSOneOutputBaseFilename SetupName + '.Express' + SetupSuffix
    #else
        #define LSOneOutputBaseFilename SetupName + SetupSuffix
    #endif
#endif
#pragma message "LSOneOutputBaseFilename: " + LSOneOutputBaseFilename

;The directives below need to be copied into each installer script since overriding directives from included files is not supported
;AppPublisher={#LSOneAppPublisher}
;AppPublisherURL={#LSOneAppPublisherURL}
;AppUpdatesURL={#LSOneAppPublisherURL}
;AppSupportURL={#LSOneAppPublisherURL}
;LicenseFile={#LSOneLicenseFile}
;Compression={#LSOneCompression}
;SolidCompression={#LSOneSolidCompression}
;MinVersion={#LSOneMinVersion}
;WizardSmallImageFile={#LSOneWizardSmallImageFile}
;WizardImageFile={#LSOneWizardImageFile}
;OutputDir={#LSOneOutputDir}
;OutputBaseFilename={#LSOneOutputBaseFilename}

;starting with InnoSetup 5.5.7, As recommended by Microsoft's desktop applications guideline, 
;DisableWelcomePage now defaults to yes. 
;Additionally DisableDirPage and DisableProgramGroupPage now default to auto. The defaults in all previous versions were no.
;see http://www.jrsoftware.org/files/is5.5-whatsnew.htm#5.5.7
;DisableWelcomePage={#LSOneDisableUI}
;DisableDirPage={#LSOneDisableUI}
;DisableProgramGroupPage={#LSOneDisableUI}

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"