using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUM.CMS.VplControl.Core;
using System.Windows.Controls;
using System.Xml;
using GroupItem = Autodesk.Navisworks.Api.GroupItem;

namespace ENGyn.Nodes.Clash
{
    public class ClashByProp : Node
    {

        public ClashByProp(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddInputPortToNode("Category", typeof(object));
            AddInputPortToNode("Property", typeof(object));
            //AddInputPortToNode("Allow Subgrouping", typeof(object));
            //AddInputPortToNode("KeepExisting", typeof(object));
            AddOutputPortToNode("Output", typeof(object));


            StackPanel stackPanel = new StackPanel();

            //Grouping Mode

            //KeepExisting
            stackPanel.Children.Add(new Label() { Content = "Selection", Foreground = System.Windows.Media.Brushes.White, VerticalContentAlignment = System.Windows.VerticalAlignment.Bottom });
            ComboBox KeepExistingComboBox2 = new ComboBox() { Items = { "A", "B" } };
            stackPanel.Children.Add(KeepExistingComboBox2);

            AddControlToNode(stackPanel);

            //Help 
            this.BottomComment.Text = "Based on BIM42 add-in this node group clashes in by Property. This nodes can group clashes twice. ";
            this.ShowHelpOnMouseOver = true;
        }



        public override void Calculate()
        {

            var input = InputPorts[0].Data;

            var stack = ControlElements[0] as StackPanel;
            var comboBox = stack.Children[1] as ComboBox;
            string selection = "A";
            if (comboBox.SelectedItem != null)
            {

                selection = comboBox.SelectedItem.ToString();
            }

            var Category = InputPorts[1].Data.ToString();
            var Property = InputPorts[2].Data.ToString();

            OutputPorts[0].Data = input;
            if (MainTools.IsList(input))
            {
                foreach (var item in input as List<object>)
                {
                    GroupByProperty(item, selection,Category,Property);
                }
            }
            else
            {
                GroupByProperty(input, selection, Category, Property);
            }


        }

