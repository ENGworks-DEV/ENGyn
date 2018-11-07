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
        private Document doc;

        public List<object> SavedViewpoints { get; private set; }

        #region Node class methods

        public SetAppearanceBySelection(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("Selection", typeof(object));
            AddInputPortToNode("Color", typeof(System.Windows.Media.Color));
            AddInputPortToNode("Transparency", typeof(object));
            AddOutputPortToNode("SearchSet", typeof(object));

            //Help 
            this.BottomComment.Text = "Applies color and transparency to selection. Transparency is an integral from 0 to 100";
            this.ShowHelpOnMouseOver = true;
        }

        public override void Calculate()
        {
            doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            SavedViewpoints = new List<object>();
            List<SelectionSet> Viewpoints = new List<SelectionSet>();
            if (doc.SavedViewpoints != null)
            {
                SavedItemCollection viewpoints = doc.SavedViewpoints.ToSavedItemCollection();
                try
                {
                    foreach (SavedItem view in viewpoints)
                    {
                        var t = view.GetType();
                        var name = view.DisplayName;
                        RecursionViewpoint(view);

                    }
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }

            }

                OutputPorts[0].Data = MainTools.RunFunction(setAppearanceBySelections, InputPorts);
        }
            private void RecursionViewpoint(object s)
            {

                if (s != null)
                {
                    if (s.GetType() == typeof(FolderItem))
                    {
                        var folder = s as FolderItem;
                        foreach (var children in folder.Children)
                        {
                            RecursionViewpoint(children);
                        }
                    }
                    if (s.GetType() == typeof(SavedViewpoint))
                    {
                        if (SavedViewpoints != null)
                        {
                            SavedViewpoints.Add(doc.SavedViewpoints.CreateReference(s as SavedViewpoint));
                        }


                    }
                }

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
                if (input is SelectionSet)
                {

                    //Convert ARGB Alpha to normaliced transparency
                    double t = ((-CurrentColor.A / 255.0) + 1) * 100;
                    double transparency = t;

                   ApplyAppearance(input as SelectionSet, TransformColor(CurrentColor), transparency);

                }

                if ( input is ModelItem)
                {

                    List<ModelItem> searchs = new List<ModelItem>() { input as ModelItem};

                    //Convert ARGB Alpha to normaliced transparency
                    double t = ((-CurrentColor.A / 255.0) + 1) * 100;
                    double transparency = t;


                    ApplyAppearance(searchs, TransformColor(CurrentColor), transparency);
            


                }

            }
            output = input;
            return output;
        }

        private object setAppearanceBySelections(object input, object color, object transparency)
        {
            object output = null;
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            if (input != null)
            {
                if (input is SelectionSet)
                {


                    double t = transparency != null ? (double.Parse(transparency.ToString())) / 100: -1;

                    var CurrentColor = color != null ? TransformColor((media.Color)color) : null;
                    if (true)
                    {
                        

                        foreach (object item in SavedViewpoints)
                        {
                            SavedItemReference vp = item as SavedItemReference;
                            if (vp!=null)
                            {

                                SavedViewpoint current = doc.SavedViewpoints.ResolveReference(vp) as SavedViewpoint;
                                doc.ActiveView.CopyViewpointFrom(current.Viewpoint, ViewChange.JumpCut);
                                ApplyAppearance(input as SelectionSet, CurrentColor, t);
                                doc.ActiveView.RequestDelayedRedraw(ViewRedrawRequests.All);
                                doc.SavedViewpoints.ReplaceFromCurrentView(current);
                            }
                        }

                    }
                    ApplyAppearance(input as SelectionSet, CurrentColor, t);

                }

                if (input is ModelItem)
                {

                    List<ModelItem> searchs = new List<ModelItem>() { input as ModelItem };

                    //Convert ARGB Alpha to normaliced transparency
                    double t = transparency != null ? (int.Parse(transparency.ToString())) * 100 : -1;


                    var CurrentColor = color != null ? TransformColor((media.Color)color) : null;

                    if (true)
                    {
                        foreach (object item in SavedViewpoints)
                        {
                            SavedItemReference vp = item as SavedItemReference;
                            if (vp != null)
                            {
                                SavedViewpoint current = doc.SavedViewpoints.ResolveReference(vp) as SavedViewpoint;
                                doc.ActiveView.CopyViewpointFrom(current.Viewpoint, ViewChange.JumpCut);
                                ApplyAppearance(input as SelectionSet, CurrentColor, t);
                                doc.ActiveView.RequestDelayedRedraw(ViewRedrawRequests.All);
                                doc.SavedViewpoints.ReplaceFromCurrentView(current);
                            }
                        }
                    }

                    ApplyAppearance(input as ModelItem, CurrentColor, t);



                }

            }
            output = input;
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
                )

            {
                IEnumerable<ModelItem> modelItems = selectionSet.GetSelectedItems() as IEnumerable<ModelItem>;
                if (color!= null)
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                if (transparency >= 0)
                { Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, transparency); }
                
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
                )

            {

                if (color != null)
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                if (transparency >= 0)
                { Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, transparency); }
            }

        }
        private void ApplyAppearance(ModelItem modelItems, Color color, double transparency)
        {
            if (modelItems != null
                )

            {

                if (color != null)
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(new List<ModelItem>() { modelItems }, color);
                if (transparency >= 0)
                { Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(new List<ModelItem>() { modelItems }, transparency); }
            }

        }
        #endregion


    }

}