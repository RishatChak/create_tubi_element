using Aveva.Core.Database;
using Aveva.Core.Database.Filters;
using Aveva.Core.Geometry;
using Aveva.Core.PMLNet;
using Aveva.Core.Presentation;
using Aveva.Core.Utilities.CommandLine;
using COUPWELD;
using COUPWELD.Other;
using PipeCheck;
using PipeReportLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aveva.C.Sharp.Training
{
    using static Aveva.C.Sharp.Training.Utilites;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    [PMLNetCallable()]
    public partial class AddSelectPipe : UserControl
    {
        private List<DbElement> elList = new List<DbElement>();

        [Obsolete]
        public AddSelectPipe()
        {
            InitializeComponent();
            InitializeDataGrid();
            SetStandartFunction();
            InitializeToolTip();
        }

        private void InitializeDataGrid()
        {
            // Очищаем существующие столбцы (если есть)
            doubleWeldDataGridView.Columns.Clear();
            lengthWeldDataGridView.Columns.Clear();

            // Создаем и добавляем столбцы для doubleWeldDataGridView
            AddColumnToDataGridView(doubleWeldDataGridView, "Refno", "Refno");
            AddColumnToDataGridView(doubleWeldDataGridView, "Pos", "Pos");
            AddColumnToDataGridView(doubleWeldDataGridView, "Bran", "Bran");
            AddColumnToDataGridView(doubleWeldDataGridView, "Pipe", "Pipe");

            // Создаем и добавляем столбцы для lengthWeldDataGridView
            AddColumnToDataGridView(lengthWeldDataGridView, "Refno", "Refno");
            AddColumnToDataGridView(lengthWeldDataGridView, "ltlen", "ltlen");
            AddColumnToDataGridView(lengthWeldDataGridView, "Bran", "Bran");
            AddColumnToDataGridView(lengthWeldDataGridView, "Pipe", "Pipe");

            // Добавляем контекстное меню к doubleWeldDataGridView
            InitializeContextMenuForDataGridView(doubleWeldDataGridView);
            InitializeContextMenuForDataGridView(lengthWeldDataGridView);

            // Общая функция для добавления столбцов в DataGridView с режимом автоподгонки
            void AddColumnToDataGridView(DataGridView dataGridView, string headerText, string name)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = headerText;
                column.Name = name;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns.Add(column);
            }

            // Добавляем контекстное меню к DataGridView
            void InitializeContextMenuForDataGridView(DataGridView dataGridView)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripMenuItem deleteRowMenuItem = new ToolStripMenuItem("Удалить строку", null, (sender, e) => DeleteSelectedRow(dataGridView));
                contextMenu.Items.Add(deleteRowMenuItem);
                ToolStripMenuItem navigateToItemMenuItem = new ToolStripMenuItem("Перейти к элементу", null, (sender, e) => NavigateToSelectedItem(dataGridView));
                contextMenu.Items.Add(navigateToItemMenuItem);
                ToolStripMenuItem deleteAllRows = new ToolStripMenuItem("Очистить список", null, (sender, e) => DeleteSelectedRows(dataGridView));
                contextMenu.Items.Add(deleteAllRows);
                dataGridView.ContextMenuStrip = contextMenu;
                dataGridView.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        var hti = dataGridView.HitTest(e.X, e.Y);
                        dataGridView.ClearSelection();
                        if (hti.RowIndex >= 0)
                            dataGridView.Rows[hti.RowIndex].Selected = true;
                    }
                };
            }

            // Функция для удаления выбранной строки.
            void DeleteSelectedRow(DataGridView dataGridView){try{dataGridView.Rows.RemoveAt(dataGridView.SelectedRows[0].Index);} catch (Exception){}}

            void DeleteSelectedRows(DataGridView dataGridView){dataGridView.Rows.Clear();}

            // Функция для перехода к выбранному элементу.
            void NavigateToSelectedItem(DataGridView dataGridView)
            {
                try
                {
                    string refno = dataGridView.SelectedRows[0].Cells["Refno"].Value.ToString();
                    Command.CreateCommand($"{refno}").RunInPdms();
                }catch (Exception) {}
            }
        }


        void InitializeToolTip()
        {
            toolTip1.SetToolTip(this.RebuildWelds, "Перестроить сварные швы/муфты");                //Блок установки подсказок на кнопки
            toolTip1.SetToolTip(this.DeleteWelds, "Удалить все сварные швы/муфты ");
            toolTip1.SetToolTip(this.ShowMark, "Показать маркировку");
            toolTip1.SetToolTip(this.HideMark, "Скрыть маркировку");
        }

        private void SetStandartFunction()
        {   //Нстройка комбобокса
            CoWeCombobox.Items.AddRange(new string[] { "WELD", "COUP" });
            CoWeCombobox.SelectedIndex = 0;
            comboBoxCeOrCollect.Items.AddRange(new string[] { "CE", "Collection" });
            comboBoxCeOrCollect.SelectedIndex = 0;
            doublecomboBox1.Items.AddRange(new string[] { "CE", "Collection" });
            doublecomboBox1.SelectedIndex = 0;
            lenghtComboBox.Items.AddRange(new string[] { "CE", "Collection" });
            lenghtComboBox.SelectedIndex = 0;
            this.Validate(false);
        }

        private void DeleteWelds_Click(object sender, EventArgs e)                                  //Удаление всех weld и/или coub
        {
            DbElement ce = DbElement.GetElement("/" + NamePipe.Text);                               //Получаем данные из текст бокса в виде DbElement
            try
            {
                if (comboBoxCeOrCollect.Text == "CE")                                               //CE - Текущий(Выбранный на данный момент элемент)
                     Command.CreateCommand($"!!deletetype(|/{ce}|)").RunInPdms(); 
                else                                                                                //Отработка если выбрана коллекция
                {
                    var collection = GetCollection(NamePipe.Text);                                  //Получаем коллекцию из AVEVA
                    foreach (var item in collection)
                        Command.CreateCommand($"!!deletetype(|/{item}|)").RunInPdms();              //Так же удаляет все weld и/или coub, но coub/weld берёт из коллекции AVEVA
                }
            }catch(Exception ex){MessageBox.Show(ex.Message);}
        }

        private void ShowMark_Click(object sender, EventArgs e)                                         //Маркировка coub/weld
        {                                                                          
            DbElement ce = DbElement.GetElement("/" + NamePipe.Text);                                   //Получаем данные из текст бокса в виде DbElement

            if (comboBoxCeOrCollect.Text == "CE")                                                       //CE - Текущий(Выбранный на данный момент элемент)
                StartMark(ce, "mark ce");                                                                          //Функция для маркировки
            else                                                                                        //Отработка если выбрана коллекция
            {
                var collection = GetCollection(NamePipe.Text);                                          //Получаем коллекцию из AVEVA
                foreach (var item in collection)
                    StartMark(item, "mark ce");                                                         //Так же маркирует, но coub/weld берёт из коллекции AVEVA
            }
        }

        private void HideMark_Click(object sender, EventArgs e)                                         //Убирает маркировку с coup/weld
        {
            DbElement ce = DbElement.GetElement("/" + NamePipe.Text);                                   //Получаем данные из текст бокса в виде DbElement

            if (comboBoxCeOrCollect.Text == "CE")                                                       //CE - Текущий(Выбранный на данный момент элемент)
                StartMark(ce, "unmark ce");                                                                          //Функция для убирания маркировки
            else                                                                                        //Отработка если выбрана коллекция
            {
                var collection = GetCollection(NamePipe.Text);                                          //Получаем коллекцию из AVEVA
                foreach (var item in collection)
                    StartMark(item, "unmark ce");                                                       //Так же убирает маркировку, но coub / weld берёт из коллекции AVEVA
            }
        }

        private void StartMark(DbElement ce, string mark)
        {
            DbElementType[] bran = { DbElementTypeInstance.COUPLING, DbElementTypeInstance.WELD };      //Выбор только сварных стыков

            TypeFilter filtBran = new TypeFilter(bran);                                                 //Настройка фильтра
            DBElementCollection CollectBran = new DBElementCollection(ce, filtBran);                    //Собираем в коллекцию
            var OutBran = CollectBran.Cast<DbElement>()
                       .Where(element => 
                       element.ElementType == DbElementTypeInstance.COUPLING    ||
                       element.ElementType == DbElementTypeInstance.WELD        ).ToList();             //Получение из коллекции сварных стыков в трубе в цикле
            var BranList = OutBran.Select(dbElement => new ItemForCheck(dbElement)).ToList();           //Подключаем класс ItemForCheck для работы с pml запросами и атрибутами

            var resultAllList = BranList.GroupBy(element => new { element.CE }).Select(group => new { ce = group.Key.CE }).ToList();
            foreach (var item in resultAllList)
            {
                Command.CreateCommand($"{item.ce}").RunInPdms();                                        //Переходим на coup/weld
                Command.CreateCommand(mark).RunInPdms();
            }
        }

        private void CEButton_Click(object sender, EventArgs e)                                         //Заполнение текстбокса первой вкладки
        {
            string text = AddToTextBox(comboBoxCeOrCollect.Text);
            if (text != null)
                NamePipe.Text = text;
        }


        private void addDoubleBtn_Click(object sender, EventArgs e)                                     //Заполнение текстбокса первой вкладки
        {
            string text = AddToTextBox(doublecomboBox1.Text);
            if (text != null)
                doubleTextBox.Text = text;
        }

        private void addLenghtBtn_Click(object sender, EventArgs e)
        {
            string text = AddToTextBox(lenghtComboBox.Text);                                            //Заполнение текстбокса первой вкладки
            if (text != null)
                lenghtTextBox.Text = text;
        }

        private string AddToTextBox(string comboBox)
        {
            if (comboBox == "CE")
            {
                DbElement ce = CurrentElement.Element;                                                  //Выбор текущего элемента в aveva
                string typeCe = AttributeStringByName(ce, "type");
                if (typeCe == "BRAN" || typeCe == "PIPE" || typeCe == "GPWL" || typeCe == "SITE" || typeCe == "ZONE")//Проверка приходящего элемента
                    return ce.ToString();
                else
                    MessageBox.Show("Нужно выбрать PIPE, BRAN, GPWL или Collection");
                
                return null;
            }
            else return "Collection";
        }

        [PMLNetCallable()]
        private void RebuildWelds_Click(object sender, EventArgs e)
        {
            try{Command.CreateCommand("$M \\\\AVEVA-NK-VM2\\Share\\AVEVA\\aveva_start\\ADDONSE3D2.1\\MACRO\\OpenCommanLine.pmlmac").RunInPdms();}catch (Exception){}

            CreateCoupling c = new CreateCoupling();                                                    //Создание объекта класса для создания Coupling
            CreateWELDING w = new CreateWELDING();                                                      //Создание объекта класса для создания WELDING

            if (CoWeCombobox.Text == "WELD")                                                            //Проверка на выбранный элемент в текстбоксе
                w.CreateWELDStart(NamePipe.Text, comboBoxCeOrCollect.Text, OnlyMem.Checked);            //Начало построения weld
            if (CoWeCombobox.Text == "COUP")
                c.CreateCoupStart(NamePipe.Text, comboBoxCeOrCollect.Text);                             //Начало построения coup

            Command.CreateCommand("!!PSTLEN.Delete()").Run();
            Command.CreateCommand("!!DIST.Delete()").Run();
        }

        private void CoWeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CoWeCombobox.Text == "WELD")
                label3.Text = "Элемент: сварной шов";                                                   //Изменение текста при выбора того или иного элемента
            if (CoWeCombobox.Text == "COUP")
                label3.Text = "Элемент: муфта";
        }

        private void comboBoxCeOrCollect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCeOrCollect.Text == "CE")                                                       //При выборе CE или Collection будет меняться и текстбокс. 
                NamePipe.Text = "CE";
            else NamePipe.Text = "Collection";
        }

        private IEnumerable<DbElement> GetCollection(string cemem)                                      //PML функция для сбора всех элементов из коллекции
        {
            var command = Command.CreateCommand($"!!LISTFILE = !!getclashcollections(|{cemem}|)");
            command.Run();
            string listfile = command.GetPMLVariableString("LISTFILE");
            Command.CreateCommand("!LISTFILE.Delete()").Run();
            return File.ReadAllLines(listfile, Encoding.Default).
                    Select(DbElement.GetElement).ToList();
        }

        private void reportDoWeldBtn_Click(object sender, EventArgs e)                                  //Отчёт выхода за пределы каталожной длинны weld'ов
        {
            if (lenghtComboBox.Text == "CE")
            {
                lengthWeldDataGridView.Rows.Clear();
                DbElement elementWeld = DbElement.GetElement(lenghtTextBox.Text);                       
                if (elementWeld.ToString() != "")
                    elementWeld = DbElement.GetElement($"/{lenghtTextBox.Text}");
                ReportWeldCoup.CheckLenght(elementWeld, lengthWeldDataGridView);
            }
            else                                                                                        //Передача в функцию коллекции
            {
                doubleWeldDataGridView.Rows.Clear();                                
                IEnumerable<DbElement> list = GetCollection("Collection");
                foreach (var item in list)
                    ReportWeldCoup.CheckLenght(item, lengthWeldDataGridView);
            }
        }

        private void button1_Click(object sender, EventArgs e)                                          //Отчёт дублированных weld'ов
        {
            if (doublecomboBox1.Text == "CE")
            {
                doubleWeldDataGridView.Rows.Clear();
                DbElement elementWeld = DbElement.GetElement(doubleTextBox.Text);
                if (elementWeld.ToString() != "")
                    elementWeld = DbElement.GetElement($"/{doubleTextBox.Text}");
                ReportWeldCoup.ReportWeldPosition(elementWeld, doubleWeldDataGridView);
            }
            else                                                                                        //Передача в функцию коллекции
            {
                doubleWeldDataGridView.Rows.Clear();
                IEnumerable<DbElement> list = GetCollection("Collection");
                foreach (var item in list)
                    ReportWeldCoup.ReportWeldPosition(item, doubleWeldDataGridView);
            }
            RemoveDuplicateRows(doubleWeldDataGridView, "Refno");                                       //Удаление из таблицы дубликатов через 'Refno'
        }

        private void RemoveDuplicateRows(DataGridView dataGridView, string columnName)                  //Удаление из таблицы дубликатов через 'Refno'
        {
            var uniqueValues = new HashSet<object>();
            var duplicateIndices = new List<int>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cellValue = row.Cells[columnName].Value;
                    if (!uniqueValues.Add(cellValue))
                        duplicateIndices.Add(row.Index);
                }
            }
            for (int i = duplicateIndices.Count - 1; i >= 0; i--)
                dataGridView.Rows.RemoveAt(duplicateIndices[i]);
        }

        private void doublecomboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doublecomboBox1.Text == "CE")                                                           //При выборе CE или Collection будет меняться и текстбокс. 
                doubleTextBox.Text = "CE";
            else doubleTextBox.Text = "Collection";
        }

        private void lenghtTextBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lenghtComboBox.Text == "CE")                                                            //При выборе CE или Collection будет меняться и текстбокс. 
                lenghtTextBox.Text = "CE";
            else lenghtTextBox.Text = "Collection";
        }

        private void reportExcelDouble_Click(object sender, EventArgs e)                                //Отчёт в эксель
        {
            try{ReportWeldCoup.ExlReportDouble(doubleWeldDataGridView, "Position");
            }catch (Exception ex){MessageBox.Show(ex.Message);}
        }

        private void ExReportBtn_Click(object sender, EventArgs e)                                      //Отчёт в эксель
        {
            try{ReportWeldCoup.ExlReportDouble(lengthWeldDataGridView, "Length");
            }catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string path = @"path";
            Process.Start(path);
        }
    }
}

