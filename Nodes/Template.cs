//using System.Windows.Controls;
//using System.Xml;
//using Autodesk.Navisworks.Api;
//using TUM.CMS.VplControl.Nodes;
//using Autodesk.Navisworks.Api.Clash;
//using TUM.CMS.VplControl.Core;
//using System.Windows.Data;

//using System.Collections.Generic;

//namespace NW_GraphicPrograming.Nodes
//{
//    public class TemplateName : Node
//    {
//        public TemplateName(VplControl hostCanvas)
//            : base(hostCanvas)
//        {
//            AddInputPortToNode("Input", typeof(object));
//            AddOutputPortToNode("Output", typeof(ClashResult));
           


//            AddControlToNode(new Label() { Content = "Title"});

            
//        }


//        public override void Calculate()
//        {


//        }


//        public override void SerializeNetwork(XmlWriter xmlWriter)
//        {
//            base.SerializeNetwork(xmlWriter);

//            // add your xml serialization methods here
//        }

//        public override void DeserializeNetwork(XmlReader xmlReader)
//        {
//            base.DeserializeNetwork(xmlReader);

//            // add your xml deserialization methods here
//        }

//        public override Node Clone()
//        {
//            return new TemplateName(HostCanvas)
//            {
//                Top = Top,
//                Left = Left
//            };

//        }
//    }

//}