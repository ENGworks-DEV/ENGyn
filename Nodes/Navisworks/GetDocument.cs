using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;

namespace ENGyn.Nodes.Navisworks
{
    public class CurrentDocument : Node
    {
        public CurrentDocument(VplControl hostCanvas)
            : base(hostCanvas)
        {
 
            AddOutputPortToNode("NW Document", typeof(object));

            OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;

            Calculate();

        }

        public override void Calculate()
        {
        
            
         OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;
        }


        public override Node Clone()
        {
            return new CurrentDocument(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}