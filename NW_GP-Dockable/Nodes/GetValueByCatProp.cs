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

            //TODO: input as part of the point.Below, temporary solution : One label per input

            foreach (Port item in this.InputPorts)
            {
                item.ToolTip = item.Name;
               // AddControlToNode(new Label() { Content = item.Name, FontSize = 13 });
            }

            AddControlToNode(new Label() { Content = "Get Value By Cat Prop", FontSize = 13 });
          
            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Returns value by category and property" };
            IsResizeable = true;


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