using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;


namespace ENGyn.Nodes.Navisworks
{
    public class ModelsInDocument : Node
    {
        public ModelsInDocument(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW_Document", typeof(Document));
            AddOutputPortToNode("Navis Models", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Get models appended to current document";

        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                Document doc = InputPorts[0].Data as Document;
                var models = new System.Collections.Generic.List<object>();
                foreach (var item in doc.Models)
                {
                    models.Add(item);
                }
                OutputPorts[0].Data = models;
            }
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