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
            AddInputPortToNode("ModelItem", typeof(object),true);
            AddInputPortToNode("Category", typeof(object));
            AddInputPortToNode("Property", typeof(object));
            AddInputPortToNode("Value", typeof(object));
            AddOutputPortToNode("ModelItem", typeof(object));


            //TODO: input as part of the point.Below, temporary solution : One label per input

            foreach (var item in this.InputPorts)
            {
                AddControlToNode(new Label() { Content = item.Name, FontSize = 13 });
            }


            this.Name = "Set Value";

            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "sets value from category/property" };
            IsResizeable = true;


        }

        public override void Calculate()
        {

            //http://adndevblog.typepad.com/aec/2013/03/add-custom-properties-to-all-desired-model-items.html



            var sel = InputPorts[0].Data;
            List<object> modelItems = new List<object>();


            var category = InputPorts[1].Data.ToString();
            var property = InputPorts[2].Data.ToString();
            var value = InputPorts[3].Data.ToString();


            

            foreach (var s in sel as List<ModelItem>)
            {
                setValues(s, category, property, value);
                modelItems.Add(s);
            }

            OutputPorts[0].Data = modelItems;
        }

        public static void setValues(ModelItem m, string category, string property, string value)
        {
            // Create collection with model item

            ModelItemCollection modelItemCollection = new ModelItemCollection();
            modelItemCollection.Add(m);

            InwOpState10 state;
            state = ComApiBridge.State;

            // get the selection in COM

            Autodesk.Navisworks.Api.Interop.ComApi.InwOpSelection comSelectionOut = ComApiBridge.ToInwOpSelection(modelItemCollection);

            /// get paths within the selection and select the last one (for some reason)

            InwSelectionPathsColl oPaths = comSelectionOut.Paths();
            InwOaPath3 oPath = (InwOaPath3)oPaths.Last();

            // get properties collection of the path

            InwGUIPropertyNode2 propertyNode = (InwGUIPropertyNode2)state.GetGUIPropertyNode(oPath, true);

            // creating tab (Category), property null variables as placeholders
            InwGUIAttribute2 existingCategory = null;
            //Create new propertyVec (whatever that is)
            InwOaPropertyVec newPvec = (InwOaPropertyVec)state.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null); ;

            //Index of userDefined Tab
            int index = 0;


           // GC.KeepAlive(propertyNode); //Call here to keep propertyNode ‘alive’ through the inner loop 

            for (int i = 1; i < propertyNode.GUIAttributes().Count; i++)
            {
                var GUIAttribute = propertyNode.GUIAttributes()[i] as InwGUIAttribute2;
                string name = GUIAttribute.ClassUserName;
                if (GUIAttribute.ClassUserName == category)
                {
                    existingCategory = GUIAttribute;
                    break;
                }
                index += 1;
            }

            if (existingCategory == null)
            {
                index = 0;
                // create new property
                InwOaProperty newP = (InwOaProperty)state.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                // set the name, username and value of the new property

                newP.name = property + "_Name";

                newP.UserName = property;

                newP.value = value;

                // add the new property to the new property category

                newPvec.Properties().Add(newP);

            }

            else
            {

                // check if property exists
                bool propertyExists = false;
                

                foreach (InwOaProperty prop in existingCategory.Properties())
                {


                    if (prop.UserName == property)
                    {
                        propertyExists = true;
                        
                        //Set existing property to new value
                        //prop.value(value);

                        // create new property
                        InwOaProperty newP = (InwOaProperty)state.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                        newP.name = prop.name;
                        newP.UserName = prop.UserName;
                        newP.value = value;

                        //Add the property to the propertyVec
                        newPvec.Properties().Add(newP);
                    }
                }


                if (!propertyExists)
                {

                    //Create new propertyVec (whatever that is)
                    //newPvec = existingCategory as InwOaPropertyVec;
                    
                    // create new property
                    InwOaProperty newP = (InwOaProperty)state.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                    // set the name, username and value of the new property
                    newP.name = property + "_Name";

                    newP.UserName = property;

                    newP.value = value;

                    // add the new property to the new property category
                    newPvec.Properties().Add(newP);
                }


            }

            // add the new property category to the path

            propertyNode.SetUserDefined(index, category, category + "_InteralName", newPvec);

        }


        public static void RecreateCategory (InwOpState10 state, InwOaProperty oldP , string property, string value)
        {
            InwGUIPropertyNode2 propertyNode;



            //category exists? N
            //property exists?
            //

            //if (category.Name == CategoryName)
            //{
            // 
            //if (property.Name == PropertyName)
            //
            //}

            //Create new propertyVec (whatever that is)
            InwOaPropertyVec newPvec = (InwOaPropertyVec)state.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
           
            // create new property
            InwOaProperty newP = (InwOaProperty)state.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

            // set the name, username and value of the new property

            newP.name = property + "_Name";

            newP.UserName = property;

            newP.value = value;

            // add the new property to the new property category

            newPvec.Properties().Add(newP);

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