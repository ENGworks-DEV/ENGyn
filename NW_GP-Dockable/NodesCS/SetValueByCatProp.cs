using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.Interop.ComApi;
using Autodesk.Navisworks.Api.ComApi;
using System;

namespace NW_GraphicPrograming.Nodes
{
    public class SetValueByCatProp : Node
    {
        public SetValueByCatProp(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ModelItem", typeof(object), true);
            AddInputPortToNode("Category", typeof(object));
            AddInputPortToNode("Property", typeof(object));
            AddInputPortToNode("Value", typeof(object));
            AddOutputPortToNode("ModelItem", typeof(object));


            foreach (Port item in this.InputPorts)
            {
                //item.ToolTip = item.DataType.ToString();
                item.Description = item.Name;

            }

            foreach (Port item in this.OutputPorts)
            {
                //item.ToolTip = item.DataType.ToString();
                item.Description = item.Name;
            }

            AddControlToNode(new Label() { Content = "Set value by" + Environment.NewLine + "category and property", FontSize = 13 });


            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "sets value from category/property" };
            IsResizeable = true;


        }

        public override void Calculate()
        {

            //http://adndevblog.typepad.com/aec/2013/03/add-custom-properties-to-all-desired-model-items.html


            if (InputPorts[0].Data is List<ModelItem>
                && InputPorts[1].Data is string
                && InputPorts[2].Data is string
                && InputPorts[3].Data != null)
            {

                var sel = InputPorts[0].Data;
                List<object> modelItems = new List<object>();


                var category = InputPorts[1].Data.ToString();
                var property = InputPorts[2].Data.ToString();
                var value = InputPorts[3].Data.ToString();




                foreach (var s in sel as List<ModelItem>)
                {
                    SetValues(s, category, property, value);
                    modelItems.Add(s);
                }

                OutputPorts[0].Data = modelItems;
            }
        }



            public  static InwOpState10 state { get; set; }

            public static void SetValues(ModelItem m, string CategoryName, string PropertyName, string value)
            {
            state = ComApiBridge.State;
            InwOaPath oPath = ComApiBridge.ToInwOaPath(m);

            // get properties collection of the path

            InwGUIPropertyNode2 propertyNode = state.GetGUIPropertyNode(oPath, true) as InwGUIPropertyNode2;

            // creating tab (Category), property null variables as placeholders
            InwGUIAttribute2 existingCategory = null;

            //Index of userDefined Tab
            int index = 1;

            //Case 1: Look for an existing category with the same CategoryName

            foreach (Autodesk.Navisworks.Api.Interop.ComApi.InwGUIAttribute2 attribute in propertyNode.GUIAttributes())
            {
                if (attribute.UserDefined)
                {
                    if (attribute.ClassUserName == CategoryName)
                    {
                        existingCategory = attribute;
                        NavisProperties properties = new NavisProperties(PropertyName, value, existingCategory);
                        setProperty(properties, index, propertyNode);
                        return;
                    }

                    index++;
                }

            }


            //Case 2: Category doesn´t exist, create category and property
            if (existingCategory == null)
            {
                NavisProperties properties = new NavisProperties(PropertyName, value, CategoryName);
                setCategoryAndProperty(properties, propertyNode);
                return;
            }

        }


        public static void setCategoryAndProperty(NavisProperties properties, InwGUIPropertyNode2 propertyNode)
        {

            try { propertyNode.SetUserDefined(0, properties.CategoryName, properties.CategoryName, properties.PropertyVec); }
            catch (Exception exception) { MessageBox.Show(exception.Message); }

        }

        public static void setProperty ( NavisProperties properties, int index, InwGUIPropertyNode2 propertyNode)

        {

                propertyNode.SetUserDefined(index, properties.CategoryName, properties.CategoryName, properties.PropertyVec);

        }


        public class NavisProperties
        {

            /// <summary>
            /// Property with value and category name
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <param name="categoryName"></param>
            public NavisProperties(string name, string value, string categoryName)
            {

                newP.name = name;
                newP.value = value;
                CategoryName = categoryName;
                PropertyVec.Properties().Add(newP);
            }

            public NavisProperties(string name, string value, InwGUIAttribute2 existingCategory)
            {

                newP.name = name;
                newP.value = value;
                
                CategoryName = existingCategory.ClassUserName;
                foreach (InwOaProperty item in existingCategory.Properties())
                {
                    if (item.name != name)
                    {
                        //Cant be the same item? do i need to re create it everytime?
                        InwOaProperty existingProp = state.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty) as InwOaProperty;
                        existingProp.name = item.name;
                        existingProp.value = item.value;

                        PropertyVec.Properties().Add(existingProp);
                    }

                }

                PropertyVec.Properties().Add(newP);
            }

            public InwOaProperty newP { get; set; } = state.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty) as InwOaProperty;
            
            public string CategoryName { get; set; }
            public InwOaPropertyVec PropertyVec { get; set; } = state.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec) as InwOaPropertyVec;
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
            return new GetValueByCatProp(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}