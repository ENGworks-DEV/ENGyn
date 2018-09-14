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
    public class SetAPIPropertyValues : Node
    {
        public SetAPIPropertyValues(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            AddInputPortToNode("Name", typeof(string));
            AddInputPortToNode("Value", typeof(object));
            AddOutputPortToNode("Output", typeof(object));

    }

        public void manyToOne(object a, object b, string Parameter)
        {
            var ListA = (System.Collections.IList)a;
            var objB = b.ToString();

            foreach (var objA in ListA)
            {
                var types = objA.GetType();
                PropertyInfo prop = types.GetProperty(Parameter);
                prop.SetValue(objA, objB);
            }
        }

        public void oneToOne(object a, object b, string Parameter)
        {
            var ListA = (System.Collections.IList)a;
            var ListB = (System.Collections.IList)b;

            if (ListB.Count == ListA.Count)

                {
                for (int i = 0; i < ListA.Count; i++)
                {
                    var objA = ListA[i];
                    string objB = ListB[i].ToString();

                    var types = objA.GetType();
                    PropertyInfo prop = types.GetProperty(Parameter);
                    prop.SetValue(objA, objB);

                }
            }
            
        }    
        
        public override void Calculate()
        {
            List<object> output = new List<object>();
            if (InputPorts[0].Data != null)
            {
                var input = InputPorts[0].Data;
                var values = InputPorts[2].Data;
                var parameter = InputPorts[1].Data.ToString();
                if (MainTools.IsList(input) && MainTools.IsList(input))
                {
                    var ListA = (System.Collections.IList)input;
                    var ListB = (System.Collections.IList)input;

                    if (ListB.Count == ListA.Count)
                    {
                        try
                        {
                            oneToOne(input, values, parameter);
                        }

                        catch (Exception e)
                        {
                            
                            output.Add(e.Message);
                        }
                    }
                    else
                    {

                        ManyToSome(input, values, parameter);
                    } 

                }
                else
                {
                    try
                    {
                        
                    }
                    catch
                    {
                        output.Add(null);
                    }
                }


            }

            OutputPorts[0].Data = output;

        }

        private void ManyToSome(object a, object b, string Parameter)
        {
            var ListA = (System.Collections.ArrayList)a;
            var ListB = (System.Collections.ArrayList)b;

                for (int i = 0; i < ListA.Count; i++)
                {
                    var objA = ListA[i];
                    string objB = ListB[0].ToString();

                    var types = objA.GetType();
                    PropertyInfo prop = types.GetProperty(Parameter);
                    prop.SetValue(objA, objB);

          
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
            return new SetAPIPropertyValues(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}

