using System.Windows.Controls;
using TUM.CMS.VplControl.Core;
using TUM.CMS.VplControl.Utilities;
using System.Reflection;
using System.Linq;
using System;
using Autodesk.Navisworks.Api;
using System.Windows;


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
            VplControl.NodeTypeMode = NodeTypeModes.All;

            //foreach (var item in VplControl.ExternalNodeTypes)
            //{
            //    NodesList.Items.Add(item.Name);
            //}

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
