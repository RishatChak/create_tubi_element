using Aveva.Core.Database.Filters;
using Aveva.Core.Database;
using PipeCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aveva.Core.Utilities.CommandLine;
using Aveva.ApplicationFramework.Presentation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;


namespace COUPWELD.Other
{
    using static Aveva.C.Sharp.Training.Utilites;
    using Command = Aveva.Core.Utilities.CommandLine.Command;

    internal class ReportWeldCoup
    {
        static List<WeldItem> transformedList = new List<WeldItem>();

        public static void ReportWeldPosition(DbElement pipe, DataGridView dataGrid)
        {
            DbElementType bran = DbElementTypeInstance.BRANCH;                                          //Выбор только бранчей
            TypeFilter filtBran = new TypeFilter(bran);                                                 //Настройка фильтра
            DBElementCollection CollectBran = new DBElementCollection(pipe, filtBran);                  //Собираем в коллекцию
            var OutBran = CollectBran.Cast<DbElement>()
                       .Where(element => element.ElementType == DbElementTypeInstance.BRANCH).ToList(); //Получение из коллекции всех бранчей в трубе в цикле
            var collectBran = OutBran.Select(dbElement => new ItemForCheck(dbElement)).ToList();        //Подключаем класс ItemForCheck для работы с pml запросами

            var BRAN = collectBran                                                                      
            .GroupBy(
                element =>
                new
                {
                    element.Refno,
                    element.HrefPos,
                    element.TrefPos,
                })
                .Select(group => new
                {
                    refno = group.Key.Refno,
                    href = group.Key.HrefPos,
                    tref = group.Key.TrefPos,
                }).ToList();

            foreach (var item in BRAN)
            {
                DbElement trefBranch = null;
                DbElement HrefBranch = null;
                DbElement CEBranch = DbElement.GetElement(item.refno);

                //Проверка головы и хвоста бранча. Здесь проверяется weld'ы в начале или в конце бранча для отчёта о дубликатах
                if (item.tref != null)
                {
                    trefBranch = DbElement.GetElement(item.tref);
                    string TypeFirstMem = AttributeStringByName(trefBranch, "type");
                    if(TypeFirstMem == "BRAN")
                        CheckLastMem(trefBranch, CEBranch);
                }
                if (item.href != null)
                {
                    HrefBranch = DbElement.GetElement(item.href);
                    string typeLastMem = AttributeStringByName(HrefBranch, "type");
                    if(typeLastMem == "BRAN")
                        CheckFirstMem(HrefBranch, CEBranch);
                }

                DbElementType weld = DbElementTypeInstance.WELD;                                            //Выбор только WELD
                TypeFilter filtWeld = new TypeFilter(weld);                                                 //Настройка фильтра
                DBElementCollection CollectWeld = new DBElementCollection(CEBranch, filtWeld);              //Собираем в коллекцию
                var OutWeld = CollectWeld.Cast<DbElement>()
                           .Where(element => element.ElementType == DbElementTypeInstance.WELD).ToList();   //Получение из коллекции всех weld в трубе в цикле
                var collectWeld = OutWeld.Select(dbElement => new ItemForCheck(dbElement)).ToList();        //Подключаем класс ItemForCheck для работы с pml запросами

                var resultWeldList = collectWeld
                       .GroupBy(
                       element =>
                       new
                       {
                           element.Refno,
                           element.Position,
                           element.NameOwnBran,
                           element.NameOwnPipe,
                       })
                       .Select(group => new
                       {
                           refno = group.Key.Refno,
                           pos = group.Key.Position,
                           nameOwnBran = group.Key.NameOwnBran,
                           nameOwnPipe = group.Key.NameOwnPipe,
                       }).ToList();

                foreach (var items in resultWeldList)
                {
                    string joinText = ConvertPos(items.pos);

                    if (!transformedList.Any(w => w.refno == items.refno))
                        transformedList.Add(new WeldItem { refno = items.refno, pos = joinText, nameBran = items.nameOwnPipe, namePipe = items.nameOwnBran});
                }

            }

            var duplicatePosItems = transformedList                                                         //Удаление лишних одинаковых элементов из списка 
                .GroupBy(item => item.pos)
                .Where(posGroup => posGroup.Count() > 1)
                .SelectMany(posGroup => posGroup.GroupBy(item => item.refno)
                                                .Select(refnoGroup => refnoGroup.First()))
                .ToList();

            try
            {
                foreach (var item in duplicatePosItems)                                                     //Добавляем в таблицу элементы
                    dataGrid.Rows.Add(item.refno, item.pos, item.namePipe, item.nameBran);
            }
            catch (Exception ex){MessageBox.Show(ex.Message);}
            transformedList.Clear();
        }

