using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static ENGyn.Nodes.Appearance.AppearanceByProfile;

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
            AppearanceTools.GetPath();


            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            foreach (var item in AppearanceTools.selectionSetsConfs.SelectionSets.Selectionset)
            {

                TreeNode treeNode = new TreeNode();
                treeNode.Tag = item;
                treeNode.Text = item.Name;
                var colorString = item.color;
                Color color = new Color();

                ApplyColorToTree(item, treeNode);
                this.treeView1.Nodes.Add(treeNode);

            }
            foreach (var item in AppearanceTools.selectionSetsConfs.SelectionSets.Viewfolder)
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

        private void ApplyColorToTree(AppearanceTools.Selectionset item, TreeNode treeNode)
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
                if (obj.GetType() == typeof(AppearanceTools.Selectionset))
                {
                    var selectionset = obj as AppearanceTools.Selectionset;
                    node.Tag = selectionset;
                    node.Text = selectionset.Name;
                    ApplyColorToTree(selectionset, node);
                }
            }


        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            AppearanceTools.Selectionset ss = this.treeView1.SelectedNode.Tag as AppearanceTools.Selectionset;
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
            AppearanceTools.Selectionset ss = this.treeView1.SelectedNode.Tag as AppearanceTools.Selectionset;
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
                if (save.FileName != null && save.FileName != "")
                {
                    AppearanceTools.convertXMLtoConfiguration(save.FileName);
                    AppearanceTools.FilePath = save.FileName;
                }

            }

        }

        private void transparencySlider_Scroll(object sender, EventArgs e)
        {
            this.Transparency.Text = transparencySlider.Value.ToString();
           
           
            try
            {
                AppearanceTools.Selectionset ss = this.treeView1.SelectedNode.Tag as AppearanceTools.Selectionset;
                ss.transparency = transparencySlider.Value;
            }
            catch { }
        }

        private void Reset_Button_Click(object sender, EventArgs e)
        {
            transparencySlider.Value = 0;
            this.treeView1.SelectedNode.BackColor = new Color();
            AppearanceTools.Selectionset ss = this.treeView1.SelectedNode.Tag as AppearanceTools.Selectionset;
            ss.color = null;
            ss.transparency = -1;
        }
    }
    public class AppearanceTools
    {
        public static JsonSelectionSetsConfiguration selectionSetsConfs { get; set; }

        public static string FilePath { get; set; }

        public static void GetPath()
        {

            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.ShowDialog();
                if (open.FileName != null)
                {
                    FilePath = open.FileName;
                    ReadConfiguration(FilePath);
                }


            }
        }

        private static void ReadConfiguration(string path)
        {

            if (path != null)
            {
                try
                {
                    string st = System.IO.File.ReadAllText(path);


                    selectionSetsConfs = JsonConvert.DeserializeObject<JsonSelectionSetsConfiguration>(st);

                    if (selectionSetsConfs != null)
                    {

                    }
                }
                catch { }

            }
            var debug = selectionSetsConfs.Filename;

        }
        public static void convertXMLtoConfiguration(string path)
        {

            var jsonXML = JsonConvert.SerializeObject(selectionSetsConfs, Newtonsoft.Json.Formatting.Indented);


            File.WriteAllText(path, jsonXML);


        }
        /// <summary>
        /// Stores selection set configuration to apply into OverridePermanent methods
        /// </summary>
        public class Viewfolder : Selectionset
        {
            public List<Viewfolder> Viewfolders { get; set; }
            public List<Selectionset> Selectionset { get; set; }
            public string Name { get; set; }
            public string Guid { get; set; }
            public object color { get; set; }
            public object transparency { get; set; }
        }

        public class Selectionset
        {
            public string Name { get; set; }
            public string Guid { get; set; }
            public object color { get; set; }
            public object transparency { get; set; }
        }

        public class SelectionSets
        {
            public List<Viewfolder> Viewfolder { get; set; }
            public List<Selectionset> Selectionset { get; set; }
        }

        public class JsonSelectionSetsConfiguration
        {
            public SelectionSets SelectionSets { get; set; }
            public string Xsi { get; set; }
            public string NoNamespaceSchemaLocation { get; set; }
            public string Units { get; set; }
            public string Filename { get; set; }
            public string Filepath { get; set; }
        }
    }
}
