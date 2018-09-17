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

namespace ENGyn.Nodes.List
{
    public class ListFlatten : Node
    {
        public ListFlatten(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddInputPortToNode("int", typeof(object));
            AddOutputPortToNode("List", typeof(object));
        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data as List<object>;
            int input1 = Int32.Parse(InputPorts[1].Data.ToString());

            if (input != null && input1 != null)
            {

                OutputPorts[0].Data = Flatten(input, input1, new List<object>());
            }
            
        }



        public override Node Clone()
        {
            return new ListFlatten(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        }



        private static List<object> Flatten(object list, int amt, List<object> acc)
        {
            if (amt == 0)
            {
                if (MainTools.IsList(list))
                { 
                    foreach (object item in (System.Collections.IEnumerable)list)
                    { 
                    acc.Add(item);
                    }
                }
                else
                {
                    acc.Add(list);
                }
            }
            else
            {
                foreach (object item in (System.Collections.IEnumerable)list)
                {
                    if (MainTools.IsList(item))
                    { 
                        var type = item.GetType();
                        acc = Flatten(item, amt - 1, acc);
                    }
                    else
                    { 
                        acc.Add(item);
                    }
                }
            }
            return acc;
        }

    }

}