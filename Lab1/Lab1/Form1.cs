namespace Lab1
{
    public partial class Form1 : Form
    {
        private Core CORE;

        bool firstPassError = false;
        FirstPass first_pass;
        SecondPass second_pass;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            firstPassError = true;
            CORE = new Core();
            first_pass = new FirstPass(CORE, dataGrid_supportTable, dataGrid_symbol_table);
            Clear_tables();

            string[,] arr_SourceCode = Core.ParseTextBox(txtSource, 4);
            string[,] arr_OperCode = Core.ParseTextBox(txtOperCode, 3);

            PrintTable(arr_SourceCode, "SourceCode");
            PrintTable(arr_OperCode, "OperCode");

            if (CORE.CheckOperationCodeTable(ref arr_OperCode))
            {
                if (first_pass.FirstPassFunc(arr_SourceCode, arr_OperCode))
                {
                    firstPassError = false;
                    button2.Enabled = true;
                    AddErrorTextBox(tbErrorOnePass, CORE.ErrorMessage);
                }
                else
                {
                    AddErrorTextBox(tbErrorOnePass, CORE.ErrorMessage);
                }
            }
            else
            {
                AddErrorTextBox(tbErrorOnePass, CORE.ErrorMessage);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            second_pass = new SecondPass(CORE);
            tbBinaryCode.Items.Clear();
            tbErrorTwoPass.Clear();

            if (firstPassError == false)
                if (second_pass.SecondPassFunc(tbBinaryCode))
                {
                    AddErrorTextBox(tbErrorTwoPass, CORE.ErrorMessage);
                }
                else
                {
                    AddErrorTextBox(tbErrorTwoPass, CORE.ErrorMessage);
                }
            else
                MessageBox.Show("Выполните первый проход");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSource.Lines = new string[]
            {
                "PROG    START   100",
                "        JMP     L1",
                "A1      RESB    10",
                "A2      RESW    20",
                "B1      BYTE    C\"Hello World!\"",
                "        BYTE    X\"5A2F016A\"",
                "        BYTE    12",
                "B2      WORD    40",
                "L1      LOADR1  B2",
                "        ADD     R1    R2",
                "        SAVER  A2",
                "        INT     ",
                "        END     100"
            };

            txtOperCode.Lines = new string[]
            {
                "JMP     01   4",
                "LOADR1  02   4",
                "ADD     04   2",
                "SAVER  05   4",
                "INT     06   1"
            };
        }

        void Clear_tables()
        {
            dataGrid_supportTable.Rows.Clear();
            dataGrid_symbol_table.Rows.Clear();
            tbErrorOnePass.Clear();
            tbErrorTwoPass.Clear();
            tbBinaryCode.Items.Clear();
        }

        void AddErrorTextBox(TextBox textBox_first_errors, string message)
        {
            textBox_first_errors.Text += message;
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        public static void PrintTable(string[,] table, string title)
        {
            Console.WriteLine("=== " + title + " ===");
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(i + (table[i, j] ?? "NONE").PadRight(12));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
