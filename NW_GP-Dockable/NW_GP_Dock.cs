//------------------------------------------------------------------
// NavisWorks Sample code
//------------------------------------------------------------------

// (C) Copyright 2010 by Autodesk Inc.

// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//------------------------------------------------------------------
//
// This sample illustrates a basic Hello world message displayed in
// a dockable pane.
//
//------------------------------------------------------------------
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
           



           
         //try {
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
            //}
            //catch
            //{
            //    MessageBox.Show("Error");

            //}
         return 0;
      }
   }
}
