using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Selection
{
    public class GetElementFromSearch : Node
    {
        public GetElementFromSearch(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("SelectionSets", typeof(object));
            AddOutputPortToNode("Output", typeof(object));

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Returns elements in SearchSet";
        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var output = new List<ModelItemCollection>();

            if (input != null)
            {

                var type = input.GetType();

                if (type == typeof(SelectionSet))
                {
                    var selectionSet = input as SelectionSet;

                    ModelItemCollection searchResults =
                    selectionSet.Search.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
                    output.Add(searchResults);

                }

                if (MainTools.IsList(input) && MainTools.ListContainsType(input, typeof(SelectionSet)))
                {
                    foreach (var item in input as List<SelectionSet>)
                    {
                        var selectionSet = item as SelectionSet;

                        ModelItemCollection searchResults =
                        selectionSet.Search.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
                        output.Add(searchResults);
                    }
                }

            }
            var objects = new List<object>();
            foreach (var item in output)
            {
                List<ModelItem> modelitems = new List<ModelItem>();
                foreach (var model in item)
                {
                    modelitems.Add(model);
                }
                objects.Add(modelitems);
            }
            OutputPorts[0].Data = objects;
        }




        public override Node Clone()
        {
            return new GetElementFromSearch(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}