using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAG
{
    public class Parser
    {
        public string _func { get; private set; } // Входная функция
        // public string _poland { get; private set; } // Польская запись
        public List<string> _polandList { get; private set; }
        
        // Конструкторы
        public Parser()
        {
            _polandList = new List<string>();
            _func = null;
        }
        public Parser(string function)
        {
            _polandList = new List<string>();
            this._func = function;
            ToPoland(this._func);
        }


        public bool ErrorCheck(string syb)
        {
            string temp = "^ERROR";

            Regex tmp = new Regex(temp);

            return tmp.IsMatch(syb);
        }

        // Перевод в польскую запись
        private void ToPoland(string func)
        {
            string temp = func;
            string cursyb;

            Stack<string> TempStack = new Stack<string>(); // Стэк
            // string tempout = ""; // Выходная строка

            while (true)
            {
                if (temp.Length == 0) // Если кончились символы
                {
                    while (TempStack.Count != 0)
                    {
                        _polandList.Add(TempStack.Pop()); // Выталкиваем всё из стека
                        // tempout += TempStack.Pop(); // Выталкиваем всё из стека
                    }
                    break;
                }

                cursyb = Convert.ToString(temp[0]); // Читаем очередной символ


                if (NumberCheck(cursyb) == true) { ActionNumber(ref temp); }// Если входной символ - число
                if (FuncCheck(cursyb) == true) { ActionFunc(cursyb, ref TempStack); temp = temp.Remove(0, 1); }// Если входной символ - символ функции
                if (cursyb == "(") { TempStack.Push(cursyb); temp = temp.Remove(0, 1); }// Если символ - открывающая скобка
                if (cursyb == ")") { ActionBraket(ref TempStack); temp = temp.Remove(0, 1); }
                if (OperatorCheck(cursyb)) { ActionOperator(cursyb, ref TempStack); temp = temp.Remove(0, 1); }

                //Console.WriteLine(temp);

                if (TempStack.Count() != 0)
                {
                    if (ErrorCheck(TempStack.Peek()))
                    {
                        _polandList.Add(TempStack.Peek());
                        return;
                    }
                }

                if (_polandList.Count != 0)
                {
                    if (ErrorCheck(_polandList.Last())) return;
                }
            }

            // this._poland = tempout;
        }


        private string printstack(Stack<string> stack)
        {
            string temp = "";
            foreach (string v in stack)
            {
                temp += v;
            }
            return temp;
        }
        public string printlist()
        {
            string temp = "";

            foreach (string t in _polandList)
            {
                temp += t;
            }

            return temp;
        }

        // ЕСЛИ ВХОДНОЙ СИМВОЛ - ЧИСЛО

        /// <summary>
        /// Проверяет, является ли символ числом
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private bool NumberCheck(string temp)
        {
            if (temp == "0") return true;

            string Reg = @"[\d\.\,]"; // Является ли символ числом
            Regex tmp = new Regex(Reg);

            return tmp.IsMatch(temp);
        }
        /// <summary>
        /// Действие, если символ число
        /// </summary>
        /// <param name="allstring">Функция</param>
        private void ActionNumber(ref string allstring)
        {
            string mystring;

            int i = 0;

            mystring = Convert.ToString(allstring[0]);
            allstring = allstring.Remove(0, 1);

            if (allstring.Length != 0)
            {
                // Если следущий символ тоже число
                if (NumberCheck(Convert.ToString(allstring[i])))
                {
                    do
                    {
                        // Пока символ число или точка (десятичное число)
                        if ((NumberCheck(Convert.ToString(allstring[i])) == false)) break;

                        // запоминаем
                        mystring += Convert.ToString(allstring[i]);
                        allstring = allstring.Remove(0, 1);
                        if (allstring.Length == 0) break;
                    } while (true);
                }
            }

            // Если конечный символ число, то возвращаем всё
            if (NumberCheck(mystring))
            {
                //temp += mystring;
                _polandList.Add(mystring);
            }// Добавляем к выходной строке
            else
            {
                _polandList.Add("ERROR -> Syb is not number"); // Потом запилить ошибку
                return;
            }
        }

        // ЕСЛИ ВХОДНОЙ СИМВОЛ - ФУНКЦИЯ
        private bool FuncCheck(string temp)
        {
            string Reg = @"[abcd]"; // Является ли символ числом // Cos, Sin, Exp
            Regex tmp = new Regex(Reg);

            return tmp.IsMatch(temp);
        }
        private void ActionFunc(string syb, ref Stack<string> temp)
        {
            temp.Push(syb); // Помещаем её в стек
        }

        // ЕСЛИ ЗАКРЫВАЮЩАЯ СКОБКА
        private void ActionBraket(ref Stack<string> st)
        {
            while (true)
            {
                if (st.Peek() == "(")
                {
                    st.Pop();
                    break;
                }
                else
                {
                    _polandList.Add(st.Pop());
                    if (st.Count() == 0)
                    {
                        _polandList.Add("ERROR! -> Неправильная расстановка скобок");
                        return;
                    }
                   // outstr += st.Pop();
                }
            }
        }

        // ЕСЛИ ОПЕРАТОР
        private bool OperatorCheck(string syb)
        {

            if (syb == "//") return true;

            string tmp = @"[\+:\\*\-\^cse]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool CheckRight(string syb)
        {
            string tmp = @"[^]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool CheckLeft(string syb)
        {
            string tmp = @"[\+/:\*\-\^]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool Prior6(string syb)
        {
            string tmp = @"[ab]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool Prior5(string syb)
        {
            string tmp = @"[esc]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool Prior4(string syb)
        {
            string tmp = @"[\^]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool Prior3(string syb)
        {
            if (syb == "//") return true;

            string tmp = @"[:\*]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool Prior2(string syb)
        {
            string tmp = @"[\-\+]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }
        private bool Prior1(string syb)
        {
            string tmp = @"[()]";
            Regex reg = new Regex(tmp);

            return reg.IsMatch(syb);
        }

        private int GetPrior(string syb)
        {
            if (Prior1(syb)) return 1;
            if (Prior2(syb)) return 2;
            if (Prior3(syb)) return 3;
            if (Prior4(syb)) return 4;
            if (Prior5(syb)) return 5;
            if (Prior6(syb)) return 6;
            else return int.MaxValue;
        }
        private void ActionOperator(string syb, ref Stack<string> st)
        {
            if (GetPrior(syb) == int.MaxValue)
            {
                _polandList.Add(("ERROR! -> Operator" + syb + " Not Foud"));
                return;
            }

            if (st.Count != 0)
            {
                do
                {
                    // if (CheckRight(syb) && (GetPrior(syb) >= GetPrior(st.Peek)) break; // Правых вродь нет
                    if (/*CheckLeft(syb) && */(GetPrior(syb) > GetPrior(st.Peek()))) { break; }

                    _polandList.Add(st.Pop());

                    if (st.Count() == 0) break;
                } while (true);
            }
            st.Push(syb);
        }


        // ТУТ УЖЕ ПАРСИМ
        private string ActionParse()
        {
            string cursyb;
            // temp = this._poland;

            Stack<string> TempStack = new Stack<string>();
            TempStack.Push("0");

            do
            {
                if (_polandList.Count()==0) break;
                cursyb = Convert.ToString(_polandList[0]);
               /* //Console.WriteLine("Считан символ " + cursyb);
                //Console.WriteLine("Строка: " + printlist());
                //Console.WriteLine("Стек: " + printstack(TempStack));*/

                if ((FuncCheck(cursyb)) || (NumberCheck(cursyb))) TempStack.Push(cursyb);
                if (OperatorCheck(cursyb)) Operation(ref TempStack, cursyb);

                _polandList.RemoveAt(0);

                /*//Console.WriteLine("Символ " + cursyb + " обработан");
                //Console.WriteLine("Строка: " + printlist());
                //Console.WriteLine("Стек: " + printstack(TempStack));
                //Console.ReadLine();*/



            } while (true);

            return TempStack.Pop();
        }
        private void Operation(ref Stack<string> temp, string syb)
        {
            double a, b, c=0;

            b = Convert.ToDouble(temp.Pop());
            a = Convert.ToDouble(temp.Pop());

            switch (syb)
            {
                case "+":
                    {
                        c = a + b;
                        break;
                    }
                case "-":
                    {
                        c = a - b;
                        break;
                    }
                case ":":
                    {
                        c = a / b;
                        break;
                    }
                case "/":
                    {
                        c = a / b;
                        break;
                    }
                case "^":
                    {
                        c = Math.Pow(a, b);
                        break;
                    }
                case "*":
                    {

                        c = a * b;
                        break;
                    }
                default:
                    {
                        temp.Push("ERROR! -> Operator is not parse");
                        break;
                    }
            }

            temp.Push(Convert.ToString(c));
        }

        public string Parse(string func)
        {
            string temp = "";

            this._func = func; // Инициализируем функцию

            _polandList = null;
            _polandList = new List<string>(); // Инициализируем выходную строку

            this._func = this._func.Replace(" ", ""); // Удаляем пробелы

            ToPoland(func); // Переводим в польскую запись

            if (ErrorCheck(_polandList.Last())) return _polandList.Last();

            temp = ActionParse(); // Парсим

            return temp;
        }


        public string Parse(string func, List<double> temp)
        {
            //Console.WriteLine("OldString:" + func);

            func = func.Replace(" ", ""); // Удаляем пробелы

            //Console.WriteLine("Del space" + func);

            List<string> tempList = new List<string>();

            Regex reg = new Regex(@"[X|x]{1}[0-9]");

            foreach (Match t in reg.Matches(func))
            {
                tempList.Add(t.Value);
            }

            //Console.WriteLine("Mathccont");

            IEnumerable<string> returnS = tempList.Distinct();
            returnS = returnS.OrderBy(x => x);

            //Console.WriteLine("Oreder");

            int i = 0;

            foreach (string t in returnS)
            {
                //Console.WriteLine("T = " + t); //Console.WriteLine("Temp[i] = " + temp[i]);

                if (temp[i] >= 0)
                {
                    if ((Math.Abs(temp[i]) <= Math.Pow(10, -4))&&(temp[i]!=0)) func = func.Replace(t, NewString(temp[i]));
                    else func = func.Replace(t, Convert.ToString(temp[i]));
                }
                else
                {
                    if (Math.Abs(temp[i]) <= Math.Pow(10, -4)) func = func.Replace(t, "(0-"+NewString(temp[i])+")");
                    else func = func.Replace(t, "(0-" + Convert.ToString(Math.Abs(temp[i]) + ")"));

                }
                i++;
                //Console.WriteLine("NewStr1" + func);
            }



            //Console.WriteLine("NewString" + func);
            ////Console.ReadKey();*/

            return Parse(func);
        }

        private string NewString(double ch)
        {
            int t = 1;
            double temp = Math.Abs(ch);

            do
            {
                if (temp * Math.Pow(10, t) >= 1)
                {
                    temp = temp * Math.Pow(10, t);
                    break;
                }
                else t++;

            } while (true);

            string sout = temp + "*" + "10^" + t;

            return sout;
        }

        public double DiffFunc(string func, List<double> temp, int i)
        {
            List<double> k = new List<double>();

            for (int j = 0; j < temp.Count; j++) k.Add(temp[j]);

            //Console.Write("temp1 = ("); for (int j = 0; j < temp.Count; j++) //Console.Write(k[j] + ";");
            //Console.WriteLine();

            double fd = Convert.ToDouble(Parse(func, k));
            k[i] = k[i] + Math.Pow(10, -8);
            
            //Console.Write("temp2 (temp+h) = ("); for (int j = 0; j < temp.Count; j++) //Console.Write(k[j] + ";");
            //Console.WriteLine();

            double fd0 = Convert.ToDouble(Parse(func, k));
            //Console.WriteLine("Запарсили");

            return (fd0 - fd) / Math.Pow(10, -8);
        }

        // Надо сначала во все функции внести возврат ERROR. Если ERROR, то сразу останавливаем прогу и выдаём ошибку.
        // Потом CheckParse проверяет введённую функцию, если ошибка - то ERROR
        // Если нет ошибки - то количество разных X
        public string CheckParse(string func)
        {
            // Считаем X
            List<string> tempList = new List<string>();

            Regex reg = new Regex(@"[X|x]{1}[0-9]");

            foreach (Match t in reg.Matches(func))
            {
                tempList.Add(t.Value);
            }

            IEnumerable<string> returnS = tempList.Distinct();

            returnS = returnS.OrderBy(x => x);

            int i = 0;

            foreach (string t in returnS)
            {
                func = func.Replace(t, Convert.ToString("1"));
                i++;
            }

            ToPoland(func);

            if (ErrorCheck(_polandList.Last())) return _polandList.Last();
            else return Convert.ToString(returnS.Count());
        }
    }
}
