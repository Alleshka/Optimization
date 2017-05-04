using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace GenAlgV1
{
    // Хранится инфа для вывода
    public class LogClass
    {
        public double MinF { get; set; } // Минимальное значение функции
        public double Average { get; set; } // Среднее значение функции

        public string RealLog { get; set; } // Держит в себе строку с популяцией
        public string CodeLog { get; set; } // Держит в себе строку с кодом

        public Population _temp;

        public LogClass(Population temp)
        {
            MinF = GetMinValue(temp);
            Average = GetAverageValue(temp);

            SetPopul(temp);
        }

        private double GetMinValue(Population temp)
        {
            return temp._population.OrderBy(x => x.GetFitness()).First().GetFitness();
        }
        private double GetAverageValue(Population temp)
        {
            double num = 0;
            foreach (Person k in temp._population)
            {
                num += k.GetFitness();
            }
            return num / temp._population.Count;
        }

        public void PrintLog(bool order, int around)
        {
            IEnumerable<Person> temp2;

            if (order == true) temp2 = _temp._population.OrderBy(x => x.GetFitness());
            else temp2 = _temp._population;

            RealLog = "";
            CodeLog = "";

            foreach (Person t in temp2)
            {
                RealLog += "(";
                foreach (double k in t.GetArgs())
                {
                    RealLog += Math.Round(k, around) + "; ";
                }
                RealLog += "); "; RealLog += "F = " + Math.Round(t.GetFitness(), around);
                RealLog += Environment.NewLine;

                CodeLog += "(";
                foreach (string k in t.GetCodeArtgs())
                {
                    CodeLog += k + "; ";
                }
                CodeLog += "); "; CodeLog += "F = " + Math.Round(t.GetFitness(), around);
                CodeLog += Environment.NewLine;
            }
        }

        public void SetPopul(Population temp)
        {
            _temp = new Population();
            foreach (Person k in temp._population)
            {
                _temp.AddPerson(k);
            }
        }
    }
    public class AnswerClass
    {
        public List<LogClass> log = new List<LogClass>();
        public List<double> minPoint = new List<double>();
        public double MinF;

        public AnswerClass()
        {
            MinF = 0;
            log = new List<LogClass>();
            minPoint = new List<double>();
        }

        public void AddLog(Population k)
        {
            log.Add(new LogClass(k));
        }
        public void SetAnswer(List<double> min, double f)
        {
            foreach (double k in min)
            {
                minPoint.Add(k);
            }
            this.MinF = f;
        }
    }

    public class GenAlgWorck
    {
        AnswerClass answ = new AnswerClass();

        private Population _population;

        private string _func; // Оптимизируемая функция
        private int _countArg;
        private Parser _Parse; // Парсер

        private Random T; // Рандом
        private TypeCode _type;

        // Характеристики алгоритма
        private int _countPopulation; // Количество особей
        private int _countMaxIter; // Максимальное число итераций
        private int _tournirCount; // Турнирный отбор
        private double _chanceCross; // Шанс Скрещивания
        private double _chanceMutation; // Шанс мутации
        private double _chanceInvers; // Шанс инверсии

        // Популяционный всплеск
        private double _eps;
        private double _counteps;
        private double _maxcounteps;

        // Уплотнение
        private int _compactSize; // На сколько уплотняем сетку
        private int _compactIter; // Как часто
        private int _countCompact;

        // Для целочисленного кодирования
        private List<double> _codeArgs;

        // Для генов
        private double _min;
        private double _max;
        private int _count;

        private bool AlernativeExit;

        public int GetNumb()
        {
            if ((answ != null)&&(answ.log!=null)) return answ.log.Count;
            else return 0;
        }

        /// <summary>
        /// Создаёт класс, инициализирует данные по умолчанию
        /// </summary>
        public GenAlgWorck()
        {
            InitStart();
        }
        public GenAlgWorck(string func)
        {
            SetFunc(func);
            InitStart();
        }

        private void InitStart()
        {
            _Parse = new Parser();
            _population = new Population();
            T = new Random();

            SetAlgPar(50, 50, 2, 0.8, 0.2, 0.2, Math.Pow(10, -7), 5, 50, 4, 0); // Инициализируем параметры алгоритма
            SetGr(-5, -5); // Инициализируем границы и тип кодирования
        }

        /// <summary>
        /// Иницализирует данные алгоритма
        /// </summary>
        /// <param name="temp">Характеристики</param>
        public void SetAlgPar(List<double> temp)
        {
            SetAlgPar(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]), temp[3], temp[4], temp[5], temp[6], Convert.ToInt32(temp[7]), Convert.ToInt32(temp[8]), Convert.ToInt32(temp[9]), temp[10]);
        }
        public void SetAlgPar(int countPopulation, int maxiter, int tourn, double chcross, double chmut, double chinvers, double eps, int maxcount, int compactiter, int compactsize, double altex)
        {
            this._countPopulation = Convert.ToInt32(countPopulation);
            this._countMaxIter = Convert.ToInt32(maxiter);
            this._tournirCount = Convert.ToInt32(tourn);

            this._chanceCross = chcross;
            this._chanceMutation = chmut;
            this._chanceInvers = chinvers;

            this._eps = eps;
            this._maxcounteps = maxcount;
            this._counteps = 0;

            this._compactIter = compactiter;
            this._compactSize = compactsize;

            if (altex != 0) this.AlernativeExit = true;
            else AlernativeExit = false;
        }

        public void SetGr(double min, double max)
        {
            _type = TypeCode.RealCode;
            _min = min;
            _max = max;

            _codeArgs = new List<double>();
            _codeArgs.Add(_min);
            _codeArgs.Add(_max);
        }

        public void SetGr(List<double> args)
        {

            _count = Convert.ToInt32(args[2]);
            SetGr(args[0], args[1]);
            _type = TypeCode.IntCode;

            _codeArgs = new List<double>();
            foreach (double k in args)
            {
                _codeArgs.Add(k);
            }
        }

        public void SetFunc(string func)
        {
            _func = func;
            _countArg = Convert.ToInt32(_Parse.CheckParse(_func));
        }

        public void InitRandPopulaton()
        {
            _population.InitPopulation(_type, _codeArgs, _countPopulation, _func, _countArg); // Собираем популяцию
        }

        public AnswerClass StartWorck()
        {
            InitRandPopulaton(); // Собираем популяцию

            Population temp = _population;

            answ.AddLog(temp);

            _counteps = 0;
            _countCompact = 0;

            for (int i = 0; i < _countMaxIter; i++)
            {
                temp = _population;
                temp = Selection(temp);
                temp = Crossover(temp);
                temp = Inverse(temp);
                double a = temp._population.OrderBy(x => x.GetFitness()).First().GetFitness(); // Наименьшее значение
                double b = temp._population.OrderByDescending(x => x.GetFitness()).First().GetFitness(); // Наибольшее значение

                if (Math.Abs(a - b) <= _eps) _counteps += 1;
                _countCompact += 1;

                if (_counteps >= _maxcounteps)
                {
                    _counteps = 0;
                    temp = Spike(temp);
                }
                if (_countCompact >= _compactIter)
                {
                    temp = Compact(temp); // Уплотняем сетку
                    _countCompact = 0;
                }
                _population = temp;
                answ.AddLog(temp);

                if (AlernativeExit == true)
                {
                    Person k = temp._population.OrderBy(x => x.GetFitness()).First();
                    if ((_Parse.DiffFunc(_func, k.GetArgs(), 0) <= Math.Pow(10, -5)) && (_Parse.DiffFunc(_func, k.GetArgs(), 1) <= Math.Pow(10, -5)))
                    {
                        break;
                    }
                }
            }

            _population.CalcFitnessAll(_func);

            Person an = _population._population.OrderBy(x => x.GetFitness()).First();
            answ.SetAnswer(an.GetArgs(), an.GetFitness());
            return answ;
        }

        private Population Selection(Population Start)
        {
            Population answ = new Population();

            do
            {
                List<Person> index = new List<Person>();

                for (int i = 0; i < _tournirCount; i++)
                {
                    index.Add(Start.GetPerson(T.Next(0, _countPopulation))); // Выбираем несколько особей
                }

                answ.AddPerson(index.OrderBy(x => x.GetFitness()).First()); // Добавляем лучшую особь в популяцию

            } while (answ._CountPersons < _countPopulation);

            return answ;
        }

        private Population Crossover(Population Start)
        {
            if (_type == TypeCode.IntCode) return CrossoverInt(Start);
            else return CrossoverReal(Start);
        }
        private Population CrossoverInt(Population Start)
        {
            int numBit, numGen;

            int p1, p2;

            Population answer = new Population();

            // Пока не получили достаточное количество особей
            while (answer._CountPersons < _countPopulation)
            {
                numGen = T.Next(_countArg); // Аргумент, с которого начинаем менять
                numBit = T.Next(_count); // Бит, с которого меняем

                do // Выбираем две особи
                {
                    p1 = T.Next(_countPopulation);
                    p2 = T.Next(_countPopulation);
                } while (p1 == p2); // Особи не могут быть равны


                if (T.NextDouble() < _chanceCross) // Особи скрешиваются
                {
                    List<string> answ1 = new List<string>();
                    List<string> answ2 = new List<string>();

                    // До загаданного гена идём спокойно
                    for (int i = 0; i < numGen; i++)
                    {
                        answ1.Add(Start.GetPerson(p1).GetCodeArtgs()[i]);
                        answ2.Add(Start.GetPerson(p2).GetCodeArtgs()[i]);
                    }

                    string sa1 = "", sa2 = "";
                    for (int i = 0; i < _count; i++)
                    {
                        // До загаданного бита идём нормально
                        if (i < numBit)
                        {
                            sa1 += Start.GetPerson(p1).GetCodeArtgs()[numGen][i];
                            sa2 += Start.GetPerson(p2).GetCodeArtgs()[numGen][i];
                        }
                        else // После этого меняем
                        {
                            sa2 += Start.GetPerson(p1).GetCodeArtgs()[numGen][i];
                            sa1 += Start.GetPerson(p2).GetCodeArtgs()[numGen][i];
                        }
                    }

                    // Добавляем полученные строки к аргументам
                    answ1.Add(sa1); answ2.Add(sa2);

                    // После гена меняем все гены местами
                    for (int i = numGen + 1; i < _countArg; i++)
                    {
                        answ2.Add(Start.GetPerson(p1).GetCodeArtgs()[i]);
                        answ1.Add(Start.GetPerson(p2).GetCodeArtgs()[i]);
                    }

                    // Создаём новые особи
                    Person ch1 = new Person(); Person ch2 = new Person();

                    // Задаём закодированные параметры
                    ch1.SetCodeNumber(_codeArgs, answ1); ch2.SetCodeNumber(_codeArgs, answ2);

                    // Пересчитываем приспособленность
                    ch1.FitnessCalc(_func); ch2.FitnessCalc(_func);

                    // Мутация
                    ch1 = MutationInt(ch1); ch2 = MutationInt(ch2); // Мутация

                    answer.AddPerson(ch1); answer.AddPerson(ch2); // Добавляем новые особи
                }
                else // Особи не скрещиваюся
                {
                    answer.AddPerson(Start.GetPerson(p1));
                    answer.AddPerson(Start.GetPerson(p2));
                }
            }

            return answer;
        }
        private Population CrossoverReal(Population Start)
        {
            int numGen = T.Next(_countArg); // Выбрали точку разрыва
            int p1, p2;

            Population answer = new Population();

            while (answer._CountPersons < _countPopulation)
            {
                do
                {
                    p1 = T.Next(_countPopulation);
                    p2 = T.Next(_countPopulation);
                } while (p1 == p2); // Выбираем две особи

                List<double> s1 = new List<double>();
                List<double> s2 = new List<double>();

                if (T.NextDouble() < _chanceCross)
                {
                    for (int i = 0; i < _countArg; i++)
                    {
                        if (i < numGen)
                        {
                            s1.Add(Start.GetPerson(p1).GetArgs()[i]);
                            s2.Add(Start.GetPerson(p2).GetArgs()[i]);
                        }
                        else
                        {
                            s2.Add(Start.GetPerson(p1).GetArgs()[i]);
                            s1.Add(Start.GetPerson(p2).GetArgs()[i]);
                        }
                    }

                    Person ch1 = new Person(); Person ch2 = new Person();
                    ch1.SetNumber(s1); ch2.SetNumber(s2);
                    ch1.FitnessCalc(_func); ch2.FitnessCalc(_func);

                    ch1 = MutationReal(ch1); ch2 = MutationReal(ch2);

                    answer.AddPerson(ch1); answer.AddPerson(ch2);
                }
                else
                {
                    answer.AddPerson(Start.GetPerson(p1));
                    answer.AddPerson(Start.GetPerson(p2));
                }
            }

            return answer;
        }

        private Population Inverse(Population start)
        {
            if (_type == TypeCode.IntCode) return InverseInt(start);
            else return start;
        }
        private Population InverseInt(Population Start)
        {
            int numBit, numGen;

            for (int i = 0; i < _countPopulation; i++)
            {
                if (T.NextDouble() < _chanceInvers)
                {
                    numBit = T.Next(_count);
                    numGen = T.Next(_countArg);

                    Start.GetPerson(i).Inverse(numGen, numBit, _codeArgs); // Инвертировали
                    Start.GetPerson(i).FitnessCalc(_func); // Пересчитали значение
                }
            }
            return Start;
        }

        private Person Mutation(Person Start)
        {
            if (_type == TypeCode.IntCode) return MutationInt(Start);
            else return MutationReal(Start);
        }
        
        private Person MutationInt(Person Start)
        {
            // Проходим по всем аргументам
            for (int i = 0; i < Start.GetCodeArtgs().Count; i++)
            {
                // По всем битам
                for (int j = 0; j < Start.GetCodeArtgs()[i].Length; j++)
                {
                    if (T.NextDouble() < _chanceMutation) Start.Inverse(i, j, _codeArgs);
                }
            }

            Start.FitnessCalc(_func);
            return Start;
        }
        private Person MutationReal(Person Start)
        {
            int numGen;

            if (T.NextDouble() < _chanceInvers)
            {
                numGen = T.Next(_countArg);

                List<double> temp = Start.GetArgs();
                temp[numGen] = (2 * T.NextDouble() - 1);
                Start.SetNumber(temp);

                Start.FitnessCalc(_func);
            }

            return Start;
        }

        // Оператор всплеска
        private Population Spike(Population Start)
        {
            int count = T.Next(1, Start._CountPersons/2); // Выбираем сколько особей заменить

            List<int> index = new List<int>();
            int temp;

            do
            {
               /* do
                {*/
                    temp = T.Next(Start._CountPersons);
               // } while (index.Select(x => x == temp).Any() != true); // Генерим число, пока такое есть

                index.Add(temp);

            }while(index.Count<count);

            // Выбрали несколько особей

            for (int i = 0; i < index.Count; i++)
            {
                List<double> k = new List<double>();
                for (int j = 0; j < _countArg; j++)
                {
                    k.Add(T.Next((int)_min, (int)_max) * T.NextDouble());
                }

                if (_type == TypeCode.IntCode) Start.GetPerson(index[i]).SetNumber(_codeArgs, k);
                else Start.GetPerson(index[i]).SetNumber(k);
            }

            Start.CalcFitnessAll(_func);
            return Start;
        }

        private Population Compact(Population Start)
        {
            if (_type == TypeCode.IntCode)
            {
                Population newpop = new Population();

                _codeArgs[2] += _compactSize; // Улучшили точность
                SetGr(_codeArgs); // Пересохраним

                foreach (Person k in Start._population)
                {
                    List<double> temp = new List<double>();

                    // Сохранили цыфирки
                    foreach (double ch in k.GetArgs())
                    {
                        temp.Add(ch);
                    }

                    Person pers = new Person();
                    pers.SetNumber(_codeArgs, temp);
                    newpop.AddPerson(pers);
                }

                newpop.CalcFitnessAll(_func);
                return newpop;
            }
            else return Start;
        }
    }
}
