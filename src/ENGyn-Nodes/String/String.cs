using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Drawing;
using System;


namespace ENGyn.Nodes.String
{

    public class StringToLower : Node
    {

        public StringToLower(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("String", typeof(object));
            AddOutputPortToNode("String", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Return string all in lower";
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
            if (MainTools.IsList(a))
            {


                List<object> output = new List<object>();
                List<object> input = a as List<object>;

                foreach (object x in input)
                {
                    output.Add(x.ToString().ToLower());
                }

                return output;
            }
            else
            {

                return a.ToString().ToLower();
            }
        }
    }

    public class StringToUpper : Node
    {

        public StringToUpper(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("String", typeof(object));
            AddOutputPortToNode("String", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Return string in UPPER";
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
            if (MainTools.IsList(a))
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

    public class StringContains : Node
    {
        public StringContains(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("String", typeof(object));
            AddInputPortToNode("String", typeof(object));
            AddOutputPortToNode("Output", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Returns true if string contains specific substring";
        }


        public override void Calculate()
        {
            var InputList = InputPorts[0].Data;
            var InputString = InputPorts[1].Data;
            if (InputList != null && InputString != null)
            {
                OutputPorts[0].Data = Contains(InputList, InputString);
            }

        }




        public override Node Clone()
        {
            return new StringContains(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }


        /// <summary>
        /// Check each element of a list of string
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public List<object> Contains(object a, object b)
        {
            List<object> output = new List<object>();

            if (MainTools.IsList(a) & MainTools.IsString(b))
            {
                foreach (var item in (System.Collections.IEnumerable)(a))
                {
                    if (MainTools.IsString(item))
                    {
                        string container = item as string;
                        output.Add(container.Contains(b as string));
                    }
                }
            }
            return output;
        }

    }

    public class StringReplace : Node
    {
        public StringReplace(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddInputPortToNode("oldValue", typeof(object));
            AddInputPortToNode("newValue", typeof(object));
            AddOutputPortToNode("Output", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Replace substring in string";
        }


        public override void Calculate()
        {
            var InputList = InputPorts[0].Data;
            var oldValue = InputPorts[1].Data;
            var newValue = InputPorts[2].Data;
            if (InputList != null && oldValue != null && newValue != null)
            {
                OutputPorts[0].Data = Replace(InputList, oldValue, newValue);
            }

        }



        public override Node Clone()
        {
            return new StringReplace(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }


        /// <summary>
        /// Check each element of a list of string
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public List<object> Replace(object a, object b, object c)
        {
            List<object> output = new List<object>();

            if (MainTools.IsList(a) && MainTools.IsString(b) && MainTools.IsString(c))
            {
                foreach (var item in (System.Collections.IEnumerable)(a))
                {
                    if (MainTools.IsString(item))
                    {
                        string container = item as string;
                        output.Add(container.Replace(b.ToString(), c.ToString()));
                    }
                }
            }
            else
            {
                if (a.GetType() == typeof(string) && MainTools.IsString(b) && MainTools.IsString(c))

                {
                    string container = a as string;
                    output.Add(container.Replace(b.ToString(), c.ToString()));
                }
            }
            return output;
        }

    }

}