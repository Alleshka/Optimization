using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;
using MartixVectorAG;
using Lab3AG;

namespace lab5
{
    class Paul1_A1_
    {
        private string _func;
        private int count;
        private Vector _X0;
        private double _eps;

        public Paul1_A1_(string func, Vector X, double eps)
        {
            this._func = func;

            Parser temp = new Parser(); this.count = Convert.ToInt32(temp.CheckParse(_func));

            _X0 = X;
            _eps = eps;
        }

        public Vector Start()
        {

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

            Vector curX = X1; Vector X2 = X1;

            while (true)
            {

                // Console.WriteLine("K = " + k);
                curX = X1;

                // Совершаем n поисков
                for (int i = 0; i < P.Count; i++)
                {
                 //   Console.WriteLine(i + "-й поиск");
                    labs = new Lab3();
                    alpha = labs.Start(this._func, curX, P[i], this._eps); // Находим вектор
                    curX = labs.Point(curX, alpha); // Перходим в новую точку           
                }

                Vector dk = curX - X1; // Находим вектор

                // curX = X1;

                labs = new Lab3();
                alpha = labs.Start(this._func, curX, dk, this._eps); // Ищем щё один вектор
                curX = labs.Point(curX, alpha); // Переходим в точку X(n+2);

                if (k >= count) break;
                else
                {
                    k++;
                    X1 = curX;

                    P.RemoveAt(0);
                    P.Add(dk);
                }
            }

            return curX;
        }
    }
}
