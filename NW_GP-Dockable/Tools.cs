using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NW_GraphicPrograming.XML;
using System.Threading;

namespace NW_GraphicPrograming
{
    public class Tools
    {
        #region Properties
        public static Exchange exchangeFile { get; set; }

        #endregion
        #region Methods
        public static  Exchange readXML(string path )
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

        public static JsonSelectionSets jsonSelectionSetsFile { get; set; }

        

        public static void convertXMLtoConfiguration(object item)
        {


                if (item.GetType() == typeof(Exchange))
                {
                    Exchange ex = item as Exchange;
                    var sets = ex.Selectionsets;
                    jsonSelectionSetsFile = new JsonSelectionSets();
                    jsonSelectionSetsFile.Name = ex.Filename;
                    foreach (var i in sets.Viewfolder)
                    {
                        

                        convertXMLtoConfiguration(i, jsonSelectionSetsFile);
                    }
                    foreach (var i in sets.Selectionset)
                    {
                        

                        convertXMLtoConfiguration(i, jsonSelectionSetsFile);
                    }
                }


        }

        public static void convertXMLtoConfiguration(object item, JsonSelectionSets jsonSelectionSets)
        {


            if (item.GetType() == typeof(XML.Viewfolder))
            {
                JsonSelectionSets jSS_Child = new JsonSelectionSets();
                Viewfolder viewfolder = item as Viewfolder;
                jSS_Child.Name = viewfolder.Name;
                jSS_Child.Guid = viewfolder.Guid;
                jSS_Child.Type = "Folder";

       
                if (jsonSelectionSets.Sets == null)
                {
                    jsonSelectionSets.Sets = new List<JsonSelectionSets>();
                }
                jsonSelectionSets.Sets.Add(jSS_Child);
                convertXMLtoConfiguration(item, jSS_Child);


            }
            if (item.GetType() == typeof(XML.Selectionset))
            {
                JsonSelectionSets jSS_Child = new JsonSelectionSets();
                XML.Selectionset ss = item as XML.Selectionset;
                jSS_Child.Name = ss.Name;
                jSS_Child.Guid = ss.Guid;
                jSS_Child.Type = "SelectionSet";
                if (jsonSelectionSets.Sets == null)
                {
                    jsonSelectionSets.Sets = new List<JsonSelectionSets>();
                }
                jsonSelectionSets.Sets.Add(jSS_Child);
            }
        }

    }



#endregion
    /// <summary>
    /// Stores selection set configuration to apply into OverridePermanent methods
    /// </summary>
    public class JsonSelectionSets
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Guid { get; set; }
        public string Color { get; set; }
        public int Transparency { get; set; }
        public List<JsonSelectionSets> Sets { get; set; }
    }


}
