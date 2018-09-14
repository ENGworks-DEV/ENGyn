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
using System.Collections;
using System.Linq;

namespace ENGyn.Nodes.List
{
    public class FilterMask : Node
    {
        public FilterMask(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddInputPortToNode("Mask", typeof(object));
            AddOutputPortToNode("In", typeof(object));
            AddOutputPortToNode("Output", typeof(object));



        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var mask = InputPorts[1].Data;
            if (input != null && mask != null)
            {
               if( MainTools.IsList(input) && MainTools.IsList(mask)) 
                {
                    
                   var filtered= FilterByBoolMask(input as IList, mask as IList);
                    OutputPorts[0].Data = ((IEnumerable)filtered["in"]).Cast<object>().ToList();
                    
                    OutputPorts[1].Data = ((IEnumerable)filtered["out"]).Cast<object>().ToList();
                }



            }
            GC.Collect();
        }


  

        public override Node Clone()
        {
            return new FilterMask(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        //https://github.com/DynamoDS/Dynamo/blob/master/src/Libraries/CoreNodes/List.cs
        public static Dictionary<string, object> FilterByBoolMask(IList list, IList mask)
        {
            Tuple<ArrayList, ArrayList> result = FilterByMaskHelper(
                list.Cast<object>(),
                mask.Cast<object>());

            return new Dictionary<string, object>
            {
                { "in", result.Item1 },
                { "out", result.Item2 }
            };
        }
        private static Tuple<ArrayList, ArrayList> FilterByMaskHelper(
           IEnumerable<object> list, IEnumerable<object> mask)
        {
            var inList = new ArrayList();
            var outList = new ArrayList();

            foreach (var p in list.Zip(mask, (item, flag) => new { item, flag }))
            {
                if (p.flag is IList && p.item is IList)
                {
                    Tuple<ArrayList, ArrayList> recur =
                        FilterByMaskHelper(
                            (p.item as IList).Cast<object>(),
                            (p.flag as IList).Cast<object>());
                    inList.Add(recur.Item1);
                    outList.Add(recur.Item2);
                }
                else
                {
                    if ((bool)p.flag)
                        inList.Add(p.item);
                    else
                        outList.Add(p.item);
                }
            }

            return Tuple.Create(inList, outList);
        }
    }

}