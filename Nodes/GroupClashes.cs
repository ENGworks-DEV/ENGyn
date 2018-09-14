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
            if (input != null)
            {
                
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {

                    var item = input;
                    if (item.GetType() == typeof(ClashTest))
                    {
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                        gInfo.NumClusters = int.Parse(InputPorts[1].Data.ToString());
                        gInfo.NumAttempts = int.Parse(InputPorts[2].Data.ToString());

                        ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.ClusterAnalysis, gInfo);
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

                            gInfo.NumClusters = int.Parse(InputPorts[1].Data.ToString());
                            gInfo.NumAttempts = int.Parse(InputPorts[2].Data.ToString());

                            ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.ClusterAnalysis, gInfo);
                        }

                        
                    }

                }

            }

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
            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {
                    var item = input;
                    OutputPorts[0].Data = input;
                    if (item.GetType() == typeof(ClashTest))
                    {
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                        ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.GridIntersection, gInfo);
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


                            ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.GridIntersection, gInfo);
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
            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {
                    OutputPorts[0].Data = input;
                    var item = input;
                    if (item.GetType() == typeof(ClashTest))
                    {
                        ClashGrouperUtils.RelevantGroupingInfo gInfo = new ClashGrouperUtils.RelevantGroupingInfo();

                        ClashGrouperUtils.GroupTestClashes(item as ClashTest, GroupingModes.Level, gInfo);
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
            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(ClashTest))
                {
                    OutputPorts[0].Data = input;
                    
                    if (input.GetType() == typeof(ClashTest) && model.GetType() == typeof(Model) )
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