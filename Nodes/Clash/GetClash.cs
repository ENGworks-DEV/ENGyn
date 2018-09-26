using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System.Windows;

namespace ENGyn.Nodes.Clash
{
    public class ClashResults : Node
    {
        public ClashResults(VplControl hostCanvas)
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
                ClashTest item =  doc.ResolveReference(input as SavedItemReference) as ClashTest;
                foreach (var t in item.Children)
                {
                        clashResultList.Add(t);
                }
                OutputPorts[0].Data = clashResultList;
                

            }


        }


        public override Node Clone()
        {
            return new ClashResults(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}