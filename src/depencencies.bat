cls
echo. Coping Dependencies
@echo off
if not "%1"=="am_admin" (powershell start -verb runas '%0' am_admin & exit /b)
set myadress= %~dp0
xcopy /Y %myadress%TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\Newtonsoft.Json.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"

xcopy /Y %myadress%TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\Newtonsoft.Json.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
echo FINISH
set /p=Hit ENTER to continue...

