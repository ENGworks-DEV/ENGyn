using System.Collections.Generic;
using TUM.CMS.VplControl.Core;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System;

namespace ENGyn.Nodes.Excel
{
    public class WriteToFile : Node
    {
        public WriteToFile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("FilePath", typeof(object));
            AddInputPortToNode("SheetName", typeof(string));
            AddInputPortToNode("Column", typeof(int));
            AddInputPortToNode("Row", typeof(int));
            AddInputPortToNode("List<String>", typeof(List<object>));
            AddOutputPortToNode("Data", typeof(object));

            this.BottomComment.Text = "Write Excel data. The node takes a " +
                "List<string> and writes each element in a different cell";
        }


        public override void Calculate()
        {
            var input = InputPorts[4].Data as List<object>;
            ExcelWrite(InputPorts[0].Data.ToString(), InputPorts[1].Data.ToString(), Int32.Parse(InputPorts[2].Data.ToString()), Int32.Parse(InputPorts[3].Data.ToString()), input);
            OutputPorts[0].Data = InputPorts[4].Data;
        }

        public override Node Clone()
        {
            return new ReadFromFile(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }


        public void ExcelWrite(string path, string worksheet, int Row, int Column, List<object> Values)
        {

            try
            {

                List<string> output = new List<string>();

                var excelApp = new Microsoft.Office.Interop.Excel.Application();

                //Dont show Excel when open
                excelApp.Visible = false;

                Microsoft.Office.Interop.Excel.Workbooks books = excelApp.Workbooks;

                //Open Excel file by Path provided by the user
                Microsoft.Office.Interop.Excel.Workbook sheet = books.Open(path);

                //Select worksheet by the name provided by the user
                Microsoft.Office.Interop.Excel.Worksheet indsheet = sheet.Sheets[worksheet];

                Microsoft.Office.Interop.Excel.Range activecell = indsheet.Cells[Row, Column];

                for (int i = 0; i < Values.Count; i += 1)
                {
                    indsheet.Cells[Row + i, Column] = Values[i].ToString();
                }

                sheet.Close(true);
                excelApp.Quit();
            }

            catch
            { }

        }

    }
}




