cls
echo. Coping Dependencies
@echo off
if not "%1"=="am_admin" (powershell start -verb runas '%0' am_admin & exit /b)
set myadress= %~dp0
xcopy /Y %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Newtonsoft.Json20.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2018\Dependencies\"
xcopy /Y %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll %myadress%\bin\2018
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\NCalc.dll %myadress%\bin\2018
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Newtonsoft.Json20.dll %myadress%\bin\2018
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\RadialMenu.dll %myadress%\bin\2018
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb %myadress%\bin\2018
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll %myadress%\bin\2018

xcopy /Y %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Newtonsoft.Json20.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\"
xcopy /Y %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll %myadress%\bin\2019
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\NCalc.dll %myadress%\bin\2019
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Newtonsoft.Json20.dll %myadress%\bin\2019
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\RadialMenu.dll %myadress%\bin\2019
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb %myadress%\bin\2019
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll %myadress%\bin\2019


xcopy /Y %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\NCalc.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Newtonsoft.Json20.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\RadialMenu.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll "C:\Program Files\Autodesk\Navisworks Manage 2017\Dependencies\"
xcopy /Y %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.dll %myadress%\bin\2017
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\NCalc.dll %myadress%\bin\2017
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Newtonsoft.Json20.dll %myadress%\bin\2017
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\RadialMenu.dll %myadress%\bin\2017
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\TUM.CMS.VplControl.pdb %myadress%\bin\2017
xcopy /Y  %myadress%src\TUM.CMS.VPLControl\bin\Xceed.Wpf.Toolkit.dll %myadress%\bin\2017

echo FINISH
set /p=Hit ENTER to continue...

