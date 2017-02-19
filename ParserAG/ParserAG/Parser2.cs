using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAG
{
    class Parser2
    {
        /// <summary>
        /// Парсим функцию без X
        /// </summary>
        /// <param name="func">Входная строка</param>
        /// <returns>Ответ</returns>
        public double Parse(string func)
        {
            List<string> _PolandList = new List<string>();
            string _func = func;

            _func = _func.Replace(" ", ""); // Удаляем пробелы
            _func = _func.Replace(".", ","); // Удаляем точки
            _func = _func.Replace("//", ":"); // Костыль для красоты деления

            _PolandList = ToPoland(_func); // Переводит строку в польскую запись

            string answ = ActionParse(_func, _PolandList);

            return Convert.ToDouble(answ);
        }


        private List<string> ToPoland(string func)
        {
            string curstring = func;
            string cursyb;

            Stack<string> TempStack = new Stack<string>();
            List<string> outList = new List<string>();

            do
            {
                if (curstring.Length == 0) // Если кончились символы
                {
                    while (TempStack.Count != 0)
                    {
                        outList.Add(TempStack.Pop()); // Выталкивам всё из стека
                    }

                    break; // Выходим
                }
                else // Если символы есть
                {
                    cursyb = Convert.ToString(curstring[0]); // Читаем очедной символ

                }

            } while (true);
        } 
    }
}
