using System;
using MartixVectorAG;
using ParserAG;

namespace Lab3AG
{
    public class Lab3
    {

        private string _func; // Рабочая функция
        private string _countVar; // Количество переменных
        private double _eps;
        private int _maxCount; // Максимальное количество переменных

        private Vector _X0; // Стартовая точка
        private Vector _P; // Направление

        // Переход в новую точку
        public Vector Point(double alpha)
        {
            return _X0 + alpha * _P;
        }
        public Vector Point(Vector X, double alpha)
        {
            return X + alpha * _P;
        }
        public Vector Point(Vector X, Vector P, double alpha)
        {
            return X + alpha * P;
        }

        private double y(double alpha)
        {
            Vector temp = Point(alpha);

            Parser pars = new Parser();
            return Convert.ToDouble(pars.Parse(_func, temp.ch));
        }
        private double y(Vector X, double alpha)
        {
            Vector temp = Point(X, alpha);

            Parser pars = new Parser();
            return Convert.ToDouble(pars.Parse(_func, temp.ch));
        }

        private double dy(double alpha)
        {
            Vector temp = Point(alpha);
            Vector G = new Vector(temp.ch.Count);
            G.NullInit();

            Parser prs = new Parser();

            /* for (int i = 0; i < temp.ch.Count; i++)
             {
                 //Console.WriteLine("Проблемы начались на " + i + "ой итерации с " + temp.printVector());

                 G.ch[i] = prs.DiffFunc(_func, temp.ch, i);

                 //Console.WriteLine("G = " + G.printVector());
             }*/

            G = Grad(temp);

            //Console.WriteLine("Значения производных: " + G.printVector());

            return temp * _P;
        }

        private Vector Grad(Vector X)
        {
            double h = this._eps;
            double[] ch = new double[X.ch.Count];

            for (int i = 0; i < X.ch.Count; i++)
            {
                ch[i] = (y(X.ch[i] + h) - (y(X.ch[i] - h))) / (2 * h);
            }

            return new Vector(ch);
        }


        private Vector Sven1()
        {
            int k = 0;

            double _h = Math.Pow(10, -2);
            double alpha = Math.Pow(10, -2);

            if (y(alpha + _h) > y(alpha)) _h = -_h;

            do
            {
                _h *= 2;
                alpha += _h;
                k++;

                if (k >= this._maxCount) break;
            } while (y(alpha + _h) < y(alpha));

            double[] ch = new double[2];
            if (_h > 0)
            {
                ch[0] = alpha - _h;
                ch[1] = alpha + _h;
            }
            else
            {
                ch[0] = alpha + _h;
                ch[1] = alpha - _h;
            }

            return new Vector(ch);
        }
        private Vector Sven2()
        {
            double alpha = Math.Pow(10, -3);
            double h = Math.Pow(10, -3);
            int k = 0;
            double df;

            df = dy(alpha);

            if (df >= 0) h = (-1) * h;

            do
            {
                h *= 2;
                alpha += h;
                k++;
                if (k >= _maxCount) break;
            } while (dy(alpha - h) * dy(alpha) > 0);

            double[] ch = new double[2];

            if (h > 0)
            {
                ch[0] = alpha - h;
                ch[1] = alpha;
            }
            else
            {
                ch[1] = alpha - h;
                ch[0] = alpha;
            }

            return new Vector(ch);
        }
        private double balcan(Vector ch)
        {
            double a = ch.ch[0]; double b = ch.ch[1];

            double c;

            int k = 1;

            do
            {
                //Console.WriteLine("Бальцан, итерация " + k);
                //Console.WriteLine("Начальные границы [" + a + "; " + b + "]");
                c = (a + b) / 2;

                if (dy(c) > 0) b = c;
                else a = c;

                k++;
                //Console.WriteLine("Конечные границы [" + a + "; " + b + "]");
                if (k >= this._maxCount) break;

            } while (Math.Abs(b - a) >= this._eps);

            //Console.WriteLine("Вернули: конечные границы[" + a + "; " + b + "]");
            //Console.WriteLine((a + b) / 2);
            return (a + b) / 2;
        }

        private double zs2(Vector ch)
        {
            double a = ch.ch[0]; double b = ch.ch[1];

            double len = Math.Abs(a - b);

            double tau = (Math.Sqrt(5) - 1) / 2;
            double tau2 = (3 - Math.Sqrt(5)) / 2;
            int k;

            k = 1;
            double l = a + tau2 * len;
            double m = a + tau * len;
            // cout<<"l = "<<l<<"; m = "<<m<<endl;

            do
            {

                if (y(l) < y(m))
                {
                    //a = a;
                    b = m;
                    m = l;
                    len = Math.Abs(a - b);
                    l = a + tau2 * len;
                }
                else
                {
                    a = l;
                    l = m;
                    len = Math.Abs(a - b);
                    m = a + tau * len;
                }

                /*Console.WriteLine("L = " + l + " M = " + m);
                Console.WriteLine(y(tempX, l));
                Console.WriteLine(y(tempX, m));*/

                k++;

                if (k >= _maxCount) break;
            }
            while ((b - a) >= _eps);

            return (a + b) / 2;
        }


        public double Start(string func, Vector X0, Vector P, double eps)
        {
            this._func = func;         
            this._X0 = X0;
            this._P = P;
            this._eps = eps;

            this._maxCount = 100;

            Parser temp = new Parser();
            this._countVar = temp.CheckParse(_func);

            //Console.WriteLine("Начали Свен1");
            Vector ch = Sven1();
            //Console.WriteLine("Закончили свен 1. Границы: " + ch.printVector());

            //Console.WriteLine("Начали бальцана");
            double min = zs2(ch);
            //Console.WriteLine("Конец бальцана");

            return min;
        }
    }
}