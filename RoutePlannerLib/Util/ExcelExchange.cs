using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
{
    public class ExcelExchange
    {
        Application excel = new Application();

        public void WriteToFile(String fileName, City from, City to, List<Link> links)
        {
            var workbook = excel.Workbooks.Add();
            var workSheet = (Worksheet)workbook.Worksheets.get_Item(1);

            var columns = 4;

            var data = new object[links.Count + 1, columns];
            data[0, 0] = "From";
            data[0, 1] = "To";
            data[0, 2] = "Distance";
            data[0, 3] = "Transporte \n Mode";

            int pos = 1;
            foreach (var link in links)
            {
                data[pos, 0] = link.FromCity.Name;
                data[pos, 1] = link.ToCity.Name;
                data[pos, 2] = link.Distance;
                data[pos, 3] = link.TransportMode;
                pos++;
            }

            var startCell = (Range)workSheet.Cells[1, 1];
            var endCell = (Range)workSheet.Cells[links.Count + 1, columns];
            var writeRange = workSheet.Range[startCell, endCell];

            writeRange.Value2 = data;
            
            excel.DisplayAlerts = false;
            workSheet.SaveAs(fileName);
        }

    }

}
