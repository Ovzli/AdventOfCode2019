using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day6
{
    class Day6Program
    {
        // https://adventofcode.com/2019/day/6
        [STAThread]
        static void Main(string[] args)
        {
            Problem2();
        }

        private static void Problem1()
        {
            //List<string> example = UsefulStuff.ImportTxtFileAsLines("Day6Example1");
            //List<Orbit> orbitList = MakeOrbitList(example);

            List<string> input = UsefulStuff.ImportTxtFileAsLines("Day6Input");
            List<Orbit> orbitList = MakeOrbitList(input);

            ILookup<string, Orbit> byOrbitalBody = orbitList.ToLookup(o => o.OrbitalBody);
            int checkSum = 0;
            foreach (var body in byOrbitalBody)
            {
                checkSum += FindNumberOfOrbits(byOrbitalBody, body.Key);
            }
            UsefulStuff.WriteSolution(checkSum.ToString());
            
        }

        private static void Problem2()
        {
            List<string> input = UsefulStuff.ImportTxtFileAsLines("Day6Input");
            List<Orbit> orbitList = MakeOrbitList(input);

            Dictionary<string, string> orbitDic = orbitList.ToDictionary(x => x.OrbitalBody, x => x.Barycenter);

            List<string> myHeirarchy = GetOrbitHeirarchy(orbitDic,"YOU");
            List<string> santaHeirarchy = GetOrbitHeirarchy(orbitDic,"SAN");

            IEnumerable<string> myPart = myHeirarchy.Except(santaHeirarchy);
            IEnumerable<string> santaPart = santaHeirarchy.Except(myHeirarchy);

            UsefulStuff.WriteSolution((myPart.Count() + santaPart.Count()).ToString());


        }

        private static List<Orbit> MakeOrbitList(List<string> input)
        {
            List<Orbit> orbitList = new List<Orbit>(); 
            foreach (string orbit in input)
            {
                string[] pair = orbit.Split(')');
                orbitList.Add(new Orbit(pair[0], pair[1]));
            }
            return orbitList;
        }

        private static int FindNumberOfOrbits(ILookup<string, Orbit> byOrbitalBody, string orbitalBody)
        {
            int checkSum = 1;
            if (orbitalBody=="COM") { return 0; }
            foreach(Orbit orbit in byOrbitalBody[orbitalBody])
            {
                checkSum += FindNumberOfOrbits(byOrbitalBody, orbit.Barycenter);
            }
            return checkSum;
        }

        private static List<string> GetOrbitHeirarchy(Dictionary<string, string> orbitDic, string orbitalBody)
        {
            if (orbitalBody == "COM") { return new List<string> { "COM" }; }
            string barycenter = orbitDic[orbitalBody];
            List<string> orbitHeirarchy = GetOrbitHeirarchy(orbitDic, barycenter);
            orbitHeirarchy.Add(barycenter);
            return orbitHeirarchy;
        }
        
    }

    class Orbit
    {
        public string Barycenter { get; private set; }//not really a barycenter but whatever
        public string OrbitalBody { get; private set; }

        public Orbit(string barycenter, string oribtalBody)
        {
            Barycenter = barycenter;
            OrbitalBody = oribtalBody;
        }
    }
}
