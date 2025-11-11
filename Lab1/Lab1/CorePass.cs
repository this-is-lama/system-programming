using Lab1.Checks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1
{
    class Core
    {
        public string NameProg = "";
        public string ErrorMessage = "";
        public int EndAddress = 0;
        public int StartAddress = 0;
        public int AddressCount = 0;

        const int MAX_MemmoryAdr = 16777215;

        //| Метка| МКОП | Операнд1 | Операнд2 | Вспомогательная таблица
        public List<List<string>> SupportTable = new List<List<string>>();

        //| Метка| Адрес | Таблица символических имен
        public List<List<string>> SymbolNameTable = new List<List<string>>();

        //Таблица исходного кода
        public List<string> BinaryCode = new List<string>();

        public bool MemoryCheck()
        {
            if (AddressCount < 0 || AddressCount > MAX_MemmoryAdr)
            {
                ErrorMessage = "Ошибка! Выход за границы доступной памяти. (16777215 или 0xFFFFFF)\n";
                return false;
            }
            return true;
        }

        public void AddStringToSupportTable(string n1, string n2, string n3, string n4)
        {
            SupportTable[0].Add(n1);
            SupportTable[1].Add(n2);
            SupportTable[2].Add(n3);
            SupportTable[3].Add(n4);
        }

        public int FindLabelInLabelTable(string label)
        {
            for (int i = 0; i < SymbolNameTable[0].Count; i++)
            {
                if (label == SymbolNameTable[0][i])
                    return i;
            }
            return -1;
        }

        public int FindCodeInCodeTable(string label, string[,] code_table)
        {
            for (int i = 0; i < code_table.GetLength(0); i++)
            {
                if (label == code_table[i, 0])
                    return i;
            }
            return -1;
        }
        static readonly Regex CConcatFull = new(
        @"^[CX]""(?:[^""]|"""")*""(?:\s*""(?:[^""]|"""")*"")*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string[,] ParseTextBox(TextBox txt, int columnCount)
        {
            if (columnCount <= 0) throw new ArgumentOutOfRangeException(nameof(columnCount));

            var rows = new List<string[]>();
            var rx = new Regex(
                @"[CX]""(?:[^""]|"""")*""|" +  
                @"""(?:[^""]|"""")*""|" +      
                @"[^,\s]+",                    
                RegexOptions.Compiled
            );

            foreach (var rawLine in txt.Lines)
            {
                if (string.IsNullOrWhiteSpace(rawLine)) continue;

                var matches = rx.Matches(rawLine);
                var tokens = new List<string>(matches.Count);
                for (int i = 0; i < matches.Count; i++) {
                    var t = matches[i].Value.Trim();
                    if (t.Length == 0) continue;
                    if (t.StartsWith("C\"", StringComparison.OrdinalIgnoreCase))
                    {
                        var sb = new System.Text.StringBuilder(t);

                        while (t.EndsWith("\"", StringComparison.Ordinal) && i + 1 < matches.Count)
                        {
                            i++;
                            t = matches[i].Value.Trim();
                            sb.Append(t);
                        }

                        while (i + 1 < matches.Count && (matches[i + 1].Value.Trim().StartsWith("\"") || matches[i + 1].Value.Trim().EndsWith("\"")))
                        {
                            i++;
                            sb.Append(matches[i].Value.Trim());
                        }

                        tokens.Add(sb.ToString());
                    }
                    else
                    {
                        tokens.Add(t.ToUpperInvariant());
                    }

                }

                if (tokens.Count == 0) continue;

                var row = new string[columnCount];
                for (int i = 0; i < columnCount; i++) row[i] = string.Empty;

                bool startsWithWhitespace = char.IsWhiteSpace(rawLine[0]);

                if (startsWithWhitespace)
                {
                    // | "" | MKOP | OP1 | OP2 |
                    if (columnCount > 1 && tokens.Count > 0) row[1] = tokens[0];
                    if (columnCount > 2 && tokens.Count > 1) row[2] = tokens[1];
                    if (columnCount > 3 && tokens.Count > 2) row[3] = tokens[2];
                }
                else
                {
                    // | LABEL | MKOP | OP1 | OP2 |
                    row[0] = tokens.ElementAtOrDefault(0) ?? "";
                    if (columnCount > 1) row[1] = tokens.ElementAtOrDefault(1) ?? "";
                    if (columnCount > 2) row[2] = tokens.ElementAtOrDefault(2) ?? "";
                    if (columnCount > 3) row[3] = tokens.ElementAtOrDefault(3) ?? "";
                }

                rows.Add(row);
            }

            var result = new string[rows.Count, columnCount];
            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < columnCount; j++)
                    result[i, j] = rows[i][j] ?? "";

            return result;
        }

        public bool CheckOperationCodeTable(ref string[,] OperationCodeTable)
        {
            int rows = OperationCodeTable.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                // Проверка поля ИМЕНИ КОМАНДЫ (1 столбец)
                if (OperationCodeTable[i, 0] == "" || OperationCodeTable[i, 1] == "" || OperationCodeTable[i, 2] == "")
                {
                    ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Пустая ячейка в ТКО.\n";
                    return false;
                }

                if (OperationCodeTable[i, 0].Length > 6 || OperationCodeTable[i, 1].Length > 2 || OperationCodeTable[i, 2].Length > 1)
                {
                    ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Ошибка в размере строки в таблице кодов операций (Команда (от 1 до 6), МКОП (от 1 до 2), Длина(не более 1)).\n";
                    return false;
                }

                if (!TypeCheck.OnlySymbolsAndNumbers(OperationCodeTable[i, 0]))
                {
                    ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! В поле команды не допустимые символы.\n";
                    return false;
                }

                if (TypeCheck.IsDirective(OperationCodeTable[i, 0]) || TypeCheck.IsRegister(OperationCodeTable[i, 0]))
                {
                    ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Поле  команды является зарезервированным словом.\n";
                    return false;
                }

                for (int k = i + 1; k < rows; k++)
                {
                    string cmp_str1 = OperationCodeTable[i, 0];
                    string cmp_str2 = OperationCodeTable[k, 0];
                    if (Equals(cmp_str1, cmp_str2))
                    {
                        ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Найдены одинаковые команды.\n";
                        return false;
                    }
                }
                // -----------------------------------


                //Проверка поля МКОП КОМАНДЫ (2 стоблец)
                if (TypeCheck.IsHEX(OperationCodeTable[i, 1]))
                {
                    int count = TypeConverter.HexToDec(OperationCodeTable[i, 1]);
                    if (count > 63)
                    {
                        ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Поле код команды не должен превышать 3F.\n";
                        return false;
                    }
                    else
                    {
                        if (OperationCodeTable[i, 1].Length == 1)
                            OperationCodeTable[i, 1] = TypeConverter.ToTwoChars(OperationCodeTable[i, 1]);
                    }
                }
                else
                {
                    ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Посторонние символы в поле МКОП.\n";
                    return false;
                }

                for (int k = i + 1; k < rows; k++)
                {
                    string str1 = Convert.ToString(TypeConverter.HexToDec(OperationCodeTable[i, 1]));
                    string str2 = Convert.ToString(TypeConverter.HexToDec(OperationCodeTable[k, 1]));
                    if (Equals(str1, str2))
                    {
                        ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Найдены совпадения в МКОП КОМАНДЫ.\n";
                        return false;
                    }
                }
                // -----------------------------------


                //Проверка поля РАЗМЕРА КОМАНДЫ (3 столбец)
                int value = 0;
                if (TypeCheck.OnlyNumbers(OperationCodeTable[i, 2]))
                {
                    bool result = Int32.TryParse(OperationCodeTable[i, 2], out value);
                    if (result)
                    {
                        if (!new[] { 1, 2, 4 }.Contains(value))
                        {
                            ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! Ошибка в размере команды (допустимо от 1, 2, 4).\n";
                            return false;
                        }
                    }
                }
                else
                {
                    ErrorMessage = "Ошибка ТКО № " + (i + 1) + "! В поле размер команды недопустимый символ.\n";
                    return false;
                }
                // -----------------------------------
            }
            return true;
        }


    }
}
