using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;

namespace NW_GraphicPrograming.Nodes.Navisworks
{
    public class ModelsInDocument : Node
    {
        public ModelsInDocument(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW_Document", typeof(Document));
            AddOutputPortToNode("Navis Models Count", typeof(object));

 
            AddControlToNode(new Label { Content = "Models in file", FontSize = 13, FontWeight = FontWeights.Bold });
            Name = "Get Models in file";
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                OutputPorts[0].Data = doc.Models.Count;
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
            return new ModelsInDocument(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}