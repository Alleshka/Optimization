using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MartixVectorAG;
using ParserAG;
using Lab3AG;

namespace GenAlgV1
{
    class PauellA3
    {
        private string _func;
        private int count;
        private Vector _X0;
        private double _eps;
        public int _count;
        public Vector MinVector;
        public List<Vector> Position;

        public string PrintAnswer()
        {
            string temp = "";

            temp += "********************************************" + Environment.NewLine;
            temp += "П3" + Environment.NewLine;
            temp += "Минимум в точке: " + MinVector.printVector() + Environment.NewLine;
            temp += "Количество итераций: " + _count + Environment.NewLine;
            temp += "Количество точек: " + Position.Count + Environment.NewLine + Environment.NewLine;
            temp += "********************************************" + Environment.NewLine;
            temp += Environment.NewLine;

            return temp;
        }

        public PauellA3(string func, Vector X, double eps)
        {
            this._func = func;

            Parser temp = new Parser(); this.count = Convert.ToInt32(temp.CheckParse(_func));

            _X0 = new Vector(X.ch.Count);
            _X0.NullInit();
            _X0 += X;

            _eps = eps;

            Position = new List<Vector>();
        }

        public Vector Start()
        {
            Parser temp = new Parser();
            int k = 1;

            Vector X1 = _X0;

            List<Vector> P = new List<Vector>(); // Направления

            for (int i = 0; i < count; i++)
            {
                Vector tempV = new Vector(this.count);
                tempV.NullInit();
                tempV.ch[i] = 1;
                P.Add(tempV);
            }

            double alpha = 0;
            Lab3 labs;

            Vector curX = X1;
            Vector tempX = X1;

            double a, b;

            while (true)
            {
                List<double> A = new List<double>(); // Сохраняет альфы
                List<double> M = new List<double>(); // Сохраняет М
                List<double> Y = new List<double>(); // Значения функции в точке

                curX = X1;

                Y.Add(Convert.ToDouble(temp.Parse(this._func, curX.ch)));

                Position.Add(curX);

                for (int i = 0; i < P.Count; i++)
                {
                    tempX = curX;
                    labs = new Lab3();
                    alpha = labs.Start(this._func, curX, P[i], this._eps); // Находим вектор
                    curX = labs.Point(curX, alpha); // Перходим в новую точку      
                    Position.Add(curX);
                    Y.Add(Convert.ToDouble(temp.Parse(this._func, curX.ch)));

                    A.Add(alpha);

                    a = Convert.ToDouble(temp.Parse(this._func, tempX.ch));
                    b = Convert.ToDouble(temp.Parse(this._func, curX.ch));

                    M.Add(a - b);
                }

                tempX = curX;

                Vector dk = curX - X1; // Находим вектор
                dk.Normilize();

                labs = new Lab3();
                alpha = labs.Start(this._func, curX, dk, this._eps); // Ищем щё один вектор
                curX = labs.Point(curX, alpha); // Переходим в точку X(n+2);
                Position.Add(curX);
               

                if ((alpha <= _eps) && ((Math.Abs(Y.Last() - Y[0]) / Y.Last())) <= this._eps) break;
                else
                {
                    X1 = curX;
                    k++;

                    if (k >= 25) break;

                    int mmax = GetMaxIndex(M);

                    a = Convert.ToDouble(temp.Parse(this._func, tempX.ch));
                    b = Convert.ToDouble(temp.Parse(this._func, curX.ch));
                    M.Add(a - b);
                    A.Add(alpha);
                    Y.Add(Convert.ToDouble(temp.Parse(this._func, curX.ch)));


                    if (uslzv(M, Y, mmax) == true)
                    {
                        P.RemoveAt(mmax);
                    }
                    else P.RemoveAt(0);

                    P.Add(dk);
                }
            }

            this._count = k;
            this.MinVector = curX;
            return curX;
        }


        private int GetMaxIndex(List<double> M)
        {
            double temp = double.MinValue;
            int index = -1;

            for (int i = 0; i < M.Count; i++)
            {
                if (M[i] > temp)
                {
                    index = i;
                    temp = M[i];
                }
            }

            return index;
        }
        private bool uslzv(List<double> M, List<double> Y, int indexmax)
        {
            Parser temp = new Parser();

            double left = 4 * M[indexmax] * (M[M.Count-1]); // 4*mi*(y(n+1) - y (n+2)

            double right = Math.Pow((Y[0]-Y[Y.Count-2]-M[indexmax]), 2); // (y1-y(n+1)-mi)^2

            if (left >= right) return true;
            else return false;
        }
    }
}
