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
using System.ComponentModel;

namespace ENGyn
{

    public partial class MainWindow : UserControl
    {
        public List<Type> Nodes { get; private set; }
        public string DefaultNodesVersion { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            KeyDown += VplControl.VplControl_KeyDown;
            KeyUp += VplControl.VplControl_KeyUp;


            #region Load DLLs as Nodes
            Nodes = new List<Type>();
            LoadExternalNodesDlls();
            VplControl.NodeTypeMode = NodeTypeModes.All;
            #endregion

            //Nodes Menues
            List<Expander> expanderList = new List<Expander>();
            StackPanel MainStack = new StackPanel();
            CreateMenuFromNodes(expanderList);
            foreach (var item in expanderList.OrderBy(o => o.Header).ToList())
            {
                MainStack.Children.Add(item);
            }
            Menu.Content = MainStack;


            //Assign run to refresh action

            runButton.MouseDown += Refresh;
            //Add version to GUI

            this.Version.Content = " GUI Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.NodeVersion.Content = " Nodes Version: " + DefaultNodesVersion;

        }

        /// <summary>
        /// Create the expander content menu with loaded nodes
        /// </summary>
        /// <param name="expanderList"></param>
        private void CreateMenuFromNodes(List<Expander> expanderList)
        {
            foreach (var item in Nodes.OrderBy(o => o.Name).ToList())
            {

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

            }
        }

        /// <summary>
        /// Load internal nodes dlls and dlls located in specific folder inside plugins
        /// </summary>
        private void LoadExternalNodesDlls()
        {
            //Loading nodes in GUI if exists
            VplControl.ExternalNodeTypes.AddRange(
            Utilities.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "ENGyn.Nodes").ToList());

            //Loading nodes dlls located in Nodes folders
            var assamblyLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Nodes";



            var dlls = Directory.GetFiles(assamblyLocation, "*.dll");
            foreach (var dllPath in dlls)
            {
                //Only load classes inherent from Node

                var assamb = Assembly.LoadFrom(Path.Combine(dllPath))
                    .GetTypes()
                    .Where(t => t != typeof(Node) &&
                                          typeof(Node).IsAssignableFrom(t));
                if (assamb != null)
                {
                    DefaultNodesVersion = assamb.First().Assembly.GetName().Version.ToString();
                    VplControl.ExternalNodeTypes.AddRange(assamb);
                }

            }


            //Load TUM nodes
            Nodes.AddRange(Utilities
                .GetTypesInNamespace(Assembly.Load("TUM.CMS.VplControl")
                , "TUM.CMS.VplControl.Nodes")
                .Where(t => t != typeof(Node) && typeof(Node).IsAssignableFrom(t))
                .ToList());

            Nodes.AddRange(VplControl.ExternalNodeTypes);
        }

        #region GUI Commands

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

            foreach (var item in Nodes)
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
        private void Refresh(object sender, RoutedEventArgs e)
        {
            var nn = SortNodes.TSort(this.VplControl.NodeCollection as IEnumerable<Node>, n => NodeDependencyTree(n));


                
                int CurrentProgress = 0;
                int TotalProgress = nn.Count();
                Progress ProgressBar = Autodesk.Navisworks.Api.Application.BeginProgress("ENGyn", "Running nodes");
                for (int i = 0; i < nn.Count(); i++)
                {
                    using (Transaction tx = Autodesk.Navisworks.Api.Application.MainDocument.BeginTransaction(nn.ElementAt(i).Name))
                    {
                        if (ProgressBar.IsCanceled) break;
                        CurrentProgress++;

                        nn.ElementAt(i).setToRun = true;
                        try
                        {
                       
                         nn.ElementAt(i).Calculate(); 
                          
                       
                        }
                        catch (Exception except)
                        {
                            MessageBox.Show(nn.ElementAt(i).GetType().ToString() + Environment.NewLine + except.Message);

                            nn.ElementAt(i).HasError = true;
                        
                        }
                        nn.ElementAt(i).setToRun = false;
                    
                        tx.Commit();
                        tx.Dispose();
                        ProgressBar.Update((double)CurrentProgress / TotalProgress);
                    }
                

            }

            ProgressBar.EndSubOperation(true);

            Autodesk.Navisworks.Api.Application.EndProgress();
         

        }
        



        public void Error(  EventHandler<ProgressErrorReportingEventArgs> e)
        {
            var l = e.ToString() ;
            MessageBox.Show("Error");
        }
        #endregion

        /// <summary>
        /// Create a dependency tree for nodes
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<Node> NodeDependencyTree(Node node)
        {
            var type = node.DependencyObjectType as IEnumerable<Node>;
            var outpt = new List<Node>();
            type = null;
            foreach (var item in node.InputPorts)
            {
                foreach (var ii in item.ConnectedConnectors)
                {
                    outpt.Add(ii.StartPort.ParentNode);

                }
            }


            return outpt as IEnumerable<Node>;
        }
    }

    /// <summary>
    /// Sorting class
    /// </summary>
    public static class SortNodes
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
