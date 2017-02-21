using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;
using MartixVectorAG;


namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser temp = new Parser();
            Lab4 lab;

            string func;
            double[] x;

            Vector minpoint;

            do
            {
                Console.WriteLine("Введите функцию");
                func = Console.ReadLine();

                x = new double[Convert.ToInt32(temp.CheckParse(func))];

                for (int i = 0; i < x.Length; i++)
                {
                    Console.WriteLine("Введите X" + i);
                    x[i] = Convert.ToInt32(Console.ReadLine());
                }

                lab = new Lab4(func, new Vector(x));

                minpoint = lab.Start();
                Console.WriteLine("Минимум в точке: " + minpoint.printVector() + " и равен " + temp.Parse(func, minpoint.ch.ToArray()));

            } while (true);
        }
    }
}
