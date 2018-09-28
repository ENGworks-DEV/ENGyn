using System.Collections.Generic;
using TUM.CMS.VplControl.Core;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System;

namespace ENGyn.Nodes.Excel
{
    public class ReadFromFile : Node
    {
        public ReadFromFile(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("FilePath", typeof(object));
            AddOutputPortToNode("Data", typeof(object));

            this.BottomComment.Text = "Read Excel data. The node takes all " +
                "used cells in the spreadsheet from the first worksheet " +
                "available";
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


        public List<List<string>> ExcelInfo (string path)
        {

            List<string> output = new List<string>();

            var excelApp = new Microsoft.Office.Interop.Excel.Application();

            //Dont show Excel when open
            excelApp.Visible = false;

            Microsoft.Office.Interop.Excel.Workbooks books = excelApp.Workbooks;

            //Open Excel file by Path provided by the user
            Microsoft.Office.Interop.Excel.Workbook sheet = books.Open(path);

            //Select worksheet by the name provided by the user
            //Microsoft.Office.Interop.Excel.Worksheet indsheet = sheet.Sheets[worksheet];
            Microsoft.Office.Interop.Excel.Worksheet indsheet = sheet.Worksheets[1];

            //Microsoft.Office.Interop.Excel.Range range = indsheet.get_Range(CellStart, CellEnd);

            //Get the used cell range in Excel
            Microsoft.Office.Interop.Excel.Range range = indsheet.UsedRange;


            object[,] cellValues = (object[,])range.Value2;
            output = cellValues.Cast<object>().ToList().ConvertAll(x => Convert.ToString(x));

            //Get number of used columns
            int columnCount = range.Columns.Count;

            //Split list by column count
            List<List<string>> list = new List<List<string>>();
            for (int i = 0; i < output.Count; i += columnCount)
            { 
                list.Add(output.GetRange(i, Math.Min(columnCount, output.Count - i)));
            }
            

            sheet.Close(true);
            excelApp.Quit();


            return list;

        }

    }
}
    



