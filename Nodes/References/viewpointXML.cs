/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace ENGyn.XML
{


    [XmlRoot(ElementName = "selectionset")]
    public class Selectionset
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "guid")]
        public string Guid { get; set; }
        [XmlAttribute(AttributeName = "color")]
        public string color { get; set; }
        [XmlAttribute(AttributeName = "transparency")]
        public int transparency { get; set; }
    }

    [XmlRoot(ElementName = "viewfolder")]
    public class Viewfolder
    {
        [XmlElement(ElementName = "selectionset")]
        public List<Selectionset> Selectionset { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "guid")]
        public string Guid { get; set; }
        [XmlAttribute(AttributeName = "color")]
        public string color { get; set; }
        [XmlAttribute(AttributeName = "transparency")]
        public int transparency { get; set; }
    }

    [XmlRoot(ElementName = "selectionsets")]
    public class Selectionsets
    {
        [XmlElement(ElementName = "viewfolder")]
        public List<Viewfolder> Viewfolder { get; set; }
        [XmlElement(ElementName = "selectionset")]
        public List<Selectionset> Selectionset { get; set; }
    }

    [XmlRoot(ElementName = "exchange")]
    public class Exchange
    {
        [XmlElement(ElementName = "selectionsets")]
        public Selectionsets Selectionsets { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string NoNamespaceSchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "units")]
        public string Units { get; set; }
        [XmlAttribute(AttributeName = "filename")]
        public string Filename { get; set; }
        [XmlAttribute(AttributeName = "filepath")]
        public string Filepath { get; set; }
    }

}
