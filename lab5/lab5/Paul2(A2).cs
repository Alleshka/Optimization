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
    class Paul2_A2_
    {
        private string _func;
        private int count;
        private Vector _X0;
        private double _eps;

        public Paul2_A2_(string func, Vector X, double eps)
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
            P.Add(P[0]); // Теперь имеем {e1, e2...e1};


            double alpha;
            Lab3 labs;

            Vector curX = X1; Vector X2 = X1;


            while (true)
            {
                Console.WriteLine("X1: " + X1.printVector());
                curX = X1; X2 = X1;


                // Совершаем n поисков
                for (int i = 0; i < P.Count; i++)
                {

                    labs = new Lab3();

                    alpha = labs.Start(this._func, curX, P[i], this._eps); // Находим вектор

                    
                    curX = labs.Point(curX, alpha); // Перходим в новую точку

                    if (i == 0) X2 = curX; // Если точка X2
                }


                Vector dk = curX - X2; // Находим вектор
                curX = X2;

                if (k >= this.count - 1)
                {

                    labs = new Lab3();
                    alpha = labs.Start(this._func, curX, dk, this._eps);

                    curX = labs.Point(curX, alpha);

                    return curX;
                }
                else
                {
                    P.RemoveAt(0); // Первый сдвигается
                    P[0] = dk;
                    P.Add(dk);

                    X1 = curX;
                    k++;   
                }
            }           
        }
    }
}
