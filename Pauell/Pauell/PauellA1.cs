using System;
using System.Collections.Generic;
using ParserAG;
using MartixVectorAG;
using Lab3AG;

namespace Pauell
{
    class PauellA1
    {
        private string _func;
        private int count;
        private Vector _X0;
        private double _eps;
        public List<Vector> Position;

        public PauellA1(string func, Vector X, double eps)
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
                curX = X1;



                Position.Add(curX);

                for (int i = 0; i < P.Count; i++) Console.Write("P" + i + P[i].printVector());
                for (int i = 0; i < P.Count; i++)
                {
                    labs = new Lab3();
                    alpha = labs.Start(this._func, curX, P[i], this._eps); // Находим вектор
                    curX = labs.Point(curX, alpha); // Перходим в новую точку    
                    Position.Add(curX);
                }


                Vector dk = curX - X1; // Находим вектор

                labs = new Lab3();
                alpha = labs.Start(this._func, curX, dk, this._eps); // Ищем щё один вектор
                curX = labs.Point(curX, alpha); // Переходим в точку X(n+2);
                Position.Add(curX);

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
