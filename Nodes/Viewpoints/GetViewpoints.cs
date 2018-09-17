using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace ENGyn.Nodes.Viewpoints
{
    public class GetViewpoints : Node
    {
        

        public GetViewpoints(VplControl hostCanvas)
            : base(hostCanvas)
        {
            
            AddOutputPortToNode("Viewpoints", typeof(object));

        }

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            SavedViewpoints = new List<SavedViewpoint>();
            List<SelectionSet> Viewpoints = new List<SelectionSet>();
            if (doc.SavedViewpoints != null)
            { 
            SavedItemCollection viewpoints =  doc.SavedViewpoints.ToSavedItemCollection();
            try {
                foreach (SavedItem view in viewpoints)
                {
                        var t = view.GetType();
                         var name = view.DisplayName;
                        recursionViewpoint(view);
                    
                }
            }
            catch (System.Exception e)
            { System.Windows.MessageBox.Show(e.Message); }
            
            OutputPorts[0].Data = SavedViewpoints;
        }
            }
        public List<SavedViewpoint> SavedViewpoints { get; set; }
        private List<SelectionSet> SelectionSet { get; set; }

        private void recursionViewpoint(object s)
        {
            if (s != null)
            {
                if (s.GetType() == typeof(FolderItem))
                {
                    var folder = s as FolderItem;
                    foreach (var children in folder.Children)
                    {
                        recursionViewpoint(children);
                    }
                }
               if (s.GetType() == typeof(SavedViewpoint))
                {
                    if (SavedViewpoints != null)
                    {
                        SavedViewpoints.Add(s as SavedViewpoint);
                    }

                   
                }
            }

        }


        private SelectionSet IterateSelections(SavedItem savedItem)
        { return null; }
        public void GetSets(SavedItem savedItem)
        {

             SelectionSet = new List<SelectionSet>();

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
                SelectionSet.Add(savedItem as SelectionSet);
            }

            
        }



        public override Node Clone()
        {
            return new GetViewpoints(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}