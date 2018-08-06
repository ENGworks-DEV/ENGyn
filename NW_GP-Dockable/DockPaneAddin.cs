using System;
using System.Linq;
using System.Windows;
using Autodesk.Navisworks.Api.Plugins;

namespace NW_GraphicPrograming
{
   [Plugin("NW_GraphicPrograming.NW_GP_Addin", "PRD",
      DisplayName = "NW_GraphicPrograming",
      ToolTip = "NW_GraphicPrograming")]
   public class NW_GP_Addin : AddInPlugin
   {
      public override int Execute(params string[] parameters)
      {
         if (Autodesk.Navisworks.Api.Application.IsAutomated)
         {
            throw new InvalidOperationException("Invalid when running using Automation");
         }
           
         //       //Find the plugin
            PluginRecord pr =
                   Autodesk.Navisworks.Api.Application.Plugins.FindPlugin("NW_GraphicPrograming.NW_GP_Dock.PRD");

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

         return 0;
      }
   }
}