        public override Node Clone()
        {
            return new ClashByProp(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }


        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);



        }


        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);




        }


        private static int clashSelection;
        private static string clashCategory;
        private static string clashProperty;
        #region ClashGroup




        /// <summary>
        ///  Group test by property
        /// </summary>
        /// <param name="clashTest"></param>
        /// <param name="Selection"></param>
        /// <param name="Category"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public static object GroupByProperty(object clashTest, string Selection, string Category, string Property)
        {
            if (string.IsNullOrWhiteSpace(Category) || string.IsNullOrWhiteSpace(Property))
            {
                return null;
            }
            clashSelection = Selection == "A" ? 0:1;
            clashCategory = Category;
            clashProperty = Property;
            var test = GroupClash(clashTest, (GroupingMode.Properties));
            return test;
        }



        internal static object GroupClash(object clashTest, GroupingMode group)
        {
            if (group > 0)
            {
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                var testData = doc.GetClash().TestsData;
                var ClashFromReference = doc.ResolveReference(clashTest as SavedItemReference) as ClashTest;

                GroupingFunctions.NewListOfClashes = new List<SavedItemReference>();

                GroupingFunctions.GroupClashes(ClashFromReference, group);
            }


            return clashTest;
        }

        internal class GroupingFunctions
        {
            internal static List<SavedItemReference> NewListOfClashes { get; set; }



            internal static void GroupClashes(ClashTest selectedClashTest, GroupingMode groupingMode, bool keepExistingGroups = false)
            {
                //Get existing clash result
                List<ClashResult> clashResults = GetIndividualClashResults(selectedClashTest, keepExistingGroups).ToList();
                List<ClashResultGroup> clashResultGroups = new List<ClashResultGroup>();

                //Create groups according to the first grouping mode
                GetExistingGroups(ref clashResultGroups, selectedClashTest);

                if (clashResultGroups.Count == 0)
                {
                    CreateGroup(ref clashResultGroups, groupingMode, clashResults, "");
                }
                else
                {
                    //Optionnaly, create subgroups
                    if (groupingMode != GroupingMode.None)
                    {
                        CreateSubGroups(ref clashResultGroups, groupingMode);
                    }
                }


                //Remove groups with only one clash
                List<ClashResult> ungroupedClashResults = RemoveOneClashGroup(ref clashResultGroups);

                //Backup the existing group, if necessary
                if (keepExistingGroups) clashResultGroups.AddRange(BackupExistingClashGroups(selectedClashTest));

                //Process these groups and clashes into the clash test
                ProcessClashGroup(clashResultGroups, ungroupedClashResults, selectedClashTest);
            }

            private static void GetExistingGroups(ref List<ClashResultGroup> clashResultGroups, ClashTest selectedClashTest)
            {
                foreach (var item in selectedClashTest.Children)
                {
                    if (item.IsGroup)
                    {
                        clashResultGroups.Add((ClashResultGroup)item);
                    }

                }
            }

            private static void CreateGroup(ref List<ClashResultGroup> clashResultGroups, GroupingMode groupingMode, List<ClashResult> clashResults, string initialName)
            {
                //group all clashes
                switch (groupingMode)
                {
                    case GroupingMode.None:
                        return;
                    case GroupingMode.Level:
                        clashResultGroups = GroupByLevel(clashResults, initialName);
                        break;
                    case GroupingMode.GridIntersection:
                        clashResultGroups = GroupByGridIntersection(clashResults, initialName);
                        break;
                    case GroupingMode.SelectionA:
                    case GroupingMode.SelectionB:
                        clashResultGroups = GroupByElementOfAGivenSelection(clashResults, groupingMode, initialName);
                        break;
                    case GroupingMode.ApprovedBy:
                    case GroupingMode.AssignedTo:
                    case GroupingMode.Status:
                        clashResultGroups = GroupByProperties(clashResults, groupingMode, initialName);
                        break;
                    case GroupingMode.Properties:
                        clashResultGroups = GroupByModelItemProperties(clashResults, clashSelection, clashCategory, clashProperty, initialName);
                        break;
                }
            }

            private static void CreateSubGroups(ref List<ClashResultGroup> clashResultGroups, GroupingMode mode)
            {
                List<ClashResultGroup> clashResultSubGroups = new List<ClashResultGroup>();

                foreach (ClashResultGroup group in clashResultGroups)
                {

                    List<ClashResult> clashResults = new List<ClashResult>();

                    foreach (SavedItem item in group.Children)
                    {
                        ClashResult clashResult = item as ClashResult;
                        if (clashResult != null)
                        {
                            clashResults.Add(clashResult);
                        }
                    }

                    List<ClashResultGroup> clashResultTempSubGroups = new List<ClashResultGroup>();
                    CreateGroup(ref clashResultTempSubGroups, mode, clashResults, group.DisplayName + "_");
                    clashResultSubGroups.AddRange(clashResultTempSubGroups);
                }

                clashResultGroups = clashResultSubGroups;
            }

            internal static void UnGroupClashes(ClashTest selectedClashTest)
            {
                List<ClashResultGroup> groups = new List<ClashResultGroup>();
                List<ClashResult> results = GetIndividualClashResults(selectedClashTest, false).ToList();
                List<ClashResult> copiedResult = new List<ClashResult>();

                foreach (ClashResult result in results)
                {
                    copiedResult.Add((ClashResult)result.CreateCopy());
                }

                //Process this empty group list and clashes into the clash test
                ProcessClashGroup(groups, copiedResult, selectedClashTest);

            }

            #region grouping functions
            private static List<ClashResultGroup> GroupByLevel(List<ClashResult> results, string initialName)
            {
                //I already checked if it exists
                GridSystem gridSystem = Application.MainDocument.Grids.ActiveSystem;
                Dictionary<GridLevel, ClashResultGroup> groups = new Dictionary<GridLevel, ClashResultGroup>();

                //Create a group for the null GridIntersection
                ClashResultGroup nullGridGroup = new ClashResultGroup
                {
                    DisplayName = initialName + "No Level"
                };

                foreach (ClashResult result in results)
                {
                    //Cannot add original result to new clash test, so I create a copy
                    ClashResult copiedResult = (ClashResult)result.CreateCopy();

                    if (gridSystem.ClosestIntersection(copiedResult.Center) != null)
                    {
                        GridLevel closestLevel = gridSystem.ClosestIntersection(copiedResult.Center).Level;

                        if (!groups.TryGetValue(closestLevel, out ClashResultGroup currentGroup))
                        {
                            currentGroup = new ClashResultGroup();
                            string displayName = closestLevel.DisplayName;
                            if (string.IsNullOrEmpty(displayName)) { displayName = "Unnamed Level"; }
                            currentGroup.DisplayName = initialName + displayName;
                            groups.Add(closestLevel, currentGroup);
                        }
                        currentGroup.Children.Add(copiedResult);
                    }
                    else
                    {
                        nullGridGroup.Children.Add(copiedResult);
                    }
                }

                IOrderedEnumerable<KeyValuePair<GridLevel, ClashResultGroup>> list = groups.OrderBy(key => key.Key.Elevation);
                groups = list.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                List<ClashResultGroup> groupsByLevel = groups.Values.ToList();
                if (nullGridGroup.Children.Count != 0) groupsByLevel.Add(nullGridGroup);

                return groupsByLevel;
            }

            private static List<ClashResultGroup> GroupByGridIntersection(List<ClashResult> results, string initialName)
            {
                //I already check if it exists
                GridSystem gridSystem = Application.MainDocument.Grids.ActiveSystem;
                Dictionary<GridIntersection, ClashResultGroup> groups = new Dictionary<GridIntersection, ClashResultGroup>();

                //Create a group for the null GridIntersection
                ClashResultGroup nullGridGroup = new ClashResultGroup
                {
                    DisplayName = initialName + "No Grid intersection"
                };

                foreach (ClashResult result in results)
                {
                    //Cannot add original result to new clash test, so I create a copy
                    ClashResult copiedResult = (ClashResult)result.CreateCopy();

                    if (gridSystem.ClosestIntersection(copiedResult.Center) != null)
                    {

                        GridIntersection closestGridIntersection = gridSystem.ClosestIntersection(copiedResult.Center);

                        if (!groups.TryGetValue(closestGridIntersection, out ClashResultGroup currentGroup))
                        {
                            currentGroup = new ClashResultGroup();
                            string displayName = closestGridIntersection.DisplayName;
                            if (string.IsNullOrEmpty(displayName)) { displayName = "Unnamed Grid Intersection"; }
                            currentGroup.DisplayName = initialName + displayName;
                            groups.Add(closestGridIntersection, currentGroup);
                        }
                        currentGroup.Children.Add(copiedResult);
                    }
                    else
                    {
                        nullGridGroup.Children.Add(copiedResult);
                    }
                }

                IOrderedEnumerable<KeyValuePair<GridIntersection, ClashResultGroup>> list = groups.OrderBy(key => key.Key.Position.X).OrderBy(key => key.Key.Level.Elevation);
                groups = list.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                List<ClashResultGroup> groupsByGridIntersection = groups.Values.ToList();
                if (nullGridGroup.Children.Count != 0) groupsByGridIntersection.Add(nullGridGroup);

                return groupsByGridIntersection;
            }

            private static List<ClashResultGroup> GroupByElementOfAGivenSelection(List<ClashResult> results, GroupingMode mode, string initialName)
            {
                Dictionary<ModelItem, ClashResultGroup> groups = new Dictionary<ModelItem, ClashResultGroup>();
                List<ClashResultGroup> emptyClashResultGroups = new List<ClashResultGroup>();

                foreach (ClashResult result in results)
                {

                    //Cannot add original result to new clash test, so I create a copy
                    ClashResult copiedResult = (ClashResult)result.CreateCopy();
                    ModelItem modelItem = null;

                    if (mode == GroupingMode.SelectionA)
                    {
                        if (copiedResult.CompositeItem1 != null)
                        {
                            modelItem = GetSignificantAncestorOrSelf(copiedResult.CompositeItem1);
                        }
                        else if (copiedResult.CompositeItem2 != null)
                        {
                            modelItem = GetSignificantAncestorOrSelf(copiedResult.CompositeItem2);
                        }
                    }
                    else if (mode == GroupingMode.SelectionB)
                    {
                        if (copiedResult.CompositeItem2 != null)
                        {
                            modelItem = GetSignificantAncestorOrSelf(copiedResult.CompositeItem2);
                        }
                        else if (copiedResult.CompositeItem1 != null)
                        {
                            modelItem = GetSignificantAncestorOrSelf(copiedResult.CompositeItem1);
                        }
                    }

                    string displayName = "Empty clash";
                    if (modelItem != null)
                    {
                        displayName = modelItem.DisplayName;
                        //Create a group
                        if (!groups.TryGetValue(modelItem, out ClashResultGroup currentGroup))
                        {
                            currentGroup = new ClashResultGroup();
                            if (string.IsNullOrEmpty(displayName)) { displayName = modelItem.Parent.DisplayName; }
                            if (string.IsNullOrEmpty(displayName)) { displayName = "Unnamed Parent"; }
                            currentGroup.DisplayName = initialName + displayName;
                            groups.Add(modelItem, currentGroup);
                        }

                        //Add to the group
                        currentGroup.Children.Add(copiedResult);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("test");
                        ClashResultGroup oneClashResultGroup = new ClashResultGroup
                        {
                            DisplayName = "Empty clash"
                        };
                        oneClashResultGroup.Children.Add(copiedResult);
                        emptyClashResultGroups.Add(oneClashResultGroup);
                    }




                }

                List<ClashResultGroup> allGroups = groups.Values.ToList();
                allGroups.AddRange(emptyClashResultGroups);
                return allGroups;
            }

            private static List<ClashResultGroup> GroupByProperties(List<ClashResult> results, GroupingMode mode, string initialName)
            {
                Dictionary<string, ClashResultGroup> groups = new Dictionary<string, ClashResultGroup>();

                foreach (ClashResult result in results)
                {
                    //Cannot add original result to new clash test, so I create a copy
                    ClashResult copiedResult = (ClashResult)result.CreateCopy();
                    string clashProperty = null;

                    if (mode == GroupingMode.ApprovedBy)
                    {
                        clashProperty = copiedResult.ApprovedBy;
                    }
                    else if (mode == GroupingMode.AssignedTo)
                    {
                        clashProperty = copiedResult.AssignedTo;
                    }
                    else if (mode == GroupingMode.Status)
                    {
                        clashProperty = copiedResult.Status.ToString();
                    }

                    if (string.IsNullOrEmpty(clashProperty)) { clashProperty = "Unspecified"; }

                    if (!groups.TryGetValue(clashProperty, out ClashResultGroup currentGroup))
                    {
                        currentGroup = new ClashResultGroup
                        {
                            DisplayName = initialName + clashProperty
                        };
                        groups.Add(clashProperty, currentGroup);
                    }
                    currentGroup.Children.Add(copiedResult);
                }

                return groups.Values.ToList();
            }


            private static List<ClashResultGroup> GroupByModelItemProperties(List<ClashResult> results, int Selection, string Category, string Property, string initialName)
            {
                Dictionary<string, ClashResultGroup> groups = new Dictionary<string, ClashResultGroup>();

                foreach (ClashResult result in results)
                {
                    //Cannot add original result to new clash test, so I create a copy
                    ClashResult copiedResult = (ClashResult)result.CreateCopy();
                    string clashProperty = null;
                    DataProperty vProp = null;
                    if (Selection == 1)
                    {
                        vProp = copiedResult.Item2.PropertyCategories.FindPropertyByDisplayName(Category, Property);
                        if (vProp == null)
                        {
                            vProp = copiedResult.Item2.Parent.PropertyCategories.FindPropertyByDisplayName(Category, Property);
                        }
                    }
                    else
                    {


                        vProp = copiedResult.Item1.PropertyCategories.FindPropertyByDisplayName(Category, Property);
                        if (vProp == null)
                        {
                            vProp = copiedResult.Item1.Parent.PropertyCategories.FindPropertyByDisplayName(Category, Property);
                        }
                    }

                    if (vProp != null && vProp.Value != null)
                    {
                        clashProperty = vProp.Value.ToDisplayString();
                    }

                    if (string.IsNullOrEmpty(clashProperty)) { clashProperty = "NotValid"; }

                    if (!groups.TryGetValue(clashProperty, out ClashResultGroup currentGroup))
                    {
                        currentGroup = new ClashResultGroup
                        {
                            DisplayName = initialName + clashProperty
                        };
                        groups.Add(clashProperty, currentGroup);
                    }
                    currentGroup.Children.Add(copiedResult);
                }

                return groups.Values.ToList();
            }


            #endregion


            #region helpers
            private static void ProcessClashGroup(List<ClashResultGroup> clashGroups, List<ClashResult> ungroupedClashResults, ClashTest selectedClashTest)
            {

                using (Transaction tx = Application.MainDocument.BeginTransaction("Group clashes"))
                {
                    ClashTest copiedClashTest = (ClashTest)selectedClashTest.CreateCopyWithoutChildren();
                    //When we replace theTest with our new test, theTest will be disposed. If the operation is cancelled, we need a non-disposed copy of theTest with children to sub back in.
                    ClashTest BackupTest = (ClashTest)selectedClashTest.CreateCopy();
                    DocumentClash documentClash = Application.MainDocument.GetClash();
                    int indexOfClashTest = documentClash.TestsData.Tests.IndexOf(selectedClashTest);
                    //Save reference of new clash
                    documentClash.TestsData.TestsReplaceWithCopy(indexOfClashTest, copiedClashTest);

                    int CurrentProgress = 0;
                    int TotalProgress = ungroupedClashResults.Count + clashGroups.Count;
                    Progress ProgressBar = Application.BeginProgress("Copying Results", "Copying results from " + selectedClashTest.DisplayName + " to the Group Clashes pane...");
                    foreach (ClashResultGroup clashResultGroup in clashGroups)
                    {
                        if (ProgressBar.IsCanceled) break;
                        documentClash.TestsData.TestsAddCopy((GroupItem)documentClash.TestsData.Tests[indexOfClashTest], clashResultGroup);

                        CurrentProgress++;
                        ProgressBar.Update((double)CurrentProgress / TotalProgress);
                    }
                    foreach (ClashResult clashResult in ungroupedClashResults)
                    {
                        if (ProgressBar.IsCanceled) break;
                        documentClash.TestsData.TestsAddCopy((GroupItem)documentClash.TestsData.Tests[indexOfClashTest], clashResult);
                        CurrentProgress++;
                        ProgressBar.Update((double)CurrentProgress / TotalProgress);
                    }
                    if (ProgressBar.IsCanceled) documentClash.TestsData.TestsReplaceWithCopy(indexOfClashTest, BackupTest);
                    tx.Commit();


                    NewListOfClashes.Add(documentClash.TestsData.CreateReference(documentClash.TestsData.Tests[indexOfClashTest]));
                    Application.EndProgress();
                }
            }

            private static List<ClashResult> RemoveOneClashGroup(ref List<ClashResultGroup> clashResultGroups)
            {
                List<ClashResult> ungroupedClashResults = new List<ClashResult>();
                List<ClashResultGroup> temporaryClashResultGroups = new List<ClashResultGroup>();
                temporaryClashResultGroups.AddRange(clashResultGroups);

                foreach (ClashResultGroup group in temporaryClashResultGroups)
                {
                    if (group.Children.Count == 1)
                    {
                        ClashResult result = (ClashResult)group.Children.FirstOrDefault();
                        result.DisplayName = group.DisplayName;
                        ungroupedClashResults.Add(result);
                        clashResultGroups.Remove(group);
                    }
                }

                return ungroupedClashResults;
            }

            private static IEnumerable<ClashResult> GetIndividualClashResults(ClashTest clashTest, bool keepExistingGroup)
            {
                for (var i = 0; i < clashTest.Children.Count; i++)
                {
                    if (clashTest.Children[i].IsGroup)
                    {
                        if (!keepExistingGroup)
                        {
                            IEnumerable<ClashResult> GroupResults = GetGroupResults((ClashResultGroup)clashTest.Children[i]);
                            foreach (ClashResult clashResult in GroupResults)
                            {
                                yield return clashResult;
                            }
                        }
                    }
                    else yield return (ClashResult)clashTest.Children[i];
                }
            }

            private static IEnumerable<ClashResultGroup> BackupExistingClashGroups(ClashTest clashTest)
            {
                for (var i = 0; i < clashTest.Children.Count; i++)
                {
                    if (clashTest.Children[i].IsGroup)
                    {
                        yield return (ClashResultGroup)clashTest.Children[i].CreateCopy();
                    }
                }
            }

            private static IEnumerable<ClashResult> GetGroupResults(ClashResultGroup clashResultGroup)
            {
                for (var i = 0; i < clashResultGroup.Children.Count; i++)
                {
                    yield return (ClashResult)clashResultGroup.Children[i];
                }
            }

            private static ModelItem GetSignificantAncestorOrSelf(ModelItem item)
            {
                ModelItem originalItem = item;
                ModelItem currentComposite = null;

                //Get last composite item.
                while (item.Parent != null)
                {
                    item = item.Parent;
                    if (item.IsComposite) currentComposite = item;
                }

                return currentComposite ?? originalItem;
            }
            #endregion

        }


        internal enum GroupingMode
        {

            [Description("<None>")]
            None,
            [Description("Level")]
            Level,
            [Description("Grid Intersection")]
            GridIntersection,
            [Description("Selection A")]
            SelectionA,
            [Description("Selection B")]
            SelectionB,
            [Description("Assigned To")]
            AssignedTo,
            [Description("Approved By")]
            ApprovedBy,
            [Description("Status")]
            Status,
            [Description("Properties")]
            Properties,

        }

        #endregion

    }
}


