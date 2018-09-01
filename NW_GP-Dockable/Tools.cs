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
using Newtonsoft.Json;

namespace NW_GraphicPrograming
{
    public class Tools
    {
        #region Properties
        public static Exchange exchangeFile { get; set; }
        public static JsonSelectionSetsConfiguration jsonSelectionSetsFile { get; set; }


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



        public static void convertXMLtoConfiguration(string path)
        {

            var jsonXML= JsonConvert.SerializeObject(exchangeFile);
            jsonSelectionSetsFile = JsonConvert.DeserializeObject< JsonSelectionSetsConfiguration>(jsonXML);

            File.WriteAllText(path, jsonXML);


        }        

    }



    #endregion
    /// <summary>
    /// Stores selection set configuration to apply into OverridePermanent methods
    /// </summary>
    public class Viewfolder
    {
        public List<object> Selectionset { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public object color { get; set; }
    }

    public class Selectionset
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public object color { get; set; }
    }

    public class Selectionsets
    {
        public List<Viewfolder> Viewfolder { get; set; }
        public List<Selectionset> Selectionset { get; set; }
    }

    public class JsonSelectionSetsConfiguration
    {
        public Selectionsets Selectionsets { get; set; }
        public string Xsi { get; set; }
        public string NoNamespaceSchemaLocation { get; set; }
        public string Units { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
    }


}
