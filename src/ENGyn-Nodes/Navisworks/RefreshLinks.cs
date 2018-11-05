using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;

namespace ENGyn.Nodes.Navisworks
{
    public class RefreshLinks : Node
    {
        public RefreshLinks(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW_Document", typeof(Document));
            AddOutputPortToNode("NW_Document", typeof(Document));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Refresh all links (NWC, NWD) in current document";
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                OutputPorts[0].Data= RefreshLink(InputPorts[0].Data);
            }
        }

        private object RefreshLink(object input)
        {
            Document doc = input as Document;
            doc.UpdateFiles();
            return doc;
        }


        public override Node Clone()
        {
            return new RefreshLinks(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}