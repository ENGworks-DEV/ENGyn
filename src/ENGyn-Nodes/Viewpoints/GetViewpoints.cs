using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System;

namespace ENGyn.Nodes.Viewpoints
{
    public class GetViewpoints : Node
    {
        

        public GetViewpoints(VplControl hostCanvas)
            : base(hostCanvas)
        {
            
            AddOutputPortToNode("Viewpoints", typeof(object));

        }

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            SavedViewpoints = new List<object>();
            List<SelectionSet> Viewpoints = new List<SelectionSet>();
            if (doc.SavedViewpoints != null)
            { 
            SavedItemCollection viewpoints =  doc.SavedViewpoints.ToSavedItemCollection();
            try {
                foreach (SavedItem view in viewpoints)
                {
                        var t = view.GetType();
                         var name = view.DisplayName;
                        RecursionViewpoint(view);
                    
                }
            }
            catch (System.Exception e)
            { System.Windows.MessageBox.Show(e.Message); }
            
            OutputPorts[0].Data = SavedViewpoints;
        }
            }
        public List<object> SavedViewpoints { get; set; }

        private List<SelectionSet> SelectionSet { get; set; }

        private void RecursionViewpoint(object s)
        {
            if (s != null)
            {
                if (s.GetType() == typeof(FolderItem))
                {
                    var folder = s as FolderItem;
                    foreach (var children in folder.Children)
                    {
                        RecursionViewpoint(children);
                    }
                }
               if (s.GetType() == typeof(SavedViewpoint))
                {
                    if (SavedViewpoints != null)
                    {
                        SavedViewpoints.Add(s as SavedViewpoint);
                    }

                   
                }
            }

        }


        private SelectionSet IterateSelections(SavedItem savedItem)
        { return null; }
        public void GetSets(SavedItem savedItem)
        {

             SelectionSet = new List<SelectionSet>();

            if (savedItem.GetType() == typeof(FolderItem))
            {
                var folder = savedItem as FolderItem;

                foreach (SavedItem si in folder.Children)
                {
                    GetSets(si);
                }
            }
            else
            {
                SelectionSet.Add(savedItem as SelectionSet);
            }

            
        }



        public override Node Clone()
        {
            return new GetViewpoints(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }


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
                    try
                    {
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

    public class ExportViewpoint : Node
    {


        public ExportViewpoint(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Viewpoints", typeof(object));
            AddInputPortToNode("Name", typeof(object));
            AddInputPortToNode("Folder Path", typeof(object));
            AddOutputPortToNode("Viewpoints", typeof(object));

        }

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            var viewpoints = doc.SavedViewpoints;

            object item = null;
            object name = null;
            object path = null;


            OutputPorts[0].Data = MainTools.RunFunction(exportViewpoint, InputPorts); 

        }

        private static object exportViewpoint( object item, object name, object path)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            var svp = item as SavedViewpoint;
            var vp = svp.Viewpoint;


            doc.ActiveView.CopyViewpointFrom(vp, ViewChange.JumpCut);
            System.Drawing.Bitmap thumbnailImage = doc.ActiveView.GenerateThumbnail(1000, 800);
            thumbnailImage.Save(string.Format(@"{0}{1}.jpg", path.ToString(), name.ToString()));
            return svp;
        }


        public override Node Clone()
        {
            return new ExportViewpoint(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }
}