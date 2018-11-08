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
        public bool Override { get; private set; }

        #region Node class methods

        public SetAppearanceBySelection(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("Selection", typeof(object));
            AddInputPortToNode("Color", typeof(System.Windows.Media.Color));
            AddInputPortToNode("Transparency", typeof(object));

            StackPanel stackPanel = new StackPanel();

            //Grouping Mode

            stackPanel.Children.Add(new Label() { Content = "Override", Foreground = System.Windows.Media.Brushes.White, VerticalContentAlignment = System.Windows.VerticalAlignment.Bottom });
            ComboBox Override = new ComboBox() { Items = { true, false } };
            stackPanel.Children.Add(Override);
            AddControlToNode(stackPanel);

            AddOutputPortToNode("SearchSet", typeof(object));

            //Help 
            this.BottomComment.Text = "Applies color and transparency to selection. " +
                "Transparency is an integral from 0 to 100." +
                " If Override is set to true, " +
                "even viewpoints with saved visibility (sticky) will be updated";

            this.ShowHelpOnMouseOver = true;
        }

        public override void Calculate()
        {
            var stack = ControlElements[0] as StackPanel;
            //Basic grouping
            var OverrideComboBox = stack.Children[1] as ComboBox;
            Override = false;
            if (OverrideComboBox.SelectedItem != null)
            {

                Override = bool.Parse(OverrideComboBox.SelectedItem.ToString());
            }

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
                    if (Override)
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

                    if (Override)
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
        /// Apply appearance to modelitem
        /// </summary>
        /// <param name="modelItems"></param>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
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

        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            var stack = ControlElements[0] as StackPanel;

            var GroupingComboBox = stack.Children[1] as ComboBox;
            if (GroupingComboBox != null)
            {
                xmlWriter.WriteStartAttribute("SelectedIndex-Relation");
                xmlWriter.WriteValue(GroupingComboBox.SelectedIndex);
                xmlWriter.WriteEndAttribute();

            }


        }


        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            var stack = ControlElements[0] as StackPanel;

            var GroupingComboBox = stack.Children[1] as ComboBox;
            if (GroupingComboBox != null)
            {
                var value = xmlReader.GetAttribute("SelectedIndex-Relation");
                var index = Convert.ToInt32(value);
                GroupingComboBox.SelectedIndex = index;
            }


        }

    }

}