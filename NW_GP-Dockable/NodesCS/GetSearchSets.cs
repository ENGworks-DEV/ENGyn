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
            if (doc.SelectionSets != null)
            { 
            SavedItemCollection selectionSets =  doc.SelectionSets;
            try {
                foreach (SavedItem selections in selectionSets)
                {


                    GetSets(selections);
                        foreach (SelectionSet item in selectionSet)
                        {
                            if (item != null && item.HasSearch)
                            {
                                searchSets.Add(item);
                            }
                        }
     
                }
            }
            catch (System.Exception e)
            { System.Windows.MessageBox.Show(e.Message); }
            
            OutputPorts[0].Data = searchSets;
        }
            }

        public List<SelectionSet> selectionSet { get; private set; }

        public SelectionSet iterateSelections(SavedItem savedItem)
        { return null; }
        public void GetSets(SavedItem savedItem)
        {

             selectionSet = new List<SelectionSet>();

            if (savedItem.GetType() == typeof(FolderItem))
            {
                var folder = savedItem as FolderItem;

                foreach (SavedItem si in folder.Children)
                {
                    GetSets(si);
                }
            }
            else
            {
                selectionSet.Add(savedItem as SelectionSet);
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
            return new GetSearchSets(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}