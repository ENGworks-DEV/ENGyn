using System.Collections.Generic;
using TUM.CMS.VplControl.Core;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System;

namespace ENGyn.Nodes.Excel
{
    public class FileWorkSheets : Node
    {
        public FileWorkSheets(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("FilePath", typeof(object));
            AddOutputPortToNode("Data", typeof(object));

            this.BottomComment.Text = "Get all the worksheets as a list " +
                "of strings";
        }


        public override void Calculate()
        {
            OutputPorts[0].Data = ExcelInfo(InputPorts[0].Data.ToString());
        }


        public override Node Clone()
        {
            return new ReadFromFile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }


        public List<string> ExcelInfo(string path)
        {

            List<string> output = new List<string>();
            try { 
            var excelApp = new Microsoft.Office.Interop.Excel.Application();

            //Dont show Excel when open
            excelApp.Visible = false;

            Microsoft.Office.Interop.Excel.Workbooks books = excelApp.Workbooks;

            //Open Excel file by Path provided by the user
            Microsoft.Office.Interop.Excel.Workbook sheet = books.Open(path);


           
            foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in sheet.Worksheets)
            {
                output.Add(worksheet.Name.ToString());
            }
            // accessing the desired worksheet in the dictionary
            


            sheet.Close(true);
            excelApp.Quit();

            }
            catch 
            {
                //To be implemented
            }
            return output;

        }

    }
}




