
using MetroFramework.Controls;

namespace Aveva.C.Sharp.Training
{
    partial class AddSelectPipe
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        [System.Obsolete]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddSelectPipe));
            this.CEButton = new System.Windows.Forms.Button();
            this.NamePipe = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CoWeCombobox = new System.Windows.Forms.ComboBox();
            this.comboBoxCeOrCollect = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DeleteWelds = new System.Windows.Forms.Button();
            this.RebuildWelds = new System.Windows.Forms.Button();
            this.HideMark = new System.Windows.Forms.Button();
            this.ShowMark = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.doubleWeldDataGridView = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.OnlyMem = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.doublecomboBox1 = new System.Windows.Forms.ComboBox();
            this.addDoubleBtn = new System.Windows.Forms.Button();
            this.doubleTextBox = new System.Windows.Forms.TextBox();
            this.reportExcelDouble = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lenghtComboBox = new System.Windows.Forms.ComboBox();
            this.addLenghtBtn = new System.Windows.Forms.Button();
            this.lenghtTextBox = new System.Windows.Forms.TextBox();
            this.ExReportBtn = new System.Windows.Forms.Button();
            this.lengthWeldDataGridView = new System.Windows.Forms.DataGridView();
            this.reportDoWeldBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doubleWeldDataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lengthWeldDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CEButton
            // 
            this.CEButton.Location = new System.Drawing.Point(3, 3);
            this.CEButton.Name = "CEButton";
            this.CEButton.Size = new System.Drawing.Size(31, 23);
            this.CEButton.TabIndex = 0;
            this.CEButton.Text = "CE";
            this.CEButton.UseVisualStyleBackColor = true;
            this.CEButton.Click += new System.EventHandler(this.CEButton_Click);
            // 
            // NamePipe
            // 
            this.NamePipe.Location = new System.Drawing.Point(131, 6);
            this.NamePipe.Multiline = true;
            this.NamePipe.Name = "NamePipe";
            this.NamePipe.ReadOnly = true;
            this.NamePipe.Size = new System.Drawing.Size(236, 20);
            this.NamePipe.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Проверяемая область: Весь PIPE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Тип элемента";
            // 
            // CoWeCombobox
            // 
            this.CoWeCombobox.FormattingEnabled = true;
            this.CoWeCombobox.Location = new System.Drawing.Point(89, 58);
            this.CoWeCombobox.Name = "CoWeCombobox";
            this.CoWeCombobox.Size = new System.Drawing.Size(156, 21);
            this.CoWeCombobox.TabIndex = 5;
            this.CoWeCombobox.SelectedIndexChanged += new System.EventHandler(this.CoWeCombobox_SelectedIndexChanged);
            // 
            // comboBoxCeOrCollect
            // 
            this.comboBoxCeOrCollect.FormattingEnabled = true;
            this.comboBoxCeOrCollect.Location = new System.Drawing.Point(41, 4);
            this.comboBoxCeOrCollect.Name = "comboBoxCeOrCollect";
            this.comboBoxCeOrCollect.Size = new System.Drawing.Size(84, 21);
            this.comboBoxCeOrCollect.TabIndex = 7;
            this.comboBoxCeOrCollect.SelectedIndexChanged += new System.EventHandler(this.comboBoxCeOrCollect_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Элемент: сварной шов";
            //this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // DeleteWelds
            // 
            this.DeleteWelds.BackColor = System.Drawing.Color.RosyBrown;
            this.DeleteWelds.BackgroundImage = global::COUPWELD.Properties.Resources.Trash;
            this.DeleteWelds.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DeleteWelds.Location = new System.Drawing.Point(242, 19);
            this.DeleteWelds.Name = "DeleteWelds";
            this.DeleteWelds.Size = new System.Drawing.Size(111, 52);
            this.DeleteWelds.TabIndex = 10;
            this.DeleteWelds.UseVisualStyleBackColor = false;
            this.DeleteWelds.Click += new System.EventHandler(this.DeleteWelds_Click);
            // 
            // RebuildWelds
            // 
            this.RebuildWelds.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.RebuildWelds.BackgroundImage = global::COUPWELD.Properties.Resources.Start;
            this.RebuildWelds.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RebuildWelds.Location = new System.Drawing.Point(7, 107);
            this.RebuildWelds.Name = "RebuildWelds";
            this.RebuildWelds.Size = new System.Drawing.Size(361, 68);
            this.RebuildWelds.TabIndex = 9;
            this.RebuildWelds.UseVisualStyleBackColor = false;
            this.RebuildWelds.Click += new System.EventHandler(this.RebuildWelds_Click);
            // 
            // HideMark
            // 
            this.HideMark.BackgroundImage = global::COUPWELD.Properties.Resources.eye_blocked;
            this.HideMark.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.HideMark.Location = new System.Drawing.Point(124, 19);
            this.HideMark.Name = "HideMark";
            this.HideMark.Size = new System.Drawing.Size(112, 52);
            this.HideMark.TabIndex = 8;
            this.HideMark.UseVisualStyleBackColor = true;
            this.HideMark.Click += new System.EventHandler(this.HideMark_Click);
            // 
            // ShowMark
            // 
            this.ShowMark.BackgroundImage = global::COUPWELD.Properties.Resources.eye_open;
            this.ShowMark.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowMark.Location = new System.Drawing.Point(7, 19);
            this.ShowMark.Name = "ShowMark";
            this.ShowMark.Size = new System.Drawing.Size(112, 52);
            this.ShowMark.TabIndex = 7;
            this.ShowMark.UseVisualStyleBackColor = true;
            this.ShowMark.Click += new System.EventHandler(this.ShowMark_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ShowMark);
            this.groupBox1.Controls.Add(this.DeleteWelds);
            this.groupBox1.Controls.Add(this.HideMark);
            this.groupBox1.Location = new System.Drawing.Point(7, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 81);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Функционал";
            // 
            // doubleWeldDataGridView
            // 
            this.doubleWeldDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doubleWeldDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.doubleWeldDataGridView.Location = new System.Drawing.Point(142, 6);
            this.doubleWeldDataGridView.Name = "doubleWeldDataGridView";
            this.doubleWeldDataGridView.Size = new System.Drawing.Size(614, 256);
            this.doubleWeldDataGridView.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(770, 294);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.OnlyMem);
            this.tabPage1.Controls.Add(this.CEButton);
            this.tabPage1.Controls.Add(this.comboBoxCeOrCollect);
            this.tabPage1.Controls.Add(this.CoWeCombobox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.RebuildWelds);
            this.tabPage1.Controls.Add(this.NamePipe);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(762, 268);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Расставить стыки";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // OnlyMem
            // 
            this.OnlyMem.AutoSize = true;
            this.OnlyMem.Location = new System.Drawing.Point(251, 60);
            this.OnlyMem.Name = "OnlyMem";
            this.OnlyMem.Size = new System.Drawing.Size(117, 17);
            this.OnlyMem.TabIndex = 12;
            this.OnlyMem.Text = "Только элементы";
            this.OnlyMem.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.doublecomboBox1);
            this.tabPage2.Controls.Add(this.addDoubleBtn);
            this.tabPage2.Controls.Add(this.doubleTextBox);
            this.tabPage2.Controls.Add(this.reportExcelDouble);
            this.tabPage2.Controls.Add(this.doubleWeldDataGridView);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(762, 268);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Проверка дублирования";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // doublecomboBox1
            // 
            this.doublecomboBox1.FormattingEnabled = true;
            this.doublecomboBox1.Location = new System.Drawing.Point(6, 6);
            this.doublecomboBox1.Name = "doublecomboBox1";
            this.doublecomboBox1.Size = new System.Drawing.Size(130, 21);
            this.doublecomboBox1.TabIndex = 18;
            this.doublecomboBox1.SelectedIndexChanged += new System.EventHandler(this.doublecomboBox1_SelectedIndexChanged);
            // 
            // addDoubleBtn
            // 
            this.addDoubleBtn.Location = new System.Drawing.Point(6, 59);
            this.addDoubleBtn.Name = "addDoubleBtn";
            this.addDoubleBtn.Size = new System.Drawing.Size(130, 23);
            this.addDoubleBtn.TabIndex = 17;
            this.addDoubleBtn.Text = "CE";
            this.addDoubleBtn.UseVisualStyleBackColor = true;
            this.addDoubleBtn.Click += new System.EventHandler(this.addDoubleBtn_Click);
            // 
            // doubleTextBox
            // 
            this.doubleTextBox.Location = new System.Drawing.Point(6, 33);
            this.doubleTextBox.Multiline = true;
            this.doubleTextBox.Name = "doubleTextBox";
            this.doubleTextBox.ReadOnly = true;
            this.doubleTextBox.Size = new System.Drawing.Size(130, 20);
            this.doubleTextBox.TabIndex = 16;
            // 
            // reportExcelDouble
            // 
            this.reportExcelDouble.Location = new System.Drawing.Point(6, 117);
            this.reportExcelDouble.Name = "reportExcelDouble";
            this.reportExcelDouble.Size = new System.Drawing.Size(130, 23);
            this.reportExcelDouble.TabIndex = 15;
            this.reportExcelDouble.Text = "Excel";
            this.reportExcelDouble.UseVisualStyleBackColor = true;
            this.reportExcelDouble.Click += new System.EventHandler(this.reportExcelDouble_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Отчёт дубликатов";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lenghtComboBox);
            this.tabPage3.Controls.Add(this.addLenghtBtn);
            this.tabPage3.Controls.Add(this.lenghtTextBox);
            this.tabPage3.Controls.Add(this.ExReportBtn);
            this.tabPage3.Controls.Add(this.lengthWeldDataGridView);
            this.tabPage3.Controls.Add(this.reportDoWeldBtn);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(762, 268);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Проверка длины";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lenghtComboBox
            // 
            this.lenghtComboBox.FormattingEnabled = true;
            this.lenghtComboBox.Location = new System.Drawing.Point(6, 6);
            this.lenghtComboBox.Name = "lenghtComboBox";
            this.lenghtComboBox.Size = new System.Drawing.Size(130, 21);
            this.lenghtComboBox.TabIndex = 21;
            this.lenghtComboBox.SelectedIndexChanged += new System.EventHandler(this.lenghtTextBox1_SelectedIndexChanged);
            // 
            // addLenghtBtn
            // 
            this.addLenghtBtn.Location = new System.Drawing.Point(6, 59);
            this.addLenghtBtn.Name = "addLenghtBtn";
            this.addLenghtBtn.Size = new System.Drawing.Size(130, 23);
            this.addLenghtBtn.TabIndex = 20;
            this.addLenghtBtn.Text = "CE";
            this.addLenghtBtn.UseVisualStyleBackColor = true;
            this.addLenghtBtn.Click += new System.EventHandler(this.addLenghtBtn_Click);
            // 
            // lenghtTextBox
            // 
            this.lenghtTextBox.Location = new System.Drawing.Point(6, 33);
            this.lenghtTextBox.Multiline = true;
            this.lenghtTextBox.Name = "lenghtTextBox";
            this.lenghtTextBox.ReadOnly = true;
            this.lenghtTextBox.Size = new System.Drawing.Size(130, 20);
            this.lenghtTextBox.TabIndex = 19;
            // 
            // ExReportBtn
            // 
            this.ExReportBtn.Location = new System.Drawing.Point(6, 117);
            this.ExReportBtn.Name = "ExReportBtn";
            this.ExReportBtn.Size = new System.Drawing.Size(130, 23);
            this.ExReportBtn.TabIndex = 18;
            this.ExReportBtn.Text = "Excel";
            this.ExReportBtn.UseVisualStyleBackColor = true;
            this.ExReportBtn.Click += new System.EventHandler(this.ExReportBtn_Click);
            // 
            // lengthWeldDataGridView
            // 
            this.lengthWeldDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lengthWeldDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lengthWeldDataGridView.Location = new System.Drawing.Point(142, 6);
            this.lengthWeldDataGridView.Name = "lengthWeldDataGridView";
            this.lengthWeldDataGridView.Size = new System.Drawing.Size(614, 256);
            this.lengthWeldDataGridView.TabIndex = 16;
            // 
            // reportDoWeldBtn
            // 
            this.reportDoWeldBtn.Location = new System.Drawing.Point(6, 88);
            this.reportDoWeldBtn.Name = "reportDoWeldBtn";
            this.reportDoWeldBtn.Size = new System.Drawing.Size(130, 23);
            this.reportDoWeldBtn.TabIndex = 17;
            this.reportDoWeldBtn.Text = "Отчёт";
            this.reportDoWeldBtn.UseVisualStyleBackColor = true;
            this.reportDoWeldBtn.Click += new System.EventHandler(this.reportDoWeldBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(331, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(36, 27);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // AddSelectPipe
            // 
            this.Controls.Add(this.tabControl1);
            this.Name = "AddSelectPipe";
            this.Size = new System.Drawing.Size(776, 299);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.doubleWeldDataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lengthWeldDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroTabPage tabPipe;
        private System.Windows.Forms.Button CEButton;
        private System.Windows.Forms.TextBox NamePipe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CoWeCombobox;
        private System.Windows.Forms.ComboBox comboBoxCeOrCollect;
        private System.Windows.Forms.Button ShowMark;
        private System.Windows.Forms.Button HideMark;
        private System.Windows.Forms.Button RebuildWelds;
        private System.Windows.Forms.Button DeleteWelds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView doubleWeldDataGridView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button reportExcelDouble;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ExReportBtn;
        private System.Windows.Forms.DataGridView lengthWeldDataGridView;
        private System.Windows.Forms.Button reportDoWeldBtn;
        private System.Windows.Forms.Button addDoubleBtn;
        private System.Windows.Forms.TextBox doubleTextBox;
        private System.Windows.Forms.Button addLenghtBtn;
        private System.Windows.Forms.TextBox lenghtTextBox;
        private System.Windows.Forms.ComboBox doublecomboBox1;
        private System.Windows.Forms.ComboBox lenghtComboBox;
        private System.Windows.Forms.CheckBox OnlyMem;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
