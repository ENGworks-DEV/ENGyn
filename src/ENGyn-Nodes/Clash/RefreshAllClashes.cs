using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System.Collections.Generic;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Clash
{
    public class RefreshAllClashTests : Node
    {
        public int countstarted { get; set; }
        public int countclosed { get; set; }


        public RefreshAllClashTests(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(object));
            AddOutputPortToNode("ClashTests", typeof(object));

        }

        public override void Calculate()
        {
            var input = InputPorts[0].Data;

            countclosed = 0;
            countstarted = 0;
            OutputPorts[0].Data = RefreshClashes(input);
            //MessageBox.Show("count", countclosed.ToString() + "  " + countstarted.ToString());
        }

        private List<object> RefreshClashes(object input)
        {
            //Get clashes from document
            Document doc = input as Document;
            var testData = doc.GetClash().TestsData;
            var output = new List<object>();
            if (input != null)
            {
                testData.TestsRunAllTests();
                foreach (var item in testData.Tests)
                {


                    output.Add(testData.CreateReference(item));

                }

                return output;
            }
            return null;
        }



        public override Node Clone()
        {
            return new RefreshAllClashTests(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}