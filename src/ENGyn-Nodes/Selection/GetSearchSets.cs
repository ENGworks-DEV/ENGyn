using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System;

namespace ENGyn.Nodes.Selection
{
    public class GetSearchSet : Node
    {
        

        public GetSearchSet(VplControl hostCanvas)
            : base(hostCanvas)
        {
            
            AddOutputPortToNode("SearchSets", typeof(object));

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
                        foreach (SelectionSet item in SelectionSet)
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

        private List<SelectionSet> SelectionSet { get; set; }

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
            return new GetSearchSet(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

    public class CreateSearchSet : Node
    {


        public CreateSearchSet(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Name", typeof(string));
            AddInputPortToNode("Category", typeof(string));
            AddInputPortToNode("Property", typeof(string));
            AddInputPortToNode("Value", typeof(object));
            AddOutputPortToNode("SearchSets", typeof(object));

        }

        public override void Calculate()
        {
            var Name = InputPorts[0].Data;
            var Category  = InputPorts[1].Data;
            var Property = InputPorts[2].Data;
            var Value = InputPorts[3].Data;

            var search =MainTools.RunFunction( createSearchSet, InputPorts);
   


            OutputPorts[0].Data = search;
            
        
       

        }

        private object createSearchSet(object name, object category, object property, object value)
        {
            //https://adndevblog.typepad.com/aec/2012/08/add-search-selectionset-in-net.html

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            //Create a new search object
            Search s = new Search();
            SearchCondition sc = SearchCondition.HasPropertyByDisplayName(category.ToString(), property.ToString());
            s.SearchConditions.Add(sc.EqualValue(VariantData.FromDisplayString(value.ToString())));


            //Set the selection which we wish to search
            s.Selection.SelectAll();
            s.Locations = SearchLocations.DescendantsAndSelf;

            //halt searching below ModelItems which match this
            s.PruneBelowMatch = true;

            //get the resulting collection by applying this search
            ModelItemCollection searchResults = s.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
            SelectionSet selectionSet = new SelectionSet();
            selectionSet.DisplayName = name.ToString();
            selectionSet.CopyFrom(s);
            Autodesk.Navisworks.Api.Application.ActiveDocument.SelectionSets.InsertCopy(0, selectionSet);
            var ss = Autodesk.Navisworks.Api.Application.ActiveDocument.SelectionSets.ToSavedItemCollection();


            return ss;
        }

        public override Node Clone()
        {
            return new CreateSearchSet(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}