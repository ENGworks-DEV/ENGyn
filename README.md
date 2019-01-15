![](https://github.com/ENGworks-DEV/ENGyn/blob/master/tools/ENGyn-300x138.png?raw=true)

Graphical programing interface based on [TUM.CMS.VPLControl](https://github.com/tumcms/TUM.CMS.VPLControl) in progress

## Examples
ENGyn: Automate Group clashes

[![](http://img.youtube.com/vi/zN5bTxGnX6E/0.jpg)](http://www.youtube.com/watch?v=zN5bTxGnX6E "ENGyn: Automate Group clashes")

ENGyn: Apply appearance by profile

[![](http://img.youtube.com/vi/Ar2xiYwzpCA/0.jpg)](http://www.youtube.com/watch?v=Ar2xiYwzpCA "ENGyn: Apply appearance by profile")


## Navisworks tested versions

* Navisworks Manage 2019
* Navisworks Manage 2018

## Dependencies
Dependencies are loaded by configuration - 2019 uses NW 2019 dlls and net 4.7 as framework, 2018 uses NW 2018 and net 4.6. 
TUM dll need to be build in submodule to use it as reference.

## Installation
Build the project and the Build events will copy the dlls to the right folder or copy them yourself to (replace 2019 with the version of Navisworks you have):

``` %APPDATA%\Autodesk Navisworks Manage 2019\Plugins\  ```

TUM.VPLcontrol should be compiled using ENG version which includes different features than original version.
[![](https://github.com/ENGworks-DEV/TUM.CMS.VPLControl)](TUM.VPLcontrol)

## Contribute ##

ENGyn is an open-source project and would be nothing without its community. You can make suggestions or track and submit bugs via Github issues.  You can submit your own code to the ENGyn project via a Github pull request.

### Commits:

***Name:*** should follow this schema (ENGyn)(-)(chore|feat|docs|fix|refactor|style|test|sonar|hack|release)(:)( )(.{0,80})

e.g:
```ENGyn-fix: ZoomOut command method fixed to include new matrix zoom.```

***Changes:*** Should be as atomic as posible remaining transactional.

## About ##

ENGworks has the combined experience of over 25 years of involvement and collaboration in the field of building technology and related services. We are supported by the talent and knowledge of over 100 highly skilled technical professionals from multiple office locations in the Americas, including Los Angeles, Chicago, Sao Paulo, Santiago de Chile and Cordoba. Alongside with our strategic partners in the UK, Australia and Middle East, ENGworks currently participates in projects on a global scale, offering verifiable superior services and project support, all the while continuing to develop the unrealized potential of BIM.

<img src="https://github.com/ENGworks-DEV/RenumberParts/blob/master/RenumberParts/Resources/EngLogo-01.png" width="650" height="200">


