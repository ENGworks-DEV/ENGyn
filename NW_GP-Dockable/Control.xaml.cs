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
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections;

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

            //Loading nodes dlls located in Nodes folders
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


            //TODO change to dynamic 

            List<Expander> expanderList = new List<Expander>();
            

            StackPanel MainStack = new StackPanel();
            //Creating buttons
            foreach (var item in VplControl.ExternalNodeTypes.OrderBy(o => o.Name).ToList())
            {
                //if (item.GetType() ==typeof( Node))
                //{
                    var types = item.GetType();
                    
                    var namespaceN = types.GetProperty("Namespace").Name;
                    namespaceN = item.Namespace.Split('.').Last();
                    int index = expanderList.FindIndex(x => x.Header.ToString() == namespaceN);

                    if (index >= 0)
                    {
                        var button = new Button() { Content = item.Name, HorizontalContentAlignment = HorizontalAlignment.Left };
                        button.Click += Add_Node;

                        DockPanel.SetDock(button, Dock.Top);
                        var stack = expanderList[index].Content as StackPanel;
                        stack.Children.Add(button);

                    }
                    if (index < 0)
                    {
                        var button = new Button() { Content = item.Name, HorizontalContentAlignment = HorizontalAlignment.Left };
                        button.Click += Add_Node;

                        DockPanel.SetDock(button, Dock.Top);
                        StackPanel stack = new StackPanel();
                        stack.Children.Add(button);
                        Expander NavisExp = new Expander() { Header = namespaceN, Content = stack };
                        expanderList.Add(NavisExp);

                    }
                //}
                 

            }


            foreach (var item in expanderList.OrderBy(o => o.Header).ToList())
            {
                MainStack.Children.Add(item);
            }


            Menu.Content = MainStack;


            runButton.Click += refresh;
            
        }


        private void NewCommand(object sender, RoutedEventArgs e)
        {
            VplControl.NewFile();
        }

        private void OpenCommand(object sender, RoutedEventArgs e)
        {
            VplControl.OpenFile();
        }

        private void SaveCommand(object sender, RoutedEventArgs e)
        {
            VplControl.SaveFile();
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
            var nn = SortNodes.TSort(this.VplControl.NodeCollection as IEnumerable<Node>, n => NodeDep(n) );

            foreach (Node n in nn)
            {
                n.setToRun = true;
                try
                {
                    n.Calculate();
                }
                catch (Exception except)
                {
                    MessageBox.Show(n.GetType().ToString() + Environment.NewLine + except.Message);

                    n.HasError = true;

                }
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

        public static IEnumerable<Node> NodeDep(Node node)

        {
            var type = node.DependencyObjectType as IEnumerable<Node>;
            var outpt = new List<Node>();
            type = null ;
            foreach (var item in node.InputPorts)
            {
                foreach (var ii in item.ConnectedConnectors)
                {
                    outpt.Add(ii.StartPort.ParentNode);

                } 
            }
             
           
            return  outpt as IEnumerable<Node>;
        }
    }
    public static  class SortNodes
    {

        public static IEnumerable<T> TSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle = false)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies, throwOnCycle);

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);
                if (dependencies(item) != null)
                {
                    foreach (var dep in dependencies(item))
                        Visit(dep, visited, sorted, dependencies, throwOnCycle);

                    sorted.Add(item);
                }

            }
            else
            {
                if (throwOnCycle && !sorted.Contains(item))
                    throw new Exception("Cyclic dependency found");
            }
        }
    }
    
}
