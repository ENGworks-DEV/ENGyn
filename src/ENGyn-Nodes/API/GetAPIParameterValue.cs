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
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            List<object> output = new List<object>();
            OutputPorts[0].Data =  Process(doc, output);

        }

        private IList<object> Process(Document doc, List<object> output)
        {
            if (InputPorts[0].Data != null)
            {
                var input = InputPorts[0].Data;
                var properties = InputPorts[1].Data;
                if (MainTools.IsList(input))
                {

                    foreach (var item in (System.Collections.IEnumerable)input)
                    {
                        var iterator = item;
                        if (item != null)
                        {
                            if (item.GetType() == typeof(SavedItemReference))
                            {
                                iterator = doc.ResolveReference(item as SavedItemReference);
                            }

                            if (MainTools.IsList(properties))
                            {
                                foreach (var prop in (System.Collections.IEnumerable)properties)
                                {
                                    try
                                    {
                                        dynamic d = prop;
                                        string method = prop as string;
                                        var types = iterator.GetType();
                                        PropertyInfo props = types.GetProperty(method);

                                        object value = props.GetValue(iterator);

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
                                    dynamic d = properties;
                                    string method = properties as string;
                                    method = d;
                                    var types = iterator.GetType();
                                    PropertyInfo prop = types.GetProperty(method);

                                    object value = prop.GetValue(iterator);

                                    output.Add(value);
                                }

                                catch
                                {
                                    output.Add(null);
                                }
                            }

                        }

                        else {output.Add(item); }

                    }

                }
                else
                {
                    var iterator = input;

                    if (input.GetType() == typeof(SavedItemReference))
                    {
                        iterator = doc.ResolveReference(input as SavedItemReference);
                    }

                    if (MainTools.IsList(properties))
                    {


                        foreach (var prop in (System.Collections.IEnumerable)properties)
                        {
                            string method = prop as string;
                            var types = iterator.GetType();
                            PropertyInfo props = types.GetProperty(method);

                            object value = props.GetValue(iterator);

                            output.Add(value);
                        }
                    }
                    else
                    {
                        try
                        {
                            string method = properties as string;
                            var types = iterator.GetType();
                            PropertyInfo prop = types.GetProperty(method);

                            object value = prop.GetValue(iterator);

                            output.Add(value);
                        }

                        catch
                        {
                            output.Add(null);
                        }
                    }
                }


            }

           return output as IList<object>;
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

