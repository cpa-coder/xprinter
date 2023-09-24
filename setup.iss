[Setup]
AppName=Fast Receipt
AppVersion=0.1.0
AppPublisher=AccounTech Business Management Services
WizardStyle=modern
DefaultDirName={autopf}\FastReceipt
DefaultGroupName=FastReceipt
SetupIconFile=src\FastReceipt.Client\Assets\favicon.ico
WizardSmallImageFile=src\FastReceipt.Client\Assets\favicon.bmp
UninstallDisplayIcon={app}\fastreceipt.exe
Compression=lzma2
SolidCompression=yes
OutputDir=userdocs:FastReceipt
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
ChangesEnvironment=yes
SignTool=signtool

[Files]
Source: "src\FastReceipt.Client\bin\Release\net7.0-windows\win-x64\publish\fastreceipt.exe"; DestDir: "{app}"; DestName: "fastreceipt.exe"
Source: "src\FastReceipt.Client\bin\Release\net7.0-windows\win-x64\publish\printer.exe"; DestDir: "{app}"; DestName: "printer.exe"

[Registry]
Root: "HKLM"; Subkey: "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"; ValueType: string; ValueName: "FASTRECEIPT_URL"; ValueData: "{code:GetUrl}"; Flags: createvalueifdoesntexist preservestringtype

[Run]
Filename: {sys}\sc.exe; Parameters: "create ""Fast Receipt"" start= auto binPath= ""{app}\fastreceipt.exe run -p {code:GetPrint} -c {code:GetConnected}""" ; Flags: runhidden;

[UninstallRun]
Filename: {sys}\sc.exe; Parameters: "stop ""Fast Receipt"""; Flags: runhidden; RunOnceId: "StopService"
Filename: {sys}\sc.exe; Parameters: "delete ""Fast Receipt"""; Flags: runhidden; RunOnceId: "DeleteService"

[Code]
var 
  ParameterSetupPage: TInputQueryWizardPage;

function GetUrl(String: string) : String;
begin
  if(ParameterSetupPage.Values[0] = '') then
     Result := 'http://localhost:5000/notification-hub'
  else
    Result := ParameterSetupPage.Values[0];
end;

function GetPrint(String: string) : String;
begin
  if(ParameterSetupPage.Values[1] = '') then
     Result := 'print-receipt'
  else
    Result := ParameterSetupPage.Values[1];
end;

function GetConnected(String: string) : String;
begin
  if(ParameterSetupPage.Values[2] = '') then
     Result := 'PrinterConnected'
  else
    Result := ParameterSetupPage.Values[2];
end;

procedure AddParameterSetupPage();
begin
  ParameterSetupPage := CreateInputQueryPage(
    wpWelcome,
    'Welcome to ' + '{#SetupSetting("AppName")}' +' setup wizard.',
    'Setup initial application parameters need to properly run the application as a service.',
    'Fill-out the fields to change the default parameters or leave it blank to use the default.');

  ParameterSetupPage.Add('Server Url: (http://localhost:5000/notification-hub)', False);
  ParameterSetupPage.Add('Print Function Name: (print-receipt)', False);
  ParameterSetupPage.Add('Connected Function Name: (PrinterConnected)', False);
end;

procedure InitializeWizard();
begin
  AddParameterSetupPage();
end;

