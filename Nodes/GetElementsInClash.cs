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

namespace ENGyn.Nodes.Clash
{
    public class GetElementsInClash : Node
    {
        public GetElementsInClash(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddOutputPortToNode("SelectionA", typeof(object));
            AddOutputPortToNode("SelectionB", typeof(object));


         


        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            if (input != null)
            {

                List<ModelItem> itemA = new List<ModelItem>();
                List<ModelItem> itemB = new List<ModelItem>();
                if (MainTools.IsList(input))
                {
                    foreach (var c in (System.Collections.IEnumerable)InputPorts[0].Data)
                    {
                        if (c.GetType() == typeof(ClashResult) )
                        {
                            var clash = c as ClashResult;
                            itemA.AddRange(clash.CompositeItemSelection1);
                            itemB.AddRange(clash.CompositeItemSelection2);

                        }
                        if (c.GetType() == typeof(ClashResultGroup))
                        {
                            var group = c as ClashResultGroup;
                            
                            itemA.AddRange(group.CompositeItemSelection1);
                            itemB.AddRange(group.CompositeItemSelection2);

                        }
                    }
                  }
                OutputPorts[0].Data = itemA;
                OutputPorts[1].Data = itemB;
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
            return new GetElementsInClash(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}