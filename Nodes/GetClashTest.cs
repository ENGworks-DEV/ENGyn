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
    public class GetClashTest : Node
    {
        public GetClashTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("Navis Clash Tests", typeof(Object));

        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                var testData = doc.GetClash().TestsData;

                if (testData != null && testData.Tests.Count > 0)
                {
                    List<Object> clashTestList = new List<Object>();
                    foreach (var t in testData.Tests)
                    {

                        clashTestList.Add(t as Object);
                    }

                    OutputPorts[0].Data = clashTestList;
                }
                else
                { OutputPorts[0].Data = 0; }
            }
        }




        public override Node Clone()
        {
            return new GetClashTest(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}