using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System.Collections.Generic;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Clash
{
    public class GetClashResults : Node
    {
        public GetClashResults(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Clash Test", typeof(object));
            AddOutputPortToNode("Navis Clash", typeof(object));


        }


        public override void Calculate()
        {



            List<object> clashResultList = new List<object>();

            var input = InputPorts[0].Data;
            //Get clashes from document
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            //Check for null in input
            if (MainTools.IsList(input))
            {

                foreach (var i in (System.Collections.IEnumerable)input)
                {
                    if (i.GetType() == typeof(SavedItemReference))
                    {
                        ClashTest item = doc.ResolveReference(i as SavedItemReference) as ClashTest;
                        foreach (var t in item.Children)
                        {
                            clashResultList.Add(t);

                        }
                    }
                }
                OutputPorts[0].Data = clashResultList;

            }

            if (input.GetType() == typeof(SavedItemReference))
            {
                ClashTest item = doc.ResolveReference(input as SavedItemReference) as ClashTest;
                foreach (var t in item.Children)
                {
                    clashResultList.Add(t);
                }
                OutputPorts[0].Data = clashResultList;


            }


        }


        public override Node Clone()
        {
            return new GetClashResults(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}
