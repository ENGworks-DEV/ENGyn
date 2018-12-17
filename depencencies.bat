cls
echo. Coping Dependencies
@echo off
if not "%1"=="am_admin" (powershell start -verb runas '%0' am_admin & exit /b)
set myadress= %~dp0
xcopy /Y %myadress%tools\dependencies\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\Newtonsoft.Json.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y %myadress%tools\dependencies\TUM.CMS.VplControl.dll %myadress%\bin\2018
xcopy /Y  %myadress%tools\dependencies\NCalc.dll %myadress%\bin\2018
xcopy /Y  %myadress%tools\dependencies\Newtonsoft.Json.dll %myadress%\bin\2018
xcopy /Y  %myadress%tools\dependencies\RadialMenu.dll %myadress%\bin\2018
xcopy /Y  %myadress%tools\dependencies\TUM.CMS.VplControl.pdb %myadress%\bin\2018
xcopy /Y  %myadress%tools\dependencies\Xceed.Wpf.Toolkit.dll %myadress%\bin\2018

xcopy /Y %myadress%tools\dependencies\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\Newtonsoft.Json.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y %myadress%tools\dependencies\TUM.CMS.VplControl.dll %myadress%\bin\2019
xcopy /Y  %myadress%tools\dependencies\NCalc.dll %myadress%\bin\2019
xcopy /Y  %myadress%tools\dependencies\Newtonsoft.Json.dll %myadress%\bin\2019
xcopy /Y  %myadress%tools\dependencies\RadialMenu.dll %myadress%\bin\2019
xcopy /Y  %myadress%tools\dependencies\TUM.CMS.VplControl.pdb %myadress%\bin\2019
xcopy /Y  %myadress%tools\dependencies\Xceed.Wpf.Toolkit.dll %myadress%\bin\2019


xcopy /Y %myadress%tools\dependencies\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\Newtonsoft.Json.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%tools\dependencies\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y %myadress%tools\dependencies\TUM.CMS.VplControl.dll %myadress%\bin\2017
xcopy /Y  %myadress%tools\dependencies\NCalc.dll %myadress%\bin\2017
xcopy /Y  %myadress%tools\dependencies\Newtonsoft.Json.dll %myadress%\bin\2017
xcopy /Y  %myadress%tools\dependencies\RadialMenu.dll %myadress%\bin\2017
xcopy /Y  %myadress%tools\dependencies\TUM.CMS.VplControl.pdb %myadress%\bin\2017
xcopy /Y  %myadress%tools\dependencies\Xceed.Wpf.Toolkit.dll %myadress%\bin\2017

echo FINISH
set /p=Hit ENTER to continue...

