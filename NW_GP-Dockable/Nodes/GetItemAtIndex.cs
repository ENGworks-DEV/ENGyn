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

            AddInputPortToNode("Index", typeof(int));
            AddInputPortToNode("List", typeof(List<object>));
            AddOutputPortToNode("result", typeof(object));
            AddControlToNode(new Label { Content = "GetItemAtIndex" });

            foreach (Port item in this.InputPorts)
            {
                item.ToolTip = item.Name;
                // AddControlToNode(new Label() { Content = item.Name, FontSize = 13 });
            }

            AddControlToNode(new Label() { Content = "Get item at index", FontSize = 13 });

            IsResizeable = true;
       
        }

        public override void Calculate()
        {
            var count = Int32.Parse( InputPorts[0].Data.ToString());
            var inputs = InputPorts[1].Data as List<object>;
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