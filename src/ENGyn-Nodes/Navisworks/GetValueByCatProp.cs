using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System;

namespace ENGyn.Nodes.Navisworks
{
    public class GetValueByCatProp : Node
    {
        public GetValueByCatProp(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ModelItem", typeof(object));
            AddInputPortToNode("Category", typeof(string));
            AddInputPortToNode("Property", typeof(string));
            AddOutputPortToNode("Value", typeof(string));

        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null
                && InputPorts[1].Data != null
                && InputPorts[2].Data != null
                && MainTools.IsList(InputPorts[0].Data))
            {

                var sel = InputPorts[0].Data;
                
                var category = InputPorts[1].Data.ToString();
                var property = InputPorts[2].Data.ToString();
                

                OutputPorts[0].Data = GetValuesFromProperties(sel, category, property); ;
            }
        }

        public static List<object> GetValuesFromProperties(object sel, string category, string property)
        {
            List<object> modelItems = new List<object>();
            foreach (var s in sel as List<ModelItem>)
            {

                var prop = s.PropertyCategories.FindPropertyByDisplayName(category, property);
                dynamic value = null;
                if (prop != null)
                {
                    switch (prop.Value.DataType)
                    {
                        case VariantDataType.None:
                            break;
                        case VariantDataType.Double:
                            value = prop.Value.ToDouble();
                            break;
                        case VariantDataType.Int32:
                            value = prop.Value.ToInt32();
                            break;
                        case VariantDataType.Boolean:
                            value = prop.Value.ToBoolean();
                            break;
                        case VariantDataType.DisplayString:
                            value = prop.Value.ToDisplayString();
                            break;
                        case VariantDataType.DateTime:
                            value = prop.Value.ToDateTime();
                            break;
                        case VariantDataType.DoubleLength:
                            value = prop.Value.ToDoubleLength();
                            break;
                        case VariantDataType.DoubleAngle:
                            value = prop.Value.ToDoubleAngle();
                            break;
                        case VariantDataType.NamedConstant:
                            value = prop.Value.ToNamedConstant();
                            break;
                        case VariantDataType.IdentifierString:
                            value = prop.Value.ToIdentifierString();
                            break;
                        case VariantDataType.DoubleArea:
                            value = prop.Value.ToDoubleArea();
                            break;
                        case VariantDataType.DoubleVolume:
                            value = prop.Value.ToDoubleVolume();
                            break;
                        case VariantDataType.Point3D:
                            value = prop.Value.ToPoint3D();
                            break;
                        case VariantDataType.Point2D:
                            value = prop.Value.ToPoint2D();
                            break;
                        default:
                            break;
                    }
                }

                if (value != null)
                {
                    value = value.ToString();
                }

                modelItems.Add(value);
            }
            return modelItems;
        }


        public override Node Clone()
        {
            return new GetValueByCatProp(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}