using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;


namespace AdventOfCode2019.Day1
{
    class Day1Problem : ExampleDay
    {
       
        public new static void Main(string[] args)
        {
            //ProblemLoop();
            Problem2();
        }

        // https://adventofcode.com/2019/day/1
        private new static void Problem1()
        {
            List<String> moduleMasses = FileImporter.Import("Problem1Input");
            int totalMass = 0;
            foreach (string massStr in moduleMasses)
            {
                double mass = Int32.Parse(massStr);
                int fuelReq = Convert.ToInt32(Math.Floor(mass / 3) - 2);
                totalMass += fuelReq;
            }
            Console.WriteLine(totalMass);
            Console.ReadKey(true);
        }

        // https://adventofcode.com/2019/day/1#part2
        private new static void Problem2()
        {
            List<String> moduleMasses = FileImporter.Import("Problem1Input");
            int totalMass = 0;
            foreach (string massStr in moduleMasses)
            {
                double mass = Int32.Parse(massStr);
                totalMass += calculateNeededFuel(mass);
            }
            Console.WriteLine(totalMass);
            Console.ReadKey(true);
        }

        private static int calculateNeededFuel(double inputMass) //yay recursion
        {
            int neededFuel = 0;
            if (inputMass > 0)
            {
                int fuelReq = Convert.ToInt32(Math.Floor(inputMass / 3) - 2);
                neededFuel = fuelReq;
                int fuelForFuel = calculateNeededFuel(fuelReq);
                if (fuelForFuel >= 0) { neededFuel += fuelForFuel; }
            }           
            return neededFuel;
        }
    }
}
