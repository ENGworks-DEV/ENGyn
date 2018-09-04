using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;

namespace NW_GraphicPrograming.Nodes
{
    public class NW_Document : Node
    {
        public NW_Document(VplControl hostCanvas)
            : base(hostCanvas)
        {
            //AddInputPortToNode("Navis1", typeof(object));
            AddOutputPortToNode("NW Document", typeof(object));

            AddControlToNode(new Label { Content = "Navisworks Document", FontSize = 13, FontWeight = FontWeights.Bold });

            OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;
            Name = "Get Document";
            Calculate();

        }

        public override void Calculate()
        {
        
            
         OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;
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
            return new NW_Document(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}