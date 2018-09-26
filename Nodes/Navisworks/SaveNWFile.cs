using Autodesk.Navisworks.Api;
using System;
using System.Windows;
using TUM.CMS.VplControl.Core;

namespace ENGyn.Nodes.Navisworks
{
    public class SaveNWFile : Node
    {
        public SaveNWFile(VplControl hostCanvas)
            : base(hostCanvas)
        {

            AddInputPortToNode("NW Document", typeof(object));
            AddInputPortToNode("Path", typeof(object));
            AddOutputPortToNode("NW Document", typeof(object));

           

        }

        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var path = InputPorts[1].Data;
            OutputPorts[0].Data = SaveAsNWF(input, path);

        }

        /// <summary>
        /// Save object Document to object Path
        /// </summary>
        /// <param name="document"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private object SaveAsNWF(object document, object path)
        {

            var filepath = path.ToString();
            Document doc = document as Document;

            if (doc != null && filepath != null
                && document.GetType() == typeof(Document)
                )
            {
                try
                {
                    doc.SaveFile(path.ToString());
                    return document;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Error", exc.Message);
                    return null;
                }

            }
            else
            { return null; }
        }

        public override Node Clone()
        {
            return new SaveNWFile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}