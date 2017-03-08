using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;
using MartixVectorAG;
using Lab3AG;

namespace Pauell
{
    class Huck
    {
        private string _func;
        private int count;
        private Vector _X0;
        private double _eps;
        public List<Vector> Position;

        private double h = 0.1;
        private double b = 10;

        public Huck(string f, Vector x, double eps)
        {
            Parser temp = new Parser();
            this._func = f;
            this.count = Convert.ToInt32(temp.CheckParse(_func));

            _X0 = new Vector(x.ch.Count);
            _X0.NullInit();
            _X0 += x;

            this._eps = eps;

            this.Position = new List<Vector>();
        }

        public Vector Start()
        {
            Parser temp = new Parser();

            Vector X1 = _X0; // Начальная точка
            Vector X2;
            Vector X3;
            Vector X4;

            Position.Add(X1);

            do
            {
                X2 = IP(X1); // ИП1 в окрестности X1

                // Если ИП1 удачен
                if (Convert.ToDouble(temp.Parse(_func, X2.ch)) < Convert.ToDouble(temp.Parse(_func, X1.ch)))
                {
                    Position.Add(X2);

                    while (true)
                    {
                        X3 = 2 * X2 - X1;
                        X1 = X2;
                        Position.Add(X1);

                        X4 = IP(X3);

                        if (Convert.ToDouble(temp.Parse(_func, X4.ch)) < Convert.ToDouble(temp.Parse(_func, X2.ch)))
                        {
                            X2 = X4;
                            Position.Add(X2);
                        }
                        else
                        {
                            h /= b;
                            break;
                        }
                    }
                }
                else // ИП1 неудачен
                {
                    h /= b;
                    if (h < this._eps) break;
                }

            } while (true);

            return X1;
        }

        private Vector IP(Vector curX)
        {
            Parser temp = new Parser();

            Vector X = curX;

            double Fx = Convert.ToDouble(temp.Parse(_func, X.ch)); // Значение функции в лучшей точки

            Vector tempX = X;
            double tempFx = Fx;

            for (int i = 0; i < count; i++)
            {
                tempX = new Vector(this.count);
                tempX.NullInit();
                tempX += X;

                tempX.ch[i] += h; // Перешли в одной координате
                tempFx = Convert.ToDouble(temp.Parse(_func, tempX.ch)); // Посчитали значение

                // Если значение лучше
                if (tempFx < Fx)
                {
                    Fx = tempFx;
                    X = tempX;
                }
                else // Если хуже
                {
                    tempX.ch[i] -= 2 * h;
                    tempFx = Convert.ToDouble(temp.Parse(_func, tempX.ch));

                    if (tempFx < Fx)
                    {
                        Fx = tempFx;
                        X = tempX;
                    }
                    else
                    {
                        tempX.ch[i] += 2 * h; // Возвращаемся в предыдущую точку
                        tempFx = Fx;
                    }
                }
            }

            return X; // Возвращаем лучшую в области точку
        }
    }
}
