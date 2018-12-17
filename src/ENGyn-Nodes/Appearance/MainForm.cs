using System;
using System.Drawing;
using System.Windows.Forms;


namespace ENGyn.Nodes.Appearance
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }


        private void Load_Click(object sender, EventArgs e)
        {
            


            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            foreach (var item in Tools.selectionSetsConfs.SelectionSets.Selectionset)
            {

                TreeNode treeNode = new TreeNode();
                treeNode.Tag = item;
                treeNode.Text = item.Name;
                var colorString = item.color;
                Color color = new Color();

                ApplyColorToTree(item, treeNode);
                this.treeView1.Nodes.Add(treeNode);

            }
            foreach (var item in Tools.selectionSetsConfs.SelectionSets.Viewfolder)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.Tag = item;
                treeNode.Text = item.Name;
                this.treeView1.Nodes.Add(treeNode);
                TreeNode children = new TreeNode();

                ApplyColorToTree(item, treeNode);

                treeNode.Nodes.Add(children);
                recursion(item, children);


            }
            treeView1.EndUpdate();
        }

        private void ApplyColorToTree(Selectionset item, TreeNode treeNode)
        {
            var colorString = item.color;
            Color color = new Color();

            if (colorString != null)
            {
                color = ColorTranslator.FromHtml(colorString.ToString());
            }
            else
            {
                color = ColorTranslator.FromHtml("#FFFFFF");
            }


            treeNode.BackColor = color;
        }

        private static void ApplyColorToTree(Viewfolder item, TreeNode treeNode)
        {
            var colorString = item.color;
            Color color = new Color();

            if (colorString != null)
            {
                color = ColorTranslator.FromHtml(colorString.ToString());
            }
            else
            {
                color = ColorTranslator.FromHtml("#FFFFFF");
            }


            treeNode.BackColor = color;
        }

        private void createColorRectangle(Color c)
        {
            Graphics g = CreateGraphics();
            SolidBrush selPen = new SolidBrush(c);
            g.FillRectangle(selPen, 374, 132, 130, 40); 

            g.Dispose();
        }

        private void recursion(object obj, TreeNode node)
        {
            if (obj != null)
            {

                if (obj.GetType() == typeof(Viewfolder))
                {
                    var folder = obj as Viewfolder;
                    node.Tag = folder;
                    node.Text = folder.Name;
                    ApplyColorToTree(folder, node);
                    foreach (var item in folder.Selectionset)
                    {
                        TreeNode children = new TreeNode();
                        node.Nodes.Add(children);
                        recursion(item, children);
                    }

                }
                if (obj.GetType() == typeof(Selectionset))
                {
                    var selectionset = obj as Selectionset;
                    node.Tag = selectionset;
                    node.Text = selectionset.Name;
                    ApplyColorToTree(selectionset, node);
                }
            }


        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            Selectionset ss = this.treeView1.SelectedNode.Tag as Selectionset;
            try
            {
                Color c = ColorTranslator.FromHtml(ss.color.ToString());
                createColorRectangle(c);
            }
            catch
            {
                Color c = ColorTranslator.FromHtml("#F0F0F0");
                createColorRectangle(c);

            }

            try
            {
                this.transparencySlider.Value = int.Parse(ss.transparency.ToString());
                this.Transparency.Text = transparencySlider.Value.ToString();
            }
            catch
            {
                this.transparencySlider.Value = 0;
                this.Transparency.Text = transparencySlider.Value.ToString();
            }
        }

        private void colorbutton_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            Selectionset ss = this.treeView1.SelectedNode.Tag as Selectionset;
            Color c = this.colorDialog1.Color;
            ss.color = ColorTranslator.ToHtml(c);
            createColorRectangle(c);

            var colorString = ss.color;
            Color color = new Color();
            if (colorString != null)
            { color = ColorTranslator.FromHtml(colorString.ToString()); }
            else
            { color = ColorTranslator.FromHtml("#FFFFFF"); }


            this.treeView1.SelectedNode.BackColor = color;

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.Filter = "Json (.json)|*.json";
                save.ShowDialog();
                if (save.FileName != null)
                { Tools.convertXMLtoConfiguration(save.FileName);
                    Tools.FilePath = save.FileName;
                }

            }

        }

        private void transparencySlider_Scroll(object sender, EventArgs e)
        {
            this.Transparency.Text = transparencySlider.Value.ToString();
           
           
            try
            {
                Selectionset ss = this.treeView1.SelectedNode.Tag as Selectionset;
                ss.transparency = transparencySlider.Value;
            }
            catch { }
        }
    }
}
