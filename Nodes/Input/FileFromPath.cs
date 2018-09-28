using Autodesk.Navisworks.Api.Clash;
using System.IO;
using TUM.CMS.VplControl.Core;
using Path = System.IO.Path;



namespace ENGyn.Nodes.Input
{
    public class FileFromPath : Node
    {
        public FileFromPath(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("FilePath", typeof(object));
            AddOutputPortToNode("File", typeof(object));
            this.BottomComment.Text = "Creates File object from given file path.";
        }


        public override void Calculate()
        {
            var InpuValue = InputPorts[0].Data.ToString();
            if (InpuValue != null)
            {
                OutputPorts[0].Data = FileFromString(InpuValue);
            }
        }



        public override Node Clone()
        {
            return new FileFromPath(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }

        public static FileInfo FileFromString(string path)
        {
            return new FileInfo(path);
        }

    }

}