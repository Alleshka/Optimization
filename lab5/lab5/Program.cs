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

            Huck h;

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

                h = new Huck(func, new Vector(x), Math.Pow(10, -8));
                Vector answ;

                answ = h.Start();

                Console.WriteLine("Min : " + answ.printVector());

            } while (true);

        }
    }
}
