using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;


namespace AdventOfCode2019.Day1
{
    class Day1Problem
    {
		[STAThread]
		public static void Main(string[] args)
        {
            Problem2();
        }

        // https://adventofcode.com/2019/day/1
        private static void Problem1()
        {
            List<String> moduleMasses = UsefulStuff.ImportTxtFileAsLines("Problem1Input");
            int totalMass = 0;
            foreach (string massStr in moduleMasses)
            {
                double mass = Int32.Parse(massStr);
                int fuelReq = Convert.ToInt32(Math.Floor(mass / 3) - 2);
                totalMass += fuelReq;
            }
			UsefulStuff.WriteSolution(totalMass.ToString());
        }

        // https://adventofcode.com/2019/day/1#part2
        private static void Problem2()
        {
            List<String> moduleMasses = UsefulStuff.ImportTxtFileAsLines("Problem1Input");
            int totalMass = 0;
            foreach (string massStr in moduleMasses)
            {
                double mass = Int32.Parse(massStr);
                totalMass += calculateNeededFuel(mass);
            }
			UsefulStuff.WriteSolution(totalMass.ToString());
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
