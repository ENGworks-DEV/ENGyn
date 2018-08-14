using System.Windows.Controls;
using TUM.CMS.VplControl.Core;
using TUM.CMS.VplControl.Utilities;
using System.Reflection;
using System.Linq;
using System;
using Autodesk.Navisworks.Api;
using System.Windows;
using System.IO;

namespace NW_GraphicPrograming
{

    public partial class MainWindow : UserControl
   {

        //Dcoument property
        public static Document docControl { get; set; }

        public MainWindow()
      {
         InitializeComponent();

            docControl = Autodesk.Navisworks.Api.Application.ActiveDocument;

            KeyDown += VplControl.VplControl_KeyDown;
            KeyUp += VplControl.VplControl_KeyUp;
            // Load a theme and set it as current.
            


            VplControl.ExternalNodeTypes.AddRange(
            Utilities.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "NW_GraphicPrograming.Nodes").ToList());

            //Loading nodes dlls located in Nodes folders
            var assamblyLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Nodes" ;
            

            var test = Directory.GetFiles(assamblyLocation,"*.dll");
            foreach (var dllPath in Directory.GetFiles(assamblyLocation))
            {
                var assamb = Assembly.LoadFrom(Path.Combine(dllPath));

                VplControl.ExternalNodeTypes.AddRange(assamb.GetTypes());
            }
            

            VplControl.NodeTypeMode = NodeTypeModes.All;



            runButton.Click += refresh;
            
        }


        //Ugly way to trigger calculate
        private void refresh(object sender, RoutedEventArgs e)
        {
            foreach (Node n in this.VplControl.NodeCollection)
            { n.setToRun = true;
                n.Calculate();
                n.setToRun = false;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NodesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var l = this;

           
        }
    }
}