        static void CheckLastMem(DbElement trefBran, DbElement ceBran)
        {
            string typeFirstMemTref = AttributeStringByName(trefBran, "type of first mem");
            string refFirstMem = AttributeStringByName(trefBran, "ref of first mem");
            string nameOwnBranTref = AttributeStringByName(trefBran, "name of bran");
            string nameOwnPipeTref = AttributeStringByName(trefBran, "name of pipe");
            string PosWeldTref = AttributeStringByName(trefBran, "pos of first mem");

            string typeLastMemCE = AttributeStringByName(ceBran, "type of last mem");
            string refLastMemCE = AttributeStringByName(ceBran, "ref of last mem");
            string nameOwnBeanCe = AttributeStringByName(ceBran, "name of bran");
            string nameOwnPipeCe = AttributeStringByName(ceBran, "name of pipe");
            string PosWeldCe = AttributeStringByName(ceBran, "pos of last mem");

            string posFirst = ConvertPos(PosWeldTref);
            string posLast = ConvertPos(PosWeldCe);

            if (typeFirstMemTref == "WELD" && typeLastMemCE == "WELD")
            {
                Command.CreateCommand($"/{ceBran}").RunInPdms();
                //Запрос на дистанцию WELD предыдущей трубы.
                Command com = Command.CreateCommand($"!!DISTWELD = !!pmlcommand(|const dist pl of last mem to pa of first mem of tref|)");
                com.Run();                                                                                  //Запуск запроса
                string message = "Ошибка: CreateWeldOneTubi";
                double dist = PMLFunction.ConvertToDouble(com.GetPMLVariableString($"DISTWELD"), message);
                Command.CreateCommand("!!DISTWELD.DELETE()").RunInPdms();

                if (dist < 99)
                {
                    transformedList.Add(new WeldItem { refno = refFirstMem, pos = posLast, nameBran = nameOwnBranTref, namePipe = nameOwnPipeTref });
                    transformedList.Add(new WeldItem { refno = refLastMemCE, pos = posLast, nameBran = nameOwnBeanCe, namePipe = nameOwnPipeCe });
                }
            }
        }

        static void CheckFirstMem(DbElement HrefBran, DbElement ceBran)
        {
            string typeLastMem = AttributeStringByName(HrefBran, "type of last mem");
            string refLastMem = AttributeStringByName(HrefBran, "ref of last mem");
            string nameOwnBranHref = AttributeStringByName(HrefBran, "name of bran");
            string nameOwnPipeHref = AttributeStringByName(HrefBran, "name of pipe");
            string PosWeldHref = AttributeStringByName(HrefBran, "pos of last mem");

            string typeFirstMem = AttributeStringByName(ceBran, "type of first mem");
            string refFirstMem = AttributeStringByName(ceBran, "ref of first mem");
            string nameOwnBeanCe = AttributeStringByName(ceBran, "name of bran");
            string nameOwnPipeCe = AttributeStringByName(ceBran, "name of pipe");
            string PosWeldCe = AttributeStringByName(ceBran, "pos of first mem");

            string posFirst = ConvertPos(PosWeldCe);
            string posLast = ConvertPos(PosWeldHref);

            if (typeLastMem == "WELD" && typeFirstMem == "WELD")
            {
                Command.CreateCommand($"/{ceBran}").RunInPdms();
                //Запрос на дистанцию WELD следующей трубы.
                Command com = Command.CreateCommand($"!!DISTWELD = !!pmlcommand(|const dist pa of first mem to pl of last mem of href|)");
                com.Run();                                                                                  //Запуск запроса
                string message = "Ошибка: CheckFirstMem";
                double dist = PMLFunction.ConvertToDouble(com.GetPMLVariableString($"DISTWELD"), message);
                Command.CreateCommand("!!DISTWELD.DELETE()").RunInPdms();

                if (dist < 99)
                {
                    transformedList.Add(new WeldItem { refno = refLastMem, pos = posLast, nameBran = nameOwnBranHref, namePipe = nameOwnPipeHref });
                    transformedList.Add(new WeldItem { refno = refFirstMem, pos = posLast, nameBran = nameOwnBeanCe, namePipe = nameOwnPipeCe });
                }
            }
        }

