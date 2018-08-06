using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms.Integration;
using Autodesk.Navisworks.Api.Plugins;
using System.Linq;

namespace NW_GraphicPrograming
{
   #region WPFDocPanePlugin

   [Plugin("NW_GraphicPrograming.NW_GP_Dock", "PRD", DisplayName = "NW_GraphicPrograming", ToolTip = "NW_GraphicPrograming")]
   [DockPanePlugin(150, 200, FixedSize=false)]
   class NW_GP_Dock : DockPanePlugin
   {
        Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
        public override System.Windows.Forms.Control CreateControlPane()
      {

            ResourceDictionary myResourceDictionary = new ResourceDictionary();

            myResourceDictionary.Source = new Uri("/TUM.CMS.VplControl;component/Themes/Generic.xaml", UriKind.Relative);

            //System.Windows.Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
            Autodesk.Navisworks.Gui.Common.View.WPFApp.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
            // Autodesk.Navisworks.Gui.Common.View.WPFApp.ResourceAssembly = GetAssemblyByName("TUM.CMS.VplControl") ;
            //create an ElementHost
            ElementHost eh = new ElementHost();


            //assign the control
            eh.AutoSize = true;
            eh.Child = new MainWindow();


         eh.CreateControl();

         //return the ElementHost
         return eh;
      }

      public override void DestroyControlPane(System.Windows.Forms.Control pane)
      {
            
         pane.Dispose();
      }
   }
   #endregion
}
