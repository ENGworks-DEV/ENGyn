# NW_GraphicPrograming

Graphical programing interface based on [TUM.CMS.VPLControl](https://github.com/tumcms/TUM.CMS.VPLControl) in progress


![](https://media.giphy.com/media/9JrzovA2JPoX95CBc8/giphy.gif)

## Navisworks tested versions

* Navisworks Manage 2019
* Navisworks Manage 2018

## Dependencies
* Version for 2019 needs .Net 4.7 (Visual Studio 2017)
* Navisworks handles reference dlls on a specific folder. Copy the content of the folder "resources" into  
``` C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies ```
* For Navisworks 2018, replace Navisworks dlls with the ones in :
``` C:\Program Files\Autodesk\Navisworks Manage 2018\ ```

## Installation
Build the project and the Build events will copy the dlls to the right folder or copy them your self to

``` %APPDATA%\Autodesk Navisworks Manage 2019\Plugins\  ```

