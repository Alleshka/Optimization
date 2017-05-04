using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MartixVectorAG;
using Lab3AG;
using ParserAG;

namespace GenAlgV1
{
    public class Rossen
    {
        private string _func;
        private int _count;
        public int _countIter;
        public Vector MinVector;
        public List<Vector> Position;

        private double _eps;
        private Vector _X0;

        public string PrintAnswer()
        {
            string temp = "";

            temp += "********************************************" + Environment.NewLine;
            temp += "Розенброк" + Environment.NewLine;
            temp += "Минимум в точке: " + MinVector.printVector() + Environment.NewLine;
            temp += "Количество итераций: " + _count + Environment.NewLine;
            temp += "Количество точек: " + Position.Count + Environment.NewLine + Environment.NewLine;
            temp += "********************************************" + Environment.NewLine;
            temp += Environment.NewLine;

            return temp;
        }

        public Rossen(string func, Vector x)
        {
            Parser temp = new Parser();

            this._eps = Math.Pow(10, -8);

            this._func = func;
            this._count = Convert.ToInt32(temp.CheckParse(this._func));

            _X0 = new Vector(x.ch.Count);
            _X0.NullInit();
            _X0 += x;

            double[] ch = new double[_count];
        }

        public Vector Start()
        {
            Position = new List<Vector>();


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
                Position.Add(_X0);
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
                    Position.Add(tempX);
                    //Console.WriteLine("    Новый вектор" + i + tempX.printVector());
                }

                curX = tempX;
                /*Console.Write("Вектор X после цпс " + curX.printVector());

                Console.WriteLine();
                Console.Write("Alpha: " + alpha.printVector());*/

                //Console.ReadLine();

                if (alpha.Norm() <= this._eps) break;
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

                        P[i] = d[i];
                    }

                    _X0 = curX;

                    count++; if (count >= 35) break;
                }
            } while (true);

            MinVector = this._X0;
            _count = count;
            return this._X0;
        }
    }
}