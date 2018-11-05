using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System;
using System.Collections.Generic;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Clash
{
    public class RenameClashesByGuid : Node
    {
        public RenameClashesByGuid(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Clash GUID", typeof(object));
            AddInputPortToNode("Clash Name", typeof(object));
            AddOutputPortToNode("Clash Results", typeof(object));

            //Help 
            this.BottomComment.Text = "Rename a clash result by GUID. It's a good idea to use in excel/CSV after exporting the info with GetClashInfo's Clash Name and GUID ";
            this.ShowHelpOnMouseOver = true;

        }


        /// <summary>
        /// 
        /// </summary>
        public override void Calculate()
        {

            List<object> clashResultList = new List<object>();

            var GUIDS = InputPorts[0].Data;
            var Names = InputPorts[1].Data;
            //Get clashes from document

            if (GUIDS != null && Names != null)
            {
               OutputPorts[0].Data= RenameClashes(GUIDS, Names);
            }
            

        }

        public List<object> RenameClashes(object Guids, object Names)
        {
            //Get clashes from document
            Document doc = Application.ActiveDocument;
            DocumentClash documentClash = doc.GetClash();
            var output = new List<object>();
            var TemplistOfTest = new List<ClashTest>();

            if (MainTools.IsList(Guids) && MainTools.IsList(Names))
            {
                var guidlist = (List<object>)Guids;
                var namesList = (List<object>)Names;

                if (guidlist.Count == namesList.Count)
                {
                    for (int i = 0; i < guidlist.Count; i++)
                    {
                        Guid guid = new Guid(guidlist[i].ToString().Replace("\"",""));
                        var clashReference = documentClash.TestsData.ResolveGuid(guid);
                        var name = namesList[i].ToString();
                        documentClash.TestsData.TestsEditDisplayName(clashReference, name); ;
                        output.Add(clashReference);
                    }
                }
                else
                {
                    output.Add("Error, be sure you have same amount of guids and names");
                }
            }
            return output;
        }

        public void process(object Guids, object Names)
        {
            //Get clashes from document
            Document doc = Application.ActiveDocument;
            DocumentClash documentClash = doc.GetClash();

            var TemplistOfTest = new List<ClashTest>();

            if (MainTools.IsList(Guids) && MainTools.IsList(Names))
            {
                var guidlist = (List<object>)Guids;
                var namesList = (List<object>)Names;

                if (guidlist.Count == namesList.Count)
                {
                    for (int i = 0; i < guidlist.Count; i++)
                    {
                        Guid guid = new Guid(guidlist[i].ToString());
                        var clashReference = documentClash.TestsData.ResolveGuid(guid);
                        documentClash.TestsData.TestsEditDisplayName(clashReference, namesList[i].ToString());

                    }
                }

            }

            }

        public override Node Clone()
        {
            return new RenameClashesByGuid(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}