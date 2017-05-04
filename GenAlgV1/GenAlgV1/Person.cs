using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;

namespace GenAlgV1
{
    public enum TypeCode { IntCode, RealCode }

    // Особь
    public class Person
    {
        private double Fitness; // Приспособленность функции

        private List<double> _numbers; // Параметры
        private List<string> _codeNumbers; // Закодированное число

        public TypeCode _type; // Тип кодирования данной особи

        // Выделяет память. Просто так
        public Person()
        {
            _numbers = new List<double>();
            _codeNumbers = new List<string>();
        }

        // Копируем все значения из входной последовательнсоти
        private void SetN(List<double> numbers)
        {
            _numbers = new List<double>();
            foreach (double t in numbers)
            {
                _numbers.Add(t);
            }
        }
        private void SetC(List<string> codenumbers)
        {
            _codeNumbers = new List<string>();

            foreach (string t in codenumbers)
            {
                _codeNumbers.Add(t);
            }
        }

        /// <summary>
        /// Добавляет цифирки и кодирует вещественным способом
        /// </summary>
        /// <param name="numbers">Последовательность цифирок</param>
        public void SetNumber(List<double> numbers)
        {
            _type = TypeCode.RealCode; // Задаём тип кодировки (хз зачем)

            SetN(numbers); // Запоминаем аргументы
            RealCode(); // Вещественное кодирование
        }
        /// <summary>
        /// Вещественное кодирование
        /// </summary>
        private void RealCode()
        {
            _codeNumbers = new List<string>(); // Выделяем память
            foreach (double t in _numbers)
            {
                _codeNumbers.Add(Convert.ToString(t)); // Копируем в строку
            }
        }

        /// <summary>
        /// Добавляет цифирки и производит целочисленное кодирование
        /// </summary>
        /// <param name="args">Аргументы кодирования</param>
        /// <param name="numbers">Последовательность цифирок</param>
        public void SetNumber(List<double> args, List<double> numbers)
        {
            _type = TypeCode.IntCode;
            SetN(numbers); // Сохраняем цифирки
            IntCode(args); // Кодируем
        }

        /// <summary>
        /// Добавляет цифирки и производит целочисленное кодирование
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="max">Максимальное значение</param>
        /// <param name="count">Количество разрядов</param>
        /// <param name="numbers">Цифирки</param>
        public void SetNumber(double min, double max, int count, List<double> numbers)
        {
            List<double> args = new List<double>(); args.Add(min); args.Add(max); args.Add(count); // Запихиваем аргументы в список
            SetNumber(args, numbers); // Вызываем предыдущий метод
        }

        // Целочисленное кодирование
        private void IntCode(List<double> args)
        {
            _codeNumbers = new List<string>();
            foreach (double t in _numbers)
            {
                _codeNumbers.Add(ActionCode(args, t)); // Действуем
            }
        }
        private string ActionCode(List<double> args, double t)
        {
            // Достаём аргументы
            double min = args[0];
            double max = args[1];
            int count = Convert.ToInt32(args[2]);

            string temp = "";

            int g = (int)((t - min) * (Math.Pow(2, count) - 1) / (max - min)); // Вычисляем штуку

            // Переводим в двоичный код
            for (int i = 0; i < count; i++)
            {
                temp += g - (int)(g / 2) * 2;
                g = (int)g / 2;
            }

            return temp; // Возвращаем двоичный код аргумента
        }

        /// <summary>
        /// Задаёт закодированную последовательность и раскодирует её при вещественном кодировании
        /// </summary>
        /// <param name="codeNumb"></param>
        public void SetCodeNumber(List<string> codeNumb)
        {
            _type = TypeCode.RealCode;

            SetC(codeNumb); // Запоминаем закодированные цифирки
            RealDeCode();  // Достаём
        }
        private void RealDeCode()
        {
            _numbers = new List<double>();

            foreach (string t in _codeNumbers)
            {
                _numbers.Add(Convert.ToDouble(t));
            }
        }


        public void SetCodeNumber(List<double> args, List<string> codeNumb)
        {
            SetC(codeNumb);
            IntDeCode(args);
        }

        public void SetCodeNumber(double min, double max, int count, List<string> codeNumb)
        {
            List<double> args = new List<double>(); args.Add(min); args.Add(max); args.Add(count);
            SetCodeNumber(args, codeNumb);
        }

        private void IntDeCode(List<double> args)
        {
            _numbers = new List<double>();

            foreach (string t in _codeNumbers)
            {
                _numbers.Add(ActionDecode(t, args));
            }
        }

        private double ActionDecode(string t, List<double> args)
        {
            double min = args[0];
            double max = args[1];
            int count = Convert.ToInt32(args[2]);

            List<char> k = t.ToList(); // Поменяли в строку

            // Переводим из двоичной в число
            double temp = 0;

            for (int i = 0; i < k.Count; i++)
            {
                if (k[i] == '1') temp += 1 * Math.Pow(2, i);
            }

            // Имеем число

            return temp * (max - min) / (Math.Pow(2, count) - 1) + min;
        }

        public void FitnessCalc(string func)
        {
            Parser temp = new Parser();

            this.Fitness = Convert.ToDouble(temp.Parse(func, _numbers));
        }

        public List<double> GetArgs()
        {
            return _numbers;
        }
        public List<string> GetCodeArtgs()
        {
            return _codeNumbers;
        }
        public double GetFitness()
        {
            return Fitness;
        }


        public void Inverse(int Arg, int Bit, List<double> args)
        {
            List<string> newString = new List<string>();

            for (int i = 0; i < _codeNumbers.Count; i++)
            {
                if (i != Arg) newString.Add(_codeNumbers[i]); // До текущего гена
                else // Дошли до нужного гена
                {
                    string temp = "";
                    for (int j = 0; j < _codeNumbers[i].Length; j++)
                    {
                        if (i != Bit) temp += _codeNumbers[i][j];
                        else
                        {
                            if (_codeNumbers[i][j] == '1') temp += "0";
                            else temp += "1";
                        }
                    }
                    newString.Add(temp);
                }
            }

            SetCodeNumber(args, newString); // Сохраняем строку
        }
    }
}
