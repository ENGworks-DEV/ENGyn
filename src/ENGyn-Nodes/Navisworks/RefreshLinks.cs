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
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                doc.UpdateFiles();
            }
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