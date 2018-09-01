/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace NW_GraphicPrograming.XML
{
    [XmlRoot(ElementName = "name")]
    public class Name
    {
        [XmlAttribute(AttributeName = "internal")]
        public string Internal { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "category")]
    public class Category
    {
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
    }

    [XmlRoot(ElementName = "property")]
    public class Property
    {
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "value")]
    public class Value
    {
        [XmlElement(ElementName = "data")]
        public Data Data { get; set; }
    }

    [XmlRoot(ElementName = "condition")]
    public class Condition
    {
        [XmlElement(ElementName = "category")]
        public Category Category { get; set; }
        [XmlElement(ElementName = "property")]
        public Property Property { get; set; }
        [XmlElement(ElementName = "value")]
        public Value Value { get; set; }
        [XmlAttribute(AttributeName = "test")]
        public string Test { get; set; }
        [XmlAttribute(AttributeName = "flags")]
        public string Flags { get; set; }
    }

    [XmlRoot(ElementName = "conditions")]
    public class Conditions
    {
        [XmlElement(ElementName = "condition")]
        public List<Condition> Condition { get; set; }
    }

    [XmlRoot(ElementName = "findspec")]
    public class Findspec
    {
        [XmlElement(ElementName = "conditions")]
        public Conditions Conditions { get; set; }
        [XmlElement(ElementName = "locator")]
        public string Locator { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string Mode { get; set; }
        [XmlAttribute(AttributeName = "disjoint")]
        public string Disjoint { get; set; }
    }

    [XmlRoot(ElementName = "selectionset")]
    public class Selectionset
    {
        [XmlElement(ElementName = "findspec")]
        public Findspec Findspec { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "guid")]
        public string Guid { get; set; }
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
