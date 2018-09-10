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

using System.Windows;

namespace ENGyne.Nodes.Appearance
{
    public class GetJsonProfileFromXLM : Node
    {
        //TODO: read xml and convert to json profile
        #region Node class methods
        public GetJsonProfileFromXLM(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("File path", typeof(string));


        }

        public override void Calculate()
        {
            var path = InputPorts[0].Data as string;
            
            if (path != null && File.Exists(path))
            {
                var exchange = Tools.readXML(path);
                Tools.convertXMLtoConfiguration(path.Replace(".xml", ".json"));
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
            return new GetJsonProfileFromXLM(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        #endregion



    }

    }

