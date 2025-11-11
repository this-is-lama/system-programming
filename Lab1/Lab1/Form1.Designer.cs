using System.Drawing;
using System.Windows.Forms;

namespace Lab1
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataGrid_supportTable = new DataGridView();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            dataGrid_symbol_table = new DataGridView();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            button1 = new Button();
            button2 = new Button();
            tbErrorOnePass = new TextBox();
            panel1 = new Panel();
            label5 = new Label();
            panel2 = new Panel();
            txtOperCode = new TextBox();
            txtSource = new TextBox();
            label8 = new Label();
            tbErrorTwoPass = new TextBox();
            tbBinaryCode = new ListBox();
            label6 = new Label();
            bindingSource1 = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)dataGrid_supportTable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGrid_symbol_table).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // dataGrid_supportTable
            // 
            dataGrid_supportTable.AllowUserToAddRows = false;
            dataGrid_supportTable.AllowUserToDeleteRows = false;
            dataGrid_supportTable.AllowUserToResizeColumns = false;
            dataGrid_supportTable.AllowUserToResizeRows = false;
            dataGrid_supportTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid_supportTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid_supportTable.BackgroundColor = SystemColors.ActiveBorder;
            dataGrid_supportTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid_supportTable.ColumnHeadersVisible = false;
            dataGrid_supportTable.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, Column7, Column8 });
            dataGrid_supportTable.Location = new Point(0, 22);
            dataGrid_supportTable.Margin = new Padding(2, 3, 2, 3);
            dataGrid_supportTable.Name = "dataGrid_supportTable";
            dataGrid_supportTable.ReadOnly = true;
            dataGrid_supportTable.RowHeadersVisible = false;
            dataGrid_supportTable.RowHeadersWidth = 62;
            dataGrid_supportTable.Size = new Size(520, 252);
            dataGrid_supportTable.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Column5";
            dataGridViewTextBoxColumn4.MinimumWidth = 8;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Column6";
            dataGridViewTextBoxColumn5.MinimumWidth = 8;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // Column7
            // 
            Column7.HeaderText = "Column7";
            Column7.MinimumWidth = 8;
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            // 
            // Column8
            // 
            Column8.HeaderText = "Column8";
            Column8.MinimumWidth = 8;
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            // 
            // dataGrid_symbol_table
            // 
            dataGrid_symbol_table.AllowUserToAddRows = false;
            dataGrid_symbol_table.AllowUserToDeleteRows = false;
            dataGrid_symbol_table.AllowUserToResizeColumns = false;
            dataGrid_symbol_table.AllowUserToResizeRows = false;
            dataGrid_symbol_table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid_symbol_table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid_symbol_table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid_symbol_table.ColumnHeadersVisible = false;
            dataGrid_symbol_table.Columns.AddRange(new DataGridViewColumn[] { Column5, Column6 });
            dataGrid_symbol_table.Location = new Point(93, 297);
            dataGrid_symbol_table.Margin = new Padding(2, 3, 2, 3);
            dataGrid_symbol_table.Name = "dataGrid_symbol_table";
            dataGrid_symbol_table.ReadOnly = true;
            dataGrid_symbol_table.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGrid_symbol_table.RowHeadersVisible = false;
            dataGrid_symbol_table.RowHeadersWidth = 62;
            dataGrid_symbol_table.Size = new Size(320, 147);
            dataGrid_symbol_table.TabIndex = 3;
            // 
            // Column5
            // 
            Column5.HeaderText = "Column5";
            Column5.MinimumWidth = 8;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.HeaderText = "Column6";
            Column6.MinimumWidth = 8;
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F);
            label1.Location = new Point(106, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(192, 17);
            label1.TabIndex = 4;
            label1.Text = "Исходный текст программы";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F);
            label2.Location = new Point(106, 369);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(170, 17);
            label2.TabIndex = 5;
            label2.Text = "Таблица кодов операций";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F);
            label3.Location = new Point(173, 0);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(179, 17);
            label3.TabIndex = 6;
            label3.Text = "Вспомогательная таблица";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F);
            label4.Location = new Point(159, 277);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(202, 17);
            label4.TabIndex = 7;
            label4.Text = "Таблица символических имен";
            // 
            // button1
            // 
            button1.Location = new Point(152, 614);
            button1.Margin = new Padding(2, 3, 2, 3);
            button1.Name = "button1";
            button1.Size = new Size(147, 37);
            button1.TabIndex = 8;
            button1.Text = "Первый проход";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(653, 614);
            button2.Margin = new Padding(2, 3, 2, 3);
            button2.Name = "button2";
            button2.Size = new Size(147, 37);
            button2.TabIndex = 9;
            button2.Text = "Второй проход";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tbErrorOnePass
            // 
            tbErrorOnePass.Location = new Point(42, 467);
            tbErrorOnePass.Margin = new Padding(2, 3, 2, 3);
            tbErrorOnePass.Multiline = true;
            tbErrorOnePass.Name = "tbErrorOnePass";
            tbErrorOnePass.ScrollBars = ScrollBars.Vertical;
            tbErrorOnePass.Size = new Size(434, 111);
            tbErrorOnePass.TabIndex = 10;
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(dataGrid_supportTable);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(dataGrid_symbol_table);
            panel1.Controls.Add(tbErrorOnePass);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(472, 14);
            panel1.Margin = new Padding(2, 3, 2, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(534, 587);
            panel1.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F);
            label5.Location = new Point(159, 447);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(174, 17);
            label5.TabIndex = 11;
            label5.Text = "Ошибки первого прохода";
            // 
            // panel2
            // 
            panel2.Controls.Add(txtOperCode);
            panel2.Controls.Add(txtSource);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(13, 14);
            panel2.Margin = new Padding(2, 3, 2, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(414, 587);
            panel2.TabIndex = 12;
            // 
            // txtOperCode
            // 
            txtOperCode.Font = new Font("Consolas", 12F);
            txtOperCode.Location = new Point(0, 392);
            txtOperCode.Margin = new Padding(4, 3, 4, 3);
            txtOperCode.Multiline = true;
            txtOperCode.Name = "txtOperCode";
            txtOperCode.ScrollBars = ScrollBars.Vertical;
            txtOperCode.Size = new Size(410, 194);
            txtOperCode.TabIndex = 7;
            // 
            // txtSource
            // 
            txtSource.Font = new Font("Consolas", 12F);
            txtSource.Location = new Point(0, 23);
            txtSource.Margin = new Padding(4, 3, 4, 3);
            txtSource.Multiline = true;
            txtSource.Name = "txtSource";
            txtSource.ScrollBars = ScrollBars.Vertical;
            txtSource.Size = new Size(414, 342);
            txtSource.TabIndex = 6;
            txtSource.TextChanged += txtSource_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 10F);
            label8.Location = new Point(1138, 12);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(102, 17);
            label8.TabIndex = 12;
            label8.Text = "Двоичный код";
            // 
            // tbErrorTwoPass
            // 
            tbErrorTwoPass.BackColor = SystemColors.ButtonHighlight;
            tbErrorTwoPass.Location = new Point(1013, 520);
            tbErrorTwoPass.Margin = new Padding(2, 3, 2, 3);
            tbErrorTwoPass.Multiline = true;
            tbErrorTwoPass.Name = "tbErrorTwoPass";
            tbErrorTwoPass.ReadOnly = true;
            tbErrorTwoPass.Size = new Size(358, 84);
            tbErrorTwoPass.TabIndex = 6;
            // 
            // tbBinaryCode
            // 
            tbBinaryCode.FormattingEnabled = true;
            tbBinaryCode.ItemHeight = 15;
            tbBinaryCode.Location = new Point(1030, 35);
            tbBinaryCode.Margin = new Padding(2, 3, 2, 3);
            tbBinaryCode.Name = "tbBinaryCode";
            tbBinaryCode.Size = new Size(314, 454);
            tbBinaryCode.TabIndex = 13;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 10F);
            label6.Location = new Point(1105, 500);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(175, 17);
            label6.TabIndex = 7;
            label6.Text = "Ошибки второго прохода";
            label6.TextAlign = ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1382, 668);
            Controls.Add(label6);
            Controls.Add(tbBinaryCode);
            Controls.Add(tbErrorTwoPass);
            Controls.Add(label8);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(2, 3, 2, 3);
            Name = "Form1";
            Text = "Лаба 1: Двухпросмотровый ассемблер в абсолютном формате";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGrid_supportTable).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGrid_symbol_table).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion
        private System.Windows.Forms.DataGridView dataGrid_supportTable;
        private System.Windows.Forms.DataGridView dataGrid_symbol_table;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbErrorOnePass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbErrorTwoPass;
        private System.Windows.Forms.ListBox tbBinaryCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox txtOperCode;
        private System.Windows.Forms.TextBox txtSource;
    }
}

