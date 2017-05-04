using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAlgV1
{
    /// <summary>
    /// Популяция
    /// </summary>
    public class Population
    {
        public List<Person> _population = new List<Person>(); // Список особей
        public TypeCode _type; // Тип кодирования
        public int _CountPersons { get; set; }

        private string _func="";

        private Random T;

        // Инициализирование пустой особи
        public Population()
        {
            _population = new List<Person>();
            _CountPersons = 0;

            T = new Random();
        }
        public Population(string func)
        {
            _func = func;
        }

        // Инициализация рандомной популяции с заданным кодированием
        public void InitPopulation(TypeCode type, List<double> args, int maxCountPerson, string func, int countArg)
        {
            _func = func;
            _type = type;

            for (int i = 0; i < maxCountPerson; i++)
            {
                Person temp = new Person();

                // Генерим рандомные параметры и пихаем создаём особь
                if (_type == TypeCode.IntCode) temp.SetNumber(args, InitRandArgs(args[0], args[1], countArg));
                else temp.SetNumber(InitRandArgs(args[0], args[1], countArg));

                temp.FitnessCalc(_func); // Считаем приспособленность

                _population.Add(temp); // Добавляем в популяцию
            }

            _CountPersons = _population.Count();
        }
        /// <summary>
        /// Генерирует параметры
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="max">Максимальное значение</param>
        /// <param name="countArg">Количество аргументов</param>
        /// <returns>Сгенерированное число</returns>
        private List<double> InitRandArgs(double min, double max, int countArg)
        {
            List<double> tempD = new List<double>();
            for (int i = 0; i < countArg; i++)
            {
                tempD.Add(T.Next((int)min, (int)(max + 1)) * T.NextDouble()); // Собираем цифирки
            }
            return tempD;
        }

        // Добавляет особь в популяцию
        public void AddPerson(Person temp)
        {
            if(_func!="") temp.FitnessCalc(_func);
            _population.Add(temp);

            _CountPersons = _population.Count;
        }

        /// <summary>
        /// Создаёт особь с вещественным кодированием и добавляет в популяцию
        /// </summary>
        /// <param name="temp">Аргументы</param>
        public void AddPerson(List<double> temp)
        {
            Person answ = new Person();
            answ.SetNumber(temp);
            if (_func != "") answ.FitnessCalc(_func);

            _population.Add(answ);

            _CountPersons = _population.Count;
        }
        /// <summary>
        /// Создаёт особь с целочисленным кодированием и пихает в популяцию
        /// </summary>
        /// <param name="args">Аргументы кодирования</param>
        /// <param name="temp">Аргументы числа</param>
        public void AddPerson(List<double> args, List<double> temp)
        {
            Person answ = new Person();
            answ.SetNumber(args, temp);
            if (_func != "") answ.FitnessCalc(_func);

            _population.Add(answ);

            _CountPersons = _population.Count;
        }

        public void CalcFitnessAll()
        {
            if (_func != "")
            {
                foreach (Person k in _population)
                {
                    k.FitnessCalc(_func);
                }
            }
        }
        public void CalcFitnessAll(string func)
        {
            _func = func;
            CalcFitnessAll();
        }

        public Person GetPerson(int i)
        {
            return _population[i];
        }

        public void Sort()
        {
            _population.OrderBy(x => x.GetFitness());
        }
    }
}
