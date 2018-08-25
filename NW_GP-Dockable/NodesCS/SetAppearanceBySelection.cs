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
    public class SetAppearanceBySelection : Node
    {

        #region Node class methods

        public SetAppearanceBySelection(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("SearchSet", typeof(List<SelectionSet>));
            AddInputPortToNode("Color", typeof(System.Windows.Media.Color));
            AddOutputPortToNode("SearchSet", typeof(List<SelectionSet>));


            AddControlToNode(new Label() { Content = "Get Selection", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            
            IsResizeable = true;

        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null)
            {
                List<SelectionSet> searchs = (List<SelectionSet>)InputPorts[0].Data;

                media.Color color = (media.Color)InputPorts[1].Data;
                
                //Convert ARGB Alpha to normaliced transparency
                double t =  ((-color.A / 255.0)+1) * 100;
                double transparency = t;

                foreach (SelectionSet s in searchs)
                {
                    ApplyAppearance(s, TransformColor(color), transparency);
                }

                OutputPorts[0].Data = (List<SelectionSet>)InputPorts[0].Data;
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
            return new SetAppearanceBySelection(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        #endregion

        #region Methods
        /// <summary>
        /// Transform RGB Media.Color to naviscolor
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Color TransformColor(System.Windows.Media.Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            Color NavisColor = new Color(r, g, b);

            return NavisColor;

        }

        /// <summary>
        /// Transform RGB Drawing.Color to naviscolor
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Color TransformColor(System.Drawing.Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            Color NavisColor = new Color(r, g, b);

            return NavisColor;

        }

        /// <summary>
        /// Apply appearance to selection set
        /// </summary>
        /// <param name="selectionSet"></param>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
        private void ApplyAppearance(SelectionSet selectionSet, Color color, double transparency)
        {
            if (selectionSet != null
                && color != null)

            {
                IEnumerable<ModelItem> modelItems = selectionSet.GetSelectedItems() as IEnumerable<ModelItem>;
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, transparency);
            }

        }
        #endregion


    }

}