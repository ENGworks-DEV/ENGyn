using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace ENGyn.Nodes.List
{
    public class ListTranspose : Node
    {
        public ListTranspose(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddOutputPortToNode("List", typeof(object));

        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            if (input != null)
            {
                if (MainTools.IsList(input))
                    {
                   OutputPorts[0].Data=  Transpose((System.Collections.IList)input);
                }
                

            }

        }
        public static IList Transpose(IList lists)
        {
            if (lists.Count == 0 || !lists.Cast<object>().Any(x => x is IList))
                return lists;

            IEnumerable<IList> ilists = lists.Cast<IList>();
            int maxLength = ilists.Max(subList => subList.Count);
            List<ArrayList> transposedList =
                Enumerable.Range(0, maxLength).Select(i => new ArrayList()).ToList();

            foreach (IList sublist in ilists)
            {
                for (int i = 0; i < transposedList.Count; i++)
                {
                    transposedList[i].Add(i < sublist.Count ? sublist[i] : null);
                }
            }

            return transposedList;
        }

        public override Node Clone()
        {
            return new ListTranspose(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}