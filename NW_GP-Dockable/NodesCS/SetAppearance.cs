using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using media = System.Windows.Media;

using System.Reflection;
using System;

namespace NW_GraphicPrograming.Nodes
{
    public class SetAppearance : Node
    {
        public SetAppearance(VplControl hostCanvas)
            : base(hostCanvas)
        {
            //TODO define Input
            AddInputPortToNode("SearchSet", typeof(List<SelectionSet>));
            AddInputPortToNode("Color", typeof(System.Windows.Media.Color));
            AddOutputPortToNode("SearchSet", typeof(List<SelectionSet>));


            foreach (Port item in this.InputPorts)
            {
                
                item.Description = item.Name;

            }

            foreach (Port item in this.OutputPorts)
            {
                
                item.Description = item.Name;
            }

            AddControlToNode(new Label() { Content = "Get Selection", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            

            this.BottomComment = new TUM.CMS.VplControl.Core.Comment(this) { Text = "Returns current selection" };
            IsResizeable = true;

        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null)
            {
                List<SelectionSet> searchs = (List<SelectionSet>)InputPorts[0].Data;
                media.Color color = (media.Color)InputPorts[1].Data;
                double t =  ((-color.A / 255.0)+1) * 100;
                double transparency = t;
                foreach (SelectionSet s in searchs)
                {
                    applyAppearance(s, transformColor(color), transparency);
                }
                

            }

            


        }


        private Color transformColor(System.Windows.Media.Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            Color NavisColor = new Color(r,g,b);
            
            return NavisColor;
            
        }


        private void applyAppearance(SelectionSet selectionSet, Color color, double transparency )
        {
            if (selectionSet != null && color != null  )

            {
                IEnumerable<ModelItem> modelItems = selectionSet.GetSelectedItems() as IEnumerable<ModelItem>;
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, transparency);
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
            return new SetAppearance(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}