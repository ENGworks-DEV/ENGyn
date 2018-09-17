using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Drawing;
using System;


namespace ENGyn.Nodes.String
{

    public class StringToUpper : Node
    {
      
        public StringToUpper(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("String", typeof(object));
            AddOutputPortToNode("Stringt", typeof(object));
        }


        public override void Calculate()
        {   
            var InpuValue = InputPorts[0].Data;
            if (InpuValue != null)
            {
                OutputPorts[0].Data = StringResult(InpuValue);
            }
        }



        public override Node Clone()
        {
            return new StringToUpper(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }

        public object StringResult(object a)
        {
           if(MainTools.IsList(a))
            {
             

                List<object> output = new List<object>();
                List<object> input = a as List<object>;
                
                foreach (object x in input)
                {
                    output.Add(x.ToString().ToUpper());
                }

                return output;
            }
           else
            {
                
                return a.ToString().ToUpper();
            }
        }
    }
}