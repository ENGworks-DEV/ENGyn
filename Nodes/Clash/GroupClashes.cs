using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using Autodesk.Navisworks.Api.DocumentParts; //For grids data
using Application = Autodesk.Navisworks.Api.Application;
using GroupItem = Autodesk.Navisworks.Api.GroupItem;
using System.ComponentModel;

namespace ENGyn.Nodes.Clash
{
    public class GroupByCluster : Node
    {
        public GroupByCluster(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddInputPortToNode("# of Clusters", typeof(int));
            AddInputPortToNode("# of Attempts", typeof(int));
            AddOutputPortToNode("ClashTest", typeof(object));


        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            ProcessClash(input);
            OutputPorts[0].Data = ClashGrouperUtils.NewListOfClashes;

        }

        private void ProcessClash(object input)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {

                var type = input.GetType();
                if (type == typeof(ClashTest))
                {

                    var item = input;
                    if (item.GetType() == typeof(SavedItemReference))
                    {
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                        gInfo.NumClusters = int.Parse(InputPorts[1].Data.ToString());
                        gInfo.NumAttempts = int.Parse(InputPorts[2].Data.ToString());

                        var ClashTest = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                        ClashGrouperUtils.GroupTestClashes(ClashTest, GroupingModes.ClusterAnalysis, gInfo);
                    }
                }
                if (MainTools.IsList(input))
                {

                    
                    foreach (var item in input as List<object>)
                    {
                        if (item.GetType() == typeof(SavedItemReference))
                        {
                            ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                            gInfo.NumClusters = int.Parse(InputPorts[1].Data.ToString());
                            gInfo.NumAttempts = int.Parse(InputPorts[2].Data.ToString());

                            var ClashTest = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                            ClashGrouperUtils.GroupTestClashes(ClashTest, GroupingModes.ClusterAnalysis, gInfo);
                        }


                    }

                }

            }

            OutputPorts[0].Data = input;
        }


        public override Node Clone()
        {
            return new GroupByCluster(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

    public class GroupByGridIntersection : Node
    {
        public GroupByGridIntersection(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddOutputPortToNode("Output", typeof(object));


        }



        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            ProcessClashByGrid(input);
            OutputPorts[0].Data = ClashGrouperUtils.NewListOfClashes;
        }

        private void ProcessClashByGrid(object input)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            var output = new List<object>();
            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {
                    var item = input;
                    OutputPorts[0].Data = input;
                    if (item.GetType() == typeof(SavedItemReference))
                    {
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                        var ClashTest = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                        ClashGrouperUtils.GroupTestClashes(ClashTest as ClashTest, GroupingModes.GridIntersection, gInfo);
                    }
                }
                if (MainTools.IsList(input))
                {
                    OutputPorts[0].Data = input;
                    foreach (var item in input as List<object>)
                    {
                        if (item.GetType() == typeof(SavedItemReference))
                        {

                            var ClashTest = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                            ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();


                            ClashGrouperUtils.GroupTestClashes(ClashTest, GroupingModes.GridIntersection, gInfo);
                        }
                    }
                }
            }
        }

