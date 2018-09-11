using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System.Windows;

namespace ENGyn.Nodes.Clash
{
    public class ClashResults : Node
    {
        public ClashResults(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Clash Test", typeof(object));
            AddOutputPortToNode("Navis Clash", typeof(object));


        }


        public override void Calculate()
        {



            List<object> clashResultList = new List<object>();

            var input = InputPorts[0].Data;


            //Check for null in input
            if (MainTools.IsList(input))
            {
                foreach (var i in (System.Collections.IEnumerable)input)
                {
                    if (i.GetType() == typeof(ClashTest))
                    {
                        ClashTest item = i as ClashTest;
                        foreach (var t in item.Children)
                        {
                            clashResultList.Add(t);

                        }
                    }
                }
                OutputPorts[0].Data = clashResultList;
                
            }

            if (input.GetType() == typeof(ClashTest))
            {
                ClashTest item = input as ClashTest;
                foreach (var t in item.Children)
                {
                        clashResultList.Add(t);
                }
                OutputPorts[0].Data = clashResultList;
                

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
            return new ClashResults(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}