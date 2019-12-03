using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day2
{
    class Day2Program
    {
		//https://adventofcode.com/2019/day/2
		[STAThread]
		public static void Main(string[] args)
        {
           Problem1();
        }  
		
		private static void Problem1()
		{
			//string exampleElfCode1 = "1,0,0,0,99";
			//string exampleElfCode2 = "2,3,0,3,99";
			//string exampleElfCode3 = "2,4,4,5,99,0";
			//string exampleElfCode4 = "1,1,1,4,99,5,6,0,99";
			string elfCode = UsefulStuff.ImportTxtFileAsLines("Problem2Input")[0];  //only want the first item of the list<string>
            List<int> program = ElfComputer.ParseInstructions(elfCode);

			program = ElfComputer.RunElfCode(12,2,program);
			string writeableProgram = string.Join(",",program);
			UsefulStuff.WriteSolution(program[0].ToString());
		}

		private static void Problem2()
		{
			string elfCode = UsefulStuff.ImportTxtFileAsLines("Problem2Input")[0];  //only want the first item of the list<string>

			int maxNoun = 99;
			int maxVerb = 99;
			int targetValue = 19690720;

			//Saddleback search
			int output = FindPair(maxNoun, maxVerb, targetValue, elfCode);
			if(output == -1)
			{
				UsefulStuff.WriteSolution("Not Found");
			}
			else
			{
				UsefulStuff.WriteSolution(output.ToString());
			}

			//original quick n' dirty search
			//for (int i = 0; i <= 99; i++)
			//	if (false) { 				
			//	{
			//		for(int j = 0; j<=99; j++)
			//		{
			//			program = ElfComputer.ParseInstructions(elfCode);
			//			program[1] = i;
			//			program[2] = j;

			//			//Console.WriteLine("Noun: " + i + " Verb: " + j);
			//			program = ElfComputer.RunElfCode(program);		
			//			if(i==0 || j == 0)
			//			{
			//				Console.WriteLine("Noun: " + i + " Verb: " + j);
			//				Console.WriteLine(program[0]);
			//			}
			//			if(program[0] == 19690720)
			//			{
			//				Console.WriteLine(100 * i + j);
			//				Console.ReadKey(true);
			//				System.Environment.Exit(1);
			//			}
			//		}
			//	}
			//}
		}

		private static int FindPair(int m, int n, int desiredValue, string elfCode)
		{
			int i = m;
			int j = 0;
			List<int> program;
			while (i >= 0 && j <= n )
			{
				program = ElfComputer.ParseInstructions(elfCode); //pull from code everytime because I didn't take the time to work out deep/shallow copy in C#

				Console.WriteLine("Noun: " + i + " Verb: " + j);
				program = ElfComputer.RunElfCode(i, j, program);
				
				if (program[0] == desiredValue)
				{
					return 100 * i + j;
				}
				else if(program[0] > desiredValue)
				{
					i--;
				}
				else //program[0] < desiredValue
				{
					j++;
				}
			}

			return -1;
		}
    }
}
