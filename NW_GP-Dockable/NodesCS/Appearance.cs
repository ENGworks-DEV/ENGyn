using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using appearance = Autodesk.Navisworks.AutoAppearancePlugin ;
using System.Reflection;
namespace NW_GraphicPrograming.Nodes
{
    public class Appearance : Node
    {
        public Appearance(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("Selection", typeof(object));


            foreach (Port item in this.InputPorts)
            {
                //item.ToolTip = item.DataType.ToString();
                item.Description = item.Name;

            }

            foreach (Port item in this.OutputPorts)
            {
                //item.ToolTip = item.DataType.ToString();
                item.Description = item.Name;
            }

            AddControlToNode(new Label() { Content = "Get Selection", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            

            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Returns current selection" };
            IsResizeable = true;

        }

        public override void Calculate()
        {

            var ap = new appearance.ViewModel.AutoAppearanceVM();
            System.Console.Write(ap.Uid);

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
            return new Appearance(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}