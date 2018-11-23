using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using Autodesk.Navisworks.Api.Clash;
using System;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms;
using System.Threading;
using Autodesk.Navisworks.Api.Automation;

namespace ENGyn.Nodes.Navisworks
{
    public class CloseNavisworks : Node
    {
        public CloseNavisworks(VplControl hostCanvas)
            : base(hostCanvas)
        {
 
            AddOutputPortToNode("NW Document", typeof(object));
            AddInputPortToNode("NW Object", typeof(object));
            //Help
            this.ShowHelpOnMouseOver = true;
            this.BottomComment.Text = "Get current Navisworks document";

        }

        public override void Calculate()
        {
            //Just a place holder. Gives the hability to connect the node to a lower stage in the execution tree
            var input = InputPorts[0].Data;
            CloseDocProc();
    
        }



        static void CloseDocProc()
        {
            try

            {

                // System.Windows.Application.Current.Shutdown();
                var handle = Autodesk.Navisworks.Api.Application.Gui.MainWindow.Handle;

                var p  = Autodesk.Navisworks.Api.ComApi.ComApiBridge.State.nwProcess;
                

            }
            catch { }
        }

        public override Node Clone()
        {
            return new CloseNavisworks(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        
        }
    }

    public class FileProtocolHandle1 : FileProtocolHandle
    {
        public override bool Close()
        {
            return false;
        }

        public override bool Read(byte[] data, ulong offset, ulong count)
        {
            return false;
        }

        public override bool Write(byte[] data, ulong offset, ulong count)
        {
            return false;
        }
    }

}