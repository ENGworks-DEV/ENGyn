using System;
using System.Linq;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Windows;

namespace ENGyne
{
        [Plugin("ENGRibbon", "ENG", DisplayName = "ENGWorks")]
        [RibbonLayout("ENGyne.xaml")]
        [RibbonTab("ENGworks")]
        [Command("ID_Button_1", LargeIcon = "ENGyn.png", ToolTip = "Visual Programming", DisplayName = "ENGyne Alpha")]

        public class Addin : CommandHandlerPlugin
        {
            public override int ExecuteCommand(string name, params string[] parameters)
            {

            switch (name)
            {
                case "ID_Button_1":
                    //Find the plugin
                    {
                        PluginRecord pr =
                                 Autodesk.Navisworks.Api.Application.Plugins.FindPlugin("ENGyne.NW_GP_Dock.ENG");

                        if (pr != null && pr is DockPanePluginRecord && pr.IsEnabled)
                        {
                            //check if it needs loading
                            if (pr.LoadedPlugin == null)
                            {
                                pr.LoadPlugin();
                            }

                            DockPanePlugin dpp = pr.LoadedPlugin as DockPanePlugin;
                            if (dpp != null)
                            {
                                //switch the Visible flag
                                dpp.Visible = !dpp.Visible;
                            }
                        }

                    }
                    break;
            }

            return 0;
      }
   }
}
