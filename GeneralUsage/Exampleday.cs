using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUsage
{
     abstract public class ExampleDay
    {
        public static void Main(string[] args)
        {
            ProblemLoop();
        }

        public static void ProblemLoop()
        {
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                Console.WriteLine("Type 1 for Problem 1, 2 for Problem 2, or q to quit.");
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.D1) { Problem1(); }
                else if (keyInfo.Key == ConsoleKey.D2) { Problem2(); }
                else if (keyInfo.Key == ConsoleKey.Q) { Environment.Exit(0); }
                else { Console.WriteLine("Unrecognized Key Command"); }
            }
        }

        public static void Problem1()
        {
            Console.WriteLine("Problem 1 not implemented yet.");
        }

        public static void Problem2()
        {
            Console.WriteLine("Problem 2 not implemented yet.");
        }
    }
}
