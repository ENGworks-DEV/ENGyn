using System.Windows.Controls;
using System.Xml;
using ColorTranslator = System.Drawing.ColorTranslator;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

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
        private List<SelectionSets> selectionSetsConfs;
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
                    selectionSetsConfs = JsonConvert.DeserializeObject<List<SelectionSets>>(st);
                }
                catch (Exception exp)
                { System.Windows.MessageBox.Show(exp.Message); }

                if (selectionSetsConfs != null)
                {
                    foreach (var set in selectionSetsConfs)
                    {
                        RecursiveSets(set);
                    }

                }
            }
           
                
        }

        /// <summary>
        /// Recurse over SelectionSets objects applying color overrides to sets
        /// </summary>
        /// <param name="selectionSets"></param>
        public void RecursiveSets(SelectionSets selectionSets)
        {
            //Apply color to set if color exists
            if (selectionSets.Color != null 
                && selectionSets.Color != "" 
                && selectionSets.Guid != null 
                && selectionSets.Guid != "")
            {
                //Get selection set
                SelectionSet set = GetSetsByGUID(selectionSets.Guid);
                //Pick color from json
                var color = TransformColor( ColorTranslator.FromHtml(selectionSets.Color));

                if (set != null 
                    && set.GetSelectedItems().Count >0)
                { 
                    //Apply override to set
                    ApplyAppearance(set, color, selectionSets.Transparency);
                    
                }
                
                //If folder has childrens, recurse
                if (selectionSets.Sets != null 
                    && selectionSets.Sets.Count > 0)

                    foreach (SelectionSets children in selectionSets.Sets)
                    {
                        RecursiveSets(children);
                    }
            }
            //Go to childrens if set has no colors
            else
            {
                //If folder has childrens, recurse
                if (selectionSets.Sets != null 
                    && selectionSets.Sets.Count > 0)

                    foreach (SelectionSets children in selectionSets.Sets)
                    {
                        RecursiveSets(children);
                    }
            }

        }

        /// <summary>
        /// Stores selection set configuration to apply into OverridePermanent methods
        /// </summary>
        public class SelectionSets
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Guid { get; set; }
            public string Color { get; set; }
            public int Transparency { get; set; }
            public List<SelectionSets> Sets { get; set; }
        }

        /// <summary>
        /// Apply appearance to selection set
        /// </summary>
        /// <param name="selectionSet"></param>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
        private void ApplyAppearance(SelectionSet selectionSet, Color color, double transparency )
        {
            if (selectionSet != null 
                && color != null  )

            {
                IEnumerable<ModelItem> modelItems = selectionSet.GetSelectedItems() as IEnumerable<ModelItem>;
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, transparency);
            }

        }

        #endregion


        
    }

}