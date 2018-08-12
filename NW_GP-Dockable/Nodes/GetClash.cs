using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;

namespace NW_GraphicPrograming.Nodes
{
    public class NW_ClashResult : Node
    {
        public NW_ClashResult(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Clash Test", typeof(object));
            AddOutputPortToNode("Navis Clash", typeof(ClashResult));


            //TODO: input as part of the point.Below, temporary solution : One label per input

            foreach (Port item in this.InputPorts)
            {
                item.ToolTip = item.Name;
                // AddControlToNode(new Label() { Content = item.Name, FontSize = 13 });
            }

            AddControlToNode(new Label() { Content = "Clash Result", FontSize = 13 });

            

            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Returns Clash Result from test" };
            IsResizeable = true;
        }


        public override void Calculate()
        {



            List<ClashResult> clashResultList = new List<ClashResult>();
            List<ClashResultGroup> clashResultGroupList = new List<ClashResultGroup>();
            List<object> lct = new List<object>();

            List<string> outputTest = new List<string>();
            foreach (var item in InputPorts[0].Data as List<ClashTest>)
            {
                foreach (var t in item.Children)
                {

                    if (t.IsGroup)
                    {
                        clashResultGroupList.Add(t as ClashResultGroup);
                    }
                    else
                    {
                        clashResultList.Add(t as ClashResult);
                    }
                }
                outputTest.Add("yep");
            }
            
   

        OutputPorts[0].Data = clashResultList;
            

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
            return new NW_ClashResult(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}