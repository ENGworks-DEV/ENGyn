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

        

        public static void convertXMLtoConfiguration(string path)
        {

            var jsonSerializer = JsonConvert.SerializeObject(exchangeFile);

            File.WriteAllText(path, jsonSerializer);


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
