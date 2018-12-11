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
           
            //Help 
            this.BottomComment.Text = "Return list of parameters in element class";
            this.ShowHelpOnMouseOver = true;

            
        }

        public static string category;
        
        public override void Calculate()
        {
            
            OutputPorts[0].Data= MainTools.RunFunction(getAPIParameters, InputPorts) ;

        }

        /// <summary>
        /// Return list of parameters in element class
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private object getAPIParameters( object input)
        {
            object output = null;
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {
                
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
                        output = prop;
                    }
                    catch { output =(null); }
                }

                
            }
            return output;
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

