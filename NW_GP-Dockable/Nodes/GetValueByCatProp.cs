using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;

namespace NW_GraphicPrograming.Nodes
{
    public class GetValueByCatProp : Node
    {
        public GetValueByCatProp(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ModelItem", typeof(object));
            AddInputPortToNode("Category", typeof(object));
            AddInputPortToNode("Property", typeof(object));
            AddOutputPortToNode("Value", typeof(object));

            AddControlToNode(new Label { Content = "NW GetValueByCatProp" });
            var label = new Label
            {
                Content = "NW GetValueByCatProp",
                FontSize = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
        }

        public override void Calculate()
        {

            
            var sel = InputPorts[0].Data;
            List<object> modelItems = new List<object>();
            var category = InputPorts[1].Data.ToString();
            var property = InputPorts[2].Data.ToString();
            foreach (var s in sel as List<ModelItem>)
            {
                var value = s.PropertyCategories.FindPropertyByDisplayName(category, property).Value;
                modelItems.Add(value);
            }

            OutputPorts[0].Data = modelItems;
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
            return new GetValueByCatProp(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}