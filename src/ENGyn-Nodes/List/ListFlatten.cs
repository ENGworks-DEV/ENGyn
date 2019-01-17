using System.Collections.Generic;
using TUM.CMS.VplControl.Core;
using System.Linq;

namespace ENGyn.Nodes.List
{
    public class ListFlatten : Node
    {
        public ListFlatten(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddInputPortToNode("Level", typeof(object));
            AddOutputPortToNode("List", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Flatten a list by specific level";
        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data; ;
            var level = InputPorts[1].Data;

            Process(input, level);

        }
        /// <summary>
        /// Flatten list by level
        /// </summary>
        /// <param name="input"></param>
        /// <param name="level"></param>
        private void Process( object input, object level)
        { 
         

            if (input != null )
            {
                if (level != null)
                {
                    OutputPorts[0].Data = Flatten(input, int.Parse(level.ToString()), new List<object>());
                }
                else
                { OutputPorts[0].Data = Flatten(input, 0, new List<object>()); }
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