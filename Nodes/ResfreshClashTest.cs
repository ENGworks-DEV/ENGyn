using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;
using System.Windows;

namespace ENGyn.Nodes.Clash
{
    public class RefreshClashTests : Node
    {
        public RefreshClashTests(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Tests", typeof(object));
            AddOutputPortToNode("Clash Tests", typeof(object));

        }

        public override void Calculate()
        {
            //Get clashes from document
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            var testData = doc.GetClash().TestsData;
            var input = InputPorts[0].Data;
            if (input != null)
            {

                
                if (MainTools.IsList(input))
               {
                    var clashTestList = new List<object>();
                    foreach (var item in (System.Collections.IEnumerable)InputPorts[0].Data)
                    {
                        Autodesk.Navisworks.Api.Clash.ClashTest test = item as Autodesk.Navisworks.Api.Clash.ClashTest;
                        var guid = test.Guid;
                        testData.TestsRunTest(test);
                        var index = doc.GetClash().TestsData.Tests.IndexOfGuid(guid);
                        var ct = doc.GetClash().TestsData.Tests[index];
                        clashTestList.Add(ct);
                    }
                    OutputPorts[0].Data = clashTestList;
                }

                if (input.GetType() == typeof( ClashTest))
                {

                    var clashTest = input as Autodesk.Navisworks.Api.Clash.ClashTest;
                    var guid = clashTest.Guid;
                    var index = doc.GetClash().TestsData.Tests.IndexOfGuid(guid);
                    var ct = doc.GetClash().TestsData.Tests[index];
                    testData.TestsRunTest(clashTest);
                    OutputPorts[0].Data = ct;
                }
            }
            

        }


    

        public override Node Clone()
        {
            return new RefreshClashTests(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}