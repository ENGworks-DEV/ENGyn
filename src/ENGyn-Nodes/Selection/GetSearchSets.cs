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
using System.Linq;
using Autodesk.Navisworks.Api.DocumentParts;

namespace ENGyn.Nodes.Selection
{
    public class GetSearchSets : Node
    {
        

        public GetSearchSets(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("object", typeof(object));
            AddOutputPortToNode("SearchSets", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Return all SelectionSets and SearchSets available in current model. SearchSets can be filters using GetAPIParameterValue and quering HasSearch for true";
        }

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            
            List<object> searchSets = new List<object>();
            if (doc.SelectionSets != null)
            { 
            SavedItemCollection selectionSets =  doc.SelectionSets;
            try {
                foreach (SavedItem selections in selectionSets)
                {


                    GetSets(selections);
                        foreach (SelectionSet item in SelectionSet)
                        {
                            if (item != null)
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
            return new GetSearchSets(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

    public class CreateSimpleSearchSet : Node
    {


        public CreateSimpleSearchSet(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Name", typeof(string));
            AddInputPortToNode("Category", typeof(string));
            AddInputPortToNode("Property", typeof(string));
            AddInputPortToNode("Value", typeof(object));
            AddOutputPortToNode("SearchSets", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Create a SearchSet based on a simple search";
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
            return new CreateSimpleSearchSet(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

    public class CreateSearch : Node
    {


        public CreateSearch(VplControl hostCanvas)
            : base(hostCanvas)
        {
            
            AddInputPortToNode("Category", typeof(string));
            AddInputPortToNode("Property", typeof(string));
            AddInputPortToNode("Value", typeof(object));
            AddInputPortToNode("Negate", typeof(string));
            AddOutputPortToNode("SearchSets", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Create a Search based on Category, Parameter & Value. Optional, true to Negate ";
        }

        public override void Calculate()
        {
            

            var search = MainTools.RunFunction(createSearch, InputPorts);

            OutputPorts[0].Data = search;

        }

        private object createSearch(object category, object property, object value, object Negate)
        {
            //https://adndevblog.typepad.com/aec/2012/08/add-search-selectionset-in-net.html

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            //Create a new search object
            Search s = new Search();
            SearchCondition sc = SearchCondition.HasPropertyByDisplayName(category.ToString(), property.ToString());
            bool negate = Negate != null ? bool.Parse(Negate.ToString()) : false;
            if (negate)
            { sc  = sc.Negate(); }

            
            s.SearchConditions.Add(sc.EqualValue(VariantData.FromDisplayString(value.ToString())));
            
            
            //Set the selection which we wish to search
            s.Selection.SelectAll();
            s.Locations = SearchLocations.DescendantsAndSelf;

            //halt searching below ModelItems which match this
            s.PruneBelowMatch = true;

            return s;
        }

        public override Node Clone()
        {
            return new CreateSearch(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

    public class CreateSearchSetFromSearch : Node
    {


        public CreateSearchSetFromSearch(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Name", typeof(string));
            
            AddInputPortToNode("Search", typeof(object));
            AddOutputPortToNode("SearchSets", typeof(object));
            
            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Create a SearchSet based on a Search";
        }

        public override void Calculate()
        {

            var search = MainTools.RunFunction(createSearchSetFromSearch, InputPorts);

            OutputPorts[0].Data = search;

        }

        private object createSearchSetFromSearch(object name, object search)
        {
            //https://adndevblog.typepad.com/aec/2012/08/add-search-selectionset-in-net.html

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            object ss = null;
            if (search is Search)
            {
                var currentSearch = new Search(search as Search);
                currentSearch.PruneBelowMatch = false;
                currentSearch.Selection.SelectAll();

                currentSearch.Locations = SearchLocations.DescendantsAndSelf;
                SavedItem selectionSet = new SelectionSet(currentSearch);
            selectionSet.DisplayName = name.ToString();
    
            
            Autodesk.Navisworks.Api.Application.ActiveDocument.SelectionSets.InsertCopy(0, selectionSet);
             Autodesk.Navisworks.Api.Application.ActiveDocument.SelectionSets.ToSavedItemCollection();

            }
            return ss;

            
        }

        public override Node Clone()
        {
            return new CreateSearchSetFromSearch(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

    public class SetSearchRelation : Node
    {
        private string relation;

        public SetSearchRelation(VplControl hostCanvas)
            : base(hostCanvas)
        {
            

            AddInputPortToNode("SearchA", typeof(object));
            AddInputPortToNode("SearchB", typeof(object));
            StackPanel stackPanel = new StackPanel();

            //Grouping Mode

            stackPanel.Children.Add(new Label() { Content = "Relationship", Foreground = System.Windows.Media.Brushes.White, VerticalContentAlignment = System.Windows.VerticalAlignment.Bottom });
            ComboBox Relationship = new ComboBox() { ItemsSource = new List<string>() { "AND", "OR" } };
            stackPanel.Children.Add(Relationship);
            AddControlToNode(stackPanel);

            AddOutputPortToNode("SearchSets", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Create an AND/OR relation between Searchs";
        }

        public override void Calculate()
        {
            var stack = ControlElements[0] as StackPanel;
            //Basic grouping
            var RelationComboBox = stack.Children[1] as ComboBox;
            relation = "AND";
            if (RelationComboBox.SelectedItem != null && RelationComboBox.SelectedItem.GetType() == typeof(string))
            {
                
                relation = RelationComboBox.SelectedItem.ToString();
            }


            var search = MainTools.RunFunction(setSearchRelation, InputPorts);

            OutputPorts[0].Data = search;

        }

        private object setSearchRelation(object SearchA, object searchB)
        {
            //https://adndevblog.typepad.com/aec/2012/08/add-search-selectionset-in-net.html

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            Search ss = new Search();
            if (relation == "AND")
            { 

                ss.SearchConditions.Add(((Search)SearchA).SearchConditions[0]);
                ss.SearchConditions.Add(((Search)searchB).SearchConditions[0]);
            
            }
            else
                {
                    ss.SearchConditions.AddGroup(((Search)SearchA).SearchConditions);
                    ss.SearchConditions.AddGroup(((Search)searchB).SearchConditions);
                }
            return ss;


        }

        public override Node Clone()
        {
            return new SetSearchRelation(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }

        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            var stack = ControlElements[0] as StackPanel;

            var GroupingComboBox = stack.Children[1] as ComboBox;
            if (GroupingComboBox != null)
            {
                xmlWriter.WriteStartAttribute("SelectedIndex-Relation");
                xmlWriter.WriteValue(GroupingComboBox.SelectedIndex);
                xmlWriter.WriteEndAttribute();

            }


        }


        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            var stack = ControlElements[0] as StackPanel;

            var GroupingComboBox = stack.Children[1] as ComboBox;
            if (GroupingComboBox != null)
            {
                var value = xmlReader.GetAttribute("SelectedIndex-Relation");
                var index = Convert.ToInt32(value);
                GroupingComboBox.SelectedIndex = index;
            }

            
        }

    }

    public class SelectionSetFromSelection : Node
    {
        private string relation;

        public SelectionSetFromSelection(VplControl hostCanvas)
            : base(hostCanvas)
        {


            AddInputPortToNode("Name", typeof(object));
            AddInputPortToNode("ModelItems", typeof(object));


            AddOutputPortToNode("Selectionset", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Create SelectionSet from selection or list of ModelItems";
        }

        public override void Calculate()
        {




            OutputPorts[0].Data = CreateSelectionSet(InputPorts[0].Data, InputPorts[1].Data);

        }

        private object CreateSelectionSet(object Name, object Selection)
        {
            //https://adndevblog.typepad.com/aec/2012/08/add-search-selectionset-in-net.html

            Document oDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            DocumentSelectionSets oCurSets = oDoc.SelectionSets;

            // my set1: current selection
            if (MainTools.IsList(Selection))
            {
                if ((IList<object>)Selection != null)
                {
                    ModelItemCollection modelItems = new ModelItemCollection();
                    var m = (IList<object>)Selection;
                    foreach (var item in m)
                    {
                        modelItems.Add(item as ModelItem);
                    }
                    

                    SelectionSet ss = new SelectionSet(modelItems);
                    ss.DisplayName = Name.ToString();
                    oCurSets.AddCopy(ss);
                    return ss;
                }
            }

            if (Selection is SelectionSet)
            {
                SelectionSet ss = new SelectionSet((SelectionSet)Selection);
                ss.DisplayName = Name.ToString();
                oCurSets.AddCopy(ss);
                return ss;
            }
            else
            {
                return null;
            }

        }

        public override Node Clone()
        {
            return new SelectionSetFromSelection(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }

    }
}