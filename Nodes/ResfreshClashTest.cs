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

namespace NW_GraphicPrograming.Nodes.Clash
{
    public class Clash_RefreshTest : Node
    {
        public Clash_RefreshTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Tests", typeof(object));
            AddOutputPortToNode("Clash Tests", typeof(object));

            AddControlToNode(new Label() { Content = "Clash Tests", FontSize = 13, FontWeight = FontWeights.Bold });

        }

        public override void Calculate()
        {
            //Get clashes from document
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            var testData = doc.GetClash().TestsData;

            if (InputPorts[0].Data != null)
            {
                try
                {
                    var clashTestList = InputPorts[0].Data as List<object>;

                    foreach (var item in clashTestList)
                    {
                        ClashTest test = item as ClashTest;
                        testData.TestsRunTest(test);
                    }

                    if (clashTestList.Count != 0)
                    {
                        OutputPorts[0].Data = clashTestList;
                    }


                    else
                    {
                        OutputPorts[0].Data = 0;
                    }


                }
                catch
                {

                    var clashTest = InputPorts[0].Data as ClashTest;
                    testData.TestsRunTest(clashTest);
                    OutputPorts[0].Data = clashTest;
                }
            }


            //var testData = doc.GetClash().TestsData;



        }


        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            // add your xml serialization methods here
        }

        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            // add your xml deserialization methods here
        }

        public override Node Clone()
        {
            return new Clash_RefreshTest(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}