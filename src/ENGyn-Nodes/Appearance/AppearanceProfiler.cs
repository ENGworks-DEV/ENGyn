using System.Windows.Controls;
using System.Xml;
using ColorTranslator = System.Drawing.ColorTranslator;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using TUM.CMS.VplControl.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using ENGyn.XML;
using System.IO;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Interop;

namespace ENGyn.Nodes.Appearance
{
    public class AppearanceByProfile : Node
    {

        #region Node class methods
        public AppearanceByProfile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddOutputPortToNode("Profile Path", typeof(string));
            System.Windows.Controls.Button button = new System.Windows.Controls.Button { Content = "Open Profiler", Width = 120 };
            button.Click += show_click;
            AddControlToNode(button);
            //Help 
            this.BottomComment.Text = "Opens ENGyn appearence profiler dialog";
            this.ShowHelpOnMouseOver = true;

        }


        public void show_click(object sender, EventArgs e)
        {

            MainForm main = new MainForm();
            main.ShowDialog();
        }

        public override void Calculate()
        {


            
            OutputPorts[0].Data =null;

        }



        public override Node Clone()
        {
            return new AppearanceByProfile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
        #endregion



      

    }

}