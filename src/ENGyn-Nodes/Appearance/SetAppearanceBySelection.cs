using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using media = System.Windows.Media;
using System.Linq;
using System.Reflection;
using System;
using System.Windows;

namespace ENGyn.Nodes.Appearance
{
    public class SetAppearanceBySelection : Node
    {

        #region Node class methods

        public SetAppearanceBySelection(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("Selection", typeof(object));
            AddInputPortToNode("Color", typeof(System.Windows.Media.Color));
            AddOutputPortToNode("SearchSet", typeof(object));

            //Help 
            this.BottomComment.Text = "Applies color and transparency to selection";
            this.ShowHelpOnMouseOver = true;
        }

        public override void Calculate()
        {
            OutputPorts[0].Data = MainTools.RunFunction(setAppearanceBySelection, InputPorts);
        }


        /// <summary>
        /// Set Appearance from color to selection
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <returns>Returns input</returns>
        private object setAppearanceBySelection(object input,object color)
        {
            object output = null;
            media.Color CurrentColor = (media.Color)color;

            if (input != null)
            {
                var tt = input.GetType();
                bool istype = input.GetType() == typeof(SelectionSet);

                if (MainTools.IsList(input) && istype)
                {

                    //Convert ARGB Alpha to normaliced transparency
                    double t = ((-CurrentColor.A / 255.0) + 1) * 100;
                    double transparency = t;

                    foreach (var s in (System.Collections.IEnumerable)input)
                    {

                        ApplyAppearance(s as SelectionSet, TransformColor(CurrentColor), transparency);
                    }

                    OutputPorts[0].Data = (System.Collections.IEnumerable)input;
                }

                if (MainTools.IsList(input) && MainTools.ListContainsType(input, typeof(ModelItem)) || input is ModelItem)
                {

                    List<ModelItem> searchs = ((List<object>)InputPorts[0].Data).Cast<ModelItem>().ToList();

                    //Convert ARGB Alpha to normaliced transparency
                    double t = ((-CurrentColor.A / 255.0) + 1) * 100;
                    double transparency = t;


                    ApplyAppearance(searchs, TransformColor(CurrentColor), transparency);

                   
                }

            }
            return output;
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

        /// <summary>
        /// Apply appearance to modelitems list
        /// </summary>
        /// <param name="modelItems"></param>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
        private void ApplyAppearance(List<ModelItem> modelItems, Color color, double transparency)
        {
            if (modelItems != null
                && color != null)

            {
                
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, transparency);
            }

        }
        #endregion


    }

}