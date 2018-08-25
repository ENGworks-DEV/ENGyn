using System.Windows.Controls;
using System.Xml;
using ColorTranslator = System.Drawing.ColorTranslator;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using media = System.Windows.Media;
using Newtonsoft.Json;

using System.Reflection;
using System;
using System.IO;

namespace NW_GraphicPrograming.Nodes
{
    public class SetAppearanceByProfile : Node
    {
        

        public SetAppearanceByProfile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("File path", typeof(string));


            AddControlToNode(new Label() { Content = "Apply appearance profile", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            IsResizeable = true;

        }

        public override void Calculate()
        {

            run();

            
        }


        /// <summary>
        /// Returns navisworks selection set matching GUID string
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
   
        private SelectionSet getSetsByGUID(string guid)
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
        private Color transformColor(System.Windows.Media.Color color)
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
        private Color transformColor(System.Drawing.Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            Color NavisColor = new Color(r, g, b);

            return NavisColor;

        }


        private List<SelectionSets> selectionSetsConfs;


        private void run()
        {

            string st = System.IO.File.ReadAllText(@"E:\Share\NW_GraphicPrograming\selectionSet.json");

            try {
                selectionSetsConfs = JsonConvert.DeserializeObject<List<SelectionSets>>(st);
                }
            catch (Exception exp)
            { System.Windows.MessageBox.Show(exp.Message); }

            if (selectionSetsConfs != null )
            {
                foreach (var set in selectionSetsConfs)
                {
                    RecursiveSets(set);
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
            if (selectionSets.color != null 
                && selectionSets.color != "" 
                && selectionSets.guid != null 
                && selectionSets.guid != "")
            {
                //Get selection set
                SelectionSet set = getSetsByGUID(selectionSets.guid);
                //Pick color from json
                var color = transformColor( ColorTranslator.FromHtml(selectionSets.color));

                if (set != null 
                    && set.GetSelectedItems().Count >0)
                { 
                    //Apply override to set
                    applyAppearance(set, color, selectionSets.transparency);
                    
                }
                
                //If folder has childrens, recurse
                if (selectionSets.sets != null 
                    && selectionSets.sets.Count > 0)

                    foreach (SelectionSets children in selectionSets.sets)
                    {
                        RecursiveSets(children);
                    }
            }
            //Go to childrens if set has no colors
            else
            {
                //If folder has childrens, recurse
                if (selectionSets.sets != null 
                    && selectionSets.sets.Count > 0)

                    foreach (SelectionSets children in selectionSets.sets)
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
            public string guid { get; set; }
            public string color { get; set; }
            public int transparency { get; set; }
            public List<SelectionSets> sets { get; set; }
        }

        /// <summary>
        /// RootObject for selectionsets
        /// </summary>
        public class SelectionSetConfig
        {
            public string help { get; set; }
            public List<SelectionSets> sets { get; set; }
        }


        private void applyAppearance(SelectionSet selectionSet, Color color, double transparency )
        {
            if (selectionSet != null 
                && color != null  )

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
            return new SetAppearanceByProfile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

}