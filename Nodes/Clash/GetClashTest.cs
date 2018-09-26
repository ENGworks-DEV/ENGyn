using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System;
using System.Collections.Generic;
using System.Linq;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Clash
{
    public class GetClashTest : Node
    {
        public GetClashTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("NW Document", typeof(Document));
            AddOutputPortToNode("ClashTest", typeof(Object));

        }

        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[0].Data is Document)
            {
                OutputPorts[0].Data = GetClashes(InputPorts[0].Data).ToList();
            }
        }

        private IEnumerable<object> GetClashes(object input)
        {
            Document doc = input as Document;
            var testData = doc.GetClash().TestsData;

            if (testData != null && testData.Tests.Count > 0)
            {
                List<Object> clashTestList = new List<Object>();
                foreach (var t in testData.Tests)
                {
                    yield return testData.CreateReference(t);
                }


            }
            else
            { yield return 0; }
        }



        public override Node Clone()
        {
            return new GetClashTest(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}