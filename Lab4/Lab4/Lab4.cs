using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;
using MartixVectorAG;
using Lab3AG;

namespace Lab4
{
    class Lab4
    {
        private string _func;
        private int _count;


        private double _eps;
        private Vector _X0;

        public Lab4(string func, Vector x)
        {
            Parser temp = new Parser();

            this._eps = Math.Pow(10, -8);

            this._func = func;
            this._count = Convert.ToInt32(temp.CheckParse(this._func));
            this._X0 = x;

            double[] ch = new double[_count];
        }

        public Vector Start()
        {
            Vector[] A = new Vector[this._count];
            Vector[] B = new Vector[this._count];
            Vector[] d = new Vector[this._count];


            // Задаём вектор направлений
            Vector[] P = new Vector[this._count];
            for (int i = 0; i < this._count; i++)
            {
                P[i] = new Vector(this._count);
                P[i].NullInit();
                P[i].ch[i] = 1;
            }

            int count = 0;
            Lab3 labs;
            do
            {
                Console.WriteLine("Итерация: " + count);
                Vector curX = _X0;
                /*Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("_________________________________");
                Console.WriteLine();
                Console.WriteLine();

               
                Console.WriteLine("Векторы P: ");
                for (int i = 0; i < this._count; i++) Console.Write("P" + i + P[i].printVector());*/

                Vector alpha = new Vector(this._count); alpha.NullInit();

                Vector tempX = curX;
               // Console.Write("Вектор X до цпс: " + curX.printVector());

                for (int i = 0; i < this._count; i++)
                {
                    //Console.Write("Func = " + this._func + " TempX = " + tempX.printVector() + "P = " + P[i].printVector());
                    labs = null;
                    labs = new Lab3();
                    alpha.ch[i] = labs.Start(this._func, tempX, P[i], _eps); // Запоминаем альфа
                    tempX = labs.Point(tempX, alpha.ch[i]); // Переходим в след точку???
                    //Console.WriteLine("    Новый вектор" + i + tempX.printVector());
                }

                curX = tempX;
                /*Console.Write("Вектор X после цпс " + curX.printVector());

                Console.WriteLine();
                Console.Write("Alpha: " + alpha.printVector());*/

                //Console.ReadLine();

                if (curX.Norm() <= this._eps) break;
                else
                {
                    for (int i = 0; i < this._count; i++)
                    {
                        if (Math.Abs(alpha.ch[i]) <= this._eps) A[i] = P[i];
                        else
                        {
                            Vector tempV = new Vector(this._count);
                            tempV.NullInit();

                            for (int k = i; k < this._count; k++)
                            {
                                tempV += alpha.ch[k] * P[k];
                            }
                            A[i] = tempV;
                        }

                        if (i == 0) B[i] = A[i];
                        else
                        {
                            Vector tempV = new Vector(this._count); tempV.NullInit();
                            for (int k = 0; k < i; k++)
                            {
                                tempV += (A[i] * d[k]) * d[k];
                            }
                            B[i] = A[i] - tempV;
                        }

                        d[i] = B[i];
                        d[i].Normilize();
                    }
                }

                for (int i = 0; i < this._count; i++)
                {
                    P[i] = d[i];
                }

                _X0 = curX;

                count++; if (count >= 15) break;

            } while (true);

            return this._X0;
        }
    }
}
