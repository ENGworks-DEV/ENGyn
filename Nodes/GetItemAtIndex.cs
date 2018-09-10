using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using System.Windows;

namespace ENGyne.Nodes.General
{
    public class GetItemAtIndex : Node
    {
        public GetItemAtIndex(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("List", typeof(List<object>));
            AddInputPortToNode("Index", typeof(int));
            AddOutputPortToNode("Result", typeof(object));
            
        }

        public override void Calculate()
        {
            if (InputPorts[1].Data != null || InputPorts[0].Data != null)
            {
                //TODO : Catch this one
                var count = Int32.Parse(InputPorts[1].Data.ToString());
                
                object inputs = InputPorts[0].Data ;
               
                object[] res  = ((IEnumerable)inputs).Cast<object>()
                                 
                                 .ToArray();

                if (res != null && res.Length >= count + 1)
                {
                    OutputPorts[0].Data = res.GetValue(count);

                }
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
            return new GetItemAtIndex(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}