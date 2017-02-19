using System;
using MartixVectorAG;
using ParserAG;

namespace Lab3AG
{
    public class Lab3
    {
        private string _func; // Рабочая функция
        private Vector _X; // Стартовая точка
        private Vector _P;
        private int countVar; // Число переменных
        private double _eps;

        private int n = 80;

        public Lab3()
        {
        }

        private double y(Vector X, double alpha)
        {
            Parser temp = new Parser();

            //Console.WriteLine("Old Vector" + X.printVector());
            Vector t = Point(X, alpha); // Переходим в новую точку
            //Console.WriteLine("New Vector" + y.printVector());

            return Convert.ToDouble(temp.Parse(this._func, t.ch.ToArray()));
        }

        // Переход в новую точку
        public Vector Point(Vector X, double alpha)
        {
            return X + alpha * _P;
        }

        private Vector sven()
        {

            Vector tempX = _X;

            double xprev = 0;
            double x = 0.01;

            double h = Math.Pow(10, -3);

            if (y(tempX, x) >= y(tempX, xprev)) h *= (-1);

            _X = Point(tempX, x);

            int k = 0;

            do
            {
                xprev = x;
                x += h;

                h *= 2;

                if (y(tempX, x) > y(tempX, xprev)) break;

                tempX = Point(tempX, x);

                k++; if (k >= n) break;

            } while (true);

            double[] ch = new double[2];

            if (x > xprev)
            {
                ch[0] = xprev;
                ch[1] = x;

            }
            else
            {
                ch[0] = x;
                ch[1] = xprev;
            }

            return new Vector(ch);
        }

        private double zs2(Vector ch)
        {
            Vector tempX = _X;

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

                if (y(tempX, l) < y(tempX, m))
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

                if (k >= n)  break;         
            }
            while ((b - a) >= _eps);

            return (a + b) / 2;
        }

        public double Start(string func, Vector X, Vector P, double eps)
        {
            Parser temp = new Parser();

            this._func = func;
            this.countVar = Convert.ToInt32(temp.CheckParse(func));

            _X = X;
            _P = P;
            this._eps = eps;

            Vector ch = sven(); //Console.WriteLine("Свен отработал:" + ch.printVector());
            double min = zs2(ch); //Console.WriteLine("ЗС отработал" + min);

            return min;

        }
    }
}