using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;
using System.Reflection;
using System.Windows;

namespace NW_GraphicPrograming.Nodes.API
{
    public class GetAPIParameters : Node
    {
        public GetAPIParameters(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            
            AddOutputPortToNode("Output", typeof(object));

            AddControlToNode(new Label() { Content = "Get API Parameters", FontSize = 13, FontWeight = FontWeights.Bold });
            this.NodeCaption = "API";
            Name = "Get API Parameters";
        }

        public static string category;
        
        public override void Calculate()
        {
            List<object> output = new List<object>();
            if (InputPorts[0].Data != null && InputPorts[1].Data != null)
            {
                if (InputPorts[0].Data.GetType() == typeof(List<object>))
                {
                    foreach (var item in InputPorts[0].Data as List<object>)
                    {
                        try
                        {

                            var properties = item.GetType().GetProperties();
                            foreach (var p in properties)
                            {
                                output.Add(p.Name);
                            }

                        }

                        catch
                        {
                            output.Add(null);
                        }

                    }

                }
                else
                {
                    try
                    {
                        var item = InputPorts[0].Data;
                        var properties = item.GetType().GetProperties();
                        foreach (var p in properties)
                        {
                            output.Add(p.Name);
                        }
                    }
                    catch
                    {
                        output.Add(null);
                    }


                }

                OutputPorts[0].Data = output;
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
            return new GetAPIParameters(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}
namespace NW_GraphicPrograming
{

}
