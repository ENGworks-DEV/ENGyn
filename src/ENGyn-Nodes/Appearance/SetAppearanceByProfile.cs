using System.Windows.Controls;
using System.Xml;
using ColorTranslator = System.Drawing.ColorTranslator;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using ENGyn.XML;
using System.IO;
using System.Xml.Serialization;
using System.Windows;

namespace ENGyn.Nodes.Appearance
{
    public class SetAppearanceByProfile : Node
    {

        #region Node class methods
        public SetAppearanceByProfile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("File path", typeof(string));


        }

        public override void Calculate()
        {

            ReadConfiguration();

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
                    foreach (var set in selectionSetsConfs.SelectionSets.Selectionset)
                    {
                        RecursiveSets(set);
                    }
                    foreach (var set in selectionSetsConfs.SelectionSets.Viewfolder)
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

                        if (set != null  )
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
                        var set = GetSetsByGUID(selectionSets.Guid);

                        if (set != null)
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
                && (Color != null || transparency.ToString() != "-1"))

            {
                var debug = selectionSet.Search.FindAll(false);
                IEnumerable<ModelItem> modelItems = debug as IEnumerable<ModelItem>;
              

                if (Color != null)
                {
                    //Pick color from json
                    var color = TransformColor(ColorTranslator.FromHtml(Color.ToString()));
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color); }
                
                if (transparency.ToString() != "-1")
                {
                    Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, int.Parse(transparency.ToString()));
                }
               
            }

        }

        #endregion
    }
    public class Tools
    {
        #region Properties
        public static Exchange exchangeFile { get; set; }
        public static JsonSelectionSetsConfiguration jsonSelectionSetsFile { get; set; }


        #endregion

        #region Methods
        public static Exchange readXML(string path)
        {

            if (path != null && File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);


                XmlSerializer serializer = new XmlSerializer(typeof(Exchange));
                exchangeFile = (Exchange)serializer.Deserialize(fs);


                fs.Close();
            }
            return exchangeFile;

        }



        public static void convertXMLtoConfiguration(string path)
        {

            var jsonXML = JsonConvert.SerializeObject(exchangeFile, Newtonsoft.Json.Formatting.Indented);
            jsonSelectionSetsFile = JsonConvert.DeserializeObject<JsonSelectionSetsConfiguration>(jsonXML);

            File.WriteAllText(path, jsonXML);


        }

    }



    #endregion
    /// <summary>
    /// Stores selection set configuration to apply into OverridePermanent methods
    /// </summary>
    public class Viewfolder : Selectionset
    {
        public List<Viewfolder> Viewfolders { get; set; }
        public List<Selectionset> Selectionset { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public object color { get; set; }
        public object transparency { get; set; } = "-1";
    }

    public class Selectionset
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public object color { get; set; }
        public object transparency { get; set; } = "-1";
    }

    public class SelectionSets
    {
        public List<Viewfolder> Viewfolder { get; set; }
        public List<Selectionset> Selectionset { get; set; }
    }

    public class JsonSelectionSetsConfiguration
    {
        public SelectionSets SelectionSets { get; set; }
        public string Xsi { get; set; }
        public string NoNamespaceSchemaLocation { get; set; }
        public string Units { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
    }
}