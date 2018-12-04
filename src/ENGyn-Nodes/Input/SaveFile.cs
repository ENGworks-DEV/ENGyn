using System.Windows;
using System.Windows.Forms;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Input
{
    public class SaveFile : Node
    {
        public SaveFile(VplControl hostCanvas)
            : base(hostCanvas)
        {

            var saveButton = new System.Windows.Controls.Button { Name = "SaveFile", Content = "SaveFile", Width = 120 };
            saveButton.Click += SaveFile_Click;
            AddControlToNode(saveButton);
            AddOutputPortToNode("Output", typeof(object));


            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Save file dialog. Select where to save a file";
        }


        public override void Calculate()
        {

            OutputPorts[0].Data = filePath;

        }

        string filePath;

        public void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")

                {

                    filePath = saveFileDialog.FileName;
                }


            }

        }

        public override Node Clone()
        {
            return new SaveFile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}