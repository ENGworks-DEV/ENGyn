using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Selection
{
    public class GetCurrentSelection : Node
    {
        public GetCurrentSelection(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("Selection", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Returns elements in current selection";
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                var sel = doc.CurrentSelection.SelectedItems;
                List<object> modelItems = new List<object>();
                foreach (var s in sel)
                {
                    modelItems.Add(s);
                }
                OutputPorts[0].Data = modelItems;
            }
        }

        public override Node Clone()
        {
            return new GetCurrentSelection(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}