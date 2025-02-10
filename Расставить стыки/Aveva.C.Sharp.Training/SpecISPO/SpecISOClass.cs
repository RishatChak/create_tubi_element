using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Aveva.Core.Database;
using Aveva.Core.Database.Filters;
using PipeCheck;
using Aveva.Core.PMLNet;
using Aveva.C.Sharp.Training;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace COUPWELD.SpecISPO
{
    [PMLNetCallable()]
    public class SpecISOClass
    {
        [PMLNetCallable()]
        public SpecISOClass()
        {

        }
        [PMLNetCallable()]
        public void Assign(SpecISOClass that)
        {

        }

        [PMLNetCallable()]
        public void ReportSpecISO(string pathFile, string revision, string template, string savePath)
        {
            try
            {
                if (File.Exists(savePath))
                    File.Delete(savePath);

                List<string> lines = new List<string>();
                using (System.IO.StreamReader file = new System.IO.StreamReader(pathFile))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Open(template);
                Excel.Worksheet worksheet = workbook.Sheets[1]; // выбор первого листа

                worksheet.Cells[1, 1].Value = "Номер документа\nDocument number"; // A1
                worksheet.Cells[1, 2].Value = "Наименование документа\nDocument title"; // B1
                worksheet.Cells[1, 3].Value = "Редакция\nRevision"; // C1

                Excel.Range headerRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]];
                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Cells[1, 3].EntireColumn.AutoFit();
                worksheet.Columns[3].ColumnWidth = 10;
                worksheet.Columns[2].ColumnWidth = 50;
                int rowIndex = 2;

                foreach (var item in lines)
                {
                    DbElement pipe = DbElement.GetElement(item);
                    DBElementCollection collectPipe = new DBElementCollection(pipe);
                    var outPipe = collectPipe.Cast<DbElement>()
                        .Where(dbElement => dbElement.ElementType == DbElementTypeInstance.PIPE).ToList();
                    var pipeList = outPipe.Select(dbElement => new ItemForCheck(dbElement)).ToList();

                    var result = pipeList
                        .GroupBy(element => new
                        {
                            element.NamnForISO,
                            element.Lineshifr,
                        })
                        .Select(group => new
                        {
                            Name = group.Key.NamnForISO,
                            Lineshifr = group.Key.Lineshifr,
                        }).ToList();

                    foreach (var items in result)
                    {
                        worksheet.Cells[rowIndex, 1].Value = items.Lineshifr; // B1, B2, B3
                        worksheet.Cells[rowIndex, 2].Value = "Isometric drawing " + items.Name + "\nИзометрический чертеж " + items.Name; // A1, A2, A3
                        worksheet.Cells[rowIndex, 3].Value = revision; // C1, C2, C3

                        worksheet.Cells[rowIndex, 1].EntireColumn.AutoFit();
                        worksheet.Cells[rowIndex, 2].EntireColumn.AutoFit();
                        //worksheet.Cells[rowIndex, 3].EntireColumn.AutoFit();
                        Excel.Range lineExcel = worksheet.Range[worksheet.Cells[rowIndex, 1], worksheet.Cells[rowIndex, 3]];
                        lineExcel.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        rowIndex++;
                    }
                }

                workbook.SaveAs(savePath);
                workbook.Close();
                excelApp.Quit();

                DialogResult dialogResult = MessageBox.Show("Открыть файл?", "Message", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    System.Diagnostics.Process.Start(savePath);
                else if (dialogResult == DialogResult.No) { }
            }catch (Exception ex){MessageBox.Show(ex.Message);}
        }
    }
}