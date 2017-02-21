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
    class Program
    {
        static void Main(string[] args)
        {
            string func;
            double[] x; Vector X;

            Parser temp = new Parser();

            Paul1_A1_ P1;
            Paul2_A2_ P;

            do
            {
                Console.WriteLine("Введите функцию");
                func = Console.ReadLine();
                if (func == "") break;

                int count = Convert.ToInt32(temp.CheckParse(func));
                x = new double[count];

                for (int i = 0; i < count; i++)
                {
                    Console.Write("Введите X" + i + ": ");
                    x[i] = Convert.ToDouble(Console.ReadLine());
                }

                P1 = new Paul1_A1_(func, new Vector(x), Math.Pow(10, -5));
                P = new Paul2_A2_(func, new Vector(x), Math.Pow(10, -5));

                Vector answ;


                answ = P1.Start();
                Console.WriteLine("Паул1: минимум в точке " + answ.printVector());

                /*answ = P.Start();
                Console.WriteLine("Паул2: инимум в точке " + answ.printVector());*/

            } while (true);

        }
    }
}
