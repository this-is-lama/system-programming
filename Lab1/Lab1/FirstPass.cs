using Lab1.Checks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab1
{
    class FirstPass
    {
        Core CORE;
        const int MAX_MemmoryAdr = 16777215;
        DataGridView DG_SupportTable;
        DataGridView DG_SymbolTable;
        int StartFlag;
        int EndFlag;

        public FirstPass(Core core, DataGridView DG_SupportTable, DataGridView DG_SymbolTable)
        {
            CORE = core;
            this.DG_SupportTable = DG_SupportTable;
            this.DG_SymbolTable = DG_SymbolTable;
            this.StartFlag = 0;
            this.EndFlag = 0;
        }

        public bool FirstPassFunc(string[,] SourceCodeTable, string[,] arr_CodeTable) {
            CORE.AddressCount = 0;
            CORE.StartAddress = 0;
            CORE.EndAddress = 0;
            CORE.SymbolNameTable.Add(new List<string>());
            CORE.SymbolNameTable.Add(new List<string>());

            CORE.SupportTable.Add(new List<string>());
            CORE.SupportTable.Add(new List<string>());
            CORE.SupportTable.Add(new List<string>());
            CORE.SupportTable.Add(new List<string>());

            int rows = SourceCodeTable.GetLength(0) - 1;
            string label, MKOP, Operand1, Operand2;

            // Проверка первой строки на директиву START
            string start_error = IsFirstStart(SourceCodeTable);
            if (start_error.Length > 0) {
                CORE.ErrorMessage = start_error;
                return false;
            }
            // ------------------------------------------

            for (int i = 1; i <= rows; i++)
            {

                //берем строку из массива и сразу же проверяем корректность данных
                //строка состоит из Label MKOP Operand1 Operand2
                if (!GetRow(SourceCodeTable, i, out label, out MKOP, out Operand1, out Operand2))
                {
                    CORE.ErrorMessage = "Синтаксическая ошибка строка №" + (i + 1) + ".\r\n";
                    return false;
                }

                int number_in_LabelTable = CORE.FindLabelInLabelTable(label);
                if (number_in_LabelTable != -1)
                {
                    CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! Найдена уже существующая метка " + label + ".\r\n";
                    return false;
                }

                //если метка это не пустая строка и встречена после директивы старт
                //то добавляем её в таблицу меток
                if (label != "" && StartFlag == 1)
                {
                    CORE.SymbolNameTable[0].Add(label);
                    CORE.SymbolNameTable[1].Add(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)));
                }

                if (MKOP.Length <= 0)
                {
                    CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + ". Ошибка в МКОП, передан некорректно.\r\n";
                    return false;
                }

                if (TypeCheck.IsDirective(MKOP)) {
                    if (!DirectiveWork(MKOP, i, Operand1, Operand2)) { return false; }
                    if (EndFlag == 1) {
                        if (i != rows) {
                            CORE.ErrorMessage += "Предупреждение строка №" + (i + 2) + "! С данной строки программа не рассматривается, так как встретилась END директива.\r\n";
                        }
                        break; 
                    }
                }
                else {
                    int num = CORE.FindCodeInCodeTable(MKOP, arr_CodeTable);
                    if (num == -1) {
                        CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! МКОП не найден в таблице исходный текст программы " + MKOP + "!\r\n";
                        return false;
                    }

                    if (!CommandWork(i, MKOP, num, arr_CodeTable, Operand1, Operand2)) { return false; }
                }

            }

            if (EndFlag == 0) {
                CORE.ErrorMessage = "Ошибка END! Не найдена директива END в программе.\r\n";
                return false;
            }

            //Помещаем сформированную Вспомагательную таблицу в датагрид
            for (int i = 0; i < CORE.SupportTable[0].Count; i++)
            {
                DG_SupportTable.Rows.Add();
                DG_SupportTable.Rows[i].Cells[0].Value = CORE.SupportTable[0][i];
                DG_SupportTable.Rows[i].Cells[1].Value = CORE.SupportTable[1][i];
                DG_SupportTable.Rows[i].Cells[2].Value = CORE.SupportTable[2][i];
                DG_SupportTable.Rows[i].Cells[3].Value = CORE.SupportTable[3][i];
            }

            //Помещаем сформированную Таблицу вспомогательных имен в датагрид
            for (int j = 0; j < CORE.SymbolNameTable[1].Count; j++)
            {
                DG_SymbolTable.Rows.Add();
                DG_SymbolTable.Rows[j].Cells[0].Value = CORE.SymbolNameTable[0][j];
                DG_SymbolTable.Rows[j].Cells[1].Value = CORE.SymbolNameTable[1][j];
            }

            return true;
        }

        public string IsFirstStart(string[,] mas)
        {
            string label = mas[0, 0];
            string command = mas[0, 1].ToUpper();
            string opr1 = mas[0, 2];
            string opr2 = mas[0, 3];

            if (label.Length == 0 || label == ""  || label.Length >= 10 || TypeCheck.IsDirective(label) || TypeCheck.IsRegister(label))
            {
                return "Ошибка START! В первой строке должна быть метка (название программы <= 10 символам).\r\n";
            }
            
            if (command == "START")
            {
                StartFlag = 1;
                int number;
                if (int.TryParse(opr1, out number))
                {
                    CORE.AddressCount = number;
                    CORE.StartAddress = CORE.AddressCount;
                    if (CORE.AddressCount == 0)
                    {
                        return "Ошибка START! Адрес начала программы не может быть равен 0.\r\n";
                    }

                    CORE.AddStringToSupportTable(label, command, TypeConverter.ToSixChars(TypeConverter.DecToHex(number)), "");
                    CORE.NameProg = label;
                    if (opr2.Length > 0)
                        CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение START! Второй операнд директивы START не рассматривается.\r\n";

                }
                else
                {
                    return "Ошибка START! Неверный адрес начала программы!\r\n";
                }

                return "";
            }
            else
            {
                return "Ошибка START! В первой строке отсутствует команда START.\r\n";
            }
        }

        public bool DirectiveWork(string MKOP, int i, string Operand1, string Operand2) { 
            switch (MKOP) {
                case "START":
                    {
                        CORE.ErrorMessage = "Ошбика строка №" + (i + 1) + ". Директива START не должна повторяться в программе.\r\n";
                        return false;
                    }
                case "WORD":
                    {
                        int number;
                        //В WORD у нас могут быть записаны только положительные числа
                        //преобразовываем операнд в число
                        if (int.TryParse(Operand1, out number))
                        {
                            if (number >= 0 && number <= MAX_MemmoryAdr)
                            {
                                if (CORE.AddressCount + 3 > MAX_MemmoryAdr)
                                {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Convert.ToString(number), "");
                                CORE.AddressCount = CORE.AddressCount + 3;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы WORD не рассматривается\r\n";
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + ". Отрицательное число, либо превышено максимальное значение числа (16777215)\r\n";
                                return false;
                            }
                        }
                        else
                        {
                            //символ вопроса, резервирует 1 слово в памяти
                            if (Operand1.Length == 1 && Operand1 == "?")
                            {
                                if (CORE.AddressCount + 3 > MAX_MemmoryAdr)
                                {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Operand1, "");
                                CORE.AddressCount = CORE.AddressCount + 3;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы WORD не рассматривается\r\n";
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + ". Недопустимый операнд директивы WORD " + Operand1 + "\r\n";
                                return false;
                            }
                        }
                    }
                    break;
                case "BYTE":
                    {
                        int number;
                        //пытаемся преобразовать операнд в число (разрешено только положительное 0 до 255)
                        if (int.TryParse(Operand1, out number))
                        {
                            if (number >= 0 && number <= 255)
                            {
                                if (CORE.AddressCount + 1 > MAX_MemmoryAdr)
                                {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Convert.ToString(number), "");
                                CORE.AddressCount = CORE.AddressCount + 1;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы BYTE не рассматривается.\r\n";
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! Отрицательное число, либо > 255 недопустимо!\r\n";
                                return false;
                            }
                        }   
                        //если преобразование в число не получилось, значит разбираем строку
                        else
                        {
                            //первый символ 'C' второй и последний символ это кавычки и длина строки >3
                            string symbols = TypeCheck.IsString(Operand1);
                            if (symbols != "")
                            {
                                if (CORE.AddressCount + symbols.Length > MAX_MemmoryAdr) {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Operand1, "");
                                CORE.AddressCount = CORE.AddressCount + symbols.Length;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы BYTE не рассматривается.\r\n";
                                   
                                return true;
                            }

                            //первый символ 'X' второй и последний символ это кавычки и длина строки >3
                            symbols = TypeCheck.ByteString(Operand1);
                            if (symbols != "")
                            {
                                int lenght = symbols.Length;
                                //1 символ = 1 байт = 2 цифры в 16ричной системе = четное число символов
                                if ((lenght % 2) == 0)
                                {
                                    if (CORE.AddressCount + symbols.Length / 2 > MAX_MemmoryAdr)
                                    {
                                        CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                        return false;
                                    }

                                    CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Operand1, "");
                                    CORE.AddressCount = CORE.AddressCount + symbols.Length / 2;
                                    if (!CORE.MemoryCheck()) { return false; }

                                    if (Operand2.Length > 0)
                                        CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы BYTE не рассматривается.\r\n";
                                    return true;
                                }
                                else
                                {
                                    CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Невозможно преобразовать BYTE из нечетного количества символов!\r\n";
                                    return false;
                                }
                            }

                            //если там всего один символ "?"
                            if (Operand1.Length == 1 && Operand1 == "?")
                            {
                                if (CORE.AddressCount + 1 > MAX_MemmoryAdr)
                                {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Operand1, "");
                                CORE.AddressCount = CORE.AddressCount + 1;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы BYTE не рассматривается.\r\n";
                                return true;
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Неверный формат строки " + Operand1 + "!\r\n";
                                return false;
                            }
                        }
                    }
                    break;
                case "RESB":
                    {
                        int number;
                        if (int.TryParse(Operand1, out number))
                        {
                            if (number > 0)
                            {
                                if (CORE.AddressCount + number > MAX_MemmoryAdr)
                                {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Convert.ToString(number), "");
                                CORE.AddressCount = CORE.AddressCount + number;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы RESB не рассматривается.\r\n";
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Количество байт равно нулю или меньше нуля!\r\n";
                                return false;
                            }
                        }
                        else
                        {
                            CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Невозможно выполнить преобразование в число " + Operand1 + "!\r\n";
                            return false;
                        }
                    }
                    break;
                case "RESW":
                    {
                        int number;
                        if (int.TryParse(Operand1, out number))
                        {
                            if (number > 0)
                            {
                                if (CORE.AddressCount + number * 3 > MAX_MemmoryAdr)
                                {
                                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                                    return false;
                                }

                                CORE.AddStringToSupportTable(TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)), MKOP, Convert.ToString(number), "");
                                CORE.AddressCount = CORE.AddressCount + number * 3;
                                if (!CORE.MemoryCheck()) { return false; }

                                if (Operand2.Length > 0)
                                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд директивы RESW не рассматривается.\r\n";
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Количество слов равно нулю или меньше нуля!\r\n";
                                return false;
                            }
                        }
                        else
                        {
                            CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Невозможно выполнить преобразование в число " + Operand1 + "!\r\n";
                            return false;
                        }
                    }
                    break;
                case "END":
                    {
                        EndFlag = 1;
                        if (Operand1.Length == 0)
                        {
                            CORE.EndAddress = CORE.StartAddress;
                        }
                        else
                        {
                            int number;
                            if (TypeCheck.OnlyNumbers(Operand1))
                            {
                                if (!int.TryParse(Operand1, out number)) {
                                    CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Адрес точки входа невереный!\r\n";
                                    return false;
                                }

                                //если да то преобразуем 16ричное число в 10чное
                                CORE.EndAddress = number;
                                if (CORE.EndAddress >= CORE.StartAddress && CORE.EndAddress <= CORE.AddressCount)
                                {
                                    break;
                                }
                                else
                                {
                                    CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "! Адрес точки входа невереный!\r\n";
                                    return false;
                                }
                            }
                            else
                            {
                                CORE.ErrorMessage = "Ошибка cтрока №" + (i + 1) + "}! Неверный адрес входа в программу, ожидается 16-ричное значение!\r\n";
                                return false;
                            }
                        }                   
                    }
                    break;
            }   
            return true;
        }

        public bool CommandWork(int i, string MKOP, int num, string[,] arr_CodeTable, string Operand1, string Operand2) {
            //ДЛИНА КОМАНДЫ = 1
            //например NOP, операндов нет, а если и есть то не смотрим на них
            if (arr_CodeTable[num, 2] == "1")
            {
                if (CORE.AddressCount + 1 > MAX_MemmoryAdr)
                {
                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                    return false;
                }

                int AddressationType = TypeConverter.HexToDec(arr_CodeTable[num, 1]) * 4;

                CORE.AddStringToSupportTable(
                    TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)),
                    TypeConverter.ToTwoChars(TypeConverter.DecToHex(AddressationType)),
                    "",
                    ""
                );

                CORE.AddressCount = CORE.AddressCount + 1;
                if (!CORE.MemoryCheck()) { return false; }

                if (Operand1.Length > 0 || Operand2.Length > 0)
                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Операнды не рассматриваются в команде " + arr_CodeTable[num, 0] + ".\r\n";
            }

            //ДЛИНА КОМАНДЫ = 2
            //ADD r1,r2   операнды это регистры, либо число //INT 200
            else if (arr_CodeTable[num, 2] == "2")
            {
                int number;
                //сначала пытаемся преобразовать первый операнд в число
                if (int.TryParse(Operand1, out number))
                {
                    if (number >= 0 && number <= 255)
                    {
                        if (CORE.AddressCount + 2 > MAX_MemmoryAdr)
                        {
                            CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                            return false;
                        }

                        //так как операнд является числом, то это непосредственная адресация
                        //просто  сдвигаем на два разряда влево
                        int AddressationType = TypeConverter.HexToDec(arr_CodeTable[num, 1]) * 4;

                        CORE.AddStringToSupportTable(
                            TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)),
                            TypeConverter.ToTwoChars(TypeConverter.DecToHex(AddressationType)),
                            Operand1,
                            ""
                        );

                        CORE.AddressCount = CORE.AddressCount + 2;
                        if (!CORE.MemoryCheck()) { return false; }

                        if (Operand2.Length > 0)
                            CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд команды" + arr_CodeTable[num, 0] + " не рассматривается.\r\n";
                    }

                    else
                    {
                        CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! Отрицательное число, либо превышено максимальное значение числа!\r\n";
                        return false;
                    }
                }
                else
                {
                    //если первый и второй операнд - регистры
                    if (TypeCheck.IsRegister(Operand1) && TypeCheck.IsRegister(Operand2))
                    {
                        if (CORE.AddressCount + 2 > MAX_MemmoryAdr)
                        {
                            CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                            return false;
                        }

                        //так как оба операнда регистры то это регистровая(регистровой==непосредственной) адресация
                        //просто  сдвигаем на два разряда влево
                        int AddressationType = TypeConverter.HexToDec(arr_CodeTable[num, 1]) * 4;
 
                        CORE.AddStringToSupportTable(
                            TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)),
                            TypeConverter.ToTwoChars(TypeConverter.DecToHex(AddressationType)),
                            Operand1,
                            Operand2
                        );

                        CORE.AddressCount = CORE.AddressCount + 2;
                        if (!CORE.MemoryCheck()) { return false; }
                    }
                    else
                    {
                        CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! Ошибка в команде " + arr_CodeTable[num, 0] + "!\r\n";
                        return false;
                    }

                }

            }

            //ДЛИНА КОМАНДЫ = 4
            else if (arr_CodeTable[num, 2] == "4")
            {
                if (TypeCheck.IsRegister(Operand1) || TypeCheck.IsDirective(Operand1) || CORE.FindCodeInCodeTable(Operand1, arr_CodeTable) != -1) {
                    CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! Операнд недопустим, использовано зарезервированное значение!\r\n";
                    return false;
                }

                if (CORE.AddressCount + 4 > MAX_MemmoryAdr)
                {
                    CORE.ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                    return false;
                }

                int AddressationType = TypeConverter.HexToDec(arr_CodeTable[num, 1]) * 4 + 1;

                CORE.AddStringToSupportTable(
                    TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.AddressCount)),
                    TypeConverter.ToTwoChars(TypeConverter.DecToHex(AddressationType)),
                    Operand1,
                    Operand2
                );

                CORE.AddressCount = CORE.AddressCount + 4;
                if (!CORE.MemoryCheck()) { return false; }

                if (Operand2.Length > 0)
                    CORE.ErrorMessage = CORE.ErrorMessage + "Предупреждение строка №" + (i + 1) + ". Второй операнд команды" + arr_CodeTable[num, 0] + " не рассматривается.\r\n";
            }
            else
            {
                CORE.ErrorMessage = "Ошибка строка №" + (i + 1) + "! Размер команды недопустим!\r\n";
                return false;
            }

            return true;
        }

        public bool GetRow(string[,] mas, int number, out string label, out string command, out string opr1, out string opr2)
        {
            label = mas[number, 0];
            command = mas[number, 1].ToUpper();
            opr1 = mas[number, 2];
            opr2 = mas[number, 3];

            if (TypeCheck.IsDirective(label) || TypeCheck.IsRegister(label))
            {   
                return false;
            }
            if (number > 0 && CORE.NameProg == label.ToUpper())
                return false;


            if ((TypeCheck.OnlySymbolsAndNumbers(label) || label.Length == 0) &&
                (TypeCheck.OnlySymbolsAndNumbers(command)) &&
                (TypeCheck.OnlySymbolsAndNumbers(opr2) || opr2.Length == 0))
            {
                if (label.Length > 0)
                {
                    if (TypeCheck.OnlySymbols(Convert.ToString(label[0])))
                        return true;
                    else
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
