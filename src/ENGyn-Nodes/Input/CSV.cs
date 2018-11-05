using System.Windows.Controls;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows;
using System.Collections.Generic;

using System.IO;
using System.Text;

namespace ENGyn.Nodes.Input
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


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Export input list content as string to CSV file";
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
                    ExportCSV(path.ToString(), input);
                }

            }

        }

   
        public static void ExportCSV(string filePath, object data)
        {
            using (var writer = new StreamWriter(System.IO.Path.GetFullPath(filePath)))
            {

                //TODO cast object to list
                foreach (var line in (System.Collections.IList)data)
                {
                    int count = 0;
                    if (MainTools.IsList(line))
                    {
                        var t = line.GetType();
                        var l = (System.Collections.IList)line ;
                        foreach (var entry in l)
                        {
                           
                            writer.Write(MainTools.Quoted((entry ?? "").ToString().Replace("\n", string.Empty)));
                            if (++count < l.Count)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                    }
                    else {

                            writer.Write(MainTools.Quoted((line ?? "").ToString().Replace("\n", string.Empty)));
                            
                                writer.Write(",");

                        writer.WriteLine();
                    }
                    
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

    public class ReadCSV : Node
    {
        public ReadCSV(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Path", typeof(object));
            AddOutputPortToNode("Output", typeof(object));



            AddControlToNode(new Label() { Content = "Title", FontSize = 13, FontWeight = FontWeights.Bold });

            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Read content of file and format as CSV input";

        }


        public override void Calculate()
        {
            var path = InputPorts[0].Data;
            if ( path != null)
            {

                   OutputPorts[0].Data =  ReadCSVFile(path.ToString());

            }

        }


        public static object ReadCSVFile(string filePath)
        {
            using (var reader = new StreamReader(System.IO.Path.GetFullPath(filePath)))
            {
                var output = new List<List<string>>();

                while (!reader.EndOfStream)
                {
                    List<string> temp = new List<string>();
                    temp.AddRange(reader.ReadLine().Split(','));
                    output.Add(temp);
                }


                return output;
            }
        }


        public override Node Clone()
        {
            return new ReadCSV(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}