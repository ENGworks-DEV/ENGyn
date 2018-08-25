using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System.Reflection;
namespace NW_GraphicPrograming.Nodes
{
    public class GetSearchSets : Node
    {
        public GetSearchSets(VplControl hostCanvas)
            : base(hostCanvas)
        {
            
            AddOutputPortToNode("SearchSets", typeof(List<SelectionSet>));

            foreach (Port item in this.OutputPorts)
            {
                
                item.Description = item.Name;
            }

            AddControlToNode(new Label() { Content = "Get SearchSets", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            

            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Returns Get Search Sets" };
            

        }

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            
            List<SelectionSet> searchSets = new List<SelectionSet>();
            SavedItemCollection selectionSets =  doc.SelectionSets;
            foreach (SelectionSet selections in selectionSets)
            {
                if (  selections.HasSearch)
                {
                    searchSets.Add(selections);
                }
            }
            
            OutputPorts[0].Data = searchSets;
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
            return new GetSearchSets(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}