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

              for (int i = 0; i < temp.ch.Count; i++)
              {
                  G.ch[i] = prs.DiffFunc(_func, temp.ch, i);
              }

            
             //Console.WriteLine("Значения производных: " + G.printVector());

             return temp * _P;
         }


     /*   private Vector Grad(Vector X)
        {
            double h = this._eps;
            double[] ch = new double[X.ch.Count];

            for (int i = 0; i < X.ch.Count; i++)
            {
                ch[i] = (y(X.ch[i] + h) - (y(X.ch[i] - h))) / (2 * h);
            }

            return new Vector(ch);
        }*/

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
            double alpha = Math.Pow(10, -2);
            double h = Math.Pow(10, -2);
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

        private Vector zs2(Vector ch)
        {
            double a = ch.ch[0]; double b = ch.ch[1];

            double len = Math.Abs(a - b);

            double tau = (Math.Sqrt(5) - 1) / 2;
            double tau2 = (3 - Math.Sqrt(5)) / 2;
            int k;

            k = 1;
            double l = a + tau2 * len;
            double m = a + tau * len;

            do
            {
                if (y(l) < y(m))
                {
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

                k++;

                if (k >= _maxCount) break;
            }
            while ((b - a) >= _eps);

            // return (a + b) / 2;

            double[] temp = new double[2];
            temp[0] = a;
            temp[1] = b;
            
            return new Vector(temp);
        }


        private Vector zs2_2(Vector ch)
        {
            double a = ch.ch[0];
            double b = ch.ch[1];

            double l = Math.Abs(a - b);
            int k = 0;

            double tau = (Math.Sqrt(5) - 1) / 2;
            double tau2 = (3 - Math.Sqrt(5)) / 2;

            double x1 = a + tau * l;
            double x2 = a + b - x1;
            do
            {

                if ((y(x1) < y(x2)) && (x1 < x2))
                {
                    //a=a;
                    b = x2;
                    //x1=x1;
                }
                else
                {
                    if ((x1 < x2) && (y(x1) > y(x2)))
                    {
                        a = x1;
                        //b = b;
                        x1 = x2;

                    }
                    else
                    {
                        if ((x1 > x2) && (y(x1) > y(x2)))
                        {
                            // a = a;
                            b = x1;
                            x1 = x2;
                        }
                        else
                        {
                            // b = b;
                            a = x2;
                            x1 = x1;
                        }
                    }
                }
                x2 = a + b - x1;
                k = k + 1;
                if (k >= this._maxCount) break;
            } while (Math.Abs(b - a) <= this._eps);
            //cout << "k1 gs = " << k;

            double[] temp = new double[2];
            temp[0] = a;
            temp[1] = b;

            return new Vector(temp);

        }
        private double DCK(Vector ch)
        {
            double a = ch.ch[0];
            double b = ch.ch[1];

            double h = 0.01;

            int k = 1;
            double c, t;

            c = (a + b) / 2 + h;

            t = dd(a, b, c);

            do
            {
                t = dd(a, b, c);
                if (c < t)
                {
                    if (y(c) > y(t))
                    {
                        a = c;
                        c = t;
                    }
                    else { b = t; }
                }
                else
                {
                    if (y(c) > y(t))
                    {
                        b = c;
                        c = t;
                    }
                    else { a = t; }
                }
		
            } while ((Math.Abs((b - t) / b) >= _eps) && (Math.Abs((y(b) - y(t))) / y(b)) >= _eps);

            return (a + b) / 2;

            double[] temp = new double[2];
            temp[0] = a;
            temp[1] = b;

           // return new Vector(temp);
        }

        double dd(double a, double b, double c)
        {
            return b + 0.5 * ((b - a) * (y(a) - y(b))) / (y(a) - 2 * y(b) + y(c));
        }


        private double balcan(Vector ch)
        {
            double a = ch.ch[0];
            double b = ch.ch[1];

            double tempX = (a + b) / 2; int k = 1;

            double x1, x2;
            double l;

            while (true)
            {
                l = Math.Abs(b - a);


                x1 = a + l / 4;
                x2 = b - l / 4;

                if (y(x1) < y(tempX))
                {
                    b = tempX;
                    tempX = x1;
                }
                else
                {
                    if (y(tempX) > y(x2))
                    {
                        a = tempX;
                        tempX = x2;
                    }
                    else
                    {
                        a = x1;
                        b = x2;
                    }
                }

                k = k + 1;
                if (k >= _maxCount) break;

                if (l < this._eps) break;
            }

            return tempX;
        }
       
        public double Start(string func, Vector X0, Vector P, double eps)
        {
            this._func = func;         
            this._X0 = X0;
            this._P = P;
            this._eps = eps;
            double min;
            this._maxCount = 100;

            Parser temp = new Parser();
            this._countVar = temp.CheckParse(_func);

            Vector ch = Sven1();

            ch = zs2_2(ch);
            // min = DCK(ch);
            min = balcan(ch);

            return min;
        }
    }
}