# NW_GraphicPrograming

Graphical programing interface based on [TUM.CMS.VPLControl](https://github.com/tumcms/TUM.CMS.VPLControl) in progress

Querying selected elements

![](https://media.giphy.com/media/9JrzovA2JPoX95CBc8/giphy.gif)

Querying Clash Results

![](https://media.giphy.com/media/1fmx4BFwHO7Nez3PCm/200w_d.gif)

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
Build the project and the Build events will copy the dlls to the right folder or copy them your self to (replace 2019 with the version of Navisworks you have):

``` %APPDATA%\Autodesk Navisworks Manage 2019\Plugins\  ```

