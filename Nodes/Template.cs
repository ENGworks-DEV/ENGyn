//using System.Windows.Controls;
//using System.Xml;
//using Autodesk.Navisworks.Api;
//using TUM.CMS.VplControl.Nodes;
//using Autodesk.Navisworks.Api.Clash;
//using TUM.CMS.VplControl.Core;
//using System.Windows.Data;
//using System.Windows;
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



//            AddControlToNode(new Label() { Content = "Title" , FontSize = 13, FontWeight = FontWeights.Bold});


//        }


//        public override void Calculate()
//        {
            //var input = InputPorts[0].Data;
            //if (input != null)
            //{
            //    var type = input.GetType();
            //    if (type == typeof(SelectionSet))
            //    {

            //    }
            //   if (type.GetType().IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            //    {

            //    }

            //}

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