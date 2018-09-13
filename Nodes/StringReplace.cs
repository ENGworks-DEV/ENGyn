using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;

namespace ENGyn.Nodes.String
{
    public class StringReplace : Node
    {
        public StringReplace(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddInputPortToNode("oldValue", typeof(object));
            AddInputPortToNode("newValue", typeof(object));
            AddOutputPortToNode("Output", typeof(object));


        }


        public override void Calculate()
        {
            var InputList = InputPorts[0].Data;
            var oldValue = InputPorts[1].Data;
            var newValue = InputPorts[1].Data;
            if (InputList !=null && oldValue != null && newValue != null)
            {
                OutputPorts[0].Data = Replace(InputList, oldValue, newValue);
            }

        }


        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            // add your xml serialization methods here
        }

        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            // add your xml deserialization methods here
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
        public List<object> Replace (object a, object b, object c)
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
            return output;
        }

    }

}