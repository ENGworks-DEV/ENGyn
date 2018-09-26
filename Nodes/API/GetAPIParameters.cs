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
    public class GetAPIParameters : Node
    {
        public GetAPIParameters(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            
            AddOutputPortToNode("Output", typeof(object));

            this.BottomComment.Text = "Texto";
        }

        public static string category;
        
        public override void Calculate()
        {
            
            var input = InputPorts[0].Data;
            OutputPorts[0].Data=  Process(input);

        }

        private IList<object> Process( object input)
        {
            List<object> output = new List<object>();
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {
                if (MainTools.IsList(input))
                {
                    foreach (var item in (System.Collections.IEnumerable)InputPorts[0].Data)
                    {
                        var iterator = item;

                        if (item.GetType() == typeof(SavedItemReference))
                        {
                            iterator = doc.ResolveReference(item as SavedItemReference);
                        }

                        try
                        {
                            var properties = iterator.GetType().GetProperties();
                            var prop = new List<string>();
                            foreach (var p in properties)
                            {
                                prop.Add(p.Name);
                            }
                            output.Add(prop);
                        }
                        catch { output.Add(null); }
                    }
                }
                else
                {
                    var iterator = input;

                    if (input.GetType() == typeof(SavedItemReference))
                    {
                        iterator = doc.ResolveReference(input as SavedItemReference);
                    }

                    try
                    {
                        var properties = iterator.GetType().GetProperties();
                        var prop = new List<string>();
                        foreach (var p in properties)
                        {
                            prop.Add(p.Name);
                        }
                        output.Add(prop);
                    }
                    catch { output.Add(null); }
                }

                
            }
            return output as IList<object>;
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
namespace ENGyn
{

}
