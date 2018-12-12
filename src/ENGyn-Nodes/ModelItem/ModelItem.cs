using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TUM.CMS.VplControl.Core;
using Application = Autodesk.Navisworks.Api.Application;

namespace ENGyn.Nodes.ModelItems
{
    public class ModelItemGeometry : Node
    {
        public ModelItemGeometry(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddOutputPortToNode("ModelItem", typeof(object));
            AddInputPortToNode("Geometry", typeof(object));


            Calculate();


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Get all geometries in ModelItem";

        }

        public override void Calculate()
        {
            //Just a place holder. Gives the hability to connect the node to a lower stage in the execution tree
            var input = InputPorts[0].Data;

            OutputPorts[0].Data = MainTools.RunFunction( GetDocument,InputPorts);
        }


        public object GetDocument(object modelItem)
        {
            List<ModelGeometry> output = new List<ModelGeometry>();

            if (modelItem  is ModelItem)
            {
                var mI = modelItem as ModelItem;
                output.AddRange(GetChildrenGeometry(mI).ToList());
            }

            return output;


        }

        private IEnumerable<ModelGeometry>GetChildrenGeometry(ModelItem m)
        {
            foreach (var item in m.Children)
            {
                if (item.HasGeometry)
                {
                    yield return item.Geometry;
                }
                else
                {
                    GetChildrenGeometry(item);
                }
            }

        }

        public override Node Clone()
        {
            return new ModelItemGeometry(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}