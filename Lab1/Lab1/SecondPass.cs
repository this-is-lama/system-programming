using Lab1.Checks;

namespace Lab1
{
    class SecondPass
    {
        Core CORE;
        public SecondPass(Core core)
        {
            CORE = core;
        }

        public bool SecondPassFunc(ListBox BinaryCode)
        {
            CORE.ErrorMessage = "";
            //запускаем его для каждой строки Вспомогательной таблицы
            for (int i = 0; i < CORE.SupportTable[0].Count; i++)
            {
                string address = CORE.SupportTable[0][i];
                string MKOP = CORE.SupportTable[1][i];
                string operand1 = CORE.SupportTable[2][i];
                string operand2 = CORE.SupportTable[3][i];

                //Если строка первая, то это директива Старт
                if (i == 0)
                {
                    string str = TypeConverter.EditingString(
                        "H",
                        CORE.SupportTable[0][0], CORE.SupportTable[2][0],
                        "",
                        Convert.ToString(CORE.AddressCount - CORE.StartAddress),
                        ""
                    );
                    BinaryCode.Items.Add(str);
                }
                //Если строка не первая, то снова смотрим, команда там или директива. Интересуют RESB и RESW, т.к. их значение операндов отражается только в длинне записи
                else
                {
                    int error, label1, label2;

                    string result1 = CheckingOperandSecondPass(operand1, out error, out label1);

                    if (error == 1)
                    { CORE.ErrorMessage = CORE.ErrorMessage + "Ошибка строка №" + (i + 1) + ". Ошибка в операнде, код отсутствует в ТСИ " + operand1 + "\r\n"; break; }

                    string result2 = CheckingOperandSecondPass(operand2, out error, out label2);

                    if (error == 1)
                    { CORE.ErrorMessage = CORE.ErrorMessage + "Ошибка строка №" + (i + 1) + ". Ошибка в операнде, код отсутствует в ТСИ " + operand2 + "\r\n"; break; }

                    if (TypeCheck.IsDirective(MKOP) == true)
                    {
                        if (MKOP == "RESB")
                        {
                            MKOP = "";
                            string str1 = TypeConverter.EditingString("T", address, MKOP, result1, "", "");
                            BinaryCode.Items.Add(str1);
                            continue;
                        }

                        if (MKOP == "RESW")
                        {
                            MKOP = "";
                            string str2 = TypeConverter.EditingString("T", address, MKOP, TypeConverter.ToTwoChars(TypeConverter.DecToHex(Convert.ToInt32(operand1) * 3)), "", "");
                            BinaryCode.Items.Add(str2);
                            continue;
                        }

                        if (MKOP == "BYTE" && (operand1 == "?" || TypeCheck.OnlyNumbers(operand1)))
                        {
                            MKOP = "";
                            string str2 = TypeConverter.EditingString("T", address, MKOP, TypeConverter.ToTwoChars(TypeConverter.DecToHex(1)), operand1 != "?" ? TypeConverter.ToTwoChars(TypeConverter.DecToHex(int.Parse(operand1))) : "", "");
                            BinaryCode.Items.Add(str2);
                            continue;
                        }

                        if (MKOP == "BYTE")
                        {
                            MKOP = "";
                            string str2 = TypeConverter.EditingString("T", address, MKOP, TypeConverter.ToTwoChars(TypeConverter.DecToHex(result1.Length / 2)), result1, result2);
                            BinaryCode.Items.Add(str2);
                            continue;
                        }

                        if (MKOP == "WORD")
                        {
                            MKOP = "";
                            result1 = TypeConverter.ToSixChars(result1);
                            string str2 = TypeConverter.EditingString("T", address, MKOP, TypeConverter.ToTwoChars(TypeConverter.DecToHex(result1.Length / 2)), result1, result2);
                            BinaryCode.Items.Add(str2);
                            continue;
                        }

                        if (MKOP == "WORD" && operand1 == "?")
                        {
                            MKOP = "";
                            string str2 = TypeConverter.EditingString("T", address, MKOP, TypeConverter.ToTwoChars(TypeConverter.DecToHex(3)), "", "");
                            BinaryCode.Items.Add(str2);
                            continue;
                        }
                    }
                    else
                    {
                        // Проверяем что команда работает с тем, что разрешено адресацией
                        // сначала смотрим на тип адресации, если там  01 , значит это прямая
                        //и в операндах может быть только метка
                        int Type_of_adr = (byte)TypeConverter.HexToDec(MKOP) & 0x03;
                        if (Type_of_adr == 1)
                        {
                            if (label1 != 1)
                            {
                                CORE.ErrorMessage = CORE.ErrorMessage + "Ошибка строка №" + (i + 1) + "! Для данного типа адресации операнд должен быть меткой " + MKOP + "!\r\n";
                                BinaryCode.Items.Clear();
                                return false;

                            }
                            if (result2 != "")
                            {
                                CORE.ErrorMessage = CORE.ErrorMessage + "Ошибка строка №" + (i + 1) + "! Данный тип адрессации поддерживает 1 операнд " + "!\r\n";
                                BinaryCode.Items.Clear();
                                return false;
                            }
                        }

                        String RecordLength = TypeConverter.ToTwoChars(TypeConverter.DecToHex((MKOP.Length + result1.Length + result2.Length)/2));
                        string str5 = TypeConverter.EditingString("T", address, MKOP, RecordLength, result1, result2);
                        BinaryCode.Items.Add(str5);
                    }
                }
            }
            string str3 = TypeConverter.EditingString("E", TypeConverter.ToSixChars(TypeConverter.DecToHex(CORE.EndAddress)), "", "", "", "");
            BinaryCode.Items.Add(str3);

            if (CORE.ErrorMessage != "")
                BinaryCode.Items.Clear();

            return true;
        }

        //проверка опреанда во втором проходе
        //Если в операнде метка - возращает адрес метки
        //Если в операнде регистр - возвращает номер регистра
        //Если там строка типа C"????" - возвращает ASCII код
        //Если там строка типа X"????" - возвращает строку
        //Если что-то в 10ричном формате - то вернет это же число в 16ричном формате
        //иначе возращает пустую строку
        public string CheckingOperandSecondPass(string operand1, out int error, out int label)
        {
            label = 0;
            string result = "";
            error = 0;
            if (operand1 != "")
            {
                //если там метка - то возвращаем адрес метки
                int LabelStringNum = CORE.FindLabelInLabelTable(operand1);
                if (LabelStringNum > -1)
                {
                    label = 1;
                    return result = CORE.SymbolNameTable[1][LabelStringNum];
                }
                else
                {
                    //если в операнде регистр
                    int regnum = TypeCheck.RegisterNumber(operand1);
                    if (regnum > -1)
                    {
                        return result = TypeConverter.DecToHex(regnum);
                    }
                    else
                    {
                        //если в операнде только цифры
                        if (TypeCheck.OnlyNumbers(operand1))
                        {
                            return result = TypeConverter.ToTwoChars(TypeConverter.DecToHex(Convert.ToInt32(operand1)));
                        }
                        else
                        {
                            string sentence = "";
                            sentence = TypeCheck.IsString(operand1);
                            if (sentence != "")
                            {
                                return TypeConverter.ToASCII(sentence);
                            }
                            sentence = TypeCheck.ByteString(operand1);
                            if (sentence != "")
                            {
                                return sentence;
                            }

                            if (operand1 == "?") {
                                return "";
                            }
                            
                            //Если перепробованы все комбинации, значит ошибка
                            error = 1;
                        }
                    }
                }
            }

            return result;
        }
    }
}
