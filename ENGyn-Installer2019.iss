; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B827E3D1-F82E-45B9-9352-53C8EF41399F}
AppName=ENGworks ENGyn 2019    
AppVersion=0.1.4.6

AppPublisher=PRD
AppPublisherURL=-
AppSupportURL=-
AppUpdatesURL=-
DefaultDirName={pf}\ENGyn
DisableDirPage=yes
DefaultGroupName=ENGworks ENGyn 2019
DisableProgramGroupPage=yes
OutputBaseFilename=ENG ENGyn 2019 Setup 0.1.4.6
Compression=lzma
SolidCompression=yes
OutputManifestFile=Setup-Manifest.txt
OutputDir=bin\Install
[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{userappdata}\Autodesk Navisworks Manage 2019\Plugins\"
[InstallDelete]
Type: filesandordirs; Name: "{userappdata}\Autodesk Navisworks Manage 2019\Plugins\ENGyn\"
    

[Files]
Source: "bin\2019\*"; DestDir: "{userappdata}\Autodesk Navisworks Manage 2019\Plugins\ENGyn\"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\*"; DestDir: "{pf64}\Autodesk\Navisworks Manage 2019\Dependencies\"; Flags: ignoreversion recursesubdirs createallsubdirs
          
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

