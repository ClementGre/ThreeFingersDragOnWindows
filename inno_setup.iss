; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ThreeFingersDragOnWindows"
#define MyAppVersion "1.1"
#define MyAppPublisher "Clement Gre"
#define MyAppURL "https://github.com/ClementGre/ThreeFingersDragOnWindows"
#define MyAppExeName "ThreeFingersDragOnWindows.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{11EFAFCF-F261-4727-98B9-3B3C343F0F69}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\deploy
OutputBaseFilename=ThreeFingersDragOnWindows
SetupIconFile=C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\Resources\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\bin\Debug\net6.0-windows\win-x64\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\bin\Debug\net6.0-windows\win-x64\publish\ThreeFingersDragOnWindows.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\bin\Debug\net6.0-windows\win-x64\publish\ThreeFingersDragOnWindows.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\bin\Debug\net6.0-windows\win-x64\publish\ThreeFingersDragOnWindows.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Clement\Developpement\CSharp\ThreeFingersDragOnWindows\bin\Debug\net6.0-windows\win-x64\publish\Resources\icon.ico"; DestDir: "{app}\Resources"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

