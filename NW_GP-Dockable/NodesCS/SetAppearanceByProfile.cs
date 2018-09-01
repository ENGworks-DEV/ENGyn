using System.Windows.Controls;
using System.Xml;
using ColorTranslator = System.Drawing.ColorTranslator;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using NW_GraphicPrograming.XML;

namespace NW_GraphicPrograming.Nodes
{
    public class SetAppearanceByProfile : Node
    {

        #region Node class methods
        public SetAppearanceByProfile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("File path", typeof(string));


            AddControlToNode(new Label() { Content = "Apply appearance profile", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            IsResizeable = true;

        }

        public override void Calculate()
        {

            ReadConfiguration();

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
            return new SetAppearanceByProfile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        #endregion

        #region Properties
        private JsonSelectionSetsConfiguration selectionSetsConfs;

        #endregion

        #region Methods

        /// <summary>
        /// Returns navisworks selection set matching GUID string
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private SelectionSet GetSetsByGUID(string guid)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            DocumentSelectionSets selections = doc.SelectionSets;

            SelectionSet outputSelectionSet = selections.ResolveGuid(System.Guid.Parse(guid)) as SelectionSet ?? new SelectionSet();

            return outputSelectionSet;
        }

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
        /// Read configuration and apply override TODO: Improve description 
        /// </summary>
        private void ReadConfiguration()
        {

            string path = InputPorts[0].Data.ToString();
            if (path != null)
            {
                string st = System.IO.File.ReadAllText(path);

                try
                {
                    selectionSetsConfs = JsonConvert.DeserializeObject<JsonSelectionSetsConfiguration>(st);
                }
                catch (Exception exp)
                { System.Windows.MessageBox.Show(exp.Message); }

                if (selectionSetsConfs != null)
                {
                    foreach (var set in selectionSetsConfs.Selectionsets.Selectionset)
                    {
                        RecursiveSets(set);
                    }
                    foreach (var set in selectionSetsConfs.Selectionsets.Viewfolder)
                    {
                        RecursiveSets(set);
                    }
                }

            }


        }

        /// <summary>
        /// Recurse over SelectionSets objects applying color overrides to sets
        /// </summary>
        /// <param name="obj"></param>
        private void RecursiveSets(object obj)
        {
            #region SelectionSet type
            //If object is a selection set only apply color
            if (obj is Selectionset)
            {
                var selectionSets = obj as Selectionset;

                //Apply color to set if color/transparency exists
                if (selectionSets.Guid != null && selectionSets.Guid != "")
                {
                    //Get selection set
                    try
                    {
                        SelectionSet set = GetSetsByGUID(selectionSets.Guid);

                        if (set != null
                            && set.GetSelectedItems().Count > 0)
                        {
                            //Apply override to set
                            ApplyAppearance(set, selectionSets.color, selectionSets.transparency);
                        }

                    }
                    catch (Exception exp)
                    {
                        // Lets do something with this in the future
                    }

                }
            }
            #endregion

            #region Viewfolder type
            if (obj is Viewfolder)
            {
                var selectionSets = obj as Viewfolder;

                //Apply color to set if color/transparency exists
                if (selectionSets.Guid != null && selectionSets.Guid != "")
                {

                    //Get selection set
                    try
                    {
                        SelectionSet set = GetSetsByGUID(selectionSets.Guid);

                        if (set != null
                            && set.GetSelectedItems().Count > 0)
                        {
                            //Apply override to set
                            ApplyAppearance(set, selectionSets.color, selectionSets.transparency);
                            foreach (var s in selectionSets.Selectionset)
                            {
                                RecursiveSets(s);
                            }
                        }

                    }
                    catch (Exception exp)
                    {
                        // Lets do something with this in the future
                    }

                }
            }
            
            #endregion
        }

        /// <summary>
        /// Apply appearance to selection set
        /// </summary>
        /// <param name="selectionSet"></param>
        /// <param name="Color"></param>
        /// <param name="transparency"></param>
        private void ApplyAppearance(SelectionSet selectionSet, object Color, object transparency)
        {
            if (selectionSet != null
                && (Color != null || transparency != null))

            {
                IEnumerable<ModelItem> modelItems = selectionSet.GetSelectedItems() as IEnumerable<ModelItem>;
                if (Color != null)
                {
                    //Pick color from json
                    var color = TransformColor(ColorTranslator.FromHtml(Color.ToString()));
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color); }
                
                if (transparency != null)
                {
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, int.Parse(transparency.ToString()));
                }
               
            }

        }

        #endregion
    }

}