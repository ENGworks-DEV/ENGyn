using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using ENGyn.Nodes;

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


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Return value of ModelItem property by category (tab name) and property ";
        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null
                && InputPorts[1].Data != null
                && InputPorts[2].Data != null)
            {
             
              OutputPorts[0].Data=   MainTools.RunFunction(GetValuesFromProperties, InputPorts);

            }

        }


        public static object GetValuesFromProperties(object sel, object category, object property)
        {
            dynamic value = null;
            
            if (sel.GetType() == typeof(ModelItem))
                    {
                ModelItem modelItem = findModelItem(sel);
               
                    if (modelItem != null)
                { 
                    var prop = modelItem.PropertyCategories.FindPropertyByDisplayName(category.ToString(), property.ToString());
                    

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
                }
                if (value != null)
                    {
                        value = value.ToString();
                    }
  
            }
            return value;
        }

        private static ModelItem findModelItem(object sel)
        {
            var guid = ((ModelItem)sel).InstanceGuid;
            Search search = new Search();
            search.Selection.SelectAll();

            search.SearchConditions.Add(

            

            SearchCondition.HasPropertyByName("LcOaNode", "LcOaNodeGuid").EqualValue(VariantData.FromDisplayString(guid.ToString())));

            // Execute Search

            ModelItemCollection items = search.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
          
            return items.FirstOrDefault() ;

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