using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventOfCode2019.Day1
{
    class Day1Problem1
    {
        // https://adventofcode.com/2019/day/1
        public static void Main(string[] args)
        {
            List<String> moduleMasses = FileImporter.Import("Problem1Input");
            int totalMass = 0;
            foreach (string massStr in moduleMasses)
            {
                double mass = Int32.Parse(massStr);
                int fuelReq = Convert.ToInt32(Math.Floor(mass/3) - 2);
                totalMass += fuelReq;
            }
            Console.WriteLine(totalMass);
            Console.ReadKey(true);
        }
    }
}