        public static void CheckLenght(DbElement CEBranch, DataGridView dataGrid)
        {
            DbElementType tibi = DbElementTypeInstance.TUBING;                                          //Выбор только TUBING
            TypeFilter filtTubi = new TypeFilter(tibi);                                                 //Настройка фильтра
            DBElementCollection CollectTubi = new DBElementCollection(CEBranch, filtTubi);              //Собираем в коллекцию
            var OutTubi = CollectTubi.Cast<DbElement>()
                       .Where(element => element.ElementType == DbElementTypeInstance.TUBING).ToList(); //Получение из коллекции всех TUBING в трубе в цикле
            var collectTubi = OutTubi.Select(dbElement => new ItemForCheck(dbElement)).ToList();        //Подключаем класс ItemForCheck для работы с pml запросами

            var resultTubiSlist = collectTubi
                   .GroupBy(
                   element =>
                   new
                   {
                       element.Refno,
                       element.PStLenghth,
                       element.NameOwnBran,
                       element.NameOwnPipe,
                       element.Itlength,
                   })
                   .Select(group => new
                   {
                       refno = group.Key.Refno,
                       pstLenghth = group.Key.PStLenghth,
                       nameOwnPipe = group.Key.NameOwnBran,
                       nameOwnBran = group.Key.NameOwnPipe,
                       ltlength = group.Key.Itlength,
                   }).ToList();

            
            try
            {
                foreach (var t in resultTubiSlist)
                {
                    string message = "Ошибка: CheckLenght";
                    double lenTubi = PMLFunction.ConvertToDouble(t.ltlength, message);       //Получение длинны трубы
                    double cataLenght = PMLFunction.ConvertToDouble(t.pstLenghth, message);  //Получение каталожной длинны

                    if(lenTubi > cataLenght)                                        //Если длинна трубы длиннее чем каталожная, добавляем в таблицу
                        dataGrid.Rows.Add(t.refno, t.ltlength, t.nameOwnBran, t.nameOwnPipe);
                }
            }catch (Exception){}
        }
        

        private static string ConvertPos(string position)                           //Получение позиции weld. Округление и удаление лишних элементов, для сравнения
        {
            string formattedPos1 = position.Replace("E", "").Replace("S", "").Replace("U", "").Replace("W", "").Replace("N", "").Replace("D", "").Replace("mm", "");
            string[] arrayPos = formattedPos1.Split(' ');
            string joinText = "";
            string newString = "";
            foreach (var coorPos in arrayPos)
            {
                if (coorPos.Contains("."))
                {
                    int dotIndex = coorPos.IndexOf(".");
                    if (dotIndex >= 0)
                        newString = coorPos.Substring(0, dotIndex);  // Обрезаем строку до точки
                }
                else
                    newString = coorPos;
                joinText += newString + " ";
            }
            return joinText;
        }

        public static void ExlReportDouble(DataGridView dataGridView, string DoubLen)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog                      // Создание диалогового окна "Сохранить как"
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Save Excel File"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;             // Отображение диалогового окна и простановка флага в случае выбора пути

            // Создание нового приложения Excel (только если пользователь выбрал файл для сохранения)
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            // Добавление новой рабочей книги
            Workbook workbook = excelApp.Workbooks.Add();
            // Получение активного листа
            Worksheet worksheet = (Worksheet)workbook.ActiveSheet;
            // Установка значений в шапку
            worksheet.Cells[1, 1] = "Refno";
            worksheet.Cells[1, 2] = DoubLen;
            worksheet.Cells[1, 3] = "Name Bran";
            worksheet.Cells[1, 4] = "Name Pipe";
            // Оформление шапки
            Range headerRange = worksheet.Range["A1", "D1"];
            headerRange.Font.Bold = true;
            headerRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            // Заполнение информации по столбцам
            int refnoColumnIndex = 0;

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    var cellValue = dataGridView.Rows[i].Cells[j].Value?.ToString();
                    if (j == refnoColumnIndex && cellValue != null)
                        cellValue = "\"" + cellValue + "\"";

                    worksheet.Cells[i + 2, j + 1] = cellValue;
                }
            }
            // Автоматическое увеличение размера ячеек
            worksheet.Cells.EntireColumn.AutoFit();

            try
            {
                // Сохранение файла в выбранной пользователем директории
                workbook.SaveAs(saveFileDialog.FileName, XlFileFormat.xlOpenXMLWorkbook);
                var result = MessageBox.Show("Файл успешно сохранён! Открыть файл?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
            catch (Exception ex){MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);}
            finally
            {
                // Очистка ресурсов Excel
                Marshal.ReleaseComObject(worksheet);
                workbook.Close(false);
                Marshal.ReleaseComObject(workbook);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
