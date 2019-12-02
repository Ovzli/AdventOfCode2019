using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Day1
{
    class Day1Problem2
    {
        // https://adventofcode.com/2019/day/1#part2
        public static void Main(string[] args)
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
            if (inputMass > 0 )
            {
                int fuelReq = Convert.ToInt32(Math.Floor(inputMass / 3) - 2);
                neededFuel = fuelReq + calculateNeededFuel(fuelReq);
            }
            return neededFuel;
        }
    }
}
