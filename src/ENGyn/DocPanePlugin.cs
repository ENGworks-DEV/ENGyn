using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms.Integration;
using Autodesk.Navisworks.Api.Plugins;
using System.Linq;

//TODO: https://www.automatetheplanet.com/specify-assembly-references-based-build-configuration-visual-studio/

namespace ENGyn
{
    #region WPFDocPanePlugin

    [Plugin("ENGyn.NW_GP_Dock", "ENG", DisplayName = "ENGyn", ToolTip = "Visual GraphicPrograming")]
    [DockPanePlugin(500, 500, FixedSize = false)]

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

            ////assign the control
            eh.AutoSize = true;
            eh.Child = new MainWindow();

            eh.CreateControl();
            eh.BringToFront();
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
