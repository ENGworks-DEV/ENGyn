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

namespace NW_GraphicPrograming.Nodes
{
    public class Clash_CompactTest : Node
    {
        public Clash_CompactTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Document", typeof(Document));
            AddInputPortToNode("Input", typeof(object));
            AddOutputPortToNode("Output", typeof(Document));
            Name = "Compact Clash Test";


            AddControlToNode(new Label() { Content = "Compact Test", FontSize= 13, FontWeight = FontWeights.Bold });


        }


        public override void Calculate()
        {
            
            if (InputPorts[1].Data != null && InputPorts[0].Data != null)
            {

                var t = InputPorts[1].Data.GetType();

                if (InputPorts[1].Data.GetType() == typeof(ClashTest))
                {
                    var doc = InputPorts[0].Data as Document;
                    
                    ClashTest ct = InputPorts[1].Data as ClashTest;
                    var clashes = doc.GetClash();
                    clashes.TestsData.TestsCompactAllTests();
                    //clashes.TestsData.TestsCompactTest(ct);
                }
                if (InputPorts[1].Data.GetType() == typeof(List<object>))

                {
                    var listData = InputPorts[1].Data as List<object>;
                    if (listData[0].GetType() == typeof(ClashTest))
                    {
                        foreach (var ct in InputPorts[1].Data as List< object>)
                        {
                            var clashTest = ct as ClashTest;
                                  var doc = InputPorts[0].Data as Document;

                                var clashes = doc.GetClash();
                                clashes.TestsData.TestsCompactTest(clashTest);


                        }
                    }

                }
                GC.Collect();
                
                OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;
            }

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
            return new Clash_CompactTest(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}