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

namespace ENGyn.Nodes.API
{
    public class GetAPIPropertyValue : Node
    {
        public GetAPIPropertyValue(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            AddInputPortToNode("Name", typeof(string));
            
            AddOutputPortToNode("Output", typeof(object));

    }

        

        public override void Calculate()
        {
            List<object> output = new List<object>();
            if (InputPorts[0].Data != null)
            {
                var t = InputPorts[0].Data.GetType();
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                {

                    foreach (var item in (System.Collections.IEnumerable)InputPorts[0].Data)
                    {
                        try
                        {
                            string method = InputPorts[1].Data as string;
                            var types = item.GetType();
                            PropertyInfo prop = types.GetProperty(method);

                            object value = prop.GetValue(item);


                            output.Add(value);
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
                        string method = InputPorts[1].Data as string;
                        var types = InputPorts[0].Data.GetType();
                        PropertyInfo prop = types.GetProperty(method);

                        object value = prop.GetValue(InputPorts[0].Data);
                        output.Add(value);
                    }
                    catch
                    {
                        output.Add(null);
                    }
                }


            }

            OutputPorts[0].Data = output;

        }



        public override Node Clone()
        {
            return new GetAPIPropertyValue(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}

