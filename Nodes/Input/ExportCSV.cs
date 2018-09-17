using System.Windows.Controls;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows;
using System.Collections.Generic;

using System.IO;
using System.Text;

namespace ENGyn.Nodes
{
    public class ExportAsCSV : Node
    {
        public ExportAsCSV(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            AddInputPortToNode("Path", typeof(object));
            AddOutputPortToNode("Output", typeof(ClashResult));



            AddControlToNode(new Label() { Content = "Title", FontSize = 13, FontWeight = FontWeights.Bold });


        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var path = InputPorts[1].Data;
            if (input != null && path !=null)
            {
                if (MainTools.IsList(input))
                {
                    var t = input.GetType();
                    ExportCSV(path.ToString(), input as object[][]);
                }

            }

        }

        public static void ExportCSV(string filePath, object[][] data)
        {
            using (var writer = new StreamWriter(System.IO.Path.GetFullPath(filePath)))
            {
                foreach (var line in data)
                {
                    int count = 0;
                    foreach (var entry in line)
                    {
                        writer.Write(entry);
                        if (++count < line.Length)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }


        public override Node Clone()
        {
            return new ExportAsCSV(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}