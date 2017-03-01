using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserAG;


namespace TestParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser temp = new Parser();

            string func = "x1^2+x2^2+x3^2";
            double[] ch = new double[3];
            ch[0] = ch[1] = ch[2] = 2;

            Console.WriteLine(temp.Parse(func, ch.ToList()));
        }
    }
}
