using TUM.CMS.VplControl.Core;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System;

namespace ENGyn.Nodes.Input
{
    public class WriteTextToPath : Node
    {
        public List<string> lines { get; private set; } 

        public WriteTextToPath(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Path", typeof(object));
            AddInputPortToNode("Text", typeof(object));

        }

        public void recursion(object obj)
        {
   
            if (MainTools.IsList(obj))
            {
                foreach (var item in (System.Collections.IEnumerable)obj)
                {
                    var t = item.GetType();
                    if (MainTools.IsList(item))
                    { recursion(item); }
                    else
                    {
                        lines.Add(item.ToString());
                    } 
                }
            }
            else
            {
                lines.Add(obj.ToString());
            }
        }
        public override void Calculate()
        {
            if (InputPorts[0].Data != null && InputPorts[1].Data != null)
            {
                string e = Path.GetExtension((string)InputPorts[0].Data);

                    try
                    {

                    lines = new List<string>();
                    recursion(InputPorts[1].Data);

                    if (lines.Count > 0 )
                    { File.WriteAllLines((string)InputPorts[0].Data, lines); }
                    




                }
                    catch (Exception ex)
                    {

                    MessageBox.Show(ex.Message, "Error writing");

                }

            }
            

        }


 
        public override Node Clone()
        {
            return new WriteTextToPath(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}