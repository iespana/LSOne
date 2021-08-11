[Code]
(* Description: Logs the operating system error code and a corresponding localized error message *)
procedure LogSysError(ErrorCode: Integer);
begin
	Log('GetLastError = ' + IntToStr(ErrorCode) + ': ' + SysErrorMessage(ErrorCode));
end;

(* Description: Updates a key or adds it to the appSettings section from a .NET configuration file *)
procedure UpdateConfigFile(fileName: string; keyName: string; newValue:string);
var
  XMLDoc, RootNode, Nodes, Node: Variant;
  ConfigFilename,Key: String;
  i: integer;
  keyExists: Boolean;
begin
  ConfigFilename := fileName;
  Log('UpdateConfigFile called  ' + ConfigFilename);
  Log('UpdateConfigFile  ' + keyName + ' - ' + newValue);
  try
    XMLDoc := CreateOleObject('Msxml2.DOMDocument.6.0');
  except
    RaiseException('MSXML is required to complete the post-installation process.'#13#13'(Error ''' + GetExceptionMessage + ''' occurred)');
  end;  

  XMLDoc.async := False;
  XMLDoc.resolveExternals := False;
  XMLDoc.preserveWhiteSpace := True;

  XMLDoc.load(ConfigFilename);
  if XMLDoc.parseError.errorCode <> 0 then
    RaiseException('Error on line ' + IntToStr(XMLDoc.parseError.line) + ', position ' + IntToStr(XMLDoc.parseError.linepos) + ': ' + XMLDoc.parseError.reason);

  XMLDoc.setProperty('SelectionLanguage', 'XPath');

  RootNode := XMLDoc.documentElement;
  Nodes := RootNode.selectNodes('//configuration/appSettings/add');
  Log('UpdateConfigFile node count: ' + IntToStr(Nodes.length));
  for i := 0 to Nodes.length - 1 do
  begin
    Node := Nodes.Item[i];
    if Node.NodeType = 1 then begin
      key := Node.getAttribute('key');

      Log(key);
      if key = keyName then begin
        keyExists := True;
        Node.setAttribute('value', newValue);       
      end
    end;
  end;

  if not keyExists then begin
    RootNode := XMLDoc.selectSingleNode('//configuration/appSettings');
    Node := XMLDoc.createElement('add');
    Node.setAttribute('key', keyName);
    Node.setAttribute('value', newValue);
    RootNode.appendChild(Node);
  end;

  try
    XMLDoc.Save(ConfigFilename); 
  except
    RaiseException('Could not save file because: ' + GetExceptionMessage);
  end;
end;

(* Windows services management functions *)
(* If you want to compile an existing script that imports ANSI Windows API calls with the Unicode compiler, either upgrade to the 'W' Unicode API call or change the parameters from 'String' or 'PChar' to 'AnsiString'. The 'AnsiString' approach will make your [Code] compatible with both the Unicode and the non Unicode version. see http://www.jrsoftware.org/is6help/index.php?topic=unicode *)
type
	SERVICE_STATUS = record
		dwServiceType				: cardinal;
		dwCurrentState				: cardinal;
		dwControlsAccepted			: cardinal;
		dwWin32ExitCode				: cardinal;
		dwServiceSpecificExitCode	: cardinal;
		dwCheckPoint				: cardinal;
		dwWaitHint					: cardinal;
	end;
	HANDLE = cardinal;

const
	SERVICE_QUERY_CONFIG		= $1;
	SERVICE_CHANGE_CONFIG		= $2;
	SERVICE_QUERY_STATUS		= $4;
	SERVICE_START				= $10;
	SERVICE_STOP				= $20;
	SERVICE_ALL_ACCESS			= $f01ff;
	SC_MANAGER_ALL_ACCESS		= $f003f;
	SERVICE_WIN32_OWN_PROCESS	= $10;
	SERVICE_WIN32_SHARE_PROCESS	= $20;
	SERVICE_WIN32				= $30;
	SERVICE_INTERACTIVE_PROCESS = $100;
	SERVICE_BOOT_START          = $0;
	SERVICE_SYSTEM_START        = $1;
	SERVICE_AUTO_START          = $2;
	SERVICE_DEMAND_START        = $3;
	SERVICE_DISABLED            = $4;
	SERVICE_DELETE              = $10000;
	SERVICE_CONTROL_STOP		= $1;
	SERVICE_CONTROL_PAUSE		= $2;
	SERVICE_CONTROL_CONTINUE	= $3;
	SERVICE_CONTROL_INTERROGATE = $4;
	SERVICE_STOPPED				= $1;
	SERVICE_START_PENDING       = $2;
	SERVICE_STOP_PENDING        = $3;
	SERVICE_RUNNING             = $4;
	SERVICE_CONTINUE_PENDING    = $5;
	SERVICE_PAUSE_PENDING       = $6;
	SERVICE_PAUSED              = $7;

function OpenSCManager(lpMachineName, lpDatabaseName: AnsiString; dwDesiredAccess :cardinal): HANDLE;
external 'OpenSCManagerA@advapi32.dll stdcall';

function OpenService(hSCManager :HANDLE; lpServiceName: AnsiString; dwDesiredAccess :cardinal): HANDLE;
external 'OpenServiceA@advapi32.dll stdcall';

function CloseServiceHandle(hSCObject :HANDLE): boolean;
external 'CloseServiceHandle@advapi32.dll stdcall';

function CreateService(hSCManager :HANDLE; lpServiceName, lpDisplayName: string; dwDesiredAccess,dwServiceType,dwStartType,dwErrorControl: cardinal; lpBinaryPathName,lpLoadOrderGroup: String; lpdwTagId : cardinal; lpDependencies,lpServiceStartName,lpPassword :string): cardinal;
external 'CreateServiceA@advapi32.dll stdcall';

function DeleteService(hService :HANDLE): boolean;
external 'DeleteService@advapi32.dll stdcall';

function StartNTService(hService :HANDLE; dwNumServiceArgs : cardinal; lpServiceArgVectors : cardinal) : boolean;
external 'StartServiceA@advapi32.dll stdcall';

function ControlService(hService :HANDLE; dwControl :cardinal; var ServiceStatus :SERVICE_STATUS) : boolean;
external 'ControlService@advapi32.dll stdcall';

function QueryServiceStatus(hService :HANDLE; var ServiceStatus :SERVICE_STATUS) : boolean;
external 'QueryServiceStatus@advapi32.dll stdcall';

function QueryServiceStatusEx(hService :HANDLE; ServiceStatus :SERVICE_STATUS) : boolean;
external 'QueryServiceStatus@advapi32.dll stdcall';

function GetServiceHandle(serviceName: AnsiString) : HANDLE;
var
	hSCM: HANDLE;
	hService: HANDLE;
begin
	Result := 0;
	Log('Opening Service Manager');
	hSCM := OpenSCManager('', '', SC_MANAGER_ALL_ACCESS);
	if hSCM <> 0 then
	begin
		Log('Opening service ' + serviceName);
		hService := OpenService(hSCM, serviceName, SERVICE_QUERY_CONFIG or SERVICE_QUERY_STATUS or SERVICE_STOP or SERVICE_START);
		if hService <> 0 then
		begin
			Log('Found service handle for ' + serviceName);
		end
		else
		begin
			Log('Could not find service handle');
			LogSysError(DLLGetLastError());
		end;
		Result := hService;
		CloseServiceHandle(hSCM);
	end
	else
	begin
		Log('The Service Manager is not available');
		LogSysError(DLLGetLastError());
	end;
end;

function StopService(hService: HANDLE) : Boolean;
var
	status: SERVICE_STATUS;
begin
	if hService <> 0 then
	begin
		Log('Stopping service');
		Result := ControlService(hService, SERVICE_CONTROL_STOP, status);
		if not Result then
		begin
			LogSysError(DLLGetLastError());
			exit;
		end;
		Log('Status = ' + IntToStr(status.dwCurrentState));
		while Result and (status.dwCurrentState <> SERVICE_STOPPED) do
		begin
			Log('Sleeping');
			Sleep(1000);
			Result := QueryServiceStatus(hService, status);
		end;
	end;
end;

function StartService(serviceName: AnsiString; var ErrorCode: Integer) : Boolean;
begin
	Result := ShellExec('', 'cmd.exe', '/C net start ' + serviceName, '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
end;