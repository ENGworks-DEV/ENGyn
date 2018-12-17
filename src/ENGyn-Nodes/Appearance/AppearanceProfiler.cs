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
using System.Windows.Data;
using System.Windows.Forms;

namespace ENGyn.Nodes.Appearance
{
    public class AppearanceByProfile : Node
    {

        #region Node class methods
        public AppearanceByProfile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddOutputPortToNode("Profile Path", typeof(string));
            System.Windows.Controls.Button button = new System.Windows.Controls.Button { Content = "Open Profiler", Width = 120 };
            button.Click += show_click;
            AddControlToNode(button);
            //Help 
            this.BottomComment.Text = "Opens ENGyn appearence profiler dialog";
            this.ShowHelpOnMouseOver = true;

        }


        public void show_click(object sender, EventArgs e)
        {
            MainForm main = new MainForm();
            main.ShowDialog();
        }

        public override void Calculate()
        {


            Tools.ReadConfiguration(InputPorts[0].Data.ToString());
            OutputPorts[0].Data = Tools.FilePath;

        }



        public override Node Clone()
        {
            return new AppearanceByProfile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        #endregion

       

        public static class Tools {
            /// <summary>
            /// Read configuration and apply override TODO: Improve description 
            /// </summary>
            public void ReadConfiguration(string path)
            {

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

            private static string path { get; set; }

            public void GetPath()
            {

                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.ShowDialog();
                    if (open.FileName != "")
                    {
                        path = open.FileName;
                        ReadConfiguration(path);
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

                            if (set != null)
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
                        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentColor(modelItems, color);
                    }

                    if (transparency.ToString() != "-1")
                    {
                        Double t = int.Parse(transparency.ToString());
                        Autodesk.Navisworks.Api.Application.ActiveDocument.Models.OverridePermanentTransparency(modelItems, t / 100);
                    }

                }

            }


        }
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
        #endregion
    }
    
}