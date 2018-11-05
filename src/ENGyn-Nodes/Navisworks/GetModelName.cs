using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;

namespace ENGyn.Nodes.Navisworks
{
    public class CurrentModelName : Node
    {
        public CurrentModelName(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("Navis Model Name", typeof(string));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Get current document's name";
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                OutputPorts[0].Data = doc.CurrentFileName;
            }
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