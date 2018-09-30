/* 
Author : Dominic Singer
Year : 2016
Edited by: Pablo Derendinger - ENGworks

The MIT License (MIT)

Copyright (c) 2015 TUM Chair of Computational Modeling and Simulation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 

Usage : Compile dll and place in folder %APPDATA%\Navisworks Manage 2019\Plugins\ENGyn\Nodes\

*/


using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes; //Look for this library at C:\Program Files\Autodesk\Navisworks Manage 2019\Dependencies\
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;


//The last part of the namespace will be used to stack the node in GUI - in this case "Nodes"
namespace ENGyne.Nodes
{
    public class TemplateName : Node
    {
        //Name of the node; it will be shown in node list at GUI
        public TemplateName(VplControl hostCanvas)
            : base(hostCanvas)
        {
            //Add one Input port per input and one Output port per output
            AddInputPortToNode("Input", typeof(object));
            AddOutputPortToNode("Output", typeof(ClashResult));

        }

        //Here place the code to run the process you want
        public override void Calculate()
        {
            var input = InputPorts[0].Data;
        //Remember to check for nulls and cast types 
            InputPorts[0].Data = input;
        }


        public override Node Clone()
        {
            //Rename to same name as the node
            return new TemplateName(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

} 