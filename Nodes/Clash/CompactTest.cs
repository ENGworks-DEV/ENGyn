using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows;

namespace ENGyn.Nodes.Clash
{
    public class CompactAllTests : Node
    {
        public CompactAllTests(VplControl hostCanvas)
               : base(hostCanvas)
        {
            AddInputPortToNode("Any", typeof(object));
            AddOutputPortToNode("ClashTests", typeof(object));


        }

        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var RESULT = CompactAllClashTest(input);
            OutputPorts[0].Data = RESULT;

        }



        public override Node Clone()
        {
            return new CompactAllTests(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }

        public List<object> CompactAllClashTest(object input)
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            var clashes = doc.GetClash();
            var output = new List<object>();
            if (input!= null)
            {


                clashes.TestsData.TestsCompactAllTests();

            }
            foreach (var item in clashes.TestsData.Tests)
            {
                output.Add(clashes.TestsData.CreateReference(item));
            }

            return output;
        }
    }


    public class CompactTest : Node
    {
        public CompactTest(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("ClashTest", typeof(object));
            AddOutputPortToNode("ClashTests", typeof(object));
        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var RESULT = CompactClashTest(input);
            OutputPorts[0].Data = RESULT;
           

        }

        /// <summary>
        /// Compacts single Clashtest list of ClashesTest
        /// </summary>
        /// <param name="input"></param>
        public IEnumerable<object> CompactClashes(object input)
        {
            var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {

                var t = input.GetType();

                if (t == typeof(SavedItemReference))
                {


                    var ClashFromReference = doc.ResolveReference(input as SavedItemReference) as ClashTest;
                    var clashes = doc.GetClash();
                    clashes.TestsData.TestsCompactTest(ClashFromReference);
                    yield return input;
                }
                if (MainTools.IsList(input))

                {
                    var listData = input as List<object>;
                    if (listData[0].GetType() == typeof(Autodesk.Navisworks.Api.Clash.ClashTest))
                    {
                        foreach (var ct in input as List<object>)
                        {
                            var ClashFromReference = doc.ResolveReference(ct as SavedItemReference) as ClashTest;
                            var clashes = doc.GetClash();
                            clashes.TestsData.TestsCompactTest(ClashFromReference);

                            yield return ct;

                        }
                    }

                }


               
            }
        }

        public List<object> CompactClashTest(object input)
        {
            var output = new List<object>();
            var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            if (input != null)
            {

                var t = input.GetType();

                if (t == typeof(SavedItemReference))
                {


                    var ClashFromReference = doc.ResolveReference(input as SavedItemReference) as ClashTest;
                    var clashes = doc.GetClash();

                    clashes.TestsData.TestsCompactTest(ClashFromReference);
                    output.Add(input);
                }
                if (MainTools.IsList(input))

                {
                    var listData = input as List<object>;
                    if (listData[0].GetType() == typeof(SavedItemReference))
                    {
                        foreach (var ct in input as List<object>)
                        {
                            var ClashFromReference = doc.ResolveReference(ct as SavedItemReference) as ClashTest;
                            var clashes = doc.GetClash();
                            
                            clashes.TestsData.TestsCompactTest(ClashFromReference);

                            output.Add(ct);

                        }
                    }

                }
            }
            return output;
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