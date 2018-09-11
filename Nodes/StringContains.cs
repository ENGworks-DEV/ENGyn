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
    public class StringContains : Node
    {
        public StringContains(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("List", typeof(object));
            AddInputPortToNode("String", typeof(object));
            AddOutputPortToNode("Output", typeof(object));


        }


        public override void Calculate()
        {
            var InputList = InputPorts[0].Data;
            var InputString = InputPorts[1].Data;
            if (InputList !=null && InputString != null)
            {
                OutputPorts[0].Data = Contains(InputList, InputString);
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
        public List<object> Contains (object a, object b)
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

}