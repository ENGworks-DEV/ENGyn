using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows;

namespace NW_GraphicPrograming.Nodes.Navisworks
{
    public class GetSelection : Node
    {
        public GetSelection(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("Selection", typeof(object));


            AddControlToNode(new Label() { Content = "Get Selection", FontSize = 13, FontWeight = FontWeights.Bold });


        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                var sel = doc.CurrentSelection.SelectedItems;
                List<ModelItem> modelItems = new List<ModelItem>();
                foreach (var s in sel)
                {
                    modelItems.Add(s);
                }
                OutputPorts[0].Data = modelItems;
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
            return new CurrentModelName(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}