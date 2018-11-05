using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using Autodesk.Navisworks.Api.Clash;

namespace ENGyn.Nodes.Navisworks
{
    public class CurrentDocument : Node
    {
        public CurrentDocument(VplControl hostCanvas)
            : base(hostCanvas)
        {
 
            AddOutputPortToNode("NW Document", typeof(object));
            AddInputPortToNode("NW Object", typeof(object));

            OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;

            Calculate();


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Get current Navisworks document";

        }

        public override void Calculate()
        {
            //Just a place holder. Gives the hability to connect the node to a lower stage in the execution tree
            var input = InputPorts[0].Data;
            
         OutputPorts[0].Data = GetDocument();
        }


        public object GetDocument()
        {

       return Autodesk.Navisworks.Api.Application.ActiveDocument; 
            

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