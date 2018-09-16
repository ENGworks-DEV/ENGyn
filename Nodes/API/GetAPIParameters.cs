﻿using System.Windows.Controls;
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

            
        }

        public static string category;
        
        public override void Calculate()
        {
            List<object> output = new List<object>();
            var input = InputPorts[0].Data;
            if (input != null )
            {
                if (MainTools.IsList(input))
                {
                    foreach (var item in (System.Collections.IEnumerable)InputPorts[0].Data)
                    {
                        try
                        {
                            var properties = item.GetType().GetProperties();
                            var prop = new List<string>();
                            foreach (var p in properties)
                            {
                                prop.Add(p.Name);
                            }
                            output.Add(prop);
                        }
                        catch {output.Add(null);}
                    }
                }
                else {
                    try
                    {
                        var properties = input.GetType().GetProperties();
                        foreach (var p in properties)
                        {
                            output.Add(p.Name);
                        }
                    }
                    catch { output.Add(null); }
                }

                OutputPorts[0].Data = output;
            }
                
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