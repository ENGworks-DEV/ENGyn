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

namespace ENGyn.Nodes.Clash
{
    public class CompactTest : Node
    {
        public CompactTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddOutputPortToNode("ClashTest", typeof(object));
        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            CompactClashes(input);
            OutputPorts[0].Data = Autodesk.Navisworks.Api.Application.ActiveDocument;

        }

        /// <summary>
        /// Compacts single Clashtest list of ClashesTest
        /// </summary>
        /// <param name="input"></param>
        public void CompactClashes(object input)
        {
            var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {

                var t = input.GetType();

                if (t == typeof(Autodesk.Navisworks.Api.Clash.ClashTest))
                {


                    Autodesk.Navisworks.Api.Clash.ClashTest ct = InputPorts[1].Data as Autodesk.Navisworks.Api.Clash.ClashTest;
                    var clashes = doc.GetClash();
                    clashes.TestsData.TestsCompactTest(input as ClashTest);

                }
                if (MainTools.IsList(input))

                {
                    var listData = input as List<object>;
                    if (listData[0].GetType() == typeof(Autodesk.Navisworks.Api.Clash.ClashTest))
                    {
                        foreach (var ct in input as List<object>)
                        {
                            var clashTest = ct as Autodesk.Navisworks.Api.Clash.ClashTest;
                            var clashes = doc.GetClash();
                            clashes.TestsData.TestsCompactTest(clashTest);


                        }
                    }

                }


               
            }
        }
 

        public override Node Clone()
        {
            return new CompactTest(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}