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
