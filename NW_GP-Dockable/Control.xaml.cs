using System.Windows.Controls;
using TUM.CMS.VplControl.Core;
using TUM.CMS.VplControl.Utilities;
using System.Reflection;
using System.Linq;
using System;
using Autodesk.Navisworks.Api;
using System.Windows;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

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
            

            var dlls = Directory.GetFiles(assamblyLocation,"*.dll");
            foreach (var dllPath in dlls)
            {
                //Only load classes inherent from Node

                var assamb = Assembly.LoadFrom(Path.Combine(dllPath))
                    .GetTypes()
                    .Where(t => t != typeof(Node) &&
                                          typeof(Node).IsAssignableFrom(t));

                VplControl.ExternalNodeTypes.AddRange(assamb);
            }
            

            VplControl.NodeTypeMode = NodeTypeModes.All;



            foreach (var item in VplControl.ExternalNodeTypes)
            {
                var button = new Button() { Content = item.Name }; // Creating button
                button.Click += Add_Node; //Hooking up to event
                button.Width = 140;
                DockPanel.SetDock(button, Dock.Top);
                ButtonStack.Children.Add(button); //Adding to grid or other parent
                
            }


            runButton.Click += refresh;
            
        }

        private void Add_Node(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            
            var el = this.VplControl.ConnectorCollection;
            
            foreach (var item in this.VplControl.ExternalNodeTypes)
            {
                if (item.Name == button.Content.ToString())
                {
                    var node = (Node)Activator.CreateInstance(item, this.VplControl);

                    node.Left = 0;
                    node.Top = 0;

                    node.Show();
                }
                

 
            }

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

        private void FillButtoms(object sender, SelectedCellsChangedEventArgs e )
        {

        }


    }
}
