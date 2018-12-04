using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.List
{
    public class GetItemAtIndex : Node
    {
        public GetItemAtIndex(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("List", typeof(List<object>));
            AddInputPortToNode("Index", typeof(int));
            AddOutputPortToNode("Result", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Get item of list at an specific index";

        }

        public override void Calculate()
        {
            if (InputPorts[1].Data != null || InputPorts[0].Data != null)
            {
                //TODO : Catch this one
                var count = Int32.Parse(InputPorts[1].Data.ToString());

                object inputs = InputPorts[0].Data;

                object[] res = ((IEnumerable)inputs).Cast<object>()

                                 .ToArray();

                if (res != null && res.Length >= count + 1)
                {
                    OutputPorts[0].Data = res.GetValue(count);

                }
            }

        }



        public override Node Clone()
        {
            return new GetItemAtIndex(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}