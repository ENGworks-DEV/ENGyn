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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

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
            //Help 
            this.BottomComment.Text = "Returns value of class property";
            this.ShowHelpOnMouseOver = true;
        }

        

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            List<object> output = new List<object>();
            OutputPorts[0].Data = MainTools.RunFunction(getAPIPropertyValue, InputPorts);

        }

        private object getAPIPropertyValue(object input, object property)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            object output = null;

            dynamic  iterator = input;
            if (input != null) { 
            if (input.GetType() == typeof(SavedItemReference))
            {
                iterator = doc.ResolveReference(input as SavedItemReference);

            }

            try
            {

         
                string method = property.ToString();
                var types = iterator.GetType();
                
                    foreach (PropertyInfo prop in types.GetProperties())
                    {
                      if (prop.Name == method)
                        {
                         
                            output = prop.GetValue(iterator, null);
                        }
                    }
               
            
            }
            catch
            {
              
            }

            }


            return output;

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

