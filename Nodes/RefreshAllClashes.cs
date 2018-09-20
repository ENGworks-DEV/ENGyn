using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;
using System.Windows;
using System.Linq;

namespace ENGyn.Nodes.Clash
{
    public class RefreshAllClashTests : Node
    {
        public RefreshAllClashTests(VplControl hostCanvas)
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
               return  output;
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