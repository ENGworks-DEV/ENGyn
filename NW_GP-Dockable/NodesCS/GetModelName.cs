using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

namespace NW_GraphicPrograming.Nodes
{
    public class NW_ModelName : Node
    {
        public NW_ModelName(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("Navis Model Name", typeof(string));


            //TODO: input as part of the point.Below, temporary solution : One label per input

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

            AddControlToNode(new Label() { Content = "Model Name", FontSize = 13 });

            this.Name = "Model Name";

            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Returns Model Name" };
            IsResizeable = true;
        }

        public override void Calculate()
        {

            Document doc = InputPorts[0].Data as Document;
         OutputPorts[0].Data = doc.CurrentFileName;
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
            return new NW_ModelName(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}