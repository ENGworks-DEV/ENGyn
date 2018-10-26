---
layout: post
title: "ENGyn Alpha 3"
description: "Download now the latest version of ENgyn!"
---

![](https://github.com/ENGworks-DEV/ENGyn/blob/master/tools/ENGyn-300x138.png?raw=true)

***We are happy to announce a new version of [ENGyn alpha 3](https://github.com/ENGworks-DEV/ENGyn/releases/tag/0.1.3.alpha3)***

## Download
To install download the installer for your Navisworks version (2018/2019) but if you feel brave, you can build your own using the source code.

------
## Examples

We have included examples of the workflows we use so you don't have to start from Scratch


[***Export Clash Names***](https://github.com/ENGworks-DEV/ENGyn/blob/master/examples/ExportClashNames.vplxml)
Use the csv export to rename clashes!
![](/ENGyn/assests/img/ExportClashNames.png)


[***Rename Clashes By GUID***](https://github.com/ENGworks-DEV/ENGyn/blob/master/examples/RenameClashesByGUID.vplxml)

Needs the previous example
![](/ENGyn/assests/img/RenameClashesByGUID.png)


[***Export Clash Test Information***](https://github.com/ENGworks-DEV/ENGyn/blob/master/examples/ExportClashTestInformation.vplxml)

Feed PowerBI
![](/ENGyn/assests/img/ExportClashTestInformation.png)


[***RefreshClash GroupBy ColorByTest***](https://github.com/ENGworks-DEV/ENGyn/blob/master/examples/RefreshClash-GroupBy-ColorByTest.vplxml)

Keep clashes visible outside ClashDetective
![](/ENGyn/assests/img/RefreshClashesGroupByLevel&SelectionA.png)


[***Refresh Clashes Group By Leve l& Selection A***](https://github.com/ENGworks-DEV/ENGyn/blob/master/examples/RefreshClashesGroupByLevel%26SelectionA.vplxml)

all in one clash group
![](/ENGyn/assests/img/RefreshClashesGroupByLevel&SelectionA.png)

------

### Cool New Features and bugs solved:

* API: GetAPIPropertyValues bug fixed
* Appearance : SetAppearanceByProfile setting transparency to 100% no matter the value bug
* Appearance: fixed bug in appearance by selection
* Appearance: GetProfileFromXML is setting default transparency to 0 instead of -1 (as invalid)bug
* Appearance: SetAppearanceBySelection not applying Appearance to clashes bug
* Clash: BIM42ClashGroup integrates all its options into a single node.
* Clash: Rename clashes from CSV
* Examples: added examples top documentation
* GUI: Copy and paste nodes working
* GUI: Nodes positions when opening are not mapped to the actual canvas size
* GUI: SearchBox text are in black bug
* GUI: SelectionNode do not appear near the mouse click position bug
* GUI: Workaround for zoom and pan by creating a sizable canvas.
* GUI: Zoom Out is limited to the real node size bug
* Input: ReadCSVFile enhancement
* Input: SaveAs dialog enhancement
* Input: SaveFile dialog added.
* List: ListTranspose not showing content bug
* List: ListTranspose now showing items in preview
* Viewpoint: Rename Viewpoint by GUID and String enhancement
* Viewpoint: Rename Viewpoints from CSV


### And now, what is in near the future?
------------------------------------------------------------


ENGyn is now open-source and we would like to invite you all to build a community of VDC/BIM Managers working to make Navisworks workflows more flexible! Check the source code here:  https://github.com/ENGworks-DEV/ENGyn

On our side, we are working on new GUI implementations and documentation/videos to share the workflows we use day by day using ENGyn.

### Here are some coming soon features:
------------------------------------------------------------
* Clash: Access clash comments
* Clash: Create clash report
* Clash: Create clash tests
* Clash: create clashes comments
* Clash: GroupClashes by Sphere (after by level)
* Clash: Rename ClashResult/ ClashGroup
* GUI: include Scripting nodes
* GUI: Run from command to schedule execution
* List: Create number range
* Selection: Create search set from Excel/CSV
* Viewpoint: Export XML
* Viewpoints: Access Element comments
* Viewpoints: Create viewpoints


We love to hear what you have to say, so contact us thru [innovation@engworks.com](mailto:minnovation@engworks.com?subject=ENGyn%200.1.2:%20Feedback)

