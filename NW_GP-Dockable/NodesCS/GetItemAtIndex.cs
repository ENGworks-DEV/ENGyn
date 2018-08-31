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
    public class GetItemAtIndex : Node
    {
        public GetItemAtIndex(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("List", typeof(List<object>));
            AddInputPortToNode("Index", typeof(int));
            AddOutputPortToNode("Result", typeof(object));
            AddControlToNode(new Label { Content = "GetItemAtIndex" });


            AddControlToNode(new Label() { Content = "Get item at index" });

            IsResizeable = true;
       
        }

        public override void Calculate()
        {
            var count = Int32.Parse( InputPorts[1].Data.ToString());
            var inputs = InputPorts[0].Data as List<object>;
            OutputPorts[0].Data = inputs[count];
          
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
            return new GetItemAtIndex(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}