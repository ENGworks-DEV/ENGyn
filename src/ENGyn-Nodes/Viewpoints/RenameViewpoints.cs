using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;
using System.Reflection;
using System.Windows;

namespace ENGyn.Nodes.Viewpoints
{
    public class RenameViewpoints : Node
    {
        public RenameViewpoints(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Viewpoints", typeof(object));
            AddInputPortToNode("Name", typeof(string));
            AddOutputPortToNode("Output", typeof(object));

    }

        public void manyToOne(object a, string Parameter)
        {
            var ListA = (System.Collections.IList)a;
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            Autodesk.Navisworks.Api.DocumentParts.DocumentSavedViewpoints documentSavedViewpoints =
                doc.SavedViewpoints;

            foreach (var objA in ListA)
            {
                documentSavedViewpoints.EditDisplayName(objA as SavedItem, Parameter);
            }
        }

        public void oneToOne(object a, object b)
        {
            var ListA = (System.Collections.IList)a;
            var ListB = (System.Collections.IList)b;

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            Autodesk.Navisworks.Api.DocumentParts.DocumentSavedViewpoints documentSavedViewpoints =
                doc.SavedViewpoints;
            if (ListB.Count == ListA.Count)

                {
                for (int i = 0; i < ListA.Count; i++)
                {
                    var objA = ListA[i];
                    string objB = ListB[i].ToString();
                    documentSavedViewpoints.EditDisplayName(objA as SavedItem, objB);

                }
            }
            
        }


        private void ManyToSome(object a, object b)
        {
            var ListA = (System.Collections.ArrayList)a;
            var ListB = (System.Collections.ArrayList)b;

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            Autodesk.Navisworks.Api.DocumentParts.DocumentSavedViewpoints documentSavedViewpoints =
                doc.SavedViewpoints;

            for (int i = 0; i < ListA.Count; i++)
            {
                var objA = ListA[i];
                string objB = ListB[0].ToString();
                documentSavedViewpoints.EditDisplayName(objA as SavedItem, objB);

            }
        }
        public override void Calculate()
        {
            List<object> output = new List<object>();
            if (InputPorts[0].Data != null)
            {
                var input = InputPorts[0].Data;
                var parameter = InputPorts[1].Data;
                if (MainTools.IsList(input) && MainTools.IsList(input))
                {
                    var ListA = (System.Collections.IList)input;
                    var ListB = (System.Collections.IList)input;

                    if (ListB.Count == ListA.Count)
                    {
                        try
                        {
                            oneToOne(input, parameter);
                        }

                        catch (Exception e)
                        {
                            
                            output.Add(e.Message);
                        }
                    }
                    else
                    {

                        ManyToSome(input,  parameter);
                    } 

                }
                else
                {
                    try
                    {
                        
                    }
                    catch
                    {
                        output.Add(null);
                    }
                }


            }

            OutputPorts[0].Data = output;

        }

       


        public override Node Clone()
        {
            return new RenameViewpoints(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }
    public class RenameViewpointsByGUID : Node
    {
        public RenameViewpointsByGUID(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("GUIDs", typeof(object));
            AddInputPortToNode("Name", typeof(string));
            AddOutputPortToNode("Output", typeof(object));

        }

        public void manyToOne(object a, string Parameter)
        {
            var ListA = (System.Collections.IList)a;
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

           

            Autodesk.Navisworks.Api.DocumentParts.DocumentSavedViewpoints documentSavedViewpoints =
                doc.SavedViewpoints;
            
            foreach (var objA in ListA)
            {
                
                Guid guid = new Guid(objA.ToString().Replace("\"", ""));
                documentSavedViewpoints.EditDisplayName(documentSavedViewpoints.ResolveGuid(guid), Parameter.Replace("\"", ""));
            }
        }

        public void oneToOne(object a, object b)
        {
            var ListA = (System.Collections.IList)a;
            var ListB = (System.Collections.IList)b;

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            Autodesk.Navisworks.Api.DocumentParts.DocumentSavedViewpoints documentSavedViewpoints =
                doc.SavedViewpoints;
            if (ListB.Count == ListA.Count)

            {
                for (int i = 0; i < ListA.Count; i++)
                {
                    try {
                        var objA = ListA[i];
                        string objB = ListB[i].ToString();
                        Guid guid = new Guid(objA.ToString().Replace("\"", ""));
                        documentSavedViewpoints.EditDisplayName(documentSavedViewpoints.ResolveGuid(guid), objB.Replace("\"", ""));
                    }
                    catch { }
         

                }
            }

        }


        private void ManyToSome(object a, object b)
        {
            var ListA = (System.Collections.ArrayList)a;
            var ListB = (System.Collections.ArrayList)b;

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            Autodesk.Navisworks.Api.DocumentParts.DocumentSavedViewpoints documentSavedViewpoints =
                doc.SavedViewpoints;

            for (int i = 0; i < ListA.Count; i++)
            {
                var objA = ListA[i];
                string objB = ListB[0].ToString();
                Guid guid = new Guid(objA.ToString().Replace("\"", ""));
                documentSavedViewpoints.EditDisplayName(documentSavedViewpoints.ResolveGuid(guid), objB.Replace("\"", ""));
               

            }
        }
        public override void Calculate()
        {
            List<object> output = new List<object>();
            if (InputPorts[0].Data != null)
            {
                var input = InputPorts[0].Data;
                var parameter = InputPorts[1].Data;
                if (MainTools.IsList(input) && MainTools.IsList(parameter))
                {
                    var ListA = (System.Collections.IList)input;
                    var ListB = (System.Collections.IList)parameter;

                    if (ListB.Count == ListA.Count)
                    {
                        try
                        {
                            oneToOne(input, parameter);
                        }

                        catch (Exception e)
                        {

                            output.Add(e.Message);
                        }
                    }
                    else
                    {

                        ManyToSome(input, parameter);
                    }

                }
                else
                {
                    try
                    {

                    }
                    catch
                    {
                        output.Add(null);
                    }
                }


            }

            OutputPorts[0].Data = output;

        }




        public override Node Clone()
        {
            return new RenameViewpointsByGUID(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}

