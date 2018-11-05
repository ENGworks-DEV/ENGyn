using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;
using System;

namespace ENGyn.Nodes.Comments
{
    public class GetComment : Node
    {
        public GetComment(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Viewpoint", typeof(object));
            AddOutputPortToNode("Comments", typeof(object));

            //Help 
            this.BottomComment.Text = "Gets Comments list associated with SavedItem: e.g. Clash or Viewpoint ";
            this.ShowHelpOnMouseOver = true;
           

        }


        public override void Calculate()
        {
            var viewpoint = InputPorts[0].Data;

          
            OutputPorts[0].Data = MainTools.RunFunction(getViewpointsComments, InputPorts);
        }

        public object getViewpointsComments(object viewpoint)
        {
            var output = new List<object>();
            if (viewpoint is Viewpoint ||
                viewpoint is SavedItem || 
                viewpoint is SavedViewpoint)
            {
                var v = viewpoint as SavedItem;
                foreach (var item in v.Comments)
                {
                    output.Add(item);
                }
                //add null to keep tree structure
                if (output.Count == 0)
                    output.Add(null);
            }
            return output;
        }

        public override Node Clone()
        {
            return new GetComment(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}