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


            AddControlToNode(new Label() { Content = "Model Name"});

            

            
            IsResizeable = true;
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                OutputPorts[0].Data = doc.CurrentFileName;
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
            return new NW_ModelName(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}