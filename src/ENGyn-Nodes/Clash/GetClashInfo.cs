using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using Autodesk.Navisworks.Api.Clash;
using Application = Autodesk.Navisworks.Api.Application;
using System;
using System.Collections.Generic;
using System.Collections;

namespace ENGyn.Nodes.Clash
{
    public class GetClashInfo : Node
    {
        public GetClashInfo(VplControl hostCanvas)
            : base(hostCanvas)
        {
 
            AddInputPortToNode("Clash", typeof(object));
            AddOutputPortToNode("Name", typeof(object));
            AddOutputPortToNode("Status", typeof(object));
            AddOutputPortToNode("Approved By", typeof(object));
            AddOutputPortToNode("Approved Time", typeof(object));
            AddOutputPortToNode("Assigned To", typeof(object));
            AddOutputPortToNode("Description", typeof(object));
            AddOutputPortToNode("Grids", typeof(object));
            AddOutputPortToNode("Level", typeof(object));
            AddOutputPortToNode("X", typeof(object));
            AddOutputPortToNode("Y", typeof(object));
            AddOutputPortToNode("Z", typeof(object));



        }

        public override void Calculate()
        {
            //Just a place holder. Gives the hability to connect the node to a lower stage in the execution tree
            var input = InputPorts[0].Data;
            var output = getClashInfo(input);
            var outputTranspose = Transpose(output);

            for (int i = 0; i < outputTranspose.Count; i++)
            {
                OutputPorts[i].Data = outputTranspose[i] as List<object>;
            }

        }


    public List<object> getClashInfo(object input)
    {
        var output = new List<object>();


        if (MainTools.IsList(input))
        {
            foreach (var item in (IList<object>)input)
            {
                   var clash = item as IClashResult;
                if (clash!=null)

                {
                        var name = clash.DisplayName;
                        var status = clash.Status;
                        var ApprovedBy = clash.ApprovedBy;
                        var ApprovedTime = clash.ApprovedTime;
                        var AssignedTo = clash.AssignedTo;
                        var Description = clash.Description;
                        
                        string grid = null;
                        string level = null;
                        var x = clash.Center.X;
                        var y = clash.Center.Y;
                        var z = clash.Center.Z;

                        GridSystem gridSystem = Application.MainDocument.Grids.ActiveSystem;

                    if (gridSystem.ClosestIntersection(clash.Center) != null)
                    {
                        GridIntersection closestGridIntersection = gridSystem.ClosestIntersection(clash.Center);
                        grid = closestGridIntersection.Line1.DisplayName + "-" + closestGridIntersection.Line2.DisplayName;
                        level = closestGridIntersection.Level.DisplayName;
             

                    }
                        output.Add(new List<object>() {
                            name,
                            status,
                            ApprovedBy,
                            ApprovedTime,
                            AssignedTo,
                            Description,
                            grid,
                            level,
                            x,
                            y,
                            z
                        });
                }
            }
        }

        return (output);
    }



    public object getLocationFromGrids(object input)
        {
            var output =new List<List<string>>();
            var grids = new List<object>();
            var level = new List<object>();
            var X = new List<object>();
            var Y = new List<object>();
            var Z = new List<object>();

            if (MainTools.IsList(input))
            {
                foreach (var item in (IList<object>)input)
                {
                    if (item.GetType() == typeof(IClashResult))

                    {
                        var clash = item as IClashResult;

                        GridSystem gridSystem = Application.MainDocument.Grids.ActiveSystem;

                        if (gridSystem.ClosestIntersection(clash.Center) != null)
                        {
                            GridIntersection closestGridIntersection = gridSystem.ClosestIntersection(clash.Center);
                            grids.Add(closestGridIntersection.DisplayName);
                            level.Add(closestGridIntersection.Level.DisplayName);
                            X.Add(clash.Center.X);
                            Y.Add(clash.Center.Y);
                            Z.Add(clash.Center.Z);

                        }
                    }
                }
            }
            
            return new List<List<object>>(){ grids,level,X,Y,Z};
    }



        public override Node Clone()
        {
            return new GetClashInfo(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }

        public static IList Transpose(IList lists)
        {
            if (lists.Count == 0 || !lists.Cast<dynamic>().Any(x => x is IList))
                return lists;

            IEnumerable<IList> ilists = lists.Cast<IList>();
            int maxLength = ilists.Max(subList => subList.Count);
            List<List<object>> transposedList =
                Enumerable.Range(0, maxLength).Select(i => new List<object>()).ToList();

            foreach (IList sublist in ilists)
            {
                for (int i = 0; i < transposedList.Count; i++)
                {
                    transposedList[i].Add(i < sublist.Count ? sublist[i] : null);
                }
            }

            return transposedList.ToList<object>();
        }
    }

}