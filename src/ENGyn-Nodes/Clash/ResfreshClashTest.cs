using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System;
using System.Collections.Generic;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Clash
{
    public class RefreshClashTests : Node
    {
        public RefreshClashTests(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Tests", typeof(object));
            AddOutputPortToNode("Clash Tests", typeof(object));

        }

        public override void Calculate()
        {
            var input = InputPorts[0].Data;


            OutputPorts[0].Data = RefreshClashes(input);

        }

        private List<object> RefreshClashes(object input)
        {
            //Get clashes from document
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            var testData = doc.GetClash().TestsData;
            var output = new List<object>();
            if (input != null)
            {


                if (MainTools.IsList(input))
                {


                    foreach (var item in (System.Collections.IEnumerable)input)
                    {

                        var ClashTest = doc.ResolveReference(item as SavedItemReference) as ClashTest;
                        var clash = doc.Clash as DocumentClash;
                        testData.TestsRunTest(ClashTest);
                        wait(1);
                        output.Add(item);
                    }

                }

                if (input.GetType() == typeof(SavedItemReference))
                {

                    var ClashTest = doc.ResolveReference(input as SavedItemReference) as ClashTest;
                    testData.TestsRunTest(ClashTest);
                    output.Add(input);
                }

            }



            return output;
        }

        void wait(int x)
        {
            DateTime t = DateTime.Now;
            DateTime tf = DateTime.Now.AddSeconds(x);

            while (t < tf)
            {
                t = DateTime.Now;
            }
        }

        public override Node Clone()
        {
            return new RefreshClashTests(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}