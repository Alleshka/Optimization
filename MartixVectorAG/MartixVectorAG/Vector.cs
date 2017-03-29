using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MartixVectorAG;

namespace MartixVectorAG
{
    public class Vector
    {
        public List<double> ch { get; set; } // Сам набор цифирок
        public int size { get; set; }

        public void NullInit()
        {
            for (int i = 0; i < size; i++) ch[i] = 0;
        }
        public void RandInit()
        {
            Random T = new Random();

            for (int i = 0; i < size; i++) ch.Add(T.Next(0, 11));
        }

        public Vector(int x)
        {
            this.ch = new List<double>();
            size = x;

            RandInit();
        }

        public Vector(double[] vec)
        {
            this.ch = new List<double>();
            this.size = vec.Length;

            for (int i = 0; i < size; i++)
            {
                ch.Add(vec[i]);
            }
        }

        public double Norm()
        {
            double temp = 0;

            for (int i = 0; i < size; i++)
            {
                temp += Math.Pow(ch[i], 2);
            }

            return Math.Pow(temp, 0.5);
        }

        public void Normilize()
        {
            double d = Norm();

            for (int i = 0; i < size; i++)
            {
                this.ch[i] = this.ch[i] / d;
            }
        }

        // Сложение векторов
        public static Vector operator +(Vector a, Vector b)
        {
            if (a.ch.Count == b.ch.Count)
            {
                Vector c;
                double[] temp = new double[a.ch.Count];

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = a.ch[i] + b.ch[i];
                }

                c = new Vector(temp);
                return c;
            }
            else return null;
        }

        // Вычитание векторов
        public static Vector operator -(Vector a, Vector b)
        {
            if (a.ch.Count == b.ch.Count)
            {
                Vector c;
                double[] temp = new double[a.ch.Count];

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = a.ch[i] - b.ch[i];
                }

                c = new Vector(temp);
                return c;
            }
            else return null;
        }

        // Умножение двух векторов
        public static double operator *(Vector a, Vector b)
        {
            if (a.ch.Count == b.ch.Count)
            {
                double temp = 0;

                for (int i = 0; i < a.ch.Count; i++)
                {
                    temp += a.ch[i] * b.ch[i];
                }

                return temp;
            }
            else return double.MinValue;
        }

        // Умножение числа на вектор
        public static Vector operator *(double a, Vector b)
        {
            Vector c;
            double[] temp = new double[b.ch.Count];

            for (int i = 0; i < b.ch.Count; i++)
            {
                temp[i] = b.ch[i] * a;
            }

            c = new Vector(temp);

            return c;
        }

        public string printVector()
        {
            string temp = "( ";

            for (int i = 0; i < ch.Count; i++)
            {
                temp += Math.Round(ch[i], 5) + "; ";
            }
            temp += "); ";

            return temp;
        }

        public Matrix ConvertToMatrix()
        {
            return new Matrix(1, this.ch.Count, this.ch.ToArray());
        }
    }

    public class Matrix
    {
        public int a { get; private set; } // Количество строк
        public int b { get; private set; } // Количество столбцов

        public double[,] ch { get; private set; } // Сама табличка


        public void RandInit()
        {
            Random T = new Random();

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    this.ch[i, j] = T.Next(0, 11);
                }
            }
        }

        public Matrix(int x, int y)
        {
            this.a = x;
            this.b = y;

            ch = new double[x, y];

            RandInit();
        }
        public Matrix(int x, int y, double[] c)
        {
            this.a = x;
            this.b = y;

            this.ch = new double[x, b];

            for (int j = 0; j < b; j++) ch[0, j] = c[j];
        }

        public Matrix(int x, int y, double[,] c)
        {
            this.a = x;
            this.b = y;

            this.ch = new double[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    ch[i, j] = c[i, j];
                }
            }
        }

        public static Matrix operator +(Matrix A, Matrix B)
        {
            if ((A.a == B.a) && (A.b == B.b))
            {
                double[,] temp = new double[A.a, A.b];

                for (int i = 0; i < A.a; i++)
                {
                    for (int j = 0; j < A.b; j++)
                    {
                        temp[i, j] = A.ch[i, j] + B.ch[i, j];
                    }
                }

                return new Matrix(A.a, A.b, temp);
            }
            else return null;
        }
        public static Matrix operator -(Matrix A, Matrix B)
        {
            if ((A.a == B.a) && (A.b == B.b))
            {
                double[,] temp = new double[A.a, A.b];

                for (int i = 0; i < A.a; i++)
                {
                    for (int j = 0; j < A.b; j++)
                    {
                        temp[i, j] = A.ch[i, j] - B.ch[i, j];
                    }
                }

                return new Matrix(A.a, A.b, temp);
            }
            else return null;
        }

        public static Matrix operator *(double A, Matrix B)
        {

            for (int i = 0; i < B.a; i++)
            {
                for (int j = 0; j < B.b; j++)
                {
                    B.ch[i, j] *= A;
                }
            }

            return B;
        }
        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.b == B.a)
            {
                double[,] temp = new double[A.a, B.b];

                for (int i = 0; i < A.a; i++)
                {
                    for (int j = 0; j < B.b; j++)
                    {
                        for (int k = 0; k < A.b; k++)
                        {
                            temp[i, j] += A.ch[i, k] * B.ch[k, j];
                        }
                    }
                }

                return new Matrix(A.a, B.b, temp);
            }
            else return null;

        }

        public static Matrix operator *(Vector A, Matrix B)
        {
           return A.ConvertToMatrix() * B;
        }

        public string printMatrix()
        {
            string temp = "";

            for (int i = 0; i < this.a; i++)
            {
                for (int j = 0; j < this.b; j++)
                {
                    temp += this.ch[i, j] + " ";
                }
                temp += Environment.NewLine;
            }

            return temp;
        }
    }
}