        public override Node Clone()
        {
            return new GroupByGridIntersection(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

    public class GroupByLevel : Node
    {
        public GroupByLevel (VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddOutputPortToNode("Output", typeof(object));

        }



        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            
            ProcessClashByLevel(input);
            OutputPorts[0].Data = ClashGrouperUtils.NewListOfClashes;
        }

        private void ProcessClashByLevel(object input)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {
                    OutputPorts[0].Data = input;
                    var item = input;
                    if (item.GetType() == typeof(SavedItemReference))
                    {

                        var ClashTest = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                        ClashGrouperUtils.GroupTestClashes(ClashTest, GroupingModes.Level, gInfo);
                    }
                }
                if (MainTools.IsList(input))
                {
                    OutputPorts[0].Data = input;
                    foreach (var item in input as List<object>)
                    {
                        if (item.GetType() == typeof(ClashTest))
                        {
                            ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();


                            ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.Level, gInfo);
                        }


                    }

                }

            }
        }



        public override Node Clone()
        {
            return new GroupByLevel(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }


    public class BIM42GroupByLevel : Node
    {
        public BIM42GroupByLevel(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddInputPortToNode("KeepExisting", typeof(object));
            AddOutputPortToNode("Output", typeof(object));

        }



        public override void Calculate()
        {

            var input = InputPorts[0].Data;
            ProcessGrouping(input);

        }

        private void ProcessGrouping(object input)
        {
            bool keepExisting =bool.Parse( InputPorts[1].Data.ToString());
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            GroupingFunctions.NewListOfClashes = new List<SavedItemReference>();
            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(SavedItemReference))
                {
                    OutputPorts[0].Data = input;
                    var item = input;
                    if (item.GetType() == typeof(SavedItemReference))
                    {
                        var ClashFromReference = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                        GroupingFunctions.GroupClashes(ClashFromReference, GroupingMode.Level, GroupingMode.None, keepExisting);
                        //var clashTest = BIM42ClashGroup.GetIndividualClashResults(ClashFromReference, keepExisting);

                        //var Clashes = BIM42ClashGroup.GroupByLevel(clashTest.ToList(), "");
                        //BIM42ClashGroup.ProcessClashGroup(Clashes, ClashFromReference as ClashTest);
                    }
                }
                if (MainTools.IsList(input))
                {
                    OutputPorts[0].Data = input;
                    foreach (var item in input as List<object>)
                    {
                        if (item.GetType() == typeof(SavedItemReference))
                        {
                            var ClashFromReference = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                            GroupingFunctions.GroupClashes(ClashFromReference, GroupingMode.Level, GroupingMode.None, keepExisting);
                            //var clashTest = BIM42ClashGroup.GetIndividualClashResults(ClashFromReference, keepExisting);

                            //var Clashes = BIM42ClashGroup.GroupByLevel(clashTest.ToList(), "");
                            //BIM42ClashGroup.ProcessClashGroup(Clashes, ClashFromReference as ClashTest);
                        }


                    }

                }

            }
        }



        public override Node Clone()
        {
            return new BIM42GroupByLevel(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }


    public class GroupByModel : Node
    {
        public GroupByModel(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddInputPortToNode("Model", typeof(object));
            AddOutputPortToNode("Output", typeof(object));

        }



        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var model = InputPorts[1].Data;
            ProcessClashByModel(input, model);

        }

        private void ProcessClashByModel(object input, object model)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {
                    OutputPorts[0].Data = input;

                    if (input.GetType() == typeof(ClashTest) && model.GetType() == typeof(Model))
                    {
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();
                        gInfo.GroupingModel = model as Model;

                        ClashGrouperUtils.GroupTestClashes(input as ClashTest, GroupingModes.ChosenModelPart, gInfo);
                    }
                }
                if (MainTools.IsList(input))
                {
                    OutputPorts[0].Data = input;
                    foreach (var item in input as List<object>)
                    {
                        if (item.GetType() == typeof(ClashTest) && model.GetType() == typeof(Model))
                        {
                            ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();
                            gInfo.GroupingModel = model as Model;

                            ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.ChosenModelPart, gInfo);

                        }


                    }

                }

            }
        }



        public override Node Clone()
        {
            return new GroupByModel(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }


    //------------------------------------------------------------------
    // NavisWorks Sample code
    //------------------------------------------------------------------

    // (C) Copyright 2013 by Autodesk Inc.

    // Permission to use, copy, modify, and distribute this software in
    // object code form for any purpose and without fee is hereby granted,
    // provided that the above copyright notice appears in all copies and
    // that both that copyright notice and the limited warranty and
    // restricted rights notice below appear in all supporting
    // documentation.

    // AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
    // AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
    // MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK
    // DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
    // UNINTERRUPTED OR ERROR FREE.
    //------------------------------------------------------------------

    class GroupingFunctions
    {
        public static List<SavedItemReference> NewListOfClashes { get; set; }

        public static void GroupClashes(ClashTest selectedClashTest, GroupingMode groupingMode, GroupingMode subgroupingMode, bool keepExistingGroups)
        {
            //Get existing clash result
            List<ClashResult> clashResults = GetIndividualClashResults(selectedClashTest, keepExistingGroups).ToList();
            List<ClashResultGroup> clashResultGroups = new List<ClashResultGroup>();

            //Create groups according to the first grouping mode
            CreateGroup(ref clashResultGroups, groupingMode, clashResults, "");

            //Optionnaly, create subgroups
            if (subgroupingMode != GroupingMode.None)
            {
                CreateSubGroups(ref clashResultGroups, subgroupingMode);
            }

            //Remove groups with only one clash
            List<ClashResult> ungroupedClashResults = RemoveOneClashGroup(ref clashResultGroups);

            //Backup the existing group, if necessary
            if (keepExistingGroups) clashResultGroups.AddRange(BackupExistingClashGroups(selectedClashTest));

            //Process these groups and clashes into the clash test
            ProcessClashGroup(clashResultGroups, ungroupedClashResults, selectedClashTest);
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

        public static void UnGroupClashes(ClashTest selectedClashTest)
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
            ClashResultGroup currentGroup;

            //Create a group for the null GridIntersection
            ClashResultGroup nullGridGroup = new ClashResultGroup();
            nullGridGroup.DisplayName = initialName + "No Level";

            foreach (ClashResult result in results)
            {
                //Cannot add original result to new clash test, so I create a copy
                ClashResult copiedResult = (ClashResult)result.CreateCopy();

                if (gridSystem.ClosestIntersection(copiedResult.Center) != null)
                {
                    GridLevel closestLevel = gridSystem.ClosestIntersection(copiedResult.Center).Level;

                    if (!groups.TryGetValue(closestLevel, out currentGroup))
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
            ClashResultGroup currentGroup;

            //Create a group for the null GridIntersection
            ClashResultGroup nullGridGroup = new ClashResultGroup();
            nullGridGroup.DisplayName = initialName + "No Grid intersection";

            foreach (ClashResult result in results)
            {
                //Cannot add original result to new clash test, so I create a copy
                ClashResult copiedResult = (ClashResult)result.CreateCopy();

                if (gridSystem.ClosestIntersection(copiedResult.Center) != null)
                {
                    GridIntersection closestGridIntersection = gridSystem.ClosestIntersection(copiedResult.Center);

                    if (!groups.TryGetValue(closestGridIntersection, out currentGroup))
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
            ClashResultGroup currentGroup;
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
                    if (!groups.TryGetValue(modelItem, out currentGroup))
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
                    ClashResultGroup oneClashResultGroup = new ClashResultGroup();
                    oneClashResultGroup.DisplayName = "Empty clash";
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
            ClashResultGroup currentGroup;

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

                if (!groups.TryGetValue(clashProperty, out currentGroup))
                {
                    currentGroup = new ClashResultGroup();
                    currentGroup.DisplayName = initialName + clashProperty;
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

    public enum GroupingMode
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
        Status
    }

    //static class BIM42ClashGroup
    //{
    //    public static void ProcessClashGroup(List<ClashResultGroup> clashGroups, ClashTest selectedClashTest)
    //    {
    //        using (Transaction tx = Application.MainDocument.BeginTransaction("Group clashes"))
    //        {
    //            ClashTest copiedClashTest = (ClashTest)selectedClashTest.CreateCopyWithoutChildren();
    //            //When we replace theTest with our new test, theTest will be disposed. If the operation is cancelled, we need a non-disposed copy of theTest with children to sub back in.
    //            ClashTest BackupTest = (ClashTest)selectedClashTest.CreateCopy();
    //            DocumentClash documentClash = Application.MainDocument.GetClash();
    //            int indexOfClashTest = documentClash.TestsData.Tests.IndexOf(selectedClashTest);
    //            documentClash.TestsData.TestsReplaceWithCopy(indexOfClashTest, copiedClashTest);

    //            int CurrentProgress = 0;
    //            //int TotalProgress = ungroupedClashResults.Count + clashGroups.Count;
    //            int TotalProgress =  clashGroups.Count;
    //            Progress ProgressBar = Application.BeginProgress("Copying Results", "Copying results from " + selectedClashTest.DisplayName + " to the Group Clashes pane...");
    //            foreach (ClashResultGroup clashResultGroup in clashGroups)
    //            {
    //                if (ProgressBar.IsCanceled) break;
    //                documentClash.TestsData.TestsAddCopy((GroupItem)documentClash.TestsData.Tests[indexOfClashTest], clashResultGroup);
    //                CurrentProgress++;
    //                ProgressBar.Update((double)CurrentProgress / TotalProgress);
    //            }
    //            //foreach (ClashResult clashResult in ungroupedClashResults)
    //            //{
    //            //    if (ProgressBar.IsCanceled) break;
    //            //    documentClash.TestsData.TestsAddCopy((GroupItem)documentClash.TestsData.Tests[indexOfClashTest], clashResult);
    //            //    CurrentProgress++;
    //            //    ProgressBar.Update((double)CurrentProgress / TotalProgress);
    //            //}
    //            if (ProgressBar.IsCanceled) documentClash.TestsData.TestsReplaceWithCopy(indexOfClashTest, BackupTest);
    //            tx.Commit();
    //            Application.EndProgress();
    //        }
    //    }



    //        public static IEnumerable<ClashResult> GetGroupResults(ClashResultGroup clashResultGroup)
    //    {
    //        for (var i = 0; i < clashResultGroup.Children.Count; i++)
    //        {
    //            yield return (ClashResult)clashResultGroup.Children[i];
    //        }
    //    }

    //    public static IEnumerable<ClashResult> GetIndividualClashResults(ClashTest clashTest, bool keepExistingGroup)
    //    {
    //        for (var i = 0; i < clashTest.Children.Count; i++)
    //        {
    //            if (clashTest.Children[i].IsGroup)
    //            {
    //                if (!keepExistingGroup)
    //                {
    //                    IEnumerable<ClashResult> GroupResults = GetGroupResults((ClashResultGroup)clashTest.Children[i]);
    //                    foreach (ClashResult clashResult in GroupResults)
    //                    {
    //                        yield return clashResult;
    //                    }
    //                }
    //            }
    //            else yield return (ClashResult)clashTest.Children[i];
    //        }
    //    }


    //    public static List<ClashResultGroup> GroupByLevel(List<ClashResult> results, string initialName)
    //    {
    //        //I already checked if it exists
    //        GridSystem gridSystem = Application.MainDocument.Grids.ActiveSystem;
    //        Dictionary<GridLevel, ClashResultGroup> groups = new Dictionary<GridLevel, ClashResultGroup>();
    //        ClashResultGroup currentGroup;

    //        //Create a group for the null GridIntersection
    //        ClashResultGroup nullGridGroup = new ClashResultGroup();
    //        nullGridGroup.DisplayName = initialName + "No Level";

    //        foreach (ClashResult result in results)
    //        {
    //            //Cannot add original result to new clash test, so I create a copy
    //            ClashResult copiedResult = (ClashResult)result.CreateCopy();

    //            if (gridSystem.ClosestIntersection(copiedResult.Center) != null)
    //            {
    //                GridLevel closestLevel = gridSystem.ClosestIntersection(copiedResult.Center).Level;

    //                if (!groups.TryGetValue(closestLevel, out currentGroup))
    //                {
    //                    currentGroup = new ClashResultGroup();
    //                    string displayName = closestLevel.DisplayName;
    //                    if (string.IsNullOrEmpty(displayName)) { displayName = "Unnamed Level"; }
    //                    currentGroup.DisplayName = initialName + displayName;
    //                    groups.Add(closestLevel, currentGroup);
    //                }
    //                currentGroup.Children.Add(copiedResult);
    //            }
    //            else
    //            {
    //                nullGridGroup.Children.Add(copiedResult);
    //            }
    //        }

    //        IOrderedEnumerable<KeyValuePair<GridLevel, ClashResultGroup>> list = groups.OrderBy(key => key.Key.Elevation);
    //        groups = list.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

    //        List<ClashResultGroup> groupsByLevel = groups.Values.ToList();
    //        if (nullGridGroup.Children.Count != 0) groupsByLevel.Add(nullGridGroup);

    //        return groupsByLevel;
    //    }


    //}


    static class ClashGrouperUtils
        {
            private static Dictionary<ModelItem, ClashResultGroup> GroupsByModelItem;
            private static Dictionary<string, ClashResultGroup> GroupsByString;
            private static Dictionary<GridIntersection, ClashResultGroup> GroupsByGridIntersection;
            private static Dictionary<GridLevel, ClashResultGroup> GroupsByLevel;
            private static List<ClashResultGroup> Groups;

            private static List<ClashResult> UngroupedResults;
            private static int UnnamedGroupCount;
            private static Dictionary<GroupingModes, string> CatchAllGroupNames;

        public static List<object> NewListOfClashes { get; set; }

        public static void Init()
            {
                CatchAllGroupNames = new Dictionary<GroupingModes, string>();
                CatchAllGroupNames.Add(GroupingModes.None, "Results");
                CatchAllGroupNames.Add(GroupingModes.ApprovedBy, "Unapproved");
                CatchAllGroupNames.Add(GroupingModes.AssignedTo, "Unassigned");
                CatchAllGroupNames.Add(GroupingModes.ChosenModelPart, "Other Models");
            }

            private class Cluster
            {
                public Point3D Position;
                public List<ClashResult> Members;

                public Cluster(double x, double y, double z)
                {
                    Position = new Point3D(x, y, z);
                }

                public double GetSumDistance()
                {
                    return Members.Sum(Item => Math.Pow(Position.DistanceTo(Item.Center), 2));
                }
            }

            private class ClusteringAttempt
            {
                public List<List<ClashResult>> Groups = new List<List<ClashResult>>();
                public double SumDistance;
            }

            public static bool GroupTestClashesByClusterAnalysis(ClashTest theTest, RelevantGroupingInfo GroupingInfo)
            {
                Random RandomGenerator = new Random();

                //Run the algorithm several times and take the best solution found
                List<ClusteringAttempt> Attempts = new List<ClusteringAttempt>();

                IEnumerable<ClashResult> Results = GetAllResults(theTest);

                //Calculate the bounding box of all the clashes
                BoundingBox3D ClashBounds = new BoundingBox3D();
                foreach (ClashResult theResult in Results)
                {
                    ClashBounds = ClashBounds.Extend(theResult.Center);
                }

                Progress ProgressBar = Autodesk.Navisworks.Api.Application.BeginProgress("Grouping Results", "Grouping Results from " + theTest.DisplayName);
                for (var AttemptNumber = 0; AttemptNumber < GroupingInfo.NumAttempts; AttemptNumber++)
                {
                    if (ProgressBar.IsCanceled)
                    {
                        Autodesk.Navisworks.Api.Application.EndProgress();
                        return false;
                    }
                    ProgressBar.Update((double)AttemptNumber / GroupingInfo.NumAttempts);
                    //To start off, distribute cluster centres randomly within this volume
                    List<Cluster> Clusters = new List<Cluster>();
                    for (var i = 0; i < GroupingInfo.NumClusters; i++)
                    {
                        double X = RandomGenerator.NextDouble() * (ClashBounds.Max.X - ClashBounds.Min.X) + ClashBounds.Min.X;
                        double Y = RandomGenerator.NextDouble() * (ClashBounds.Max.Y - ClashBounds.Min.Y) + ClashBounds.Min.Y;
                        double Z = RandomGenerator.NextDouble() * (ClashBounds.Max.Z - ClashBounds.Min.Z) + ClashBounds.Min.Z;
                        Clusters.Add(new Cluster(X, Y, Z));
                    }

                    bool Changed = true;
                    int Iterations = 0;
                    int MaxIterations = 500;

                    while (Changed && (Iterations < MaxIterations))
                    {
                        Changed = false;
                        foreach (Cluster theCluster in Clusters)
                        {
                            theCluster.Members = new List<ClashResult>();
                        }

                        //Iterate through the results assigning each to the nearest cluster
                        foreach (ClashResult theResult in Results)
                        {
                            Clusters.Aggregate((i1, i2) => i1.Position.DistanceTo(theResult.Center) < i2.Position.DistanceTo(theResult.Center) ? i1 : i2).Members.Add(theResult);
                        }
                        //Remove any clusters with no members
                        for (var i = Clusters.Count - 1; i >= 0; i--)
                        {
                            if (!Clusters[i].Members.Any()) Clusters.RemoveAt(i);
                        }

                        //Iterate through all the clusters moving them to the average position of their results.
                        foreach (Cluster theCluster in Clusters)
                        {
                            Point3D FormerPosition = theCluster.Position;
                            theCluster.Position = new Point3D(
                                theCluster.Members.Average(Item => ((ClashResult)Item).Center.X),
                                theCluster.Members.Average(Item => ((ClashResult)Item).Center.Y),
                                theCluster.Members.Average(Item => ((ClashResult)Item).Center.Z));
                            if (!Point3D.Equals(FormerPosition, theCluster.Position)) Changed = true;
                        }
                        Iterations++;
                    }
                    //Copy the groups found into a ClusteringAttemp, calculate the SumDistance and add to list
                    ClusteringAttempt theAttempt = new ClusteringAttempt();
                    double SumDistance = 0;
                    foreach (Cluster theCluster in Clusters)
                    {
                        SumDistance += theCluster.GetSumDistance();
                        theAttempt.Groups.Add(theCluster.Members);
                    }
                    theAttempt.SumDistance = SumDistance;
                    Attempts.Add(theAttempt);
                }
                Autodesk.Navisworks.Api.Application.EndProgress();

                ClusteringAttempt BestAttempt = Attempts.Aggregate((i1, i2) => i1.SumDistance < i2.SumDistance ? i1 : i2); //Gets the groups from the attempt with the lowest sum distance
                Groups = new List<ClashResultGroup>();
                UngroupedResults = new List<ClashResult>();
                int GroupNumber = 0;
                foreach (List<ClashResult> theList in BestAttempt.Groups)
                {
                    ClashResultGroup theGroup = new ClashResultGroup();
                    theGroup.DisplayName = "Group " + GroupNumber;
                    GroupNumber++;

                    foreach (ClashResult theResult in theList)
                    {
                        ClashResult CopyResult = (ClashResult)theResult.CreateCopy();
                        theGroup.Children.Add(CopyResult);
                    }
                    Groups.Add(theGroup);
                }
                return true;
            }

            private static IEnumerable<ClashResult> GetAllResults(ClashTest theTest)
            {
                for (var i = 0; i < theTest.Children.Count; i++)
                {
                    if (theTest.Children[i].IsGroup)
                    {
                        IEnumerable<ClashResult> GroupResults = GetGroupResults((ClashResultGroup)theTest.Children[i]);
                        foreach (ClashResult theResult in GroupResults)
                        {
                            yield return theResult;
                        }
                    }
                    else yield return (ClashResult)theTest.Children[i];
                }
            }

            private static IEnumerable<ClashResult> GetGroupResults(ClashResultGroup theGroup)
            {
                for (var i = 0; i < theGroup.Children.Count; i++)
                {
                    yield return (ClashResult)theGroup.Children[i];
                }
            }

            public class RelevantGroupingInfo
            {
                public bool CreateCatchAll;
                public Model GroupingModel;
                public int NumClusters;
                public int NumAttempts;
            }

            /// <summary>
            /// Groups all of the clashes in the given test according to which component of the given GroupingModel they involve.
            /// </summary>
            public static void GroupTestClashes(ClashTest theTest, GroupingModes Mode, RelevantGroupingInfo GroupingInfo)
            {
                DocumentClash documentClash = Application.MainDocument.GetClash();
                UngroupedResults = new List<ClashResult>();

                if (Mode == GroupingModes.ClusterAnalysis)
                {
                    if (!GroupTestClashesByClusterAnalysis(theTest, GroupingInfo)) return; //If the process is cancelled
                }
                else
                {
                    GroupsByModelItem = new Dictionary<ModelItem, ClashResultGroup>();
                    GroupsByString = new Dictionary<string, ClashResultGroup>();
                    GroupsByGridIntersection = new Dictionary<GridIntersection, ClashResultGroup>();
                    GroupsByLevel = new Dictionary<GridLevel, ClashResultGroup>();

                    UnnamedGroupCount = 0;

                    int CurrentProgress = 0;
                    int TotalProgress = theTest.Children.Count;
                    foreach (SavedItem theItem in theTest.Children)
                    {
                        if (theItem.IsGroup) TotalProgress += ((ClashResultGroup)theItem).Children.Count - 1;
                    }

                    IEnumerable<ClashResult> Results = GetAllResults(theTest);

                    Progress ProgressBar = Application.BeginProgress("Grouping Results", "Grouping results from " + theTest.DisplayName);

                    foreach (ClashResult theResult in Results)
                    {
                        GroupResult(theResult, Mode, GroupingInfo.GroupingModel);
                        CurrentProgress++;
                        if (ProgressBar.IsCanceled)
                        {
                            Application.EndProgress();
                            return;
                        }
                        ProgressBar.Update((double)CurrentProgress / TotalProgress);
                    }
                    Application.EndProgress();
                }

                //Sort the list of groups according to the size of the group, so that the largest ones appear first
                List<ClashResultGroup> ResultGroups = null;
                switch (Mode)
                {
                    case GroupingModes.None:
                        ResultGroups = new List<ClashResultGroup>();
                        break;

                    case GroupingModes.ApprovedBy:
                    case GroupingModes.AssignedTo:
                        ResultGroups = GroupsByString.Values.ToList();
                        break;

                    case GroupingModes.ChosenModelPart:
                        ResultGroups = GroupsByModelItem.Values.ToList();
                        break;

                    case GroupingModes.GridIntersection:
                        ResultGroups = GroupsByGridIntersection.Values.ToList();
                        break;

                    case GroupingModes.Level:
                        ResultGroups = GroupsByLevel.Values.ToList();
                        break;

                    case GroupingModes.ClusterAnalysis:
                        ResultGroups = Groups;
                        break;
                }
                ResultGroups = ResultGroups.OrderByDescending(Group => Group.Children.Count).ToList();

                if (GroupingInfo.CreateCatchAll)
                {
                    ClashResultGroup CatchAll = new ClashResultGroup();
                    CatchAll.DisplayName = "(" + CatchAllGroupNames[Mode] + ")";

                    foreach (ClashResult theResult in UngroupedResults)
                    {
                        CatchAll.Children.Add(theResult);
                    }
                    ResultGroups.Add(CatchAll);
                    UngroupedResults = new List<ClashResult>(); //Clear out list so that results are not added twice.
                }

                /*This next section will insert a new ClashTest into the list and copy the groups and ungrouped results into it. At the time of writing, there is a bug 
                 * that prevents me from adding the new groups to the ClashTest object before adding it to TestsData.Tests.
                 * Transactions should only be instantiated within a using statement, and you must call Transaction.Commit when finished editing.*/
                using (Transaction theTransaction = Application.MainDocument.BeginTransaction("Clash Grouping"))
                {
                    ClashTest NewTest = (ClashTest)theTest.CreateCopyWithoutChildren();
                    //When we replace theTest with our new test, theTest will be disposed. If the operation is cancelled, we need a non-disposed copy of theTest with children to sub back in.
                    ClashTest BackupTest = (ClashTest)theTest.CreateCopy();
                    int i = documentClash.TestsData.Tests.IndexOf(theTest);
                    documentClash.TestsData.TestsReplaceWithCopy(i, NewTest);

                    int CurrentProgress = 0;
                    int TotalProgress = UngroupedResults.Count + ResultGroups.Count;
                    Progress ProgressBar = Application.BeginProgress("Copying Results", "Copying results from " + theTest.DisplayName + " to the Clash Detective pane...");
                    foreach (ClashResultGroup theGroup in ResultGroups)
                    {
                        if (ProgressBar.IsCanceled) break;
                        documentClash.TestsData.TestsAddCopy((GroupItem)documentClash.TestsData.Tests[i], theGroup);
                        CurrentProgress++;
                        ProgressBar.Update((double)CurrentProgress / TotalProgress);
                    }
                    foreach (ClashResult theResult in UngroupedResults)
                    {
                        if (ProgressBar.IsCanceled) break;
                        documentClash.TestsData.TestsAddCopy((GroupItem)documentClash.TestsData.Tests[i], theResult);
                        CurrentProgress++;
                        ProgressBar.Update((double)CurrentProgress / TotalProgress);
                    }
                    if (ProgressBar.IsCanceled) documentClash.TestsData.TestsReplaceWithCopy(i, BackupTest);
                    theTransaction.Commit();
                NewListOfClashes.Add(documentClash.TestsData.CreateReference(documentClash.TestsData.Tests[i]));
                Application.EndProgress();
                }
            }


 


        /// <summary>
        /// Takes a result and either adds it to the ungrouped results list or adds it to a new or existing group based on the grouping mode.
        /// </summary>
        private static void GroupResult(ClashResult theResult, GroupingModes Mode, Model GroupingModel = null)
            {
                //Cannot add original result to new clash test
                theResult = (ClashResult)theResult.CreateCopy();
                theResult.Guid = Guid.Empty;
                ModelItem RelevantItem;
                ClashResultGroup theGroup;
                GridSystem theGrids = Application.MainDocument.Grids.ActiveSystem; //Already checked that this is not null in dialog
                string RelevantString = null;
                if (Mode == GroupingModes.ApprovedBy) RelevantString = theResult.ApprovedBy;
                else if (Mode == GroupingModes.AssignedTo) RelevantString = theResult.AssignedTo;

                switch (Mode)
                {
                    case GroupingModes.None:
                        UngroupedResults.Add(theResult);
                        return;

                    case GroupingModes.ChosenModelPart:
                        if (GetRootModel(theResult.Item1) == GroupingModel) RelevantItem = theResult.Item1;
                        else if (GetRootModel(theResult.Item2) == GroupingModel) RelevantItem = theResult.Item2;
                        else
                        {
                            UngroupedResults.Add(theResult);
                            return;
                        }

                        RelevantItem = GetSignificantAncestorOrSelf(RelevantItem);

                        if (!GroupsByModelItem.TryGetValue(RelevantItem, out theGroup))
                        {
                            theGroup = new ClashResultGroup();
                            theGroup.DisplayName = GetNamedAncestor(RelevantItem).DisplayName; //In case there was no significant item or the significant item was unnamed
                            if (string.IsNullOrEmpty(theGroup.DisplayName))
                            {
                                theGroup.DisplayName = "Unnamed Part (" + UnnamedGroupCount.ToString() + ")";
                                UnnamedGroupCount++;
                            }
                            GroupsByModelItem.Add(RelevantItem, theGroup);
                        }
                        theGroup.Children.Add(theResult);
                        break;

                    case GroupingModes.ApprovedBy:
                    case GroupingModes.AssignedTo:
                        if (string.IsNullOrEmpty(RelevantString))
                        {
                            UngroupedResults.Add(theResult);
                            return;
                        }
                        else if (!GroupsByString.TryGetValue(RelevantString, out theGroup))
                        {
                            theGroup = new ClashResultGroup();
                            theGroup.DisplayName = RelevantString;
                            GroupsByString.Add(RelevantString, theGroup);
                        }
                        theGroup.Children.Add(theResult);
                        break;

                    case GroupingModes.GridIntersection:
                        GridIntersection theIntersection = theGrids.ClosestIntersection(theResult.Center);
                        if (!GroupsByGridIntersection.TryGetValue(theIntersection, out theGroup))
                        {
                            theGroup = new ClashResultGroup();
                            theGroup.DisplayName = theIntersection.DisplayName;
                            GroupsByGridIntersection.Add(theIntersection, theGroup);
                        }
                        theGroup.Children.Add(theResult);
                        break;

                    case GroupingModes.Level:
                        GridLevel theLevel = theGrids.ClosestIntersection(theResult.Center).Level;
                        if (!GroupsByLevel.TryGetValue(theLevel, out theGroup))
                        {
                            theGroup = new ClashResultGroup();
                            theGroup.DisplayName = theLevel.DisplayName;
                            GroupsByLevel.Add(theLevel, theGroup);
                        }
                        theGroup.Children.Add(theResult);
                        break;
                }

            }

            private static Model GetRootModel(ModelItem Item)
            {
                //Walk up the tree until we find something that's in the MainDocument.Models collection (there can be other models in between)
                while (!(Item.HasModel && Application.MainDocument.Models.Contains(Item.Model)))
                {
                    Item = Item.Parent;
                }
                return Item.Model;
            }

            /// <summary>
            /// Returns the highest level composite item in the ModelItem Item's ancestry. Returns Item itself if there is no composite ancestor.
            /// </summary>
            private static ModelItem GetSignificantAncestorOrSelf(ModelItem Item)
            {
                ModelItem OriginalItem = Item;
                ModelItem CurrentComposite = null;

                //Get last composite item.
                while (Item.Parent != null)
                {
                    Item = Item.Parent;
                    if (Item.IsComposite) CurrentComposite = Item;
                }
                return CurrentComposite ?? OriginalItem; //If there are no composite objects in the tree, go for the clashing ModelItem
            }

            /// <summary>
            /// Returns the first item with a non-empty DisplayName in the ModelItem Item's ancestry including the item itself. Returns null if there is no named item.
            /// </summary>
            private static ModelItem GetNamedAncestor(ModelItem Item)
            {
                while (string.IsNullOrEmpty(Item.DisplayName))
                {
                    if (Item.Parent == null) return null;
                    Item = Item.Parent;
                }
                return Item;
            }
        }

        enum GroupingModes
        {
            None,
            ChosenModelPart,
            ApprovedBy,
            AssignedTo,
            Level,
            GridIntersection,
            ClusterAnalysis,
        }
    

}