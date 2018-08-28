using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;

namespace NW_GraphicPrograming.Nodes
{
    public class NW_RefreshClashTest : Node
    {
        public NW_RefreshClashTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Tests", typeof(object));
            AddOutputPortToNode("Clash Tests", typeof(List<Object>));


            //TODO: input as part of the point.Below, temporary solution : One label per input
            foreach (Port item in this.InputPorts)
            {
                item.ToolTip = item.Name;
                // AddControlToNode(new Label() { Content = item.Name, FontSize = 13 });
            }

            AddControlToNode(new Label() { Content = "Clash Tests", FontSize = 13 });



            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Refresh clash test from document" };
            IsResizeable = true;
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
                    var clashTestList = InputPorts[0].Data as List<ClashTest>;

                    foreach (ClashTest item in clashTestList)
                    {
                        testData.TestsRunTest(item);
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
            return new NW_RefreshClashTest(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}