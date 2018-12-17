using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ENGyn.Nodes.Appearance
{
    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm();
            
            Application.Run(mainForm);

        }



    }

static class Tools
    {
        public static JsonSelectionSetsConfiguration selectionSetsConfs { get; set; }

        private static string path { get; set; }

        public static void GetPath()
        {

            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.ShowDialog();
                if (open.FileName != null)
                {
                    path = open.FileName;
                    ReadConfiguration(path);
                }


            }
        }

        private static void ReadConfiguration(string path)
        {

            if (path != null)
            {
                try {
                    string st = System.IO.File.ReadAllText(path);


                    selectionSetsConfs = JsonConvert.DeserializeObject<JsonSelectionSetsConfiguration>(st);

                    if (selectionSetsConfs != null)
                    {

                    }
                }
               catch { }

            }
            var debug = selectionSetsConfs.Filename;

        }
        public static void convertXMLtoConfiguration(string path)
        {

            var jsonXML = JsonConvert.SerializeObject(selectionSetsConfs, Newtonsoft.Json.Formatting.Indented);
            

            File.WriteAllText(path, jsonXML);


        }
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
            public object transparency { get; set; }
        }

        public class  Selectionset
        {
            public string Name { get; set; }
            public string Guid { get; set; }
            public object color { get; set; }
            public object transparency { get; set; }
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
}



