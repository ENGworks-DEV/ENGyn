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
                var input = InputPorts[0].Data;
                var properties = InputPorts[1].Data;
                if (MainTools.IsList(input))
                {

                    foreach (var item in (System.Collections.IEnumerable)input)
                    {
                        if (MainTools.IsList(properties))
                        {
                            foreach (var prop in (System.Collections.IEnumerable)properties)
                            {
                                string method = prop as string;
                                var types = item.GetType();
                                PropertyInfo props = types.GetProperty(method);

                                object value = props.GetValue(item);

                                output.Add(value);
                            }
                        }
                        else {
                            try
                            {
                                string method = properties as string;
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

                }
                else
                {
                    if (MainTools.IsList(properties))
                    {
                        foreach (var prop in (System.Collections.IEnumerable)properties)
                        {
                            string method = prop as string;
                            var types = input.GetType();
                            PropertyInfo props = types.GetProperty(method);

                            object value = props.GetValue(input);

                            output.Add(value);
                        }
                    }
                    else
                    {
                        try
                        {
                            string method = properties as string;
                            var types = input.GetType();
                            PropertyInfo prop = types.GetProperty(method);

                            object value = prop.GetValue(input);

                            output.Add(value);
                        }

                        catch
                        {
                            output.Add(null);
                        }
                    }
                }


            }

            OutputPorts[0].Data = output as IList<object>;

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

