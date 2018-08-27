using System.Windows.Controls;
using System.Xml;
using ColorTranslator = System.Drawing.ColorTranslator;
using Autodesk.Navisworks.Api;
using System.Text;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using System.IO;

using NW_GraphicPrograming.XML;

namespace NW_GraphicPrograming.Nodes
{
    public class GetProfileFromXML : Node
    {
        //TODO: read xml and convert to json profile
        #region Node class methods
        public GetProfileFromXML(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("File path", typeof(string));


            AddControlToNode(new Label() { Content = "Appearance profile from XML", FontSize = 13, VerticalAlignment = System.Windows.VerticalAlignment.Top });
            IsResizeable = true;

        }

        public override void Calculate()
        {
            readXML();



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
            return new GetProfileFromXML(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        #endregion

        #region Properties
        public exchange exchange { get; set; }

        #endregion
        #region Methods
        public void readXML ()
        {
            
            string path = InputPorts[0].Data as string ?? @"C:\Users\pdere\Desktop\python\171002_PRO_VPA_ARC_QJA_M3D_10.xml";
            FileStream fs = new FileStream(path, FileMode.Open);

            try {
            XmlSerializer serializer = new XmlSerializer(typeof(exchange));
                exchange = (exchange)serializer.Deserialize(fs);
            }
            catch (System.Exception ex)
            { System.Windows.MessageBox.Show(ex.Message); }

            

            fs.Close();


        }

        private JsonSelectionSets jsonSelectionSets { get; set; } = new JsonSelectionSets();

        public void convertXMLtoConfiguration(object item)
        {
              if (item.GetType() == typeof(exchange))
            {
                exchange ex = item as exchange;
                foreach (var i in ex.selectionsets)
                {
                    convertXMLtoConfiguration(i);
                }
               
            }

              if (item.GetType() == typeof(XML.exchangeSelectionsetsViewfolder))
                {

                exchangeSelectionsetsViewfolder viewfolder = item as exchangeSelectionsetsViewfolder;

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

